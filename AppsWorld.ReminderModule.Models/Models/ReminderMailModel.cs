using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Models.Models
{
    public class ReminderMailModel
    {
        public ReminderMailModel()
        {
        }
        public List<Guid> Ids { get; set; }
        public string UserName { get; set; }
        public bool isConfirm { get; set; }
        public bool isPreview { get; set; }
        public string UserFirstName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Ccmail { get; set; }
        public string bccmail { get; set; }
    }
}
