using Domain.Entities.Fodo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IFodoRepository
    {
        Task<IReadOnlyList<InvoicesToZATCA>> GetFodoNotSentInvoicesAsync();
        Task UpdateFodoSentInvoicesAsync(IReadOnlyList<InvoicesToZATCA> invoices);
    }
}
