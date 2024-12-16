using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Models
{
	public partial class ProvisionModel
	{
		public System.Guid Id { get; set; }
		public System.Guid InvoiceId { get; set; }
		public long CompanyId { get; set; }
		public string DocumentType { get; set; }
		public System.DateTime DocumentDate { get; set; }
		public string DocNo { get; set; }
		public string Remarks { get; set; }
		public bool IsNoSupportingDocument { get; set; }
		public Nullable<bool> NoSupportingDocument { get; set; }
		public string Currency { get; set; }
		public decimal Provisionamount { get; set; }
		public bool IsAllowableDisallowable { get; set; }
		public Nullable<bool> IsDisAllow { get; set; }
		public string SystemRefNo { get; set; }
		public string UserCreated { get; set; }
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

		public string RecordStatus { get; set; }
		//public virtual Invoice Invoice { get; set; }
	}
}
