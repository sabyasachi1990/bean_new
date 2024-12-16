using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.MasterModule.Entities
{
    public partial class Receipt : Entity
    {
        public Receipt()
        {
			 
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocNo { get; set; }
        public long COAId { get; set; }
        public decimal BankReceiptAmmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string EntityType { get; set; }
        public System.Guid EntityId { get; set; }
        public string DocumentState { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public Nullable<decimal> ReceiptApplicationAmmount { get; set; }
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
