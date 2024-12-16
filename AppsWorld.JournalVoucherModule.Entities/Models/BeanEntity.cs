using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class BeanEntity : Entity
    {
        public BeanEntity()
        {
            //this.Invoices = new List<Invoice>();
        }
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        //public Nullable<long> TypeId { get; set; }
        //public Nullable<long> IdTypeId { get; set; }
        //[StringLength(50)]
        //public string IdNo { get; set; }
        //[StringLength(50)]
        //public string GSTRegNo { get; set; }
        public Nullable<bool> IsCustomer { get; set; }
        //public Nullable<long> CustTOPId { get; set; }
        //[StringLength(50)]
        //public string CustTOP { get; set; }
        //public Nullable<double> CustTOPValue { get; set; }
        public Nullable<decimal> CustCreditLimit { get; set; }
        //[StringLength(5)]
        //public string CustCurrency { get; set; }
        //[StringLength(25)]
        //public string CustNature { get; set; }
        //public Nullable<bool> IsVendor { get; set; }
        //public Nullable<long> VenTOPId { get; set; }
        //[StringLength(50)]
        //public string VenTOP { get; set; }
        //public Nullable<double> VenTOPValue { get; set; }
        //public Nullable<decimal> VenCreditLimit { get; set; }
        //[StringLength(5)]
        //public string VenCurrency { get; set; }
        //[StringLength(25)]
        //public string VenNature { get; set; }
        //public Nullable<Guid> AddressBookId { get; set; }
        //public Nullable<int> RecOrder { get; set; }
        //[StringLength(1000)]
        //public string Remarks { get; set; }
        //[StringLength(254)]
        //public string UserCreated { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //[StringLength(254)]
        //public string ModifiedBy { get; set; }
        //public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public Nullable<short> Version { get; set; }

        //public string Communication { get; set; }

        public string VendorType { get; set; }
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
        //public virtual ICollection<Invoice> Invoices { get; set; }

        //public virtual AccountType1 AccountType1 { get; set; }
        //public virtual AddressBook AddressBook { get; set; }
        //public virtual Company Company { get; set; }
        //public virtual IdType IdType { get; set; }
        //public virtual Invoice Invoice { get; set; }
      
    }
}
