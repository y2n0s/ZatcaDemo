using Application.Contracts.IRepository;
using Application.Contracts.IServices;
using Application.Contracts.Zatca;
using Application.Models.Zatca;
using Domain.Entities;
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

        public InvoicesReportingBackgroundJob(
            IInvoiceToZatcaRepository invoiceToZatcaRepository,
            IZatcaInvoiceSender zatcaInvoiceSender,
            ILogger<InvoicesReportingBackgroundJob> logger,
            IInvoiceRepository invoiceRepository,
            ISupplierRepository supplierRepository,
            ICertificateConfiguration certificateConfiguration)
        {
            _invoiceToZatcaRepository = invoiceToZatcaRepository;
            _zatcaInvoiceSender = zatcaInvoiceSender;
            _logger = logger;
            _invoiceRepository = invoiceRepository;
            _supplierRepository = supplierRepository;
            _certificateConfiguration = certificateConfiguration;
        }

        public  async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("background service has been started to fetch invoice from api");
            //call api
            var fodoInvoices = new List<InvoiceToZatca>()
            {
                //Index =  1, ProductName = "Item", Quantity = 1, NetPrice = 100, Tax = 15, TaxCategory = "S"
                new InvoiceToZatca
                {
                    CreationDate = DateTime.Now,
                    DetailId=1234567,
                    InvoiceCreationDate= DateTime.Now,
                    InvoiceDeliveryDate= DateTime.Now,
                    //TaxAmount=300,
                    InvoiceId=987654321,
                    InvoiceItemsJson="[{\"Id\":\"wjhg\",\"Index\":1,\"ProductName\":\"item 1\",\"Quantity\":2,\"NetPrice\":100,\"LineDiscount\":0,\"PriceDiscount\":0,\"TaxAmount\":0,\"TotalWithTax\":0,\"TotalWithoutTax\":0,\"GrossPrice\":100,\"Tax\":15,\"TaxCategory\":\"S\",\"TaxCategoryReasonCode\":null,\"TaxCategoryReason\":null},{\"Id\":\"hjkh\",\"Index\":1,\"ProductName\":\"item 1\",\"Quantity\":2,\"NetPrice\":100,\"LineDiscount\":0,\"PriceDiscount\":0,\"TaxAmount\":0,\"TotalWithTax\":0,\"TotalWithoutTax\":0,\"GrossPrice\":100,\"Tax\":15,\"TaxCategory\":\"S\",\"TaxCategoryReasonCode\":null,\"TaxCategoryReason\":null},{\"Id\":\"kjhkj\",\"Index\":1,\"ProductName\":\"item 1\",\"Quantity\":2,\"NetPrice\":100,\"LineDiscount\":0,\"PriceDiscount\":0,\"TaxAmount\":0,\"TotalWithTax\":0,\"TotalWithoutTax\":0,\"GrossPrice\":100,\"Tax\":15,\"TaxCategory\":\"S\",\"TaxCategoryReasonCode\":null,\"TaxCategoryReason\":null}]",
                    IsRefundInvoice=false,
                    IsSalesInvoice=true,
                    IsSimplifiedInvoice=true,
                    //IsTaxInvoice=false,
                    TaxPercentage=15,
                    PaymentMeans=10
                }
            };

            _logger.LogInformation($"{fodoInvoices.Count()} invoices have been fetched from api");
            
            if (!fodoInvoices.Any())
                await Task.CompletedTask;

            await _invoiceToZatcaRepository.AddRangeInvoicesAsync(fodoInvoices);

            var invoices = await _invoiceToZatcaRepository.GetAllInvoicesToSendAsync();
            var supplier = await _supplierRepository.GetSupplierAsync();
            var certificateDetails = _certificateConfiguration.GetCertificateDetails();

            foreach (var invoice in invoices)
            {      
                await _zatcaInvoiceSender.SendInvoiceToZatcaV2Async(invoice, supplier, certificateDetails);
            }
        }
    }
}
