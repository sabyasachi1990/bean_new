using Repository.Pattern.Ef6;
using System;

namespace AppsWorld.BankTransferModule.Entities.Models
{
    public class Bill : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public System.DateTime PostingDate { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public string DocNo { get; set; }
        public long ServiceCompanyId { get; set; }
        public string DocumentState { get; set; }
        public System.Guid EntityId { get; set; }
        public string Nature { get; set; }
        public decimal GrandTotal { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string DocType { get; set; }
        public Guid? PayrollId { get; set; }
    }
}
