using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.TemplateModule.Entities.Models.V2
{
    public partial class BeanEntity : Entity
    {
       

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string IdNo { get; set; }
        public string GSTRegNo { get; set; }
        public ICollection<Invoice> Invoices { get; set; }

    }
}
