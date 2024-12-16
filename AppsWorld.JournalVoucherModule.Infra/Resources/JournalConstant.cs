using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Infra
{
    public static class JournalConstant
    {
        public const string DocumentState_should_be_parked_only = "DocumentState should be 'parked' only";
        public const string Invalid_journal = "Invalid journal";
        public const string State_Should_Be_posted = "State Should Be posted";
        public const string DocumentState_should_be_posted_only = "DocumentState should be 'posted' only";
        public const string DocumentState_should_be_Posted_or_Reversed_only = "DocumentState should be 'Posted or Reversed' only";
        public const string Reversal_date_must_be_greater_than_Doc_date = "Reversal date must be greater than Doc date";
        public const string Total_debit_must_equal_to_total_credit = "Total debit must equal to total credit";
        public const string Delete_aplicable_void_records_only="Delete aplicable 'void' records only";
        //public const string Cannot_give_EndDate_less_than_Last_Posted_Date = "FrequencyEndDate shouldn't be less than Last Posted Date";
        public const string Cannot_give_EndDate_less_than_Last_Posted_Date = "End Date cannot be earlier than the last posted/parked Doc Date";
        public const string JournalApplicationService = "JournalApplicationService";
        public const string Journal_update_posting = "Entered into UpdatePosting Journal internal update enpoint";
        public const string Journal_update_posting_save = "Executed UpdatePosting Journal internal update enpoint";
        public const string NonCurrent = "Non-current";
        public const string Parked = "Parked";
    }
}
