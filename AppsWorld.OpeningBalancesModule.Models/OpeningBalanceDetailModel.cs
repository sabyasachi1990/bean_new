using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class OpeningBalanceDetailModel
    {
        public OpeningBalanceDetailModel()
        {
            this.LineItems = new List<OpeningBalanceLineItemModel>();
        }
        public Guid Id { get; set; }
        public Guid OpeningBalanceId { get; set; }
        [Required]
        public long COAId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> DocCredit { get; set; }
        public Nullable<decimal> DocDebit { get; set; }
        [Required]
        public string BaseCurrency { get; set; }
        public Nullable<decimal> BaseCredit { get; set; }
        public Nullable<decimal> BaseDebit { get; set; }
        public int ShowLineItemCount { get; set; }
        public List<OpeningBalanceLineItemModel> LineItems { get; set; }
        public bool IsCashandbank { get; set; }
        public string PayReceivableAccName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string RecordStatus { get; set; }
        public bool IsSystemAccount { get; set; }
        public int ObjectState { get; set; }
        public bool? IsOrginalAccount { get; set; }
        public bool? IsAllowDisAllow { get; set; }
        public bool IsPLAccount { get; set; }
        public string Nature { get; set; }
        public string ClearingState { get; set; }
        public DateTime? ClearingDate { get; set; }
        public DateTime? ReconciliationDate { get; set; }
        public Guid? ReconciliationId { get; set; }
        public int RecOrder { get; set; }
        public bool? IsEditEnable { get; set; }
    }
}
