using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Infra
{
    public static class CreditMemoValidations
    {
        public const string The_Financial_setting_should_be_activated = "The Financial setting should be activated";
        public const string Entity_is_mandatory = "Entity is mandatory";
        public const string Invalid_Document_Date = "Invali Document Date";
        public const string Invalid_Due_Date = "Invalid Due Date";
        public const string Grand_Total_Should_Be_Grater_Than_Zero = "Grand total should be greater than zero.";
        public const string Grand_Total_Should_Be_Greater_Than_Or_Equal_To_Balance_Amount = "Grand Total Should Be Greater Than Or Equal To Balance Amount";
        public const string Credit_Note_Amount_should_be_less_than_or_equal_to_Remaining_Amount = "Credit Note Amount should be less than or equal to Remaining Amount";
        public const string Terms_of_Payment_is_mandatory = "Terms of Payment is mandatory";
        public const string Document_number_already_exist = "Document number already exist";
        public const string Service_Company_Is_Mandatory = "Service Company is Mandatory";
        public const string Grand_Total_should_be_greater_than_zero = "Grand Total should be greater than zero";
        public const string Atleast_one_Sale_Item_is_required = "Atleast_one_Sale_Item is required";
        public const string ExchangeRate_Should_Be_Grater_Than_Zero = "ExchangeRate Should Be Grater Than Zero";
        public const string GSTExchangeRate_Should_Be_Grater_Than_Zero = "GSTExchangeRate Should Be Grater Than Zero";
        public const string Transaction_date_is_in_closed_financial_period_and_cannot_be_posted = "Transaction date is in closed financial period and cannot be posted";
        public const string Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted = "Transaction date is in locked accounting period and cannot be posted";
        public const string Invalid_Financial_Period_Lock_Password = "Invalid Financial Period Lock Password";
        public const string Credit_amount_should_be_less_than_or_equal_to_Balance_Amount = "Credit_amount_should_be_less_than_or_equal_to_Balance_Amount";
        public const string Invalid_CreditMemo = "Invalid CreditMemo";
        public const string Invalid_Application_Date = "Invalid Application Date";
        public const string Atleast_one_Application_is_required = "Atleast one Application is required";
        public const string Total_Amount_To_Credit_should_be_greater_than_Zero = "Total applied amount should be greater than zero.";
        public const string Atleast_one_Application_should_be_given = "Atleast one Application should be given";
        public const string Duplicate_documents_in_details = "Duplicate documents in details";
        public const string Credit_Amount_should_be_less_than_or_equal_to_Remaining_Amount = "Credit Amount should be less than or equal to Remaining Amount";
        public const string Invalid_Bill_to_Update_Balance_Amount = "Invalid Bill to Update Balance Amount";
        public const string Invalid_Credit_Memo_Application = "Invalid Credit Memo Application";
        public const string Reset_Date_should_be_greater_than_Allocation_date = "Reset Date should be greater than Allocation date";
        public const string credit_memo_amount = "Credit memo amount shouldn't be greater than outstanding balance of bill";
        public const string DocumentState_has_been_changed_please_kindly_refresh_the_screen = "DocumentState has been changed please kindly refresh the screen";
        public const string CM_application_status_Change = "The outstanding documents has been changed, kindly refresh to proceed.";
    }
}

