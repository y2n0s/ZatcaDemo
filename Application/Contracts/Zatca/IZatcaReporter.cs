using Application.Models.Zatca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Zatca
{
    public interface IZatcaReporter
    {
        Task<ZatcaInvoiceReportResult> ReportInvoiceAsync<T>(T model) where T : class;
    }
}
