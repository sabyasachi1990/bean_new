using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Models;

namespace AppsWorld.RevaluationModule.Models
{
    public class RevaluationModel
    {
        public RevaluationModel()
        {
            this.RecOrder = ++_count;
        }
        private static int _count = 0;
        public int? RecOrder { get; set; }
        public System.Guid Id { get; set; }
        public System.Guid RevalutionId { get; set; }
        public long? COAId { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public string DocCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public decimal? OrgExchangeRate { get; set; }
        public decimal? RevalExchangeRate { get; set; }
        public decimal? BaseBal { get; set; }
        public decimal? DocBal { get; set; }
        public decimal? RevalBal { get; set; }
        public decimal? UnrealisedExchangegainorlose { get; set; }
        public string EntityName { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string ServiceEntityName { get; set; }
        public string COAName { get; set; }
        public string TableName { get; set; }
        public bool? IsChecked { get; set; }
        public bool? IsBankData { get; set; }
        public string Nature { get; set; }
        public Guid? DocumentId { get; set; }
        public string COAClass { get; set; }
        public string Version { get; set; }

        //RecordStatusEnum _status;
        //[Required]
        //[JsonConverter(typeof(StringEnumConverter))]
        //[StatusValue]
        //public RecordStatusEnum Status
        //{
        //    get
        //    {
        //        return _status;
        //    }
        //    set { _status = (RecordStatusEnum)value; }
        //}
    }
}
