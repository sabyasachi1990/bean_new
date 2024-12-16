using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Models.V2
{
    public class EmailModel
    {

        [StringLength(100)]
        public string TemplateName { get; set; }

        public Guid InvoiceId { get; set; }

    }
}
