using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Zatca
{
    public class CsrInvoiceTypeRestriction
    {
        public bool StandardAllowed { get; set; }
        public bool SimplifiedAllowed { get; set; }
        public string VatRegNumber { get; set; }
    }
}
