using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities
{
    public partial class Journal : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
    }
}
