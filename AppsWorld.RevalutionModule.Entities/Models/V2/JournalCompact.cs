using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;using FrameWork;
using AppsWorld.Framework;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;


namespace AppsWorld.RevaluationModule.Entities.V2
{
    public partial class JournalCompact : Entity
    {
        public JournalCompact()
        {
            this.JournalDetails = new List<JournalDetailCompact>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string DocNo { get; set; }
        public long? ServiceCompanyId { get; set; }
        public DateTime? CreatedDate { get; set; }

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
        public virtual ICollection<JournalDetailCompact> JournalDetails { get; set; }
    }
}
