using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class CompanyUser : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Username { get; set; }
       // public string ServiceEntities { get; set; }

    }

}
