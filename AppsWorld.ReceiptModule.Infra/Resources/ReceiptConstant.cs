using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Infra
{
    public static class ReceiptConstant
    {
        public const string Receipt_Total_Amount_Should_Be_Grater_Than_Zero="Receipt Total Amount Should Be Grater Than Zero";
        
        public const string Please_create_atleast_one_balancing_item="Please create atleast one balancing item";
        public const string BankReceiptAmount_should_be_greater_than_zero="BankReceiptAmount should be greater than zero";
        public const string Please_enter_the_Balancing_Items_amount="Please enter the Balancing Items amount";
        public const string Balancing_Item_amount_should_be_greater_than_zero="Balancing Item amount should be greater than zero";      
        public const string Invalid_Receipt="Invalid Receipt";
        public const string Clearing_Receipts = "Clearing - Receipts";
        public const string Receipt_Status_Change = "The outstanding documents has been changed, kindly reload the page.";
        public const string NA = "NA";
    }
}
