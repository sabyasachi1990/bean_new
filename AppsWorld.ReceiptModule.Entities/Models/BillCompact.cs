using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class BillCompact : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public System.DateTime PostingDate { get; set; }
       // public System.DateTime DocumentDate { get; set; }
       //public System.DateTime DueDate { get; set; }
        public string DocNo { get; set; }
        public long ServiceCompanyId { get; set; }
        public string DocumentState { get; set; }
        public System.Guid EntityId { get; set; }
       // public string EntityType { get; set; }
        public string Nature { get; set; }
        public decimal GrandTotal { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        //public string BaseCurrency { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string UserCreated { get; set; }
        public bool? IsExternal { get; set; }
        public string DocType { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public Guid? OpeningBalanceId { get; set; }
        public string DocDescription { get; set; }
        public decimal? BaseGrandTotal { get; set; }
        public decimal? BaseBalanceAmount { get; set; }
        public decimal? RoundingAmount { get; set; }
    }
}
