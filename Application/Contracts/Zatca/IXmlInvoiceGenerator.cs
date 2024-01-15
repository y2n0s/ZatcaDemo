using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Zatca
{
    public interface IXmlInvoiceGenerator
    {
        string GenerateInvoiceAsXml<T>(string xmlFilePath, T model) where T : class;

        string GenerateInvoiceAsXml<T>(Stream xmlStream, T model) where T : class;
    }
}
