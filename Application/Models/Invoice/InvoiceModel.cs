using Application.Models.Zatca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Invoice
{
    public class InvoiceModel
    {
        //Inv/2023/2/131231
        public string InvoiceNumber { get; set; }
        /// <summary>
        /// GUID / UUID
        /// </summary>
        public string Id { get; set; }

        public int InvoiceType { get; set; }

        public int InvoiceTypeCode { get; set; }

        public string TransactionTypeCode { get; set; }

        public string Notes { get; set; }
        public int Order { get; set; }

        public string IssueDate { get; set; }

        public string IssueTime { get; set; }

        public List<InvoiceItemModel> Lines { get; set; }

        public double Discount { get; set; }

        public string ReferenceId { get; set; }

        public int PaymentMeansCode { get; set; } = 10;


        /// <summary>
        /// VAT category taxable amount (BT-116) = ∑(Invoice line net amounts (BT-113)) − Document level allowance amount (BT-93)
        /// VAT category tax amount(BT-117) = VAT category taxable amount(BT-116) × (VAT rate (BT-119) ÷ 100)
        /// </summary>
        public double TaxAmount { get; set; }

        public double TotalWithTax { get; set; }

        /// <summary>
        /// Invoice total amount without VAT (BT109) = Σ Invoice line net amount (BT-131) - Sum of allowances on document level (BT-107)
        /// Item line net amount (BT-131) = ((Item net price (BT-146) ÷ Item price base quantity(BT-149)) 
        ///     × (Invoiced Quantity (BT-129)) −Invoice line allowance amount(BT136)
        /// </summary>
        public double TotalWithoutTax { get; set; }

        public double TotalWithoutTaxAndDiscount { get; set; }


        public double PaymentAmount { get; set; }
        public int LinesCount
        {
            get { return Lines.Count; }
        }

        public string DeliveryDate { get; set; }

        public double Tax { get; set; } = 15;

        public List<TaxSubtotal> SubTotals { get; set; }

        public string DiscountTaxCategory { get; set; }
    }

    public class TaxSubtotal
    {
        public double TotalWithoutTax { set; get; }
        public double TaxAmount { set; get; }

        public string TaxCategory { get; set; }
        public double Tax { get; set; }
        public string TaxCategoryReasonCode { set; get; }
        public string TaxCategoryReason { set; get; }
    }
}

