using Application.Contracts.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public InvoiceRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Invoice> AddInvoiceAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);  
            await _context.SaveChangesAsync();

            return invoice;
        }

        public async Task<string> GetPreviousHashAsync()
        {
            var previousInvoice = await _context.Invoices.OrderByDescending(x => x.SubmissionDate)
                .FirstOrDefaultAsync();

            return previousInvoice is not null ? previousInvoice.InvoiceHash : "";
        }
    }
}
