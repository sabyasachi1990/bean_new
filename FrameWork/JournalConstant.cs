using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork
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
    }
}
