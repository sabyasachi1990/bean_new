using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork
{
    public static class DocTypeConstants
    {
        public const string Receipt = "Receipt";
        public const string Bill = "VendorBill";
		public const string Invoice = "Invoice";
		public const string DebitNote = "Debit Note";
        public const string CreditNote = "Credit Note";
		public const string DoubtFulDebitNote = "Doubtful Debt";
	    public const string BillCreditMemo = "Credit Memo";
		public const string JournalVocher = "Journal";
        public const string Payment = "Payment";
        public const string Bills = "Bill";
        public const string CashSale = "Cash Sale";
        public const string Withdrawal = "Withdrawal";
        public const string Deposit = "Deposit";
        public const string OpeningBalance = "OpeningBalance";        
    }

    public enum CreditNoteApplicationStatus
    {
        Posted = 1,
        Reset = 2
    }
    public enum DoubtfulDebtAllocationStatus
    {
        Posted = 1,
        Reset = 2
    }
}
