using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Zatca
{
    public interface IQrGenerator
    {
        string Generate(string sellerName, string vatRegistrationNumber, 
            string timeStamp, string invoiceTotal, string vatTotal, string hashedXml, 
            byte[] publicKey, string digitalSignature, bool isSimplified, 
            byte[] certificateSignature);
    }
}
