using Application.Dtos.Requests;
using Application.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface ISupplierRepository
    {
        Task AddSupplierAsync(Seller seller);
        Task<Seller> GetSupplierAsync();
        Task<bool> IsSupplierFoundAsync();
    }
}
