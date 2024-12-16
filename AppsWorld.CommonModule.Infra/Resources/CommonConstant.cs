using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Infra
{
    public static class CommonConstant
    {
        public const string The_Financial_setting_should_be_activated = "The Financial settings should be activate";
        public const string Invalid_Document_Date = "Invalid Document Date.";
        public const string Entity_is_mandatory = "Entity is mandatory.";
        public const string Document_number_already_exist = "Document number already exist";
        public const string Segments_duplicates_cannot_be_allowed = "Segments duplicates cannot be allowed";
        public const string Atleast_one_line_Item_is_required = "Atleast one line Item is required";
        public const string Tax_Codes_are_not_selected_in_detail = "Tax Codes are not selected in detail";
        public const string Transaction_date_is_in_closed_financial_period_and_cannot_be_posted = "Transaction date is in closed financial period and cannot be posted.";
        public const string Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted = "Transaction date is in locked accounting period and cannot be posted.";
        public const string Invalid_Financial_Period_Lock_Password = "Invalid Financial Period Lock Password.";
        public const string Reversal_date_is_in_closed_financial_period_and_cannot_be_posted = "Reversal date is in closed financial period and cannot be posted.";
        public const string Reversal_date_is_in_locked_accounting_period_and_cannot_be_posted = "Reversal date is in locked accounting period and cannot be posted.";
        public const string Reverse_Doc_is_locked_Cant_void_posted_document = "Reversed Document is Locked, Can't void posted document";
        public const string ServiceCompany_is_mandatory = "ServiceCompany is mandatory.";
        public const string Grand_Total_should_be_greater_than_zero = "Grand Total should be greater than zero";
        public const string ExchangeRate_Should_Be_Grater_Than_0 = "Exchange Rate Should Be Grater Than '0' ";
        public const string GSTExchangeRate_Should_Be_Grater_Than_0 = "GSTExchangeRate Should Be Grater Than '0' ";
        public const string DocCurrencyAmount_Should_Be_Grater_Than_0 = "Doc amount should be greater than zero.";
        public const string BeanCursor = "Bean";
        public const string DocumentState_has_been_changed_to_void_cannot_save_the_record = "The state of the transaction has been chaged to void, you can't save.";
        public const string DocumentState_has_been_changed_please_kindly_refresh_the_screen = "The state of the transaction has been changed, kindly refresh.";
        public const string Document_has_been_modified_outside = "Document updated successfully. Please refresh to proceed.";
        public const string The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction = "The state of the transaction has been changed by clearing, cann't save.";
        public const string This_transaction_has_already_void = "This transaction has already void.";
        public const string The_State_of_the_service_entity_had_been_changed = "The state of the service entity had been changed";
        public const string Failed_While_inserting_the_record_in_Document_History = "Failed while inserting the record in Document History";
        public const string Outstanding_transactions_list_has_changed_Please_refresh_to_proceed = "Outstanding transactions list has changed.  Please refresh to proceed.";
        public const string Access_denied = "Access denied! Please contact administrator";
    }
    public interface ISvcEntityFilter
    {
        string ServiceCompanyName { get; set; }
    }
    public static class DocumentConstants
    {
        public const string CursorName = "Bean";
        public const string Entities = "Entities";
        public const string Bills = "Bills";
        public const string Journals = "Journals";
    }
}
