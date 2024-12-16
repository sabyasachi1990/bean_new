using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Infra
{
    public static class BillLoggingValidation
    {
        public const string Log_GetAllBillModelk_GetCall_Request_Message = "Bill GetAllBillModelk GetCall  Request has Processing";
        public const string Log_GetAllBillModelk_GetCall_Exception_Message = "Bill GetAllBillModelk GetCall  has Failed Due to Exception";
        public const string Log_GetAllBillModelk_GetCall_SuccessFully_Message = "Bill GetAllBillModelk GetCall  has SuccessFully Completed";


        public const string Log_FillBillModelk_FillCall_Request_Message = "Bill FillBillModelk FillCall  Request has Processing";
        public const string Log_FillBillModelk_FillCall_Exception_Message = "Bill FillBillModelk FillCall  has Failed Due to Exception";
        public const string Log_FillBillModelk_FillCall_SuccessFully_Message = "Bill FillBillModelk FillCall  has SuccessFully Completed";

        public const string Log_FillBillModel_FillCall_Request_Message = "Bill FillBillModel FillCall  Request has Processing";
        public const string Log_FillBillModel_FillCall_Exception_Message = "Bill FillBillModel FillCall  has Failed Due to Exception";
        public const string Log_FillBillModel_FillCall_SuccessFully_Message = "Bill FillBillModel FillCall  has SuccessFully Completed";

        public const string Log_GetAllBillLUs_LookUPCall_Request_Message = "Bill GetAllBillLUs LookUPCall  Request has Processing";
        public const string Log_GetAllBillLUs_LookUPCall_Exception_Message = "Bill GetAllBillLUs LookUPCall  has Failed Due to Exception";
        public const string Log_GetAllBillLUs_LookUPCall_SuccessFully_Message = "Bill GetAllBillLUs LookUPCall  has SuccessFully Completed";


        public const string Log_CreateBill_CreateCall_Request_Message = "Bill CreateBill CreateCall  Request has Processing";
        public const string Log_CreateBill_CreateCall_Exception_Message = "Bill CreateBill CreateCall  has Failed Due to Exception";
        public const string Log_CreateBill_CreateCall_SuccessFully_Message = "Bill CreateBill CreateCall  has SuccessFully Completed";

        public const string Log_GetNewBillDocumentNumber_GetById_Request_Message = "Bill GetNewBillDocumentNumber GetById  Request has Processing";
        public const string Log_GetNewBillDocumentNumber_GetById_Exception_Message = "Bill GetNewBillDocumentNumber GetById  has Failed Due to Exception";
        public const string Log_GetNewBillDocumentNumber_GetById_SuccessFully_Message = "Bill GetNewBillDocumentNumber GetById  has SuccessFully Completed";


        public const string Log_GetNewBillCreditMemoDocumentNumber_GetById_Request_Message = "Bill GetNewBillCreditMemoDocumentNumber GetById  Request has Processing";
        public const string Log_GetNewBillCreditMemoDocumentNumber_GetById_Exception_Message = "Bill GetNewBillCreditMemoDocumentNumber GetById  has Failed Due to Exception";
        public const string Log_GetNewBillCreditMemoDocumentNumber_GetById_SuccessFully_Message = "Bill GetNewBillCreditMemoDocumentNumber GetById  has SuccessFully Completed";


        public const string Log_GetAllBillDetailModel_GetById_Request_Message = "Bill GetAllBillDetailModel GetById  Request has Processing";
        public const string Log_GetAllBillDetailModel_GetById_Exception_Message = "Bill GetAllBillDetailModel GetById  has Failed Due to Exception";
        public const string Log_GetAllBillDetailModel_GetById_SuccessFully_Message = "Bill GetAllBillDetailModel GetById  has SuccessFully Completed";

        public const string Log_FillBillDetailModel_FillCall_Request_Message = "Bill FillBillDetailModel FillCall  Request has Processing";
        public const string Log_FillBillDetailModel_FillCall_Exception_Message = "Bill FillBillDetailModel FillCall  has Failed Due to Exception";
        public const string Log_FillBillDetailModel_FillCall_SuccessFully_Message = "Bill FillBillDetailModel FillCall  has SuccessFully Completed";

        public const string Log_GetGstBillDetailById_GetById_Request_Message = "Bill GetGstBillDetailById GetById  Request has Processing";
        public const string Log_GetGstBillDetailById_GetById_Exception_Message = "Bill GetGstBillDetailById GetById  has Failed Due to Exception";
        public const string Log_GetGstBillDetailById_GetById_SuccessFully_Message = "Bill GetGstBillDetailById GetById  has SuccessFully Completed";


        public const string Log_FillBillGstDetailModel_FillCall_Request_Message = "Bill FillBillGstDetailModel FillCall  Request has Processing";
        public const string Log_FillBillGstDetailModel_FillCall_Exception_Message = "Bill FillBillGstDetailModel FillCall  has Failed Due to Exception";
        public const string Log_FillBillGstDetailModel_FillCall_SuccessFully_Message = "Bill FillBillGstDetailModel FillCall  has SuccessFully Completed";



        public const string Log_GenerateAutoNumberForType_GetById_Request_Message = "Bill GenerateAutoNumberForType GetById  Request has Processing";
        public const string Log_GenerateAutoNumberForType_GetById_Exception_Message = "Bill GenerateAutoNumberForType GetById  has Failed Due to Exception";
        public const string Log_GenerateAutoNumberForType_GetById_SuccessFully_Message = "Bill GenerateAutoNumberForType GetById  has SuccessFully Completed";

        public const string Log_GenerateFromFormat_GetById_Request_Message = "Bill GenerateFromFormat GetById  Request has Processing";
        public const string Log_GenerateFromFormat_GetById_Exception_Message = "Bill GenerateFromFormat GetById  has Failed Due to Exception";
        public const string Log_GenerateFromFormat_GetById_SuccessFully_Message = "Bill GenerateFromFormat GetById  has SuccessFully Completed";

        public const string Log_CreateBillDocumentVoid_CreateCall_Request_Message = "Bill CreateBillDocumentVoid CreateCall  Request has Processing";
        public const string Log_CreateBillDocumentVoid_CreateCall_Exception_Message = "Bill CreateBillDocumentVoid CreateCall  has Failed Due to Exception";
        public const string Log_CreateBillDocumentVoid_CreateCall_SuccessFully_Message = "Bill CreateBillDocumentVoid CreateCall  has SuccessFully Completed";


        public const string Log_SaveBillNoteDocumentVoid_SaveCall_Request_Message = "Bill SaveBillNoteDocumentVoid SaveCall  Request has Processing";
        public const string Log_SaveBillNoteDocumentVoid_SaveCall_Exception_Message = "Bill SaveBillNoteDocumentVoid SaveCall  has Failed Due to Exception";
        public const string Log_SaveBillNoteDocumentVoid_SaveCall_SuccessFully_Message = "Bill SaveBillNoteDocumentVoid SaveCall  has SuccessFully Completed";


        public const string Log_SaveBill_SaveCall_Request_Message = "Bill SaveBill SaveCall  Request has Processing";
        public const string Log_SaveBill_SaveCall_Exception_Message = "Bill SaveBill SaveCall  has Failed Due to Exception";
        public const string Log_SaveBill_SaveCall_SuccessFully_Message = "Bill SaveBill SaveCall  has SuccessFully Completed";


        public const string Log_IsDocumentNumberExists_GetById_Request_Message = "Bill IsDocumentNumberExists GetById  Request has Processing";
        public const string Log_IsDocumentNumberExists_GetById_Exception_Message = "Bill IsDocumentNumberExists GetById  has Failed Due to Exception";
        public const string Log_IsDocumentNumberExists_GetById_SuccessFully_Message = "Bill IsDocumentNumberExists GetById  has SuccessFully Completed";

        public const string Log_InsertBill_FillCall_Request_Message = "Bill InsertBill FillCall  Request has Processing";
        public const string Log_InsertBill_FillCall_Exception_Message = "Bill InsertBill FillCall  has Failed Due to Exception";
        public const string Log_InsertBill_FillCall_SuccessFully_Message = "Bill InsertBill FillCall  has SuccessFully Completed";

        public const string Log_UpdateBillDetails_UpdateCall_Request_Message = "Bill UpdateBillDetails UpdateCall  Request has Processing";
        public const string Log_UpdateBillDetails_UpdateCall_Exception_Message = "Bill UpdateBillDetails UpdateCall  has Failed Due to Exception";
        public const string Log_UpdateBillDetails_UpdateCall_SuccessFully_Message = "Bill UpdateBillDetails UpdateCall  has SuccessFully Completed";

        public const string Log_UpdateBillGSTDetails_UpdateCall_Request_Message = "Bill UpdateBillGSTDetails UpdateCall  Request has Processing";
        public const string Log_UpdateBillGSTDetails_UpdateCall_Exception_Message = "Bill UpdateBillGSTDetails UpdateCall  has Failed Due to Exception";
        public const string Log_UpdateBillGSTDetails_UpdateCall_SuccessFully_Message = "Bill UpdateBillGSTDetails UpdateCall  has SuccessFully Completed";

        public const string Log_UpdateBillCreditMemo_UpdateCall_Request_Message = "Bill UpdateBillCreditMemo UpdateCall  Request has Processing";
        public const string Log_UpdateBillCreditMemo_UpdateCall_Exception_Message = "Bill UpdateBillCreditMemo UpdateCall  has Failed Due to Exception";
        public const string Log_UpdateBillCreditMemo_UpdateCall_SuccessFully_Message = "Bill UpdateBillCreditMemo UpdateCall  has SuccessFully Completed";

        public const string Log_UpdateBillCreditMemoGstDetail_UpdateCall_Request_Message = "Bill UpdateBillCreditMemoGstDetail UpdateCall  Request has Processing";
        public const string Log_UpdateBillCreditMemoGstDetail_UpdateCall_Exception_Message = "Bill UpdateBillCreditMemoGstDetail UpdateCall  has Failed Due to Exception";
        public const string Log_UpdateBillCreditMemoGstDetail_UpdateCall_SuccessFully_Message = "Bill UpdateBillCreditMemoGstDetail UpdateCall  has SuccessFully Completed";

        public const string Log_UpdateBillCrediMemoDetailsDetails_UpdateCall_Request_Message = "Bill UpdateBillCrediMemoDetailsDetails UpdateCall  Request has Processing";
        public const string Log_UpdateBillCrediMemoDetailsDetails_UpdateCall_Exception_Message = "Bill UpdateBillCrediMemoDetailsDetails UpdateCall  has Failed Due to Exception";
        public const string Log_UpdateBillCrediMemoDetailsDetails_UpdateCall_SuccessFully_Message = "Bill UpdateBillCrediMemoDetailsDetails UpdateCall  has SuccessFully Completed";

        public const string Log_VendorLu_LookUPCall_Request_Message = "Bill VendorLu LookUPCall  Request has Processing";
        public const string Log_VendorLu_LookUPCall_Exception_Message = "Bill VendorLu LookUPCall  has Failed Due to Exception";
        public const string Log_VendorLu_LookUPCall_SuccessFully_Message = "Bill VendorLu LookUPCall  has SuccessFully Completed";

        public const string Enter_into_SaveBill_method = "Enter into SaveBill method";
        public const string Enter_into_if_condition_of_Journal_and_check_Journal_is_null_or_not = "Enter into if condition of Journal and check Journal is null or not";
        public const string Insert_Bill_Method_Came = "Insert Bill Method Came";
        public const string Come_Out_From_Insert_Bill_Method = "Come Out From Insert Bill Method";

        public const string Enter_Into_Update_Bill_Details_Method = "Enter Into Update Bill Details Method";
        public const string Come_Out_From_Update_Bill_Details = "Come Out From Update Bill Details";
        public const string Enter_Into_Update_Bill_GST_Details = "Enter Into Update Bill GST Details";
        public const string Come_Out_Form_Update_GST_Details = "Come Out Form Update GST Details";
        public const string Enter_Into_Update_Bill_Credit_Memo = "Enter Into Update Bill Credit Memo";
        public const string Come_Out_From_Update_Bill_Credit_Memo = "Come Out From Update Bill Credit Memo";
        public const string Enter_Into_Fill_Journal = "Enter Into Fill Journal";
        public const string Come_Out_From_Fill_Journal = "Come Out From Fill Journal";
        public const string Enter_Into_Save_Bill = "Enter Into Save Bill";
        public const string Come_Out_From_Save_Bill = "Come Out From Save Bill";

        public const string Enter_Into_Insert_Bill = "Enter Into Insert Bill ";
        public const string Come_Out_From_Insert_Bill = "Come Out From Insert Bill";
        public const string Enter_In_If_Conditionand_Check_TObject_IsGstSetting = "Enter In If Conditionand Check TObject IsGstSetting";
        public const string Enter_In_If_Condition_And_Check_BillNew_IsSegmentReporting = "Enter In If Condition And Check BillNew IsSegmentReporting";
        public const string Enter_In_If_Condition_And_Check_TObject_IsSegmentActive1_Value = "Enter In If Condition And Check TObject IsSegmentActive1 Value";
        public const string Enter_In_If_Condition_And_Check_TObject_IsSegmentActive2_Value = "Enter In If Condition And Check TObject IsSegmentActive2 Value";
        public const string Enter_In_Else_Condition_Of_billNew_IsSegmentReporting = "Enter In Else Condition Of billNew IsSegmentReporting";
        public const string Enter_Into_If_Condition_And_Check_Tobject_BillDetail_Modules_Count_And_Bill_Details_Module = "Enter Into If Condition And Check Tobject BillDetail Modules Count And Bill Details Module";
        public const string Enter_Into_BillDetaulModel_Foreach = "Enter Into BillDetaulModel Foreach";
        public const string Come_Out_From_BillDetailModule_Foereach = "Come Out From BillDetailModule Foereach";
        public const string Enter_If_Condition_and_Check_Tobject_BillCreditMemoModel_Is_Not_Null = "Enter If Condition and Check Tobject BillCreditMemoModel Is Not Null";
        public const string Enter_In_If_Condition_And_Check_Tobject_BillCreditMemoModel_BillCreditMemoGSTDetails_is_not_null = "Enter In If Condition And Check Tobject BillCreditMemoModel BillCreditMemoGSTDetails is not null";
        public const string Enter_In_If_Condition_And_Check_Tobject_BillGSTDetails_Is_Null = "Enter In If Condition And Check Tobject BillGSTDetails Is Null";
        public const string Enter_In_object_BillCreditMemoModel_BillCreditMemoGSTDetails_Foreach = "Enter In object BillCreditMemoModel BillCreditMemoGSTDetails Foreach";
        public const string Enter_In_Tobject_BillGSTDetails_Foreach = "Enter In Tobject BillGSTDetails Foreach";
        public const string Enter_Into_Try_Block_Of_the_SaveCall = "Enter Into Try Block Of the SaveCall";
        public const string Enter_If_Condition_And_Check_Event_Store_Of_SaveCall = "Enter If Condition And Check Event Store Of SaveCall";
        public const string Enter_Else_Condition_And_Check_Event_Store_Of_SaveCall = "Enter Else Condition And Check Event Store Of SaveCall";
        public const string Enter_Into_Catch_Of_The_SaveCall = "Enter Into Catch Of The SaveCall";
        public const string Log_CreateCreditNoteByDebitNote_CreateCall_Request_Message = "Log CreateCreditNoteByDebitNote CreateCall Request Message";
        public const string Log_CreateCreditMemoBybill_CreateCall_Request_Message = "Log CreateCreditMemoBybill CreateCall Request Message";
        public const string Log_CreateCreditMemoBybill_CreateCall_SuccessFully_Message = "Log CreateCreditMemoBybill CreateCall SuccessFully Message";

        public const string No_exchange_rate_present_based_on_currency = "Sorry ! No exchange rate present based on currency,you can't run payroll bill";
        public const string Entred_into_SaveBillJv_Method = "Entred into SaveBillJv Method";
        public const string Entred_into_deleteJVPostInvoce_Method = "Entred into deleteJVPostInvoce Method";
        public const string Issues_in_Bill_Folder_creation = "Issues in Bill Folder Creation";
        public const string HR_bill_status_call_executing = "HR bill status call executing";
        public const string Out_from_HR_bill_status_call_executing = "Out_from_HR_bill_status_call_executing";












        public const string Log_InBoundAllInvoiceMethod_Request_Message = "InBoundAllInvoiceMethod  Request Message";
        public const string Log_InvoiceURL_LoadTOXMLDOC = "Invoice URL Load to XML Documnet sucessful.";
        public const string Log_XMLDOCSerilization = "XML Documnet Serilization sucessful.";

        public const string Log_XMLDOC_DeSerilization_MultipleLineItems = "XML Documnet DeSerilization Multiple Line Items Sucessful.";
        public const string Log_MultipleLineItems_ModelBinding_Sucessful = "Multiple Line Items Model Binding Sucessful.";
        public const string Log_XMLDOC_DeSerilization_SingleLineItems = "XML Documnet DeSerilization Single Line Items Sucessful.";
        public const string Log_SingleLineItems_ModelBinding_Sucessful = "Single Line Items Model Binding Sucessful.";
        public const string Log_ConvertInBoundInvoiceToBill_Request_Message = "ConvertInBoundInvoiceToBill Request Message";
        public const string Log_ConvertInBoundInvoiceToBill_Request_Sucessful = "ConvertInBoundInvoiceToBill Request Sucessful";
        public const string Log_XMLDocument_Start_Message = "XMLDocument Start Message";
    }
}
