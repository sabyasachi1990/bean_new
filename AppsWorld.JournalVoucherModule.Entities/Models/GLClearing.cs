using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class GLClearing:Entity
    {
        public GLClearing()
        {
            this.GLClearingDetails = new List<GLClearingDetail>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public long ServiceCompanyId { get; set; }
        public long COAId { get; set; }
        public string DocDescription { get; set; }
        public bool IsMultiCurrency { get; set; }
        public string SystemRefNo { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public string  CrDr { get; set; }
        public string DocumentState { get; set; }
        public decimal? CheckAmount { get; set; }
        public long? COAId2 { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
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
        [ForeignKey("COAId")]
        public virtual ChartOfAccount ChartOfAccount { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public virtual ICollection<GLClearingDetail> GLClearingDetails { get; set; }
    }
}
