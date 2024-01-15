using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Seller
    {
        public Guid Id { get; set; }

        public string SellerTRN { get; set; }
        public string SellerName { get; set; }

        public string StreetName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }

        public string BuildingNumber { get; set; }

        public string IdentityType { get; set; } = "CRN";

        public string IdentityNumber { get; set; }

        public string CountryCode { get; set; } = "SA";

        public string AdditionalStreetAddress { get; set; }

        public string PostalCode { get; set; }
    }
}
