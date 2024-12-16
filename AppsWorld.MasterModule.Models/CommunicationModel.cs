using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class CommunicationModel
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public string CommunicationType { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string SentBy { get; set; }
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public string Subject { get; set; }
        public string UserCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.Guid> TemplateId { get; set; }
        RecordStatusEnum _status;
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get { return _status; }
            set { _status = (RecordStatusEnum)value; }
        }
        public string TemplateName { get; set; }
        public string TemplateCode { get; set; }
        public string ReportPath { get; set; }
        public string Category { get; set; }
        public string ModifiedBy { get; set; }
        public decimal? InvoiceFee { get; set; }
        public string InvoiceType { get; set; }
        public string DocNo { get; set; }
        public string CaseNumber { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string AzurePath { get; set; }
        public string EntityName { get; set; }
        public string ScreenName { get; set; }
        public string DisplayFileName { get; set; }
        public Guid EntityId { get; set; }
    }
}
