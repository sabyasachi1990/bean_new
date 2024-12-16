using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Models.Models
{
    public class AzureModel
    {
        public string Path { get; set; }
        public double? FileSize { get; set; }
        public string FileName { get; set; }
        public string DisplayFileName { get; set; }
        public string CommonType { get; set; }
    }
    public class UploadFileModel
    {
        public long CompanyId { get; set; }
        public byte[] FileBytes { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FileShare { get; set; }
        public string CursorName { get; set; }
        public string CreatedBy { get; set; }
        public string CommonType { get; set; }
    }
}
