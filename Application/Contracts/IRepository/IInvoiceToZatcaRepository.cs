using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IInvoiceToZatcaRepository
    {
        Task<IReadOnlyList<InvoiceToZatca>> GetAllInvoicesToSendAsync();
        Task AddRangeInvoicesAsync(List<InvoiceToZatca> invoices);
        Task UpdateInvoiceAsync( InvoiceToZatca invoice);
        Task UpdateInvoicesAsync(List<InvoiceToZatca> invoices);
    }
}
