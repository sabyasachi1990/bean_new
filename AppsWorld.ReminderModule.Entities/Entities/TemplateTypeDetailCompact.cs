using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Entities
{
    public partial class TemplateTypeDetailCompact : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid TemplateTypeId { get; set; }
        public string ViewModelName { get; set; }
        public string ViewModelJson { get; set; }
        public virtual TemplateTypeCompact TemplateType { get; set; }
    }
}
