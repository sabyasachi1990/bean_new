using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Infra
{
    public static class OpeningBalanceLoggingValidation
    {
        public const string Entred_Into_GetOpeningBalance_Method="Entred Into GetOpeningBalance Method";
        public const string Entred_Into_GetByCurrencies_Method = "Entred Into GetByCurrencies Method";
        public const string Leave_From_GetOpeningBalance_Method = "Leave From GetOpeningBalance Method";
        public const string OpeningBalancesApplicationService = "OpeningBalancesApplicationService";
        public const string OpeningBalancesController = "OpeningBalancesController";
        public const string Opening_Balance_web_job_calling = "calling the Opening Balance web job.";
        public const string Opening_Balance_web_job_calling_completed = "calling the Opening Balance web job completed.";

        public const string Entred_Into_Get_Service_Company_OpeningBalance_Method = "Entred Into Get Service Company OpeningBalance Method";
        public const string Checking_financials_Is_Not_Equal_To_Null = "Checking Financials Is Not Equal To Null";
        public const string OpeningBalance_Is_Not_Equal_To_Null = "Checking OpeningBalance Is Not Equal To Null";
        public const string Entered_Into_FillOpeningBalance_Method = "Entered Into FillOpeningBalance Method";
        public const string Leave_From_openingBalance_Not_Equal_To_Null_Method = "Leave From openingBalance Not Equal To Null Method";
        public const string Entered_Into_Else_OpeningBalance_Method = "Entered Into Else OpeningBalance Method";
        public const string Leave_From_Get_ServiceCompany_OpeningBalance_Method = "Leave From Get ServiceCompany OpeningBalance Method";

        public const string Entered_Into_Get_LineItemsForCOA_Method = "Entered Into Get LineItemsForCOA Method";
        public const string Leave_From_Get_LineItemsForCOA_Method = "Leave From Get LineItemsForCOA Method";

        public const string Entred_Into_SaveOpeningBalance_Method = "Entred Into SaveOpeningBalance Method";
        public const string Checking_errors = "Checking Errors";
        public const string Doc_Currency_Should_Not_Be_Empty = "Doc Currency Should Not Be Empty";
        public const string BaseCredit_And_BaseDebit_are_not_exisit_at_a_time = "BaseCredit And BaseDebit Should Not Be Exisist At a Time";
        public const string DocCredit_And_DocDebit_are_not_exisit_at_a_time = "DocCredit And DocDebitShould Not Be Exisist At a Time";
        public const string BaseCredit_And_BaseDebit_Shoulb_Be_Equal = "BaseCredit And BaseDebit Shoulb Be Equal";
        public const string Checking_openingBalance_Is_Not_Equal_To_Null = "Checking OpeningBalance Is Not Equal To Null";
        public const string Entered_Into_OBDetail_Line_Item_Block = "Entered Into OBDetail Line Item Block";
        public const string Entered_Into_openingBalance_Else_Block = "Entered Into OpeningBalance Else Block";
        public const string Entered_Into_EventStore_Method="Entered Into EventStore Method";
        public const string Entered_Into_Catch_Block = "Entered Into Catch Block";
        public const string Opening_Balance_Date_cannot_be_less_than_Opening_Balance_Line_Item_Date="Opening Balance Date cannot be less than Opening Balance Line Item Date";



    }
}
