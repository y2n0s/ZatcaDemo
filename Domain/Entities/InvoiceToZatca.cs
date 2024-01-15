using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InvoiceToZatca

    {
        public Guid Id { get; set; }

        public long DetailId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyTaxNumber { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyAddressCity { get; set; }
        public string? CompanyAddressDistrict { get; set; }
        public string? Currency { get; set; }
        public long? InvoiceId { get; set; }
        public DateTime? InvoiceCreationDate { get; set; }
        public bool? IsSalesInvoice { get; set; }
        public bool? IsRefundInvoice { get; set; }
        public bool? IsTaxInvoice { get; set; }
        public bool? IsSimplifiedInvoice { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerAddressCity { get; set; }
        public string? CustomerAddressDistrict { get; set; }
        public DateTime? InvoiceDeliveryDate { get; set; }
        public string? RefundReason { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalForItems { get; set; }
        public decimal? NetWithoutVAT { get; set; }
        public string? InvoiceItemsJson { get; set; }
        public int? InsertFlag { get; set; }
        public int? UpdateFlag { get; set; }
        public int? DeleteFlag { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatorId { get; set; }
        public int? ModifierId { get; set; }
        public string PaymentMeans { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool IsSent { get; set; }
        public bool IsAccepted { get; set; }
        public int CountOfRetries { get; set; }

    }
}
