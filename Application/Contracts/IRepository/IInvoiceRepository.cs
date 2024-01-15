using Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IInvoiceRepository
    {
        Task<string> GetPreviousHashAsync();
        Task<Invoice> AddInvoiceAsync(Invoice invoice);
    }
}
