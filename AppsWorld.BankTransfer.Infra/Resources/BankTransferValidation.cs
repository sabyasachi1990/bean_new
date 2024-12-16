using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Infra.Resources
{
    public class BankTransferValidation
    {
        public const string The_Financial_setting_should_be_activated = "The Financial settings should be activate";
        public const string Invalid_BankTransfer = "Invalid BankTransfer";
        public const string Invalid_Transfer_Date = "Invalid Transfer Date";
        public const string ExchangeRate_Should_Be_Grater_Than_zero = "Exchange Rate Should Be Grater Than zero";
        public const string Transaction_date_is_in_closed_financial_period_and_cannot_be_posted = "Transaction date is in closed financial period and cannot be posted";
        public const string Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted = "Transaction date is in locked accounting period and cannot be posted";
        public const string Invalid_Financial_Period_Lock_Password = "Invalid Financial Period Lock Password";
        public const string BaseCurrency_should_match_Any_One_of_the_Withdrawal_Or_Deposit_Currency = "Base currency should match with any one of the withdrawal or deposit";
        public const string Do_Not_allow_Same_company_and_Same_COAAccounts = "Do Not allow Same company and Same COA Accounts";
        public const string Amount_Must_Be_Grater_than_Zero = "Amount should be greater than zero.";
       
    }
}
