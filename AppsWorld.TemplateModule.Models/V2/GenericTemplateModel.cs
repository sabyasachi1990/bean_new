using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Models.V2
{
   public class GenericTemplateModel
    {
        public System.Guid Id { get; set; }
        public string TempletContent { get; set; }
        public Nullable<bool> IsFooterExist { get; set; }
        public Nullable<bool> IsHeaderExist { get; set; }
        public string Attachments { get; set; }
    }
}
