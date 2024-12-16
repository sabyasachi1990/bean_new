using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Entities.Models
{
    public partial class ChartOfAccount : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsBank { get; set; }
        public bool? ShowRevaluation { get; set; }
        public Nullable<int> IsRevaluation { get; set; }
        public Nullable<bool> Revaluation { get; set; }
        //[NotMapped]
        public long? AccountTypeId { get; set; }
        public string Nature { get; set; }
    }
}
