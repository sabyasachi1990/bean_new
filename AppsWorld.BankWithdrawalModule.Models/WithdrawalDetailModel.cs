using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.BankWithdrawalModule.Models
{
    public class WithdrawalDetailModel
    {
        public System.Guid Id { get; set; }
        public System.Guid WithdrawalId { get; set; }
        public long COAId { get; set; }
        public Nullable<bool> AllowDisAllow { get; set; }
        public Nullable<long> TaxId { get; set; }
        public string TaxType { get; set; }
        public string TaxCode { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public decimal DocAmount { get; set; }
        public Nullable<decimal> DocTaxAmount { get; set; }
        public decimal DocTotalAmount { get; set; }
        public Nullable<decimal> BaseAmount { get; set; }
        public Nullable<decimal> BaseTaxAmount { get; set; }
        public Nullable<decimal> BaseTotalAmount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public int? RecOrder { get; set; }
        public string RecordStatus;
        public string AccountName;
        public string TaxIdCode;
        public bool? IsPLAccount { get; set; }
        public string ClearingState { get; set; }
    }
}
