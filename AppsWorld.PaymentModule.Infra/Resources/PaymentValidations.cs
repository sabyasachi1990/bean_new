using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Infra
{
    public static class PaymentValidations
    {
        public const string Payment_Total_Amount_Should_Be_Grater_Than_Zero = "Payment Total Amount Should Be Grater Than Zero";
        public const string BankPaymentAmount_should_be_greater_than_zero = "BankPaymentAmount should be greater than zero";
        public const string Please_enter_the_Balancing_Items_amount = "Please enter the Balancing Items Amount";
        public const string Balancing_Item_amount_should_be_greater_than_zero = "Balancing Item amount should be greater than zero";
        public const string Invalid_Payment = "Invalid Payment";
        public const string Payment_Status_Change = "The outstanding documents has been changed, kindly reload the page.";
    }
}
