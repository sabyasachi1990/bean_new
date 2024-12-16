using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.Infra
{
    public static class ClearingValidations
    {
        public const string Balance_Sheet = "Balance Sheet";
        public const string The_Financial_setting_should_be_activated = "The Financial setting should be activated";
        public const string Invalid_Clearing = "Invalid Clearing";
        public const string Transaction_date_is_in_closed_financial_period_and_cannot_be_posted = "Transaction date is in closed financial period and cannot be posted";
        public const string Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted = "Transaction date is in locked accounting period and cannot be posted";
        public const string Invalid_Financial_Period_Lock_Password = "Invalid_Financial_Period_Lock_Password";
        public const string Reset_Date_should_be_greater_than_Allocation_date = "Reset Date should be greater than Allocation date";


        public const string Posting_date_is_in_closed_financial_period_and_cannot_be_posted = "Posting date is in closed financial period and cannot be posted.";
        public const string Posting_date_is_in_locked_accounting_period_and_cannot_be_posted = "Posting date is in locked accounting period and cannot be posted.";
        public const string Invalid_Due_Date = "Invalid Due Date.";
        public const string Terms_Payment_is_mandatory = "Terms Payment is mandatory.";
        public const string Accounts_are_duplicated_in_the_Details = "Accounts are duplicated in the Details.";
        public const string Segments_duplicates_cannot_be_allowed = "Segments duplicates cannot be allowed";
        public const string Atleast_one_Sale_Item_is_required_in_the_Bill = "Atleast one line item is required.";
        public const string Please_enter_the_valid_Base_Currency = "Please enter the valid Base Currency.";
        public const string Payroll_Bill = "Payroll Bill";
        public const string Bill = "Bill";
        public const string Payment = "Payment";
        public const string Payroll_Payment = "Payroll Payment";

    }
}
