using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Infra
{
    public static class DebitNoteValidation
    {
        public const string Invalid_DebitNote = "Invalid DebitNote";
        public const string Invalid_Due_Date = "Invalid Due Date.";
        public const string Debit_Amount_Should_Be_Grater_Than_0 = "Debit Amount Should Be Grater Than 0.";
        public const string Terms_of_Payment_Is_Mandatory = "Terms of Payment Is Mandatory";
        public const string Atleast_one_Chart_of_Account_is_required_in_the_Debit_Note = "Atleast one Chart of Account is required in the Debit Note.";
        public const string Chart_of_Accounts_are_duplicated_in_the_Details = "Chart of Accounts are duplicated in the Details.";
        public const string Tax_Codes_are_not_selected_in_detail = "Tax Codes are not selected in detail";
        public const string DebiteNote_Note_is_already_exist = "DebiteNote Note is already exist";
        public const string The_Financial_setting_should_be_activated = "The Financial settings should be activate";
        public const string No_active_financial_settings_found = "No active financial settings found";
        public const string DocumentState_has_been_changed_please_kindly_refresh_the_screen = "DocumentState has been changed please kindly refresh the screen";
    }
}
