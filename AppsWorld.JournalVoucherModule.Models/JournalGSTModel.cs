﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JournalGSTModel
    {
        public decimal? GrandAmount { get; set; }
        public decimal? GrandTaxAmount { get; set; }
        public decimal? GrandTotal { get; set; }
    }
}