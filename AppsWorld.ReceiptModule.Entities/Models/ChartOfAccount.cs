using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class ChartOfAccount : Entity
    {
        public ChartOfAccount()
        {
            this.DebitNoteDetails = new List<DebitNoteDetail>();
        }
        public long Id { get; set; }
        public long CompanyId { get; set; }
      
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public long AccountTypeId { get; set; }
        public long? SubsidaryCompanyId { get; set; }
       
        public Nullable<bool> IsSystem { get; set; }
       
        public Nullable<System.DateTime> CreatedDate { get; set; }

      
        [StringLength(50)]
        public string ModuleType { get; set; }

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

       
        public bool? DisAllowable { get; set; }
        public bool? IsRealCOA { get; set; }
       // public virtual AccountType AccountType { get; set; }

        public virtual ICollection<DebitNoteDetail> DebitNoteDetails { get; set; }
    }
}
     