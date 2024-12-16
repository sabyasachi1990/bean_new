using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Infra
{
    public static class DebitNoteLoggingValaidation
    {
        public const string  Log_GetDebitNoteAllLUs_GetCall_Request_Message="Log GetDebitNoteAllLUs GetCall Request Message";
        public const string Log_GetDebitNoteAllLUs_GetCall_SuccessFully_Message = "Log GetDebitNoteAllLUs GetCall SuccessFully Message";
        public const string Log_GetDebitNoteAllLUs_GetCall_Exception_Message = "Log GetDebitNoteAllLU GetCall Exception Message";
        public const string Log_CreateDebitNote_CreateCall_Request_Message = "Log CreateDebitNote CreateCall Request Message";
        public const string Log_FillViewModel_FillCall_Request_Message = "Log FillViewModel FillCall Request Message";
        public const string Log_FillViewModel_FillCall_SuccessFully_Message = "Log FillViewModel FillCall SuccessFully Message";
        public const string Log_FillViewModel_FillCall_Exception_Message = "Log FillViewModel FillCall Exception Message";
        public const string Log_CreateDebitNote_CreateCall_SuccessFully_Message = "Log CreateDebitNote CreateCall SuccessFully Message";
        public const string Log_CreateDebitNote_CreateCall_Exception_Message = "Log CreateDebitNote CreateCall Exception Message";
        public const string Log_GetNewDebitNoteDocumentNumber_GetCall_SuccessFully_Message = "Log GetNewDebitNoteDocumentNumber GetCall SuccessFully Message";
        public const string Log_GetNewDebitNoteDocumentNumber_GetCall_Exception_Message = "Log GetNewDebitNoteDocumentNumber GetCall Exception Message";
        public const string Log_GetNewDebitNoteDocumentNumber_GetCall_Request_Message = "Log GetNewDebitNoteDocumentNumber GetCall Request Message";
        public const string Log_GetDebitNoteDetail_GetCall_Request_Message = "Log GetDebitNoteDetail GetCall Request Message";
        public const string Log_GetDebitNoteDetail_GetCall_SuccessFully_Message = "Log GetDebitNoteDetail GetCall SuccessFully Message";
        public const string Log_GetDebitNoteDetail_GetCall_Exception_Message = "Log GetDebitNoteDetail GetCall Exception Message";
        public const string Log_GetDebitNoteNote_GetCall_Request_Message = "Log GetDebitNoteNote GetCall Request Message";
        public const string Log_GetDebitNoteNote_GetCall_SuccessFully_Message = "Log GetDebitNoteNote GetCall SuccessFull Message";
        public const string Log_GetDebitNoteNote_GetCall_Exception_Message = "Log GetDebitNoteNote GetCall Exception Message";
        public const string Log_GenerateAutoNumberForType_GenerateCall_Request_Message = "Log GenerateAutoNumberForType GenerateCall Request Message";
        public const string Log_GenerateAutoNumberForType_GenerateCall_SuccessFully_Message = "Log GenerateAutoNumberForType GenerateCall SuccessFully Message";
        public const string Log_GenerateAutoNumberForType_GenerateCall_Exception_Message = "Log GenerateAutoNumberForType GenerateCall Exception Message";
        public const string Log_InsertDebitNote_FillCall_SuccessFully_Message = "Log InsertDebitNote FillCall SuccessFully Message";
        public const string Log_InsertDebitNote_FillCall_Exception_Message = "Log InsertDebitNote FillCall Exception Message";
        public const string Log_GenerateFromFormat_GenerateCall_Exception_Message = "Log GenerateFromFormat GenerateCall Exception Message";
        public const string Log_GenerateFromFormat_GenerateCall_SuccessFully_Message = "Log GenerateFromFormat GenerateCall SuccessFully Message";
        public const string Log_InsertDebitNote_FillCall_Request_Message = "Log InsertDebitNote FillCall Request Message";
        public const string Log_Save_SaveCalll_SuccessFully_Message = "Log Save SaveCalll SuccessFully Message";
        public const string Log_UpdateDebitNoteDetails_UpdateCall_Request_Message = "Log UpdateDebitNoteDetails UpdateCall Request Message";
        public const string Log_UpdateDebitNoteDetails_UpdateCall_SuccessFully_Message = "Log UpdateDebitNoteDetails UpdateCall SuccessFully Message";
        public const string Log_UpdateDebitNoteDetails_UpdateCall_Exception_Message = "Log UpdateDebitNoteDetails UpdateCall Exception Message";
        public const string Log_UpdateDebitNoteNotes_UpdateCall_Request_Message = "Log UpdateDebitNoteNotes UpdateCall Reques _Message";
        public const string Log_UpdateDebitNoteNotes_UpdateCall_SuccessFully_Message = "Log UpdateDebitNoteNotes UpdateCall SuccessFully Message";
        public const string Log_UpdateDebitNoteNotes_UpdateCall_Exception_Message = "Log UpdateDebitNoteNotes UpdateCall_Exception Message";
        public const string Log_UpdateDebitNoteGSTDetails_UpdateCall_Request_Message = "Log UpdateDebitNoteGSTDetails UpdateCall Request Message";
        public const string Log_UpdateDebitNoteGSTDetails_UpdateCall_SuccessFully_Message = "Log UpdateDebitNoteGSTDetail UpdateCall SuccessFully Message";
        public const string Log_UpdateDebitNoteGSTDetails_UpdateCall_Exception_Message = "Log UpdateDebitNoteGSTDetails UpdateCall Exception Message";
        public const string Log_Save_SaveCall_Request_Message = "Log_Save_SaveCall_Request_Message";
        public const string Log_CreateDebitNoteDocumentVoid_CreateCall_Request_Message = "Log CreateDebitNoteDocumentVoid CreateCall Request Message";
        public const string Log_CreateDebitNoteDocumentVoid_CreateCall_SuccessFully_Message = "Log CreateDebitNoteDocumentVoid CreateCall SuccessFully Message";
        public const string Log_CreateDebitNoteDocumentVoid_CreateCall_Exception_Message = "Log CreateDebitNoteDocumentVoid CreateCall Exception Message";
        public const string Log_SaveDebitNoteDocumentVoid_SaveCall_Request_Message = "Log SaveDebitNoteDocumentVoid SaveCall Request Message";
        public const string Log_SaveDebitNoteDocumentVoid_SaveCalll_SuccessFully_Message = "Log SaveDebitNoteDocumentVoid SaveCall _SuccessFully Message";
        public const string Log_SaveDebitNoteDocumentVoid_SaveCall_Exception_Message = "Log SaveDebitNoteDocumentVoid SaveCall Exception Message";
        public const string Log_UpdateProvision_UpdateCall_Request_Message = "Log UpdateProvision UpdateCall Request Message";
        public const string Log_UpdateProvision_UpdateCall_SuccessFully_Message = "Log UpdateProvision UpdateCall SuccessFully Message";
        public const string Log_UpdateProvision_UpdateCall_Exception_Message = "Log UpdateProvision UpdateCall Exception Message";
        public const string Log_FillProvision_FillCall_Request_Message = "Log FillProvision FillCal Reques Message";
        public const string Log_FillProvision_FillCall_SuccessFully_Message = "Log FillProvision FillCall SuccessFully Message";
        public const string Log_FillProvision_FillCall_Exception_Message = "Log FillProvision FillCall Exception Message";
        public const string Log_UpdateCreditNote_UpdateCall_Request_Message = "Log UpdateCreditNote UpdateCall Request Message";
        public const string Log_UpdateCreditNote_UpdateCall_SuccessFully_Message = "Log UpdateCreditNote UpdateCall SuccessFully Message";
        public const string Log_UpdateCreditNote_UpdateCall_Exception_Message = "Log UpdateCreditNote UpdateCall Exception Message";
        public const string Log_UpdateCreditNoteDetail_UpdateCall_Request_Message = "Log UpdateCreditNoteDetail UpdateCall Request Message";
        public const string Log_UpdateCreditNoteDetail_UpdateCall_SuccessFully_Message = "Log UpdateCreditNoteDetail UpdateCall SuccessFully Message";
        public const string Log_UpdateCreditNoteDetail_UpdateCall_Exception_Message = "Log UpdateCreditNoteDetail UpdateCall Exception Message";
        public const string Log_FillCreditNoteDetail_FillCall_Request_Message = "Log FillCreditNoteDetai FillCall Request Message";
        public const string Log_FillCreditNoteDetail_FillCall_SuccessFully_Message = "Log FillCreditNoteDetail FillCall SuccessFully Message";
        public const string Log_FillCreditNoteDetail_FillCall_Exception_Message = "Log FillCreditNoteDetail FillCall Exception Message";
        public const string Log_GenerateFromFormat_GenerateCall_Request_Message = "Log GenerateFromFormat GenerateCall Request Message";
        public const string Log_GetNewInvoiceDocumentNumber_GetCall_Exception_Message = "Log GetNewInvoiceDocumentNumber GetCall Exception Message";
        public const string Log_GetNewInvoiceDocumentNumber_GetCall_SuccessFully_Message = "Log GetNewInvoiceDocumentNumbe _GetCall SuccessFully Message";
        public const string Log_GetNewInvoiceDocumentNumber_GetCall_Request_Message = "Log GetNewInvoiceDocumentNumber GetCall Request Message";
        public const string Log_CreateCreditNoteByDebitNote_CreateCall_Request_Message = "Log CreateCreditNoteByDebitNote CreateCall Request Message";
        public const string Log_CreateCreditNoteByDebitNote_CreateCall_Exception_Message = "Log CreateCreditNoteByDebitNote CreateCall Exception Message";
        public const string Log_CreateCreditNoteByDebitNote_CreateCall_SuccessFully_Message = "Log CreateCreditNoteByDebitNote CreateCall SuccessFully Message";
        public const string Log_CreateDoubtFulDebtByDebitNote_CreateCall_Request_Message = "Log CreateDoubtFulDebtByDebitNote CreateCal Request Message";
        public const string Log_CreateDoubtFulDebtByDebitNote_CreateCall_SuccessFully_Message = "Log CreateDoubtFulDebtByDebitNote CreateCall SuccessFully Message";
        public const string Log_CreateDoubtFulDebtByDebitNote_CreateCall_Exception_Message = "Log CreateDoubtFulDebtByDebitNote CreateCall Exception Message";
        public const string Log_GetNewProvisionDocumentNumber_GetCall_Request_Message = "Log GetNewProvisionDocumentNumber GetCall Request Message";
        public const string Log_GetNewProvisionDocumentNumber_GetCall_Exception_Message = "Log GetNewProvisionDocumentNumber GetCall Exception Message";
    }


}
