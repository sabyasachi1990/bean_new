using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Models
{
    public class BankTransferDetailModel
    {
        public System.Guid Id { get; set; }
        public System.Guid BankTransferId { get; set; }
        public string ChartOfAccountName { get; set; }
        public string ServiceCompanyName { get; set; }
        [Required(ErrorMessage = "COAId Is Required")]
        public long COAId { get; set; }
        [Required(ErrorMessage = "ServiceCompanyId Is Required")]
        public long ServiceCompanyId { get; set; }
        [Required(ErrorMessage = "Currency Is Required")]
        public string Currency { get; set; }
        [Required(ErrorMessage = "Amount Is Required")]
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string ClearingState { get; set; }
        public bool? IsHyperLinkEnable { get; set; }

    }
}
