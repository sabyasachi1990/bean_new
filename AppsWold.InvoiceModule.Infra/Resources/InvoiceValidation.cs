using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Infra
{
    public class InvoiceValidation
    {
        public const string The_Financial_setting_should_be_activated = "The Financial settings should be activate";
        public const string Entity_is_mandatory = "Entity is mandatory";
        public const string Invalid_Document_Date = "Invalid Document Date";
        public const string Service_Company_Is_Mandatory = "Service Company Is Mandatory";
        public const string Invalid_Due_Date = "Invalid Due Date";
        public const string Terms_Payment_is_mandatory = "Terms Payment is mandatory";
        public const string Document_number_already_exist = "Document number already exist";
        public const string Grand_Total_should_be_greater_than_zero = "Grand Total should be greater than zero";
        public const string Please_Enter_Quantity = "Please Enter Quantity";
        public const string Please_Select_Item = "Please Select Item";
        public const string Terms_of_Payment_is_mandatory = "Terms of Payment is mandatory";
        public const string Segments_duplicates_cannot_be_allowed = "Segments duplicates cannot be allowed";
        public const string Atleast_one_Sale_Item_is_required_in_the_Invoice = "Atleast one Sale Item is required in the Invoice";
        public const string Repeating_Invoice_fields_should_be_entered = "Repeating Invoice fields should be entered";
        public const string ExchangeRate_Should_Be_Grater_Than_Zero = "ExchangeRate Should Be Grater Than '0' ";
        public const string GSTExchangeRate_Should_Be_Grater_Than_Zero = "GSTExchangeRate Should Be Grater Than '0' ";
        public const string Items_are_duplicated_in_the_Details = "Items are duplicated in the Details";
        public const string Tax_Codes_are_not_selected_in_detail = "Tax Codes are not selected in detail";
        public const string Transaction_date_is_in_closed_financial_period_and_cannot_be_posted = "Transaction date is in closed financial period and cannot be posted";
        public const string Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted = "Transaction date is in locked accounting period and cannot be posted";
        public const string Invalid_Financial_Period_Lock_Password = "Invalid Financial Period Lock Password";
        public const string Invalid_Invoice = "Invalid Invoice";
        public const string Invoice_Note_is_already_exist = "Invoice Note is already exist";
        //public const string Cannot_give_EndDate_less_than_Last_Posted_Date = "FrequencyEndDate shouldn't be less than Last Posted Date";
        public const string Cannot_give_EndDate_less_than_Last_Posted_Date = "End Date cannot be earlier than the last posted/parked Doc Date";
        public const string Invoice_Number_already_exists = "Invoice Number already exists";
        public const string No_Active_Finance_Setting_found = "No Active Finance Setting found";
        public const string State_should_be = "State should be ";
        public const string Tax_rate_missing_for_required_line_items = "Tax rate missing for required line items";
        public const string DocumentState_has_been_changed_please_kindly_refresh_the_screen = "DocumentState has been changed, please kindly refresh the screen!";
        public const string DocumentState_has_been_changed_to_void_cannot_save_the_record = "DocumentState hase been chaged to Void, you can't save this record!";
    }
}
