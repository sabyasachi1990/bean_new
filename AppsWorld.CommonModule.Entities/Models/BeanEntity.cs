using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.CommonModule.Entities
{
    public partial class BeanEntity : Entity
    {
        public BeanEntity()
        {

        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        //public Nullable<long> TypeId { get; set; }
        //public Nullable<long> IdTypeId { get; set; }
        //public string IdNo { get; set; }
        public string GSTRegNo { get; set; }
        public Nullable<bool> IsCustomer { get; set; }
        //public Nullable<long> CustTOPId { get; set; }
        //public string CustTOP { get; set; }
        //public Nullable<double> CustTOPValue { get; set; }
        //public Nullable<decimal> CustCreditLimit { get; set; }
        //public string CustCurrency { get; set; }
        public string CustNature { get; set; }
        public Nullable<bool> IsVendor { get; set; }
        public Nullable<long> VenTOPId { get; set; }
        public string VenTOP { get; set; }
        //public Nullable<double> VenTOPValue { get; set; }
        public string VenCurrency { get; set; }
        public string VenNature { get; set; }
        //public Nullable<System.Guid> AddressBookId { get; set; }
        //public Nullable<bool> IsLocal { get; set; }
        //public string ResBlockHouseNo { get; set; }
        //public string ResStreet { get; set; }
        //public string ResUnitNo { get; set; }
        //public string ResBuildingEstate { get; set; }
        //public string ResCity { get; set; }
        //public string ResPostalCode { get; set; }
        //public string ResState { get; set; }
        //public string ResCountry { get; set; }
        //public Nullable<int> RecOrder { get; set; }
        //public string Remarks { get; set; }
        //public string UserCreated { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //public string ModifiedBy { get; set; }
        //public Nullable<System.DateTime> ModifiedDate { get; set; }
        public decimal? CreditLimitValue { get; set; }
        //public Nullable<short> Version { get; set; }
        public bool? IsShowPayroll { get; set; }
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
        public Nullable<decimal> VenCreditLimit { get; set; }
        //public string Communication { get; set; }
        //public string VendorType { get; set; }
        public long? COAId { get; set; }
        public long? TaxId { get; set; }
        public long? ServiceEntityId { get; set; }

        //public virtual Company Company { get; set; }


        public string PeppolDocumentId { get; set; }

    }
}