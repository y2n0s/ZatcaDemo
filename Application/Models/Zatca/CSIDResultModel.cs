using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Zatca
{
    public class InputCSIDOnboardingModel
    {
        [Required]
        public string CSR { get; set; }
        [Required]
        public string OTP { get; set; }
        [Required]
        public Supplier Supplier { get; set; }
    }

    public class InputCSIDRenewingModel
    {
        [Required]
        public string CSR { get; set; }
        [Required]
        public string OTP { get; set; }
    }
    public class CSIDResultModel
    {
        /// <summary>
        /// Working as User Name (in Authentication)
        /// </summary>
        public string Certificate { get; set; }
        public string Secret { get; set; }

        public DateTime ExpiredDate { get; set; }
        public DateTime StartedDate { get; set; }
    }
}
