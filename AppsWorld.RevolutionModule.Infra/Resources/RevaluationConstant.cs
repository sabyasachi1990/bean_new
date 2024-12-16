using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Infra
{
    public static class RevaluationConstant
    {
        public const string Multi_Currency_should_be_activate = "Multi currency should be activate";
        public const string The_Financial_setting_should_be_activated = "The Financial setting should be activated";
        public const string IdentityBean = "IdentityBean";
        public const string Posted = "Posted";
        public const string Void = "Void";
        public const string Transaction_date_is_in_closed_financial_period_and_cannot_be_posted = "Transaction date is in closed financial period and cannot be posted.";
        public const string Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted = "Transaction date is in locked accounting period and cannot be posted.";
        public const string Invalid_Financial_Period_Lock_Password = "Invalid Financial Period Lock Password.";
        public const string Revaluation_already_void = "Revaluation already void";
        public const string Document_has_been_modified_outside = "The document has been modified outside, kindly refresh.";
    }
}
