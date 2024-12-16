using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Models
{
    public class TailsModel
    {
        public string Name { get; set; }       //Name of file or folder
        public string FileSize { get; set; }  //size of file
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsSystem { get; set; }
        public bool IsFolder { get; set; }      //is folder or file
        public string Ext { get; set; }  //extension of file (folder does not get the extension)
        public string Description { get; set; }
        public string FileType { get; set; }   //Attached Files,Attachments,Tails
        public string Url { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
    }
}
