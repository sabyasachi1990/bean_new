using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Infra.Resources
{
    public static class InvoiceLoggingValidation
    {
        public const string Log_CreateInvoice_CreateCall_Request_Message = "Log CreateInvoice CreateCall Request Message";
        public const string Log_CreateInvoice_CreateCall_SuccessFully_Message = "Log CreateInvoice CreateCall SuccessFully Message";
        public const string Log_CreateInvoice_CreateCall_Exception_Message = "Log reateInvoice CreateCall Exception Message";
        public const string Log_GetAllInvoiceLUs_LookupCall_Request_Message = "Log GetAllInvoiceLUs LookupCall Request Message";
        public const string Log_GetAllInvoiceLUs_LookupCall_SuccessFully_Message = "Log GetAllInvoiceLUs LookupCall SuccessFully Message";
        public const string Log_GetAllInvoiceLUs_LookupCall_Exception_Message = "Log GetAllInvoiceLUs LookupCall Exception Message";
        public const string Log_SaveInvoice_SaveCall_Request_Message = "Log SaveInvoice SaveCall Request Message";
        public const string Checking_Invoice_is_null_or_not = "Checking Invoice is null or not";
        public const string InsertInvoice_method_came = "InsertInvoice method came";
        public const string Exited_from_SavePostedFromRecurring_method = "Exited from SavePostedFromRecurring method";
        public const string Came_to_FillInvoiceDetail_method = "Came to FillInvoiceDetail method";
        public const string Came_to_FillGStReporting_method_for_GST_Calculation = "Came to FillGStReporting method for GST Calculation";
        public const string Exist_from_FillGStReporting_method_for_GST_Calculation = "Existed from FillGStReporting method for GST Calculation";
        public const string Existed_from_FillInvoiceDetail_method = "Existed from FillInvoiceDetail method";
        public const string UpdateInvoiceDetails_method_came = "UpdateInvoiceDetails method came";
        public const string UpdateInvoiceNotes_method_came = "UpdateInvoiceNotes method came";
        public const string UpdateInvoiceGSTDetails_method_came = "UpdateInvoiceGSTDetails method came";
        public const string FillJournal_method_came = "FillJournal method came";
        public const string SaveInvoice1_method_came = "SaveInvoice1 method came";
        public const string Log_SaveInvoice_SaveCall_SuccessFully_Message = "Log SaveInvoice SaveCall SuccessFully Message";
        public const string Log_SaveInvoice_SaveCall_Exception_Message = "Log_SaveInvoice SaveCall Exception Message";
        public const string Log_GetInvoiceDetail_GetCall_Request_Message="Log GetInvoiceDetail GetCall Request Message";
        public const string Log_GetInvoiceDetail_GetCall_SuccessFully_Message = "Log GetInvoiceDetail GetCall SuccessFully Message";
        public const string Log_CreateInvoiceDocumentVoid_CreateCall_Request_Message = "Log CreateInvoiceDocumentVoid CreateCall Request Message";
        public const string Log_CreateInvoiceDocumentVoid_CreateCall_SuccessFully_Message = "Log CreateInvoiceDocumentVoid CreateCall SuccessFully Message";
        public const string Log_CreateInvoiceDocumentVoid_CreateCall_Exception_Message = "Log CreateInvoiceDocumentVoid CreateCall Exception Message";
        public const string Log_GetProvision_CreateCall_Request_Message = "Log GetProvision CreateCall Request Message";
        public const string Log_GetProvision_CreateCall_SuccessFully_Message = "Log GetProvision CreateCall SuccessFully Message";
        public const string Log_GetProvision_GetCall_Exception_Message = "Log GetProvision GetCall Exception Message";
        public const string Log_CreateCreditNoteByInvoice_CreateCall_Request_Message = "Log CreateCreditNoteByInvoice CreateCall Request Message";
        public const string Log_SaveInvoiceDocumentVoid_SaveCall_Request_Message = "Log SaveInvoiceDocumentVoid SaveCall Request Message";
        public const string Log_SaveInvoiceDocumentVoid_SuccessFully_Message = "Log SaveInvoiceDocumentVoid SuccessFully Message";
        public const string Log_SaveInvoiceDocumentVoid_SaveCall_Exception_Message = "Log SaveInvoiceDocumentVoid SaveCall Exception Message";
        public const string Log_CreateCreditNoteByInvoice_CreateCall_SuccessFully_Message = "Log CreateCreditNoteByInvoice CreateCall SuccessFully Message";
        public const string Log_CreateCreditNoteByInvoice_CreateCall_Exception_Message = "Log CreateCreditNoteByInvoice CreateCall Exception Message";
        public const string Log_CreateDoubtFulDebtByInvoice_CreateCall_Request_Message = "Log CreateDoubtFulDebtByInvoice CreateCall Request Message";
        public const string Log_CreateDoubtFulDebtByInvoice_CreateCall_SuccessFully_Message = "Log CreateDoubtFulDebtByInvoice CreateCall SuccessFully Message";
        public const string Log_CreateDoubtFulDebtByInvoice_CreateCall_Exception_Message = "Log CreateDoubtFulDebtByInvoice CreateCall Exception Message";
        public const string Log_ModuleActivations_ActivationCall_Request_Message = "Log ModuleActivations ActivationCall Request Message";
        public const string Log_ModuleActivations_ActivationCall_SuccessFully_Message = "Log ModuleActivations ActivationCall SuccessFully Message";
        public const string Log_ModuleActivations_ActivationCall_Exception_Message = "Log ModuleActivations ActivationCall Exception Message";
        public const string Log_FillInvoice_FillCall_Request_Message = "Log FillInvoice FillCall Request Message"; 
        public const string Log_FillInvoice_FillCall_SuccessFully_Message = "Log FillInvoice FillCall SuccessFully Message";
        public const string Log_FillInvoice_FillCall_Exception_Message = "Log FillInvoice FillCall Exception Message";
        public const string Log_FillProvisionModel_FillCall_Request_Message = "Log FillProvisionModel FillCall Request Message";
        public const string Log_FillProvisionModel_FillCall_SuccessFully_Message = "Log FillProvisionModel FillCall SuccessFully Message";
        public const string Log_FillProvisionModel_FillCall_Exception_Message = "Log FillProvisionModel FillCall Exception Message";
        public const string Log_FillCreditNoteDetailModel_FillCall_Request_Message = "Log FillCreditNoteDetailModel FillCall Request Message";
        public const string Log_FillCreditNoteDetailModel_FillCall_SuccessFully_Message = "Log FillCreditNoteDetailModel FillCall SuccessFully Message";
        public const string Log_FillCreditNoteDetailModel_FillCall_Exception_Message = "Log FillCreditNoteDetailModel FillCall Exception Message";
        public const string Log_UpdateInvoiceDetails_Update_Request_Message = "Log UpdateInvoiceDetails Update Request Message";
        public const string Log_UpdateInvoiceDetails_Update_SuccessFully_Message = "Log UpdateInvoiceDetails Update SuccessFully Message";
        public const string Log_UpdateInvoiceDetails_Update_Exception_Message = "Log UpdateInvoiceDetails Update Exception Message";
        public const string Log_UpdateInvoiceNotes_Update_Request_Message = "Log UpdateInvoiceNotes Update Request Message";
        public const string Log_UpdateInvoiceNotes_Update_Exception_Message = "Log UpdateInvoiceNotes Update Exception Message";
        public const string Log_UpdateInvoiceGSTDetails_Update_Request_Message = "Log UpdateInvoiceGSTDetails Update Request Message";
        public const string Log_UpdateInvoiceGSTDetails_Update_SuccessFully_Message = "Log UpdateInvoiceGSTDetails Update SuccessFully Message";
        public const string Log_UpdateInvoiceGSTDetails_Update_Exception_Message = "Log UpdateInvoiceGSTDetails Update Exception Message";
        public const string Log_Enter_into_CreateReceiptByInvoice_action = "Enter into CreateReceiptByInvoice action";

        public const string InvoiceApplicationService = "InvoiceApplicationService";
        public const string Enter_Into_Update_OB_LineItem = "Enter Into Update OB LineItem";
        public const string WF_invoice_status_call_executing = "WF invoice status call executing";
        public const string Out_from_WF_invoice_status_call_executing = "Out from wf invoice status call executing";
        public const string WF_invoice_status_call_execution_completed = "WF invoice status call execution completed";
        public const string Invoice_Folder_Creation_Failed = "Invoice Folder Creation Failed";
        public const string InvoiceController = "InvoiceController";
        public const string CNApplicationController = "CNApplicationController";
        public const string CNAApplicationService = "CNAApplicationService";
    }
}
