using Application.Contracts.Zatca;
using Application.Models.Zatca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca.Helpers
{
    public class ZatcaInvoiceSigner : IZatcaInvoiceSigner
    {
        private readonly IInvoiceSigner _signer;
        public ZatcaInvoiceSigner(IInvoiceSigner signer)
        {
            this._signer = signer;
        }
        public ZatcaInvoiceResult SignInvoice(string xmlContent)
        {
            var result = _signer.Sign(xmlContent);

            if (result.IsValid)
            {
                var invoiceHash = result.Steps.Find(s => s.IsValid && s.StepType == Application.Models.Zatca.StepType.InvoiceHash).ResultValue;
                var qrCodeContent = result.Steps.Find(s => s.IsValid && s.StepType == Application.Models.Zatca.StepType.QrCode).ResultValue;
                var invoiceAsBase64 = result.Steps.Find(s => s.IsValid && s.StepType == Application.Models.Zatca.StepType.InvoiceBase64).ResultValue;
                return new ZatcaInvoiceResult
                {
                    Success = true,
                    Data = new ZatcaInvoiceModel
                    {
                        SignedXml = result.SingedXML,
                        InvoiceHash = invoiceHash,
                        QrCode = qrCodeContent,
                        InvoiceBase64 = invoiceAsBase64,
                        PreviousInvoiceHash = result.PreviousInvoiceHash
                    }
                };
            }
            var stepErrorMessages = string.Join(",", result.Steps.Select(s => s.ErrorMessage));
            return new ZatcaInvoiceResult
            {
                Success = false,
                Message = string.Join(",", result.ErrorMessage, stepErrorMessages)
            };
        }

        public ZatcaInvoiceResult SignInvoice(string xmlContent, string certificateContent, string privateKeyContent)
        {
            throw new NotImplementedException();
        }
    }
}
