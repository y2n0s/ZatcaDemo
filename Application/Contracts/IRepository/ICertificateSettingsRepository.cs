using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface ICertificateSettingsRepository
    {
        Task<Guid> SaveCsrAndPrivateKeyAsync(string csr, string privateKey);
        Task<CertificateSettings> GetCertificateByIdAsync(string id);
        Task<CertificateSettings> AddAsync(CertificateSettings certificate);
        CertificateSettings GetCertificateSettings();
    }
}
