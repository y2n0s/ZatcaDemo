using Application.Contracts.IRepository;
using Application.Contracts.IServices;
using Application.Contracts.Zatca;
using Application.Models.Zatca;
using Domain.Entities;
using Domain.Entities.Fodo;
using Domain.Helpers;
using Microsoft.Extensions.Logging;
using Polly;
using Quartz;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Jobs
{
    [DisallowConcurrentExecution]
    public class InvoicesReportingBackgroundJob : IJob
    {
        private readonly IInvoiceToZatcaRepository _invoiceToZatcaRepository;
        private readonly IZatcaInvoiceSender _zatcaInvoiceSender;
        private readonly ILogger _logger;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICertificateConfiguration _certificateConfiguration;
        private readonly IFodoRepository _fodoRepository;

        public InvoicesReportingBackgroundJob(
            IInvoiceToZatcaRepository invoiceToZatcaRepository,
            IZatcaInvoiceSender zatcaInvoiceSender,
            ILogger<InvoicesReportingBackgroundJob> logger,
            IInvoiceRepository invoiceRepository,
            ISupplierRepository supplierRepository,
            ICertificateConfiguration certificateConfiguration,
            IFodoRepository fodoRepository)
        {
            _invoiceToZatcaRepository = invoiceToZatcaRepository;
            _zatcaInvoiceSender = zatcaInvoiceSender;
            _logger = logger;
            _invoiceRepository = invoiceRepository;
            _supplierRepository = supplierRepository;
            _certificateConfiguration = certificateConfiguration;
            _fodoRepository = fodoRepository;
        }

        public  async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("background service has been started to fetch invoice from api");

            #region Calling Fodo Db
           var fodoInvoices = await _fodoRepository.GetFodoNotSentInvoicesAsync();
            _logger.LogInformation($"{fodoInvoices.Count()} invoices have been fetched from api");

            if (!fodoInvoices.Any())
                return;
            #endregion

            #region Fodo Invoices To Zatca Invoices Conversion And Saving
            var invoicesToZatca = await MapFodoInvoicesToZatcaInvoicesAsync(fodoInvoices);
            
            await _invoiceToZatcaRepository.AddRangeInvoicesAsync(invoicesToZatca.ToList());
            #endregion

            #region Send Invoices To Zatca
            var invoices = await _invoiceToZatcaRepository.GetAllInvoicesToSendAsync();
            var supplier = await _supplierRepository.GetSupplierAsync();
            var certificateDetails = _certificateConfiguration.GetCertificateDetails();

            foreach (var invoice in invoices)
            {
                await _zatcaInvoiceSender.SendInvoiceToZatcaAsync(invoice, supplier, certificateDetails);

                var fodoInvoice = fodoInvoices.SingleOrDefault(x=>x.InvoiceId == invoice.InvoiceId);
                if(fodoInvoice is not null) fodoInvoice.IsSent = true;
            }
            #endregion

            #region Update Fodo Invoices
            await _fodoRepository.UpdateFodoSentInvoicesAsync(fodoInvoices);
            #endregion
        }

        private async Task<IReadOnlyList<InvoiceToZatca>> MapFodoInvoicesToZatcaInvoicesAsync(
            IReadOnlyList<InvoicesToZATCA> fodoInvoices)
        {
            var zatcaInvoices = new List<InvoiceToZatca>();

            foreach (var fodoInvoice in fodoInvoices)
            {
                var zatcaInvoice = new InvoiceToZatca
                {
                    CompanyAddress = fodoInvoice.CompanyAddress,
                    CompanyAddressCity = fodoInvoice.CompanyAddressCity,
                    CompanyAddressDistrict = fodoInvoice.CompanyAddressDistrict,
                    CompanyName = fodoInvoice.CompanyName,
                    CompanyTaxNumber = fodoInvoice.CompanyTaxNumber,
                    CreationDate = fodoInvoice.CreationDate,
                    CreatorId = fodoInvoice.CreatorId,
                    Currency = fodoInvoice.Currency,
                    CustomerAddress = fodoInvoice.CustomerAddress,
                    CustomerAddressCity = fodoInvoice.CustomerAddressCity,
                    CustomerAddressDistrict = fodoInvoice.CustomerAddressDistrict,
                    CustomerName = fodoInvoice.CustomerName,
                    CustomerId = fodoInvoice.CustomerId,
                    DeleteFlag = fodoInvoice.DeleteFlag,
                    DetailId = fodoInvoice.DetailId,
                    InsertFlag = fodoInvoice.InsertFlag,
                    InvoiceCreationDate = fodoInvoice.InvoiceCreationDate,
                    InvoiceId = fodoInvoice.InvoiceId,
                    InvoiceDeliveryDate = fodoInvoice.InvoiceDeliveryDate,
                    InvoiceItemsJson = fodoInvoice.InvoiceItemsJson,
                    IsDeleted = fodoInvoice.IsDeleted,
                    IsRefundInvoice = fodoInvoice.IsRefundInvoice,
                    IsSalesInvoice = fodoInvoice.IsSalesInvoice,
                    IsSimplifiedInvoice = fodoInvoice.IsSimplifiedInvoice,
                    IsTaxInvoice = fodoInvoice.IsTaxInvoice,
                    ModificationDate = fodoInvoice.ModificationDate,
                    ModifierId = fodoInvoice.ModifierId,
                    NetWithoutVAT = fodoInvoice.NetWithoutVAT,
                    PaymentMeans = fodoInvoice.InvoicePayments,
                    RefundReason = fodoInvoice.RefundReason,
                    TaxAmount = fodoInvoice.TaxAmount,
                    TaxPercentage = fodoInvoice.TaxPercentage,
                    TotalAmount = fodoInvoice.TotalAmount,
                    TotalDiscount = fodoInvoice.TotalDiscount,
                    TotalForItems = fodoInvoice.TotalForItems,
                    UpdateFlag = fodoInvoice.UpdateFlag,
                    IsSent = false
                };

                zatcaInvoices.Add(zatcaInvoice);
            }

            return zatcaInvoices;
        }
    }
}
