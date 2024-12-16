using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class DeleteViewModel
    {
        public Guid id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
    }
}
