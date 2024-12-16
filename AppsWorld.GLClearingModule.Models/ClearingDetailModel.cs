using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppsWorld.GLClearingModule.Models
{
   public class ClearingDetailModel
    {
        public System.Guid Id { get; set; }
        public System.Guid GLClearingId { get; set; }
        public System.Guid? DocumentId { get; set; }
        //public System.Guid? TypeId { get; set; }
        public string DocType { get; set; }
        public System.DateTime DocDate { get; set; }
        public System.DateTime? BankClearingDate { get; set; }
        public string DocNo { get; set; }
        public string SystemRefNo { get; set; }
        public decimal? DocAmount { get; set; }
        public string  DocCurrency { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? DocCredit { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? BaseCredit { get; set; }
        public decimal? BaseDebit { get; set; }
        public string BaseCurrency { get; set; }
        //public string CrDr { get; set; }
        public int? RecOrder { get; set; }
        public bool? IsCheck { get; set; }
        public string ClearingStatus { get; set; }
        public string RecordStatus { get; set; }
        public string EntityName { get; set; }
        public string AccountDescription { get; set; }
        public string DocSubType { get; set; }
        public Guid? DocumentDetailId { get; set; }

    }
}
