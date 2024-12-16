using AppsWorld.Framework;
using AppsWorld.ReceiptModule.Entities;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Models
{
    public class ReceiptModelK
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public double BankReceiptAmmount { get; set; }
        public Nullable<double> ReceiptApplicationAmmount { get; set; }
        public string ExchangeRate { get; set; }

        public string CashBankAccount { get; set; }
        public string ServiceCompanyName { get; set; }
        public string BankReceiptAmmountCurrency { get; set; }

        public string DocumentState { get; set; }
        public string EntityName { get; set; }
        public Guid? EntityId { get; set; }
        public string ModeOfReceipt { get; set; }

        public string ReceiptRefNo { get; set; }

        public string DocCurrency { get; set; }
        public DateTime? BankClearingDate { get; set; }
        public string ScreenName { get; set; }
        public bool? IsLocked { get; set; }


    }
}
