using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using AppsWorld.CommonModule.Infra;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AppsWorld.CreditMemoModule.Entities
{
    public partial class Journal:Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.Guid? DocumentId { get; set; }
        //public long? ServiceCompanyId { get; set; }
        public string SystemReferenceNo { get; set; }
        public string DocSubType { get; set; }
        public bool? IsWithdrawal { get; set; }
        public string DocumentState { get; set; }
        //public string ModifiedBy { get; set; }
        //public Nullable<System.DateTime> ModifiedDate { get; set; }
        public decimal? BalanceAmount { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        CreditMemoApplicationStatus _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public CreditMemoApplicationStatus Status
        {
            get
            {
                return _status;
            }
            set { _status = (CreditMemoApplicationStatus)value; }
        }
    }
}
