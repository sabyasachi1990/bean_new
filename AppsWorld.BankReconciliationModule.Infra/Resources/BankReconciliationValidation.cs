using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Infra
{
    public static class BankReconciliationValidation
    {
        public static string No_Active_Finance_Setting_found = "No Active Finance Setting found";
        public static string From_Date_And_To_Date_Must_be_in_Financial_Year_End = "From Date And To Date Must be in Financial Year End";
        public const string The_Financial_setting_should_be_activated = "The Financial setting should be activated";
        public static string payments = "Bill Payment";
        public static string receipt = "Receipt";
        public static string deposit = "Deposit";
        public static string Bank_Transfer = "Transfer"; 
        public static string BankReconciliation_Void = "Void";
        public static string Withdrawal = "Withdrawal";
        public static string Bill = "Bill";
        public static string CreditNote = "Credit Note";
        public static string Invoice = "Invoice";
        public static string DebitNote = "Debit Note";
        public static string DoubtfulDebt = "Debt Provision";
        public static string CashSale = "Cash Sale";
        public static string Journal = "Journal";
        public static string CashPayments = "Cash Payment";
        public static string CreditMemo = "Credit Memo";
        public static string Payroll_Payment = "Payroll Payment";
        public static string Payroll_Bill = "Payroll Bill";
    }
}
