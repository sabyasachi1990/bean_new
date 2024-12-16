using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Models
{
    public class InvoiceVM
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<decimal> InvoiceFee { get; set; }
        public Nullable<decimal> TotalFee { get; set; }
        public Nullable<decimal> BalanceFee { get; set; }
        public string InvoiceState { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        RecordStatusEnum _status;

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get { return _status; }
            set { _status = (RecordStatusEnum)value; }
        }
    }
}
