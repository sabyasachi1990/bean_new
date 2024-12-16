using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Models
{
    public partial class CashSaleModelK
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        //public string CashSaleNumber { get; set; }
        public string DocNo { get; set; }
        public string DocCurrency { get; set; }
        //public string ExCurrency { get; set; }
        public double BaseAmount { get; set; }
        //public string SystemRefNo { get; set; }
        public string DocumentState { get; set; }
        public string EntityName { get; set; }
        public System.DateTime DocDate { get; set; }
        public string BaseCurrency { get; set; }

        public string ModeOfReceipt { get; set; }
        public string ReceiptrefNo { get; set; }

        //public string DocDescription { get; set; }
        public string ServiceCompanyName { get; set; }
        public long ServiceCompanyId { get; set; }
        public string ExchangeRate { get; set; }
        public string CashBankAccount { get; set; }
        public string PONo { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public double GrandTotal { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ScreenName { get; set; }
        public Guid? EntityId { get; set; }
        public bool? IsLocked { get; set; }
    }
}
