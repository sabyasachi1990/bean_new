using AppsWorld.Framework;
using FrameWork;
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
    public class BeanEntityModelk
    {
        public string Name { get; set; }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string CustNature { get; set; }
        public Nullable<bool> IsCustomer { get; set; }
        public Nullable<bool> IsVendor { get; set; }
        public string VenNature { get; set; }
        public string VenCurrency { get; set; }
        public string CustCurrency { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Type { get; set; }
        public string CreditTerms { get; set; }
        public string PaymentTerms { get; set; }
        public double? CustCreditLimit { get; set; }
        public double? VenCreditLimit { get; set; }
        public string VendorType { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string UserCreated { get; set; }
        public string BaseCurrency { get; set; }
        public string Status { get; set; }
        //RecordStatusEnum _status;
        //[Required]
        //[JsonConverter(typeof(StringEnumConverter))]
        //[StatusValue]
        //public RecordStatusEnum Status
        //{
        //    get { return _status; }
        //    set { _status = (RecordStatusEnum)value; }
        //}
        public bool? IsExternalData { get; set; }
        public double? CustBal { get; set; }
        public decimal? VenBal { get; set; }


        public string  PeppolId { get; set; }
    }
}
