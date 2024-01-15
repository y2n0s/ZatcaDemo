using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface IZatcaSettingsRepository
    {
        Task<IReadOnlyList<InvoiceType>> GetAllInvoiceTypes();
        Task<IReadOnlyList<SellerIdentity>> GetAllSellerIdentities();
    }
}
