using Application.Models.Zatca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Zatca
{
    public interface IZatcaAPICaller
    {
        Task<ComplianceModelResult> CompleteComplianceCSIDAsync(InputComplianceModel model, string otp);

        Task<InvoiceModelResult> PerformComplianceCheckAsync(InputInvoiceModel model);

        Task<InvoiceModelResult> ReportSingleInvoiceAsync(InputInvoiceModel model, int clearanceStatus);

        Task<InvoiceModelResult> ClearSingleInvoiceAsync(InputInvoiceModel model, int clearanceStatus);

        Task<ComplianceModelResult> OnboardingCSIDAsync(InputCSIDModel model);

        Task<ComplianceModelResult> RenewalCSIDAsync(InputComplianceModel model, string otp);
    }
}
