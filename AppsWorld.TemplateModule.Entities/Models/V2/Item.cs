using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.TemplateModule.Entities.Models.V2
{
    public partial class Item : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }

        public string Code { get; set; }
    
       
    }
}
