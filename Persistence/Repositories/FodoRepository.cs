using Application.Contracts.IRepository;
using Domain.Entities.Fodo;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Fodo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class FodoRepository : IFodoRepository
    {
        private readonly FodoDbContext _fodoDbContext;

        public FodoRepository(FodoDbContext fodoDbContext)
        {
            _fodoDbContext = fodoDbContext;
        }

        public async Task<IReadOnlyList<InvoicesToZATCA>> GetFodoNotSentInvoicesAsync()
        {
            return await _fodoDbContext.InvoicesToZATCAs
                .Where(x=>x.IsSent == false)
                .ToListAsync();
        }

        public async Task UpdateFodoSentInvoicesAsync(IReadOnlyList<InvoicesToZATCA> invoices)
        {
            _fodoDbContext.InvoicesToZATCAs
                .UpdateRange(invoices);
            await _fodoDbContext.SaveChangesAsync();
        }
    }
}
