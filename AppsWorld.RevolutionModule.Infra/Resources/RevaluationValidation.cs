using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Infra
{
    public static class RevaluationValidation
    {
        public const string Revaluation_records_are_not_found = "Revaluation records are not found";
        public const string Exchange_rates_are_not_found = "Exchange rates are not found";
        public const string Revaluation_can_run_once_per_same_date = "Revaluation has been posted before on this date,please choose another date to run the revaluation";
        public const string Date_should_be_in_accounting_period_to_run_the_revaluation = "Date should be in accounting period to run the revaluation";
        public const string Please_select_only_one_service_entity_to_proceed_save = "Please select only one service entity to proceed save";
        public const string Revaluation_cant_run_for_future_date = "Revaluation cann't run on future date";
        public const string Revaluation_date_is_in_locked_accounting_period_and_cannot_be_posted = "Revaluation date is in locked accounting period and cannot be posted.";
    }
}
