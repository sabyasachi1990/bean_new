using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities
{
    public partial class AccountType : Entity
    {
        public AccountType()
        {
            //this.ChartOfAccounts = new List<ChartOfAccount>();
        }

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Nature { get; set; }
        public string AppliesTo { get; set; }
        public Nullable<bool> IsSystem { get; set; }
        public Nullable<bool> ShowCurrency { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public Nullable<bool> ShowAllowable { get; set; }
        public Nullable<bool> ShowRevaluation { get; set; }
        public Nullable<bool> ShowCashflowType { get; set; }
        public Guid? FRATId { get; set; }
        public string Indexs { get; set; }
        public string ModuleType { get; set; }
        public string CashflowType { get; set; }
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
        [NotMapped]
        public string RecStatus { get; set; }
        // public virtual List<ChartOfAccount> ChartOfAccounts { get; set; }
    
    }
}
