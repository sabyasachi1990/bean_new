using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities
{
    public partial class CompanySetting:Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string CursorName { get; set; }
        public string ModuleName { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsEnabled { get; set; }
      //  public virtual Company Company { get; set; }
    }
}
