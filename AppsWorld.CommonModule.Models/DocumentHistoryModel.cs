using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class DocumentHistoryModelVM
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public Guid? DocumentId { get; set; }
        public string DocState { get; set; }
        public decimal DocAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public Guid? TransactionId { get; set; }
        public string DocCurrency { get; set; }
        public decimal DocBalanaceAmount { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal BaseBalanaceAmount { get; set; }
        public string Remarks { get; set; }
        public string StateChangedBy { get; set; }
        public Nullable<System.DateTime> StateChangedDate { get; set; }
    }
}
