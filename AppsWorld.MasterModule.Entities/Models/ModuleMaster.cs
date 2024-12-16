using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;
namespace AppsWorld.MasterModule.Entities
{
    public partial class ModuleMaster : Entity
    {

        public long Id { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
    }
}
