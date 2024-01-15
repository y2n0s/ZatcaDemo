using Application.Contracts.IRepository;
using Application.Contracts.IServices;
using Application.Contracts.Zatca;
using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Application.Exceptions;
using Application.Models.Invoice;
using Application.Models.Zatca;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CertificateCreationService : ICertificateCreationService
    {
        private readonly IZatcaCsrGenerator _zatcaCsrGenerator;
        private readonly ICertificateSettingsRepository _certificateSettingsRepository;
        private readonly IZatcaCSIDIssuer _zatcaCSIDIssuer;
        private readonly IConfiguration _config;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IZatcaSettingsRepository _zatcaSettingsRepository;
        private readonly ILogger<CertificateCreationService> _logger;

        public CertificateCreationService(IZatcaCsrGenerator zatcaCsrGenerator,
            ICertificateSettingsRepository certificateSettingsRepository,
            IZatcaCSIDIssuer zatcaCSIDIssuer,
            IConfiguration config,
            ISupplierRepository supplierRepository,
            IZatcaSettingsRepository zatcaSettingsRepository,
            ILogger<CertificateCreationService> logger)
        {
            _zatcaCsrGenerator = zatcaCsrGenerator;
            _certificateSettingsRepository = certificateSettingsRepository;
            _zatcaCSIDIssuer = zatcaCSIDIssuer;
            _config = config;
            _supplierRepository = supplierRepository;
            _zatcaSettingsRepository = zatcaSettingsRepository;
            _logger = logger;
        }

        public async Task<SuccessResponse<Seller>> AddSupplierAsync(ZatcaSupplierCreationRequestDto request)
        {
            if (await _supplierRepository.IsSupplierFoundAsync())
                throw new BadRequestException("you can add only one supplier");

            var supplier = new Seller
            {
                SellerTRN = request.SellerTRN,
                AdditionalStreetAddress = request.AdditionalStreetAddress,
                BuildingNumber = request.BuildingNumber,
                CityName = request.CityName,
                CountryCode = request.CountryCode,
                DistrictName = request.DistrictName,
                IdentityNumber = request.IdentityNumber,
                IdentityType = request.IdentityType,
                PostalCode = request.PostalCode,
                SellerName = request.SellerName,
                StreetName= request.StreetName
            };

            await _supplierRepository.AddSupplierAsync(supplier);

            return new SuccessResponse<Seller>
            {
                Data = supplier
            };
        }
        
        public async Task<SuccessResponse<CertificateSettings>> CreateCertificateAsync(
            ZatcaCsrCreationRequestDto request)
        {
            try
            {
                if (!await _supplierRepository.IsSupplierFoundAsync())
                    throw new BadRequestException("you must add only one supplier");

                var csrResponse = await CreateZatcaCsrAsync(request);

                if (!string.IsNullOrEmpty(csrResponse.Csr))
                {
                    var seller = await _supplierRepository.GetSupplierAsync();

                    var result = await _zatcaCSIDIssuer.OnboardingCSIDAsync(new InputCSIDOnboardingModel
                    {
                        CSR = Convert.ToBase64String(Encoding.UTF8.GetBytes(csrResponse.Csr)),
                        OTP = _config["ZatcaSettings:Otp"],
                        Supplier = new Supplier
                        {
                            SellerName = seller.SellerName,
                            SellerTRN = seller.SellerTRN,
                            AdditionalStreetAddress = seller.AdditionalStreetAddress,
                            BuildingNumber = seller.BuildingNumber,
                            CityName = seller.CityName,
                            IdentityNumber = seller.IdentityNumber,
                            IdentityType = seller.IdentityType,
                            CountryCode = "SA",
                            DistrictName = seller.DistrictName,
                            PostalCode = seller.PostalCode,
                            StreetName = seller.StreetName,
                        },
                    });

                    if (result != null)
                    {
                        //Save to Database  (Certificate & Secret & PrivateKey & CSR & StartedDate & ExpiredDate)

                        var certificate = new CertificateSettings
                        {
                            Certificate = result.Certificate,
                            Secret = result.Secret,
                            StartedDate = result.StartedDate,
                            ExpiredDate = result.ExpiredDate,
                            CertificateDate = DateTime.UtcNow,
                            Csr = csrResponse.Csr,
                            PrivateKey = csrResponse.PrivateKey,
                            //UserName = result.Certificate
                            UserName = SharedData.UserName
                        };
                        await _certificateSettingsRepository.AddAsync(certificate);

                        return new SuccessResponse<CertificateSettings>
                        {
                            Data = certificate
                        };
                    }
                    else
                    {
                        _logger.LogError($"Calling Message: {Helper.ZatcaHttpClientMessage}");
                    }
                }
            }
            catch (Exception)
            {
                throw new BadRequestException("CSID not created");
            }
            throw new BadRequestException("CSID not created");
        }

        public async Task<SuccessResponse<IReadOnlyList<Domain.Entities.InvoiceType>>> GetAllInvoiceTypesAsync()
        {
            var types = await _zatcaSettingsRepository.GetAllInvoiceTypes();
            return new SuccessResponse<IReadOnlyList<Domain.Entities.InvoiceType>>
            {
                Data = types
            };
        }

        public async Task<SuccessResponse<IReadOnlyList<SellerIdentity>>> GetAllSellerIdentitiesAsync()
        {
            var identities = await _zatcaSettingsRepository.GetAllSellerIdentities();
            return new SuccessResponse<IReadOnlyList<SellerIdentity>>
            {
                Data = identities
            };
        }
        #region Helper Methods
        private async Task<ZatcaCsrResponseDto> CreateZatcaCsrAsync(ZatcaCsrCreationRequestDto request)
        {
            try
            {

                if (!string.IsNullOrEmpty(request.VATNumber) && request.VATNumber.Length == 15)
                {
                    var csrInput = new Models.Zatca.InputZatcaCsr
                    {
                        OrganizationName = request.OrganizationName,
                        VATNumber = request.VATNumber,
                        BranchName = request.VATNumber.Substring(0, 10),//.Substring(0, 10),
                        BusinessCategory = request.BusinessCategory,
                        LocationAddress = request.LocationAddress,
                        CountryName = "SA",
                        InvoiceType = request.InvoiceType,
                        Email = request.Email,
                        SerialNumber = //request.SerialNumber ?? 
                        String.Format("1-{0}|2-V1|3-{1}", request.OrganizationName, Guid.NewGuid().ToString()),
                        //create list**IsProduction = csrTypecomboBox.SelectedItem.ToString() == "Production" ? true : false
                        IsProduction = request.IsProduction
                    };
                    var result = _zatcaCsrGenerator.GenerateCsr(csrInput);

                    if (result.Success)
                    {
                        // You can save csr and private key to Database
                        string privateKey = result.PrivateKey;//.Replace("\n", "").Replace("\r", "");
                        string csr = result.Csr;//.Replace("\n", "").Replace("\r", "");

                        return new ZatcaCsrResponseDto
                        {
                            Csr = csr,
                            PrivateKey = privateKey
                        };
                    }
                    else
                    {
                        throw new BadRequestException("problem in csr creation");
                    }
                }
                else
                {
                    throw new BadRequestException("invalid data");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                throw new BadRequestException("invalid data");
            }
        }
        #endregion
    }
}
