using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork
{
    public static class BillConstant
    {
        public const string Invalid_BillNote="Invalid BillNote";
        public const string Posting_date_is_in_closed_financial_period_and_cannot_be_posted="Posting date is in closed financial period and cannot be posted.";
        public const string Posting_date_is_in_locked_accounting_period_and_cannot_be_posted="Posting date is in locked accounting period and cannot be posted.";
        public const string Invalid_Due_Date="Invalid Due Date.";
        public const string Terms_Payment_is_mandatory="Terms Payment is mandatory.";
        public const string Accounts_are_duplicated_in_the_Details="Accounts are duplicated in the Details.";
        public const string Segments_duplicates_cannot_be_allowed="Segments duplicates cannot be allowed";
        public const string Atleast_one_Sale_Item_is_required_in_the_Bill="Atleast one Sale Item is required in the Bill.";
    }
}
