using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Models
{
    public class EmailModel
    {
        public Guid ScreenId { get; set; }
        public string CursorName { get; set; }
        //public string Type { get; set; }
        public long? CompanyId { get; set; }
        public string ScreenName { get; set; }
        public string TemplateName { get; set; }
        public string TemplateContent { get; set; }
        public string UserName { get; set; }

    }
}
