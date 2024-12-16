using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Infra.Resources
{
    public class MasterModuleLoggingValidation
    {
        public const string Log_BeanEntity_Save_SaveEntityModel_Request_Message = "BeanEntity Save SaveEntityModel Request Message";
        public const string Entered_into_CreateEntity_Method = "Entered into CreateEntity Method";
        public const string GetSSICodeByType_Entered = "Get SSICode By Type method entered";
        public const string Entered_into_QuickCreateEntity_Method = "Entered into QuickCreateEntity Method";
        public const string Existed_from_QuickCreateEntity_Method = "Existed Form QuickCreateEntity Method";
        public const string Existed_from_CreateEntity_Method = "Existed Form CreateEntity Method";
        public const string Log_BeanEntity_Save_SaveEntityModel_UpdateRequest_Message = "BeanEntity Save SaveEntityModel UpdateRequest Message";
        public const string Log_BeanEntity_Save_SaveEntityModel_NewRequest_Message = "BeanEntity Save SaveEntityModel NewRequest Message";
        public const string Log_BeanEntity_Entered_into_fill_Method_SaveEntityModel_Request_Message = "BeanEntity Entered into fill Method SaveEntityModel Request Message";
        public const string Log_BeanEntity_Entered_into_fill_Method_Update_SaveEntityModel_Request_Message = "BeanEntity Entered into fill Method Update SaveEntityModel Request Message";
        public const string Log_BeanEntity_Save_Address_Exisist_NewRequest_Message = "BeanEntity Save Address Exisist NewRequest Message";
        public const string Log_BeanEntity_Save_Saved_Data_Request_Message = "BeanEntity Save Saved Data Request Message";
        public const string Log_BeanEntity_Save_SaveEntityModel_SuccessFully_Message = "BeanEntity Save SaveEntityModel SuccessFully Message";
        public const string Log_BeanEntity_Save_SaveEntityModel_Exception_Message = "Log BeanEntity Save SaveEntityModel Exception Message";
        public const string Log_BeanEntity_QuickEntitySave_Request_Message = "BeanEntity QuickEntitySave Request Message";
        public const string Log_BeanEntity_QuickEntitySave_SuccessFully_Message = "BeanEntity QuickEntitySave SuccessFully Message";
        public const string Log_BeanEntity_QuickEntitySave_Exception_Message = "BeanEntity QuickEntitySave Exception Message";
        public const string Log_FinancialSetting_Save_FinancialSettingModel_Request_Message = "FinancialSetting Save FinancialSettingModel Request Message";
        public const string Log_FinancialSetting_Save_FinancialSettingModel_SavedSuccesfully_Request_Message = "FinancialSetting Save FinancialSettingModel SavedSuccesfully Request Message";
        public const string Log_FinancialSetting_Save_FinancialSettingModel_multicurrency_Exisist_Request_Message = "FinancialSetting Save FinancialSettingModel multicurrency Exisist Request Message";

        public const string Log_MultiCurrencySetting_Save_MultiCurrencySettingModel_Request_Message = "MultiCurrencySetting Save MultiCurrencySettingModel Request_Message ";
        public const string Log_MultiCurrencySetting_Save_Update_Block_MultiCurrencySettingModel_Request_Message = "MultiCurrencySetting Save Update Block MultiCurrencySettingModel Request Message";
        public const string Log_MultiCurrencySetting_Save_Create_Block_MultiCurrencySettingModel_Request_Message = "MultiCurrencySetting Save Create Block MultiCurrencySettingModel Request Message";
        public const string Log_MultiCurrencySetting_Save_Revalution_Checked_True_MultiCurrencySettingModel_Request_Message = "MultiCurrencySetting Save Revalution Checked True MultiCurrencySettingModel Request Message";
        public const string Log_MultiCurrencySetting_Save_MultiCurrencySettingModel_saved_SuccessFully_Request_Message = "MultiCurrencySetting Save MultiCurrencySettingModel saved SuccessFully Request Message";
        public const string Log_GSTSetting_Save_GSTSettingModel_Request_Message = "GSTSetting Save GSTSettingModel Request Message";
        public const string Log_GSTSetting_Save_Update_GSTSettingModel_Request_Message = "GSTSetting Save Update GSTSettingModel Request Message";
        public const string Log_GSTSetting_Save_Create_GSTSettingModel_Request_Message = "GSTSetting Save Create GSTSettingModel Request Message";
        public const string Log_GSTSetting_Save_GSTSettingModel_Saved_Successfully_Request_Message = "Log_GSTSetting_Save_GSTSettingModel_Saved_Successfully Request Message";
        public const string Log_BankReconciliationSetting_Save_BankReconciliationSettingModel_Request_Message = "BankReconciliationSetting Save BankReconciliationSettingModel Request Message";
        public const string Log_BankReconciliationSetting_Save_Updated_BankReconciliationSettingModel_Request_Message = "BankReconciliationSetting Save Updated BankReconciliationSettingModel Request Message";
        public const string Log_BankReconciliationSetting_Save_Created_BankReconciliationSettingModel_Request_Message = "BankReconciliationSetting  Save Created BankReconciliationSettingModel Request Message";
        public const string Log_BankReconciliationSetting_Save__BankReconciliationSettingModel_Saved_Succefully_Request_Message = "BankReconciliationSetting Save BankReconciliationSettingModel Saved Succefully Request Message";
        public const string Log_SaveSegmentMaster_Save_SaveSegmentMasterModel_Request_Message = "SaveSegmentMaster Save SaveSegmentMasterModel Request Message";
        public const string Log_SaveSegmentMaster_Save_segmentMasterSelect_Not_equal_To_Null_Request_Message = "SaveSegmentMaster Save segmentMasterSelect Notequal_To_Null Request Message";
        public const string Log_SaveSegmentMaster_Save_segmentMaster_First_Else_If_Request_Message = "SaveSegmentMaster Save segmentMaster First Else_If Request Message";
        public const string Log_SaveSegmentMaster_Save_segmentMaster_Second_Else_If_Request_Message = "Log SaveSegmentMaster Save segmentMaster Second Else_If Request Message";
        public const string Log_SaveSegmentMaster_Save_segmentMaster_Saved_SuccessFully_Request_Message = "SaveSegmentMaster Save segmentMaster Saved_SuccessFully Request Message";
        public const string Log_BankReconciliationSetting_Save_SaveSegmentDetailModel_Request_Message = "SaveSegmentMasterDetail Save SaveSegmentDetailModel Request Message";
        public const string Log_SegmentDetail_Save_SaveSegmentDetailModel_Exissist_Request_Message = "SegmentDetail Save SaveSegmentDetailModel Exissist Request Message";
        public const string Log_SegmentDetail_Save_SaveSegmentDetailModel_New_Request_Message = "SegmentDetail Save SaveSegmentDetailModel New Request Message";
        public const string Log_SegmentDetail_Save_SaveSegmentDetailModel_Saved_SuccessFully_Request_Message = "SegmentDetail Save SaveSegmentDetailModel Saved SuccessFully Request Message";
        public const string MasterModuleApplicationService = "MasterModuleApplicationService";
        public const string Log_Into_Billngs_Receipts_Balance_Amount = "Enter Into Billngs Receipts Balance Amounts";
        public const string Log_CameOut_Billngs_Receipts_Balance_Amount = "CameOut from Billngs Receipts Balance Amounts calculation";
        public const string Issues_In_Entity_Folder_creation = "Issues in Entity Folder Creataion";
        #region MasterModule
        public const string Log_ModuleMaster_GetCustomersNewLU_Request_Message = "Log ModuleMaster GetCustomersNewLU Request Message";
        public const string Log_ModuleMaster_GetCustomersNewLU_Request_Message_Completed = "Log ModuleMaster GetCustomersNewLU Request Message Completed";
        public const string Log_ModuleMaster_GetCustomersNewLU_Ecxeption_Message = "Log ModuleMaster GetCustomersNewLU Ecxeption Message";
        #endregion
    }
}
