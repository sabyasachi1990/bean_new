﻿using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Models
{
    public class CreditNoteApplicationDetailModel
    {
        public System.Guid Id { get; set; }

        public System.Guid CreditNoteApplicationId { get; set; }
       
        public string DocType { get; set; }
        public System.Guid DocumentId { get; set; }
        public string DocCurrency { get; set; }
        public decimal CreditAmount { get; set; }
        public string DocNo { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public string SystemReferenceNumber { get; set; }
        public string Nature { get; set; }
        public decimal? DocAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public decimal BaseCurrencyExchangeRate { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
    }
}