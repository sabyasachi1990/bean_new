using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Entities.Models.V2
{
    public class MediaRepository : Entity
    {
        public System.Guid Id { get; set; }
        public int? CompanyId { get; set; }
        public string SourceType { get; set; }
        public string MediaType { get; set; }
        public string Original { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Large { get; set; }
        public string CssSprite { get; set; }
        public int? Status { get; set; }
    }
}
