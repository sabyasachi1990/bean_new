using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Infra.Resources
{
    public static class OpeningBalanceConstants
    {
        public const string IdentityBean = "IdentityBean";
        public const string DocType = "Opening Balance";
        public const string Posted = "Posted";
        public const string DraftType = "Draft";
        public const string Save = "Save";
        public const string Document_number_are_existing_in_Invoice= "Duplicate document number(s) in invoice. Verify that you’ve entered the correct information.";
        public const string Document_number_are_duplicated_in_TR_OR_lines= "Duplicate document number(s) in TR/OR line items. Verify that you’ve entered the correct information.";
        public const string Document_number_are_existing_in_Bill="Document number(s) are existing in Bill";
        public const string Document_number_are_duplicated_in_TP_OP_lines="Document number(s) are duplicated in TP/OP line items";
        public const string Document_number_are_existing_in_CreditNote = "Document number(s) are existing in Credit Note";
        public const string Document_number_are_existing_in_CreditMemo = "Document number(s) are existing in Credit Memo";

        public const string WebJobQueueName = "WebJobQueueNameForOb";
        public const string WebJobStorageAccConnectionString = "QueueConnectionString";

    }
}
