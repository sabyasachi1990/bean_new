using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Infra 
{
    public static class CashSaleValidation
    {
        public const string Entity_is_mandatory = "Entity is mandatory";
        public const string Invalid_Document_Date = "Invalid document date";
        public const string Service_Company_Is_Mandatory = "Service company is mandatory";
        public const string Grand_total_should_be_greater_than_zero = "Grand total should be greater than zero";
        public const string Pleas_Enter_Quantity="Please Enter Quantity";
        public const string Please_Select_Item="Please Select Item";
        public const string Segments_duplicates_cannot_be_allowed="Segments duplicates cannot be allowed";
        public const string Atleast_one_Sale_Item_is_required="Atleast one Sale Item is required";
        public const string Atleast_one_Sale_Item_is_requireds = "Atleast one Sale Item is required";
       // public const string Repeating_Invoice_fields_should_be_entered = "Repeating Invoice fields should be entered";
        public const string ExchangeRate_Should_Be_Grater_Than_zero="Exchange rate should be greater than zero";
        public const string GSTExchangeRate_Should_Be_Grater_Than_zero = "GST exchange rate should be greater than zero";
        public const string Items_are_duplicated_in_the_Details="Items are duplicated in the Details";
        public const string Tax_Codes_are_not_selected_in_detail="Tax codes are not selected in detail";
        public const string Transaction_date_is_in_closed_financial_period_and_cannot_be_posted="Transaction date is in closed financial period and cannot be posted.";
        public const string Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted="Transaction date is in locked accounting period and cannot be posted.";
        public const string Invalid_Financial_Period_Lock_Password="Invalid financial period lock password";
        public const string The_Financial_setting_should_be_activated = "The financial settings should be activate";
        public const string Invalid_CashSale="Invalid cash sale";
        public const string No_Active_Finance_Setting_found = "No Active Finance Setting found";

    }
}
