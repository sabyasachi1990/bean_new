using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class GSTSetting : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public string Number { get; set; }
        //[Required]
        //[DefaultDate(ErrorMessage = "The DateOfRegistration field is required.")]
        public Nullable<System.DateTime> DateOfRegistration { get; set; }
        public Nullable<System.DateTime> DeRegistration { get; set; }
        public Nullable<bool> IsDeregistered { get; set; }
        [Required]
        [StringLength(30)]
        public string ReportingYearEnd { get; set; }
        [Required]
        [StringLength(20)]
        public string ReportingInterval { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [StringLength(254)]
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [StringLength(50)]
        public string GSTRepoCurrency { get; set; }

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
        [NotMapped]
        public string RecStatus { get; set; }
        public long? ServiceCompanyId { get; set; }
        //public virtual Company Company { get; set; }
    }
    public class DefaultDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return d != DateTime.MinValue;
        }
    }
}
