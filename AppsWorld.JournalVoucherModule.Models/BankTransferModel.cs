using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class BankTransferModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string SystemRefNo { get; set; }
        public string DocDescription { get; set; }
        public System.DateTime TransferDate { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public string DocNo { get; set; }
        public Nullable<bool> IsNoSupportingDocument { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public bool IsMultiCompany { get; set; }
        public Nullable<bool> NoSupportingDocument { get; set; }
        public string ModeOfTransfer { get; set; }
        public string TransferRefNo { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public string DocumentState { get; set; }
        public string WithdrawalServiceCompanyName { get; set; }
        public string WithdrawalChartOfAccountName { get; set; }
        public string DepositServiceCompanyName { get; set; }
        public string DepositChartOfAccountName { get; set; }
        public long COAId { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string WithdrawalCurrency { get; set; }
        public decimal? WithdrawalAmount { get; set; }
        public string DepositCurrency { get; set; }
        public decimal? DepositAmount { get; set; }
        public string Type { get; set; }
        public string TransferFrom { get; set; }
        public string TransferTo { get; set; }
        public Guid? DocumentId { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public string AccountName { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? DocCredit { get; set; }
        public decimal? DepositExchangeRate { get; set; }
        public decimal? WithdrawalExchangeRate { get; set; }
        public Nullable<decimal> BaseDebit { get; set; }
        public Nullable<decimal> BaseCredit { get; set; }
        public Nullable<System.DateTime> WithdrawalClearingDate { get; set; }
        public Nullable<System.DateTime> DepositClearingDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ServiceCompanyName { get; set; }
        public virtual List<JVViewModel> JVViewModels { get; set; }
        public virtual List<BankTransferDetailModel> BankTransferDetailModels { get; set; }

    }
}
