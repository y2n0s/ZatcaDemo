using Application.Contracts.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ZatcaDemoDB"));
            });
            services.AddDbContext<FodoDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("FodoDemoDB"));
            });

            services.AddScoped<ICertificateSettingsRepository, CertificateSettingsRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IZatcaSettingsRepository, ZatcaSettingsRepository>();
            services.AddScoped<IInvoiceToZatcaRepository, InvoiceToZatcaRepository>();

            return services;
        }
    }
}
