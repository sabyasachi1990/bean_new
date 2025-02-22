﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Infra
{
    public static class JournalLoggingValidation
    {
        public const string Enter_into_SaveJournal_method="Enter into SaveJournal method";
        public const string Enter_into_if_condition_of_Journal_and_check_Journal_is_null_or_not="Enter into if condition of Journal and check Journal is null or not";
        public const string InsertJournal_method_came="InsertJournal method came";
        public const string Come_out_from_InsertJournal_method="Come out from InsertJournal method";
        public const string Enter_in_if_condition_and_check_Tobject_IsPosted="Enter in if condition and check Tobject.IsPosted";
        public const string UpdateJournalDetails_method_came="UpdateJournalDetails method came";
        public const string Come_out_from_UpdateJournalDetails_method="Come out from UpdateJournalDetails method";
        public const string UpdateJournalGSTDetails_method_came="UpdateJournalGSTDetails method came";
        public const string Come_out_from_UpdateJournalGSTDetails_method="Come out from UpdateJournalGSTDetails method";
        public const string Calling_Update_method_through_JournalService="Calling Update method through JournalService";
        public const string Enter_into_InsertJournal_method="Enter into InsertJournal method";
        public const string Enter_into_if_condition_and_checking_IsRecurringJournal_is_true_or_not="Enter into if condition and checking IsRecurringJournal is true or not";
        public const string Enter_into_if_condition_and_checking_IsAutoReversalJournal_is_true_or_not="Enter into if condition and checking IsAutoReversalJournal is true or not";
        public const string Enter_into_if_condition_and_checking_IsGstSettings_is_present_or_not="Enter into if condition and checking IsGstSettings is present or not";
        public const string Check_GetModuleStatus_method_through_CompanySettingService_and_store_into_IsSegmentReporting="Check GetModuleStatus method through CompanySettingService and store into IsSegmentReporting";
        public const string Enter_into_if_condition_and_checking_IsSegmentReporting_is_present_or_not = "Enter into if condition and checking IsSegmentReporting is present or not";
        public const string Enter_into_if_condition_and_checking_ReversalDate_is_greater_than_DocDate_or_not="Enter into if condition and checking ReversalDate is greater than DocDate or not";
        public const string End_of_the_InsertJournal_method="End of the InsertJournal method";
        public const string Enter_into_foreach_loop_of_UpdateJournalDetails_method="Enter into foreach loop of UpdateJournalDetails method";
        public const string Enter_into_if_condition_and_checking_RecordStatus_is_added_or_not="Enter into if condition and checking RecordStatus is added or not";
        public const string Enter_into_else_if_condition_and_checking_RecordStatus_is_added_and_deleted="Enter into else-if condition and checking RecordStatus is added and deleted";
        public const string Enter_into_if_condition_and_checking_journalDetail_is_null_or_not="Enter into if condition and checking journalDetail is null or not";
        public const string Enter_into_else_if_condition_and_checking_RecordStatus_is_deleted_or_not="Enter into else-if condition and checking RecordStatus is deleted or not";
        public const string Check_in_through_JournalDetail_and_store_into_journalDetail="Check in through JournalDetail and store into journalDetail";
        public const string Check_in_JournalDetail_is_null_or_not="Check in JournalDetail is null or not";
        public const string Enter_into_UpdateJournalGSTDetails_method="Enter into UpdateJournalGSTDetails method";
        public const string Enter_into_foreach_loop_of_JournalGSTDetail="Enter into foreach loop of JournalGSTDetail";
        public const string Enter_into_if_condition_of_JournalGSTDetail_and_checking_RecordStatus_is_Added_or_not="Enter into if condition of JournalGSTDetail and checking RecordStatus is Added or not";
        public const string Enter_into_else_if_condition_of_JournalGSTDetail_and_checking_RecordStatus_is_Added_and_Deleted="Enter into else-if condition of JournalGSTDetail and checking RecordStatus is Added and Deleted";
        public const string Checking_in_GetJournalGSTDetailsById_method="Checking in GetJournalGSTDetailsById method";
        public const string Checking_in_journalGSTDetail_null_or_not="Checking in journalGSTDetail null or not";
        public const string Checking_in_RecordStatus_deleted_or_not="Checking in RecordStatus deleted or not";
        public const string Checking_in_journalGSTDetail_null_or_not_in_if_condition="Checking in journalGSTDetail null or not in if condition";
        public const string Enter_into_else_condition_of_journal="Enter into else condition of journal";
        public const string Get_Company_through_GetById_method="Get Company through GetById method";
        public const string Get_SystemReferenceNo_through_GenerateAutoNumberForType_method="Get SystemReferenceNo through GenerateAutoNumberForType method";
        public const string Enter_into_if_condition_and_checking_JournalDetailModels_count_O_or_nul="Enter into if condition and checking JournalDetailModels count O or null";
        public const string Enter_into_foreach_loop_of_JournalDetailModels="Enter into foreach loop of JournalDetailModels";
        public const string FillJournalDetailmodel_method_came="FillJournalDetailmodel method came";
        public const string Come_out_from_FillJournalDetailmodel_method="Come out from FillJournalDetailmodel method";
        public const string Enter_into_if_condition_and_checking_JournalGSTDetails_is_null_or_not="Enter into if condition and checking JournalGSTDetails is null or not";
        public const string Enter_into_foreach_loop_of_JournalGSTDetails="Enter into foreach loop of JournalGSTDetails";
        public const string Entering_into_try_block_and_execute_the_SaveChanges_method="Entering into try block and execute the SaveChanges method";
        public const string Event_Store_is_going_to_start="Event Store is going to start";
        public const string Event_Store_process_ended="Event Store process ended";
        public const string Catching_the_exception_thru_DbEntityValidationException_class_in_catch_block="Catching the exception thru DbEntityValidationException class in catch block";
        public const string End_of_the_aveJournal_process="End of the Save Journal process";
        public const string Entered_into_SaveReversal_method="Entered into SaveReversal method";
        public const string Checked_through_GetJournalById_method_and_store_in_Journal="Checked through GetJournalById method and store in Journal";
        public const string Enter_into_if_condition_of_SaveReversal_and_check_journal_null_or_not="Enter into if condition of SaveReversal and check journal null or not";
        public const string FillJournal_method_came="FillJournal method came";
        public const string Come_out_from_FillJournal_method="Come out from FillJournal method";
        public const string Entered_into_if_condition_and_checking_ReverseParentRefId_is_null_or_not="Entered into if condition and checking ReverseParentRefId is null or not";
        public const string Got_DocNo_thru_GetJournalNewDocumentNo1_method="Got DocNo thru GetJournalNewDocumentNo1 method";
        public const string Got_DocumentDescription_thru_GetJournalNewDocumentNo1_method="Got DocumentDescription thru GetJournalNewDocumentNo1 method";
        public const string Entered_into_FillJournal_method="Entered into FillJournal method";
        public const string Entered_into_If_condition_and_checking_company_null_or_not="Entered into If condition and checking company null or not";
        public const string FillJournal_method_ended="FillJournal method ended";
        public const string Got_List_of_JournalDetail_thru_GetAllJournalDetailsByid="Got List of JournalDetail thru GetAllJournalDetailsByid";
        public const string Entered_into_foreach_loop="Entered into foreach loop";
        public const string FillJournalDetail_method_came="FillJournalDetail method came";
        public const string Come_out_from_FillJournalDetail_method="Come out from FillJournalDetail method";
        public const string Got_List_of_JournalGSTDetail_thru_GetAllJournalGSTDetails="Got List of JournalGSTDetail thru GetAllJournalGSTDetails";
        public const string Entered_into_foreach_loop_JournalGSTDetails="Entered into foreach loop JournalGSTDetails";
        public const string FillGSTDetails_method_came="FillGSTDetails method came";
        public const string Come_out_from_FillGSTDetails_method="Come out from FillGSTDetails method";
        public const string Enter_into_try_block_and_will_execute_the_SaveChanges_method_of_SaveReversal="Enter into try block and will execute the SaveChanges method of SaveReversal";
        public const string End_of_the_SaveReversal="End of the SaveReversal";
        public const string Entered_into_FillGSTDetails_method="Entered into FillGSTDetails method";
        public const string End_of_FillGSTDetails_method="End of FillGSTDetails method";
        public const string Entered_into_FillJournalDetail_method="Entered into FillJournalDetail method";
        public const string End_of_FillJournalDetail_method="End of FillJournalDetail method";
        public const string Entered_into_FillJournalDetailmodel_method="Entered into FillJournalDetailmodel method";
        public const string Entered_into_if_condition_of_FillJournalDetailmodel_and_check_tax_null_or_not="Entered into if condition of FillJournalDetailmodel and check tax null or not";
        public const string Entered_into_if_condition_of_FillJournalDetailmodel_and_check_coa_null_or_not="Entered into if condition of FillJournalDetailmodel and check coa null or not";
        public const string End_of_the_FillJournalDetailmodel="End of the FillJournalDetailmodel";
        public const string Enter_into_try_block_and_will_execute_the_SaveChanges_method_of_SaveCopy = "Enter into try block and will execute the SaveChanges method of SaveCopy";
        public const string Entered_into_SaveCopy_method="Entered into SaveCopy method";
        public const string Got_the_value_from_GetJournalById_and_saveed_in_journal="Got the value from GetJournalById and saveed in journal";
        public const string Entered_into_if_block_of_SaveCopy_and_check_journal_is_null_or_not="Entered into if block of SaveCopy and check journal is null or not";
        public const string Got_the_data_thru_GetById_and_save_in_Journal = "Got the data thru GetById and save in Journal";
        public const string Got_the_StstemRefNo_thru_GenerateAutoNumberForType_method="Got the StstemRefNo thru GenerateAutoNumberForType method";
        public const string Entered_into_if_block_and_check_IsCopy_true_or_not="Entered into if block and check IsCopy true or not";
        public const string Got_the_Doc_no_thru_GetJournalNewDocumentNo1_method="Got the Doc no thru GetJournalNewDocumentNo1 method";
        public const string Got_the_DocumentDescription_thru_GetJournalNewDocumentNo1_method="Got the DocumentDescription thru GetJournalNewDocumentNo1 method";
        public const string Got_the_ListJournalDetail_thru_GetAllJournalDetailsByid_method="Got the ListJournalDetail thru GetAllJournalDetailsByid method";
        public const string Entered_into_foreach_loop_journal_detail="Entered into foreach loop journal detail";
        public const string Out_from_FillJournalDetail_method="Out from FillJournalDetail method";
        public const string Got_the_ListJournalGSTDetail_thru_GetAllJournalGSTDetails_method="Got the ListJournalGSTDetail thru GetAllJournalGSTDetails method";
        public const string Entered_into_foreach_loop_of_JournalGSTDetail="Entered into foreach loop of JournalGSTDetail";
        public const string End_of_the_SaveCopy_method="End of the SaveCopy method";
        public const string Entered_into_SaveJournalVoid_method="Entered into SaveJournalVoid method";
        public const string Got_the_data_thru_GetJournalById_method_and_save_in_journal="Got the data thru GetJournalById method and save in journal";
        public const string Entered_into_try_block_and_will_execute_SaveChanges_method_of_SaveJournalVoid="Entered into try block and will execute SaveChanges method of SaveJournalVoid";
        public const string End_of_the_SaveJournalVoid_method="End of the SaveJournalVoid method";
        public const string Entered_into_SavePostCallforParked_method="Entered into SavePostCallforParked method";
        public const string Got_the_data_thru_GetJournalById_and_save_in_getJournal="Got the data thru GetJournalById and save in getJournal";
        public const string Entered_into_if_block_and_check_getJournal_is_null_or_not="Entered into if block and check getJournal is null or not";
        public const string Entered_into_try_block_and_execute_SaveChanges_method_of_SavePostCallforParked="Entered into try block and execute SaveChanges method of SavePostCallforParked";
        public const string End_of_the_SavePostCallforParked_method="End of the SavePostCallforParked method";
        public const string Entered_into_FillJournalModel_method="Entered into FillJournalModel method";
        public const string Get_company_thru_ServiceCompanyId="Get company thru ServiceCompanyId";
        public const string Entered_into_if_block_and_check_company_is_null_or_not="Entered into if block and check company is null or not";
        public const string Entered_into_if_block_and_checking_IsAutoReversalJournal_is_true_or_not="Entered into if block and checking IsAutoReversalJournal is true or not";
        public const string Entered_into_if_block_and_checking_SegmentMasterid1_is_null_or_not="Entered into if block and checking SegmentMasterid1 is null or not";
        public const string Entered_into_if_block_and_checking_SegmentMasterid2_is_null_or_not = "Entered into if block and checking SegmentMasterid2 is null or not";
        public const string Got_the_JournalDetail_thru_GetAllJournalDetailsByid_method="Got the JournalDetail thru GetAllJournalDetailsByid method";
        public const string Entered_into_foreach_loop_of_ListJournalDetail="Entered into foreach loop of ListJournalDetail";
        public const string FillJournalDetailToModel_method_came="FillJournalDetailToModel method came";
        public const string Come_out_from_FillJournalDetailToModel_method="Come out from FillJournalDetailToModel method";
        public const string End_of_the_FillJournalModel_method="End of the FillJournalModel method";
        public const string Entered_into_FillGSTDetailModel_method="Entered into FillGSTDetailModel method";
        public const string End_of_the_FillGSTDetailModel_method="End of the FillGSTDetailModel method";
        public const string Entered_into_FillJournalDetailToModel_method="Entered into FillJournalDetailToModel method";
        public const string Entered_into_if_block_of_FillJournalDetailToModel_and_check_tax_is_null_or_not="Entered into if block of FillJournalDetailToModel and check tax is null or not";
        public const string Entered_into_if_block_of_FillJournalDetailToModel_and_checking_coa_is_null_or_not="Entered into if block of FillJournalDetailToModel and checking coa is null or not";
        public const string End_of_FillJournalDetailToModel_method="End of FillJournalDetailToModel method";
        public const string Enter_into_SaveRecurringJournal_method = "Enter into SaveRecurringJournal method";
        public const string Update_Failed_In_CopyJournal_Save = "Update failed in journal copy save while updating the master counter";
    }
}
