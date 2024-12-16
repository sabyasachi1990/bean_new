using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class ItemVM
    {
        public bool IsActiveItems { get; set; }
        public long companyId { get; set; }
        public List<Guid> itemIds { get; set; }
    }
}
