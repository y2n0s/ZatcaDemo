using Application.Models.Zatca;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IServices
{
    public interface IZatcaInvoiceSender
    {
        Task SendInvoiceToZatcaAsync(InvoiceToZatca invoice);
        Task SendInvoiceToZatcaAsync(InvoiceToZatca invoice, Seller supplier,
            CertificateDetails certificateDetails);
    }
}
