using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CertificateSettings
    {
        public Guid Id { get; set; }
        public string Csr { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
        public string? Secret { get; set; }
        public string? Certificate { get; set; }
        public DateTime? CertificateDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public DateTime? StartedDate { get; set; }
        public string? UserName { get; set; }
    }
}
