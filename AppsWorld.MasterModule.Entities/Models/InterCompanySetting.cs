using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.MasterModule.Entities
{
    public partial class InterCompanySetting : Entity
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string InterCompanyType { get; set; }
        public bool? IsInterCompanyEnabled { get; set; }

        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
       public virtual ICollection<InterCompanySettingDetail> InterCompanySettingDetails { get; set; }

    }
}
