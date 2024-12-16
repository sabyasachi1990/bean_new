using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Entities.Models
{
    public class Invoice : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public System.Guid EntityId { get; set; }
        public string Nature { get; set; }
        public string DocCurrency { get; set; }
        [DataType("decimal(15 ,10")]
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public string DocumentState { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public long? ServiceCompanyId { get; set; }
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
        public Guid? DocumentId { get; set; }
    }
}
