using Application.Dtos.Requests;
using Application.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IServices
{
    public interface ICertificateCreationService
    {
        Task<SuccessResponse<Seller>> AddSupplierAsync(ZatcaSupplierCreationRequestDto request);
        Task<SuccessResponse<CertificateSettings>> CreateCertificateAsync(
            ZatcaCsrCreationRequestDto request);
        Task<SuccessResponse<IReadOnlyList<Domain.Entities.InvoiceType>>> GetAllInvoiceTypesAsync();
        Task<SuccessResponse<IReadOnlyList<SellerIdentity>>> GetAllSellerIdentitiesAsync();
    }
}
