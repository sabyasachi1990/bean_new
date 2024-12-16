using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Models.V2
{
    public class EmailTemplatesVM
    {
        public Guid ScreenId { get; set; }
        public string CursorName { get; set; }
        //public string Type { get; set; }
        public long CompanyId { get; set; }
        public string ScreenName { get; set; }
        public string TemplateName { get; set; }
        public string TemplateContent { get; set; }
        public string Attachments { get; set; }
        public string AttachmentName { get; set; }
        public string EmailBody { get; set; }
        public List<FrameWork.LookUps.LookUp<string>> ToEmails { get; set; }
        public string Subject { get; set; }
        public string ToEmail { get; set; }
        public string CcMail { get; set; }
        public MustacheModel mustacheModel { get; set; }

    }
}
