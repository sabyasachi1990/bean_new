using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Entities
{
   public class ActivityHistory:Entity
    {
       public Guid Id { get; set; }
       public long CompanyId { get; set; }
       public string Type { get; set; }
       public string Activity { get; set; }
       public string Action { get; set; }
       public string CreatedBy { get; set; }
       public DateTime CreateDate { get; set; }
       public Guid DocumentId { get; set; }
    }
}
