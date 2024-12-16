using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Infra.Resources
{
    public class CreditNoteValidation
    {
        public const string The_Financial_setting_should_be_activated = "The Financial settings should be activate";
        public const string Entity_is_mandatory="Entity is mandatory";
        public const string Invalid_Document_Date="Invalid Document Date";
        public const string Invalid_Due_Date="Invalid Due Date";
        public const string Grand_Total_Should_Be_Grater_Than_Zero="Grand Total Should Be Grater Than Zero";
        public const string Grand_Total_Should_Be_Greater_Than_Or_Equal_To_Balance_Amount="Grand Total Should Be Greater Than Or Equal To Balance Amount";
        public const string Credit_Note_Amount_should_be_less_than_or_equal_to_Remaining_Amount="Credit Note Amount should be less than or equal to Remaining Amount";
        public const string Credit_Note_amount_should_be_less_than_or_equal_to_Invoice_Balance_Amount="Credit Note amount should be less than or equal to Invoice Balance Amount";
        public const string Terms_of_Payment_is_mandatory="Terms of Payment is mandatory";
        public const string Document_number_already_exist="Document number already exist";
        public const string Service_Company_Is_Mandatory="Service Company Is Mandatory";
        public const string Grand_Total_should_be_greater_than_zero="Grand Total should be greater than Zero";
        public const string Atleast_one_Sale_Item_is_required="Atleast one Sale Item is required";
        public const string ExchangeRate_Should_Be_Grater_Than_Zero = "ExchangeRate Should Be Grater Than Zero";
        public const string GSTExchangeRate_Should_Be_Grater_Than_Zero="GSTExchangeRate Should Be Grater Than '0' ";
        public const string Items_are_duplicated_in_the_Details="Items are duplicated in the Details";
        public const string Tax_Codes_are_not_selected_in_detail="Tax Codes are not selected in detail";
        public const string Transaction_date_is_in_closed_financial_period_and_cannot_be_posted="Transaction date is in closed financial period and cannot be posted.";
        public const string Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted="Transaction date is in locked accounting period and cannot be posted";
        public const string Invalid_Financial_Period_Lock_Password="Invalid Financial Period Lock Password";
        public const string Credit_Amount_should_be_less_than_or_equal_to_Remaining_Amount="Credit Amount should be less than or equal to Remaining Amount";
        public const string Credit_amount_should_be_less_than_or_equal_to_Balance_Amount="Credit amount should be less than or equal to Balance Amount";
        public const string Invalid_CreditNote="Invalid CreditNote";
        public const string Invalid_Credit_Note_Application="Invalid Credit Note Application";
        public const string Reset_Date_should_be_greater_than_Allocation_date="Reset Date should be greater than Application date";
        public const string DocumentState_has_been_changed_please_kindly_refresh_the_screen = "DocumentState has been changed please kindly refresh the screen";
        public const string CN_application_status_Change = "The outstanding documents has been changed, kindly refresh .";
    }
}
