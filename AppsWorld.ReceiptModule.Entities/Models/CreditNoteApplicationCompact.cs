using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class CreditNoteApplicationCompact : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid InvoiceId { get; set; }
        public long CompanyId { get; set; }
       // public System.DateTime CreditNoteApplicationDate { get; set; }
        //public Nullable<System.DateTime> CreditNoteApplicationResetDate { get; set; }
       // public bool IsNoSupportingDocumentActivated { get; set; }
       // public Nullable<bool> IsNoSupportingDocument { get; set; }
        public decimal CreditAmount { get; set; }
       // public string Remarks { get; set; }
       // public string CreditNoteApplicationNumber { get; set; }
       // public string UserCreated { get; set; }
       // public Nullable<System.DateTime> CreatedDate { get; set; }
        //public string ModifiedBy { get; set; }
       // public Nullable<System.DateTime> ModifiedDate { get; set; }
       // public Nullable<decimal> ExchangeRate { get; set; }
        public Guid? DocumentId { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
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
