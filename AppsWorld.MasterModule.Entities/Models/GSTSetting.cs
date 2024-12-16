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
    public partial class GSTSetting:Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Number { get; set; }
        public Nullable<System.DateTime> DateOfRegistration { get; set; }
        public Nullable<System.DateTime> DeRegistration { get; set; }
        public Nullable<bool> IsDeregistered { get; set; }
        public string ReportingYearEnd { get; set; }
        public string ReportingInterval { get; set; }
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
        public long? ServiceCompanyId { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string GSTRepoCurrency { get; set; }
      //  public virtual Company Company { get; set; }
        [NotMapped]
        public bool IsCurrencyEditable { get; set; }
    }
}
