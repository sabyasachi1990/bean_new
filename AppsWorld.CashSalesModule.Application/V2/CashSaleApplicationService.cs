using AppsWorld.CashSalesModule.Entities.V2;
using AppsWorld.CashSalesModule.Infra;
using AppsWorld.CashSalesModule.Models;
using AppsWorld.CashSalesModule.RepositoryPattern.V2;
using AppsWorld.CashSalesModule.Service.V2;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.Models;
using AppsWorld.Framework;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ziraff.FrameWork.Logging;
using Ziraff.Section;

namespace AppsWorld.CashSalesModule.Application.V2
{
    public class CashSaleApplicationService
    {
        private readonly ICashSalesService _cashSaleService;
        private readonly IMasterCompactService _masterService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IItemCompactService _itemService;
        private readonly ICashSalesMasterUnitOfWorkAsync _unitOfWork;
        public CashSaleApplicationService(ICashSalesService cashSaleService, IMasterCompactService masterService, IAutoNumberService autoNumberService, IItemCompactService itemService, ICashSalesMasterUnitOfWorkAsync unitOfWork)
        {
            this._cashSaleService = cashSaleService;
            _masterService = masterService;
            _itemService = itemService;
            _autoNumberService = autoNumberService;
            _unitOfWork = unitOfWork;
        }

        #region Create And Lookup Call

