using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Models
{
    public class RevalDetailModel
    {
        public long? ServiceEntityId { get; set; }
        public long? COAID { get; set; }
        public Guid? EntityId { get; set; }
        public string COAName { get; set; }
        public string ServiceEntityName { get; set; }
        public string EntityName { get; set; }
        public string TABLENAME { get; set; }
        public string Nature { get; set; }
    }
}
