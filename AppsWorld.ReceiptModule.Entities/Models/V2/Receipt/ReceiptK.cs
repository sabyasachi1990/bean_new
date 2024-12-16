using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Entities.Models.V2.Receipt
{
    public partial class ReceiptK : Entity
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        //public string SystemRefNo { get; set; }
        public DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public decimal BankReceiptAmmount { get; set; }
        public Nullable<decimal> ReceiptApplicationAmmount { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public long ServiceCompanyId { get; set; }
        //public string CashBankAccount { get; set; }
        //public string ServiceCompanyName { get; set; }
        public long COAId { get; set; }
        public string BankReceiptAmmountCurrency { get; set; }
        public System.Guid EntityId { get; set; }
        public string DocumentState { get; set; }
        //public string EntityName { get; set; }
        public string ModeOfReceipt { get; set; }

        public string ReceiptRefNo { get; set; }

        public string DocCurrency { get; set; }
        public DateTime? BankClearingDate { get; set; }
        //public string ScreenName { get; set; }



      
    }
}
