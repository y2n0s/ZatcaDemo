﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum InvoiceType
    {
        Standard = 1,
        Simplified = 2
    }

    public enum InvoiceTypeCode
    {
        Invoice = 388,
        Debit = 383,
        Credit = 381,
        Prepayment = 386
    }
}
