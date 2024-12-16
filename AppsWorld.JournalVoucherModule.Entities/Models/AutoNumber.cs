using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using System;
using Repository.Pattern.Ef6;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.BillModule.Entities;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class AutoNumber : Entity
    {
        public AutoNumber()
        {
            this.AutoNumberCompanies = new List<AutoNumberCompany>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<long> ModuleMasterId { get; set; }
        public string EntityType { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public string GeneratedNumber { get; set; }
        public Nullable<int> CounterLength { get; set; }
        public Nullable<int> MaxLength { get; set; }
        public Nullable<long> StartNumber { get; set; }
        public string Reset { get; set; }
        public string Preview { get; set; }
        public Nullable<bool> IsResetbySubsidary { get; set; }
        public Nullable<int> Status { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Variables { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsDisable { get; set; }
        public virtual Company Company { get; set; }
        //public virtual ModuleMaster ModuleMaster { get; set; }
        public virtual ICollection<AutoNumberCompany> AutoNumberCompanies { get; set; }
    }
}
