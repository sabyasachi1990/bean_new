using AppsWorld.CommonModule.Infra;
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

namespace AppsWorld.BillModule.Entities
{
    public class PeppolInboundInvoice : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid? DocId { get; set; }
        public string SenderPeppolId { get; set; }
        public string ReciverPeppolId { get; set; }
        public string XmlFilepath { get; set; }
        public string XMLFileData { get; set; }

        [StringLength(254)]
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
     
        public Nullable<short> Version { get; set; }


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
        public string  ErrorMessage { get; set; }
    }
}
