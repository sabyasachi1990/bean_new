using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class GetEntityModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Nullable<long> CustTOPId { get; set; }
        public Nullable<double> CustTOPValue { get; set; }
        public string CustNature { get; set; }
    }
}
