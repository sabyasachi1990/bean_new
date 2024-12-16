using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Infra
{
    public static class DocTypeConstants
    {
        public const string Receipt = "Receipt";
        public const string Bill = "VendorBill";
        public const string Invoice = "Invoice";
        public const string DebitNote = "Debit Note";
        public const string CreditNote = "Credit Note";
        //public const string DoubtFulDebitNote = "Debt Provision";
        public const string DoubtFulDebitNote = "Debt Provision";
        public const string BillCreditMemo = "Credit Memo";
        public const string JournalVocher = "Journal";
        public const string Payment = "Payment";
        public const string Bills = "Bill";
        public const string CashSale = "Cash Sale";
        public const string Withdrawal = "Withdrawal";
        public const string Deposit = "Deposit";
        //public const string OpeningBalance = "Opening Balance";
        public const string OpeningBalance = "Opening Bal";
        public const string Provision = "Provision";
        public const string Revaluation = "Reval";
        public const string BankTransfer = "Transfer";
        public const string CashPayment = "Cash Payment";
        public const string GLClearing = "Clearing";
        public const string PayrollBills = "Payroll Bill";
        public const string PayrollBill = "Payroll";
        public const string PayrollPayments = "Payroll Payment";
        public const string PayrollPayment = "Payroll";
        public const string ReceiptDoc = "Customer";
        public const string General = "General";
        public const string Payroll = "Payroll";
        public const string BillPayment = "Bill Payment";
        public const string Interco = "Interco";
        public const string OBBill = "OBBill";
        public const string Claim = "Claim";
        public const string CMApplication = "CM Application";
        public const string CNApplication = "CN Application";
        public const string OBInvoice = "OBInvoice";
        public const string Application = "Application";
        public const string Allocation = "Allocation";
        public const string Revaluations = "Revaluation";
    }

    public static class CursorConstants
    {
        public const string Workflow = "Workflow Cursor";
        public const string HRCursor = "HR Cursor";

    }

    public enum CreditNoteApplicationStatus
    {
        Posted = 1,
        Void = 2

    }
    public enum CreditMemoApplicationStatus
    {
        Posted = 1,
        Void = 2
    }
    public enum ClearingApplicationStatus
    {
        Posted = 1,
        Reset = 2
    }
    public enum DoubtfulDebtAllocationStatus
    {
        Posted = 1,
        Reset = 2,
        Tagged = 1
    }
    public static class InvoiceStates
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
        public const string ExcesPaid = "Excess Paid";
        public const string NotApplied = "Not Applied";
        public const string Posted = "Posted";
        public const string Parked = "Parked";
        public const string Deleted = "Deleted";
        public const string Recurring = "Recurring";
    }

    public static class CreditNoteState
    {
        public const string NotApplied = "Not Applied";
        public const string PartialApplied = "Partial Applied";
        public const string FullyApplied = "Fully Applied";
        public const string Void = "Void";
    }

    public static class DoubtfulDebtState
    {
        public const string NotAllocated = "Not Allocated";
        public const string PartialAllocated = "Partial Allocated";
        public const string FullyAllocated = "Fully Allocated";
        public const string Void = "Void";
        public const string Reversed = "Reversed";
    }

    public static class ExtensionType
    {
        public const string General = "General";
        public const string Invoice = "Invoice";
        public const string DebitNote = "DebitNote";
        public const string Receipt = "Receipt";

    }
    public static class DebitNoteStates
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
        public const string ExcesPaid = "Excess Paid";

    }
    public static class BanktransferStatus
    {
        public const string Void = "Void";
        public const string Created = "Created";

    }
    public static class ReceiptState
    {
        public const string Cleared = "Cleared";
        public const string Created = "Created";
        public const string Void = "Void";
        public const string Posted = "Posted";
    }
}
