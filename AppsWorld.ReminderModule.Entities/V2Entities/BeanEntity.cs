using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.V2Entities
{
    public class BeanEntity : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Entity Name")]
        public string Name { get; set; }
      
      
  


    }
}
