using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Infra
{
    public static  class WithdrawalLoggingValidation
    {
        public const string Enter_into_create_method_of_withdrawal="Enter into create method of withdrawal";
        public const string Validating_withdrawal_and_going_to_enter_FillWithdrawalModel_method="Validating withdrawal and going to enter FillWithdrawalModel method";
        public const string Entered_into_SaveWithdrawal_method="Entered into SaveWithdrawal method";
        public const string Checking_all_validations="Checking all validations";
        public const string Validation_checking_finished="Validation checking finished";
        public const string Validating_withdrawal_entity_in_edit_mode="Validating withdrawal entity in edit mode";
        public const string Going_to_execute_InsertWithdrawal_method="Going to execute InsertWithdrawal method";
        public const string Going_to_execute_UpdateWithdrwalDetail_method="Going to execute UpdateWithdrwalDetail method";
        public const string Going_execute_UpdateWithDrawalGSTDetail_method="Going to execute UpdateWithDrawalGSTDetail method";
        public const string Going_to_execute_InsertWithdrawal_method_in_insert_new_mode="Going to execute InsertWithdrawal method in insert new mode";
        public const string Going_to_execute_FillWithdrawalDetail_method="Going to execute FillWithdrawalDetail method";
        public const string Executing_GstDetail="Executing GstDetail";
        public const string Going_to_execute_FillGstDetail_private_method="Going to execute FillGstDetail private method";
        public const string Validating_List_of_Withdrawal="Validating List of Withdrawal";
        public const string Executing_auto_number_method="Executing auto number method";
        public const string SaveChanges_method_execution_happened="SaveChanges method execution happened";
        public const string Going_to_execute_EventStore_process="Going to execute EventStore process";
        public const string EventStore_process_done="EventStore process done";
        public const string End_of_the_SaveWithdrawal_method="End of the SaveWithdrawal method";
        public const string Entered_into_SaveWithdrawalDocumentVoid_method="Entered into SaveWithdrawalDocumentVoid method";
        public const string Validating_model_and_proceed_towards_the_functional_validation="Validating model and proceed towards the functional validation";
        public const string Functionality_validation_going_on="Functionality validation going on";
        public const string End_of_the_Functionality_validation="End of the Functionality validation";
        public const string SaveChanges_method_execution_happened_in_void_method="SaveChanges method execution happened in void method";
        public const string Entered_into_FillWithdrawalModel_method="Entered into FillWithdrawalModel method";
        public const string Validating_COA="Validating COA";
        public const string Validating_GSTSettings="Validating GSTSettings";
        public const string Validating_Company="Validating Company";
        public const string Exited_from_FillWithdrawalModel_method="Exited from FillWithdrawalModel method";
        public const string Entered_into_FillNewWithdrawalModel_method="Entered into FillNewWithdrawalModel method";
        public const string Exited_from_FillNewWithdrawalModel_method="Exited from FillNewWithdrawalModel method";
        public const string Entered_into_FillWithDrawalDetail_method="Entered into FillWithDrawalDetail method";
        public const string Exited_from_FillWithDrawalDetail_method="Exited from FillWithDrawalDetail method";
        public const string Entered_into_GetNewWithdrawalDocNumber_method="Entered into GetNewWithdrawalDocNumber method";
        public const string Exited_from_GetNewWithdrawalDocNumber_method="Exited from GetNewWithdrawalDocNumber method";
        public const string Entered_into_FillGstDetail_method="Entered into FillGstDetail method";
        public const string Exited_from_FillGstDetail_method="Exited from FillGstDetail method";
        public const string Entered_into_InsertWithdrawal_method="Entered into InsertWithdrawal method";
        public const string Exited_from_InsertWithdrawal_method="Exited from InsertWithdrawal method";
        public const string Entered_into_UpdateWithdrwalDetail_method_and_checking_the_conditions="Entered into UpdateWithdrwalDetail method and checking the conditions";
        public const string Exited_from_UpdateWithdrwalDetail_method="Exited from UpdateWithdrwalDetail method ";
        public const string Entered_into_FillWithdrawalDetail_private_method="Entered into FillWithdrawalDetail private method";
        public const string Exited_from_FillWithdrawalDetail_private_method="Exited from FillWithdrawalDetail private method";
        public const string Entered_into_UpdateWithDrawalGSTDetail_private_method="Entered into UpdateWithDrawalGSTDetail private method";
        public const string Exited_from_UpdateWithDrawalGSTDetail_method="Exited from UpdateWithDrawalGSTDetail method";
        public const string Entered_into_FillGstDetail_private_method="Entered into FillGstDetail private method";
        public const string Exited_from_FillGstDetail_private_method="Exited from FillGstDetail private method";
        public const string Entered_into_GetWithdrawalAutoNumber_method="Entered into GetWithdrawalAutoNumber method";
        public const string Exited_from_GetWithdrawalAutoNumber_private_method="Exited from GetWithdrawalAutoNumber private method";
        public const string Entered_into_FillAutoNumberModel_private_method="Entered into FillAutoNumberModel private method";
        public const string Exited_from_FillAutoNumberModel_private_method="Exited from FillAutoNumberModel private method";
        public const string Come_Out_From_GenerateAutoNumberForType_Of_Payment_Method = "Come Out From GenerateAutoNumberForType Of Payment Method";
        public const string Entered_Into_GenerateAutoNumberForType_Of_Payment = "Entered Into GenerateAutoNumberForType Of Payment";
        public const string BankWithdrawalApplicationService = "BankWithdrawalApplicationService"; 
        public const string Execute_getetity_lookup = "Execute entity lookup";
        public const string Out_from_getetity_lookup = "Out from entity lookup";
        public const string Execute_withdrawals_lookup_method = "Execute withdrawals lookup method";
        public const string Out_fom_withdrawals_lookup_method = "Out from withdrawals lookup method";
        public const string Execute_withdrawal_Kendo_call = "Execute withdrawal Kendo call";
        public const string Come_out_from_withdrawal_Kendo_call = "Come out from withdrawal Kendo call";
        public const string Execute_deposit_Kendo_call = "Execute deposit Kendo call";
        public const string Execute_cash_paymewnt_Kendo_call = "Execute cash payment Kendo call";
        public const string BankWithdrawalController = "BankWithdrawalController";
    }
}
