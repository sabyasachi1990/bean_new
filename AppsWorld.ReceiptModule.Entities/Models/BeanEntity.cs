using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class BeanEntity : Entity
    {
        //public BeanEntity()
        //{
        //    //this.Invoices = new List<Invoice>();
        //    //this.Receipts = new List<Receipt>();
        //}
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(25)]
        public string CustNature { get; set; }
        public decimal? CreditLimitValue { get; set; }
        [StringLength(5)]
        public string VenCurrency { get; set; }

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
