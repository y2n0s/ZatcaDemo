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
    public class CertificateSettingsRepository : ICertificateSettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public CertificateSettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> SaveCsrAndPrivateKeyAsync(string csr, string privateKey)
        {
            var newCsr = new Domain.Entities.CertificateSettings
            {
                Csr = csr,
                PrivateKey = privateKey
            };

            await _context.CertificateSettings.AddAsync(newCsr);

            await _context.SaveChangesAsync();

            return newCsr.Id;
        }

        public async Task<CertificateSettings> GetCertificateByIdAsync(string id)
        {
            return await _context.CertificateSettings.SingleOrDefaultAsync(
                x => x.Id == new Guid(id));
        }

        public async Task<CertificateSettings> AddAsync(CertificateSettings certificate)
        {
            await _context.CertificateSettings.AddAsync(certificate);
            await _context.SaveChangesAsync();

            return certificate;
        }

        public CertificateSettings GetCertificateSettings()
        {
            return _context.CertificateSettings.FirstOrDefault();
        }
    }
}
