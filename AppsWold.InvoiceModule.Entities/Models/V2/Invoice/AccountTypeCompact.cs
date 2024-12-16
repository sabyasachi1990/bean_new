using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    public partial class AccountTypeCompact : Entity
    {
        public AccountTypeCompact()
        {
            this.ChartOfAccounts = new List<ChartOfAccountCompact>();
        }

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
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
        [ForeignKey("AccountTypeId")]
        public virtual List<ChartOfAccountCompact> ChartOfAccounts { get; set; }
    }
}
