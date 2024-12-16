using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
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
    public class QuickBeanEntityModel
    {
        public QuickBeanEntityModel()
        {

        }
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string ExchangeRate { get; set; }
        public long? TermsOfPayment { get; set; }
        public string CustCurrency { get; set; }
        [StringLength(25)]
        public string CustNature { get; set; }
        public string VenCurrency { get; set; }
        [StringLength(25)]
        public string VenNature { get; set; }
        public decimal? CustCreditLimit { get; set; }
        public Guid? ContactId { get; set; }
        public string Salutation { get; set; }
        public string ContactName { get; set; }
        public string Communication { get; set; }
        public LookUpCategory<string> TermsOfPaymentLU { get; set; }
        public LookUpCategory<string> CurrencyLU { get; set; }
        //public LookUpCategory<string> NatureLU { get; set; }
        public List<string> NatureLU { get; set; }
        public LookUpCategory<string> VendorTypeLU { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [StringLength(254)]
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public bool? IsMultyCurrency { get; set; }
        public Nullable<bool> IsVendor { get; set; }
        public Nullable<bool> IsCustomer { get; set; }
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
        public bool? IsExternalData { get; set; }
        public Guid? DocumentId { get; set; }
        public long? COAId { get; set; }
        public long? TaxId { get; set; }
        public List<COALookup<string>> ChartOfAccountLU { get; set; }
        public List<EntityTaxCodeLookUp<string>> TaxCodeLU { get; set; }
        //public LookUpCategory<string> CommunicationLU { get; set; }
        public LookUpCategory<string> SalutationLU { get; set; }
        public FrameWork.LookUpCategory<string> CommunicationLU { get; set; }

    }


}
