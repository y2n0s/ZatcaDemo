using Application.Contracts.IRepository;
using Application.Contracts.IServices;
using Application.Contracts.Zatca;
using Application.Models.Invoice;
using Application.Models.Zatca;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ZatcaInvoiceSender : IZatcaInvoiceSender
    {
        private readonly IZatcaReporter _reporter;
        private readonly ICertificateConfiguration _certificateConfiguration;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICertificateSettingsRepository _certificateSettingsRepository;
        private readonly IInvoiceToZatcaRepository _invoiceToZatcaRepository;
        private readonly ILogger<ZatcaInvoiceSender> _logger;

        public ZatcaInvoiceSender(IZatcaReporter reporter, 
            ICertificateConfiguration certificateConfiguration,
            IInvoiceRepository invoiceRepository,
            ISupplierRepository supplierRepository,
            ICertificateSettingsRepository certificateSettingsRepository,
            IInvoiceToZatcaRepository invoiceToZatcaRepository,
            ILogger<ZatcaInvoiceSender> logger)
        {
            _reporter = reporter;
            _certificateConfiguration = certificateConfiguration;
            _invoiceRepository = invoiceRepository;
            _supplierRepository = supplierRepository;
            _certificateSettingsRepository = certificateSettingsRepository;
            _invoiceToZatcaRepository = invoiceToZatcaRepository;
            _logger = logger;
        }

        public async Task SendInvoiceToZatcaAsync(InvoiceToZatca invoice)
        {
            var certificateDetails = _certificateConfiguration.GetCertificateDetails();
            if (certificateDetails != null)
            {
                SharedData.UserName = certificateDetails.UserName;
                SharedData.Secret = certificateDetails.Secret;
            }

            var invoiceItems = await GetInvoiceItemsAsnc(invoice.InvoiceItemsJson);

            var supplier = await _supplierRepository.GetSupplierAsync();
            
            var invoiceDataModel = new InvoiceDataModel
            {
                InvoiceNumber = invoice.InvoiceId.ToString(),
                //Id = invoice.InvoiceId.ToString(),//guid
                Id = invoice.Id.ToString(),//guid
                Order = (int)invoice.DetailId,
                //Tax = (double)invoice.TaxPercentage,
                //                Discount = 30.00,
                Lines= invoiceItems,
                //PaymentMeansCode = (int)invoice.PaymentMeans,
                Supplier = new Supplier
                {
                    SellerName = supplier.SellerName,
                    SellerTRN = supplier.SellerTRN,
                    AdditionalStreetAddress = supplier.AdditionalStreetAddress,
                    BuildingNumber = supplier.BuildingNumber,
                    CityName = supplier.CityName,
                    IdentityNumber = supplier.IdentityNumber,
                    IdentityType = supplier.IdentityType,
                    CountryCode = supplier.CountryCode,
                    DistrictName = supplier.DistrictName,
                    PostalCode = supplier.PostalCode,
                    StreetName = supplier.StreetName,
                },
                //Customer = null,
                IssueDate = invoice.InvoiceCreationDate?.ToString("yyyy-MM-dd"),// "2022-09-26",
                IssueTime = invoice.InvoiceCreationDate?.ToString("HH:mm:ssZ"), // "17:00:00Z",
                PreviousInvoiceHash = await GetPreviousInvoiceHash(),
                DeliveryDate = invoice.InvoiceDeliveryDate?.ToString("yyyy-MM-dd"),
            };
            
            if (!invoice.IsSimplifiedInvoice ?? false)
            {
                invoiceDataModel.TransactionTypeCode = TransactionTypeCode.Standard;
                var customer = new Customer
                {
                    CustomerName = invoice.CustomerName,
                    IdentityNumber = "311111111111113",
                    IdentityType = "NAT",
                    VatRegNumber = "323042342342333",
                    StreetName = "Makka",
                    BuildingNumber = "1111",
                    ZipCode = "12345",
                    CityName = invoice.CustomerAddressCity,
                    DistrictName = invoice.CustomerAddressDistrict,
                    RegionName = "Al Riyadh"
                };

                invoiceDataModel.Customer = customer;
            }
            else
            {
                invoiceDataModel.TransactionTypeCode = TransactionTypeCode.Simplified;
            }
            

            if (!invoice.IsRefundInvoice ?? false)
            {
                invoiceDataModel.InvoiceTypeCode = (int)InvoiceTypeCode.Invoice;
            }
            else
            {
                invoiceDataModel.InvoiceTypeCode = (int)InvoiceTypeCode.Credit;
                invoiceDataModel.Notes = invoice.RefundReason;
                invoiceDataModel.ReferenceId = "";// invoice.ReferenceId;
            }

            //invoiceDataModel = new InvoiceDataModel
            //{
            //    InvoiceNumber = GetNextInvoiceNumber(),
            //    InvoiceTypeCode = (int)InvoiceTypeCode.Invoice,
            //    Id = Guid.NewGuid().ToString(),
            //    Order = 2,
            //    TransactionTypeCode = TransactionTypeCode.Simplified,
            //    //Tax = 15,
            //    Lines = new List<LineItem>
            //        {
            //            new LineItem
            //            {
            //                Index =  1, ProductName = "Item", Quantity = 1, NetPrice = 100, Tax = 15, TaxCategory = "S",
            //                //Charges = new List<Charge>
            //                // {
            //                //     new Charge { Amount = 20 , ChargeReason = "Cleaning", ChargeReasonCode = "CG" , Tax = 15, TaxCategory  = "S"}
            //                // }
            //            },
            //           //new LineItem {  Index =  33, ProductName = "Item", Quantity = 21, NetPrice = 236042.1, Tax = 15.00 },
            //            //new LineItem {  PriceDiscount = 1 , LineDiscount = 5, Index = 4, ProductName = "Gold", Quantity = 0.3, NetPrice = 579.71, Tax = 15.00 },
            //            //new LineItem { Index = 1, ProductName = "T-Shirt", Quantity = 1, NetPrice = 345, Tax = 15.00 },
            //            //new LineItem { Index = 2, ProductName = "LCD Screen", Quantity = 2, NetPrice = 2499.99, Tax = 15.00 },
            //            //new LineItem { Index = 3, ProductName = "Glod 24", Quantity = 1, NetPrice = 230, Tax = 0.00,
            //            //    TaxCategory = "Z"  , TaxCategoryReason = "Export of goods", TaxCategoryReasonCode = "VATEX-SA-32" },
            //             //new LineItem { Index = 2, ProductName = "Product 1", Quantity = 1, NetPrice = 100.00d, Tax = 0.00,
            //             //   TaxCategory = "E"  , TaxCategoryReason = "Financial services", TaxCategoryReasonCode = "VATEX-SA-29" },
            //            //new LineItem { Index = 4, ProductName = "Product 2", Quantity = 1, NetPrice = 230, Tax = 0.00,
            //            //    TaxCategory = "E"  , TaxCategoryReason = "Real estate transactions", TaxCategoryReasonCode = "VATEX-SA-30" },
            //            //new LineItem {  Index = 244, Quantity = 1, ProductName = "Prepayment #1", PrepaidAmount = 86.96, TaxCategory = "S", Tax = 15.00 , PrepaymentId = GetPrepaymentNextInvoiceNumber(), PrepaymentIssueDate= DateTime.Now.ToString("yyyy-MM-dd") , PrepaymentIssueTime = DateTime.Now.ToString("HH:mm:ss") },
            //    },
            //    //                Discount = 30.00,
            //    PaymentMeansCode = 10,
            //    Supplier = new Supplier
            //    {
            //        SellerName = supplier.SellerName,
            //        SellerTRN = supplier.SellerTRN,
            //        AdditionalStreetAddress = supplier.AdditionalStreetAddress,
            //        BuildingNumber = supplier.BuildingNumber,
            //        CityName = supplier.CityName,
            //        IdentityNumber = supplier.IdentityNumber,
            //        IdentityType = supplier.IdentityType,
            //        CountryCode = supplier.CountryCode,
            //        DistrictName = supplier.DistrictName,
            //        PostalCode = supplier.PostalCode,
            //        StreetName = supplier.StreetName,
            //    },
            //    Customer = null,
            //    IssueDate = DateTime.Now.ToString("yyyy-MM-dd"),// "2022-09-26",
            //    IssueTime = DateTime.Now.ToString("HH:mm:ssZ"), // "17:00:00Z",
            //    PreviousInvoiceHash = await GetPreviousInvoiceHash(),
            //    //Notes = "Cancellation or suspension of the supplies after its occurrence either wholly or partially",
            //    //ReferenceId = "INV/2022/9/26/1",
            //    //DeliveryDate = DateTime.Now.ToString("yyyy-MM-dd"),
            //    //Charges = new List<Charge>
            //    //{
            //    //    new Charge { Amount = 20 , ChargeReason = "Cleaning", ChargeReasonCode = "CG" , Tax = 15, TaxCategory  = "S"},
            //    //    //new Charge { Amount = 40 , ChargeReason = "Cleaning", ChargeReasonCode = "CG" , Tax = 00, TaxCategory  = "Z"}
            //    //}
            //};
            var reportResult = await _reporter.ReportInvoiceAsync(invoiceDataModel);

            if (reportResult.Data != null)
            {
                await _invoiceRepository.AddInvoiceAsync(new Domain.Entities.Invoice
                {
                    InvoiceHash = reportResult.Data.InvoiceHash,
                    InvoiceType = invoiceDataModel.TransactionTypeCode == TransactionTypeCode.Simplified
                    ? (int)Domain.Enums.InvoiceType.Simplified : (int)Domain.Enums.InvoiceType.Standard,
                    InvoiceTypeCode = invoiceDataModel.InvoiceTypeCode,
                    QrCode = reportResult.Data.QrCode,
                    SignedXml = reportResult.Data.SignedXml,
                    ReportingStatus = reportResult.Data.ReportingStatus,
                    ReportingResult = reportResult.Data.ReportingResult,
                    ReportedToZatca = true,
                    SubmissionDate= reportResult.Data.SubmissionDate
                });

                _logger.LogWarning("invoice reported successfully");
            }
            else
            {
                _logger.LogWarning(Helper.ZatcaHttpClientMessage);
            }
        }

        public async Task SendInvoiceToZatcaAsync(InvoiceToZatca invoice, Seller supplier, 
            CertificateDetails certificateDetails)
        {
            if (certificateDetails != null)
            {
                SharedData.UserName = certificateDetails.UserName;
                SharedData.Secret = certificateDetails.Secret;
            }

            var invoiceItems = await GetInvoiceItemsAsnc(invoice.InvoiceItemsJson);

            var invoiceDataModel = new InvoiceDataModel
            {
                InvoiceNumber = invoice.InvoiceId.ToString(),
                //Id = invoice.InvoiceId.ToString(),//guid
                Id = invoice.Id.ToString(),//guid
                Order = (int)invoice.DetailId,
                //Tax = (double)invoice.TaxPercentage,
                //                Discount = 30.00,
                Lines = invoiceItems,
                PaymentMeans = new List<PaymentMean>
                {
                    new PaymentMean(){PaymentMeansCode=10 },
                    new PaymentMean(){PaymentMeansCode=30 }
                },
                Supplier = new Supplier
                {
                    SellerName = supplier.SellerName,
                    SellerTRN = supplier.SellerTRN,
                    AdditionalStreetAddress = supplier.AdditionalStreetAddress,
                    BuildingNumber = supplier.BuildingNumber,
                    CityName = supplier.CityName,
                    IdentityNumber = supplier.IdentityNumber,
                    IdentityType = supplier.IdentityType,
                    CountryCode = supplier.CountryCode,
                    DistrictName = supplier.DistrictName,
                    PostalCode = supplier.PostalCode,
                    StreetName = supplier.StreetName,
                },
                //Customer = null,
                IssueDate = invoice.InvoiceCreationDate?.ToString("yyyy-MM-dd"),// "2022-09-26",
                IssueTime = invoice.InvoiceCreationDate?.ToString("HH:mm:ssZ"), // "17:00:00Z",
                PreviousInvoiceHash = await GetPreviousInvoiceHash(),
                DeliveryDate = invoice.InvoiceDeliveryDate?.ToString("yyyy-MM-dd"),
            };

            if (!invoice.IsSimplifiedInvoice ?? false)
            {
                invoiceDataModel.TransactionTypeCode = TransactionTypeCode.Standard;
                var customer = new Customer
                {
                    CustomerName = invoice.CustomerName,
                    IdentityNumber = "311111111111113",
                    IdentityType = "NAT",
                    VatRegNumber = "323042342342333",
                    StreetName = "Makka",
                    BuildingNumber = "1111",
                    ZipCode = "12345",
                    CityName = invoice.CustomerAddressCity,
                    DistrictName = invoice.CustomerAddressDistrict,
                    RegionName = "Al Riyadh"
                };

                invoiceDataModel.Customer = customer;
            }
            else
            {
                invoiceDataModel.TransactionTypeCode = TransactionTypeCode.Simplified;
            }


            if (!invoice.IsRefundInvoice ?? false)
            {
                invoiceDataModel.InvoiceTypeCode = (int)InvoiceTypeCode.Invoice;
            }
            else
            {
                invoiceDataModel.InvoiceTypeCode = (int)InvoiceTypeCode.Credit;
                invoiceDataModel.Notes = invoice.RefundReason;
                invoiceDataModel.ReferenceId = "";// invoice.ReferenceId;
            }

            var reportResult = await _reporter.ReportInvoiceAsync(invoiceDataModel);

            if (reportResult.Data != null)
            {
                _logger.LogWarning("invoice reported successfully");
                await _invoiceRepository.AddInvoiceAsync(new Domain.Entities.Invoice
                {
                    InvoiceHash = reportResult.Data.InvoiceHash,
                    InvoiceType = invoiceDataModel.TransactionTypeCode == TransactionTypeCode.Simplified
                    ? (int)Domain.Enums.InvoiceType.Simplified : (int)Domain.Enums.InvoiceType.Standard,
                    InvoiceTypeCode = invoiceDataModel.InvoiceTypeCode,
                    QrCode = reportResult.Data.QrCode,
                    SignedXml = reportResult.Data.SignedXml,
                    ReportingStatus = reportResult.Data.ReportingStatus,
                    ReportingResult = reportResult.Data.ReportingResult,
                    ReportedToZatca = true,
                    SubmissionDate = reportResult.Data.SubmissionDate
                });

                invoice.IsSent = true;
                invoice.CountOfRetries += 1;   
                invoice.IsAccepted= string.Equals(reportResult.Data.ReportingStatus, "REPORTED") ? true : false;

                await _invoiceToZatcaRepository.UpdateInvoiceAsync(invoice);
            }
            else
            {
                invoice.IsSent = true;
                invoice.IsAccepted = false;
                invoice.CountOfRetries += 1;

                await _invoiceToZatcaRepository.UpdateInvoiceAsync(invoice);

                _logger.LogWarning(Helper.ZatcaHttpClientMessage);
            }
        }

        #region Helper Methods
        private async Task<List<LineItem>> GetInvoiceItemsAsnc(string invoiceItemsJson)
        {
            List<InvoiceItemModel> invoiceItems = DeSerializeInvoiceItems(invoiceItemsJson);

            var lineItems = CastInvoiceItemModelListToLineItemList(invoiceItems);

            return lineItems;
        }

        private async Task<string> GetPreviousInvoiceHash()
        {
            string previousInvoiceHash = await _invoiceRepository.GetPreviousHashAsync();

            if (string.IsNullOrEmpty(previousInvoiceHash))
                return "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==";

            return previousInvoiceHash;
        }

        private string GetNextInvoiceNumber()
        {
            return "INV/2022/9/26/2";
        }

        private List<InvoiceItemModel> DeSerializeInvoiceItems(string invoiceItemsJson)
        {
            var items = JsonSerializer.Deserialize<List<InvoiceItemModel>>(invoiceItemsJson);

            return items;
        }

        private List<LineItem> CastInvoiceItemModelListToLineItemList(
            List<InvoiceItemModel> invoiceItemModels)
        {
            //Index =  1, ProductName = "Item", Quantity = 1, NetPrice = 100, Tax = 15, TaxCategory = "S",

            List<LineItem> items = new List<LineItem>();
            foreach (var model in invoiceItemModels)
            {
                var lineItem = new LineItem
                {
                    Id = model.Id,
                    Index = model.Index,
                    ProductName = model.ProductName,
                    Quantity = model.Quantity,
                    NetPrice = model.NetPrice,
                    Tax = model.Tax
                };

                items.Add(lineItem);
            }

            return items;
        }
        #endregion
    }
}
