using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Entities
{
    public partial class DocRepository : Entity
    {
	   public System.Guid Id { get; set; }
	   public long CompanyId { get; set; }
	   public long TypeIntId { get; set; }
	   public System.Guid TypeId { get; set; }
	   public string Type { get; set; }
	   public string TypeKey { get; set; }
	   public string ModuleName { get; set; }
	   public string FilePath { get; set; }
	   public string DisplayFileName { get; set; }
	   public string Description { get; set; }
	   public Nullable<decimal> FileSize { get; set; }
	   public string NameofApprovalAuthority { get; set; }
	   public string FileExt { get; set; }
	   public Nullable<int> RecOrder { get; set; }
	   public string UserCreated { get; set; }
	   public Nullable<System.DateTime> CreatedDate { get; set; }
	   public string ModifiedBy { get; set; }
	   public Nullable<System.DateTime> ModifiedDate { get; set; }
	   public Nullable<short> Version { get; set; }


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
	   public string MongoFilesId { get; set; }
    }
}


