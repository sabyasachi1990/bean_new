using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Infra.Resources
{
   public static class CreditNoteLoggingValidation
    {
       public const string Log_CreateCreditNote_CreateCall_Request_Message = "Log_CreateCreditNote_CreateCall_Request_Message";
       public const string Log_CreateCreditNote_CreateCall_SuccessFully_Message = "Log_CreateCreditNote_CreateCall_SuccessFully_Message";
       public const string Log_CreateCreditNote_CreateCall_Exception_Message = "Log_CreateCreditNote_CreateCall_Exception_Message";
       public const string Log_FillCreditNote_FillCall_Request_Message = "Log_FillCreditNote_FillCall_Request_Message";
       public const string Log_FillCreditNote_FillCall_SuccessFully_Message = "Log_FillCreditNote_FillCall_SuccessFully_Message";
       public const string Log_FillCreditNote_FillCall_Exception_Message = "Log_FillCreditNote_FillCall_Exception_Message";
       public const string Log_FillCreditNoteApplicationModel_FillCall_Request_Message = "Log_FillCreditNoteApplicationModel_FillCall_Request_Message";
       public const string Log_FillCreditNoteApplicationModel_FillCall_SuccessFully_Message = "Log_FillCreditNoteApplicationModel_FillCall_SuccessFully_Message";
       public const string Log_FillCreditNoteApplicationModel_FillCall_Exception_Message = "Log_FillCreditNoteApplicationModel_FillCall_Exception_Message";
       public const string Log_GetAllCreditNoteLUs_LookupCall_Request_Message = "Log_GetAllCreditNoteLUs_LookupCall_Request_Message";
       public const string Log_GetAllCreditNoteLUs_LookupCall_SuccessFully_Message = "Log_GetAllCreditNoteLUs_LookupCall_SuccessFully_Message";
       public const string Log_GetAllCreditNoteLUs_LookupCall_Exception_Message = "Log_GetAllCreditNoteLUs_LookupCall_Exception_Message";
       public const string Log_SaveCreditNote_SaveCall_UpdateRequest_Message = "Log_SaveCreditNote_SaveCall_UpdateRequest_Message";
       public const string Log_SaveCreditNote_SaveCall_NewRequest_Message = "Log_SaveCreditNote_SaveCall_NewRequest_Message";
       public const string Log_SaveCreditNote_SaveCall_SuccessFully_Message = "Log_SaveCreditNote_SaveCall_SuccessFully_Message";
       public const string Log_SaveCreditNote_SaveCall_Exception_Message = "Log_SaveCreditNote_SaveCall_Exception_Message";
       public const string Log_UpdateCreditNoteDetails_Update_Request_Message = "Log_UpdateCreditNoteDetails_Update_Request_Message";
       public const string Log_UpdateCreditNoteDetails_Update_SuccessFully_Message = "Log_UpdateCreditNoteDetails_Update_SuccessFully_Message";
       public const string Log_UpdateCreditNoteDetails_Update_Exception_Message = "Log_UpdateCreditNoteDetails_Update_Exception_Message";
       public const string Log_InsertCreditNote_FillCall_Request_Message = "Log_InsertCreditNote_FillCall_Request_Message";
       public const string Log_InsertCreditNote_FillCall_SuccessFully_Message = "Log_InsertCreditNote_FillCall_SuccessFully_Message";
       public const string Log_InsertCreditNote_FillCall_Exception_Message = "Log_InsertCreditNote_FillCall_Exception_Message";
       public const string Log_SaveCreditNoteApplication_SaveCall_NewRequest_Message = "Log_SaveCreditNoteApplication_SaveCall_NewRequest_Message";
       public const string Log_UpdateCreditNoteGSTDetails_Update_Request_Message = "Log_UpdateCreditNoteGSTDetails_Update_Request_Message";
       public const string Log_UpdateCreditNoteGSTDetails_Update_SuccessFully_Message = "Log_UpdateCreditNoteGSTDetails_Update_SuccessFully_Message";
       public const string Log_UpdateCreditNoteGSTDetails_Update_Exception_Message = "Log_UpdateCreditNoteGSTDetails_Update_Exception_Message";
       public const string Log_SaveCreditNoteApplication_SaveCall_UpdateRequest_Message = "Log_SaveCreditNoteApplication_SaveCall_UpdateRequest_Message";
       public const string Log_SaveCreditNoteApplication_SaveCall_SuccessFully_Message = "Log_SaveCreditNoteApplication_SaveCall_SuccessFully_Message";
       public const string Log_SaveCreditNoteApplication_SaveCall_Exception_Message = "Log_SaveCreditNoteApplication_SaveCall_Exception_Message";
       public const string Log_UpdateCreditNoteApplicationDetails_Update_Request_Message = "Log_UpdateCreditNoteApplicationDetails_Update_Request_Message";
       public const string Log_GetNextApplicationNumber_GetCall_Request_Message = "Log_GetNextApplicationNumber_GetCall_Request_Message";
       public const string Log_GetNextApplicationNumber_GetCall_SuccessFully_Message = "Log_GetNextApplicationNumber_GetCall_SuccessFully_Message";
       public const string Log_GetNextApplicationNumber_GetCall_Exception_Message = "Log_GetNextApplicationNumber_GetCall_Exception_Message";
       public const string Log_UpdateCreditNoteApplicationDetails_Update_SuccessFully_Message = "Log_UpdateCreditNoteApplicationDetails_Update_SuccessFully_Message";
       public const string Log_UpdateCreditNoteApplicationDetails_Update_Exception_Message = "Log_UpdateCreditNoteApplicationDetails_Update_Exception_Message";
       public const string Log_CreateCreditNoteApplication_CreateCall_Request_Message = "Log_CreateCreditNoteApplication_CreateCall_Request_Message";
       public const string Log_CreateCreditNoteApplication_CreateCall_Exception_Message = "Log_CreateCreditNoteApplication_CreateCall_Exception_Message";
       public const string Log_CreateCreditNoteApplication_CreateCall_SuccessFully_Message = "Log_CreateCreditNoteApplication_CreateCall_SuccessFully_Message";
       public const string Log_CreateCreditNoteDocumentVoid_CreateCall_Request_Message = "Log_CreateCreditNoteDocumentVoid_CreateCall_Request_Message";
       public const string Log_CreateCreditNoteDocumentVoid_CreateCall_SuccessFully_Message = "Log_CreateCreditNoteDocumentVoid_CreateCall_SuccessFully_Message";
       public const string Log_CreateCreditNoteApplicationReset_CreateCall_Request_Message = "Log_CreateCreditNoteApplicationReset_CreateCall_Request_Message";
       public const string Log_CreateCreditNoteApplicationReset_CreateCall_SuccessFully_Message = "Log_CreateCreditNoteApplicationReset_CreateCall_SuccessFully_Message";
       public const string Log_CreateCreditNoteApplicationReset_CreateCall_Exception_Message = "Log_CreateCreditNoteApplicationReset_CreateCall_Exception_Message";
       public const string Log_SaveCreditNoteDocumentVoid_SaveCall_Request_Message = "Log_SaveCreditNoteDocumentVoid_SaveCall_Request_Message";
       public const string Log_SaveCreditNoteDocumentVoid_SaveCall_SuccessFully_Message = "Log_SaveCreditNoteDocumentVoid_SaveCall_SuccessFully_Message";
       public const string Log_CreateCreditNoteByDebitNote_CreateCall_Request_Message = "Log_CreateCreditNoteByDebitNote_CreateCall_Request_Message";
       public const string Log_CreateCreditNoteByDebitNote_CreateCall_SuccessFully_Message = "Log_CreateCreditNoteByDebitNote_CreateCall_SuccessFully_Message";
       public const string Log_CreateCreditNoteByDebitNote_CreateCall_Exception_Message = "Log_CreateCreditNoteByDebitNote_CreateCall_Exception_Message";
    }
}