        public CashSaleModelLU GetAllCashSalesLUs(string userName, Guid cashsaleId, long companyId,string ConnectionString)
        {
            CashSaleModelLU cashsaleLU = new CashSaleModelLU();
            SqlConnection con = null;
            SqlCommand cmd = null;
            try
            {
                DateTime lastCashSale = _cashSaleService.GetCashsaleByCompanyId(companyId);
                CashSale cashSale = _cashSaleService.GetCashSaleLU(companyId, cashsaleId);
                DateTime date = cashSale == null ? lastCashSale == null ? DateTime.Now : lastCashSale : cashSale.DocDate;
                long? comp = cashSale == null ? 0 : cashSale.ServiceCompanyId == null ? 0 : cashSale.ServiceCompanyId;
                List<CommonLookUps<string>> lstLookUps = new List<CommonLookUps<string>>();
                LookUpCategory<string> currency = new LookUpCategory<string>();
                string currencyCode = cashSale != null ? cashSale.DocCurrency : string.Empty;
                string modeOfReceipt = cashSale != null ? cashSale.ModeOfReceipt : null;
                string query = $"SELECT 'CURRENCYEDIT' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as DEFAULTVALUE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY FROM Bean.Currency WHERE CompanyId={ companyId  } AND (Status=1 OR Code='{currencyCode}' OR DefaultValue='SGD');SELECT 'CURRENCYNEW' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as DEFAULTVALUE,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY  FROM Bean.Currency WHERE CompanyId={ companyId } AND (Status=1 OR DefaultValue='SGD');SELECT 'MODEOFRECEIPTNEW' as TABLENAME,CT.Id as ID,CC.CodeValue as NAME,0 as TOPVALUE,CC.RecOrder as RECORDER,CC.Id as ControlCodeId,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,CC.Status as STATUS,CC.CodeKey as CATEGORY,CT.DefaultValue as DEFAULTVALUE,CT.ControlCodeCategoryCode as CODE FROM Common.ControlCodeCategory CT JOIN Common.ControlCode CC on CT.Id=CC.ControlCategoryId  where CT.CompanyId={companyId} AND CT.Status=1 AND CT.ControlCodeCategoryCode='{ControlCodeConstants.Control_codes_ModeOfTransfer}'  AND CC.Status=1;SELECT 'MODEOFRECEIPTEDIT' as TABLENAME,CT.Id as ID,CC.CodeValue as NAME,0 as TOPVALUE,CC.RecOrder as RECORDER,CC.Id as ControlCodeId,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,CC.Status as STATUS,CC.CodeKey as CATEGORY,CT.DefaultValue as DEFAULTVALUE,CT.ControlCodeCategoryCode as CODE FROM Common.ControlCodeCategory CT JOIN Common.ControlCode CC on CT.Id=CC.ControlCategoryId  where CT.CompanyId={companyId} AND CT.Status=1 AND CT.ControlCodeCategoryCode='{ControlCodeConstants.Control_codes_ModeOfTransfer}'  AND (CC.Status=1 OR CC.CodeKey='{modeOfReceipt}');SELECT 'SERVICECOMPANY' as TABLENAME,C.Id as ID,c.Name as NAME,c.ShortName as SHOTNAME,'' as DEFAULTVALUE,c.IsGstSetting as IsGstActive,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,'' as CODE,1 as STATUS,0 as RECORDER,'' as CATEGORY FROM Common.Company c JOIN Common.CompanyUser CU on C.ParentId=CU.CompanyId where (c.Status = 1 or c.Id = { comp }) and c.ParentId ={companyId } and CU.Username='{ userName }';SELECT 'TAXCODE' as TABLENAME,Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY FROM Bean.TaxCode where CompanyId=0 and Status<3 and EffectiveFrom<='{ String.Format("{0:MM/dd/yyyy}'", date) } and (EffectiveTo>='{ String.Format("{0:MM/dd/yyyy}", date) }' OR EffectiveTo is null);SELECT 'CHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as DEFAULTVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY FROM Bean.AccountType A JOIN Bean.ChartOfAccount COA on A.Id = COA.AccountTypeId where COA.CompanyId ={ companyId } and a.Name in ('Revenue','Other income') and COA.IsRealCOA=1;";
                int? resultsetCount = query.Split(';').Count();
                con = new SqlConnection(ConnectionString);
                if (con.State != ConnectionState.Open)
                    con.Open();
                using (cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    for (int i = 1; i <= resultsetCount; i++)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                lstLookUps.Add(new CommonLookUps<string>
                                {
                                    TableName = dr["TABLENAME"].ToString(),
                                    Code = dr["CODE"].ToString(),
                                    Id = dr["ID"] != DBNull.Value ? Convert.ToInt64(dr["ID"]) : 0,
                                    Name = dr["NAME"].ToString(),
                                    RecOrder = dr["RECORDER"] != DBNull.Value ? Convert.ToInt32(dr["RECORDER"]) : (int?)null,
                                    TaxRate = dr["TAXRATE"] != DBNull.Value ? Convert.ToDouble(dr["TAXRATE"]) : (double?)null,
                                    TaxType = dr["TAXTYPE"].ToString(),
                                    TaxCode = dr["TXCODE"].ToString(),
                                    TOPValue = dr["TOPVALUE"] != DBNull.Value ? Convert.ToDouble(dr["TOPVALUE"]) : (double?)null,
                                    Currency = dr["CURRENCY"].ToString(),
                                    Class = dr["Class"].ToString(),
                                    isGstActivated = dr["IsGstActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsGstActive"]) : (bool?)null,
                                    ShortName = dr["SHOTNAME"].ToString(),
                                    //ServiceCompanyId= row["Id"] != DBNull.Value ? Convert.ToInt64(row["Id"]) : (long?)null,
                                    DefaultValue = "SGD",
                                    CategoryName = "SGD",
                                    Status = (RecordStatusEnum)dr["STATUS"]
                                });
                            }
                        }
                        dr.NextResult();
                    }
                }
                if (lstLookUps.Any())
                {
                    if (cashSale != null)
                    {
                        currency.CategoryName = ControlCodeConstants.Currency_DefaultCode;
                        currency.DefaultValue = ControlCodeConstants.Currency_DefaultCode;
                        currency.Lookups = lstLookUps.Where(c => c.TableName == "CURRENCYEDIT").Select(c => new LookUp<string>()
                        {
                            Code = c.Code,
                            Name = c.Name,
                            RecOrder = c.RecOrder
                        }).ToList();
                        cashsaleLU.CurrencyLU = currency;
                        currency = new LookUpCategory<string>();
                        currency.CategoryName = ControlCodeConstants.Currency_DefaultCode;
                        currency.DefaultValue = ControlCodeConstants.Currency_DefaultCode;
                        currency.Lookups = lstLookUps.Where(c => c.TableName == "MODEOFRECEIPTEDIT").Select(c => new LookUp<string>()
                        {
                            Code = c.Code,
                            Name = c.Name,
                            RecOrder = c.RecOrder
                        }).ToList();
                        cashsaleLU.ModeOfReciptLU = currency;
                    }
                    else
                    {
                        currency.CategoryName = ControlCodeConstants.Currency_DefaultCode;
                        currency.DefaultValue = ControlCodeConstants.Currency_DefaultCode;
                        currency.Lookups = lstLookUps.Where(c => c.TableName == "CURRENCYNEW").Select(c => new LookUp<string>()
                        {
                            Code = c.Code,
                            Name = c.Name,
                            RecOrder = c.RecOrder
                        }).ToList();
                        cashsaleLU.CurrencyLU = currency;
                        currency = new LookUpCategory<string>();
                        currency.CategoryName = ControlCodeConstants.Currency_DefaultCode;
                        currency.DefaultValue = ControlCodeConstants.Currency_DefaultCode;
                        currency.Lookups = lstLookUps.Where(c => c.TableName == "MODEOFRECEIPTNEW").Select(c => new LookUp<string>()
                        {
                            Code = c.Code,
                            Name = c.Name,
                            RecOrder = c.RecOrder
                        }).ToList();
                        cashsaleLU.ModeOfReciptLU = currency;
                    }
                    cashsaleLU.SubsideryCompanyLU = lstLookUps.Where(c => c.TableName == "SERVICECOMPANY").Select(x => new LookUpCompany<string>()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        isGstActivated = x.isGstActivated
                    }).ToList();
                }
                List<COALookup<string>> lstEditCoa = null;
                List<TaxCodeLookUp<string>> lstEditTax = null;
                List<string> coaName = new List<string> { COANameConstants.Revenue, COANameConstants.Other_income };
                cashsaleLU.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "CHARTOFACCOUNT" && z.Status == RecordStatusEnum.Active).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Code = x.Code,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsPLAccount = x.COACategory == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status,
                    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                }).OrderBy(d => d.Name).ToList();
                cashsaleLU.TaxCodeLU = lstLookUps.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    TaxRate = x.TaxRate,
                    TaxType = x.TaxType,
                    Status = x.Status,
                    TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                }).OrderBy(c => c.Code).ToList();
                if (cashSale != null && cashSale.CashSaleDetails.Count > 0)
                {
                    List<long> CoaIds = cashSale.CashSaleDetails.Select(c => c.COAId).ToList();
                    if (cashsaleLU.ChartOfAccountLU.Any())
                        CoaIds = CoaIds.Except(cashsaleLU.ChartOfAccountLU.Select(x => x.Id)).ToList();
                    List<long?> taxIds = cashSale.CashSaleDetails.Select(x => x.TaxId).ToList();
                    if (cashsaleLU.TaxCodeLU.Any())
                        taxIds = taxIds.Except(cashsaleLU.TaxCodeLU.Select(d => d.Id)).ToList();
                    if (CoaIds.Any())
                    {

                        lstEditCoa = lstLookUps.Where(x => x.TableName == "CHARTOFACCOUNT" && CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Code = x.Code,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsPLAccount = x.COACategory == "Income Statement" ? true : false,
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                        }).OrderBy(d => d.Name).ToList();
                        cashsaleLU.ChartOfAccountLU.AddRange(lstEditCoa);
                    }
                    if (taxIds.Any())
                    {
                        lstEditTax = lstLookUps.Where(c => taxIds.Contains(c.Id) && c.TableName == "TAXCODE").Select(x => new TaxCodeLookUp<string>()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            TaxRate = x.TaxRate,
                            TaxType = x.TaxType,
                            Status = x.Status,
                            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                        }).OrderBy(c => c.Code).ToList();
                        cashsaleLU.TaxCodeLU.AddRange(lstEditTax);
                        cashsaleLU.TaxCodeLU = cashsaleLU.TaxCodeLU.OrderBy(c => c.Code).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }

            return cashsaleLU;
        }

        public CashSaleModel CreateCashSales(long companyId, Guid id)
        {
            CashSaleModel cashSaleDTO = new CashSaleModel();
            try
            {

                FinancialSettingCompact financialsetting = _masterService.GetFinancialSetting(companyId);
                if (financialsetting == null)
                {
                    throw new Exception(CashSaleValidation.The_Financial_setting_should_be_activated);
                }
                cashSaleDTO.FinancialPeriodLockStartDate = financialsetting.PeriodLockDate;
                cashSaleDTO.FinancialPeriodLockEndDate = financialsetting.PeriodEndDate;
                CashSale cashSale = _cashSaleService.GetCashSaleByIdAndCompanyId(id, companyId);
                if (cashSale == null)
                {
                    AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.CashSale);
                    Dictionary<DateTime?, DateTime> listcashSale = _cashSaleService.GetCashsaleCreatedDate(companyId);
                    cashSaleDTO.Id = Guid.NewGuid();
                    cashSaleDTO.CompanyId = companyId;
                    cashSaleDTO.DocDate = listcashSale == null ? DateTime.Now : listcashSale.Values.FirstOrDefault();
                    cashSaleDTO.NoSupportingDocument = false;
                    bool? isEdit = false;
                    cashSaleDTO.DocNo = GetAutoNumberByEntityType(companyId, listcashSale.Keys.FirstOrDefault(), _autoNo, ref isEdit);
                    cashSaleDTO.IsDocNoEditable = isEdit;
                    cashSaleDTO.CreatedDate = DateTime.UtcNow;
                    cashSaleDTO.BaseCurrency = financialsetting.BaseCurrency;
                    cashSaleDTO.DocCurrency = cashSaleDTO.BaseCurrency;
                    cashSaleDTO.DocType = DocTypeConstants.CashSale;
                    cashSaleDTO.DocSubType = DocTypeConstants.CashSale;
                }
                else
                {
                    List<CashSaleDetailModel> listCashsaleDetailModel = new List<CashSaleDetailModel>();
                    FillCashSales(cashSaleDTO, cashSale);
                    cashSaleDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.CashSale);
                    List<ItemCompact> lstItem = _itemService.GetAllItemById(cashSale.CashSaleDetails.Select(c => c.ItemId).ToList(), cashSale.CompanyId);
                    foreach (var detail in cashSale.CashSaleDetails)
                    {
                        CashSaleDetailModel cashSaleDetailModel = new CashSaleDetailModel();
                        FillCashDetails(cashSaleDetailModel, detail);
                        cashSaleDetailModel.ItemDescription = (detail.ItemDescription != null || detail.ItemDescription != string.Empty) ? detail.ItemDescription : lstItem.Where(c => c.Id == detail.ItemId).Select(d => d.Description).FirstOrDefault();
                        listCashsaleDetailModel.Add(cashSaleDetailModel);
                    }
                    cashSaleDTO.CashSaleDetails = listCashsaleDetailModel.OrderBy(c => c.RecOrder).ToList();
                }
            }

            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }
            return cashSaleDTO;
        }

        public CashSaleDetailModel GetCashSale(Guid id)
        {
            CashSaleDetailModel cashSaledetailModel = new CashSaleDetailModel();
            return cashSaledetailModel;
        }
        #endregion

        #region SaveCall

        public CashSale SaveCashSale(CashSaleModel TObject)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entered_Into_Save_Cashsale);
            bool isNew = false;
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Checking_All_Validations);
            string _errors = CommonValidation.ValidateObject(TObject);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }
            CashSaleValidations(TObject);
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Validations_Checking_Finished);
            CashSale _cashSale = _cashSaleService.GetCashSaleByIdAndCompanyId(TObject.Id, TObject.CompanyId);
            if (_cashSale != null)
            {
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Validationg_The_CashSale_In_Edit_Mode);
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_To_Execute_InsertCashSale_Methos);
                InsertCashSale(TObject, _cashSale);
                _cashSale.DocNo = TObject.DocNo;
                _cashSale.CashSaleNumber = _cashSale.DocNo;
                _cashSale.ModifiedBy = TObject.ModifiedBy;
                _cashSale.ModifiedDate = DateTime.UtcNow;
                _cashSale.ObjectState = ObjectState.Modified;
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_To_Execute_UpdateCashSaleDetail_Methos);
                UpdateCashSaleDetails(TObject, _cashSale);
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_to_Execute_UpdateCashSaleGstDetail_Method);
                //UpdateCashSaleGSTDetails(TObject, _cashSale);
                _cashSaleService.Update(_cashSale);
            }
            else
            {
                _cashSale = new CashSale();
                isNew = true;
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_to_execute_InsertCashSle_method_in_insert_new_mode);
                InsertCashSale(TObject, _cashSale);
                _cashSale.Id = Guid.NewGuid();
                int? recorder = 0;
                if (TObject.CashSaleDetails.Count > 0 || TObject.CashSaleDetails != null)
                {
                    foreach (CashSaleDetailModel detailModel in TObject.CashSaleDetails)
                    {
                        LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.EntredInto_FillCashSaleDetail_Method);
                        CashSaleDetail ncashSaleDetail = new CashSaleDetail();
                        LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_to_Execute_Fill_Method_Of_CashSaleDetatil);
                        FillCashSaleDetail(ncashSaleDetail, detailModel, TObject.ExchangeRate);
                        ncashSaleDetail.RecOrder = recorder + 1;
                        recorder = ncashSaleDetail.RecOrder;
                        ncashSaleDetail.Id = Guid.NewGuid();
                        ncashSaleDetail.CashSaleId = _cashSale.Id;
                        //_cashSalesDetailService.Insert(ncashSaleDetail);
                        ncashSaleDetail.ObjectState = ObjectState.Added;
                    }
                }
                _cashSale.Status = AppsWorld.Framework.RecordStatusEnum.Active;
                _cashSale.UserCreated = TObject.UserCreated;
                _cashSale.CreatedDate = DateTime.UtcNow;

                //List<string> listCashSale = _cashSaleService.GetAutoNumber(TObject.CompanyId);
                Dictionary<long, string> company = _masterService.GetCompanyByCompanyid(TObject.CompanyId);
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_To_Execute_Auto_Number_Method);
                _cashSale.CashSaleNumber = TObject.IsDocNoEditable != true ? GenerateAutoNumberForType(company.Keys.FirstOrDefault(), DocTypeConstants.CashSale, company.Values.FirstOrDefault()) : TObject.DocNo;
                _cashSale.DocNo = _cashSale.CashSaleNumber;
                _cashSale.ObjectState = ObjectState.Added;
                _cashSaleService.Insert(_cashSale);
            }
            try
            {
                _unitOfWork.SaveChanges();
                JVModel jvm = new JVModel();
                FillJournal(jvm, _cashSale, isNew, DocTypeConstants.CashSale);
                jvm.DocumentState = CashSaleStatus.FullyPaid;
                SaveCashSalePosting(jvm);
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.SaveChanges_method_execution_happened);
            }
            catch (DbEntityValidationException e)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, e, e.Message);
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw e;
            }
            return _cashSale;
        }



        public CashSale SaveCashSaleDocumentVoid(DocumentVoidModel TObject)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entered_into_SaveCashSaleDocumentVoid_method);
            string DocNo = "-V";
            CashSale cashSale = _cashSaleService.GetCashSaleByIdAndCompanyId(TObject.Id, TObject.CompanyId);
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Validating_model_and_proceed_towards_the_functional_validation);
            if (cashSale == null)
                throw new Exception(CashSaleValidation.Invalid_CashSale);
            if (!_masterService.ValidateYearEndLockDate(cashSale.DocDate, cashSale.CompanyId))
            {
                throw new Exception(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Functionality_validation_going_on);
            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_masterService.ValidateFinancialOpenPeriod(cashSale.DocDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_masterService.ValidateFinancialLockPeriodPassword(cashSale.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.End_of_the_Functionality_validation);
            }
            cashSale.DocumentState = CashSaleStatus.Void;
            cashSale.DocNo = cashSale.DocNo + DocNo;
            cashSale.ObjectState = ObjectState.Modified;
            cashSale.ModifiedBy = TObject.ModifiedBy;
            try
            {
                _unitOfWork.SaveChanges();
                DocumentVoidModel tObject = new DocumentVoidModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocType = cashSale.DocType;
                tObject.DocNo = cashSale.DocNo;
                tObject.ModifiedBy = TObject.ModifiedBy;
                VoidJVPostCashSale(tObject);
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.SaveChanges_method_execution_happened_in_void_method);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.SaveChanges_method_execution_happened_in_void_method);
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.End_of_the_Doc_Void_method);
            return cashSale;
        }


        #endregion Save_Block

        #region Private_Block

        private string GetAutoNumberByEntityType(long companyId, DateTime? listcashSale, AutoNumberCompact _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;

            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewCashSleDocumentNumber(DocTypeConstants.CashSale, companyId);
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
                    if (listcashSale != null)
                    {
                        if (_autoNo.Format.Contains("{MM/YYYY}"))
                        {
                            //var lastCreatedInvoice = lstInvoice.FirstOrDefault();
                            if (listcashSale.Value.Month != DateTime.UtcNow.Month)
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

        public string GetNewCashSleDocumentNumber(string docType, long companyId)
        {
            CashSale cashSale = _cashSaleService.GetDocTypeAndCompanyid(docType, companyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            if (cashSale != null)
            {
                string strOldNo = String.Empty;
                CashSale duplicatCashSale;
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

                    duplicatCashSale = _cashSaleService.DuplicateCashsale(strNewDocNo, docType, companyId);
                } while (duplicatCashSale != null);
            }
            return strNewDocNo;
        }
        private void FillCashSales(CashSaleModel cashSaleDTO, CashSale cashSale)
        {
            cashSaleDTO.Id = cashSale.Id;
            cashSaleDTO.CompanyId = cashSale.CompanyId;
            cashSaleDTO.EntityType = cashSale.EntityType;
            cashSaleDTO.DocSubType = DocTypeConstants.CashSale;
            cashSaleDTO.DocType = cashSale.DocType;
            cashSaleDTO.DocNo = cashSale.DocNo;
            cashSaleDTO.DocDate = cashSale.DocDate;
            cashSaleDTO.PONo = cashSale.PONo;
            cashSaleDTO.ModeOfReceipt = cashSale.ModeOfReceipt;
            cashSaleDTO.EntityId = cashSale.EntityId;
            cashSaleDTO.EntityName = cashSale.EntityId != null ? _masterService.GetEntityName(cashSale.EntityId.Value) : null;
            cashSaleDTO.COAId = cashSale.COAId;
            cashSaleDTO.ReceiptrefNo = cashSale.ReceiptrefNo;
            cashSaleDTO.CashSaleNumber = cashSale.CashSaleNumber;
            cashSaleDTO.DocCurrency = cashSale.DocCurrency;
            cashSaleDTO.ServiceCompanyId = cashSale.ServiceCompanyId;
            cashSaleDTO.ReceiptrefNo = cashSale.ReceiptrefNo;
            cashSaleDTO.IsMultiCurrency = cashSale.IsMultiCurrency;
            cashSaleDTO.BaseCurrency = cashSale.ExCurrency;
            cashSaleDTO.ExchangeRate = cashSale.ExchangeRate;
            cashSaleDTO.ExDurationFrom = cashSale.ExDurationFrom;
            cashSaleDTO.ExDurationTo = cashSale.ExDurationTo;
            cashSaleDTO.IsGstSettings = cashSale.IsGstSettings;
            cashSaleDTO.GSTExCurrency = cashSale.GSTExCurrency;
            cashSaleDTO.GSTExchangeRate = cashSale.GSTExchangeRate;
            cashSaleDTO.GSTExDurationFrom = cashSale.GSTExDurationFrom;
            cashSaleDTO.GSTExDurationTo = cashSale.GSTExDurationTo;
            cashSaleDTO.IsSegmentReporting = cashSale.IsSegmentReporting;
            cashSaleDTO.BankClearingDate = cashSale.BankClearingDate;
            cashSaleDTO.BalanceAmount = cashSale.BalanceAmount;
            cashSaleDTO.GSTTotalAmount = cashSale.GSTTotalAmount;
            cashSaleDTO.GrandTotal = cashSale.GrandTotal;
            cashSaleDTO.Remarks = cashSale.Remarks;
            cashSaleDTO.IsGSTApplied = cashSale.IsGSTApplied;
            cashSaleDTO.DocDescription = cashSale.DocDescription;
            cashSaleDTO.IsAllowableNonAllowable = cashSale.IsAllowableNonAllowable;
            cashSaleDTO.IsNoSupportingDocument = cashSale.IsNoSupportingDocument;
            cashSaleDTO.NoSupportingDocument = cashSale.NoSupportingDocs;
            cashSaleDTO.IsBaseCurrencyRateChanged = cashSale.IsBaseCurrencyRateChanged;
            cashSaleDTO.IsGSTCurrencyRateChanged = cashSale.IsGSTCurrencyRateChanged;
            cashSaleDTO.Status = cashSale.Status;
            cashSaleDTO.DocumentState = cashSale.DocumentState;
            cashSaleDTO.ModifiedDate = cashSale.ModifiedDate;
            cashSaleDTO.ModifiedBy = cashSale.ModifiedBy;
            cashSaleDTO.CreatedDate = cashSale.CreatedDate;
            cashSaleDTO.UserCreated = cashSale.UserCreated;
        }

        private void FillCashDetails(CashSaleDetailModel detail, CashSaleDetail casDet)
        {
            detail.AllowDisAllow = casDet.AllowDisAllow;
            detail.AmtCurrency = casDet.AmtCurrency;
            detail.BaseAmount = casDet.BaseAmount;
            detail.BaseTaxAmount = casDet.BaseTaxAmount;
            detail.BaseTotalAmount = casDet.BaseTotalAmount;
            // detail.Cashsales= casDet.CashSale;
            detail.CashSaleId = casDet.CashSaleId;
            detail.COAId = casDet.COAId;
            detail.Discount = casDet.Discount;
            detail.DiscountType = casDet.DiscountType;
            detail.DocAmount = casDet.DocAmount;
            detail.DocTaxAmount = casDet.DocTaxAmount;
            detail.DocTotalAmount = casDet.DocTotalAmount;
            detail.Id = casDet.Id;
            //detail.ItemCode = casDet.ItemCode;
            detail.ItemId = casDet.ItemId;
            detail.Qty = casDet.Qty;
            detail.RecOrder = casDet.RecOrder;
            //detail.Remarks = casDet.Remarks;
            detail.TaxCurrency = casDet.TaxCurrency;
            detail.Unit = casDet.Unit;
            detail.UnitPrice = casDet.UnitPrice;
            detail.IsPLAccount = casDet.IsPLAccount;
            detail.TaxId = casDet.TaxId;
            detail.TaxRate = casDet.TaxRate;
            detail.TaxIdCode = casDet.TaxIdCode;
            //ChartOfAccount coa = _chartOfAccountRepository.Query(a => a.Id == casDet.COAId).Select().FirstOrDefault();
            //if (coa != null)
            //    detail.AccountName = coa.Name;
            //if (casDet.TaxId != null)
            //{
            //    TaxCode tax = _taxCodeService.GetTaxCode(casDet.TaxId.Value);
            //    if (tax != null)
            //    {
            //        detail.TaxId = tax.Id;
            //        detail.TaxRate = tax.TaxRate;
            //        detail.TaxIdCode = tax.Code != "NA" ? tax.Code + "-" + tax.TaxRate + (tax.TaxRate != null ? "%" : "NA") + "(" + tax.TaxType[0] + ")" : tax.Code;
            //        detail.TaxType = tax.TaxType;
            //        detail.TaxCode = tax.Code;
            //    }
            //}
            //var lstGSTDetail = _gstDetailService.GetAllGstDetail(casDet.Id);
            //var lstGSTModel = new List<GSTDetailModel>();
            //if (lstGSTDetail != null)
            //{
            //    foreach (var gstDetail in lstGSTDetail)
            //    {
            //        GSTDetailModel gstModel = new GSTDetailModel();
            //        FillGstDetail(gstModel, gstDetail);
            //        lstGSTModel.Add(gstModel);
            //    }
            //}
            //detail.GstDetailModels = lstGSTModel;
        }

        private void UpdateCashSaleDetails(CashSaleModel TObject, CashSale _CashSaleNew)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entred_Into_UpdateCashSaleDetail_Method_And_checking_Condition);
            try
            {
                int? recorder = 0;

                foreach (CashSaleDetailModel detail in TObject.CashSaleDetails)
                {
                    if (detail.RecordStatus == "Added")
                    {
                        CashSaleDetail cashCaleNew = new CashSaleDetail();
                        FillCashSaleDetail(cashCaleNew, detail, TObject.ExchangeRate);
                        cashCaleNew.RecOrder = recorder + 1;
                        recorder = cashCaleNew.RecOrder;
                        cashCaleNew.Id = Guid.NewGuid();
                        cashCaleNew.CashSaleId = TObject.Id;
                        cashCaleNew.ObjectState = ObjectState.Added;
                        // _cashSalesDetailService.Insert(cashCaleNew);

                    }
                    else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                    {
                        CashSaleDetail cashSalesDetailE = _CashSaleNew.CashSaleDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                        if (cashSalesDetailE != null)
                        {
                            CashSaleDetail cashsalesDetail = new CashSaleDetail();
                            FillCashSalesDetails(detail, cashSalesDetailE, TObject.ExchangeRate);
                            cashsalesDetail.RecOrder = recorder + 1;
                            recorder = cashsalesDetail.RecOrder;
                            cashsalesDetail.ObjectState = ObjectState.Modified;
                        }
                    }
                    else if (detail.RecordStatus == "Deleted")
                    {
                        CashSaleDetail CashSaleDetails = _CashSaleNew.CashSaleDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                        if (CashSaleDetails != null)
                        {
                            CashSaleDetails.ObjectState = ObjectState.Deleted;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Exited_From_UpdateCashsaleDetail_Method);
        }

        private void FillCashSaleDetail(CashSaleDetail cashCaleNew, CashSaleDetailModel detail, decimal? exchangeRate)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entered_Into_Fill_Method_Of_Fill_CashSaleDeatil_Method);
            cashCaleNew.AllowDisAllow = detail.AllowDisAllow;
            cashCaleNew.AmtCurrency = detail.AmtCurrency;
            cashCaleNew.DocAmount = detail.DocAmount;
            cashCaleNew.DocTaxAmount = detail.DocTaxAmount;
            cashCaleNew.DocTotalAmount = detail.DocTotalAmount;
            cashCaleNew.TaxCurrency = detail.TaxCurrency;
            cashCaleNew.TaxId = detail.TaxId;
            cashCaleNew.TaxRate = detail.TaxRate;
            cashCaleNew.Unit = detail.Unit;
            cashCaleNew.UnitPrice = detail.UnitPrice;
            cashCaleNew.BaseAmount = exchangeRate != null ? Math.Round(cashCaleNew.DocAmount * (decimal)exchangeRate, 2, MidpointRounding.AwayFromZero) : cashCaleNew.DocAmount;
            cashCaleNew.BaseTaxAmount = exchangeRate != null ? cashCaleNew.DocTaxAmount != null ? Math.Round((decimal)cashCaleNew.DocTaxAmount * (decimal)exchangeRate, 2, MidpointRounding.AwayFromZero) : cashCaleNew.DocTaxAmount : cashCaleNew.DocTaxAmount;
            cashCaleNew.BaseTotalAmount = Math.Round((decimal)cashCaleNew.BaseAmount + (cashCaleNew.BaseTaxAmount != null ? (decimal)cashCaleNew.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
            cashCaleNew.CashSaleId = detail.CashSaleId;
            cashCaleNew.COAId = detail.COAId;
            cashCaleNew.Discount = detail.Discount;
            cashCaleNew.DiscountType = detail.DiscountType;
            cashCaleNew.Id = detail.Id;
            cashCaleNew.ItemDescription = detail.ItemDescription;
            cashCaleNew.ItemId = detail.ItemId;
            cashCaleNew.Qty = detail.Qty;
            cashCaleNew.IsPLAccount = detail.IsPLAccount;
            cashCaleNew.TaxIdCode = detail.TaxIdCode;
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Exited_From_FillCashSaleDetail_Method);
        }

        private void FillCashSalesDetails(CashSaleDetailModel cDetailNew, CashSaleDetail cashSalesDetail, decimal? exchangeRate)
        {
            cashSalesDetail.ItemId = cDetailNew.ItemId;
            cashSalesDetail.ItemDescription = cDetailNew.ItemDescription;
            cashSalesDetail.Qty = cDetailNew.Qty;
            cashSalesDetail.Unit = cDetailNew.Unit;
            cashSalesDetail.UnitPrice = cDetailNew.UnitPrice;
            cashSalesDetail.DiscountType = cDetailNew.DiscountType;
            cashSalesDetail.Discount = cDetailNew.Discount;
            cashSalesDetail.COAId = cDetailNew.COAId;
            cashSalesDetail.AllowDisAllow = cDetailNew.AllowDisAllow;
            cashSalesDetail.TaxId = cDetailNew.TaxId;
            //TaxCode taxcode = _taxCodeService.Query(a => a.Id == cashSalesDetail.TaxId).Select().FirstOrDefault();
            cashSalesDetail.TaxRate = cDetailNew.TaxRate;
            cashSalesDetail.DocTaxAmount = cDetailNew.DocTaxAmount;
            cashSalesDetail.TaxCurrency = cDetailNew.TaxCurrency;
            cashSalesDetail.DocAmount = cDetailNew.DocAmount;
            cashSalesDetail.AmtCurrency = cDetailNew.AmtCurrency;
            cashSalesDetail.DocTotalAmount = cDetailNew.DocTotalAmount;
            cashSalesDetail.BaseAmount = exchangeRate != null ? Math.Round(cashSalesDetail.DocAmount * (decimal)exchangeRate, 2, MidpointRounding.AwayFromZero) : cDetailNew.DocAmount;
            cashSalesDetail.BaseTaxAmount = (cashSalesDetail.TaxRate != null && cashSalesDetail.TaxRate != 0) ? Math.Round((decimal)cashSalesDetail.BaseAmount * (decimal)cashSalesDetail.TaxRate / 100, 2, MidpointRounding.AwayFromZero) : 0;
            cashSalesDetail.BaseTotalAmount = Math.Round((decimal)cashSalesDetail.BaseAmount + (decimal)cashSalesDetail.BaseTaxAmount, 2, MidpointRounding.AwayFromZero);
            cashSalesDetail.IsPLAccount = cDetailNew.IsPLAccount;
            cashSalesDetail.TaxIdCode = cDetailNew.TaxIdCode;
        }

        private void InsertCashSale(CashSaleModel TObject, CashSale cashsalenew)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entred_into_UpdateCashSaleDetail_method_and_checking_the_conditions);
            try
            {
                cashsalenew.BalanceAmount = TObject.BalanceAmount;
                cashsalenew.BankClearingDate = TObject.BankClearingDate;
                cashsalenew.CashSaleNumber = TObject.CashSaleNumber;
                cashsalenew.CompanyId = TObject.CompanyId;
                cashsalenew.CreatedDate = TObject.CreatedDate;
                cashsalenew.COAId = TObject.COAId;
                cashsalenew.DocDate = TObject.DocDate;
                cashsalenew.DocSubType = "General";
                cashsalenew.DocType = DocTypeConstants.CashSale;
                cashsalenew.DocumentState = "Posted";
                cashsalenew.EntityId = TObject.EntityId;
                cashsalenew.EntityType = "Customer";
                cashsalenew.ExchangeRate = TObject.ExchangeRate == null ? 1 : TObject.ExchangeRate;
                cashsalenew.ExDurationFrom = TObject.ExDurationFrom;
                cashsalenew.ExDurationTo = TObject.ExDurationTo;
                cashsalenew.ExCurrency = TObject.BaseCurrency;
                cashsalenew.ModeOfReceipt = TObject.ModeOfReceipt;
                cashsalenew.ReceiptrefNo = TObject.ReceiptrefNo;
                cashsalenew.DocDescription = TObject.DocDescription;
                cashsalenew.GrandTotal = TObject.GrandTotal;
                cashsalenew.GSTExCurrency = TObject.GSTExCurrency;
                cashsalenew.GSTTotalAmount = TObject.GSTTotalAmount;
                cashsalenew.Id = TObject.Id;
                cashsalenew.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;
                cashsalenew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                cashsalenew.IsGSTApplied = TObject.IsGSTApplied;
                cashsalenew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                cashsalenew.IsGstSettings = TObject.IsGstSettings;
                if (TObject.IsGstSettings)
                {
                    cashsalenew.GSTExchangeRate = TObject.GSTExchangeRate;

                    cashsalenew.GSTExDurationFrom = TObject.GSTExDurationFrom;
                    cashsalenew.GSTExDurationTo = TObject.GSTExDurationTo;
                }
                FinancialSettingCompact financSettings = _masterService.GetFinancialSetting(cashsalenew.CompanyId);
                if (TObject.IsMultiCurrency == true)
                    cashsalenew.DocCurrency = TObject.DocCurrency;
                else
                    cashsalenew.DocCurrency = financSettings.BaseCurrency;
                cashsalenew.IsMultiCurrency = TObject.IsMultiCurrency;
                cashsalenew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                cashsalenew.ModifiedBy = TObject.ModifiedBy;
                cashsalenew.ModifiedDate = TObject.ModifiedDate;
                cashsalenew.NoSupportingDocs = TObject.NoSupportingDocument;
                cashsalenew.PONo = TObject.PONo;
                cashsalenew.RecOrder = TObject.RecOrder;
                cashsalenew.ServiceCompanyId = TObject.ServiceCompanyId;
                cashsalenew.Status = TObject.Status;
                cashsalenew.UserCreated = TObject.UserCreated;

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Exited_From_InsertCashSale_Methos);
        }

        private void CashSaleValidations(CashSaleModel TObject)
        {
            if (TObject.DocDate == null)
            {
                throw new Exception(CashSaleValidation.Invalid_Document_Date);
            }
            if (TObject.ServiceCompanyId == null)
                throw new Exception();
            if (TObject.IsDocNoEditable == true)
            {
                CashSale _cashSaleDoc = _cashSaleService.GetCashSaleDocNo(TObject.Id, TObject.DocNo, TObject.CompanyId);
                if (_cashSaleDoc != null)
                {
                    throw new Exception(CommonConstant.Document_number_already_exist);
                }
            }
            if (TObject.GrandTotal < 0)
            {
                throw new Exception(CashSaleValidation.Grand_total_should_be_greater_than_zero);
            }
            if (TObject.CashSaleDetails.Any())
            {
                foreach (var Cashsale in TObject.CashSaleDetails)
                {
                    if (Cashsale.ItemId != null && Cashsale.Qty == null)
                        throw new Exception(CashSaleValidation.Pleas_Enter_Quantity);
                    if (Cashsale.ItemId == null && Cashsale.Qty != null)
                        throw new Exception(CashSaleValidation.Please_Select_Item);
                }
            }
            if (TObject.CashSaleDetails == null || TObject.CashSaleDetails.Count == 0)
            {
                throw new Exception(CashSaleValidation.Atleast_one_Sale_Item_is_required);
            }
            else
            {
                int itemCount = TObject.CashSaleDetails.Where(a => a.RecordStatus != RecordStatus.Deleted).Count();
                if (itemCount == 0)
                {
                    throw new Exception(CashSaleValidation.Atleast_one_Sale_Item_is_requireds);
                }
            }
            if (TObject.ExchangeRate == 0)
                throw new Exception(CashSaleValidation.ExchangeRate_Should_Be_Grater_Than_zero);

            if (TObject.GSTExchangeRate == 0)
                throw new Exception(CashSaleValidation.GSTExchangeRate_Should_Be_Grater_Than_zero);
            //Need to verify the invoice is within Financial year
            if (!_masterService.ValidateYearEndLockDate(TObject.DocDate, TObject.CompanyId))
            {
                throw new Exception(CashSaleValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }
            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_masterService.ValidateFinancialOpenPeriod(TObject.DocDate.Date, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(CashSaleValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_masterService.ValidateFinancialLockPeriodPassword(TObject.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CashSaleValidation.Invalid_Financial_Period_Lock_Password);
                }
            }
        }

        #endregion Private_Block

        #region Auto_Number_Block
        string value = "";
        public string GenerateAutoNumberForType(long companyId, string Type, string DocSubType)
        {

            AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(companyId, Type);
            string generatedAutoNumber = "";
            try
            {

                generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength),
                    _autoNo.GeneratedNumber, companyId, _autoNo, DocSubType);
                if (_autoNo != null)
                {
                    _autoNo.GeneratedNumber = value;
                    _autoNo.IsDisable = true;
                    _autoNo.ObjectState = ObjectState.Modified;
                    _autoNumberService.Update(_autoNo);
                }
                var _autonumberCompany = _autoNumberService.GetAutoNumberCompany(_autoNo.Id);
                if (_autonumberCompany.Any())
                {
                    AutoNumberComptCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                    //_autoNumberCompanyService.Update(_autoNumberCompanyNew);
                }
                else
                {
                    AutoNumberComptCompany _autoNumberCompanyNew = new AutoNumberComptCompany();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.Id = Guid.NewGuid();
                    _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                    //_autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }
            return generatedAutoNumber;
        }

        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal,
         long companyId, AutoNumberCompact autonumber, string Companycode = null)
        {
            List<CashSale> lstCashsale = null;
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
            if (Type == DocTypeConstants.CashSale)
            {
                lstCashsale = _cashSaleService.GetAllCashSale(companyId);

                if (lstCashsale.Any() && ifMonthContains)
                {
                    //AppsWorld.CashSalesModule.Entities.Models.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    int? lastCretedDate = lstCashsale.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                    if (DateTime.Now.Year == lstCashsale.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                    {
                        if (lastCretedDate == currentMonth)
                        {
                            foreach (var bill in lstCashsale)
                            {
                                if (bill.CashSaleNumber != autonumber.Preview)
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

                else if (lstCashsale.Any() && ifMonthContains == false)
                {
                    //AppsWorld.CashSalesModule.Entities.Models.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    foreach (var bill in lstCashsale)
                    {
                        if (bill.CashSaleNumber != autonumber.Preview)
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

            if (lstCashsale.Any())
            {
                OutputNumber = GetNewNumber(lstCashsale, Type, OutputNumber, counter, companyFormatHere, counterLength);
            }

            return OutputNumber;
        }
        private string GetNewNumber(List<CashSale> lstCashsale, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstCashsale.Where(a => a.CashSaleNumber == outputNumber).FirstOrDefault();
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
                    var inv = lstCashsale.Where(c => c.CashSaleNumber == val2).FirstOrDefault();
                    if (inv == null)
                        isNotexist = true;
                }
                val1 = val2;
                //value = counter = val1;
            }
            return val1;
        }
        #endregion Auto_Number_Block

        #region Posting_Block

        private void FillJournal(JVModel headJournal, CashSale _cashSale, bool isNew, string type)
        {

            //string strServiceCompany = _companyService.Query(a => a.Id == _cashSale.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = _cashSale.Id;
            FillJv(headJournal, _cashSale);
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel jModel = new JVVDetailModel();
            foreach (CashSaleDetail detail in _cashSale.CashSaleDetails)
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                FillJvDetail(_cashSale, journal, detail);
                journal.RecOrder = detail.RecOrder;
                lstJD.Add(journal);
                //journal.BaseCreditTotal = detail.BaseAmount == null ? 0 : detail.BaseAmount.Value;
            }

            if (_cashSale.IsGstSettings)
            {
                foreach (CashSaleDetail detail in _cashSale.CashSaleDetails.Where(a => a.TaxRate != null && a.TaxIdCode != "NA"))
                {
                    JVVDetailModel journal = new JVVDetailModel();
                    if (isNew)
                        journal.Id = Guid.NewGuid();
                    else
                        journal.Id = detail.Id;
                    FillJvGstDetail(_cashSale, journal, detail);
                    //journal.RecOrder = recOrder + 1;
                    //recOrder = journal.RecOrder;
                    journal.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    lstJD.Add(journal);
                }
            }
            FillJvInDetail(_cashSale, jModel);
            jModel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
            jModel.AccountDescription = _cashSale.DocDescription;
            lstJD.Add(jModel);
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0).OrderBy(c => c.RecOrder).ToList();
        }

        private void FillJvGstDetail(CashSale _cashSale, JVVDetailModel journal, CashSaleDetail detail)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = _cashSale.Id;
            journal.PostingDate = _cashSale.DocDate;
            //journal.Nature = _cashSale.Nature;
            journal.ServiceCompanyId = _cashSale.ServiceCompanyId.Value;
            journal.DocNo = _cashSale.DocNo;
            journal.DocType = _cashSale.DocType;
            journal.DocSubType = _cashSale.DocSubType;
            journal.COAId = _masterService.GetTaxPaybleGstCOA(_cashSale.CompanyId, COANameConstants.TaxPayableGST);
            journal.DocCurrency = _cashSale.DocCurrency;
            journal.BaseCurrency = _cashSale.ExCurrency;
            journal.ExchangeRate = _cashSale.ExchangeRate;
            journal.GSTExCurrency = _cashSale.GSTExCurrency;
            journal.GSTExchangeRate = _cashSale.GSTExchangeRate;
            //journal.AccountDescription = detail.ItemDescription;
            journal.AccountDescription = _cashSale.DocDescription;
            if (detail.TaxId != null)
            {
                TaxCodeCompact tax = _masterService.GetTaxById(detail.TaxId.Value);
                journal.TaxId = tax.Id;
                journal.TaxCode = tax.Code;
                journal.TaxRate = tax.TaxRate;
                journal.TaxType = tax.TaxType;
            }
            journal.DocCredit = detail.DocTaxAmount.Value;
            journal.BaseCredit = Math.Round((decimal)_cashSale.ExchangeRate == null
                ? journal.DocCredit.Value
                : (journal.DocCredit.Value * _cashSale.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            journal.IsTax = true;
        }

        private void FillJvDetail(CashSale _cashSale, JVVDetailModel journal, CashSaleDetail detail)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = _cashSale.Id;
            journal.DocNo = _cashSale.DocNo;
            journal.DocType = _cashSale.DocType;
            journal.SystemRefNo = _cashSale.CashSaleNumber;
            journal.DocSubType = _cashSale.DocSubType;
            //journal.DocSubType = DocTypeConstants.CashSale;
            journal.COAId = _masterService.GetCOAId(_cashSale.CompanyId, detail.COAId);
            journal.ServiceCompanyId = _cashSale.ServiceCompanyId.Value;
            if (detail.ItemId != null)
            {
                journal.ItemId = detail.ItemId.Value;
                journal.ItemCode = detail.ItemCode;
                journal.AccountDescription = detail.ItemDescription;
            }
            journal.Qty = detail.Qty.Value;
            journal.Unit = detail.Unit;
            journal.UnitPrice = detail.UnitPrice.Value;
            journal.Discount = detail.Discount.Value;
            journal.DiscountType = detail.DiscountType;
            journal.AllowDisAllow = detail.AllowDisAllow;
            journal.DocCurrency = _cashSale.DocCurrency;
            journal.BaseCurrency = _cashSale.ExCurrency;
            journal.ExchangeRate = _cashSale.ExchangeRate;
            journal.GSTExCurrency = _cashSale.GSTExCurrency;
            journal.GSTExchangeRate = _cashSale.GSTExchangeRate;
            if (detail.TaxId != null)
            {
                TaxCodeCompact tax = _masterService.GetTaxById(detail.TaxId.Value);
                journal.TaxId = tax.Id;
                journal.TaxCode = tax.Code;
                journal.TaxRate = tax.TaxRate;
                journal.TaxType = tax.TaxType;
            }
            journal.DocCredit = detail.DocAmount;
            journal.BaseCredit = Math.Round((decimal)(_cashSale.ExchangeRate == null
                ? journal.DocCredit
                : (journal.DocCredit * _cashSale.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
            journal.DocTaxableAmount = detail.DocAmount;
            journal.DocTaxAmount = detail.DocTaxAmount;
            journal.BaseTaxableAmount = detail.BaseAmount;
            journal.BaseTaxAmount = detail.BaseTaxAmount;
            journal.IsTax = false;
            journal.DocCreditTotal = detail.DocTotalAmount;
            journal.PostingDate = _cashSale.DocDate;
        }

        private static void FillJvInDetail(CashSale _cashSale, JVVDetailModel jModel)
        {
            jModel.DocumentId = _cashSale.Id;
            jModel.SystemReferenceNo = _cashSale.CashSaleNumber;
            jModel.SystemRefNo = _cashSale.CashSaleNumber;
            jModel.DocNo = _cashSale.DocNo;
            jModel.ServiceCompanyId = _cashSale.ServiceCompanyId.Value;
            //jModel.Nature = _cashSale.Nature;
            jModel.DocSubType = _cashSale.DocSubType;
            jModel.DocType = DocTypeConstants.CashSale;
            jModel.COAId = _cashSale.COAId;
            //jModel.Remarks = _cashSale.Remarks;
            //jModel.PONo = _cashSale.PONo;
            //jModel.CreditTermsId = _cashSale.CreditTermsId;
            //jModel.DueDate = _cashSale.DueDate;
            jModel.AccountDescription = _cashSale.DocDescription;
            jModel.PostingDate = _cashSale.DocDate;
            jModel.EntityId = _cashSale.EntityId;
            jModel.DocCurrency = _cashSale.DocCurrency;
            jModel.BaseCurrency = _cashSale.ExCurrency;
            jModel.ExchangeRate = _cashSale.ExchangeRate;
            jModel.GSTExCurrency = _cashSale.GSTExCurrency;
            jModel.GSTExchangeRate = _cashSale.GSTExchangeRate;
            jModel.DocDebit = _cashSale.GrandTotal;
            //jModel.BaseDebit = Math.Round((decimal)jModel.ExchangeRate == null ? jModel.DocDebit.Value : (jModel.DocDebit * jModel.ExchangeRate).Value, 2);
            decimal amount = 0;
            foreach (var detail in _cashSale.CashSaleDetails)
            {
                amount = Math.Round((decimal)(amount + (detail.DocAmount * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
            }
            jModel.BaseDebit = amount;

            jModel.SegmentCategory1 = _cashSale.SegmentCategory1;
            jModel.SegmentCategory2 = _cashSale.SegmentCategory2;
            jModel.SegmentMasterid1 = _cashSale.SegmentMasterid1;
            jModel.SegmentMasterid2 = _cashSale.SegmentMasterid2;
            jModel.SegmentDetailid1 = _cashSale.SegmentDetailid1;
            jModel.SegmentDetailid2 = _cashSale.SegmentDetailid2;
            jModel.BaseAmount = _cashSale.BalanceAmount;
            jModel.DocDate = _cashSale.DocDate;
        }

        private void FillJv(JVModel headJournal, CashSale _cashSale)
        {
            headJournal.DocumentId = _cashSale.Id;
            headJournal.CompanyId = _cashSale.CompanyId;
            headJournal.PostingDate = _cashSale.DocDate;
            headJournal.DocNo = _cashSale.DocNo;
            headJournal.DocumentDescription = _cashSale.DocDescription;
            headJournal.DocType = DocTypeConstants.CashSale;
            headJournal.DocSubType = _cashSale.DocSubType;
            //headJournal.DocSubType = DocTypeConstants.CashSale;
            headJournal.DocDate = _cashSale.DocDate;
            //headJournal.DueDate = _cashSale.DueDate;
            headJournal.DocumentState = _cashSale.DocumentState;
            headJournal.SystemReferenceNo = _cashSale.CashSaleNumber;
            headJournal.ServiceCompanyId = _cashSale.ServiceCompanyId;
            //headJournal.ServiceCompany = strServiceCompany;
            //headJournal.Nature = invoice.Nature;
            headJournal.PoNo = _cashSale.PONo;
            headJournal.ExDurationFrom = _cashSale.ExDurationFrom;
            headJournal.ExDurationTo = _cashSale.ExDurationTo;
            headJournal.IsAllowableNonAllowable = _cashSale.IsAllowableNonAllowable;
            headJournal.GSTExDurationFrom = _cashSale.GSTExDurationFrom;
            headJournal.GSTExDurationTo = _cashSale.GSTExDurationTo;
            //headJournal.CreditTerms = _cashSale;
            //headJournal.BalanceAmount = invoice.BalanceAmount;
            //if (top != null)
            //    headJournal.CreditTerms = (int)top.TOPValue;
            headJournal.IsSegmentReporting = _cashSale.IsSegmentReporting;
            headJournal.SegmentCategory1 = _cashSale.SegmentCategory1;
            headJournal.SegmentCategory2 = _cashSale.SegmentCategory2;
            headJournal.SegmentMasterid1 = _cashSale.SegmentMasterid1;
            headJournal.SegmentMasterid2 = _cashSale.SegmentMasterid2;
            headJournal.SegmentDetailid1 = _cashSale.SegmentDetailid1;
            headJournal.SegmentDetailid2 = _cashSale.SegmentDetailid2;
            headJournal.NoSupportingDocument = _cashSale.NoSupportingDocs;
            headJournal.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            headJournal.EntityId = _cashSale.EntityId;
            headJournal.EntityType = _cashSale.EntityType;
            headJournal.ModeOfReceipt = _cashSale.ModeOfReceipt;
            //headJournal.IsRepeatingInvoice = _cashSale.IsRepeatingInvoice;
            //headJournal.RepEveryPeriod = _cashSale.RepEveryPeriod;
            //headJournal.RepEveryPeriodNo = _cashSale.RepEveryPeriodNo;
            //headJournal.EndDate = invoice.RepEndDate;
            //ChartOfAccount account =
            //    _chartOfAccountRepository.Query(
            //            a =>
            //                a.Name ==
            //                (_cashSale.CashSaleNumber == "Trade"
            //                    ? COANameConstants.AccountsReceivables
            //                    : COANameConstants.OtherReceivables) && a.CompanyId == _cashSale.CompanyId)
            //        .Select()
            //        .FirstOrDefault();
            headJournal.COAId = _cashSale.COAId;
            //headJournal.AccountCode = account.Code;
            //headJournal.AccountName = account.Name;
            headJournal.DocCurrency = _cashSale.DocCurrency;
            headJournal.BaseCurrency = _cashSale.ExCurrency;
            headJournal.ExchangeRate = _cashSale.ExchangeRate;
            headJournal.IsGstSettings = _cashSale.IsGstSettings;
            headJournal.IsGSTApplied = _cashSale.IsGSTApplied;
            headJournal.IsMultiCurrency = _cashSale.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = _cashSale.IsNoSupportingDocument;
            headJournal.GrandDocDebitTotal = _cashSale.GrandTotal;
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(_cashSale.GrandTotal * (_cashSale.ExchangeRate != null ? _cashSale.ExchangeRate : 1)), 2);
            if (_cashSale.IsGstSettings)
            {
                headJournal.GSTExCurrency = _cashSale.GSTExCurrency;
                headJournal.GSTExchangeRate = _cashSale.GSTExchangeRate;
            }

            headJournal.Remarks = _cashSale.Remarks;
            headJournal.UserCreated = _cashSale.UserCreated;
            headJournal.CreatedDate = _cashSale.CreatedDate;
            headJournal.ModifiedBy = _cashSale.ModifiedBy;
            headJournal.ModifiedDate = _cashSale.ModifiedDate;
            headJournal.TransferRefNo = _cashSale.ReceiptrefNo;//added by lokanath
        }

        public void SaveCashSalePosting(JVModel jvModel)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entered_into_SaveInvoice1_Method);

            var json = RestSharpHelper.ConvertObjectToJason(jvModel);

            string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
            //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
            //if (section.Ziraff.Count > 0)
            //{
            //    for (int i = 0; i < section.Ziraff.Count; i++)
            //    {
            //        if (section.Ziraff[i].Name == CommonConstants.IdentityBean)
            //        {
            //            url = section.Ziraff[i].ServerUrl;
            //            break;
            //        }
            //    }
            //}
            try
            {
                object obj = jvModel;
                // string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestSharpHelper.Post(url, "api/journal/saveposting", json);
                if (response.ErrorMessage != null)
                {
                }
            }
            catch (Exception ex)
            {
                var response = RestSharpHelper.Post(url, "api/journal/saveposting", json);
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                var message = ex.Message;
            }
        }

        #endregion Posting_Block

        #region VoidPosting
        public void VoidJVPostCashSale(DocumentVoidModel tObject)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entered_into_VoidJVPostCashSale_Method);
            var json = RestSharpHelper.ConvertObjectToJason(tObject);

            string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
            //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
            //if (section.Ziraff.Count > 0)
            //{
            //    for (int i = 0; i < section.Ziraff.Count; i++)
            //    {
            //        if (section.Ziraff[i].Name == CommonConstants.IdentityBean)
            //        {
            //            url = section.Ziraff[i].ServerUrl;
            //            break;
            //        }
            //    }
            //}
            object obj = tObject;
            try
            {
                var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
                if (response.ErrorMessage != null)
                {
                    //LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
                var message = ex.Message;
            }
        }
        #endregion
    }
}
