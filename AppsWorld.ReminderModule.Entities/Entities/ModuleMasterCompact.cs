using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Entities
{
    public class ModuleMasterCompact : Entity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
    }
}
