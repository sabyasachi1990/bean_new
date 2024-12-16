using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class ChartOfAccountDTO
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long AccountTypeId { get; set; }
        public string Class { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Nature { get; set; }
        public string Currency { get; set; }
        public bool? ShowRevaluation { get; set; }
        public string CashflowType { get; set; }
        public string AppliesTo { get; set; }
        public bool? IsSystem { get; set; }
        public Nullable<bool> IsShowforCOA { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public bool? IsBank { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public bool? IsLinkedAccount { get; set; }
        public bool? IsPostedJournal { get; set; }
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

        public bool? IsSubLedger { get; set; }
        public bool? IsCodeEditable { get; set; }
        public bool? ShowCurrency { get; set; }
        public bool? ShowCashFlow { get; set; }
        public bool? ShowAllowable { get; set; }
        public bool? Revaluation { get; set; }
        public bool? DisAllowable { get; set; }
        public bool? RealisedExchangeGainOrLoss { get; set; }
        public bool? UnrealisedExchangeGainOrLoss { get; set; }
        public virtual LookUp LookUp { get; set; }
        public string AccountTypeName { get; set; }
        public bool? IsAllowableNotAllowableActivated { get; set; }
        public bool IsRevaluationActivated { get; set; }
        public bool? IsMultyCurrency { get; set; }
        public string ModuleType { get; set; }
        public string AccountTypeIndex { get; set; }
        public bool? IsSeedData { get; set; }
        public bool? IsCashAndBank { get; set; }
    }
}
