using Application.Contracts.IServices;
using Application.Models.Zatca;
using Application.Services;
using EInvoiceKSADemo.Helpers.Zatca;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;

namespace Presentation
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            
            SharedData.APIUrl = builder.Configuration["ZatcaSettings:Url"];

            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("ZatcaDemoDB"));
            //});

            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddZatcaServices(builder.Configuration);
            builder.Services.AddScoped<ICertificateCreationService, CertificateCreationService>();
            builder.Services.AddScoped<IZatcaInvoiceSender, ZatcaInvoiceSender>();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            return builder.Build();
        }
    }
}
