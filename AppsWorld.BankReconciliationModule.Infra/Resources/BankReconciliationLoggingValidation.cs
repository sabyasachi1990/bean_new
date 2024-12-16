using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Infra
{
    public static class BankReconciliationLoggingValidation
    {
        public static string Entred_Into_SaveBankReconciliation_Method = "Entred Into SaveBankReconciliation Method";
        public static string Checking_errors = "Checking errors";
        public static string Entered_Into_New_Block = "Entered Into New Block";
        public static string Entered_Into_Details_Block = "Entered Into New Details Block";
        public static string Entered_Into_Updated = "Entered Into Update Block";
        public static string Entered_Into_Update_Details_Block = "Entered Into Update Details Block";
        public static string Data_Saved_Successfully_And_EventRised = "Data Saved Successfully And EventRised";
        public static string Entered_Into_Catch_Block = "Entered Into Catch Block";
        public static string Cash_and_bank_balances="Cash and bank balances";

        public const string Enter_into_GetClearingTransaction_method = "Enter into GetClearingTransaction method";
        public const string Come_out_from_GetClearingTransaction_method = "Come out from GetClearingTransaction method";
        public const string Enter_into_the_Create_call_of_BankReconciliation = "Enter into the Create call of BankReconciliation";
        public const string Asign_the_value_to_BankReconciliationDetails = "Asign the value to BankReconciliationDetails";
        public const string Come_out_from_CreateBankReconciliation_method = "Come out from CreateBankReconciliation method";
        public const string Enter_into_GetClearingRec_method = "Enter into GetClearingRec method";
        public const string Come_out_from_GetClearingRec_metod = "Come out from GetClearingRec metod";

        public const string Entity_Error= "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:";
        public static string _Property="- Property: \"{0}\", Error: \"{1}\"";
    }
}
