using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Requests
{
    public class ZatcaSupplierCreationRequestDto
    {
        [RegularExpression("^3\\d{13}3$", ErrorMessage = " The value must be a 15 - digit  starting with 3 and ending with 3.")]
        [MaxLength(15)]
        public string SellerTRN { get; set; }
        public string SellerName { get; set; }

        public string StreetName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        [RegularExpression("^\\d{4}$", ErrorMessage = " The value must be a 4-digit")]
        [MaxLength(4)]
        public string BuildingNumber { get; set; }

        public string IdentityType { get; set; } = "CRN";

        public string IdentityNumber { get; set; }

        public string CountryCode { get; set; } = "SA";
        [RegularExpression("^\\d{4}$", ErrorMessage = " The value must be 4-digit")]
        [MaxLength(4)]
        public string AdditionalStreetAddress { get; set; }
        [RegularExpression("^\\d{5}$", ErrorMessage = " The value must be a 5-digit")]
        [MaxLength(5)]
        public string PostalCode { get; set; }
    }
}
