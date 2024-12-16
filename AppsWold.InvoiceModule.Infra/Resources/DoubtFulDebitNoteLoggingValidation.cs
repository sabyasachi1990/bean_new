using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Infra.Resources
{
    public class DoubtFulDebitNoteLoggingValidation
    {
        public const string Log_CreateDoubtfulDebt_CreateCall_Request_Message = "Log_CreateDoubtfulDebt_CreateCall_Request_Message";
        public const string Log_FillDoubtfulDebt_FillCall_Request_Message = "Log_FillDoubtfulDebt_FillCall_Request_Message";
        public const string Log_FillDoubtfulDebt_FillCall_SuccessFully_Message = "Log_FillDoubtfulDebt_FillCall_SuccessFully_Message";
        public const string Log_FillDoubtfulDebt_FillCall_Exception_Message = "Log_FillDoubtfulDebt_FillCall_Exception_Message";
        public const string Log_FillDoubtfulDebtAllocationModel_FillCall_Request_Message = "Log_FillDoubtfulDebtAllocationModel_FillCall_Request_Message";
        public const string Log_FillDoubtfulDebtAllocationModel_FillCall_SuccessFully_Message = "Log_FillDoubtfulDebtAllocationModel_FillCall_SuccessFully_Message";
        public const string Log_FillDoubtfulDebtAllocationModel_FillCall_Exception_Message = "Log_FillDoubtfulDebtAllocationModel_FillCall_Exception_Message";
        public const string Log_GetAllDoubtfulDebtLUs_GetCall_Request_Message = "Log_GetAllDoubtfulDebtLUs_GetCall_Request_Message";
        public const string Log_GetNextAllocationNumber_GetCall_Exception_Message = "Log_GetNextAllocationNumber_GetCall_Exception_Message";
        public const string Log_SaveDoubtfulDebt_SaveCall_Request_Message = "Log_SaveDoubtfulDebt_SaveCall_Request_Message";
        public const string Log_SaveDoubtfulDebt_saveCall_SuccessFully_Message = "Log_SaveDoubtfulDebt_saveCall_SuccessFully_Message";
        public const string Log_SaveDoubtfulDebt_SaveCall_Exception_Message = "Log_SaveDoubtfulDebt_SaveCall_Exception_Message";
        public const string Log_InsertDoubtfulDebt_FillCall_Request_Message = "Log_InsertDoubtfulDebt_FillCall_Request_Message";
        public const string Log_InsertDoubtfulDebt_FillCall_SuccessFully_Message = "Log_InsertDoubtfulDebt_FillCall_SuccessFully_Message";
        public const string Log_InsertDoubtfulDebt_FillCall_Exception_Message = "Log_InsertDoubtfulDebt_FillCall_Exception_Message";
        public const string Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_Request_Message = "Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_Request_Message";
        public const string Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_SuccessFully_Message = "Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_SuccessFully_Message";
        public const string Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_Exception_Message = "Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_Exception_Message";
        public const string Log_SaveDoubtFulDebtAllocation_SaveCall_Request_Message = "Log_SaveDoubtFulDebtAllocation_SaveCall_Request_Message";
        public const string Log_SaveDoubtFulDebtAllocation_saveCall_SuccessFully_Message = "Log_SaveDoubtFulDebtAllocation_saveCall_SuccessFully_Message";
        public const string Log_SaveDoubtFulDebtAllocation_SaveCall_Exception_Message = "Log_SaveDoubtFulDebtAllocation_SaveCall_Exception_Message";
        public const string Log_CreateDoubtfulDebtReverse_CreateCall_Request_Message = "Log_CreateDoubtfulDebtReverse_CreateCall_Request_Message";
        public const string Log_CreateDoubtfulDebtReverse_CreateCall_SuccessFully_Message = "Log_CreateDoubtfulDebtReverse_CreateCall_SuccessFully_Message";
        public const string Log_CreateDoubtfulDebtReverse_CreateCall_Exception_Message = "Log_CreateDoubtfulDebtReverse_CreateCall_Exception_Message";
        public const string Log_SaveDoubtfulDebtReverse_SaveCall_Request_Message = "Log_SaveDoubtfulDebtReverse_SaveCall_Request_Message";
        public const string Log_SaveDoubtfulDebtReverse_saveCall_SuccessFully_Message = "Log_SaveDoubtfulDebtReverse_saveCall_SuccessFully_Message";
        public const string Log_SaveDoubtfulDebtReverse_SaveCall_Exception_Message = "Log_SaveDoubtfulDebtReverse_SaveCall_Exception_Message";
        public const string Log_CreateDoubtfulDebtDocumentVoid_CreateCall_Request_Message = "Log_CreateDoubtfulDebtDocumentVoid_CreateCall_Request_Message";
        public const string Log_CreateDoubtfulDebtDocumentVoid_CreateCall_SuccessFully_Message = "Log_CreateDoubtfulDebtDocumentVoid_CreateCall_SuccessFully_Message";
        public const string Log_CreateDoubtfulDebtDocumentVoid_CreateCall_Exception_Message = "Log_CreateDoubtfulDebtDocumentVoid_CreateCall_Exception_Message";
        public const string Log_SaveDoubtfulDebtDocumentVoid_SaveCall_Request_Message = "Log_SaveDoubtfulDebtDocumentVoid_SaveCall_Request_Message";
        public const string Log_SaveDoubtfulDebtDocumentVoid_saveCall_SuccessFully_Message = "Log_SaveDoubtfulDebtDocumentVoid_saveCall_SuccessFully_Message";
        public const string Log_SaveDoubtfulDebtDocumentVoid_SaveCall_Exception_Message = "Log_SaveDoubtfulDebtDocumentVoid_SaveCall_Exception_Message";
        public const string Log_CreateDoubtfulDebtAllocationReset_CreateCall_Request_Message = "Log_CreateDoubtfulDebtAllocationReset_CreateCall_Request_Message";
        public const string Log_CreateDoubtfulDebtAllocationReset_CreateCall_SuccessFully_Message = "Log_CreateDoubtfulDebtAllocationReset_CreateCall_SuccessFully_Message";
        public const string Log_CreateDoubtfulDebtAllocationReset_CreateCall_Exception_Message = "Log_CreateDoubtfulDebtAllocationReset_CreateCall_Exception_Message";
        public const string Log_SaveDobtfulDebtAllocationReset_SaveCall_Request_Message = "Log_SaveDobtfulDebtAllocationReset_SaveCall_Request_Message";
        public const string Log_SaveDobtfulDebtAllocationReset_saveCall_SuccessFully_Message = "Log_SaveDobtfulDebtAllocationReset_saveCall_SuccessFully_Message";
        public const string Log_SaveDobtfulDebtAllocationReset_SaveCall_Exception_Message = "Log_SaveDobtfulDebtAllocationReset_SaveCall_Exception_Message";
        public const string Log_CreateDoubtFulDebtAllocation_CreateCall_Request_Message = "Log_CreateDoubtFulDebtAllocation_CreateCall_Request_Message";
        public const string Log_CreateDoubtFulDebtAllocation_CreateCall_SuccessFully_Message = "Log_CreateDoubtFulDebtAllocation_CreateCall_SuccessFully_Message";
        public const string Log_CreateoubtFulDebtAllocation_CreateCall_Exception_Message = "Log_CreateDoubtFulDebtAllocation_CreateCall_Exception_Message";
        public const string Log_CreateDoubtFulDebtByDebitNote_CreateCall_Request_Message = "Log_CreateDoubtFulDebtByDebitNote_CreateCall_Request_Message";
        public const string Log_CreateDoubtFulDebtAllocation_CreateCall_Exception_Message = "Log_CreateDoubtFulDebtAllocation_CreateCall_Exception_Message";
        public const string Log_CreateDoubtFulDebtByDebitNote_CreateCall_SuccessFully_Message = "Log_CreateDoubtFulDebtByDebitNote_CreateCall_SuccessFully_Message";
        public const string Log_CreateDoubtFulDebtByDebitNote_CreateCall_Exception_Message = "Log_CreateDoubtFulDebtByDebitNote_CreateCall_Exception_Message";
        public const string Log_SaveDoubtFulDebtAllocationReset_Jornal_Exception_Message = "Log_SaveDoubtFulDebtAllocationReset_Jornal_Exception_Message";


    }
}
