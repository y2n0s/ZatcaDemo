using Application.Contracts.Zatca;
using Application.Models.Zatca;
using EInvoiceKSADemo.Helpers.Zatca.Constants;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace EInvoiceKSADemo.Helpers.Zatca.Helpers
{
    public class ZatcaReporter : IZatcaReporter
    {
        private readonly IZatcaAPICaller _apiCaller;
        private readonly IXmlInvoiceGenerator _xmlGenerator;
        private readonly IInvoiceSigner _signer;

        public ZatcaReporter(IZatcaAPICaller apiCaller,
            IXmlInvoiceGenerator xmlGenerator, IInvoiceSigner signer)
        {
            this._apiCaller = apiCaller;
            this._xmlGenerator = xmlGenerator;
            this._signer = signer;
        }

        public async Task<ZatcaInvoiceReportResult> ReportInvoiceAsync<T>(T model) where T : class
        {
            try
            {
                // 01 - Generate XML 
                var xmlStream = ZatcaUtility.ReadInternalEmbededResourceStream(XslSettings.Embeded_InvoiceXmlFile);
                var invoiceXml = _xmlGenerator.GenerateInvoiceAsXml(xmlStream, model);

                // 02- Sign XML
                var signingResult = _signer.Sign(invoiceXml);

                // 03- Report to API
                if (signingResult.IsValid)
                {
                    int clearanceStatus = GetClearanceStatus(signingResult);
                    var apiResult = await _apiCaller.ReportSingleInvoiceAsync(new InputInvoiceModel
                    {
                        Invoice = signingResult.InvoiceAsBase64,
                        InvoiceHash = signingResult.InvoiceHash,
                        UUID = signingResult.UUID,
                    }, clearanceStatus);

                    if (apiResult != null)
                    {
                        //Save File 
                        SaveXmlFile(signingResult);

                        // 04- return results
                        return new ZatcaInvoiceReportResult
                        {
                            Success = signingResult.IsSimplified ? apiResult.ReportingStatus == "REPORTED" : apiResult.ClearanceStatus == "CLEARED",
                            Data = new ZatcaInvoiceModel
                            {
                                InvoiceHash = signingResult.InvoiceHash,
                                QrCode = signingResult.IsSimplified ? signingResult.QrCode : getQrCode(apiResult.ClearedInvoice),
                                SignedXml = signingResult.SingedXML,
                                ReportingStatus = signingResult.IsSimplified ? apiResult.ReportingStatus : apiResult.ClearanceStatus,
                                ReportingResult = JsonSerializer.Serialize(apiResult),
                                SubmissionDate = DateTime.Now,
                                IsReportedToZatca = signingResult.IsSimplified ? apiResult.ReportingStatus == "REPORTED" : apiResult.ClearanceStatus == "CLEARED",
                                InvoiceBase64 = signingResult.InvoiceAsBase64,
                                ErrorMessages = apiResult.ValidationResults.ErrorMessages,
                                WarningMessages = apiResult.ValidationResults.WarningMessages
                            }
                        };
                    }
                }
                return new ZatcaInvoiceReportResult() { Success = false, Message = signingResult.ErrorMessage };
            }
            catch (Exception ex)
            {
                return new ZatcaInvoiceReportResult() { Success = false, Message = ex.Message };
            }
        }

        private string getQrCode(string clearedInvoice)
        {
            if (!string.IsNullOrEmpty(clearedInvoice))
            {
                var signedXml = Encoding.UTF8.GetString(Convert.FromBase64String(clearedInvoice));
                if (!string.IsNullOrEmpty(signedXml))
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(signedXml);

                    var qrCode = ZatcaUtility.GetNodeInnerText(xmlDoc, XslSettings.QR_CODE_XPATH);
                    return qrCode;
                }
            }
            return null;
        }

        private static void SaveXmlFile(ZatcaResult signingResult)
        {
            var fileName = signingResult.IsSimplified ? "Simplified" : "Standard";
            if (!Directory.Exists(@"C:\Invoice Files3\"))
            {
                Directory.CreateDirectory(@"C:\Invoice Files3\");
            }
            var pathToSave = $@"C:\Invoice Files3\{fileName}.xml";
            File.WriteAllText(pathToSave, signingResult.SingedXML);
        }

        private int GetClearanceStatus(ZatcaResult signingResult)
        {
            return signingResult.IsSimplified ? 0 : 1;
        }
    }
}
