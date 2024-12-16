using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Infra
{
    public static class PaymentLoggingValidation
    {
        public const string Entred_Into_SavePayment_Method="Entred Into SavePayment Method";
        public const string Checking_Payment_Is_Not_Equal_To_Null="Checking Payment Is Not Equal To Null";
        public const string Entred_Into_InsertPayment_Method_Of_Payment = "Entred Into InsertPayment Method Of Payment";
        public const string Come_Out_From_InsertPayment_Of_Payment="Come Out From InsertPayment Of Payment";
        public const string Enterd_Into_UpdatePaymentDetails_Of_Payment="Enterd Into UpdatePaymentDetails Of Payment";
        public const string Come_Out_From_UpdatePaymentDetails_Of_Payment="Come Out From UpdatePaymentDetails Of Payment";
        public const string Entered_Into_GenerateAutoNumberForType_Of_Payment="Entered Into GenerateAutoNumberForType Of Payment";
        public const string Come_Out_From_GenerateAutoNumberForType_Of_Payment_Method="Come Out From GenerateAutoNumberForType of Payment Method";
        public const string Entered_Into_SavePaymentDocumentVoid_Of_payment_Method="Entered Into SavePaymentDocumentVoid Of payment Method";
        public const string Come_Out_From_SavePaymentDocumentVoid_Of_Payment="Come Out From SavePaymentDocumentVoid Of Payment";
        public const string Entred_Into_Else_And_check_PaymentDetail = "Entred Into Else And check PaymentDetail";
        public const string Entred_into_deleteJVPostInvoce_Method = "Entred into deleteJVPostInvoce Method";
        public const string Existed_from_deleteJVPostInvoce_Method = "Existed from deleteJVPostInvoce Method";
        public const string Entred_into_UpdatePosting_Method = "Entred into UpdatePosting Method";
        public const string Existed_from_UpdatePosting_Method = "Existed from UpdatePosting Method";
        public const string Entred_into_PaymentJVPostVoid_Method = "Entred into PaymentJVPostVoid Method";
        public const string Existed_from_PaymentJVPostVoid_Method = "Existed from PaymentJVPostVoid Method";
        public const string Issues_While_inserting_the_record_in_document_history = "Issues While inserting the record in document history";
        public const string Entering_Into_Saving_DocumentHistory_Block = "Entering into Saving DocumentHistory Block";
        public const string Sucessfully_inserted_the_documents_in_DocumentHistory = "Sucessfully inserted the documents in DocumentHistory";
        public const string Sucessfully_Updated_the_documents_in_DocumentHistory_if_DocDate_is_changed_in_EditMode = "Sucessfully Updated the documents in DocumentHistory if DocDate is changed in Edit Mode";
    }
}
