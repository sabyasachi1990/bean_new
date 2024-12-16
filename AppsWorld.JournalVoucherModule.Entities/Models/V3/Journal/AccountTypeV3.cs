using AppsWorld.Framework;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Infra;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal
{
    public class AccountTypeV3 : Entity
    {

        public long Id { get; set; }
        public Guid? FRATId { get; set; }
        public long CompanyId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Class { get; set; }



        [StringLength(100)]
        public string Category { get; set; }


        [StringLength(100)]
        public string SubCategory { get; set; }


        //[StringLength(100)]
        //public string Nature { get; set; }

        //[StringLength(50)]
        //public string AppliesTo { get; set; }

        // public Nullable<bool> IsSystem { get; set; }
        //public Nullable<bool> ShowCurrency { get; set; }
        public Nullable<int> RecOrder { get; set; }
        //[StringLength(256)]
        //public string Remarks { get; set; }

        //[StringLength(254)]
        //public string UserCreated { get; set; }
       // public Nullable<System.DateTime> CreatedDate { get; set; }
        //[StringLength(254)]
        //public string ModifiedBy { get; set; }
      //  public Nullable<System.DateTime> ModifiedDate { get; set; }
       // public Nullable<short> Version { get; set; }
       // public Nullable<bool> ShowAllowable { get; set; }
      //  public Nullable<bool> ShowRevaluation { get; set; }
       // public Nullable<bool> ShowCashflowType { get; set; }
       // public string Indexs { get; set; }
       // public string ModuleType { get; set; }

        //RecordStatusEnum _status;
        //[Required]
        //[JsonConverter(typeof(StringEnumConverter))]
        //[StatusValue]
        //public RecordStatusEnum Status
        //{
        //    get
        //    {
        //        return _status;
        //    }
        //    set { _status = (RecordStatusEnum)value; }
        //}
        //[NotMapped]
        //public string RecStatus { get; set; }
    }
}
