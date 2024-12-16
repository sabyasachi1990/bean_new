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
    public partial class Communication : Entity
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }

        [StringLength(50)]
        public string CommunicationType { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Date { get; set; }

        [StringLength(256)]
        public string SentBy { get; set; }

        [StringLength(256)]
        public string FromMail { get; set; }

        [StringLength(256)]
        public string ToMail { get; set; }

        [StringLength(100)]
        public string Subject { get; set; }

        [StringLength(254)]
        public string UserCreated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(254)]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedDate { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }

        public Nullable<System.Guid> TemplateId { get; set; }
        RecordStatusEnum _status;

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get { return _status; }
            set { _status = (RecordStatusEnum)value; }
        }

        [StringLength(100)]
        public string TemplateName { get; set; }

        public string TemplateCode { get; set; }

        [StringLength(100)]
        public string ReportPath { get; set; }
        [StringLength(100)]
        public string Category { get; set; }



    }
}
