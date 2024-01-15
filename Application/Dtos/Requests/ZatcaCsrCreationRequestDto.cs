using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Requests
{
    public class ZatcaCsrCreationRequestDto
    {
        
        public bool IsProduction { get; set; }
        //public string ?SerialNumber { get; set; }
        //public string TaxPayerName { get; set; }
        public string BusinessCategory { get; set; }
        public string InvoiceType { get; set; }
        public string LocationAddress { get; set; }
        [RegularExpression("^3\\d{13}3$", ErrorMessage = " The value must be a 15 - digit  starting with 3 and ending with 3.")]
        [MaxLength(15)]
        public string VATNumber { get; set; }
        public string CountryName { get; set; } = "SA";
        //public string BranchName { get; set; }
        public string OrganizationName { get; set; }

        [EmailAddress(ErrorMessage ="Must be valid email")]
        public string Email { get; set; }
    }
}
