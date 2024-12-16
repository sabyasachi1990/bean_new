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

namespace AppsWorld.InvoiceModule.Models
{
    public class DocTilesFilesVM
    {
        public string TempFileId { get; set; }
        public string FileId { get; set; }
        public string RecordId { get; set; }
        public string ReferenceId { get; set; }
        public long CompanyId { get; set; }
        public string FileSize { get; set; }
        public string FileExt { get; set; }
        public bool IsMyDrive { get; set; }
        public string Name { get; set; }
        public bool IsSystem { get; set; }
        public string TabName { get; set; }
        public string Source { get; set; }
        public string ModuleName { get; set; }
        public string Description { get; set; }
        public string FileType { get; set; }
        RecordStatusEnum _status;
        //[Required]
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
        public string moduleName;
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string FeatureId { get; set; }
        public string RecordStatus { get; set; }
    }
}
