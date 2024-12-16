
using AppaWorld.Bean;
using AppsWorld.BankTransferModule.Entities;
using AppsWorld.BankTransferModule.Entities.Models;
using AppsWorld.BankTransferModule.Infra;
using AppsWorld.BankTransferModule.Infra.Resources;
using AppsWorld.BankTransferModule.Models;
using AppsWorld.BankTransferModule.RepositoryPattern;
using AppsWorld.BankTransferModule.Service;
using AppsWorld.CommonModule.Application;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.Service;
using AppsWorld.Framework;
using Domain.Events;
using Logger;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Serilog;
using ServiceStack.Templates;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ziraff.FrameWork;
using Ziraff.FrameWork.Logging;
using Ziraff.Section;

namespace AppsWorld.BankTransferModule.Application
{
	public class BankTransferApplicationService
	{
		private readonly IBankTransferService _bankTransferService;
		private readonly IFinancialSettingService _financialSettingService;
		//private readonly IBankReconciliationSettingService _bankReconciliationService;
		private readonly IChartOfAccountService _chartOfAccountService;
		private readonly ICompanyService _companyService;
		private readonly IMultiCurrencySettingService _multiCurrencyRepository;
		private readonly IAccountTypeService _accountTypeService;
		private readonly IBankTransferDetailService _bankTransferDetailService;
		//private readonly IJournalService _journalService;
		//private readonly IForexService _forexService;
		private readonly IControlCodeCategoryService _controlCodeCatService;
		private readonly AppsWorld.BankTransferModule.Service.IAutoNumberService _autoNumberService;
		//private readonly AppsWorld.BankTransferModule.Service.IAutoNumberCompanyService _autoNumberCompanyService;
		private readonly IBankTransferModuleUnitOfWorkAsync _unitOfWork;
		private readonly AppsWorld.CommonModule.Service.IAutoNumberService _autoService;
		private readonly IInvoiceService _invoiceService;
		private readonly IDebitNoteService _debitNoteService;
		private readonly IBillService _billService;
		private readonly ISettlementDetailService _settlementDetailService;
		private readonly IBeanEntityService _beanEntityService;
		private readonly BankTransferModule.Service.IJournalService _journalService;
		private readonly CommonApplicationService _commonApplicationService;
		string doc = "";
		SqlConnection con = null;
		SqlCommand cmd = null;
		SqlDataReader dr = null;
		string query = string.Empty;
		//private string ConnectionString { get; set; }

		public BankTransferApplicationService(IBankTransferService bankTransferService, IFinancialSettingService financialSettingService, /*IBankReconciliationSettingService bankReconciliationService,*/ IChartOfAccountService chartOfAccountService, ICompanyService companyService, IMultiCurrencySettingService multiCurrencyRepository, IAccountTypeService accountTypeService, IBankTransferDetailService bankTransferDetailService, /*IJournalService journalService,*/ /*IForexService forexService,*/ IControlCodeCategoryService controlCodeCatService, AppsWorld.BankTransferModule.Service.IAutoNumberService autoNumberService, /*AppsWorld.BankTransferModule.Service.IAutoNumberCompanyService autoNumberCompanyService,*/ IBankTransferModuleUnitOfWorkAsync unitOfWork, AppsWorld.CommonModule.Service.IAutoNumberService autoService, IInvoiceService invoiceService, IDebitNoteService debitNoteService, IBillService billService, ISettlementDetailService settlementDetailService, IBeanEntityService beanEntityService, BankTransferModule.Service.IJournalService journalService, CommonApplicationService commonApplicationService)
		{
			this._bankTransferService = bankTransferService;
			this._financialSettingService = financialSettingService;
			//this._bankReconciliationService = bankReconciliationService;
			this._chartOfAccountService = chartOfAccountService;
			this._companyService = companyService;
			this._multiCurrencyRepository = multiCurrencyRepository;
			this._accountTypeService = accountTypeService;
			this._bankTransferDetailService = bankTransferDetailService;
			//this._journalService = journalService;
			//this._forexService = forexService;
			this._controlCodeCatService = controlCodeCatService;
			this._autoNumberService = autoNumberService;
			//this._autoNumberCompanyService = autoNumberCompanyService;
			this._unitOfWork = unitOfWork;
			this._autoService = autoService;
			this._invoiceService = invoiceService;
			this._debitNoteService = debitNoteService;
			this._billService = billService;
			this._settlementDetailService = settlementDetailService;
			this._beanEntityService = beanEntityService;
			this._journalService = journalService;
			this._commonApplicationService = commonApplicationService;

		}

		#region Kendo Call
		public async Task<IQueryable<BankTransferModelK>> GetAllBankTransferK(string username, long companyId)
		{

			return await _bankTransferService.GetAllBankTransferKAsync(username, companyId);
		}
		#endregion

		#region Create And Lookup Call
		public BankTransferModel CreateBankTransfer(Guid Id, long companyId, string connectionString, string username)
		{
			BankTransferModel bankTransferModel = new BankTransferModel();
			FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
			if (financSettings == null)
			{
				throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
			}
			bankTransferModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
			bankTransferModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
			try
			{

				BankTransfer bankTransfer = _bankTransferService.GetBankTransferById(Id, companyId);
				Dictionary<long, string> lstCompanies = null;
				//bankTransfer.BankTransferDetails = _bankTransferDetailService.GetBankTransfeById(id);
				//List<BankTransferDetailModel> listBankTransferDetailModel = new List<BankTransferDetailModel>();
				//var bank = _bankReconciliationService.GetByCompanyId(companyId);
				//if (bank != null)
				//    bankTransferModel.IsBankReconciliation = true;

				#region For BT_Settlement
				Dictionary<string, bool> ICData = new Dictionary<string, bool>();
				using (con = new SqlConnection(connectionString))
				{
					query = $"Select IC.InterCompanyType as ICType,IC.IsInterCompanyEnabled as ICEnabled from Bean.InterCompanySetting IC where CompanyId={companyId}";
					if (con.State != System.Data.ConnectionState.Open)
						con.Open();
					cmd = new SqlCommand(query, con);
					dr = cmd.ExecuteReader();
					while (dr.Read())
					{
						ICData.Add(dr["ICType"].ToString(), Convert.ToBoolean(dr["ICEnabled"]));
					}
					if (con.State != System.Data.ConnectionState.Closed)
						con.Close();

				}
				if (ICData.Any())
				{
					bankTransferModel.IsClearing = ICData.Any(c => c.Key == "Clearing" && c.Value == true);
					bankTransferModel.IsIB = ICData.Any(c => c.Key == "Billing" && c.Value == true);
				}
				#endregion For BT_Settlement

				if (bankTransfer == null)
				{
					//AppsWorld.BankTransferModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.BankTransfer);
					BankTransfer listbankTransfer = _bankTransferService.GetCompanyIdById(companyId);
					bankTransferModel.Id = Guid.NewGuid();
					bankTransferModel.CompanyId = companyId;
					bankTransferModel.TransferDate = listbankTransfer == null ? DateTime.Now : listbankTransfer.TransferDate;
					//bankTransferModel.DocNo = GetNewBankTransferDocumentNumber(DocTypeConstants.BankTransfer, companyId);

					bankTransferModel.IsDocNoEditable = _autoService.GetAutoNumberIsEditable(companyId, DocTypeConstants.BankTransfer);
					if (bankTransferModel.IsDocNoEditable == true)
						bankTransferModel.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstants.BankTransfer, connectionString);

					//bool? isEdit = false;
					//bankTransferModel.DocNo = GetAutoNumberByEntityType(companyId, listbankTransfer, DocTypeConstants.BankTransfer, _autoNo, ref isEdit);
					//bankTransferModel.IsDocNoEditable = isEdit;

					//if (bank != null)
					//bankTransferModel.BankClearingDate = _bankReconciliationService.GetByCompanyDate(companyId);
					bankTransferModel.CreatedDate = DateTime.UtcNow;
					bankTransferModel.DocType = DocTypeConstants.BankTransfer;
					bankTransferModel.Status = RecordStatusEnum.Active;

					//if (listbankTransfer != null)
					//    if (listbankTransfer.VarianceExchangeRate != null)
					//    {
					//        var deci = Convert.ToDecimal(listbankTransfer.VarianceExchangeRate);
					//        bankTransferModel.VarianceExchangeRate = deci + "%";
					//    }
					//bankTransferModel.VarianceExchangeRate = listbankTransfer.VarianceExchangeRate + '%';
					//Forex forexBean;
					//forexBean = _forexService.GetMultiCurrencyInformation(bankTransferModel.ExCurrency, bankTransferModel.TransferDate, true, bankTransferModel.CompanyId);
					//if (forexBean != null)
					//{
					//    bankTransferModel.ExchangeRate = forexBean.UnitPerUSD;
					//    bankTransferModel.ExDurationFrom = forexBean.DateFrom;
					//    bankTransferModel.ExDurationTo = forexBean.Dateto;
					//}
					List<BankTransferDetailModel> lstdetail = new List<BankTransferDetailModel>();
					BankTransferDetailModel detail = new BankTransferDetailModel();
					detail.Id = Guid.NewGuid();
					detail.Type = "Withdrawal";
					lstdetail.Add(detail);
					detail = new BankTransferDetailModel();
					detail.Id = Guid.NewGuid();
					detail.Type = "Deposit";
					lstdetail.Add(detail);
					bankTransferModel.BankTransferDetailsModel = lstdetail;
				}
				else
				{
					//foreach (var item in bankTransfer.BankTransferDetails)
					//{
					//    if (!_companyService.GetPermissionBasedOnUser(item.ServiceCompanyId, bankTransfer.CompanyId,username))
					//        throw new Exception(CommonConstant.Access_denied);
					//}
					FillBankTransfer(bankTransferModel, bankTransfer);
					bankTransferModel.IsDocNoEditable = _autoService.GetAutoNumberIsEditable(companyId, DocTypeConstants.BankTransfer);
					lstCompanies = _companyService.GetAllSubCompany(username, companyId);
					//var account = _accountTypeService.GetById(companyId, AccountNameConstants.Cash_and_bank_balances);
					//List<ChartOfAccount> lstcoa = _chartOfAccountService.GetCashAndBankCOAId(companyId, account.Id);
					//List<Company> lstcom = _companyService.GetlstCompany(companyId);
					//List<long> lstCoaiIds = bankTransfer.BankTransferDetails.Select(a => a.COAId).ToList();
					bankTransferModel.BankTransferDetailsModel = (from btd in bankTransfer.BankTransferDetails
																	  //join coa in _chartOfAccountService.Queryable() on btd.COAId equals coa.Id
																	  //join at in _accountTypeService.Queryable() on coa.AccountTypeId equals at.Id
																	  //join com in _companyService.Queryable() on btd.ServiceCompanyId equals com.Id
																	  //where at.CompanyId == companyId && at.Name == AccountNameConstants.Cash_and_bank_balances && at.Status == RecordStatusEnum.Active
																  select new BankTransferDetailModel
																  {
																	  Id = btd.Id,
																	  Amount = btd.Amount,
																	  BankClearingDate = btd.BankClearingDate,
																	  BankTransferId = btd.BankTransferId,
																	  Currency = btd.Currency,
																	  COAId = btd.COAId,
																	  ServiceCompanyId = btd.ServiceCompanyId,
																	  IsHyperLinkEnable = lstCompanies.Any() ? lstCompanies.Where(c => c.Key == btd.ServiceCompanyId).Any() ? true : false : false,
																	  Type = btd.Type,
																	  RecOrder = btd.RecOrder,
																	  ClearingState = btd.ClearingState
																  }).ToList();
					//listCashsaleDetailModel.Add(BankTransferDetailsModel);
					bankTransferModel.BankTransferDetailsModel = bankTransferModel.BankTransferDetailsModel.OrderBy(c => c.RecOrder).ToList();
					string DocuNo = _commonApplicationService.StringCharactersReplaceFunction(bankTransfer.DocNo);
					//     bankTransfer.DocNo.Replace('"', '_').Replace('\\', '_').Replace('/', '_')
					//.Replace(':', '_').Replace('|', '_').Replace('<', '_').Replace('>', '_').Replace('*', '_').Replace('?', '_').Replace("'", "_");
					bankTransferModel.Path = bankTransfer.DocType + "s" + "/" + DocuNo;

					bankTransferModel.SettlementDetailModels = bankTransfer.SettlementDetails.Any() ? bankTransfer.SettlementDetails.Select(a => new SettlementDetailModel()
					{
						Id = a.Id,
						BankTransferId = bankTransfer.Id,
						DocumentNo = a.DocumentNo,
						DocumentDate = a.DocumentDate,
						DocumentType = a.DocumentType,
						ServiceCompanyId = a.ServiceCompanyId,
						IsHyperlinkEnable = lstCompanies.Any() ? lstCompanies.Where(c => c.Key == a.ServiceCompanyId).Any() ? true : false : false,
						DocumentState = a.DocumentState,
						DocumentId = a.DocumentId,
						Nature = "Interco",
						Currency = a.Currency,
						SettlemetType = a.SettlemetType,
						DocumentAmmount = a.DocumentAmmount,
						AmmountDue = a.AmmountDue,
						SettledAmount = a.SettledAmount,
						ExchangeRate = a.ExchangeRate,
						RecOrder = a.RecOrder
					}).OrderBy(a => a.DocumentDate).ThenBy(c => c.DocumentNo).ToList() : null;






					//foreach (var detail in bankTransfer.BankTransferDetails)
					//{
					//    BankTransferDetailModel banktransferDetailModel = new BankTransferDetailModel();
					//    FillBankTransferDetail(banktransferDetailModel, detail);
					//    listBankTransferDetailModel.Add(banktransferDetailModel);
					//}
					// bankTransferModel.BankTransferDetailsModel = listBankTransferDetailModel;
					//bankTransferModel.BankTransferDetails = listBankTransferDetailModel.OrderBy(c => c.RecOrder).ToList();
					//if (bankTransfer.DocumentState != BanktransferStatus.Void)
					//{
					//    //if (bankTransferModel.BankTransferDetailsModel.Any())
					//    //{
					//    //    if (bankTransferModel.BankTransferDetailsModel.Where(a => a.BankClearingDate == null).Any() == true)
					//    //    {
					//    //        bankTransferModel.DocumentState = "Created";
					//    //    }
					//    //    else
					//    //        bankTransferModel.DocumentState = bankTransfer.DocumentState;
					//    //}

					//    //List<Journal> lstJournal = _journalService.GetLstJournal(companyId, Id);
					//    //List<JVViewModel> lstViewModel = new List<JVViewModel>();
					//    //if (lstJournal.Any())
					//    //{
					//    //    foreach (var journal in lstJournal)
					//    //    {
					//    //        JVViewModel viewM = new JVViewModel();
					//    //        viewM.Id = journal.Id;
					//    //        viewM.DocType = journal.DocType;
					//    //        viewM.SystemReferenceNo = journal.SystemReferenceNo;
					//    //        lstViewModel.Add(viewM);
					//    //    }
					//    //}

					//    List<JVViewModel> lstViewModel = (from journal in _journalService.Queryable()
					//                                      where journal.CompanyId == companyId && journal.Id == Id
					//                                      select new JVViewModel()
					//                                      {
					//                                          Id = journal.Id,
					//                                          DocType = journal.DocType,
					//                                          SystemReferenceNo = journal.SystemReferenceNo

					//                                      }).ToList();

					//    bankTransferModel.JVViewModels = lstViewModel.OrderBy(x => x.SystemReferenceNo).ToList();
					//}

				}
				//bankTransferModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
				//bankTransferModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
				//bankTransferModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
				//bankTransferModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
			}
			catch (Exception ex)
			{
				LoggingHelper.LogError(BankTransferLoggingValidation.BankTransferApplicationService, ex, ex.Message);
				throw ex;
			}
			return bankTransferModel;
		}
		public BankTransferLU GetAllBankTransferLU(Guid banktransferId, long companyId, string userName, string connectionString)
		{
			BankTransferLU bankTransferLU = new BankTransferLU();
			try
			{
				//long? coaIdw = 0;
				//long? coaIdd = 0;
				BankTransfer banktransfer = _bankTransferService.GetBankTransferLU(banktransferId, companyId);
				List<long> lstBTsubComp = null;
				if (banktransfer != null)
					lstBTsubComp = banktransfer.BankTransferDetails.Select(x => x.ServiceCompanyId).ToList();
				List<long> lstSubComp = _bankTransferService.lstServiceCompanyIds(companyId, userName);
				List<long> lstCompany = null;
				string compIds = null;
				if (lstBTsubComp != null)
				{
					lstCompany = lstBTsubComp;
					lstCompany.AddRange(lstSubComp);
				}

				if (banktransfer != null)
					compIds = string.Join(",", lstCompany.Distinct());
				else
					compIds = string.Join(",", lstSubComp.Distinct());
				//bankTransferLU.ModeOfTransferLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_codes_ModeOfTransfer);
				if (banktransfer != null)
				{
					bankTransferLU.ModeOfTransferLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId,
		ControlCodeConstants.Control_codes_ModeOfTransfer, banktransfer.ModeOfTransfer);
				}
				else
				{
					bankTransferLU.ModeOfTransferLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId,
ControlCodeConstants.Control_codes_ModeOfTransfer, string.Empty);
				}

				//if (banktransfer != null)
				//{
				//    //List<BankTransferDetail> details = _bankTransferDetailService.GetBankTransfeById(banktransferId);
				//    //foreach (var detail in details)
				//    //    if (detail.Type == "Deposit")
				//    //        coaIdd = detail.COAId;
				//    //    else
				//    //        coaIdw = detail.COAId;
				//    //added by lokanath
				//    var lookupcategory = _controlCodeCatService.GetInactiveControlcode(companyId, ControlCodeConstants.Control_codes_ModeOfTransfer, banktransfer.ModeOfTransfer);
				//    if (lookupcategory != null)
				//    {
				//        bankTransferLU.ModeOfTransferLU.Lookups.Add(lookupcategory);
				//    }
				//}
				//  var account = _accountTypeService.GetById(companyId, AccountNameConstants.Cash_and_bank_balances);
				// var listcoa = _chartOfAccountService.GetCashAndBankCOAId(companyId, account.Id);
				long comp = banktransfer == null ? 0 : banktransfer.CompanyId;

