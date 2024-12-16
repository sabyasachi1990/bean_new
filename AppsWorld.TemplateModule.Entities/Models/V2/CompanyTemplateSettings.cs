using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Entities.Models.V2
{
    public partial class CompanyTemplateSettings : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public long ServiceCompanyId { get; set; }
        public string HeaderContent { get; set; }
        public string FooterContent { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public Nullable<int> Status { get; set; }
        public virtual Company Company { get; set; }
    }
}
