using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public partial class AutoNumberCompact : Entity
    {
        //public AutoNumber()
        //{
        //    this.AutoNumberCompanies = new List<AutoNumberCompany>();
        //}

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string EntityType { get; set; }
        public string Format { get; set; }
        public string GeneratedNumber { get; set; }
        public Nullable<int> CounterLength { get; set; }
        public string Reset { get; set; }
        public bool? IsEditable { get; set; }
        public string Preview { get; set; }
        public bool? IsDisable { get; set; }

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
        //public string UserCreated { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //public string ModifiedBy { get; set; }
        //public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public string Variables { get; set; }
        ////public virtual Company Company { get; set; }
        
        //public virtual ICollection<AutoNumberCompany> AutoNumberCompanies { get; set; }
    }
}
