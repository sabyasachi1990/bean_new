using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.CommonModule.Infra
{

    public class LookUpCategory<T>
    {
        public LookUpCategory()
        {
            List<LookUp<T>> Lookups = new List<LookUp<T>>();
            List<LookUpGuid<T>> LookUps = new List<LookUpGuid<T>>();
            List<LookUpUser<T>> LookUpUsers = new List<LookUpUser<T>>();
        }
        public string CategoryName { get; set; }
        public string Code { get; set; }
        public string DefaultValue { get; set; }
        public long Id { get; set; }
        public bool? IsActive { get; set; }
        public List<LookUp<T>> Lookups { get; set; }
        public List<LookUpGuid<T>> LookUps { get; set; }
        public List<LookUpUser<T>> LookUpUsers { get; set; }
    }
    public class LookUp<T>
    {
        public T Code { get; set; }
        public long? Id { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public int? RecOrder { get; set; }
        public double? TOPValue { get; set; }
        public string Currency { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string TaxCode { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsAllowDisAllow { get; set; }
        RecordStatusEnum _status;
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
    }
    public class LookUpGuid<T>
    {
        public T Code { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public int? RecOrder { get; set; }

        public long? TOPId { get; set; }
        public string EntityName { get; set; }
        public string Nature { get; set; }

        public decimal? CustCreditlimit { get; set; }
        public long? ServiceEntityId { get; set; }
    }
    public class LookUpUser<T>
    {
        public T Username { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? RecOrder { get; set; }
    }
    public class TaxCodeLookUp<T>
    {
        public T Code { get; set; }
        public long? Id { get; set; }
        public string Name { get; set; }
        public int? RecOrder { get; set; }
        public double? TaxRate { get; set; }
        public string TaxIdCode { get; set; }
        public string TaxType { get; set; }
        public bool? IsApplicable { get; set; }
        public bool? IsInterCoBillingTaxCode { get; set; }
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

        public bool? IsTaxAmountEditable { get; set; }
    }
    public class LookUpCompany<T>
    {
        public T Code { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool? IsBaseCompany { get; set; }
        public bool? isGstActivated { get; set; }
        public bool? IsIBServiceEntity { get; set; }
        public bool? IsICServiceEntity { get; set; }
        public List<LookUp<T>> LookUps { get; set; }
        public bool? IsHyperLinkEnable { get; set; }

    }
    public class COALookup<T>
    {
        public T Code { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsAllowDisAllow { get; set; }
        public bool? IsPLAccount { get; set; }
        public int? RecOrder { get; set; }
        public string Currency { get; set; }
        public string PayReceivableAccName { get; set; }
        public string Class { get; set; }
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
        public bool? IsTaxCodeNotEditable { get; set; }
        public bool? IsBank { get; set; }
        public long? SubsidaryCompanyId { get; set; }
        public bool? IsInterCoBillingCOA { get; set; }
    }
    public class LookUpVendor<T>
    {
        public T Code { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? RecOrder { get; set; }
        public long? TOPId { get; set; }
        public string EntityName { get; set; }
        public string Nature { get; set; }
        public decimal? CustCreditlimit { get; set; }
        public string COAName { get; set; }
        public string TaxCode { get; set; }
        public long? COAId { get; set; }
        public List<long> TaxId { get; set; }
    }
    public class EntityTaxCodeLookUp<T>
    {
        public T Code { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public int? RecOrder { get; set; }
        public double? TaxRate { get; set; }
        public string TaxIdCode { get; set; }
        public string TaxType { get; set; }
        public string TaxCode { get; set; }
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
    }
    public class CommonLookUps<T>
    {
        public string TableName { get; set; }
        public T Code { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public int? RecOrder { get; set; }
        public double? TaxRate { get; set; }
        public string TaxType { get; set; }
        public string TaxCode { get; set; }
        public string Currency { get; set; }
        public string Class { get; set; }
        public double? TOPValue { get; set; }
        //public bool? IsDefault { get; set; }
        public string ShortName { get; set; }
        public bool? isGstActivated { get; set; }
        public List<LookUp<T>> LookUps { get; set; }
        //public long? ServiceCompanyId { get; set; }
        public string CategoryName { get; set; }
        public string DefaultValue { get; set; }
        public string COACategory { get; set; }
        public bool? IsInterCo { get; set; }
        //public long? ControlCodeId { get; set; }
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
    }
}
