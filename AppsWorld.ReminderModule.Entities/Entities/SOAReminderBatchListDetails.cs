using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Entities
{
    public class SOAReminderBatchListDetails : Entity
    {
        //public SOAReminderBatchListDetails()
        //{
        //    this.SOAReminderBatchList = new SOAReminderBatchList();
        //}
        public System.Guid Id { get; set; }
        public System.Guid MasterId { get; set; }
        public System.Guid EntityId { get; set; }
        public System.Guid DocumentId { get; set; }
        public DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public decimal? DocBalance { get; set; }
        public decimal DocumentTotal { get; set; }
        public decimal? CreditNoteBalance { get; set; }
        public string Currency { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string Remarks { get; set; }
        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
        public virtual SOAReminderBatchList SOAReminderBatchList { get; set; }
    }
}
