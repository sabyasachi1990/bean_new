using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Infra.Resources
{
    public class DoubtfulDebitValidation
    {
        public const string The_Financial_setting_should_be_activated = "The Financial settings should be activate";
        public const string Entity_is_mandatory = "Entity is mandatory";
        public const string Grand_Total_Should_Be_Grater_Than_Zero = "Grand Total Should Be Grater Than Zero";
        public const string Grand_Total_Should_Be_Less_Than_Or_Equal_To_Balance_Amount = "Grand Total Should Be Less Than Or Equal To Balance Amount";
        public const string DoubtfulDebt_Amount_should_be_less_than_or_equal_to_Remaining_Amount = "DoubtfulDebt Amount should be less than or equal to Remaining Amount";
        public const string DoubtfulDebt_amount_should_be_less_than_or_equal_to_Invoice_Balance_Amount = "DoubtfulDebt amount should be less than or equal to Invoice Balance Amount";
        public const string Invalid_Document_Date = "Invalid Document Date";
        public const string Service_Company_is_mandatory = "Service Company is mandatory";
        public const string Amount_should_be_greater_than_zero = "Amount should be greater than zero";
        public const string ExchangeRate_Should_Be_Grater_Than_Zero = "ExchangeRate Should Be Grater Than '0' ";
        public const string Document_number_already_exist = "Document number already exist";
        public const string Transaction_date_is_in_closed_financial_period_and_cannot_be_posted = "Transaction date is in closed financial period and cannot be posted.";
        public const string Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted = "Transaction date is in locked accounting period and cannot be posted";
        public const string Invalid_Financial_Period_Lock_Password = "Invalid Financial Period Lock Password.";
        public const string Invalid_Doubtful_Debit = "Invalid Doubtful Debit";
        public const string It_Is_Already_In_Reset_State = "It Is Already In Reset State";
        public const string Reset_Date_should_be_greater_than_Allocation_date = "Reset Date should be greater than Allocation date";
        public const string Invalid_Application_Date = "Invalid Application Date";
        public const string Atleast_one_Application_is_required = "Atleast one Application is required";
        public const string Total_Amount_To_Credit_should_be_greater_than_Zero = "Total Amount To Credit  should be greater than 0.";
        public const string Atleast_one_Application_should_be_given = "Atleast one Application should be given";
        public const string Duplicate_documents_in_details = "Duplicate documents in details";
        public const string Invalid_Debit_Note_to_Update_Balance_Amount = "Invalid Debit Note to Update Balance Amount";
        public const string Invalid_Invoice_to_Update_Balance_Amount = "Invalid Invoice to Update Balance Amount";
        public const string Invalid_Allocation_Date = "Invalid Allocation Date";
        public const string Atleast_one_Allocation_is_required = "Atleast one Allocation is required.";
        public const string Atleast_one_allocation_should_be_given = "Atleast one allocation should be given";
        public const string Invalid_Debit_Note_to_Update_Balance_Amount_ = "Invalid Debit Note to Update Balance Amount";
        public const string Amount_to_Allocate_cannot_be_greater_than_Amount_Due = "Amount to Allocate cannot be greater than 'Amount Due'";
        public const string You_cannt_run_I_C_transaction_rather_I_C_is_not_activate_in_this_in_this_company = "You cann't run I/C transaction, rather I/C is not activate in this in this company";
        public const string DocumentState_has_been_changed_please_kindly_refresh_the_screen = "DocumentState has been changed please kindly refresh the screen";
    }
}
