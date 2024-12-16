using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Infra
{
    public static class WithdrawalValidation
    {
        public const string Doc_Curency_Amount_Should_Be_Grater_Than_0= "Total should be greater than zero.";
        public const string Atleast_one_Chart_of_Account_is_required_in_the_Withdrawal="Atleast one Chart of Account is required in the Withdrawal.";
        public const string Chart_of_Accounts_are_duplicated_in_the_Details="Chart of Accounts are duplicated in the Details";
        public const string Invalid_Withdrawal="Invalid Withdrawal";
        public const string Total_Document_Currency_must_be_eqals_to_Withdrawal_Amount="Total Document Currency must be eqals to Withdrawal Amount";
        public const string Bank_ClearingDate_activated = "will not Void BankClearingDate has been activated";
        
    }
}
