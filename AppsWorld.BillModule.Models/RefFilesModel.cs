using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class RefFilesModel
    {
        public string FileId { get; set; }
        public string RecordId { get; set; }
        //public string FilePath { get; set; }
        //public string ContentType { get; set; }
        public string ReferenceId { get; set; }
        public long CompanyId { get; set; }
        public string FileSize { get; set; }
        public string FileExt { get; set; }
        public bool IsLock { get; set; }
        public bool IsMyDrive { get; set; }
        public string Name { get; set; }
        public bool IsSystem { get; set; }
        public bool IsFolder { get; set; }
        public string TabName { get; set; }
        public string Source { get; set; }
        public string ModuleName { get; set; }
        public string RecordStatus { get; set; }
        public string FeatureId { get; set; }
        public string FileName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Description { get; set; }
    }
}
