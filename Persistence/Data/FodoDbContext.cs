using Application.Models.Fodo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public  class FodoDbContext : DbContext
    {
        public FodoDbContext(DbContextOptions<FodoDbContext> options):base(options) { }

        DbSet<InvoicesToZATCA> InvoicesToZATCA {  get; set; }
    }
}
