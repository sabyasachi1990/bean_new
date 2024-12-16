using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Models
{
    public class EntityModel
    {
       public string EntityName {get;set;}
        public string EntityType { get; set; }
        public Nullable<Guid> EntityId { get; set; }
    }
}
