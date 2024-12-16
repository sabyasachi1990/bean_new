using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class DocRepositoryModel
    {
        // public Guid? CompanyDetailId { get; set; }
        public System.Guid Id { get; set; }
        [Required]
        public long CompanyId { get; set; }
        public long TypeIntId { get; set; }
        public string TabName { get; set; }
        public System.Guid TypeId { get; set; }
        [StringLength(50)]

        public string Type { get; set; }
        [StringLength(50)]
        public string TypeKey { get; set; }
        [StringLength(50)]
        public string ModuleName { get; set; }
        [StringLength(100)]
        public string NameofApprovalAuthority { get; set; }
        public string FilePath { get; set; }
        [StringLength(500)]
        public string DisplayFileName { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [System.ComponentModel.DataAnnotations.DataType("decimal(16 ,3")]
        public Nullable<decimal> FileSize { get; set; }
        public string FileExt { get; set; }
        public Nullable<int> RecOrder { get; set; }
        [StringLength(254)]
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
        public string RecordStatus { get; set; }

        public string ContentType { get; set; }

        public bool IsFavourite { get; set; }

        public string MongoFilesId { get; set; }
        public string FeatureId { get; set; }
    }
}
