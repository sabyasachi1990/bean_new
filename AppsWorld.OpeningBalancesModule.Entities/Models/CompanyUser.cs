using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Entities
{
    public class CompanyUser:Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Username { get; set; }
        public string ServiceEntities { get; set; }
    }
}
