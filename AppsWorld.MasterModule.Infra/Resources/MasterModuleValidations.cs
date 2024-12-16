using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Infra.Resources
{
    public class MasterModuleValidations
    {
        public const string Entity_Name_Already_Exists_for_this_company = "Duplicate entity name. Verify that you’ve entered the correct information.";
        public const string Entity = "Entity";
        public const string statuschanged = "statuschanged";
        public const string Updated = "Updated";
        public const string No_Segment_Category_is_found_with_provided_details = "No Segment Category is found with provided details";
        public const string Two_Segment_Categories_already_exist_in_active_state = "Two Segment Categories already exist in active state";
        public const string Segment_Category_parent_node_is_required = "Segment Category parent node is required";
        public const string Segment_Detail_Name_Already_Exist = "Segment Detail Name Already Exist";
        public const string Segment_Category_child_node_is_required = "Segment Category child node is required";
        public const string You_Have_Already_Two_Activated_Segment_Masters = "You Have Already Two Activated Segment Masters";
        public const string Segment_Category_is_required = "Segment Category is required";
        public const string Segment_Name_Already_Exist = "Segment Name Already Exist";
        public const string Segment_Parent_Name_Already_Exist = "Segment Parent Name Already Exist";
        public const string Segment_Child_Name_Already_Exist = "Segment Child Name Already Exist";
        public const string Save_Data_Failed = "Save Data Failed";
        public const string It_is_overlapping_the_existing_document_dates = "It is overlapping the existing document dates.";
        public const string SGD = "SGD";
        public const string Empty = "";
        public const string Period_Lock_Date_Password_is_mandatory = "Period Lock Date Password is mandatory.";
        public const string Period_Lock_Date_Achieved_cannot_be_saved = "Period Lock Date Achieved, cannot be saved.";
        public const string This_ChartOfAccount_is_linked_to_Linked_Account = "You can't delete the linked account";
        public const string This_ChartOfAccount_is_linked_to_Syatem_Account = "You can't delete the system account";
        public const string This_ChartOfAccount_is_have_posting= "This account is using by some other process,you can't delete it.";
        public const string This_ChartOfAccount_is_Linked_to_Item= "This account is using by some other process,you can't disable it.";
        public const string Deleted_Successfully="Deleted Successfully";
        public const string This_ChartOfAccount_CanNotBedeleted_ThisIS_Linked_to_Entity = "This COA is linked to ";
        public const string This_chartOfAccount_have_postings_you_cantbe_be_delete = "This COA have posting's you can't be delete";
        public const string entity_you_cant_delete=" entity, can't delete";
        public const string Item_you_cant_delete=" Item, can't delete";
        public const string This_COA_is_used_in_Opening_Balance_Draft_State_you_cant_be_deleted="This COA is used in Opening Balance Draft State, can't delete";
        public const string Need_Jurisdiction_To_Save_TaxCode = "Need Jurisdiction to save Taxcode";

        public const string Entities = "Entities";
        public const string Customers = "Customers";
        public const string Set_the_fiancial_settings_in_admin_general_settings = "Please set the Financial Settings in Admin Cursor General Settings";
    }
}
