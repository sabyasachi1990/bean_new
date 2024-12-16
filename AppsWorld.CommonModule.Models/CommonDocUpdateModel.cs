using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class CommonDocUpdateModel
    {
        public System.Guid Id { get; set; }
        public long? CompanyId { get; set; }
        public string DocType { get; set; }
        public string DocState { get; set; }
        public string DocDescription { get; set; }
        public string PONo { get; set; }
        public bool? IsNoSupportingDoc { get; set; }
        public bool? NoSupportingDocument { get; set; }
        public string Version { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Dictionary<Guid,string> AccountDescDetails { get; set; }
    }
    public class AccountDesc
    {
        public Guid? Id { get; set; }
        public string Dec { get; set; }
    }
}
