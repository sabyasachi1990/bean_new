using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Entities
{
    public partial class TemplateTypeCompact : Entity
    {
        public TemplateTypeCompact()
        {
            this.GenericTemplates = new List<GenericTemplateCompact>();
            this.TemplateTypeDetails = new List<TemplateTypeDetailCompact>();
        }

        public System.Guid Id { get; set; }
        public long ModuleMasterId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
        public virtual ICollection<GenericTemplateCompact> GenericTemplates { get; set; }
        public virtual ModuleMasterCompact ModuleMaster { get; set; }
        public virtual ICollection<TemplateTypeDetailCompact> TemplateTypeDetails { get; set; }
    }
}
