using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Models
{
    public class ContactEmailModel
    {
        public Guid? ContactId { get; set; }

        public string Email { get; set; }
        public bool? IsPrimary { get; set; }
    }
}
