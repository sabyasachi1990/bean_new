using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Models.Models
{
    public class EmailModelNew
    {
        public string TempletContent { get; set; }
        public string EmailBody { get; set; }
        public string ReportPath { get; set; }
        public string Attachement { get; set; }
        public string OriginalFileName { get; set; }
        public List<FrameWork.LookUps.LookUp<string>> ToEmails { get; set; }
    }
}
