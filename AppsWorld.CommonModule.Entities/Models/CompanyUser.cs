using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Entities.Models
{
    public partial class CompanyUser : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Username { get; set; }
        public string ServiceEntities { get; set; }
        public virtual ICollection<CompanyUserDetail> CompanyUserDetails { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public Nullable<bool> IsPrimary { get; set; }
        //public Nullable<int> Status { get; set; }

        //public string Salutation { get; set; }
        //public string Remarks { get; set; }
        //public Nullable<System.DateTime> DeactivationDate { get; set; }
        ////public string PhoneNo { get; set; }
        //public bool IsAdmin { get; set; }
        //public System.Guid UserId { get; set; }
        //public string UserIntial { get; set; }
        ////[ForeignKey("UserId")]
        ////public virtual UserAccount UserAccount { get; set; }
        //public bool? IsFavourite { get; set; }
        ////Added by Pavan
        //public Guid? PhotoId { get; set; }
        //public string Communication { get; set; }
        //public string Gender { get; set; }
        //public string ModifiedBy { get; set; }
        //public Nullable<System.DateTime> ModifiedDate { get; set; }
        ////public virtual MediaRepository1 MediaRepository { get; set; }
        //public string Nationality { get; set; }

    }
}
