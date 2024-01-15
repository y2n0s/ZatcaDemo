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
    public class SupplierRepository: ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddSupplierAsync(Seller seller)
        {
            await _context.Sellers.AddAsync(seller);
            await _context.SaveChangesAsync();  
        }

        public async Task<Seller> GetSupplierAsync()
        {
            var suppliers = await _context.Sellers.ToListAsync();
            return suppliers.FirstOrDefault();
        }

        public async Task<bool> IsSupplierFoundAsync()
        {
            return await _context.Sellers.AnyAsync();
        }
    }
}