				//var lstCompany = _companyService.GetCompany(companyId, comp).Select(x => new LookUpCompany<string>()
				//{
				//    Id = x.Id,
				//    Name = x.Name,
				//    ShortName = x.ShortName,
				//    LookUps = listcoa.Where(c => c.SubsidaryCompanyId == x.Id && (c.Id == coaIdd || c.Id == coaIdw || c.Status == RecordStatusEnum.Active)).Select(a => new LookUp<string>()
				//    {
				//        Id = a.Id,
				//        //Name = x.ShortName + '-' + a.Name,
				//        Name = a.Name,
				//        Currency = a.Currency,
				//        Code = a.Code,
				//        ServiceCompanyId = a.SubsidaryCompanyId.Value,
				//    }).ToList()
				//}).ToList();
				//bankTransferLU.SubsideryCompanyLU = _companyService.Listofsubsudarycompany(companyId, comp);

				//commnted code on 18-11-2019
				//List<long> coaIds = new List<long>();
				//bankTransferLU.SubsideryCompanyLU = _companyService.ListOfSubsudaryCompanyActiveInactive(companyId, comp, banktransferId, banktransfer != null ? banktransfer.BankTransferDetails.Any() ? banktransfer.BankTransferDetails.Select(c => c.COAId).ToList() : coaIds : coaIds, userName);

				List<long> coaIds = banktransfer != null ? banktransfer.BankTransferDetails.Any() ? banktransfer.BankTransferDetails.Select(c => c.COAId).ToList() : new List<long>() : new List<long>();

				//new Subsidary company look up call modifications

				//string query = $"Select distinct COMP.Id,comp.ShortName,comp.Name,comp.IsGstSetting,COMP.Status,Loc.BaseCurrency, Case when inter.InterCompanyType = 'Billing' and interDetail.Status = 1 Then 1 End as IsIBServiceEntity,Case when inter.InterCompanyType = 'Clearing' and interDetail.Status = 1 Then 1 End as IsICServiceEntity from Bean.BankTransfer BT Join Bean.BankTransferDetail BTD on BT.Id = BTD.BankTransferId  Right Join Common.Company comp on comp.Id = BTD.ServiceCompanyId JOIN Common.CompanyUser CU on comp.ParentId = CU.CompanyId Left join Bean.InterCompanySettingDetail interDetail on interDetail.ServiceEntityId = comp.Id and interDetail.Status = 1 Left join Bean.InterCompanySetting inter on inter.CompanyId = comp.ParentId join Common.Localization Loc on Loc.CompanyId = comp.ParentId where comp.ParentId = {companyId} and(comp.Status = 1 or BT.Id = '{banktransferId}') and CU.Username = '{userName}' order by comp.ShortName";
				string query = $"Select distinct A.Id,A.ShortName,A.Name,A.IsGstSetting,A.Status,A.BaseCurrency, Case when B.InterCompanyType = 'Billing' and B.Status = 1  Then 1 Else 0 End as IsIBServiceEntity,Case when C.InterCompanyType = 'Clearing' and c.Status = 1  Then 1 Else 0 End as IsICServiceEntity from (Select Distinct COMP.Id, comp.ShortName, comp.Name, comp.IsGstSetting, COMP.Status, Loc.BaseCurrency, comp.ParentId from Bean.BankTransfer BT Join Bean.BankTransferDetail BTD on BT.Id = BTD.BankTransferId Right Join Common.Company comp on comp.Id = BTD.ServiceCompanyId join Common.Localization Loc on Loc.CompanyId = comp.ParentId where comp.id in ({compIds}) and(comp.Status = 1 or BT.Id = '{banktransferId}')) As A Left Join ( Select distinct interDetail.ServiceEntityId, inter.InterCompanyType, inter.CompanyId, interDetail.Status from Bean.InterCompanySetting inter join Bean.InterCompanySettingDetail interDetail on inter.Id = interDetail.InterCompanySettingId and interDetail.Status = 1 and inter.CompanyId = {companyId} and Inter.InterCompanyType = 'Billing') As B on A.ParentId = B.CompanyId and A.Id = B.ServiceEntityId Left join ( Select distinct interDetail.ServiceEntityId, inter.InterCompanyType, inter.CompanyId, interDetail.Status from Bean.InterCompanySetting inter join Bean.InterCompanySettingDetail interDetail on inter.Id = interDetail.InterCompanySettingId and interDetail.Status = 1 and inter.CompanyId = {companyId} and Inter.InterCompanyType = 'Clearing') As c on A.ParentId = c.CompanyId and A.Id = c.ServiceEntityId";

