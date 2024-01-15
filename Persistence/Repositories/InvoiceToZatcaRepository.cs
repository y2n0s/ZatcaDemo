using Application.Contracts.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InvoiceToZatcaRepository : IInvoiceToZatcaRepository
    {
        private readonly ApplicationDbContext _context;
        public InvoiceToZatcaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddRangeInvoicesAsync(List<InvoiceToZatca> invoices)
        {
            await _context.InvoiceToZatcas.AddRangeAsync(invoices);
            await _context.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<InvoiceToZatca>> GetAllInvoicesToSendAsync()
        {
            return await _context.InvoiceToZatcas
                .Where(i => (i.IsSent == false) || (i.IsSent == true && i.IsAccepted == false && i.CountOfRetries < 3))
                .ToListAsync();
        }

        public async Task UpdateInvoiceAsync(InvoiceToZatca invoice)
        {
            _context.InvoiceToZatcas.Update(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInvoicesAsync(List<InvoiceToZatca> invoices)
        {
            _context.InvoiceToZatcas.UpdateRange(invoices);
            await _context.SaveChangesAsync();
        }
    }
}
