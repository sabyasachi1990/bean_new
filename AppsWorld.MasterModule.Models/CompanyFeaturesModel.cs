using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class CompanyFeaturesModel
    {
        public Guid? Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public Guid? FeatureId { get; set; }
        public bool? IsChecked { get; set; }
        public bool? IsReversible { get; set; }
        public string UserCreated { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool? IsTabShow { get; set; }
        public bool? IsTabActivated { get; set; }
    }
}
