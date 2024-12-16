using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class ItemModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(20)]
        public string UOM { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        [StringLength(5)]
        public string Currency { get; set; }
        public long COAId { get; set; }
        public Nullable<long> DefaultTaxcodeId { get; set; }
        [StringLength(20)]
        public bool? AllowableDis { get; set; }

        public bool? IsEditabled { get; set; }
        public bool? IsAllowable { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }
        public Nullable<int> RecOrder { get; set; }
        [StringLength(256)]
        public string Remarks { get; set; }
        [StringLength(254)]
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [StringLength(254)]
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public bool? IsSaleItem { get; set; }
        public bool? IsPurchaseItem { get; set; }

        public bool? IsAccountEditable { get; set; }
        public bool? IsAllowableNotAllowableActivated { get; set; }
        public bool? IsMultiCurrencyActivated { get; set; }
        public bool? IsPLAccount { get; set; }
        public bool? IsGSTActivated { get; set; }        
 
        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
        //public string TaxCode { get; set; }
        public bool? IsExternalData { get; set; }
        public bool? IsIncidental { get; set; }
        public long? DocumentId { get; set; }

	   //[ForeignKey("COAId")]
	   //public virtual ChartOfAccount ChartOfAccounts { get; set; }

	   //[ForeignKey("DefaultTaxcodeId")]
	   //public virtual TaxCode TaxCodes { get; set; }
	   public string GroupBy { get; set; }
	   public string RecordStatus { get; set; }

    }
}
