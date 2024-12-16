using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Models
{
    public class TemplatesVM
    {
        public Guid ScreenId { get; set; }
        public string CursorName { get; set; }
        //public string Type { get; set; }
        public long CompanyId { get; set; }
        public string ScreenName { get; set; }
        public string MenuType { get; set; }
        public string TemplateContent { get; set; }
        public string MailBody { get; set; }
        public string TemplateType { get; set; }
        public Guid EntityId { get; set; }
        public List<FrameWork.LookUps.LookUp<string>> ToEmails { get; set; }

        #region Old
        //public long CompanyId { get; set; }

        //public Guid ScreenId { get; set; }


        //public Guid EntityId { get; set; }

        //public string TemplateType { get; set; }
        //public string MenuType { get; set; }
        //public string Subject { get; set; }
        //public string Attachments { get; set; }
        //public string Content { get; set; }
        //public string TemplateContent { get; set; }
        //public string EntityName { get; set; }
        #endregion


    }
}
