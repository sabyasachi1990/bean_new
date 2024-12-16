using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Entities.Models
{
    public partial class BeanEntity:Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public bool? IsCustomer { get; set; }
        public bool? IsVendor { get; set; }
    }
}
