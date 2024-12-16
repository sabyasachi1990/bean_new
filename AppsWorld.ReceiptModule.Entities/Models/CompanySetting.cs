using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class CompanySetting : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        //public string CursorName { get; set; }
        public string ModuleName { get; set; }
        //public string UserCreated { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //public string ModifiedBy { get; set; }
        //public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsEnabled { get; set; }
        public virtual Company Company { get; set; }
    }
}
