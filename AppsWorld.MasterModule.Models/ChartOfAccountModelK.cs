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
    public partial class ChartOfAccountModelK
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string AccountTypeName { get; set; }
        public bool? IsSystem { get; set; }
        public bool? DisAllowable { get; set; }
        public bool? ShowRevaluation { get; set; }
        public string Class { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Nature { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool? IsLinkedAccount { get; set; }
        public string ModuleType { get; set; }
        public string CashflowType { get; set; }
        public bool? Revaluation { get; set; }
        public string Status { get; set; }
        //RecordStatusEnum _status;
        //[Required]
        //[JsonConverter(typeof(StringEnumConverter))]
        //[StatusValue]
        //public RecordStatusEnum Status
        //{
        //    get
        //    {
        //        return _status;
        //    }
        //    set { _status = (RecordStatusEnum)value; }
        //}
        //public string CashflowType { get; set; }
    }
}