				AccountType account = _accountTypeService.GetById(companyId, AccountNameConstants.Cash_and_bank_balances);
				List<LookUpCompany<string>> lstSubLookup = new List<LookUpCompany<string>>();
				using (SqlConnection con = new SqlConnection(connectionString))
				{
					if (con.State != System.Data.ConnectionState.Open)
						con.Open();
					SqlCommand cmd = new SqlCommand(query, con);
					SqlDataReader dr = cmd.ExecuteReader();

					while (dr.Read())
					{
						LookUpCompany<string> subLookup = new LookUpCompany<string>();
						subLookup.Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0;
						subLookup.Name = Convert.ToString(dr["Name"]);
						subLookup.ShortName = Convert.ToString(dr["ShortName"]);
						subLookup.isGstActivated = dr["IsGstSetting"] != DBNull.Value ? Convert.ToBoolean(dr["IsGstSetting"]) : (bool?)null;
						subLookup.IsIBServiceEntity = dr["IsIBServiceEntity"] != DBNull.Value ? Convert.ToBoolean(dr["IsIBServiceEntity"]) : (bool?)null;
						subLookup.IsICServiceEntity = dr["IsICServiceEntity"] != DBNull.Value ? Convert.ToBoolean(dr["IsICServiceEntity"]) : (bool?)null;
						subLookup.IsHyperLinkEnable = lstSubComp.Any(x => x == subLookup.Id);

						subLookup.LookUps = banktransferId == new Guid() ? account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == subLookup.Id || c.SubsidaryCompanyId == null) && c.IsLinkedAccount != null && c.IsRevaluation == null && c.Status == RecordStatusEnum.Active).Select(a => new CommonModule.Infra.LookUp<string>()
						{
							Id = a.Id,
							// Name = x.ShortName + '-' + a.Name,                    
							Name = ((a.Name == COANameConstants.Petty_Cash || a.Name == COANameConstants.Fixed_Deposit) || ((a.Currency != null && a.Currency != string.Empty) && a.IsBank.Equals(null))) ? a.Name + "(" + (a.Currency != null ? a.Currency : Convert.ToString(dr["BaseCurrency"])) + ")" : a.Name,
							Currency = a.Currency != null ? a.Currency : Convert.ToString(dr["BaseCurrency"]),
							Code = a.Code,
							ServiceCompanyId = subLookup.Id,
							Status = a.Status
						}).OrderBy(d => d.Name).ToList() : account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == subLookup.Id || c.SubsidaryCompanyId == null) && (coaIds.Contains(c.Id) || c.Status == RecordStatusEnum.Active) && (/*c.IsSystem == false || */c.IsLinkedAccount != null && c.IsRevaluation == null)).Select(a => new CommonModule.Infra.LookUp<string>()
						{
							Id = a.Id,
							// Name = x.ShortName + '-' + a.Name,                    
							Name = ((a.Name == COANameConstants.Petty_Cash || a.Name == COANameConstants.Fixed_Deposit) || ((a.Currency != null && a.Currency != string.Empty) && a.IsBank.Equals(null))) ? a.Name + "(" + (a.Currency != null ? a.Currency : Convert.ToString(dr["BaseCurrency"])) + ")" : a.Name,
							Currency = a.Currency != null ? a.Currency : Convert.ToString(dr["BaseCurrency"]),
							Code = a.Code,
							ServiceCompanyId = subLookup.Id,
							Status = a.Status
						}).OrderBy(d => d.Name).ToList();

						lstSubLookup.Add(subLookup);
					}
					if (con.State != System.Data.ConnectionState.Closed)
						con.Close();
				}
				bankTransferLU.SubsideryCompanyLU = lstSubLookup;
			}
			catch (Exception ex)
			{
				LoggingHelper.LogError(BankTransferLoggingValidation.BankTransferApplicationService, ex, ex.Message);
				throw ex;
			}
			return bankTransferLU;
		}
		#endregion

		#region SaveCall
		public BankTransfer SaveBankTransfer(BankTransferModel bankTransferModel, string connectionString)
		{
			bool isAdd = false;
			bool isDocAdd = false;
			try
			{
				var AdditionalInfo = new Dictionary<string, object>();
				AdditionalInfo.Add("Data", JsonConvert.SerializeObject(bankTransferModel));
				LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, "ObjectSave", AdditionalInfo);
				//bool isAdd = false;
				LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Entered_Into_Save_BankTransfer);

				//to check if it is void or not
				if (_bankTransferService.IsVoid(bankTransferModel.CompanyId, bankTransferModel.Id))
					throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

				string eventstore = string.Empty;
				string eventDocStatusChanged = string.Empty;
				bool? isInterccBillingChecked = false;
				LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Checking_All_Validations);
				if (bankTransferModel.IsDocNoEditable == true)
				{
					string _errors = CommonValidation.ValidateObject(bankTransferModel);
					if (!string.IsNullOrEmpty(_errors))
					{
						throw new Exception(_errors);
					}
				}

				if (bankTransferModel.TransferDate == null)
				{
					throw new Exception(BankTransferValidation.Invalid_Transfer_Date);
				}
				if (bankTransferModel.IsDocNoEditable == true)
				{
					BankTransfer bankTransfer = _bankTransferService.GetBankTransferDocNo(bankTransferModel.Id, bankTransferModel.DocNo, bankTransferModel.CompanyId);
					if (bankTransfer != null)
					{
						throw new Exception(CommonConstant.Document_number_already_exist);
					}
				}
				List<DocumentHistoryModel> lstOfDocHistoryModels = new List<DocumentHistoryModel>();//for document history

				BankTransferDetailModel bankTransferDetail = new BankTransferDetailModel();
				if (bankTransferModel.ExCurrency != bankTransferDetail.Type)
					if (bankTransferModel.ExchangeRate == 0)
						throw new Exception(BankTransferValidation.ExchangeRate_Should_Be_Grater_Than_zero);
				//Need to verify the BankTransfer is within Financial year
				if (bankTransferModel.IsIntercoBilling != true)
					foreach (var transferDetail in bankTransferModel.BankTransferDetailsModel)
					{
						if (transferDetail.Amount <= 0)
						{
							throw new Exception(BankTransferValidation.Amount_Must_Be_Grater_than_Zero);
						}
					}
				if (!_financialSettingService.ValidateYearEndLockDate(bankTransferModel.TransferDate, bankTransferModel.CompanyId))
				{
					throw new Exception(BankTransferValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
				}

				var WithdrawalCurrency = bankTransferModel.BankTransferDetailsModel.Where(c => c.Type == "Withdrawal").Select(c => c.Currency).FirstOrDefault();
				var DepositCurrency = bankTransferModel.BankTransferDetailsModel.Where(c => c.Type == "Deposit").Select(c => c.Currency).FirstOrDefault();
				if ((bankTransferModel.ExCurrency != WithdrawalCurrency) && (bankTransferModel.ExCurrency != DepositCurrency) && (WithdrawalCurrency != DepositCurrency))
				{
					throw new Exception(BankTransferValidation.BaseCurrency_should_match_Any_One_of_the_Withdrawal_Or_Deposit_Currency);
				}

				long? withDrawalCOAID = bankTransferModel.BankTransferDetailsModel.Where(c => c.Type == "Withdrawal").Select(c => c.COAId).FirstOrDefault();
				long? depositCOAID = bankTransferModel.BankTransferDetailsModel.Where(c => c.Type == "Deposit").Select(c => c.COAId).FirstOrDefault();

				long withdrawalServiceCompany = bankTransferModel.BankTransferDetailsModel.Where(c => c.Type == "Withdrawal").Select(c => c.ServiceCompanyId).FirstOrDefault();
				long depositServiceCompany = bankTransferModel.BankTransferDetailsModel.Where(c => c.Type == "Deposit").Select(c => c.ServiceCompanyId).FirstOrDefault();

				#region Validation for IC or IB, if serviceentity status is one or not
				if (bankTransferModel.IsIntercoClearing == true || bankTransferModel.IsIntercoBilling == true)
				{
					string query = null;
					if (bankTransferModel.IsIntercoClearing == true)
					{
						query = $" select COUNT(distinct ServiceEntityId) from Bean.InterCompanySetting ICS  Join Bean.InterCompanySettingDetail ICSD on ICS.Id = ICSD.InterCompanySettingId  where ICS.CompanyId = {bankTransferModel.CompanyId}  and ICS.InterCompanyType = 'Clearing' and ICSD.ServiceEntityId in({withdrawalServiceCompany},{depositServiceCompany}) and Status = 1";
					}
					else if (bankTransferModel.IsIntercoBilling == true)
					{
						query = $" select COUNT(distinct ServiceEntityId) from Bean.InterCompanySetting ICS  Join Bean.InterCompanySettingDetail ICSD on ICS.Id = ICSD.InterCompanySettingId  where ICS.CompanyId = {bankTransferModel.CompanyId}  and ICS.InterCompanyType = 'Billing' and ICSD.ServiceEntityId in({withdrawalServiceCompany},{depositServiceCompany}) and Status = 1";
					}
					if (query != null)
					{
						using (SqlConnection con = new SqlConnection(connectionString))
						{
							if (con.State != System.Data.ConnectionState.Open)
								con.Open();
							SqlCommand cmd = new SqlCommand(query, con);
							cmd.CommandType = System.Data.CommandType.Text;
							int count = Convert.ToInt32(cmd.ExecuteScalar());
							if (con.State != System.Data.ConnectionState.Closed)
								con.Close();
							if (count != 2)
								throw new Exception(CommonConstant.The_State_of_the_service_entity_had_been_changed);
						}
					}
				}
				#endregion

				if ((withDrawalCOAID == depositCOAID) && (WithdrawalCurrency == DepositCurrency) && (withdrawalServiceCompany == depositServiceCompany))
				{
					throw new Exception(BankTransferValidation.Do_Not_allow_Same_company_and_Same_COAAccounts);
				}

				//Verify if the BankTransfer is out of open financial period and lock password is entered and valid
				if (!_financialSettingService.ValidateFinancialOpenPeriod(bankTransferModel.TransferDate.Date, bankTransferModel.CompanyId))
				{
					if (String.IsNullOrEmpty(bankTransferModel.PeriodLockPassword))
					{
						throw new Exception(BankTransferValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
					}
					else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(bankTransferModel.TransferDate, bankTransferModel.PeriodLockPassword, bankTransferModel.CompanyId))
					{
						throw new Exception(BankTransferValidation.Invalid_Financial_Period_Lock_Password);
					}
				}
				LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Validations_Checking_Finished);
				BankTransfer _bankTransfer = _bankTransferService.GetBankTransferById(bankTransferModel.Id, bankTransferModel.CompanyId);
				string oldDocumentNo = string.Empty;
				if (_bankTransfer != null)
				{
					oldDocumentNo = _bankTransfer.DocNo;
					LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Validationg_The_BankTransfor_In_Edit_Mode);

					//Data concurrency verify
					string timeStamp = "0x" + string.Concat(Array.ConvertAll(_bankTransfer.Version, x => x.ToString("X2")));
					//if (!timeStamp.Equals(bankTransferModel.Version))
					//    throw new Exception(CommonConstant.Document_has_been_modified_outside);

					eventstore = "update";
					LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Going_To_Execute_InsertBanTransfor_Method);
					isInterccBillingChecked = _bankTransfer.IsIntercoBilling;
					InsertBankTransfer(_bankTransfer, bankTransferModel);
					_bankTransfer.DocNo = bankTransferModel.DocNo;
					_bankTransfer.SystemRefNo = _bankTransfer.DocNo;
					_bankTransfer.ModifiedBy = bankTransferModel.ModifiedBy;
					_bankTransfer.ModifiedDate = DateTime.UtcNow;
					_bankTransfer.ObjectState = ObjectState.Modified;
					_bankTransferService.Update(_bankTransfer);
					LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Going_To_Execute_UpdateBanktransferDetail_Method);
					UpdateBankTransferDetails(_bankTransfer, bankTransferModel);

					if (_bankTransfer.IsIntercoBilling == true && (bankTransferModel.SettlementDetailModels != null || bankTransferModel.SettlementDetailModels.Any()))
					{
						LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.EntredInto_FillBankTransferDetail_Method);
						InsertOrUpdateSettlemntDetail(_bankTransfer, bankTransferModel.SettlementDetailModels.ToList(), lstOfDocHistoryModels, true);
					}
					if (isInterccBillingChecked == true && _bankTransfer.IsIntercoBilling != true)
					{
						LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Enterd_Into_IfIBisUncheckedin_Edit_mode);
						UpdateDocStateIfIBUnchecked(_bankTransfer, lstOfDocHistoryModels, true);
					}

					if (_bankTransfer.IsIntercoBilling != true)
					{
						JVModel jvm = new JVModel();
						var withdral = _bankTransfer.BankTransferDetails.Where(c => c.Type == "Withdrawal").FirstOrDefault();
						var deposit = _bankTransfer.BankTransferDetails.Where(c => c.Type == "Deposit").FirstOrDefault();
						if (withdral.Currency == deposit.Currency && withdral.ServiceCompanyId == deposit.ServiceCompanyId)
						{
							FillJournal(jvm, _bankTransfer, false, true);
							SaveInvoice1(jvm);
						}
						else
						{
							FillJournal1(jvm, _bankTransfer, false, true, true);
							jvm.IsFirst = true;
							SaveInvoice1(jvm);
							FillJournal1(jvm, _bankTransfer, false, false, false);
							jvm.IsFirst = false;
							SaveInvoice1(jvm);
						}
					}
				}
				else
				{
					isAdd = true;
					eventstore = "new";
					_bankTransfer = new BankTransfer();
					LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Going_to_execute_InsertBankTransfer_method_in_insert_new_mode);
					InsertBankTransfer(_bankTransfer, bankTransferModel);
					_bankTransfer.Id = Guid.NewGuid();
					int? recorder = 0;
					if (bankTransferModel.BankTransferDetailsModel != null || bankTransferModel.BankTransferDetailsModel.Count > 0)
					{
						foreach (BankTransferDetailModel detailModel in bankTransferModel.BankTransferDetailsModel)
						{
							LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.EntredInto_FillBankTransferDetail_Method);
							BankTransferDetail nbankTransferDetail = new BankTransferDetail();
							LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Going_to_Execute_Fill_Method_Of_BankTransferDetatil);
							FillBankTransferDetail(nbankTransferDetail, detailModel);
							nbankTransferDetail.RecOrder = recorder.Value + 1;
							recorder = nbankTransferDetail.RecOrder;
							nbankTransferDetail.Id = Guid.NewGuid();
							nbankTransferDetail.BankTransferId = _bankTransfer.Id;
							_bankTransferDetailService.Insert(nbankTransferDetail);
							nbankTransferDetail.ObjectState = ObjectState.Added;
						}
					}
					if (_bankTransfer.IsIntercoBilling == true && (bankTransferModel.SettlementDetailModels != null || bankTransferModel.SettlementDetailModels.Any()))
					{
						LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.EntredInto_FillBankTransferDetail_Method);
						InsertOrUpdateSettlemntDetail(_bankTransfer, bankTransferModel.SettlementDetailModels.ToList(), lstOfDocHistoryModels, false);
					}
					_bankTransfer.Status = RecordStatusEnum.Active;
					_bankTransfer.UserCreated = bankTransferModel.UserCreated;
					_bankTransfer.CreatedDate = DateTime.UtcNow;
					LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Going_To_Execute_Auto_Number_Method);
					_bankTransfer.SystemRefNo = bankTransferModel.IsDocNoEditable != true ? _autoService.GetAutonumber(bankTransferModel.CompanyId, DocTypeConstants.BankTransfer, connectionString) : bankTransferModel.DocNo;
					isDocAdd = true;
					_bankTransfer.DocNo = _bankTransfer.SystemRefNo;
					_bankTransfer.ObjectState = ObjectState.Added;
					_bankTransferService.Insert(_bankTransfer);
					if (_bankTransfer.IsIntercoBilling != true)
					{
						JVModel jvm = new JVModel();
						var withdral = _bankTransfer.BankTransferDetails.Where(c => c.Type == "Withdrawal").FirstOrDefault();
						var deposit = _bankTransfer.BankTransferDetails.Where(c => c.Type == "Deposit").FirstOrDefault();
						if (withdral.Currency == deposit.Currency && withdral.ServiceCompanyId == deposit.ServiceCompanyId)
						{
							FillJournal(jvm, _bankTransfer, false, true);
							SaveInvoice1(jvm);
						}
						else
						{
							FillJournal1(jvm, _bankTransfer, false, true, true);
							jvm.IsFirst = true;
							SaveInvoice1(jvm);
							FillJournal1(jvm, _bankTransfer, false, false, false);
							jvm.IsFirst = false;
							SaveInvoice1(jvm);
						}
					}
				}
				try
				{
					_unitOfWork.SaveChanges();

					if (_bankTransfer.IsIntercoBilling == true)
					{
						AppaWorld.Bean.Common.SaveMultiplePosting(_bankTransfer.CompanyId, _bankTransfer.Id, _bankTransfer.DocType, connectionString);
					}

					#region Documentary History
					try
					{
						List<DocumentHistoryModel> lstDocuemnt = AppaWorld.Bean.Common.FillDocumentHistory(_bankTransfer.Id, _bankTransfer.CompanyId, _bankTransfer.Id, _bankTransfer.DocType, null, _bankTransfer.DocumentState, _bankTransfer.ExCurrency, 0, 0, _bankTransfer.ExchangeRate.Value, _bankTransfer.ModifiedBy != null ? _bankTransfer.ModifiedBy : _bankTransfer.UserCreated, _bankTransfer.Remarks, _bankTransfer.TransferDate, 0, 0);
						if (lstDocuemnt.Any())
						{
							lstOfDocHistoryModels.AddRange(lstDocuemnt);
						}
						AppaWorld.Bean.Common.SaveDocumentHistory(lstOfDocHistoryModels, connectionString);
					}
					catch (Exception ex)
					{

					}
					#endregion Documentary History
					#region DocumentAttachment_Save_Call
					if (isAdd && bankTransferModel.TileAttachments != null && bankTransferModel.TileAttachments.Count() > 0)
					{
						string DocuNo = _commonApplicationService.StringCharactersReplaceFunction(_bankTransfer.DocNo);
						//         _bankTransfer.DocNo.Replace('"', '_').Replace('\\', '_').Replace('/', '_')
						//.Replace(':', '_').Replace('|', '_').Replace('<', '_').Replace('>', '_').Replace('*', '_').Replace('?', '_').Replace("'", "_");
						string path = _bankTransfer.DocType + "s" + "/" + DocuNo;
						SaveTailsAttachments(bankTransferModel.CompanyId, path, bankTransferModel.UserCreated, bankTransferModel.TileAttachments);
					}
					#endregion
					#region Document Folder rename

					if (!isAdd && oldDocumentNo != bankTransferModel.DocNo)
						_commonApplicationService.ChangeFolderName(bankTransferModel.CompanyId, bankTransferModel.DocNo, oldDocumentNo, "Transfers");

					#endregion
					LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.SaveChanges_method_execution_happened);
					LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Going_to_execute_EventStore_process);
					LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.EventStore_process_done);
				}
				catch (DbEntityValidationException ex)
				{
					LoggingHelper.LogError(BankTransferLoggingValidation.BankTransferApplicationService, ex, ex.Message);
					foreach (var eve in ex.EntityValidationErrors)
					{
						Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
							eve.Entry.Entity.GetType().Name, eve.Entry.State);
						foreach (var ve in eve.ValidationErrors)
						{
							Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
								ve.PropertyName, ve.ErrorMessage);
						}
					}
					throw new Exception("An error has occurred!Please try after sometimes.");
				}
				return _bankTransfer;
			}
			catch (Exception ex)
			{
				if (isAdd && isDocAdd && bankTransferModel.IsDocNoEditable == false)
				{
					AppaWorld.Bean.Common.SaveDocNoSequence(bankTransferModel.CompanyId, bankTransferModel.DocType, connectionString);
				}
				throw ex;
			}
		}

		public BankTransfer SaveBankTransferDocumentVoid(DocumentVoidModel TObject, string connectionString)
		{
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Entered_into_SaveBankTransferDocumentVoid_method);
			string DocNo = "-V";
			List<DocumentHistoryModel> lstDocumentHistoryModel = new List<DocumentHistoryModel>();
			BankTransfer bankTransfer = _bankTransferService.GetBankTransferById(TObject.Id, TObject.CompanyId);
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Validating_model_and_proceed_towards_the_functional_validation);
			if (bankTransfer == null)
				throw new Exception(BankTransferValidation.Invalid_BankTransfer);
			else
			{
				//Data concurrency verify
				string timeStamp = "0x" + string.Concat(Array.ConvertAll(bankTransfer.Version, x => x.ToString("X2")));
				if (!timeStamp.Equals(TObject.Version))
					throw new Exception(CommonConstant.Document_has_been_modified_outside);
			}

			//if (_withDrawal.DocumentState != DebitNoteState.NotPaid)
			//    throw new Exception("State should be " + DebitNoteState.NotPaid);

			//Need to verify the withdrawal within Financial year
			if (!_financialSettingService.ValidateYearEndLockDate(bankTransfer.TransferDate, bankTransfer.CompanyId))
			{
				throw new Exception(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
			}
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Functionality_validation_going_on);
			//Verify if the invoice is out of open financial period and lock password is entered and valid
			if (!_financialSettingService.ValidateFinancialOpenPeriod(bankTransfer.TransferDate, TObject.CompanyId))
			{
				if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
				{
					throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
				}
				else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(bankTransfer.TransferDate, TObject.PeriodLockPassword, TObject.CompanyId))
				{
					throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
				}
				LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.End_of_the_Functionality_validation);
			}
			bankTransfer.DocumentState = BanktransferStatus.Void;
			bankTransfer.DocNo = bankTransfer.DocNo + DocNo;
			bankTransfer.ModifiedDate = DateTime.UtcNow;
			bankTransfer.ModifiedBy = TObject.ModifiedBy;
			bankTransfer.ObjectState = ObjectState.Modified;

			if (bankTransfer.IsIntercoBilling == true)
			{
				UpdateDocStateIfIBUnchecked(bankTransfer, lstDocumentHistoryModel, false);
			}

			try
			{
				_unitOfWork.SaveChanges();
				#region Documentary History
				try
				{
					List<DocumentHistoryModel> lstDocuments = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, bankTransfer.CompanyId, bankTransfer.Id, bankTransfer.DocType, null, bankTransfer.DocumentState, bankTransfer.ExCurrency, 0, 0, bankTransfer.ExchangeRate.Value, bankTransfer.ModifiedBy != null ? bankTransfer.ModifiedBy : bankTransfer.UserCreated, bankTransfer.Remarks, bankTransfer.TransferDate, 0, 0);
					if (lstDocuments.Any())
						lstDocumentHistoryModel.AddRange(lstDocuments);
					AppaWorld.Bean.Common.SaveDocumentHistory(lstDocumentHistoryModel, connectionString);
				}
				catch (Exception ex)
				{

				}
				#endregion Documentary History
				DocumentVoidModel tObject = new DocumentVoidModel();
				tObject.Id = TObject.Id;
				tObject.CompanyId = TObject.CompanyId;
				tObject.DocType = bankTransfer.DocType;
				tObject.DocNo = bankTransfer.DocNo;
				tObject.ModifiedBy = TObject.ModifiedBy;
				deleteJVPostBT(tObject);
				LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.SaveChanges_method_execution_happened_in_void_method);
			}
			catch (Exception ex)
			{
				LoggingHelper.LogError(BankTransferLoggingValidation.BankTransferApplicationService, ex, ex.Message);
				throw ex;
			}
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.SaveChanges_method_execution_happened_in_void_method);
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.End_of_the_Doc_Void_method);
			return bankTransfer;
		}
		#endregion

		#region Private Methods

		private string GetAutoNumberByEntityType(long companyId, BankTransfer lastInvoice, string entityType, AppsWorld.BankTransferModule.Entities.AutoNumber _autoNo, ref bool? isEdit)
		{
			string outPutNumber = null;
			//isEdit = false;
			//AppsWorld.BankTransferModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, entityType);
			if (_autoNo != null)
			{
				if (_autoNo.IsEditable == true)
				{
					outPutNumber = GetNewBankTransferDocumentNumber(DocTypeConstants.BankTransfer, companyId);
					//invDTO.IsEditable = true;
					isEdit = true;
				}
				else
				{
					//invDTO.IsEditable = false;
					isEdit = false;
					//List<Invoice> lstInvoice = _invoiceEntityService.GetAllInvoiceByCIDandType(companyid, DocTypeConstants.Invoice);
					string autonoFormat = _autoNo.Format.Replace("{YYYY}", DateTime.UtcNow.Year.ToString()).Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.UtcNow.Month) + "/" + DateTime.UtcNow.Year.ToString());
					string number = "1";
					if (lastInvoice != null)
					{
						if (_autoNo.Format.Contains("{MM/YYYY}"))
						{
							//var lastCreatedInvoice = lstInvoice.FirstOrDefault();
							if (lastInvoice.CreatedDate.Value.Month != DateTime.UtcNow.Month)
							{
								//number = "1";
								outPutNumber = autonoFormat + number.PadLeft(_autoNo.CounterLength.Value, '0');
							}
							else
							{
								string output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
								outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
							}
						}
						else
						{
							string output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
							outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
						}
					}
					else
					{
						string output = (Convert.ToInt32(_autoNo.GeneratedNumber)).ToString();
						outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
						//counter = Convert.ToInt32(value);
					}
				}
			}
			return outPutNumber;
		}

		private void FillBankTransfer(BankTransferModel bankTransferModel, BankTransfer bankTransfer)
		{
			bankTransferModel.Id = bankTransfer.Id;
			bankTransferModel.CompanyId = bankTransfer.CompanyId;
			bankTransferModel.IsModify = bankTransfer.ClearCount > 0;
			bankTransferModel.IsLocked = bankTransfer.IsLocked;
			bankTransferModel.BankClearingDate = bankTransfer.BankClearingDate;
			bankTransferModel.CreatedDate = bankTransfer.CreatedDate;
			bankTransferModel.DocDescription = bankTransfer.DocDescription;
			bankTransferModel.DocNo = bankTransfer.DocNo;
			bankTransferModel.DocType = bankTransfer.DocType;
			bankTransferModel.DocumentState = bankTransfer.DocumentState;
			bankTransferModel.ExchangeRate = bankTransfer.ExchangeRate;
			bankTransferModel.Version = "0x" + string.Concat(Array.ConvertAll(bankTransfer.Version, x => x.ToString("X2")));
			bankTransferModel.ExCurrency = bankTransfer.ExCurrency;
			bankTransferModel.ExDurationFrom = bankTransfer.ExDurationFrom;
			bankTransferModel.ExDurationTo = bankTransfer.ExDurationTo;
			bankTransferModel.IsGstSetting = bankTransfer.IsGstSetting;
			bankTransferModel.IsMultiCompany = bankTransfer.IsMultiCompany;
			bankTransferModel.IsMultiCurrency = bankTransfer.IsMultiCurrency;
			bankTransferModel.IsNoSupportingDocument = bankTransfer.IsNoSupportingDocument;
			bankTransferModel.ModeOfTransfer = bankTransfer.ModeOfTransfer;
			bankTransferModel.ModifiedBy = bankTransfer.ModifiedBy;
			bankTransferModel.ModifiedDate = bankTransfer.ModifiedDate;
			bankTransferModel.NoSupportingDocument = bankTransfer.NoSupportingDocument;
			bankTransferModel.Remarks = bankTransfer.Remarks;
			bankTransferModel.Status = bankTransfer.Status;
			bankTransferModel.SystemCalculatedExchangeRate = bankTransfer.SystemCalculatedExchangeRate;
			bankTransferModel.SystemRefNo = bankTransfer.SystemRefNo;
			bankTransferModel.TransferDate = bankTransfer.TransferDate;
			bankTransferModel.TransferRefNo = bankTransfer.TransferRefNo;
			bankTransferModel.IsBaseCurrencyRateChanged = bankTransfer.IsBaseCurrencyRateChanged;
			bankTransferModel.UserCreated = bankTransfer.UserCreated;
			bankTransferModel.IsIntercoBilling = bankTransfer.IsIntercoBilling;
			bankTransferModel.IsIntercoClearing = bankTransfer.IsIntercoClearing;
			// bankTransferModel.IsInterCompany = bankTransfer.IsInterCompany.Value;
			if (bankTransfer.VarianceExchangeRate != null)
			{
				LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Entred_If_Condition_Of_Insert_Banktransfer_And_Check_VarianceExchangeRate_Is_Not_Null);
				var varience = Convert.ToDecimal(bankTransfer.VarianceExchangeRate);
				bankTransferModel.VarianceExchangeRate = Math.Round(varience, 2) + "%";
			}
			//bankTransferModel.VarianceExchangeRate = bankTransfer.VarianceExchangeRate;

		}

		private string GetNewBankTransferDocumentNumber(string docType, long companyId)
		{
			BankTransfer cashSale = _bankTransferService.GetDocTypeAndCompanyid(docType, companyId);
			string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
			if (cashSale != null)
			{
				string strOldNo = String.Empty;
				BankTransfer duplicatBankTransfer;
				int index;
				strOldDocNo = cashSale.DocNo;

				for (int i = strOldDocNo.Length - 1; i >= 0; i--)
				{
					if (Char.IsDigit(strOldDocNo[i]))
						strOldNo = strOldDocNo[i] + strOldNo;
					else
						break;
				}
				long docNo = 0;
				try
				{ docNo = long.Parse(strOldNo); }
				catch { }

				index = strOldDocNo.LastIndexOf(strOldNo);

				do
				{
					docNo++;
					strNewNo = docNo.ToString().PadLeft(strOldNo.Length, '0');
					strNewDocNo = (docNo == 1) ? strOldDocNo + strNewNo : strOldDocNo.Substring(0, index) + strNewNo;

					duplicatBankTransfer = _bankTransferService.DuplicateBankTransfer(strNewDocNo, docType, companyId);
				} while (duplicatBankTransfer != null);
			}
			return strNewDocNo;
		}
		private void InsertBankTransfer(BankTransfer banktransfernew, BankTransferModel TObject)
		{
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Entred_into_UpdateBankTransferDetail_method_and_checking_the_conditions);
			try
			{
				banktransfernew.BankClearingDate = TObject.BankClearingDate;
				banktransfernew.CompanyId = TObject.CompanyId;
				banktransfernew.CreatedDate = TObject.CreatedDate;
				banktransfernew.DocDescription = TObject.DocDescription;
				banktransfernew.DocType = TObject.DocType;
				banktransfernew.DocumentState = BankTransferConstants.TransferState;
				banktransfernew.ExchangeRate = TObject.ExchangeRate == null ? 1 : TObject.ExchangeRate;
				banktransfernew.ExCurrency = TObject.ExCurrency;
				banktransfernew.ExDurationFrom = TObject.ExDurationFrom;
				banktransfernew.ExDurationTo = TObject.ExDurationTo;
				banktransfernew.IsGstSetting = TObject.IsGstSetting;
				banktransfernew.IsMultiCompany = TObject.IsMultiCompany;
				banktransfernew.IsMultiCurrency = TObject.IsMultiCurrency;
				banktransfernew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
				banktransfernew.ModeOfTransfer = TObject.ModeOfTransfer;
				banktransfernew.NoSupportingDocument = TObject.NoSupportingDocument;
				banktransfernew.Remarks = TObject.Remarks;
				banktransfernew.Status = TObject.Status;
				banktransfernew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
				banktransfernew.SystemCalculatedExchangeRate = TObject.SystemCalculatedExchangeRate;
				banktransfernew.SystemRefNo = TObject.SystemRefNo;
				banktransfernew.TransferDate = TObject.TransferDate;
				banktransfernew.TransferRefNo = TObject.TransferRefNo;
				banktransfernew.UserCreated = TObject.UserCreated;
				banktransfernew.IsInterCompany = TObject.IsInterCompany;
				banktransfernew.IsIntercoBilling = TObject.IsIntercoBilling;
				banktransfernew.IsIntercoClearing = TObject.IsIntercoClearing;
				if (TObject.VarianceExchangeRate != null)
				{
					LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Entred_If_Condition_Of_Insert_Banktransfer_And_Check_VarianceExchangeRate_Is_Not_Null);
					var varience = Convert.ToDecimal(TObject.VarianceExchangeRate);
					banktransfernew.VarianceExchangeRate = varience;
				}


			}
			catch (Exception ex)
			{
				LoggingHelper.LogError(BankTransferLoggingValidation.BankTransferApplicationService, ex, ex.Message);
				throw ex;
			}
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Exited_From_InsertBankTransfer_Method);
		}
		private void UpdateBankTransferDetails(BankTransfer _banktrasferNew, BankTransferModel TObject)
		{
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Entred_into_UpdateBankTransferDetail_method_and_checking_the_conditions);
			try
			{
				var lstDetails = _bankTransferDetailService.GetBankTransfeById(TObject.Id);
				if (lstDetails.Any())
				{
					foreach (BankTransferDetailModel detail in TObject.BankTransferDetailsModel)
					{
						var data = lstDetails.Where(c => c.Id == detail.Id).FirstOrDefault();
						if (data != null)
						{
							FillBankTransferDetail(data, detail);
							data.BankTransferId = TObject.Id;
							data.ObjectState = ObjectState.Modified;
							_bankTransferDetailService.Update(data);
						}

					}
				}
			}
			catch (Exception ex)
			{
				LoggingHelper.LogError(BankTransferLoggingValidation.BankTransferApplicationService, ex, ex.Message);
				throw ex;
			}
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Exited_From_UpdateCashsale_Method);
		}

		private void FillBankTransferDetail(BankTransferDetail banktransferNew, BankTransferDetailModel detail)
		{
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Entered_Into_Fill_Method_Of_Fill_BankTransferDeatil_Method);
			banktransferNew.Amount = detail.Amount;
			banktransferNew.BankClearingDate = detail.BankClearingDate;
			banktransferNew.BankTransferId = detail.BankTransferId;
			banktransferNew.COAId = detail.COAId;
			banktransferNew.Currency = detail.Currency;
			banktransferNew.ServiceCompanyId = detail.ServiceCompanyId;
			banktransferNew.Type = detail.Type;
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Exited_From_FillBankTranferDetail_Method);
		}

		#endregion

		#region GenerateAutoNumberForType
		string value = "";
		//public string GenerateAutoNumberForType(long companyId, string Type, string DocSubType)
		//{

		//    AppsWorld.BankTransferModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, Type);
		//    string generatedAutoNumber = "";
		//    try
		//    {
		//        if (Type == DocTypeConstants.BankTransfer)
		//        {
		//            generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength),
		//                _autoNo.GeneratedNumber, companyId, DocSubType);

		//            if (_autoNo != null)
		//            {
		//                _autoNo.GeneratedNumber = value;
		//                _autoNo.IsDisable = true;
		//                _autoNo.ObjectState = ObjectState.Modified;
		//                _autoNumberService.Update(_autoNo);
		//            }
		//            var _autonumberCompany = _autoNumberCompanyService.GetAutoNumberCompany(_autoNo.Id);
		//            if (_autonumberCompany.Any())
		//            {
		//                AppsWorld.BankTransferModule.Entities.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
		//                _autoNumberCompanyNew.GeneratedNumber = value;
		//                _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
		//                _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
		//                _autoNumberCompanyService.Update(_autoNumberCompanyNew);
		//            }
		//            else
		//            {
		//                AppsWorld.BankTransferModule.Entities.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.BankTransferModule.Entities.AutoNumberCompany();
		//                _autoNumberCompanyNew.GeneratedNumber = value;
		//                _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
		//                _autoNumberCompanyNew.Id = Guid.NewGuid();
		//                _autoNumberCompanyNew.ObjectState = ObjectState.Added;
		//                _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
		//            }
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        LoggingHelper.LogError(BankTransferLoggingValidation.BankTransferApplicationService, ex, ex.Message);
		//        throw ex;
		//    }
		//    return generatedAutoNumber;
		//}
		#endregion

		#region GenerateFromFormat
		public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal,
		 long companyId, string Companycode = null)
		{
			List<BankTransfer> lstbankTransfer = null;
			int? currentMonth = 0;
			bool ifMonthContains = false;
			string OutputNumber = "";
			string counter = "";
			string companyFormatHere = companyFormatFrom.ToUpper();

			if (companyFormatHere.Contains("{YYYY}"))
			{
				companyFormatHere = companyFormatHere.Replace("{YYYY}", DateTime.Now.Year.ToString());
			}
			else if (companyFormatHere.Contains("{MM/YYYY}"))
			{
				companyFormatHere = companyFormatHere.Replace("{MM/YYYY}",
					string.Format("{0:00}", DateTime.Now.Month) + "/" + DateTime.Now.Year.ToString());
				currentMonth = DateTime.Now.Month;
				ifMonthContains = true;
			}
			else if (companyFormatHere.Contains("{COMPANYCODE}"))
			{
				companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
			}
			double val = 0;
			if (Type == DocTypeConstants.BankTransfer)
			{
				lstbankTransfer = _bankTransferService.GetAllBankTransfer(companyId);

				if (lstbankTransfer.Any() && ifMonthContains)
				{
					int? lastCretedDate = lstbankTransfer.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
					if (DateTime.Now.Year == lstbankTransfer.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
					{
						if (lastCretedDate == currentMonth)
						{
							AppsWorld.BankTransferModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
							foreach (var bill in lstbankTransfer)
							{
								if (bill.SystemRefNo != autonumber.Preview)
									val = Convert.ToInt32(IncreamentVal);
								else
								{
									val = Convert.ToInt32(IncreamentVal) + 1;
									break;
								}
							}
						}
						else
							val = 1;
					}
					else
						val = 1;

				}

				if (lstbankTransfer.Any() && ifMonthContains == false)
				{
					AppsWorld.BankTransferModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
					foreach (var bill in lstbankTransfer)
					{
						if (bill.SystemRefNo != autonumber.Preview)
							val = Convert.ToInt32(IncreamentVal);
						else
						{
							val = Convert.ToInt32(IncreamentVal) + 1;
							break;
						}
					}
				}
				else
				{
					val = Convert.ToInt32(IncreamentVal);
				}
			}
			if (counterLength == 1)
				counter = string.Format("{0:0}", val);
			else if (counterLength == 2)
				counter = string.Format("{0:00}", val);
			else if (counterLength == 3)
				counter = string.Format("{0:000}", val);
			else if (counterLength == 4)
				counter = string.Format("{0:0000}", val);
			else if (counterLength == 5)
				counter = string.Format("{0:00000}", val);
			else if (counterLength == 6)
				counter = string.Format("{0:000000}", val);
			else if (counterLength == 7)
				counter = string.Format("{0:0000000}", val);
			else if (counterLength == 8)
				counter = string.Format("{0:00000000}", val);
			else if (counterLength == 9)
				counter = string.Format("{0:000000000}", val);
			else if (counterLength == 10)
				counter = string.Format("{0:0000000000}", val);

			value = counter;
			OutputNumber = companyFormatHere + counter;

			if (lstbankTransfer.Any())
			{
				OutputNumber = GetNewNumber(lstbankTransfer, Type, OutputNumber, counter, companyFormatHere, counterLength);
			}
			return OutputNumber;
		}
		private string GetNewNumber(List<BankTransfer> lstCashsale, string type, string outputNumber, string counter, string format, int counterLength)
		{
			string val1 = outputNumber;
			string val2 = "";
			var invoice = lstCashsale.Where(a => a.SystemRefNo == outputNumber).FirstOrDefault();
			bool isNotexist = false;
			int i = Convert.ToInt32(counter);
			if (invoice != null)
			{
				while (isNotexist == false)
				{
					i++;
					string length = i.ToString();
					value = length.PadLeft(counterLength, '0');
					val2 = format + value;
					var inv = lstCashsale.Where(c => c.SystemRefNo == val2).FirstOrDefault();
					if (inv == null)
						isNotexist = true;
				}
				val1 = val2;
			}
			return val1;
		}
		#endregion GenerateFromFormat

		#region Posting

		private void FillJournal(JVModel jvm, BankTransfer _bankTransfer, bool isNew, bool isWithdrawal)
		{
			var withdral = _bankTransfer.BankTransferDetails.Where(c => c.Type == "Withdrawal").FirstOrDefault();
			var deposit = _bankTransfer.BankTransferDetails.Where(c => c.Type == "Deposit").FirstOrDefault();
			if (isNew)
			{
				jvm.Id = Guid.NewGuid();
				jvm.CreatedDate = DateTime.UtcNow;
			}
			else
				jvm.Id = _bankTransfer.Id;
			jvm.DocumentId = _bankTransfer.Id;
			jvm.CompanyId = _bankTransfer.CompanyId;
			jvm.PostingDate = _bankTransfer.TransferDate;
			jvm.DocNo = _bankTransfer.DocNo;
			jvm.DocType = DocTypeConstants.BankTransfer;
			jvm.DocSubType = DocTypeConstants.General;
			jvm.DocumentDescription = _bankTransfer.DocDescription;
			jvm.SystemReferenceNo = _bankTransfer.SystemRefNo;
			jvm.DocDate = _bankTransfer.TransferDate;
			jvm.DocType = _bankTransfer.DocType;
			jvm.BaseCurrency = _bankTransfer.ExCurrency;
			jvm.ExDurationFrom = _bankTransfer.ExDurationFrom;
			jvm.ExDurationTo = _bankTransfer.ExDurationTo;
			jvm.ExCurrency = _bankTransfer.ExCurrency;
			jvm.Status = _bankTransfer.Status;
			jvm.DocumentState = _bankTransfer.DocumentState;
			jvm.ExchangeRate = (_bankTransfer.ExCurrency == withdral.Currency) || (_bankTransfer.ExCurrency == deposit.Currency) ? 1 : _bankTransfer.ExchangeRate;
			jvm.IsNoSupportingDocument = _bankTransfer.IsNoSupportingDocument.Value;
			jvm.NoSupportingDocument = _bankTransfer.NoSupportingDocument;
			jvm.IsMultiCurrency = _bankTransfer.IsMultiCurrency;
			jvm.VarianceExchangeRate = _bankTransfer.VarianceExchangeRate;
			jvm.SystemCalculatedExchangeRate = _bankTransfer.SystemCalculatedExchangeRate;
			jvm.ModeOfReceipt = _bankTransfer.ModeOfTransfer;
			jvm.TransferRefNo = _bankTransfer.TransferRefNo;
			jvm.IsWithdrawal = isWithdrawal;
			jvm.ActualSysRefNo = _bankTransfer.SystemRefNo;
			jvm.IsFirst = true;
			jvm.ModifiedDate = _bankTransfer.ModifiedDate;
			jvm.UserCreated = _bankTransfer.UserCreated;
			jvm.CreatedDate = _bankTransfer.CreatedDate;
			jvm.ModifiedBy = _bankTransfer.ModifiedBy;
			if (isWithdrawal)
				jvm.GrandDocDebitTotal = withdral.Amount;
			else
				jvm.GrandDocDebitTotal = deposit.Amount;
			jvm.GrandBaseDebitTotal = Math.Round((decimal)(jvm.GrandDocDebitTotal * (_bankTransfer.ExchangeRate != null ? _bankTransfer.ExchangeRate : 1)), 2);
			jvm.ServiceCompanyId = isWithdrawal ? withdral.ServiceCompanyId : deposit.ServiceCompanyId;
			jvm.COAId = isWithdrawal ? withdral.COAId : deposit.COAId;
			if (isWithdrawal)
			{
				List<JVVDetailModel> lstjvdm = new List<JVVDetailModel>();
				foreach (BankTransferDetail detail in _bankTransfer.BankTransferDetails)
				{
					JVVDetailModel jvdetail = new JVVDetailModel();
					jvdetail.RecOrder = detail.RecOrder;
					jvdetail.Id = detail.Id;
					jvdetail.DocType = DocTypeConstants.BankTransfer;
					jvdetail.DocSubType = DocTypeConstants.General;
					jvdetail.AccountDescription = _bankTransfer.DocDescription;
					jvdetail.ServiceCompanyId = detail.ServiceCompanyId;
					jvdetail.COAId = detail.COAId;
					jvdetail.DocumentId = _bankTransfer.Id;
					jvdetail.BaseCurrency = jvm.BaseCurrency;
					jvdetail.PostingDate = _bankTransfer.TransferDate;
					jvdetail.DocDate = _bankTransfer.TransferDate;
					jvdetail.ExchangeRate = withdral.Currency == deposit.Currency ? 1 : _bankTransfer.ExchangeRate;
					jvdetail.DocCurrency = detail.Currency;
					if (detail.Type == "Withdrawal")
					{
						jvdetail.DocCredit = detail.Amount;
						decimal? exchangerate = 0;
						jvdetail.Type = detail.Type;
						if (_bankTransfer.ExCurrency == jvdetail.DocCurrency)
							exchangerate = 1;
						else
							exchangerate = _bankTransfer.SystemCalculatedExchangeRate == null ? 1 : _bankTransfer.SystemCalculatedExchangeRate == 0 ? _bankTransfer.ExchangeRate : _bankTransfer.SystemCalculatedExchangeRate;
						jvdetail.BaseCredit = exchangerate == 0
							? jvdetail.DocCredit
							: Math.Round((decimal)(jvdetail.DocCredit * exchangerate), 2, MidpointRounding.AwayFromZero);
						jvdetail.DocumentDetailId = detail.Id;
					}
					else
					{
						jvdetail.DocDebit = detail.Amount;
						jvdetail.Type = detail.Type;
						decimal? exchangerate1 = 0;
						if (_bankTransfer.ExCurrency == jvdetail.DocCurrency)
							exchangerate1 = 1;
						else
							exchangerate1 = _bankTransfer.SystemCalculatedExchangeRate == null ? 1 : _bankTransfer.SystemCalculatedExchangeRate == 0 ? _bankTransfer.ExchangeRate : _bankTransfer.SystemCalculatedExchangeRate;
						jvdetail.BaseDebit = exchangerate1 == 0
							? jvdetail.DocDebit
							: Math.Round((decimal)(jvdetail.DocDebit * exchangerate1), 2, MidpointRounding.AwayFromZero);
					}
					jvdetail.Type = detail.Type;
					lstjvdm.Add(jvdetail);
				}
				jvm.JVVDetailModels = lstjvdm.OrderBy(c => c.RecOrder).ToList();
			}
		}
		private void FillJournal1(JVModel jvm, BankTransfer _bankTransfer, bool isNew, bool isWithdrawal, bool isFirst)
		{

			if (isFirst)
				doc = _bankTransfer.SystemRefNo;

			//JVModel jvm = new JVModel();
			BankTransferDetail withdral = _bankTransfer.BankTransferDetails.Where(c => c.Type == "Withdrawal").FirstOrDefault();
			BankTransferDetail deposit = _bankTransfer.BankTransferDetails.Where(c => c.Type == "Deposit").FirstOrDefault();
			if (isNew)
			{
				jvm.Id = Guid.NewGuid();
				jvm.CreatedDate = DateTime.UtcNow;
			}
			else
				jvm.Id = _bankTransfer.Id;
			jvm.DocumentId = _bankTransfer.Id;
			jvm.CompanyId = _bankTransfer.CompanyId;
			jvm.PostingDate = _bankTransfer.TransferDate;
			jvm.DocNo = _bankTransfer.DocNo;
			jvm.DocType = DocTypeConstants.BankTransfer;
			//jvm.DocSubType = DocTypeConstants.Interco;
			jvm.DocSubType = DocTypeConstants.General;
			jvm.DocumentDescription = _bankTransfer.DocDescription;
			//jvm.Remarks = _bankTransfer.Remarks;
			jvm.SystemReferenceNo = doc = GetNextApplicationNumber(doc, isFirst, _bankTransfer.SystemRefNo);
			jvm.DocDate = _bankTransfer.TransferDate;
			jvm.BaseCurrency = _bankTransfer.ExCurrency;
			jvm.ExDurationFrom = _bankTransfer.ExDurationFrom;
			jvm.ExDurationTo = _bankTransfer.ExDurationTo;
			jvm.ExCurrency = _bankTransfer.ExCurrency;
			jvm.DocCurrency = isWithdrawal == true ? withdral.Currency : deposit.Currency;
			jvm.Status = _bankTransfer.Status;
			jvm.ActualSysRefNo = _bankTransfer.SystemRefNo;
			jvm.ExchangeRate = _bankTransfer.ExchangeRate;
			jvm.IsNoSupportingDocument = _bankTransfer.IsNoSupportingDocument.Value;
			jvm.NoSupportingDocument = _bankTransfer.NoSupportingDocument;
			jvm.IsMultiCurrency = _bankTransfer.IsMultiCurrency;
			jvm.VarianceExchangeRate = _bankTransfer.VarianceExchangeRate;
			jvm.SystemCalculatedExchangeRate = _bankTransfer.SystemCalculatedExchangeRate;
			jvm.ModeOfReceipt = _bankTransfer.ModeOfTransfer;
			jvm.TransferRefNo = _bankTransfer.TransferRefNo;
			jvm.IsWithdrawal = isWithdrawal;
			jvm.DocumentState = _bankTransfer.DocumentState;
			jvm.ModifiedDate = _bankTransfer.ModifiedDate;
			jvm.UserCreated = _bankTransfer.UserCreated;
			jvm.CreatedDate = _bankTransfer.CreatedDate;
			jvm.ModifiedBy = _bankTransfer.ModifiedBy;
			if (isWithdrawal)
				jvm.GrandDocCreditTotal = withdral.Amount;
			else
				jvm.GrandDocDebitTotal = deposit.Amount;
			jvm.ServiceCompanyId = isWithdrawal ? withdral.ServiceCompanyId : deposit.ServiceCompanyId;
			jvm.COAId = isWithdrawal ? withdral.COAId : deposit.COAId;

			FillBankTransferDetailMethod(jvm, _bankTransfer, isWithdrawal, isWithdrawal == true ? withdral : deposit, withdral, deposit);

			//List<JVVDetailModel> lstjvdm = new List<JVVDetailModel>();

			//int? recOrder = 0;
			//JVVDetailModel jvdetail = new JVVDetailModel();
			//BankTransferDetail detail = new BankTransferDetail();
			//var details = _bankTransfer.BankTransferDetails;
			//if (isWithdrawal)
			//{
			//    detail = details.Where(c => c.Type == "Withdrawal").FirstOrDefault();
			//    jvdetail.DocCredit = detail.Amount;
			//}
			//else
			//{
			//    detail = details.Where(c => c.Type != "Withdrawal").FirstOrDefault();
			//    jvdetail.DocDebit = detail.Amount;
			//}
			//jvdetail.RecOrder = detail.RecOrder;
			//jvdetail.Id = detail.Id;
			//jvdetail.DocumentDetailId = detail.Id;
			//jvdetail.ServiceCompanyId = detail.ServiceCompanyId;
			//jvdetail.COAId = detail.COAId;
			//jvdetail.DocumentId = _bankTransfer.Id;
			//jvdetail.BaseCurrency = detail.Currency;
			//jvdetail.DocCurrency = detail.Currency;
			//decimal? exchangerate = 0;
			//jvdetail.Type = detail.Type;
			//if (_bankTransfer.ExCurrency == jvdetail.DocCurrency)
			//    exchangerate = 1;
			//else
			//    exchangerate = _bankTransfer.SystemCalculatedExchangeRate == null ? 1 : _bankTransfer.SystemCalculatedExchangeRate == 0 ? _bankTransfer.ExchangeRate : _bankTransfer.SystemCalculatedExchangeRate;
			//if (isWithdrawal)
			//{
			//    jvdetail.BaseCredit = exchangerate == 0
			//        ? jvdetail.DocCredit
			//        : (jvdetail.DocCredit * exchangerate);

			//}
			//else
			//{
			//    jvdetail.BaseDebit = exchangerate == 0
			//                ? jvdetail.DocDebit
			//                : (jvdetail.DocDebit * exchangerate);
			//}
			//lstjvdm.Add(jvdetail);
			//JVVDetailModel jvdetailmodel = new JVVDetailModel();
			////jvdetail.RecOrder = recOrder + 1;
			////recOrder = jvdetail.RecOrder;
			//jvdetail.RecOrder = detail.RecOrder;
			//jvdetailmodel.Id = detail.Id;
			//jvdetailmodel.ServiceCompanyId = detail.ServiceCompanyId;
			//jvdetailmodel.COAId = detail.COAId;
			//jvdetailmodel.DocumentDetailId = detail.Id;
			//var coa = _chartOfAccountService.GetChartOfAccountByName(COANameConstants.Clearing,
			//    _bankTransfer.CompanyId);
			//if (coa != null)
			//    jvdetailmodel.COAId = coa.Id;
			//jvdetailmodel.BaseCurrency = detail.Currency;
			//jvdetailmodel.DocCurrency = detail.Currency;
			//jvdetailmodel.Type = detail.Type;

			//decimal? exchangerate2 = 0;
			//if (_bankTransfer.ExCurrency == jvdetailmodel.DocCurrency)
			//    exchangerate2 = 1;
			//else
			//    exchangerate2 = _bankTransfer.SystemCalculatedExchangeRate == null ? 1 : _bankTransfer.SystemCalculatedExchangeRate == 0 ? _bankTransfer.ExchangeRate : _bankTransfer.SystemCalculatedExchangeRate;
			////jvdetailmodel.Type = "Deposit";
			//if (coa != null)
			//    jvdetailmodel.AccountName = COANameConstants.Clearing + "-" + coa.Name;
			//if (isWithdrawal)
			//{
			//    jvdetailmodel.DocDebit = detail.Amount;
			//    jvdetailmodel.BaseDebit = exchangerate2 == 0
			//                ? jvdetailmodel.DocDebit
			//                : (jvdetailmodel.DocDebit * exchangerate2);
			//}
			//else
			//{
			//    jvdetailmodel.DocCredit = detail.Amount;
			//    jvdetailmodel.BaseCredit = exchangerate2 == 0
			//            ? jvdetailmodel.DocCredit
			//            : (jvdetailmodel.DocCredit * exchangerate2);
			//}
			//lstjvdm.Add(jvdetailmodel);
			//jvm.JVVDetailModels = lstjvdm.OrderBy(c => c.RecOrder).ToList();
		}


		//private void filljournal(jvmodel jvm, banktransfer _banktransfer, bool isnew)
		//{
		//    //jvmodel jvm = new jvmodel();

		//    if (isnew)
		//        jvm.id = guid.newguid();
		//    else
		//        jvm.id = _banktransfer.id;
		//    jvm.documentid = _banktransfer.id;
		//    jvm.companyid = _banktransfer.companyid;
		//    jvm.postingdate = _banktransfer.transferdate;
		//    jvm.docno = _banktransfer.docno;
		//    jvm.doctype = doctypeconstants.banktransfer;
		//    jvm.documentdescription = _banktransfer.docdescription;
		//    jvm.remarks = _banktransfer.remarks;
		//    jvm.systemreferenceno = _banktransfer.systemrefno;
		//    jvm.docdate = _banktransfer.transferdate;
		//    jvm.doctype = _banktransfer.doctype;
		//    jvm.basecurrency = _banktransfer.excurrency;
		//    jvm.exdurationfrom = _banktransfer.exdurationfrom;
		//    jvm.exdurationto = _banktransfer.exdurationto;
		//    jvm.excurrency = _banktransfer.excurrency;
		//    jvm.status = _banktransfer.status;
		//    jvm.exchangerate = _banktransfer.exchangerate;
		//    jvm.isnosupportingdocument = _banktransfer.isnosupportingdocument.value;
		//    jvm.nosupportingdocument = _banktransfer.nosupportingdocument;
		//    jvm.ismulticurrency = _banktransfer.ismulticurrency;
		//    jvm.varianceexchangerate = _banktransfer.varianceexchangerate;
		//    jvm.systemcalculatedexchangerate = _banktransfer.systemcalculatedexchangerate;
		//    jvm.modeofreceipt = _banktransfer.modeoftransfer;
		//    jvm.transferrefno = _banktransfer.transferrefno;

		//    list<jvvdetailmodel> lstjvdm = new list<jvvdetailmodel>();

		//    var withdral = _banktransfer.banktransferdetails.where(c => c.type == "withdrawal").firstordefault();
		//    var deposit = _banktransfer.banktransferdetails.where(c => c.type == "deposit").firstordefault();

		//    foreach (banktransferdetail detail in _banktransfer.banktransferdetails)
		//    {
		//        jvvdetailmodel jvdetail = new jvvdetailmodel();
		//        int? recorder = 0;
		//        jvdetail.recorder = recorder + 1;
		//        recorder = jvdetail.recorder;
		//        jvdetail.id = detail.id;
		//        jvdetail.documentdetailid = detail.id;
		//        jvdetail.servicecompanyid = detail.servicecompanyid;
		//        jvdetail.coaid = detail.coaid;
		//        jvdetail.documentid = _banktransfer.id;
		//        jvdetail.basecurrency = detail.currency;
		//        jvdetail.doccurrency = detail.currency;
		//        if (detail.type == "withdrawal")
		//        {
		//            jvdetail.doccredit = detail.amount;
		//            jvdetail.basecredit = _banktransfer.exchangerate == null ? jvdetail.doccredit : (jvdetail.doccredit * _banktransfer.exchangerate);
		//        }
		//        else
		//        {
		//            jvdetail.docdebit = detail.amount;
		//            jvdetail.basedebit = _banktransfer.exchangerate == null ? jvdetail.docdebit : (jvdetail.docdebit * _banktransfer.exchangerate);
		//        }
		//        jvdetail.type = detail.type;
		//        lstjvdm.add(jvdetail);
		//        if (withdral.servicecompanyid != deposit.servicecompanyid)
		//        {
		//            jvvdetailmodel jvdetailmodel = new jvvdetailmodel();
		//            jvdetail.recorder = recorder + 1;
		//            recorder = jvdetail.recorder;
		//            jvdetailmodel.id = detail.id;
		//            jvdetailmodel.servicecompanyid = detail.servicecompanyid;
		//            jvdetailmodel.coaid = detail.coaid;
		//            jvdetailmodel.basecurrency = detail.currency;
		//            jvdetailmodel.doccurrency = detail.currency;
		//            if (detail.type == "withdrawal")
		//            {
		//                jvdetailmodel.doccredit = detail.amount;
		//                jvdetailmodel.basecredit = _banktransfer.exchangerate == null ? jvdetailmodel.doccredit : (jvdetailmodel.doccredit * _banktransfer.exchangerate);
		//            }
		//            else
		//            {
		//                jvdetailmodel.docdebit = detail.amount;
		//                jvdetailmodel.basedebit = _banktransfer.exchangerate == null ? jvdetailmodel.docdebit : (jvdetailmodel.docdebit * _banktransfer.exchangerate);
		//            }
		//            jvdetailmodel.type = detail.type;
		//            lstjvdm.add(jvdetailmodel);
		//        }
		//    }
		//    jvm.jvvdetailmodels = lstjvdm;
		//}
		public void SaveInvoice1(JVModel jvModel)
		{
			LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, BankTransferLoggingValidation.Entering_into_rest_sharp_method);
			var json = RestSharpHelper.ConvertObjectToJason(jvModel);
			try
			{
				string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
				object obj = jvModel;
				var response = RestSharpHelper.Post(url, "api/journal/saveposting", json);
				if (response.ErrorMessage != null)
				{
					Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
				}
			}
			catch (Exception ex)
			{
				LoggingHelper.LogError(BankTransferLoggingValidation.BankTransferApplicationService, ex, ex.Message);
				var message = ex.Message;
			}
		}
		private string GetNextApplicationNumber(string sysNumber, bool isFirst, string originalSysNumber)
		{
			string DocNumber = "";
			try
			{
				int DocNo = 0;
				if (!isFirst)
				{
					DocNo = Convert.ToInt32(sysNumber.Substring(sysNumber.LastIndexOf("-JV") + 3));
				}
				DocNo++;
				DocNumber = originalSysNumber + ("-JV" + DocNo);
			}
			catch (Exception ex)
			{
				LoggingHelper.LogError(BankTransferLoggingValidation.BankTransferApplicationService, ex, ex.Message);
				throw ex;
			}

			return DocNumber;
		}

		private void FillBankTransferDetailMethod(JVModel jvm, BankTransfer _bankTransfer, bool isWithdrawal, BankTransferDetail detail, BankTransferDetail withdrawal, BankTransferDetail deposit)
		{
			List<JVVDetailModel> lstjvdm = new List<JVVDetailModel>();

			//int? recOrder = 0;
			JVVDetailModel jvdetail = new JVVDetailModel();
			//var details = _bankTransfer.BankTransferDetails;
			//foreach (var detail in _bankTransfer.BankTransferDetails.OrderBy(c => c.RecOrder))
			//{
			// BankTransferDetail detail = new BankTransferDetail();
			jvdetail = new JVVDetailModel();
			//if (isWithdrawal)
			//{
			//    detail = details.Where(c => c.Type == "Withdrawal").FirstOrDefault();
			//    jvdetail.DocCredit = detail.Amount;
			//}
			//else
			//{
			//    detail = details.Where(c => c.Type != "Withdrawal").FirstOrDefault();
			//    jvdetail.DocDebit = detail.Amount;
			//}
			jvdetail.RecOrder = detail.RecOrder;
			jvdetail.Id = Guid.NewGuid();
			jvdetail.DocumentDetailId = detail.Id;
			jvdetail.ServiceCompanyId = detail.ServiceCompanyId;
			jvdetail.COAId = detail.COAId;
			jvdetail.DocType = _bankTransfer.DocType;
			//jvdetail.DocSubType = DocTypeConstants.Interco;
			jvdetail.DocSubType = DocTypeConstants.General;
			jvdetail.AccountDescription = _bankTransfer.DocDescription;
			jvdetail.DocumentId = _bankTransfer.Id;
			jvdetail.PostingDate = _bankTransfer.TransferDate;
			jvdetail.DocDate = _bankTransfer.TransferDate;
			jvdetail.BaseCurrency = _bankTransfer.ExCurrency;
			jvdetail.DocCurrency = detail.Currency;
			decimal? exchangerate = 0;
			decimal? Calculatedexerate = 0;
			jvdetail.Type = detail.Type;
			if (_bankTransfer.ExCurrency == jvdetail.DocCurrency)
				exchangerate = 1;
			else
			{
				exchangerate = _bankTransfer.SystemCalculatedExchangeRate == null ? 1 : _bankTransfer.SystemCalculatedExchangeRate == 0 ? _bankTransfer.ExchangeRate : _bankTransfer.SystemCalculatedExchangeRate;
				jvdetail.ExchangeRate = exchangerate;
				if (_bankTransfer.ExCurrency == withdrawal.Currency)
					Calculatedexerate = withdrawal.Amount / deposit.Amount;
				else
					Calculatedexerate = deposit.Amount / withdrawal.Amount;
				exchangerate = _bankTransfer.SystemCalculatedExchangeRate == null ? 1 : _bankTransfer.SystemCalculatedExchangeRate == 0 ? _bankTransfer.ExchangeRate : Calculatedexerate;
			}
			if (/*_bankTransfer.ExCurrency != detail.Currency*/isWithdrawal)
			{
				jvdetail.DocCredit = detail.Amount;
				jvdetail.BaseCredit = Math.Round(exchangerate == 0
					? (decimal)jvdetail.DocCredit
					: (decimal)(jvdetail.DocCredit * exchangerate), 2, MidpointRounding.AwayFromZero);
				jvdetail.DocDebit = null;
				jvdetail.BaseDebit = null;
			}

			else
			{
				jvdetail.DocDebit = detail.Amount;
				jvdetail.BaseDebit = Math.Round(exchangerate == 0
							? (decimal)jvdetail.DocDebit
							: (decimal)(jvdetail.DocDebit * exchangerate), 2, MidpointRounding.AwayFromZero);
				jvdetail.DocCredit = null;
				jvdetail.BaseCredit = null;
			}
			lstjvdm.Add(jvdetail);
			JVVDetailModel jvdetailmodel = new JVVDetailModel();
			//jvdetail.RecOrder = recOrder + 1;
			//recOrder = jvdetail.RecOrder;
			jvdetail.RecOrder = lstjvdm.Max(c => c.RecOrder) + 1;
			jvdetailmodel.Id = Guid.NewGuid();
			jvdetailmodel.ServiceCompanyId = detail.ServiceCompanyId;
			jvdetailmodel.COAId = detail.COAId;
			jvdetailmodel.DocType = _bankTransfer.DocType;
			jvdetailmodel.PostingDate = _bankTransfer.TransferDate;
			jvdetailmodel.DocDate = _bankTransfer.TransferDate;
			jvdetailmodel.DocumentId = _bankTransfer.Id;
			//jvdetailmodel.DocumentDetailId = detail.Id;
			if (withdrawal.ServiceCompanyId == deposit.ServiceCompanyId && withdrawal.Currency != deposit.Currency)
			{
				jvdetailmodel.COAId = _chartOfAccountService.GetChartOfAccountIDByName(/*COANameConstants.Clearing*/COANameConstants.Clearing_Transfer,
					_bankTransfer.CompanyId);
				//if (coa != null)
				//    jvdetailmodel.COAId = coa.Id;
			}
			else
			{
				string shotCode = _companyService.Query(a => a.Id == (isWithdrawal == true ? deposit.ServiceCompanyId : withdrawal.ServiceCompanyId)).Select(a => a.ShortName).FirstOrDefault();
				shotCode = "I/C" + " - " + shotCode;
				jvdetailmodel.COAId = _chartOfAccountService.Query(a => a.Name == shotCode
					&& a.CompanyId == _bankTransfer.CompanyId && a.SubsidaryCompanyId == (isWithdrawal == true ? deposit.ServiceCompanyId : withdrawal.ServiceCompanyId)).Select(a => a.Id).FirstOrDefault();
				//if (chartOfAccount != null)
				//    jvdetailmodel.COAId = chartOfAccount.Id;
			}
			jvdetailmodel.BaseCurrency = _bankTransfer.ExCurrency;
			jvdetailmodel.DocCurrency = detail.Currency;
			jvdetailmodel.Type = detail.Type;
			jvdetailmodel.AccountDescription = _bankTransfer.DocDescription;
			decimal? exchangerate2 = 0;
			if (_bankTransfer.ExCurrency == jvdetailmodel.DocCurrency)
				exchangerate2 = 1;
			else
			{
				exchangerate2 = _bankTransfer.SystemCalculatedExchangeRate == null ? 1 : _bankTransfer.SystemCalculatedExchangeRate == 0 ? _bankTransfer.ExchangeRate : _bankTransfer.SystemCalculatedExchangeRate;
				jvdetailmodel.ExchangeRate = exchangerate2;

				if (_bankTransfer.ExCurrency == withdrawal.Currency)
					Calculatedexerate = withdrawal.Amount / deposit.Amount;
				else
					Calculatedexerate = deposit.Amount / withdrawal.Amount;
				exchangerate2 = _bankTransfer.SystemCalculatedExchangeRate == null ? 1 : _bankTransfer.SystemCalculatedExchangeRate == 0 ? _bankTransfer.ExchangeRate : Calculatedexerate;
			}
			//jvdetailmodel.Type = "Deposit";
			//if (coa != null)
			//    jvdetailmodel.AccountName = COANameConstants.Clearing + "-" + coa.Name;
			if (isWithdrawal)
			{
				jvdetailmodel.DocDebit = detail.Amount;
				jvdetailmodel.BaseDebit = Math.Round(exchangerate2 == 0
							? (decimal)jvdetailmodel.DocDebit
							: (decimal)(jvdetailmodel.DocDebit * exchangerate2), 2, MidpointRounding.AwayFromZero);
				jvdetailmodel.DocCredit = null;
				jvdetailmodel.BaseCredit = null;
			}
			else
			{
				jvdetailmodel.DocCredit = detail.Amount;
				jvdetailmodel.BaseCredit = Math.Round(exchangerate2 == 0
						? (decimal)jvdetailmodel.DocCredit
						: (decimal)(jvdetailmodel.DocCredit * exchangerate2), 2, MidpointRounding.AwayFromZero);
				jvdetailmodel.DocDebit = null;
				jvdetailmodel.BaseDebit = null;
			}
			lstjvdm.Add(jvdetailmodel);
			//  }
			jvm.GrandDocDebitTotal = Math.Round((decimal)lstjvdm.Sum(c => c.DocDebit), 2);
			jvm.GrandDocCreditTotal = Math.Round((decimal)lstjvdm.Sum(c => c.DocCredit), 2);
			jvm.GrandBaseDebitTotal = Math.Round((decimal)lstjvdm.Sum(c => c.BaseDebit), 2);
			jvm.GrandBaseCreditTotal = Math.Round((decimal)lstjvdm.Sum(c => c.BaseCredit), 2);
			jvm.JVVDetailModels = lstjvdm.OrderBy(c => c.RecOrder).ToList();
		}
		public void deleteJVPostBT(DocumentVoidModel tObject)
		{
			//LoggingHelper.LogMessage(BankTransferLoggingValidation.BankTransferApplicationService, tObject);
			var json = RestSharpHelper.ConvertObjectToJason(tObject);
			try
			{
				string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
				//ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
				//if (section.Ziraff.Count > 0)
				//{
				//    for (int i = 0; i < section.Ziraff.Count; i++)
				//    {
				//        if (section.Ziraff[i].Name == BankTransferConstants.IdentityBean)
				//        {
				//            url = section.Ziraff[i].ServerUrl;
				//            break;
				//        }
				//    }
				//}
				object obj = tObject;
				// string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
				var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
				if (response.ErrorMessage != null)
				{
					Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
				}
			}
			catch (Exception ex)
			{
				LoggingHelper.LogError(BankTransferLoggingValidation.BankTransferApplicationService, ex, ex.Message);

				var message = ex.Message;
			}
		}
		#endregion

		#region Attachment_Rest_client_call
		public void SaveTailsAttachments(long CompanyId, string path, string usercreated, List<TailsModel> lsttailsattachments)
		{
			Tails tails = new Tails();
			tails.FileShareName = CompanyId;
			tails.CompanyId = CompanyId;
			tails.Path = path;
			tails.LstTailsModel = lsttailsattachments;
			tails.CursorName = "Bean";
			if (tails.LstTailsModel.Count() > 0)
			{
				var json = RestHelper.ConvertObjectToJason(tails);
				try
				{
					string url = ConfigurationManager.AppSettings["AzureUrl"];
					var response = RestSharpHelper.Post(url, "api/storage/tailsaddmodesave", json);
					if (response.StatusCode == HttpStatusCode.OK)
					{
						var data = JsonConvert.DeserializeObject<Tails>(response.Content);
					}
					else
					{
						throw new Exception(response.Content);
					}
				}
				catch (Exception ex)
				{
					var message = ex.Message;
				}
			}
		}
		#endregion


		#region Intercobilling
		public List<SettlementDetailModel> GetListOfSettlementDetails(long companyId, long withdrawalServiceEntityId, long depositServiceEntityId, Guid transferId, DateTime transferDate, string docCurrency)
		{
			try
			{
				Guid withDrawalEntityId = new Guid();
				Guid depositEntityId = new Guid();
				List<SettlementDetailModel> lstOfSettlementDetailModel = new List<SettlementDetailModel>();

				List<long?> lstServiceEntityIds = new List<long?>() { withdrawalServiceEntityId, depositServiceEntityId };
				Dictionary<long?, Guid> lstEntityIds = _beanEntityService.GetListOfEntityIdsandServiceEntityId(companyId, lstServiceEntityIds);
				withDrawalEntityId = lstEntityIds.Where(a => a.Key == depositServiceEntityId).Select(c => c.Value).FirstOrDefault();//eg for co2 we need co1 entityid
				depositEntityId = lstEntityIds.Where(a => a.Key == withdrawalServiceEntityId).Select(c => c.Value).FirstOrDefault();//eg for co1 we need co2 entityid
																																	//List<Bill> listOfBill = _billService.GetListOfBill(companyId, depositServiceEntityId, depositEntityId, transferDate, docCurrency);

				//List<Invoice> lstOfWithDrawalInv = _invoiceService.GetListOfInvoice(companyId, withdrawalServiceEntityId, withDrawalEntityId, transferDate, docCurrency);


				List<Invoice> LstOfInvoices = _invoiceService.GetListOfICInvoiceBySEIdandEntId(companyId, lstServiceEntityIds, lstEntityIds.Select(a => a.Value).ToList(), transferDate, docCurrency);
				List<DebitNote> lstOfDebitNotes = _debitNoteService.GetListOfICDNBySEIdandEntId(companyId, lstServiceEntityIds, lstEntityIds.Select(a => a.Value).ToList(), transferDate, docCurrency);

				List<SettlementDetail> lstSettlementDetail = _settlementDetailService.GetListOfSettlemetDetails(transferId, withdrawalServiceEntityId, depositServiceEntityId, docCurrency, transferDate);
				if (lstSettlementDetail.Any())
				{
					foreach (SettlementDetail detail in lstSettlementDetail)
					{
						#region commentedCode for Bill


						//if (detail.DocumentType == DocTypeConstants.Bills)
						//{
						//    Bill bill = listOfBill.Where(a => a.Id == detail.DocumentId && a.ServiceCompanyId == depositServiceEntityId && a.EntityId == depositEntityId && a.DocCurrency == docCurrency).FirstOrDefault();
						//    if (bill != null)
						//    {
						//        SettlementDetailModel detailModel = new SettlementDetailModel();
						//        detailModel.Id = detail.Id;
						//        detailModel.DocumentId = detail.DocumentId;
						//        detailModel.DocumentNo = bill.DocNo;
						//        detailModel.DocumentType = bill.DocType;
						//        detailModel.ServiceCompanyId = bill.ServiceCompanyId;
						//        detailModel.Currency = bill.DocCurrency;
						//        detailModel.DocumentAmmount = bill.GrandTotal;
						//        detailModel.AmmountDue = Math.Abs((decimal)detail.SettledAmount + (decimal)bill.BalanceAmount);
						//        detailModel.SettledAmount = Math.Abs((decimal)detail.SettledAmount);
						//        detailModel.DocumentState = detailModel.DocumentAmmount == detailModel.AmmountDue ? BankTransferConstants.Not_Paid : bill.DocumentState;
						//        detailModel.DocumentDate = bill.PostingDate;
						//        detailModel.ExchangeRate = bill.ExchangeRate;
						//        detailModel.Nature = bill.Nature;
						//        detailModel.SettlemetType = detail.SettlemetType;
						//        lstOfSettlementDetailModel.Add(detailModel);
						//    }
						//}
						#endregion

						if (detail.DocumentType == DocTypeConstants.Invoice)
						{
							Invoice invoice = LstOfInvoices.Where(a => a.Id == detail.DocumentId & a.ServiceCompanyId == withdrawalServiceEntityId && a.EntityId == withDrawalEntityId && a.DocCurrency == docCurrency).FirstOrDefault() ?? LstOfInvoices.Where(a => a.Id == detail.DocumentId & a.ServiceCompanyId == depositServiceEntityId && a.EntityId == depositEntityId && a.DocCurrency == docCurrency).FirstOrDefault();
							if (invoice != null)
							{
								SettlementDetailModel detailModel = new SettlementDetailModel();
								detailModel.Id = detail.Id;
								detailModel.DocumentId = detail.DocumentId;
								detailModel.DocumentNo = invoice.DocNo;
								detailModel.DocumentType = invoice.DocType;
								detailModel.ServiceCompanyId = invoice.ServiceCompanyId;
								detailModel.Currency = invoice.DocCurrency;
								detailModel.DocumentAmmount = invoice.GrandTotal;
								detailModel.AmmountDue = Math.Abs((decimal)detail.SettledAmount + invoice.BalanceAmount);
								detailModel.SettledAmount = Math.Abs((decimal)detail.SettledAmount);
								detailModel.DocumentState = detailModel.DocumentAmmount == detailModel.AmmountDue ? BankTransferConstants.Not_Paid : invoice.DocumentState;
								detailModel.DocumentDate = invoice.DocDate;
								detailModel.ExchangeRate = invoice.ExchangeRate;
								detailModel.Nature = invoice.Nature;
								detailModel.SettlemetType = detail.SettlemetType;
								lstOfSettlementDetailModel.Add(detailModel);
							}

						}
						else if (detail.DocumentType == DocTypeConstants.DebitNote)
						{
							DebitNote debitNote = lstOfDebitNotes.Where(a => a.Id == detail.DocumentId & a.ServiceCompanyId == withdrawalServiceEntityId && a.EntityId == withDrawalEntityId && a.DocCurrency == docCurrency).FirstOrDefault() ?? lstOfDebitNotes.Where(a => a.Id == detail.DocumentId & a.ServiceCompanyId == depositServiceEntityId && a.EntityId == depositEntityId && a.DocCurrency == docCurrency).FirstOrDefault();
							if (debitNote != null)
							{
								SettlementDetailModel detailModel = new SettlementDetailModel();
								detailModel.Id = detail.Id;
								detailModel.DocumentId = detail.DocumentId;
								detailModel.DocumentNo = debitNote.DocNo;
								detailModel.DocumentType = DocTypeConstants.DebitNote;
								detailModel.ServiceCompanyId = debitNote.ServiceCompanyId;
								detailModel.Currency = debitNote.DocCurrency;
								detailModel.DocumentAmmount = debitNote.GrandTotal;
								detailModel.AmmountDue = Math.Abs((decimal)detail.SettledAmount + debitNote.BalanceAmount);
								detailModel.SettledAmount = Math.Abs((decimal)detail.SettledAmount);
								detailModel.DocumentState = detailModel.DocumentAmmount == detailModel.AmmountDue ? BankTransferConstants.Not_Paid : debitNote.DocumentState;
								detailModel.DocumentDate = debitNote.DocDate;
								detailModel.ExchangeRate = debitNote.ExchangeRate;
								detailModel.Nature = debitNote.Nature;
								detailModel.SettlemetType = detail.SettlemetType;
								lstOfSettlementDetailModel.Add(detailModel);
							}

						}

					}
				}
				//if (listOfBill.Any())
				//{
				//    foreach (Bill bill in listOfBill.Where(a => a.DocumentState != "Fully Paid"))
				//    {
				//        SettlementDetailModel detail = lstOfSettlementDetailModel.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
				//        if (detail == null)
				//        {
				//            SettlementDetailModel detailModel = new SettlementDetailModel();
				//            detailModel.Id = new Guid();
				//            detailModel.DocumentId = bill.Id;
				//            detailModel.DocumentNo = bill.DocNo;
				//            detailModel.DocumentState = bill.DocumentState;
				//            detailModel.DocumentType = bill.DocType;
				//            detailModel.ServiceCompanyId = bill.ServiceCompanyId;
				//            detailModel.Currency = bill.DocCurrency;
				//            detailModel.DocumentAmmount = bill.GrandTotal;
				//            detailModel.AmmountDue = Math.Abs((decimal)bill.BalanceAmount);
				//            detailModel.DocumentDate = bill.PostingDate;
				//            detailModel.ExchangeRate = bill.ExchangeRate;
				//            detailModel.Nature = bill.Nature;
				//            detailModel.SettlemetType = BankTransferConstants.Withdrawal;
				//            lstOfSettlementDetailModel.Add(detailModel);
				//        }
				//    }
				//}
				if (LstOfInvoices.Any())
				{
					foreach (Invoice invoice in LstOfInvoices.Where(a => a.DocumentState != "Fully Paid" && a.ServiceCompanyId == withdrawalServiceEntityId && a.EntityId == withDrawalEntityId))
					{
						SettlementDetailModel detail = lstOfSettlementDetailModel.Where(a => a.DocumentId == invoice.Id).FirstOrDefault();
						if (detail == null)
						{
							SettlementDetailModel detailModel = new SettlementDetailModel();
							detailModel.Id = new Guid();
							detailModel.DocumentId = invoice.Id;
							detailModel.DocumentNo = invoice.DocNo;
							detailModel.DocumentState = invoice.DocumentState;
							detailModel.DocumentType = invoice.DocType;
							detailModel.ServiceCompanyId = invoice.ServiceCompanyId;
							detailModel.Currency = invoice.DocCurrency;
							detailModel.DocumentAmmount = invoice.GrandTotal;
							detailModel.AmmountDue = Math.Abs(invoice.BalanceAmount);
							detailModel.DocumentDate = invoice.DocDate;
							detailModel.ExchangeRate = invoice.ExchangeRate;
							detailModel.Nature = invoice.Nature;
							detailModel.SettlemetType = invoice.ServiceCompanyId == depositServiceEntityId ? BankTransferConstants.Withdrawal : BankTransferConstants.Deposit;
							lstOfSettlementDetailModel.Add(detailModel);
						}
					}
					foreach (Invoice invoice in LstOfInvoices.Where(a => a.DocumentState != "Fully Paid" && a.ServiceCompanyId == depositServiceEntityId && a.EntityId == depositEntityId))
					{
						SettlementDetailModel detail = lstOfSettlementDetailModel.Where(a => a.DocumentId == invoice.Id).FirstOrDefault();
						if (detail == null)
						{
							SettlementDetailModel detailModel = new SettlementDetailModel();
							detailModel.Id = new Guid();
							detailModel.DocumentId = invoice.Id;
							detailModel.DocumentNo = invoice.DocNo;
							detailModel.DocumentState = invoice.DocumentState;
							detailModel.DocumentType = invoice.DocType;
							detailModel.ServiceCompanyId = invoice.ServiceCompanyId;
							detailModel.Currency = invoice.DocCurrency;
							detailModel.DocumentAmmount = invoice.GrandTotal;
							detailModel.AmmountDue = Math.Abs(invoice.BalanceAmount);
							detailModel.DocumentDate = invoice.DocDate;
							detailModel.ExchangeRate = invoice.ExchangeRate;
							detailModel.Nature = invoice.Nature;
							detailModel.SettlemetType = invoice.ServiceCompanyId == depositServiceEntityId ? BankTransferConstants.Withdrawal : BankTransferConstants.Deposit;
							lstOfSettlementDetailModel.Add(detailModel);
						}
					}
				}
				if (lstOfDebitNotes.Any())
				{
					foreach (DebitNote debitNote in lstOfDebitNotes.Where(a => a.DocumentState != "Fully Paid" && a.ServiceCompanyId == withdrawalServiceEntityId && a.EntityId == withDrawalEntityId))
					{
						SettlementDetailModel detail = lstOfSettlementDetailModel.Where(a => a.DocumentId == debitNote.Id).FirstOrDefault();
						if (detail == null)
						{
							SettlementDetailModel detailModel = new SettlementDetailModel();
							detailModel.Id = new Guid();
							detailModel.DocumentId = debitNote.Id;
							detailModel.DocumentNo = debitNote.DocNo;
							detailModel.DocumentState = debitNote.DocumentState;
							detailModel.DocumentType = DocTypeConstants.DebitNote;
							detailModel.ServiceCompanyId = debitNote.ServiceCompanyId;
							detailModel.Currency = debitNote.DocCurrency;
							detailModel.DocumentAmmount = debitNote.GrandTotal;
							detailModel.AmmountDue = Math.Abs(debitNote.BalanceAmount);
							detailModel.DocumentDate = debitNote.DocDate;
							detailModel.ExchangeRate = debitNote.ExchangeRate;
							detailModel.Nature = debitNote.Nature;
							detailModel.SettlemetType = debitNote.ServiceCompanyId == depositServiceEntityId ? BankTransferConstants.Withdrawal : BankTransferConstants.Deposit;
							lstOfSettlementDetailModel.Add(detailModel);
						}
					}
					foreach (DebitNote debitNote in lstOfDebitNotes.Where(a => a.DocumentState != "Fully Paid" && a.ServiceCompanyId == depositServiceEntityId && a.EntityId == depositEntityId))
					{
						SettlementDetailModel detail = lstOfSettlementDetailModel.Where(a => a.DocumentId == debitNote.Id).FirstOrDefault();
						if (detail == null)
						{
							SettlementDetailModel detailModel = new SettlementDetailModel();
							detailModel.Id = new Guid();
							detailModel.DocumentId = debitNote.Id;
							detailModel.DocumentNo = debitNote.DocNo;
							detailModel.DocumentState = debitNote.DocumentState;
							detailModel.DocumentType = DocTypeConstants.DebitNote;
							detailModel.ServiceCompanyId = debitNote.ServiceCompanyId;
							detailModel.Currency = debitNote.DocCurrency;
							detailModel.DocumentAmmount = debitNote.GrandTotal;
							detailModel.AmmountDue = Math.Abs(debitNote.BalanceAmount);
							detailModel.DocumentDate = debitNote.DocDate;
							detailModel.ExchangeRate = debitNote.ExchangeRate;
							detailModel.Nature = debitNote.Nature;
							detailModel.SettlemetType = debitNote.ServiceCompanyId == depositServiceEntityId ? BankTransferConstants.Withdrawal : BankTransferConstants.Deposit;
							lstOfSettlementDetailModel.Add(detailModel);
						}
					}
				}

				//}
				return lstOfSettlementDetailModel.OrderBy(a => a.DocumentDate).ThenBy(c => c.DocumentNo).ToList();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void InsertOrUpdateSettlemntDetail(BankTransfer bankTransfer, List<SettlementDetailModel> lstSettlemntDetailModel, List<DocumentHistoryModel> lstOfDocModels, bool isEdit)
		{
			try
			{
				#region EditMode_if_we_change_service_entity_or_Date_changed
				if (isEdit == true)
				{
					List<Guid> lstOfDocIds = new List<Guid>();
					List<Invoice> lstOfEditInvoices = _invoiceService.GetListOfInvoicesByCompanyIdAndDocId(bankTransfer.CompanyId, bankTransfer.SettlementDetails.Where(a => a.DocumentType == DocTypeConstants.Invoice).Select(a => a.DocumentId.Value).ToList());
					List<DebitNote> lstOfEditDebitNotes = _debitNoteService.GetListOfDNsByCompanyIdAndDocId(bankTransfer.CompanyId, bankTransfer.SettlementDetails.Where(a => a.DocumentType == DocTypeConstants.DebitNote).Select(a => a.DocumentId.Value).ToList());

					List<Bill> lstOfEditBills = _billService.GetListOfBillsByInvoiceIds(bankTransfer.CompanyId, lstOfEditInvoices.Select(a => a.Id).ToList());

					lstOfDocIds.AddRange(bankTransfer.SettlementDetails.Where(a => a.DocumentType == DocTypeConstants.DebitNote || a.DocumentType == DocTypeConstants.DebitNote).Select(a => a.DocumentId.Value).ToList());
					lstOfDocIds.AddRange(lstOfEditBills.Select(a => a.Id).ToList());

					List<BankTransferModule.Entities.Models.Journal> lstOfEditJournals = _journalService.GetListOfJournalBYCompIdandDocId(bankTransfer.CompanyId, lstOfDocIds/*bankTransfer.SettlementDetails.Select(a => a.DocumentId.Value).ToList()*/);
					foreach (SettlementDetail detail in bankTransfer.SettlementDetails)
					{
						if (lstSettlemntDetailModel.Where(a => a.DocumentId == detail.DocumentId).Any() != true)
						{
							#region Commented_bill_code


							//if (detail.DocumentType == DocTypeConstants.Bills)
							//{
							//    Bill bill = lstOfEditBills.Where(a => a.Id == detail.DocumentId).FirstOrDefault();
							//    if (bill != null)
							//    {
							//        if (bill.GrandTotal != detail.DocumentAmmount)
							//            throw new Exception("Invalid Document");

							//        bill.BalanceAmount += detail.SettledAmount;


							//        if (bill.BalanceAmount > bill.GrandTotal)
							//            throw new Exception("Invalid Document");

							//        if (bill.GrandTotal == bill.BalanceAmount)
							//            bill.DocumentState = BankTransferConstants.Not_Paid;
							//        else if (bill.BalanceAmount == 0)
							//            bill.DocumentState = BankTransferConstants.Fully_Paid;
							//        else if (bill.GrandTotal != bill.BalanceAmount)
							//            bill.DocumentState = BankTransferConstants.Partial_Paid;
							//        bill.ModifiedBy = BankTransferConstants.System;
							//        bill.ModifiedDate = DateTime.UtcNow;
							//        bill.ObjectState = ObjectState.Modified;
							//        _billService.Update(bill);

							//        BankTransferModule.Entities.Models.Journal journal = lstOfEditJournals.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
							//        if (journal != null)
							//        {
							//            journal.BalanceAmount = bill.BalanceAmount;
							//            journal.DocumentState = bill.DocumentState;
							//            journal.ModifiedBy = bill.ModifiedBy;
							//            journal.ModifiedDate = bill.ModifiedDate;
							//            journal.ObjectState = ObjectState.Modified;
							//            _journalService.Update(journal);
							//        }
							//        try
							//        {
							//            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy, string.Empty, bankTransfer.TransferDate, 0);
							//            if (lstdocumet.Any())
							//                //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
							//                lstOfDocModels.AddRange(lstdocumet);
							//        }
							//        catch (Exception ex)
							//        { }

							//    }
							//    detail.ObjectState = ObjectState.Deleted;
							//}
							#endregion commented_bill_code  
							if (detail.DocumentType == DocTypeConstants.Invoice)
							{
								Invoice invoice = lstOfEditInvoices.Where(a => a.Id == detail.DocumentId).FirstOrDefault();
								if (invoice != null)
								{
									if (invoice.GrandTotal != detail.DocumentAmmount)
										throw new Exception("Invalid Document");

									invoice.BalanceAmount += detail.SettledAmount.Value;

									if (invoice.BalanceAmount > invoice.GrandTotal)
										throw new Exception("Invalid Document");

									if (invoice.GrandTotal == invoice.BalanceAmount)
										invoice.DocumentState = BankTransferConstants.Not_Paid;
									else if (invoice.BalanceAmount == 0)
										invoice.DocumentState = BankTransferConstants.Fully_Paid;
									else if (invoice.GrandTotal != invoice.BalanceAmount)
										invoice.DocumentState = BankTransferConstants.Partial_Paid;
									invoice.ModifiedBy = BankTransferConstants.System;
									invoice.ModifiedDate = DateTime.UtcNow;
									invoice.ObjectState = ObjectState.Modified;
									_invoiceService.Update(invoice);

									BankTransferModule.Entities.Models.Journal journal = lstOfEditJournals.Where(a => a.DocumentId == invoice.Id).FirstOrDefault();
									if (journal != null)
									{
										journal.BalanceAmount = invoice.BalanceAmount;
										journal.DocumentState = invoice.DocumentState;
										journal.ModifiedBy = invoice.ModifiedBy;
										journal.ModifiedDate = invoice.ModifiedDate;
										journal.ObjectState = ObjectState.Modified;
										_journalService.Update(journal);
									}
									try
									{
										List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy, string.Empty, bankTransfer.TransferDate, 0, 0);
										if (lstdocumet.Any())
											lstOfDocModels.AddRange(lstdocumet);
									}
									catch (Exception ex)
									{ }

									Bill bill = lstOfEditBills.Where(a => a.PayrollId == invoice.Id && a.CompanyId == invoice.CompanyId && a.Nature == BankTransferConstants.Interco).FirstOrDefault();
									if (bill != null)
									{
										if (bill.GrandTotal != detail.DocumentAmmount)
											throw new Exception("Invalid Document");

										bill.BalanceAmount += detail.SettledAmount;


										if (bill.BalanceAmount > bill.GrandTotal)
											throw new Exception("Invalid Document");

										if (bill.GrandTotal == bill.BalanceAmount)
											bill.DocumentState = BankTransferConstants.Not_Paid;
										else if (bill.BalanceAmount == 0)
											bill.DocumentState = BankTransferConstants.Fully_Paid;
										else if (bill.GrandTotal != bill.BalanceAmount)
											bill.DocumentState = BankTransferConstants.Partial_Paid;
										bill.ModifiedBy = BankTransferConstants.System;
										bill.ModifiedDate = DateTime.UtcNow;
										bill.ObjectState = ObjectState.Modified;
										_billService.Update(bill);

										BankTransferModule.Entities.Models.Journal billJournal = lstOfEditJournals.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
										if (billJournal != null)
										{
											billJournal.BalanceAmount = bill.BalanceAmount;
											billJournal.DocumentState = bill.DocumentState;
											billJournal.ModifiedBy = bill.ModifiedBy;
											billJournal.ModifiedDate = bill.ModifiedDate;
											billJournal.ObjectState = ObjectState.Modified;
											_journalService.Update(billJournal);
										}
										try
										{
											List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy, string.Empty, bankTransfer.TransferDate, 0, 0);
											if (lstdocumet.Any())
												lstOfDocModels.AddRange(lstdocumet);
										}
										catch (Exception ex)
										{ }
									}
									else
									{
										throw new Exception(string.Concat(BankTransferConstants.Correspending_Bill_Is_not_avilable, invoice.DocNo));
									}


								}
								detail.ObjectState = ObjectState.Deleted;
							}
							if (detail.DocumentType == DocTypeConstants.DebitNote)
							{
								DebitNote debitNote = lstOfEditDebitNotes.Where(a => a.Id == detail.DocumentId).FirstOrDefault();
								if (debitNote != null)
								{
									if (debitNote.GrandTotal != detail.DocumentAmmount)
										throw new Exception("Invalid Document");

									debitNote.BalanceAmount += detail.SettledAmount.Value;

									if (debitNote.BalanceAmount > debitNote.GrandTotal)
										throw new Exception("Invalid Document");

									if (debitNote.GrandTotal == debitNote.BalanceAmount)
										debitNote.DocumentState = BankTransferConstants.Not_Paid;
									else if (debitNote.BalanceAmount == 0)
										debitNote.DocumentState = BankTransferConstants.Fully_Paid;
									else if (debitNote.GrandTotal != debitNote.BalanceAmount)
										debitNote.DocumentState = BankTransferConstants.Partial_Paid;
									debitNote.ModifiedBy = BankTransferConstants.System;
									debitNote.ModifiedDate = DateTime.UtcNow;
									debitNote.ObjectState = ObjectState.Modified;
									_debitNoteService.Update(debitNote);

									BankTransferModule.Entities.Models.Journal journal = lstOfEditJournals.Where(a => a.DocumentId == debitNote.Id).FirstOrDefault();
									if (journal != null)
									{
										journal.BalanceAmount = debitNote.BalanceAmount;
										journal.DocumentState = debitNote.DocumentState;
										journal.ModifiedBy = debitNote.ModifiedBy;
										journal.ModifiedDate = debitNote.ModifiedDate;
										journal.ObjectState = ObjectState.Modified;
										_journalService.Update(journal);
									}
									try
									{
										List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, debitNote.CompanyId, debitNote.Id, DocTypeConstants.DebitNote, DocTypeConstants.Interco, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy, string.Empty, bankTransfer.TransferDate, 0, 0);

										if (lstdocumet.Any())
											lstOfDocModels.AddRange(lstdocumet);
									}
									catch (Exception ex)
									{ }

									Bill bill = lstOfEditBills.Where(a => a.PayrollId == debitNote.Id && a.CompanyId == debitNote.CompanyId && a.Nature == BankTransferConstants.Interco).FirstOrDefault();
									if (bill != null)
									{
										if (bill.GrandTotal != detail.DocumentAmmount)
											throw new Exception("Invalid Document");

										bill.BalanceAmount += detail.SettledAmount;


										if (bill.BalanceAmount > bill.GrandTotal)
											throw new Exception("Invalid Document");

										if (bill.GrandTotal == bill.BalanceAmount)
											bill.DocumentState = BankTransferConstants.Not_Paid;
										else if (bill.BalanceAmount == 0)
											bill.DocumentState = BankTransferConstants.Fully_Paid;
										else if (bill.GrandTotal != bill.BalanceAmount)
											bill.DocumentState = BankTransferConstants.Partial_Paid;
										bill.ModifiedBy = BankTransferConstants.System;
										bill.ModifiedDate = DateTime.UtcNow;
										bill.ObjectState = ObjectState.Modified;
										_billService.Update(bill);

										BankTransferModule.Entities.Models.Journal billJournal = lstOfEditJournals.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
										if (billJournal != null)
										{
											billJournal.BalanceAmount = bill.BalanceAmount;
											billJournal.DocumentState = bill.DocumentState;
											billJournal.ModifiedBy = bill.ModifiedBy;
											billJournal.ModifiedDate = bill.ModifiedDate;
											billJournal.ObjectState = ObjectState.Modified;
											_journalService.Update(billJournal);
										}
										try
										{
											List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy, string.Empty, bankTransfer.TransferDate, 0, 0);
											if (lstdocumet.Any())
												lstOfDocModels.AddRange(lstdocumet);
										}
										catch (Exception ex)
										{ }
									}
									else
									{
										throw new Exception(string.Concat(BankTransferConstants.Correspending_Bill_Is_not_avilable, debitNote.DocNo));
									}


								}
								detail.ObjectState = ObjectState.Deleted;
							}
						}
					}
				}
				#endregion


				List<Guid> lstOfDocumentIds = new List<Guid>();
				List<SettlementDetail> lstSeetlementDetails = _settlementDetailService.GetListOfSettlementDetails(bankTransfer.Id, lstSettlemntDetailModel.Where(a => a.RecordStatus != "Added" && a.RecordStatus != "Deleted").Select(a => a.Id).ToList());
				List<Invoice> lstOfInvoices = _invoiceService.GetListOfInvoicesByCompanyIdAndDocId(bankTransfer.CompanyId, lstSettlemntDetailModel.Where(a => a.DocumentType == DocTypeConstants.Invoice).Select(a => a.DocumentId.Value).ToList());
				List<DebitNote> lstOfDebitNotes = _debitNoteService.GetListOfDNsByCompanyIdAndDocId(bankTransfer.CompanyId, lstSettlemntDetailModel.Where(a => a.DocumentType == DocTypeConstants.DebitNote).Select(a => a.DocumentId.Value).ToList());

				List<Bill> lstOfBills = _billService.GetListOfBillsByInvoiceIds(bankTransfer.CompanyId, lstSettlemntDetailModel.Where(a => a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.DebitNote).Select(a => a.DocumentId.Value).ToList());

				lstOfDocumentIds.AddRange(lstSettlemntDetailModel.Where(a => a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.DebitNote).Select(a => a.DocumentId.Value).ToList());
				lstOfDocumentIds.AddRange(lstOfBills.Select(a => a.Id).ToList());

				List<BankTransferModule.Entities.Models.Journal> lstOfJournal = _journalService.GetListOfJournalBYCompIdandDocId(bankTransfer.CompanyId, lstOfDocumentIds);
				decimal amount = 0;
				bool isFirst = false;
				foreach (SettlementDetailModel detail in lstSettlemntDetailModel)
				{
					SettlementDetail settlementDetail = new SettlementDetail();
					if (detail.RecordStatus == "Added" && detail.SettledAmount > 0)
					{
						settlementDetail.Id = Guid.NewGuid();
						settlementDetail.BankTransferId = bankTransfer.Id;
						FillSettlementDetail(detail, settlementDetail);
						isFirst = true;
						amount = detail.SettledAmount.Value;
						UpdateDocumentsandJournalState(settlementDetail, lstOfBills, lstOfInvoices, lstOfJournal, amount, isFirst, bankTransfer, lstOfDocModels, lstOfDebitNotes);
						settlementDetail.ObjectState = ObjectState.Added;
						_settlementDetailService.Insert(settlementDetail);
					}
					else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
					{
						settlementDetail = lstSeetlementDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
						if (settlementDetail != null)
						{
							if (detail.SettledAmount == 0)
							{
								amount = settlementDetail.SettledAmount.Value;
								isFirst = false;
								settlementDetail.SettledAmount = 0;
								UpdateDocumentsandJournalState(settlementDetail, lstOfBills, lstOfInvoices, lstOfJournal, amount, isFirst, bankTransfer, lstOfDocModels, lstOfDebitNotes);
								settlementDetail.ObjectState = ObjectState.Deleted;
								_settlementDetailService.Delete(settlementDetail);
							}
							else
							{
								amount = settlementDetail.SettledAmount.Value;
								isFirst = false;
								FillSettlementDetail(detail, settlementDetail);
								UpdateDocumentsandJournalState(settlementDetail, lstOfBills, lstOfInvoices, lstOfJournal, amount, isFirst, bankTransfer, lstOfDocModels, lstOfDebitNotes);
								settlementDetail.ObjectState = ObjectState.Modified;
								_settlementDetailService.Update(settlementDetail);
							}
						}
					}
					else if (detail.RecordStatus == "Deleted")
					{
						settlementDetail = lstSeetlementDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
						if (settlementDetail != null)
						{
							settlementDetail.ObjectState = ObjectState.Deleted;
							_settlementDetailService.Delete(settlementDetail);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static void FillSettlementDetail(SettlementDetailModel detail, SettlementDetail settlementDetail)
		{
			settlementDetail.DocumentType = detail.DocumentType;
			settlementDetail.DocumentId = detail.DocumentId;
			settlementDetail.DocumentNo = detail.DocumentNo;
			settlementDetail.DocumentDate = detail.DocumentDate;
			settlementDetail.DocumentType = detail.DocumentType;
			settlementDetail.DocumentState = detail.DocumentState;
			settlementDetail.DocumentAmmount = detail.DocumentAmmount;
			settlementDetail.AmmountDue = detail.AmmountDue;
			settlementDetail.SettledAmount = detail.SettledAmount;
			settlementDetail.SettlemetType = detail.SettlemetType;
			settlementDetail.Currency = detail.Currency;
			settlementDetail.ServiceCompanyId = detail.ServiceCompanyId;
			settlementDetail.ExchangeRate = detail.ExchangeRate;
		}

		private void UpdateDocumentsandJournalState(SettlementDetail detail, List<Bill> lstOfBills, List<Invoice> lstOfInvoices, List<BankTransferModule.Entities.Models.Journal> lstOfJournals, decimal amount, bool isfirst, BankTransfer transfer, List<DocumentHistoryModel> lstDocHistoryModels, List<DebitNote> lstOfDebitNotes)
		{
			try
			{
				#region Commented_for_Bill


				//if (detail.DocumentType == DocTypeConstants.Bills)
				//{
				//    Bill bill = lstOfBills.Where(a => a.Id == detail.DocumentId).FirstOrDefault();
				//    if (bill != null)
				//    {
				//        if (bill.GrandTotal != detail.DocumentAmmount)
				//            throw new Exception("Invalid Document");
				//        if (isfirst == true)
				//            bill.BalanceAmount -= detail.SettledAmount;
				//        else if (isfirst == false && detail.SettledAmount != amount)
				//            bill.BalanceAmount = amount > detail.SettledAmount ? bill.BalanceAmount + Math.Abs((decimal)detail.SettledAmount - amount) : bill.BalanceAmount - Math.Abs((decimal)detail.SettledAmount - amount);
				//        //else if(isfirst==false&&detail.SettledAmount==amount)
				//        if (bill.BalanceAmount > bill.GrandTotal)
				//            throw new Exception("Invalid Document");

				//        if (bill.GrandTotal == bill.BalanceAmount)
				//            bill.DocumentState = BankTransferConstants.Not_Paid;
				//        else if (bill.BalanceAmount == 0)
				//            bill.DocumentState = BankTransferConstants.Fully_Paid;
				//        else if (bill.GrandTotal != bill.BalanceAmount)
				//            bill.DocumentState = BankTransferConstants.Partial_Paid;
				//        bill.ModifiedBy = BankTransferConstants.System;
				//        bill.ModifiedDate = DateTime.UtcNow;
				//        bill.ObjectState = ObjectState.Modified;
				//        _billService.Update(bill);

				//        BankTransferModule.Entities.Models.Journal journal = lstOfJournals.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
				//        if (journal != null)
				//        {
				//            journal.BalanceAmount = bill.BalanceAmount;
				//            journal.DocumentState = bill.DocumentState;
				//            journal.ModifiedBy = bill.ModifiedBy;
				//            journal.ModifiedDate = bill.ModifiedDate;
				//            journal.ObjectState = ObjectState.Modified;
				//            _journalService.Update(journal);
				//        }
				//        try
				//        {
				//            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(transfer.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy, string.Empty, transfer.TransferDate, -detail.SettledAmount);
				//            if (lstdocumet.Any())
				//                //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
				//                lstDocHistoryModels.AddRange(lstdocumet);
				//        }
				//        catch (Exception ex)
				//        { }

				//    }
				//}
				#endregion Commented_for_Bill
				if (detail.DocumentType == DocTypeConstants.Invoice)
				{
					Invoice invoice = lstOfInvoices.Where(a => a.Id == detail.DocumentId).FirstOrDefault();
					if (invoice != null)
					{
						if (invoice.GrandTotal != detail.DocumentAmmount)
							throw new Exception("Invalid Document");
						if (isfirst == true)
							invoice.BalanceAmount -= detail.SettledAmount.Value;
						else if (isfirst == false && detail.SettledAmount != amount)
							invoice.BalanceAmount = amount > detail.SettledAmount ? invoice.BalanceAmount + Math.Abs((decimal)detail.SettledAmount - amount) : invoice.BalanceAmount - Math.Abs((decimal)detail.SettledAmount - amount);
						if (invoice.BalanceAmount > invoice.GrandTotal)
							throw new Exception("Invalid Document");

						if (invoice.GrandTotal == invoice.BalanceAmount)
							invoice.DocumentState = BankTransferConstants.Not_Paid;
						else if (invoice.BalanceAmount == 0)
							invoice.DocumentState = BankTransferConstants.Fully_Paid;
						else if (invoice.GrandTotal != invoice.BalanceAmount)
							invoice.DocumentState = BankTransferConstants.Partial_Paid;
						invoice.ModifiedBy = BankTransferConstants.System;
						invoice.ModifiedDate = DateTime.UtcNow;
						invoice.ObjectState = ObjectState.Modified;
						_invoiceService.Update(invoice);

						BankTransferModule.Entities.Models.Journal journal = lstOfJournals.Where(a => a.DocumentId == invoice.Id).FirstOrDefault();
						if (journal != null)
						{
							journal.BalanceAmount = invoice.BalanceAmount;
							journal.DocumentState = invoice.DocumentState;
							journal.ModifiedBy = invoice.ModifiedBy;
							journal.ModifiedDate = invoice.ModifiedDate;
							journal.ObjectState = ObjectState.Modified;
							_journalService.Update(journal);
						}
						try
						{
							List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(transfer.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy, string.Empty, transfer.TransferDate, -detail.SettledAmount, 0);

							if (lstdocumet.Any())
								lstDocHistoryModels.AddRange(lstdocumet);
						}
						catch (Exception ex)
						{ }

						Bill bill = lstOfBills.Where(a => a.PayrollId == detail.DocumentId).FirstOrDefault();
						if (bill != null)
						{
							if (bill.GrandTotal != detail.DocumentAmmount)
								throw new Exception("Invalid Document");
							if (isfirst == true)
								bill.BalanceAmount -= detail.SettledAmount;
							else if (isfirst == false && detail.SettledAmount != amount)
								bill.BalanceAmount = amount > detail.SettledAmount ? bill.BalanceAmount + Math.Abs((decimal)detail.SettledAmount - amount) : bill.BalanceAmount - Math.Abs((decimal)detail.SettledAmount - amount);
							if (bill.BalanceAmount > bill.GrandTotal)
								throw new Exception("Invalid Document");

							if (bill.GrandTotal == bill.BalanceAmount)
								bill.DocumentState = BankTransferConstants.Not_Paid;
							else if (bill.BalanceAmount == 0)
								bill.DocumentState = BankTransferConstants.Fully_Paid;
							else if (bill.GrandTotal != bill.BalanceAmount)
								bill.DocumentState = BankTransferConstants.Partial_Paid;
							bill.ModifiedBy = BankTransferConstants.System;
							bill.ModifiedDate = DateTime.UtcNow;
							bill.ObjectState = ObjectState.Modified;
							_billService.Update(bill);

							BankTransferModule.Entities.Models.Journal billJournal = lstOfJournals.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
							if (billJournal != null)
							{
								billJournal.BalanceAmount = bill.BalanceAmount;
								billJournal.DocumentState = bill.DocumentState;
								billJournal.ModifiedBy = bill.ModifiedBy;
								billJournal.ModifiedDate = bill.ModifiedDate;
								billJournal.ObjectState = ObjectState.Modified;
								_journalService.Update(billJournal);
							}
							try
							{
								List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(transfer.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy, string.Empty, transfer.TransferDate, -detail.SettledAmount, 0);
								if (lstdocumet.Any())
									lstDocHistoryModels.AddRange(lstdocumet);
							}
							catch (Exception ex)
							{ }

						}
						else
						{
							throw new Exception(string.Concat(BankTransferConstants.Correspending_Bill_Is_not_avilable, invoice.DocNo));
						}
					}
				}
				else if (detail.DocumentType == DocTypeConstants.DebitNote)
				{
					DebitNote debitNote = lstOfDebitNotes.Where(a => a.Id == detail.DocumentId).FirstOrDefault();
					if (debitNote != null)
					{
						if (debitNote.GrandTotal != detail.DocumentAmmount)
							throw new Exception("Invalid Document");
						if (isfirst == true)
							debitNote.BalanceAmount -= detail.SettledAmount.Value;
						else if (isfirst == false && detail.SettledAmount != amount)
							debitNote.BalanceAmount = amount > detail.SettledAmount ? debitNote.BalanceAmount + Math.Abs((decimal)detail.SettledAmount - amount) : debitNote.BalanceAmount - Math.Abs((decimal)detail.SettledAmount - amount);
						if (debitNote.BalanceAmount > debitNote.GrandTotal)
							throw new Exception("Invalid Document");

						if (debitNote.GrandTotal == debitNote.BalanceAmount)
							debitNote.DocumentState = BankTransferConstants.Not_Paid;
						else if (debitNote.BalanceAmount == 0)
							debitNote.DocumentState = BankTransferConstants.Fully_Paid;
						else if (debitNote.GrandTotal != debitNote.BalanceAmount)
							debitNote.DocumentState = BankTransferConstants.Partial_Paid;
						debitNote.ModifiedBy = BankTransferConstants.System;
						debitNote.ModifiedDate = DateTime.UtcNow;
						debitNote.ObjectState = ObjectState.Modified;
						_debitNoteService.Update(debitNote);

						BankTransferModule.Entities.Models.Journal journal = lstOfJournals.Where(a => a.DocumentId == debitNote.Id).FirstOrDefault();
						if (journal != null)
						{
							journal.BalanceAmount = debitNote.BalanceAmount;
							journal.DocumentState = debitNote.DocumentState;
							journal.ModifiedBy = debitNote.ModifiedBy;
							journal.ModifiedDate = debitNote.ModifiedDate;
							journal.ObjectState = ObjectState.Modified;
							_journalService.Update(journal);
						}
						try
						{
							List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(transfer.Id, debitNote.CompanyId, debitNote.Id, DocTypeConstants.DebitNote, DocTypeConstants.Interco, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy, string.Empty, transfer.TransferDate, -detail.SettledAmount, 0);

							if (lstdocumet.Any())
								lstDocHistoryModels.AddRange(lstdocumet);
						}
						catch (Exception ex)
						{ }

						Bill bill = lstOfBills.Where(a => a.PayrollId == detail.DocumentId).FirstOrDefault();
						if (bill != null)
						{
							if (bill.GrandTotal != detail.DocumentAmmount)
								throw new Exception("Invalid Document");
							if (isfirst == true)
								bill.BalanceAmount -= detail.SettledAmount;
							else if (isfirst == false && detail.SettledAmount != amount)
								bill.BalanceAmount = amount > detail.SettledAmount ? bill.BalanceAmount + Math.Abs((decimal)detail.SettledAmount - amount) : bill.BalanceAmount - Math.Abs((decimal)detail.SettledAmount - amount);
							if (bill.BalanceAmount > bill.GrandTotal)
								throw new Exception("Invalid Document");

							if (bill.GrandTotal == bill.BalanceAmount)
								bill.DocumentState = BankTransferConstants.Not_Paid;
							else if (bill.BalanceAmount == 0)
								bill.DocumentState = BankTransferConstants.Fully_Paid;
							else if (bill.GrandTotal != bill.BalanceAmount)
								bill.DocumentState = BankTransferConstants.Partial_Paid;
							bill.ModifiedBy = BankTransferConstants.System;
							bill.ModifiedDate = DateTime.UtcNow;
							bill.ObjectState = ObjectState.Modified;
							_billService.Update(bill);

							BankTransferModule.Entities.Models.Journal billJournal = lstOfJournals.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
							if (billJournal != null)
							{
								billJournal.BalanceAmount = bill.BalanceAmount;
								billJournal.DocumentState = bill.DocumentState;
								billJournal.ModifiedBy = bill.ModifiedBy;
								billJournal.ModifiedDate = bill.ModifiedDate;
								billJournal.ObjectState = ObjectState.Modified;
								_journalService.Update(billJournal);
							}
							try
							{
								List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(transfer.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy, string.Empty, transfer.TransferDate, -detail.SettledAmount, 0);
								if (lstdocumet.Any())
									lstDocHistoryModels.AddRange(lstdocumet);
							}
							catch (Exception ex)
							{ }

						}
						else
						{
							throw new Exception(string.Concat(BankTransferConstants.Correspending_Bill_Is_not_avilable, debitNote.DocNo));
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void UpdateDocStateIfIBUnchecked(BankTransfer bankTransfer, List<DocumentHistoryModel> lstOfDocModels, bool isDelete)
		{
			try
			{
				List<Guid> lstOfDocIds = new List<Guid>();
				List<Invoice> lstOfEditInvoices = _invoiceService.GetListOfInvoicesByCompanyIdAndDocId(bankTransfer.CompanyId, bankTransfer.SettlementDetails.Where(a => a.DocumentType == DocTypeConstants.Invoice).Select(a => a.DocumentId.Value).ToList());
				List<DebitNote> lstOfDebitNotes = _debitNoteService.GetListOfDNsByCompanyIdAndDocId(bankTransfer.CompanyId, bankTransfer.SettlementDetails.Where(a => a.DocumentType == DocTypeConstants.DebitNote).Select(a => a.DocumentId.Value).ToList());

				List<Bill> lstOfEditBills = _billService.GetListOfBillsByInvoiceIds(bankTransfer.CompanyId, bankTransfer.SettlementDetails.Where(a => a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.DebitNote).Select(a => a.DocumentId.Value).ToList());

				lstOfDocIds.AddRange(bankTransfer.SettlementDetails.Where(a => a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.DebitNote).Select(a => a.DocumentId.Value).ToList());
				lstOfDocIds.AddRange(lstOfEditBills.Select(a => a.Id).ToList());

				List<BankTransferModule.Entities.Models.Journal> lstOfEditJournals = _journalService.GetListOfJournalBYCompIdandDocId(bankTransfer.CompanyId, /*bankTransfer.SettlementDetails.Select(a => a.DocumentId.Value).ToList()*/lstOfDocIds);
				foreach (SettlementDetail detail in bankTransfer.SettlementDetails)
				{
					#region CommntedCode_for_bill


					//if (detail.DocumentType == DocTypeConstants.Bills)
					//{
					//    Bill bill = lstOfEditBills.Where(a => a.Id == detail.DocumentId).FirstOrDefault();
					//    if (bill != null)
					//    {
					//        if (bill.GrandTotal != detail.DocumentAmmount)
					//            throw new Exception("Invalid Document");
					//        bill.BalanceAmount += detail.SettledAmount;
					//        //else if(isfirst==false&&detail.SettledAmount==amount)
					//        if (bill.BalanceAmount > bill.GrandTotal)
					//            throw new Exception("Invalid Document");

					//        if (bill.GrandTotal == bill.BalanceAmount)
					//            bill.DocumentState = BankTransferConstants.Not_Paid;
					//        else if (bill.BalanceAmount == 0)
					//            bill.DocumentState = BankTransferConstants.Fully_Paid;
					//        else if (bill.GrandTotal != bill.BalanceAmount)
					//            bill.DocumentState = BankTransferConstants.Partial_Paid;
					//        bill.ModifiedBy = BankTransferConstants.System;
					//        bill.ModifiedDate = DateTime.UtcNow;
					//        bill.ObjectState = ObjectState.Modified;
					//        _billService.Update(bill);

					//        BankTransferModule.Entities.Models.Journal journal = lstOfEditJournals.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
					//        if (journal != null)
					//        {
					//            journal.BalanceAmount = bill.BalanceAmount;
					//            journal.DocumentState = bill.DocumentState;
					//            journal.ModifiedBy = bill.ModifiedBy;
					//            journal.ModifiedDate = bill.ModifiedDate;
					//            journal.ObjectState = ObjectState.Modified;
					//            _journalService.Update(journal);
					//        }
					//        try
					//        {
					//            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy, string.Empty, bankTransfer.TransferDate, 0);
					//            if (lstdocumet.Any())
					//                //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
					//                lstOfDocModels.AddRange(lstdocumet);
					//        }
					//        catch (Exception ex)
					//        { }

					//    }
					//    if (isDelete == true)
					//        detail.ObjectState = ObjectState.Deleted;
					//}
					#endregion
					if (detail.DocumentType == DocTypeConstants.Invoice)
					{
						Invoice invoice = lstOfEditInvoices.Where(a => a.Id == detail.DocumentId).FirstOrDefault();
						if (invoice != null)
						{
							if (invoice.GrandTotal != detail.DocumentAmmount)
								throw new Exception("Invalid Document");

							invoice.BalanceAmount += detail.SettledAmount.Value;

							if (invoice.BalanceAmount > invoice.GrandTotal)
								throw new Exception("Invalid Document");

							if (invoice.GrandTotal == invoice.BalanceAmount)
								invoice.DocumentState = BankTransferConstants.Not_Paid;
							else if (invoice.BalanceAmount == 0)
								invoice.DocumentState = BankTransferConstants.Fully_Paid;
							else if (invoice.GrandTotal != invoice.BalanceAmount)
								invoice.DocumentState = BankTransferConstants.Partial_Paid;
							invoice.ModifiedBy = BankTransferConstants.System;
							invoice.ModifiedDate = DateTime.UtcNow;
							invoice.ObjectState = ObjectState.Modified;
							_invoiceService.Update(invoice);

							BankTransferModule.Entities.Models.Journal journal = lstOfEditJournals.Where(a => a.DocumentId == invoice.Id).FirstOrDefault();
							if (journal != null)
							{
								journal.BalanceAmount = invoice.BalanceAmount;
								journal.DocumentState = invoice.DocumentState;
								journal.ModifiedBy = invoice.ModifiedBy;
								journal.ModifiedDate = invoice.ModifiedDate;
								journal.ObjectState = ObjectState.Modified;
								_journalService.Update(journal);
							}
							try
							{
								List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy, string.Empty, bankTransfer.TransferDate, 0, 0);

								if (lstdocumet.Any())
									lstOfDocModels.AddRange(lstdocumet);
							}
							catch (Exception ex)
							{ }

							Bill bill = lstOfEditBills.Where(a => a.PayrollId == invoice.Id && a.CompanyId == invoice.CompanyId && a.Nature == BankTransferConstants.Interco).FirstOrDefault();
							if (bill != null)
							{
								if (bill.GrandTotal != detail.DocumentAmmount)
									throw new Exception("Invalid Document");
								bill.BalanceAmount += detail.SettledAmount;
								if (bill.BalanceAmount > bill.GrandTotal)
									throw new Exception("Invalid Document");

								if (bill.GrandTotal == bill.BalanceAmount)
									bill.DocumentState = BankTransferConstants.Not_Paid;
								else if (bill.BalanceAmount == 0)
									bill.DocumentState = BankTransferConstants.Fully_Paid;
								else if (bill.GrandTotal != bill.BalanceAmount)
									bill.DocumentState = BankTransferConstants.Partial_Paid;
								bill.ModifiedBy = BankTransferConstants.System;
								bill.ModifiedDate = DateTime.UtcNow;
								bill.ObjectState = ObjectState.Modified;
								_billService.Update(bill);

								BankTransferModule.Entities.Models.Journal billJournal = lstOfEditJournals.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
								if (billJournal != null)
								{
									billJournal.BalanceAmount = bill.BalanceAmount;
									billJournal.DocumentState = bill.DocumentState;
									billJournal.ModifiedBy = bill.ModifiedBy;
									billJournal.ModifiedDate = bill.ModifiedDate;
									billJournal.ObjectState = ObjectState.Modified;
									_journalService.Update(billJournal);
								}
								try
								{
									List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy, string.Empty, bankTransfer.TransferDate, 0, 0);
									if (lstdocumet.Any())
										lstOfDocModels.AddRange(lstdocumet);
								}
								catch (Exception ex)
								{ }
							}
							else
							{
								throw new Exception(string.Concat(BankTransferConstants.Correspending_Bill_Is_not_avilable, invoice.DocNo));
							}
						}
						if (isDelete == true)
							detail.ObjectState = ObjectState.Deleted;
					}
					if (detail.DocumentType == DocTypeConstants.DebitNote)
					{
						DebitNote debitNote = lstOfDebitNotes.Where(a => a.Id == detail.DocumentId).FirstOrDefault();
						if (debitNote != null)
						{
							if (debitNote.GrandTotal != detail.DocumentAmmount)
								throw new Exception("Invalid Document");

							debitNote.BalanceAmount += detail.SettledAmount.Value;

							if (debitNote.BalanceAmount > debitNote.GrandTotal)
								throw new Exception("Invalid Document");

							if (debitNote.GrandTotal == debitNote.BalanceAmount)
								debitNote.DocumentState = BankTransferConstants.Not_Paid;
							else if (debitNote.BalanceAmount == 0)
								debitNote.DocumentState = BankTransferConstants.Fully_Paid;
							else if (debitNote.GrandTotal != debitNote.BalanceAmount)
								debitNote.DocumentState = BankTransferConstants.Partial_Paid;
							debitNote.ModifiedBy = BankTransferConstants.System;
							debitNote.ModifiedDate = DateTime.UtcNow;
							debitNote.ObjectState = ObjectState.Modified;
							_debitNoteService.Update(debitNote);

							BankTransferModule.Entities.Models.Journal journal = lstOfEditJournals.Where(a => a.DocumentId == debitNote.Id).FirstOrDefault();
							if (journal != null)
							{
								journal.BalanceAmount = debitNote.BalanceAmount;
								journal.DocumentState = debitNote.DocumentState;
								journal.ModifiedBy = debitNote.ModifiedBy;
								journal.ModifiedDate = debitNote.ModifiedDate;
								journal.ObjectState = ObjectState.Modified;
								_journalService.Update(journal);
							}
							try
							{
								List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, debitNote.CompanyId, debitNote.Id, DocTypeConstants.DebitNote, DocTypeConstants.Interco, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy, string.Empty, bankTransfer.TransferDate, 0, 0);

								if (lstdocumet.Any())
									lstOfDocModels.AddRange(lstdocumet);
							}
							catch (Exception ex)
							{ }

							Bill bill = lstOfEditBills.Where(a => a.PayrollId == debitNote.Id && a.CompanyId == debitNote.CompanyId && a.Nature == BankTransferConstants.Interco).FirstOrDefault();
							if (bill != null)
							{
								if (bill.GrandTotal != detail.DocumentAmmount)
									throw new Exception("Invalid Document");
								bill.BalanceAmount += detail.SettledAmount;
								if (bill.BalanceAmount > bill.GrandTotal)
									throw new Exception("Invalid Document");

								if (bill.GrandTotal == bill.BalanceAmount)
									bill.DocumentState = BankTransferConstants.Not_Paid;
								else if (bill.BalanceAmount == 0)
									bill.DocumentState = BankTransferConstants.Fully_Paid;
								else if (bill.GrandTotal != bill.BalanceAmount)
									bill.DocumentState = BankTransferConstants.Partial_Paid;
								bill.ModifiedBy = BankTransferConstants.System;
								bill.ModifiedDate = DateTime.UtcNow;
								bill.ObjectState = ObjectState.Modified;
								_billService.Update(bill);

								BankTransferModule.Entities.Models.Journal billJournal = lstOfEditJournals.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
								if (billJournal != null)
								{
									billJournal.BalanceAmount = bill.BalanceAmount;
									billJournal.DocumentState = bill.DocumentState;
									billJournal.ModifiedBy = bill.ModifiedBy;
									billJournal.ModifiedDate = bill.ModifiedDate;
									billJournal.ObjectState = ObjectState.Modified;
									_journalService.Update(billJournal);
								}
								try
								{
									List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bankTransfer.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy, string.Empty, bankTransfer.TransferDate, 0, 0);
									if (lstdocumet.Any())
										lstOfDocModels.AddRange(lstdocumet);
								}
								catch (Exception ex)
								{ }
							}
							else
							{
								throw new Exception(string.Concat(BankTransferConstants.Correspending_Bill_Is_not_avilable, debitNote.DocNo));
							}
						}
						if (isDelete == true)
							detail.ObjectState = ObjectState.Deleted;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion
	}

}
