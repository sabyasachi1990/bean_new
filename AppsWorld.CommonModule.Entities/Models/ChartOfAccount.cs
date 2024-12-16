using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;


namespace AppsWorld.CommonModule.Entities
{
    public partial class ChartOfAccount : Entity
    {
        public ChartOfAccount()
        {
           
        }

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long AccountTypeId { get; set; }
        public string Class { get; set; }
        public string Category { get; set; }
        //public string SubCategory { get; set; }
        public string Nature { get; set; }
        public string Currency { get; set; }
        //public Nullable<bool> ShowRevaluation { get; set; }
        //public string CashflowType { get; set; }
        //public string AppliesTo { get; set; }
        public Nullable<bool> IsSystem { get; set; }
        //public Nullable<bool> IsShowforCOA { get; set; }
        public Nullable<int> RecOrder { get; set; }
        //public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        //public string ModifiedBy { get; set; }
        //public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public Nullable<short> Version { get; set; }
        public long?  SubsidaryCompanyId { get; set; }

        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
        //public Nullable<bool> IsSubLedger { get; set; }
        //public Nullable<bool> IsCodeEditable { get; set; }
        //public Nullable<bool> ShowCurrency { get; set; }
        //public Nullable<bool> ShowCashFlow { get; set; }
        //public Nullable<bool> ShowAllowable { get; set; }
        public Nullable<int> IsRevaluation { get; set; }
        //public Nullable<bool> Revaluation { get; set; }
        public Nullable<bool> DisAllowable { get; set; }
        //public Nullable<bool> RealisedExchangeGainOrLoss { get; set; }
        //public Nullable<bool> UnrealisedExchangeGainOrLoss { get; set; }
        public string ModuleType { get; set; }
        public bool? IsSeedData { get; set; }
        public bool? IsBank { get; set; }
        public bool? IsRealCOA { get; set; }
        public bool? IsLinkedAccount { get; set; }
        //public virtual ICollection<CashSale> CashSales { get; set; }
        //public virtual AccountType AccountType { get; set; }

        //public virtual Company Company { get; set; }

    }
}
