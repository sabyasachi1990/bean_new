using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
//using FrameWork;
using AppsWorld.MasterModule.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class BeanEntityModel
    {

        public BeanEntityModel()
        {
            //List<CCAccountTtpeNew> CCAccountTypeNameLU = new List<CCAccountTtpeNew>();
            //List<TermsOfPaymentNew> CreditTermsNameLU = new List<TermsOfPaymentNew>();
            //List<TermsOfPaymentNew> PaymentTermsNameLU = new List<TermsOfPaymentNew>();
            //List<IdTypeNew> IdTypeNameLU = new List<IdTypeNew>();
            //this.Addresses = new List<Address>();
            this.ContactModelList = new List<ContactModel>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public Nullable<long> TypeId { get; set; }
        public Nullable<long> IdTypeId { get; set; }
        [StringLength(50)]
        public string IdNo { get; set; }
        [StringLength(50)]
        public string GSTRegNo { get; set; }
        public Nullable<bool> IsCustomer { get; set; }
        public Nullable<long> CustTOPId { get; set; }
        [StringLength(50)]
        public string CustTOP { get; set; }
        public string EventStatus { get; set; }
        public Nullable<double> CustTOPValue { get; set; }
        public Nullable<decimal> CustCreditLimit { get; set; }
        public bool IsContactExist { get; set; }
        [StringLength(5)]
        public string CustCurrency { get; set; }
        [StringLength(25)]
        public string CustNature { get; set; }
        public Nullable<bool> IsVendor { get; set; }
        public Nullable<long> VenTOPId { get; set; }
        [StringLength(50)]
        public string VenTOP { get; set; }
        public Nullable<double> VenTOPValue { get; set; }
        public Nullable<decimal> VenCreditLimit { get; set; }
        [StringLength(5)]
        public string VenCurrency { get; set; }
        [StringLength(25)]
        public string VenNature { get; set; }

        public bool? IsMultiCurrencyActivated { get; set; }
        public Nullable<int> RecOrder { get; set; }
        [StringLength(1000)]
        public string Remarks { get; set; }
        [StringLength(254)]
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [StringLength(254)]
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }

        public string Communication { get; set; }
        public string VendorType { get; set; }
        public string ExternalEntityType { get; set; }
        public bool? IsShowPayroll { get; set; }

        AppsWorld.Framework.RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public AppsWorld.Framework.RecordStatusEnum Status
        {
            get { return _status; }
            set { _status = (AppsWorld.Framework.RecordStatusEnum)value; }
        }
        public bool? IsExternalData { get; set; }
        public Guid? DocumentId { get; set; }
        public long? COAId { get; set; }
        public long? TaxId { get; set; }
        public List<ContactModel> ContactModelList { get; set; }
        public virtual List<Address> Addresses { get; set; }
        public List<LookUpCategory<string>> CCAccountTypeLU { get; set; }
        public List<LookUp<string>> CreditTermsLU { get; set; }
        public List<LookUp<string>> PaymentTermsLU { get; set; }
        public List<LookUp<string>> IdTypeLU { get; set; }
        public LookUpCategory<string> CurrencyLU { get; set; }
        public List<string> NatureLU { get; set; }
        public LookUpCategory<string> VendorTypeLU { get; set; }
        public LookUpCategory<string> AddressBookLU { get; set; }
        public LookUpCategory<string> IndustriesLU { get; set; }
        public List<COALookup<string>> ChartOfAccountLU { get; set; }
        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        public FrameWork.LookUpCategory<string> CommunicationLU { get; set; }
        public string PrincipalActivities { get; set; }
        public string Industry { get; set; }
        public string IndustryCode { get; set; }
        //public decimal? BillingsAmount { get; set; }
        //public decimal? ReceiptsAmount { get; set; }
        //public decimal? BalanceAmount { get; set; }
        //public decimal? CreditAmount { get; set; }
        //public decimal? DebtProvAmount { get; set; }
        //public decimal? NetBalAmount { get; set; }
        public string PeppolDocumentId { get; set; }

        public bool IsPeppolEnable { get; set; }
    }
}
