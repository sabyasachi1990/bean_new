using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class DocumentHistoryModel
    {
        public Guid TransactionId { get; set; }
        public long CompanyId { get; set; }
        public Guid DocumentId { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string DocState { get; set; }
        public string DocCurrency { get; set; }
        public decimal DocAmount { get; set; }
        public decimal DocBalanaceAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal BaseBalanaceAmount { get; set; }
        public string StateChangedBy { get; set; }
        public string Remarks { get; set; }
    }
}
