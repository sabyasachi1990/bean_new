using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.CommonModule.Entities
{
    public partial class AccountType : Entity
    {
        public AccountType()
        {
            this.ChartOfAccounts = new List<ChartOfAccount>();
        }

        public long Id { get; set; }
        public Guid? FRATId { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Category { get; set; }
        //public string SubCategory { get; set; }
        //public string Nature { get; set; }
        //public string AppliesTo { get; set; }
        //public Nullable<bool> IsSystem { get; set; }
        //public Nullable<bool> ShowCurrency { get; set; }
        public Nullable<int> RecOrder { get; set; }
        //public string Remarks { get; set; }
        //public string UserCreated { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //public string ModifiedBy { get; set; }
        //public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public Nullable<short> Version { get; set; }

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
        //public Nullable<bool> ShowCashflowType { get; set; }
        //public Nullable<bool> ShowAllowable { get; set; }
        //public Nullable<bool> ShowRevaluation { get; set; }
        //public string Indexs { get; set; }
        //public string ModuleType { get; set; }
        //public virtual Company Company { get; set; }
        public virtual List<ChartOfAccount> ChartOfAccounts { get; set; }
    }
}
