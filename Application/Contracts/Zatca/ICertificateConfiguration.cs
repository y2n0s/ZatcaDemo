using Application.Models.Zatca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Zatca
{
    public interface ICertificateConfiguration
    {
        CertificateDetails GetCertificateDetails();

        CertificateDetails GetCertificateDetails(string companyId);

        Task SaveCsrAndPK(InputCsrModel model);

        Task UpdateCertificate(CSIDResultModel model);
    }
}
