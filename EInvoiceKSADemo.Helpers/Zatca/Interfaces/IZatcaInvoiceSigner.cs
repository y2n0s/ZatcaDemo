﻿/*
 * Author  : Ahmed Moosa
 * Email   : ahmed_moosa83@hotmail.com
 * LinkedIn: https://www.linkedin.com/in/ahmoosa/
 * Date    : 26/9/2022
 */
using EInvoiceKSADemo.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceKSADemo.Helpers.Zatca
{
    public interface IZatcaInvoiceSigner
    {
        ZatcaInvoiceResult SignInvoice(string xmlContent, string certificateContent, string privateKeyContent);

        ZatcaInvoiceResult SignInvoice(string xmlContent);
    }
}
