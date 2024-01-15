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
    public class ZatcaSettingsRepository : IZatcaSettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public ZatcaSettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<InvoiceType>> GetAllInvoiceTypes()
        {
            return await _context.InvoiceTypes.ToListAsync();
        }

        public async Task<IReadOnlyList<SellerIdentity>> GetAllSellerIdentities()
        {
            return await _context.SellerIdentities.ToListAsync(); 
        }
    }
}
