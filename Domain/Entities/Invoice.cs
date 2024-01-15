using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Invoice
    {
        public Guid Id { get; set; }

        public int InvoiceType { get; set; }
        public int InvoiceTypeCode { get; set; }
        public string InvoiceHash { get; set; }
        public bool ReportedToZatca { get; set; }
        public string ReportingResult { get; set; }
        public string ReportingStatus { get; set; }
        public string QrCode { get; set; }
        public string SignedXml { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
