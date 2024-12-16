using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Infra.Resources
{
    public static class OpeningBalnceValidations
    {
      //  public const string Payment_Total_Amount_Should_Be_Grater_Than_Zero = "Payment Total Amount Should Be Grater Than Zero";
        public const string accounts_receivables= "trade receivables";
        public const string other_receivables = "other receivables";
        public const string accounts_payable = "trade payables";
        public const string other_payables = "other payables";
        public const string Doc_Currency_Should_Not_Be_Empty = "Doc Currency Should Not Be Empty";
        public const string BaseCredit_And_BaseDebit_are_not_exisit_at_a_time = "BaseCredit And BaseDebit Should Not Be Exisist At a Time";
        public const string DocCredit_And_DocDebit_are_not_exisit_at_a_time = "DocCredit And DocDebitShould Not Be Exisist At a Time";
        public const string BaseCredit_And_BaseDebit_Shoulb_Be_Equal = "BaseCredit And BaseDebit Should Be Equal";
        public const string income_statement = "income statement";

    }
}
