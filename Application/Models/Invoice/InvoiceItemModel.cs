using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Invoice
{
    public class InvoiceItemModel
    {
        public InvoiceItemModel()
        {
            //  Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        //public int Index { get; set; }
        //public string ProductName { get; set; }
        public string Name { get; set; }
        //public double Quantity { get; set; }
        public string Qty { get; set; }
        //public double NetPrice { get; set; }
        public string Price { get; set; }//gross price
        public string NetValue { get; set; }
        //public double LineDiscount { get; set; }
        //public double PriceDiscount { get; set; }
        public string TotalDiscount { get; set; }
        /// <summary>
        /// The line VAT amount (KSA-11) must be Invoice line net amount (BT-131) x(Line VAT rate (BT152)/100)
        /// </summary>
        public string VAT { get; set; }
        //public double TaxAmount { get; set; }
        //public double TotalWithTax { get; set; }

        /// <summary>
        /// The invoice line net amount without VAT, and inclusive of line level allowance.
        /// Item line net amount (BT-131) = ((Item net price (BT-146) ÷ Item price base quantity(BT-149)) 
        ///     × (Invoiced Quantity (BT-129)) − Invoice line allowance amount(BT-136)
        /// </summary>
        //public double TotalWithoutTax { get; set; }

        //public double GrossPrice
        //{
        //    get
        //    {
        //        return NetValue + TotalDiscount;
        //    }
        //}


        public string VATPercentage { get; set; }
        //public double Tax { get; set; }
        //public string TaxCategory { get; set; } = "S";
        //public string TaxCategoryReasonCode { set; get; }
        //public string TaxCategoryReason { set; get; }
    }
}
