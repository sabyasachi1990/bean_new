using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    public static class Constant
    {
        #region ModuleMasterNames
        public const string ClientCursor = "Client Cursor";
        public const string WorkflowCursor = "Workflow Cursor";
        public const string AuditCursor = "Audit Cursor";
        public const string BeanCursor = "Bean Cursor";
        public const string KnowledgeCursor = "Knowledge Cursor";
        public const string DocCursor = "Doc Cursor";
        public const string TaxCursor = "Tax Cursor";
        public const string HRCursor = "HR Cursor";
        public const string BRCursor = "BR Cursor";
        public const string AdminCursor = "Admin Cursor";
        public const string Analytics = "Analytics";
        public const string SuperUser = "Super User";
        #endregion ModuleMasterNames


		#region AllPemissions
		public const string ViewAndAdd = "View,Add";
		public const string AddandEdit = "Add,Edit";
		public const string ViewandEdit = "View,Edit";
		public const string ViewandAddandEdit = "View,Add,Edit";
		public const string DisableAndDelete = "Delete,Disable";
        public const string EnableAndDisable = "Enable,Disable";


        public const string View = "View";
		public const string Add = "Add";
		public const string Edit = "Edit";
		public const string Disable = "Disable";
		public const string Delete = "Delete";
		public const string Copy = "Copy";
		public const string Quotation = "Quotation";
		public const string Void = "Void";
		public const string AddAllocation = "AddAllocation";
		public const string AddApplication = "AddApplication";
		public const string AddDoubtFulDebit = "AddDoubtFulDebit";
		public const string AddCreditMemo = "AddCreditMemo";
		public const string AddCreditNote = "AddCreditNote";
		public const string Reverse = "Reverse";
		public const string RevisedQuotation = "Revised Quotation";
		public const string AccountIncharge = "AccountIncharge";
		public const string OpportunityIncharge = "OpportunityIncharge";
		public const string State = "State";
		public const string RecruitmentAdmin = "RecruitmentAdmin";
		public const string LeaveApprover = "LeaveApprover";
		public const string LeaveRecommender = "LeaveRecommender";
		public const string Print = "Print";
		public const string AttendanceManager = "AttendanceManager";
		public const string HRManager = "HRManager";
		public const string CaseInCharge = "CaseInCharge";
		public const string Scheduler = "Scheduler";
		public const string SaveLayout = "Save Layout";
		public const string ADOC = "ADOC";
		public const string ViewSchedule = "ViewSchedule";
		public const string CaseScheduler = "CaseScheduler";
		public const string CaseState = "CaseState";
		public const string CaseCancel = "CaseCancel";
		public const string OnHold = "OnHold";
		public const string Cancel = "Cancel";
		public const string Approved = "Approved";
		public const string Rejected = "Rejected";
		public const string Recommended = "Recommended";
		public const string TimeLogManager = "TimeLogManager";
		public const string ManageLeadsheet = "ManageLeadsheet";
		public const string UploadTrialBalance = "Upload TrialBalance";
		public const string UploadGeneralLedger = "Upload General Ledger";
		public const string Reconcile = "Reconcile";
		public const string AllLeadsheets = "All Leadsheets";
		public const string GeneratePDF = "GeneratePDF";
		public const string DocRepository = "DocRepository";
		public const string Prepared = "Prepared";
		public const string Reviewer = "Reviewer";
		public const string Approval = "Approval";
		public const string DeleteDocument = "DeleteDocument";
		public const string CancelAdjustment = "CancelAdjustment";
		public const string Likelihood = "Likelihood";
		public const string ConvertToEmployee = "ConvertToEmployee";
		public const string ViewLeadsheet = "ViewLeadsheet";
		public const string Tax = "Tax";
		public const string Technical = "Technical";
		public const string ViewAll = "ViewAll";
		public const string PayrollManager = "Payroll Manager";
		public const string DefaultWorkProfile = "DefaultWorkProfile";
		public const string Approve = "Approve";
		public const string GenerateOrSend = "GenerateOrSend";
		public const string InvoiceStatus = "InvoiceStatus";
		public const string Verifier = "Verifier";
		public const string ChangeTrainingStatus = "ChangeTrainingStatus";
		public const string HandlingOfficer = "Handling Officer";
		public const string SendEmail = "SendEmail";
		public const string Lost = "Lost";
		public const string AppraisalAdmin = "AppraisalAdmin";
		public const string Preparer = "Preparer";
		public const string Approver = "Approver";
		public const string ApproveCancel = "ApproveCancel";
		public const string Status = "Status";
		public const string Inactive_Tab = "Inactive_Tab";
		public const string Lost_Tab = "Lost_Tab";
		public const string Preview = "Preview";
		public const string ChangeApprisalStatus = "ChangeApprisalStatus";
		public const string Download = "Download";
		#endregion AllPemissions

		#region Bean_ScreenNames
		public const string Invoices = "Invoices";
		public const string Cashsales = "Cash sales";
		public const string DebitNotes = "Debit Notes";
		public const string CreditNotes = "Credit Notes";
		public const string ProvisionforDoubtfulDebts = "Provision for Doubtful Debts";
		public const string Receipts = "Receipts";
		public const string Bills = "Bills";
		public const string CreditMemos = "Credit Memos";
		public const string Payments = "Payments,Payroll Payments";
		public const string CashPayments = "Cash Payments";
		public const string Deposits = "Deposits";
		public const string Withdrawals = "Withdrawal,Deposits,Cash Payments";
		public const string BankReconciliation = "Bank Reconciliation";
		public const string Journals = "Journals";
		public const string Clearing = "Clearing";
		public const string GLaccount = "GL account";
		public const string Revaluation = "Revaluation";
		public const string Entities = "Entities";
		public const string Items = "Items";
		public const string ExchangeRates = "Exchange Rates";
		public const string ChartofAccounts = "Chart of Accounts";
		public const string GeneralSettings = "General Settings";
		public const string TaxCodes = "Tax Codes";
		public const string LinkedAccounts = "Linked Accounts";
		public const string Transfers = "Transfers";
		public const string Reports = "Reports";
		public const string OpeningBalance = "Opening Balance";
		public const string DashBoards = "DashBoards";
        public const string Financial = "Financial";
        public const string MultiCurrency = "MultiCurrency";
        public const string SegmentReporting = "SegmentReporting";



		public const string TransfersandJournals = "Transfers,Journals";
		public const string ChartofAccountsAndOpenningbalance = "Chart of Accounts,Opening Balance";
        public const string GeneralSettings1 = "General Settings,";

		public const string DepositsandJournals = "Deposits,Journals";

		#endregion Bean_ScreenNames 


        public const string RoleUnAuthorizedExceptionMessage = "You are not authorized.Please contact Administrator.";
		public const string Admin_Getuserpermissionsfortoolbar = "/api/user/getuserpermissionsfortoolbar";
		public const string AdminUrl = "AdminUrl";
		public const string UserName = "userName";
		public const string CompanyId = "companyId";
		public const string ScreenName = "screenName";
		public const string PermissionName = "permissionName";
		public const string ModuleMasterName = "moduleMasterName";
		public const string False = "false";
        public const string PermissionType = "PermissionType";

        public const string True = "true";
		public const string ScreenPermissionEnable = "ScreenPermissionEnable";
		public const string AuthInformation = "AuthInformation";
		public const string SecureInfo = "SecureInfo";
		public const string UnAuthorized = "Un Authorized";

		public const string ModuleDetailId = "moduleDetailId";
		public const string GroupName = "GroupName";
		public const string ParentScreenName = "ParentScreenName";


        //public const string PermissionName = "permissionName";
        //public const string CompanyId = "companyId";
        //public static readonly string ModuleDetailId = "ModuleDetailId";
        //public const string UserName = "userName";
        //public const string ApiName = "api/user/";
        //public const string getuserpermissionexist = "getuserpermissionexist";
        //public const string False = "false";
        //public const string RoleUnAuthorizedExceptionMessage = "Your not authorized.Please contact Administrator.";


        public const string ApiName = "api/user/";
        public const string getuserpermissionexist = "getuserpermissionexist";

    }
}