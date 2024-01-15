﻿using Application.Contracts.Zatca;
using Application.Models.Zatca;
using Domain.Enums;
using EInvoiceKSADemo.Helpers.Zatca.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca.Helpers
{
    public class ZatcaCSIDIssuer : IZatcaCSIDIssuer
    {
        private readonly IZatcaAPICaller _zatcaAPICaller;
        private readonly ICsrReader _csrReader;
        private readonly IXmlInvoiceGenerator _xmlGenerator;
        private readonly IInvoiceSigner _signer;
        private readonly ILogger<ZatcaCSIDIssuer> _logger;

        public ZatcaCSIDIssuer(IZatcaAPICaller zatcaAPICaller, ICsrReader csrReader
            , IXmlInvoiceGenerator xmlGenerator, IInvoiceSigner signer,
            ILogger<ZatcaCSIDIssuer> logger)
        {
            this._zatcaAPICaller = zatcaAPICaller;
            this._csrReader = csrReader;
            this._xmlGenerator = xmlGenerator;
            this._signer = signer;
            _logger = logger;
        }

        public Supplier Supplier { get; set; }
        public async Task<CSIDResultModel> OnboardingCSIDAsync(InputCSIDOnboardingModel model)
        {
            if (string.IsNullOrEmpty(model.CSR))
            {
                return null;
            }
            var companyCsr = model.CSR;
            this.Supplier = model.Supplier;
            if (!string.IsNullOrEmpty(companyCsr))
            {
                _logger.LogInformation("CompleteComplianceCSIDAsync is going to start");

                var complianceResult = await _zatcaAPICaller.CompleteComplianceCSIDAsync(new InputComplianceModel
                {
                    CSR = companyCsr
                }, model.OTP);

                _logger.LogInformation($"CompleteComplianceCSIDAsync has been " +
                    $"finished and BinarySecurityToken is {complianceResult?.BinarySecurityToken} " +
                    $"and errors are {complianceResult?.Errors}");

                if (!string.IsNullOrEmpty(complianceResult?.BinarySecurityToken))
                {
                    SharedData.UserName = complianceResult.BinarySecurityToken;
                    SharedData.Secret = complianceResult.Secret;

                    var csrResult = _csrReader.GetCsrInvoiceType(companyCsr);
                    if (csrResult != null)
                    {
                        if (csrResult.StandardAllowed)
                        {
                            var invoiceModel = GetInvoiceModel(InvoiceType.Standard, InvoiceTypeCode.Invoice, TransactionTypeCode.Standard);
                            var invoiceSigned = GetSignedXmlResult(invoiceModel);
                            var debitModel = GetInvoiceModel(InvoiceType.Standard, InvoiceTypeCode.Debit, TransactionTypeCode.Standard);
                            var debitSigned = GetSignedXmlResult(debitModel);
                            var creditModel = GetInvoiceModel(InvoiceType.Standard, InvoiceTypeCode.Credit, TransactionTypeCode.Standard);
                            var creditSigned = GetSignedXmlResult(creditModel);

                            foreach (var m in new[] { invoiceSigned, debitSigned, creditSigned })
                            {
                                if (m != null)
                                {
                                    var invoiceCompliance = await _zatcaAPICaller.PerformComplianceCheckAsync(new InputInvoiceModel
                                    {
                                        Invoice = m.InvoiceAsBase64,
                                        InvoiceHash = m.InvoiceHash,
                                        UUID = m.UUID
                                    });
                                    if (invoiceCompliance?.ClearanceStatus != "CLEARED")
                                    {
                                        return null;
                                    }
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }

                        if (csrResult.SimplifiedAllowed)
                        {
                            var invoiceModel = GetInvoiceModel(InvoiceType.Simplified, InvoiceTypeCode.Invoice, TransactionTypeCode.Simplified);
                            var invoiceSigned = GetSignedXmlResult(invoiceModel);
                            var debitModel = GetInvoiceModel(InvoiceType.Simplified, InvoiceTypeCode.Debit, TransactionTypeCode.Simplified);
                            var debitSigned = GetSignedXmlResult(debitModel);
                            var creditModel = GetInvoiceModel(InvoiceType.Simplified, InvoiceTypeCode.Credit, TransactionTypeCode.Simplified);
                            var creditSigned = GetSignedXmlResult(creditModel);

                            foreach (var m in new[] { invoiceSigned, debitSigned, creditSigned })
                            {
                                if (m != null)
                                {
                                    var invoiceCompliance = await _zatcaAPICaller.PerformComplianceCheckAsync(new InputInvoiceModel
                                    {
                                        Invoice = m.InvoiceAsBase64,
                                        InvoiceHash = m.InvoiceHash,
                                        UUID = m.UUID
                                    });
                                    if (invoiceCompliance?.ReportingStatus != "REPORTED")
                                    {
                                        return null;
                                    }
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                        

                        _logger.LogInformation("OnboardingCSIDAsync is going to start");

                        var certResult = await _zatcaAPICaller.OnboardingCSIDAsync(new InputCSIDModel
                        {
                            compliance_request_id = complianceResult.RequestId.ToString()
                        });

                        if (certResult?.RequestId > 0)
                        {
                            X509Certificate2 certificate = new X509Certificate2(Convert.FromBase64String(certResult.BinarySecurityToken));
                            if (certificate != null)
                            {
                                return new CSIDResultModel
                                {
                                    Secret = certResult.Secret,
                                    Certificate = certResult.BinarySecurityToken,
                                    StartedDate = certificate.NotBefore,
                                    ExpiredDate = certificate.NotAfter,
                                };
                            }
                        }
                    }
                }
            }
            return null;
        }

        private InvoiceDataModel GetInvoiceModel(InvoiceType invoiceType, InvoiceTypeCode invoiceTypeCode, string transactionTypeCode)
        {
            var model = new InvoiceDataModel
            {
                InvoiceNumber = GetNextInvoiceNumber(),
                InvoiceType = (int)invoiceType,
                InvoiceTypeCode = (int)invoiceTypeCode,
                Id = Guid.NewGuid().ToString(),
                Order = 1,
                TransactionTypeCode = transactionTypeCode,
                Lines = new List<LineItem>
                {
                    new LineItem {  Index = 1, ProductName = "T-Shirt" , Quantity = 1, NetPrice = 40 , Tax = 15.00},
                    new LineItem {  Index = 2, ProductName = "LCD Screen" , Quantity = 1, NetPrice = 5000 , Tax = 15.00},
                },
                //Tax = 15.00,
                //PaymentMeansCode = 10,
                PaymentMeans = new List<PaymentMean> { new PaymentMean { PaymentMeansCode = 10 } },
                Supplier = this.Supplier,
                Customer = new Customer
                {
                    CustomerName = "Saleh Saleh",
                    IdentityNumber = "311111111111113",
                    IdentityType = "NAT",
                    VatRegNumber = "323042342342333",
                    StreetName = "Makka",
                    BuildingNumber = "1111",
                    ZipCode = "12345",
                    CityName = "Al Riyadh",
                    DistrictName = "Al Olia",
                    RegionName = "Al Riyadh"
                },
                IssueDate = "2022-09-26",
                IssueTime = "17:00:00Z",
                PreviousInvoiceHash = GetPreviousInvoiceHash(),
                DeliveryDate = "2022-09-26"
            };

            //if (invoiceType == InvoiceType.Simplified)
            //{
            //    model.Customer = null;
            //}

            if (invoiceTypeCode == InvoiceTypeCode.Credit || invoiceTypeCode == InvoiceTypeCode.Debit)
            {
                model.Notes = "Cancellation or suspension of the supplies after its occurrence either wholly or partially";
                model.ReferenceId = "INV2010";
            }

            return model;
        }

        private string GetPreviousInvoiceHash()
        {
            return "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==";
        }

        private string GetNextInvoiceNumber()
        {
            //Sample Number
            return "INV201";
        }

        private ZatcaResult GetSignedXmlResult(InvoiceDataModel model)
        {
            // 01 - Generate XML 
            var xmlStream = ZatcaUtility.ReadInternalEmbededResourceStream(XslSettings.Embeded_InvoiceXmlFile);

            var invoiceXml = _xmlGenerator.GenerateInvoiceAsXml(xmlStream, model);

            // 02- Sign XML
            var signingResult = _signer.Sign(invoiceXml);

            // 03- Report to API
            if (signingResult.IsValid)
            {
                return signingResult;
            }

            return null;
        }

        public async Task<CSIDResultModel> RenewCSIDAsync(InputCSIDRenewingModel model)
        {
            if (string.IsNullOrEmpty(model.CSR))
            {
                return null;
            }
            if (!string.IsNullOrEmpty(model.CSR))
            {
                var renewalResult = await _zatcaAPICaller.RenewalCSIDAsync(new InputComplianceModel
                {
                    CSR = model.CSR
                }, model.OTP);


                X509Certificate2 certificate = new X509Certificate2(Convert.FromBase64String(renewalResult.BinarySecurityToken));
                if (certificate != null)
                {
                    return new CSIDResultModel
                    {
                        Secret = renewalResult.Secret,
                        Certificate = renewalResult.BinarySecurityToken,
                        StartedDate = certificate.NotBefore,
                        ExpiredDate = certificate.NotAfter,
                    };
                }
            }
            return null;
        }
    }
}
