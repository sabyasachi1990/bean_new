using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities
{
   public class Feature:Entity
    {
        public System.Guid Id { get; set; }
        public Nullable<int> ModuleId { get; set; }
        public string Name { get; set; }
        public string VisibleStyle { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsTab { get; set; }
        public virtual ICollection<CompanyFeature> CompanyFeatures { get; set; }

    }
}
