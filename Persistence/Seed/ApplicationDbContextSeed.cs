using Application.Models.Zatca;
using Domain.Entities;
using Persistence.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seed
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.InvoiceTypes.Any())
            {
                await context.InvoiceTypes.AddRangeAsync(new List<InvoiceType>
            {
                new InvoiceType{Key="1000", Description="Standard Only"},
                new InvoiceType{Key="0100", Description="Simplified Only"},
                new InvoiceType{Key="1100", Description="Standard and Simplified"},
            });
                await context.SaveChangesAsync();
            }

            if (!context.SellerIdentities.Any())
            {
                await context.SellerIdentities.AddRangeAsync(new List<SellerIdentity>
            {
                new SellerIdentity{Key="CRN", Description="Commercial Registration Number"},
                new SellerIdentity{Key="MOM", Description="Momra License"},
                new SellerIdentity{Key="MLS", Description="MLSD License"},
                new SellerIdentity{Key="700", Description="700 Number"},
                new SellerIdentity{Key="SAG", Description="Sagia License"},
                new SellerIdentity{Key="OTH", Description="Other ID"}
            });

                await context.SaveChangesAsync();
            }
        }
    }
}
