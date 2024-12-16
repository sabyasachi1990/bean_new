using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.RevaluationModule.Entities.V2;
using AppsWorld.RevaluationModule.Models;
using AppsWorld.RevaluationModule.RepositoryPattern.V2;
using AppsWorld.RevaluationModule.Service.V2;
using AppsWorld.RevaluationModule.Infra;
using System.Data.SqlClient;
using System.Data;
using AppsWorld.Framework;
using System.Net;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Ziraff.FrameWork.Logging;
using Ziraff.Section;
using System.Configuration;
using AppsWorld.CommonModule.Infra;
using AppsWorld.RevaluationModule.Service.V2.Get_Save_Services;
using AppsWorld.RevaluationModule.Entities.Models.V2;

namespace AppsWorld.RevaluationModule.Application.V2
{
    public class RevaluationApplicationService
    {
        readonly IRevaluationService _revaluationService;
        readonly IRevaluationUnitOfWorkAsync _unitOfWorkAsync;
        readonly IMasterCompactService _masterService;
        readonly IAutoNumberService _autoNumberService;
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string value = string.Empty;
        readonly ICommonForexService _commonForexService;
        public RevaluationApplicationService(IRevaluationService revaluationService, AppsWorld.RevaluationModule.Service.IMultiCurrencySettingService multiCurrencyService, IRevaluationUnitOfWorkAsync unitOfWorkAsync, IMasterCompactService masterService, IAutoNumberService autoNumberService, ICommonForexService commonForexService)
        {
            this._revaluationService = revaluationService;
            this._unitOfWorkAsync = unitOfWorkAsync;
            _masterService = masterService;
            _autoNumberService = autoNumberService;
            this._commonForexService = commonForexService;
        }

        #region Lookup
        public RevaluationLUs RevaluationLUs(long companyId, Guid revaluationId, string userName, string ConnectionString)
        {
            Revaluation revaluation = _revaluationService.GetRevaluationById(revaluationId, companyId);
            List<Lookups<string>> lstLookUps = new List<Lookups<string>>();
            RevaluationLUs revaluationLus = new Models.RevaluationLUs();
            try
            {
                long? comp = revaluation == null ? 0 : revaluation.ServiceCompanyId == null ? 0 : revaluation.ServiceCompanyId;
                string query = $"SELECT 'FINANCIAL' as TABLENAME,F.Id as ID,'' as NAME,'' as SHOTNAME,0 as IsGstActive,1 as STATUS,F.PeriodLockDate as StartDate,F.PeriodEndDate as EndDate,0 as IsRevaluation FROM Bean.FinancialSetting F where CompanyId={companyId};SELECT 'MULTICURRENCY' as TABLENAME, M.Id as ID,'' as NAME,'' as SHOTNAME,0 as IsGstActive,1 as STATUS,NULL as StartDate,NULL as EndDate,M.Revaluation as IsRevaluation from Bean.MultiCurrencySetting M where CompanyId={companyId};SELECT 'SERVICECOMPANY' as TABLENAME,C.Id as ID,c.Name as NAME,c.ShortName as SHOTNAME,c.IsGstSetting as IsGstActive,1 as STATUS,NULL as StartDate,NULL as EndDate,0 as IsRevaluation FROM Common.Company c JOIN Common.CompanyUser CU on C.ParentId=CU.CompanyId Join common.CompanyUserDetail CUD On (c.Id = CUD.ServiceEntityId and CU.Id = CUD.CompanyuserId) where (c.Status = 1 or c.Id = {comp} ) and c.ParentId = {companyId} and CU.Username= '{userName}';";
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
                                lstLookUps.Add(new Lookups<string>
                                {
                                    TableName = dr["TABLENAME"].ToString(),
                                    Id = dr["ID"] != DBNull.Value ? Convert.ToInt64(dr["ID"]) : 0,
                                    Name = dr["NAME"].ToString(),
                                    ShortName = dr["SHOTNAME"].ToString(),
                                    isGstActivated = dr["IsGstActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsGstActive"]) : (bool?)null,
                                    StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : (DateTime?)null,
                                    EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : (DateTime?)null,
                                    IsRevaluation = dr["IsRevaluation"] != DBNull.Value ? Convert.ToBoolean(dr["IsRevaluation"]) : (bool?)null,
                                    Status = (RecordStatusEnum)dr["STATUS"]
                                });
                            }
                        }
                        dr.NextResult();
                    }
                }
                if (lstLookUps.Any())
                {

                    long financeId = lstLookUps.Where(c => c.TableName == "FINANCIAL").Select(d => d.Id).FirstOrDefault();
                    if (financeId == 0)
                        throw new Exception(RevaluationConstant.The_Financial_setting_should_be_activated);
                    revaluationLus.FinancialPeriodLockStartDate = lstLookUps.Where(c => c.TableName == "FINANCIAL").Select(d => d.StartDate).FirstOrDefault();
                    revaluationLus.FinancialPeriodLockEndDate = lstLookUps.Where(c => c.TableName == "FINANCIAL").Select(d => d.EndDate).FirstOrDefault();
                    revaluationLus.IsRevaluation = lstLookUps.Where(c => c.TableName == "MULTICURRENCY").Select(d => d.IsRevaluation).FirstOrDefault();
                    revaluationLus.IsLocked = revaluation != null ? revaluation.IsLocked : false;
                    revaluationLus.SubsideryCompanyLU = lstLookUps.Where(c => c.TableName == "SERVICECOMPANY").Select(x => new AppsWorld.RevaluationModule.Models.LookUpCompany<string>()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        isGstActivated = x.isGstActivated
                    }).OrderBy(x => x.ShortName).ToList();
                    if (revaluation != null)
                    {
                        revaluationLus.CreatedDate = revaluation.CreatedDate;
                        revaluationLus.UserCreated = revaluation.UserCreated;
                        revaluationLus.IsMultiCurrency = revaluation.IsMultiCurrency;
                        revaluationLus.DocState = revaluation.DocState;
                        revaluationLus.Version = "0x" + string.Concat(Array.ConvertAll(revaluation.Version, x => x.ToString("X2")));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(RevaluationLoggingValidation.RevaluationApplicationService, ex, ex.Message);
            }
            return revaluationLus;
        }
        #endregion Lookup

        #region Create_Calls
        public List<RevaluationModel> CreateRevaluation(DateTime revalDate, long companyId, Guid id, string serviceCompanyIds, string ConnectionString)
        {
            if (revalDate.Date > DateTime.Now.Date)
                throw new InvalidOperationException(RevaluationValidation.Revaluation_cant_run_for_future_date);
            List<RevaluationModel> lstRevaluationModel = new List<RevaluationModel>();
            Revaluation revaluation = _revaluationService.GetAllRevaluationAndDetail(id, companyId);
            try
            {
                bool isMultiCurrency = _masterService.IsMultiCurrecySettings(companyId);
                if (!isMultiCurrency)
                    throw new InvalidOperationException(RevaluationConstant.Multi_Currency_should_be_activate);
                if (revaluation == null)
                {
                    List<long> servCompanyIds = serviceCompanyIds.Split(',').Select(long.Parse).ToList();
                    List<long?> lstRevaluedCOAIds = _revaluationService.GetAllRevaluedCOAIds(revalDate, servCompanyIds);
                    List<string> currencies = _revaluationService.GetJDDocCurrecies(servCompanyIds);
                    string baseCurr = _masterService.GetBaseCurrency(companyId);
                    Dictionary<string, decimal> exchangesRates = new Dictionary<string, decimal>();
                    decimal? revalBal = 0;
                    decimal? baseBal = 0;
                    decimal exRates = 0;
                    long? COAId = 0;
                    //string docCurr = null;
                    if (currencies.Any())
                    {
                        //foreach (var curr in currencies)
                        //    exchangesRates.Add(curr, GetExRateInformations(curr, revalDate, companyId, baseCurr));//commented on 01/06/2020



                        Dictionary<string, decimal> getListOfExrates = _commonForexService.GetListOfExchangeRates(companyId, currencies, baseCurr, revalDate);
                        //int counter = 0;
                        //int threshold = 6;
                        foreach (var curr in currencies)
                        {
                            exchangesRates.Add(curr, getListOfExrates.Any() ? getListOfExrates.Where(a => a.Key == curr).Any() ? getListOfExrates.Where(a => a.Key == curr).Select(c => c.Value).FirstOrDefault() : GetExRateInformation(curr, baseCurr, revalDate, companyId) : GetExRateInformation(curr, baseCurr, revalDate, companyId));
                            //counter++;
                            //if (counter >= threshold)
                            //{
                            //    _unitOfWorkAsync.SaveChanges();
                            //    _unitOfWorkAsync.Dispose();
                            //    context = new RevaluationContext();
                            //    context.Configuration.AutoDetectChangesEnabled = false;
                            //    counter = 0;
                            //}

                        }

                    }
                    //string query = $"SELECT 'GENERAL' as TABLENAME,JD.DocDate as DocDate,c.ShortName as SevEntitity,JD.DocNo as DocNo,JD.DocType as DocType,Jd.DocCurrency as Currency,J.BalanceAmount as DocBalance,J.ExchangeRate as OrgRate, SUM(ISNULL(J.BalanceAmount, 0) * J.ExchangeRate) as BaseBal,E.Name as EntName,COA.Name as COAName FROM Bean.Journal J INNER JOIN Bean.JournalDetail JD on JD.JournalId = J.Id INNER JOIN Bean.ChartOfAccount COA on JD.COAId = COA.Id INNER JOIN Bean.Entity E on J.EntityId = E.Id INNER JOIN Common.Company c on J.ServiceCompanyId = c.Id where J.DocDate <= '2019-01-07 00:00:00.0000000' AND J.CompanyId = 1 AND J.DocumentState NOT IN('Fully Paid', 'Fully Applied', 'Void', 'Parked', 'Recurring') AND J.ServiceCompanyId = 3 AND(J.ClearingStatus <> 'Cleared' OR J.ClearingDate is NULL) AND J.DocSubType NOT IN('Revaluation', 'CM Application', 'Application') AND JD.IsTax <> 1 AND JD.DocCurrency <> JD.BaseCurrency AND COA.Revaluation = 1 AND COA.IsBank <> 1 AND JD.DocumentDetailId <> '00000000-0000-0000-0000-000000000000' group by JD.DocDate, c.ShortName, JD.DocNo, JD.DocType, Jd.DocCurrency, J.BalanceAmount, J.ExchangeRate";

                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand("BEAN_REVALUATION", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);
                        cmd.Parameters.AddWithValue("@ServiceCompanyId", serviceCompanyIds);
                        cmd.Parameters.AddWithValue("@DocDate", revalDate);
                        dr = cmd.ExecuteReader();
                        do
                        {
                            while (dr.Read())
                            {
                                lstRevaluationModel.Add(new RevaluationModel()
                                {
                                    TableName = dr["TABLENAME"] != DBNull.Value ? Convert.ToString(dr["TABLENAME"]) : null,
                                    DocDate = (Convert.ToString(dr["TABLENAME"]) == "BANK" || Convert.ToString(dr["TABLENAME"]) == "OTHER") ? revalDate : dr["DocDate"] != DBNull.Value ? Convert.ToDateTime(dr["DocDate"]) : (DateTime?)null,
                                    ServiceEntityName = dr["SevEntitity"] != DBNull.Value ? Convert.ToString(dr["SevEntitity"]) : null,
                                    DocNo = dr["DocNo"] != DBNull.Value ? Convert.ToString(dr["DocNo"]) : null,
                                    DocType = dr["DocType"] != DBNull.Value ? Convert.ToString(dr["DocType"]) : null,
                                    DocCurrency = dr["Currency"] != DBNull.Value ? Convert.ToString(dr["Currency"]) : null,
                                    DocBal = Math.Round(dr["DocBal"] != DBNull.Value ? Convert.ToDecimal(dr["DocBal"]) : 0, 2, MidpointRounding.AwayFromZero),
                                    OrgExchangeRate = dr["OrgRate"] != DBNull.Value ? Convert.ToDecimal(dr["OrgRate"]) : (decimal?)null,
                                    BaseBal = baseBal = Math.Round(dr["BaseBal"] != DBNull.Value ? Convert.ToDecimal(dr["BaseBal"]) : 0, 2, MidpointRounding.AwayFromZero),
                                    EntityName = dr["EntName"] != DBNull.Value ? Convert.ToString(dr["EntName"]) : null,
                                    COAName = dr["COAName"] != DBNull.Value ? Convert.ToString(dr["COAName"]) : null,
                                    EntityId = dr["EntityId"] != DBNull.Value ? Guid.Parse(dr["EntityId"].ToString()) : (Guid?)null,
                                    COAId = COAId = dr["COAID"] != DBNull.Value ? Convert.ToInt64(dr["COAID"].ToString()) : (long?)null,
                                    //RevalExchangeRate = exRates != null ? exRates.Where(c => c.Key == dr["Currency"].ToString()).Select(c => c.Value).FirstOrDefault() : (decimal?)null,
                                    //RevalExchangeRate = exRates = GetExRateInformations(dr["Currency"].ToString(), revalDate, companyId, baseCurr),
                                    RevalExchangeRate = exRates = exchangesRates.Where(c => c.Key == dr["Currency"].ToString()).Select(c => c.Value).FirstOrDefault(),
                                    ServiceCompanyId = dr["ServiceEntityId"] != DBNull.Value ? Convert.ToInt64(dr["ServiceEntityId"].ToString()) : (long?)null,
                                    Nature = Convert.ToString(dr["Nature"]),
                                    RevalBal = revalBal = Math.Round((dr["DocBal"] != DBNull.Value ? Convert.ToDecimal(dr["DocBal"]) : 0) * (exRates != 0 ? exRates : 0), 2, MidpointRounding.AwayFromZero),
                                    DocumentId = dr["DocumentId"] != DBNull.Value ? Guid.Parse(dr["DocumentId"].ToString()) : (Guid?)null,
                                    UnrealisedExchangegainorlose = Convert.ToString(dr["Nature"]) == "Credit" ? -Math.Round((decimal)revalBal - (decimal)baseBal, 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)revalBal - (decimal)baseBal, 2, MidpointRounding.AwayFromZero),
                                    BaseCurrency = baseCurr,
                                    IsChecked = lstRevaluedCOAIds != null ? lstRevaluedCOAIds.Any(c => c.Value == COAId) : false,
                                    COAClass = dr["COAClass"] != DBNull.Value ? Convert.ToString(dr["COAClass"]) : null,
                                    IsBankData = Convert.ToString(dr["TABLENAME"]) == "BANK"
                                });
                            }
                        } while (dr.NextResult());
                    }
                    con.Close();
                }
                else
                {
                    string entIds = string.Join(",", revaluation.RevalutionDetails.Where(c => c.EntityId != new Guid()).Select(c => c.EntityId).ToList()) == string.Empty ? "00000000-0000-0000-0000-000000000000" : string.Join(",", revaluation.RevalutionDetails.Where(c => c.EntityId != new Guid()).Select(c => c.EntityId).ToList());
                    string coaids = string.Join(",", revaluation.RevalutionDetails.Select(c => c.COAId).ToList());
                    //string query = $"SELECT 'SERVICECOMP' as TABLENAME,C.Id as ServiceEntityId,0 as COAID,'00000000-0000-0000-0000-000000000000' as EntityId,C.ShortName as ServiceEntityName,'' as COAName,'' as EntityName FROM Common.Company C where Id={serviceCompanyIds};SELECT 'ENTITY' as TABLENAME,0 as ServiceEntityId,0 as COAID,E.Id as EntityId,'' as ServiceEntityName,'' as COAName,E.Name as EntityName FROM Bean.Entity E where Id in (Select items from dbo.SplitToTable('{entIds}',','));SELECT 'COA' as TABLENAME,0 as ServiceEntityId,COA.Id as COAID,'00000000-0000-0000-0000-000000000000' as EntityId,'' as ServiceEntityName,COA.Name as COAName,'' as EntityName FROM Bean.ChartOfAccount COA where Id in (Select items from dbo.SplitToTable('{coaids}',','))";//OLD Commented on 20/01/2020
                    string query = $"SELECT 'SERVICECOMP' as TABLENAME,C.Id as ServiceEntityId,0 as COAID,'00000000-0000-0000-0000-000000000000' as EntityId,C.ShortName as ServiceEntityName,'' as COAName,'' as EntityName,'' as Nature FROM Common.Company C where Id={serviceCompanyIds};SELECT 'ENTITY' as TABLENAME,0 as ServiceEntityId,0 as COAID,E.Id as EntityId,'' as ServiceEntityName,'' as COAName,E.Name as EntityName,'' as Nature FROM Bean.Entity E where Id in (Select items from dbo.SplitToTable('{entIds}',','));SELECT 'COA' as TABLENAME,0 as ServiceEntityId,COA.Id as COAID,'00000000-0000-0000-0000-000000000000' as EntityId,'' as ServiceEntityName,COA.Name as COAName,'' as EntityName,COA.Nature as Nature FROM Bean.ChartOfAccount COA where Id in (Select items from dbo.SplitToTable('{coaids}',','))";
                    int resultSetCount = query.Split(';').Count();
                    List<RevalDetailModel> lstRevalDetail = new List<RevalDetailModel>();
                    con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    using (cmd = new SqlCommand(query, con))
                    {
                        SqlDataReader dr = cmd.ExecuteReader();
                        for (int i = 1; i <= resultSetCount; i++)
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    lstRevalDetail.Add(new RevalDetailModel()
                                    {
                                        TABLENAME = dr["TABLENAME"].ToString(),
                                        COAID = dr["COAID"] != DBNull.Value ? Convert.ToInt64(dr["COAID"]) : 0,
                                        ServiceEntityId = dr["ServiceEntityId"] != DBNull.Value ? Convert.ToInt64(dr["ServiceEntityId"]) : 0,
                                        EntityId = dr["EntityId"] != DBNull.Value ? Guid.Parse(dr["EntityId"].ToString()) : (Guid?)null,
                                        ServiceEntityName = dr["ServiceEntityName"].ToString(),
                                        COAName = dr["COAName"].ToString(),
                                        EntityName = dr["EntityName"].ToString(),
                                        Nature = dr["Nature"].ToString(),
                                    });
                                }
                            }
                            dr.NextResult();
                        }
                    }
                    if (con.State != ConnectionState.Closed)
                        con.Close();
                    lstRevaluationModel.AddRange(revaluation.RevalutionDetails.Select(d => new RevaluationModel()
                    {
                        Id = d.Id,
                        RevalutionId = d.RevalutionId,
                        EntityId = d.EntityId,
                        COAId = d.COAId,
                        DocBal = d.DocBal,
                        BaseBal = d.BaseCurrencyAmount1,
                        RevalBal = d.BaseCurrencyAmount2,
                        DocNo = d.DocumentNumber,
                        DocDate = d.DocumentDate,
                        ServiceCompanyId = d.ServiceEntityId,
                        IsChecked = d.IsChecked,
                        COAName = lstRevalDetail.Where(c => c.TABLENAME == "COA" && c.COAID == d.COAId).Select(c => c.COAName).FirstOrDefault(),
                        ServiceEntityName = lstRevalDetail.Where(c => c.TABLENAME == "SERVICECOMP" && c.ServiceEntityId == d.ServiceEntityId).Select(c => c.ServiceEntityName).FirstOrDefault(),
                        EntityName = lstRevalDetail.Where(c => c.TABLENAME == "ENTITY" && c.EntityId == d.EntityId).Select(c => c.EntityName).FirstOrDefault() == null || lstRevalDetail.Where(c => c.TABLENAME == "ENTITY" && c.EntityId == d.EntityId).Select(c => c.EntityName).FirstOrDefault() == string.Empty ? lstRevalDetail.Where(c => c.TABLENAME == "COA" && c.COAID == d.COAId).Select(c => c.COAName).FirstOrDefault() : lstRevalDetail.Where(c => c.TABLENAME == "ENTITY" && c.EntityId == d.EntityId).Select(c => c.EntityName).FirstOrDefault(),
                        DocCurrency = d.DocCurrency,
                        DocType = d.DocumentType,
                        DocumentId = d.DocId,
                        RecOrder = d.RecOrder,
                        OrgExchangeRate = d.ExchangerateOld,
                        RevalExchangeRate = d.ExchangerateNew,
                        Version = "0x" + string.Concat(Array.ConvertAll(revaluation.Version, x => x.ToString("X2"))),
                        UnrealisedExchangegainorlose = /*lstRevalDetail.Where(c => c.TABLENAME == "COA" && c.COAID == d.COAId).Select(c => c.Nature).FirstOrDefault() == "Credit" ? -d.UnrealisedExchangegainorlose :*/ d.UnrealisedExchangegainorlose
                    }));
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(RevaluationLoggingValidation.RevaluationApplicationService, ex, ex.Message);
            }
            return lstRevaluationModel.Where(a => a.DocBal != 0).OrderBy(c => c.RecOrder).ToList();
        }
        public RevaluationSaveModel CreateRevaluationModel(long companyId)
        {
            RevaluationSaveModel revaluationSaveModel = new RevaluationSaveModel();
            revaluationSaveModel.RevaluationDate = DateTime.UtcNow;
            List<RevaluationModel> lstRevaluationModel = new List<RevaluationModel>();
            lstRevaluationModel.Add(new RevaluationModel());
            revaluationSaveModel.RevaluationModels = lstRevaluationModel;
            return revaluationSaveModel;
        }

        #endregion Create_Calls

        #region Save Revaluation
        public Revaluation SaveRevaluation(RevaluationSaveModel Robject, string ConnectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(Robject));
            Ziraff.FrameWork.Logging.LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, "ObjectSave", AdditionalInfo);

            LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, RevaluationLoggingValidation.Entered_into_SaveRevaluation_method);
            if (Robject.RevaluationModels == null)
                throw new Exception(RevaluationValidation.Revaluation_records_are_not_found);
            if (Robject.RevaluationDate > DateTime.Now.Date)
                throw new Exception(RevaluationValidation.Revaluation_cant_run_for_future_date);
            //if (Robject.FinancialPeriodLockEndDate != null && Robject.FinancialPeriodLockStartDate != null)
            //    if (!_masterService.RevaluationPeriodLuck(Robject.RevaluationDate.Value.AddDays(1), Robject.CompanyId))
            //        throw new Exception(RevaluationValidation.Date_should_be_in_accounting_period_to_run_the_revaluation);
            if (!_masterService.ValidateFinancialOpenPeriod(Robject.RevaluationDate.Value.AddDays(1), Robject.CompanyId))
            {
                if (String.IsNullOrEmpty(Robject.PeriodLockPassword))
                {
                    throw new Exception(RevaluationValidation.Revaluation_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_masterService.ValidateFinancialLockPeriodPassword(Robject.RevaluationDate.Value.AddDays(1), Robject.PeriodLockPassword, Robject.CompanyId))
                {
                    throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }
            }
            if (Robject.RevaluationModels.GroupBy(c => c.ServiceCompanyId).Count() > 1)
                throw new Exception(RevaluationValidation.Please_select_only_one_service_entity_to_proceed_save);
            if (Robject.RevaluationModels.Any(c => c.RevalExchangeRate == null || c.RevalExchangeRate == 0))
                throw new Exception(RevaluationValidation.Exchange_rates_are_not_found);
            if (_revaluationService.GetAllPostedRevaluation(Robject.RevaluationDate, Robject.ServiceCompanyId))
                throw new Exception(RevaluationValidation.Revaluation_can_run_once_per_same_date);
            Revaluation revaluation = _revaluationService.GetRevaluationById(Robject.Id, Robject.CompanyId);
            List<RevalutionDetail> lstRevaluation = new List<RevalutionDetail>();
            if (revaluation != null)
            {
                LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, RevaluationLoggingValidation.Entered_into_if_block_and_check_revaluation_is_null_or_not);
                //Data concurrency verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(revaluation.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(Robject.Version))
                    throw new Exception(RevaluationConstant.Document_has_been_modified_outside);

                LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, RevaluationLoggingValidation.UpdateRevaluation_method_came);
                FillRevaluation(revaluation, Robject);
                revaluation.ModifiedBy = Robject.ModifiedBy;
                revaluation.ModifiedDate = DateTime.UtcNow;
                revaluation.ObjectState = ObjectState.Modified;
                _revaluationService.Update(revaluation);
            }
            else
            {
                revaluation = new Revaluation();
                LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, RevaluationLoggingValidation.Execution_save_new_revaluation);
                FillRevaluation(revaluation, Robject);
                LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, RevaluationLoggingValidation.Out_from_UpdateRevaluation_method);
                revaluation.Id = Guid.NewGuid();
                revaluation.DocState = RevaluationConstant.Posted;
                if (Robject.RevaluationModels.Any())
                {
                    //Robject.RevaluationModels.Select(d => new RevalutionDetail()
                    //{
                    //    Id = Guid.NewGuid(),
                    //    COAId = d.COAId,
                    //    RevalutionId = revaluation.Id,
                    //    CreatedDate = DateTime.UtcNow,
                    //    DocCurrency = d.DocCurrency,
                    //    DocumentDate = d.DocDate,
                    //    DocumentNumber = d.DocNo,
                    //    EntityId = d.EntityId,
                    //    ExchangerateNew = d.RevalExchangeRate,
                    //    ExchangerateOld = d.OrgExchangeRate,
                    //    UnrealisedExchangegainorlose = d.UnrealisedExchangegainorlose,
                    //    Status = AppsWorld.Framework.RecordStatusEnum.Active,
                    //    DocBal = d.DocBal,
                    //    ServiceEntityId = d.ServiceCompanyId,
                    //    IsChecked = d.IsChecked,
                    //    ObjectState = ObjectState.Added,
                    //});
                    RevalutionDetail revalDetail = new RevalutionDetail();
                    foreach (var detailModel in Robject.RevaluationModels)
                    {
                        revalDetail = new RevalutionDetail();
                        RevaluationDetailModelToEntity(revalDetail, detailModel);
                        revalDetail.RevalutionId = revaluation.Id;
                        revalDetail.ObjectState = ObjectState.Added;
                        _revaluationService.RevalDetailInsert(revalDetail);
                    }
                }
                revaluation.SystemRefNo = _autoNumberService.GetAutonumber(Robject.CompanyId, DocTypeConstants.JournalVocher, ConnectionString);
                revaluation.Status = RecordStatusEnum.Active;
                revaluation.CreatedDate = DateTime.UtcNow;
                revaluation.UserCreated = Robject.UserCreated;
                revaluation.ObjectState = ObjectState.Added;
                _revaluationService.Insert(revaluation);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                //JVModel jvm = new JVModel();
                //int count = 1;
                //while (count <= 2)
                //{
                //    FillRevaluationPosting(jvm, revaluation, true, count == 2, ConnectionString);
                //    SaveRevaluationPosting(jvm);
                //    count++;
                //}

                #region Revaluation_Posting_SP
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("Revaluation_Posting_Sp", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyId", revaluation.CompanyId);
                    cmd.Parameters.AddWithValue("@RevalId", revaluation.Id);
                    cmd.Parameters.AddWithValue("@Type", DocTypeConstant.Revaluation);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                #endregion Revaluation_Posting_SP

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(RevaluationLoggingValidation.RevaluationApplicationService, ex, ex.Message);
            }
            LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, RevaluationLoggingValidation.End_of_the_SaveRevaluation_method);
            return revaluation;
        }

        public Revaluation SaveRevaluationVoid(RevaluationCancelModel TObject, string ConnectionString)
        {
            LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, RevaluationLoggingValidation.Execution_of_void_save);
            LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, RevaluationLoggingValidation.Account_period_checking);
            Revaluation revaluation = _revaluationService.GetRevalForVoid(TObject.Id, TObject.CompanyId);
            if (revaluation != null)
            {
                //Data concurrency verify
                //string timeStamp = "0x" + string.Concat(Array.ConvertAll(revaluation.Version, x => x.ToString("X2")));
                //if (!timeStamp.Equals(TObject.Version))
                //    throw new Exception(RevaluationConstant.Document_has_been_modified_outside);

                //if (!_masterService.ValidateYearEndLockDate(TObject.RevalDate, TObject.CompanyId))
                //    throw new Exception(RevaluationConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                //if (!_masterService.RevaluationPeriodLuck(revaluation.RevalutionDate.Value.AddDays(1), revaluation.CompanyId))
                //    throw new Exception(RevaluationValidation.Date_should_be_in_accounting_period_to_run_the_revaluation);
                LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, RevaluationLoggingValidation.End_of_the_Functionality_validation);

                #region Delete_Previous_Void_Baseed_on_Date
                //using (con = new SqlConnection(ConnectionString))
                //{
                //    if (con.State != ConnectionState.Open)
                //        con.Open();
                //    cmd = new SqlCommand("DELETE_VOIDED_REVAL", con);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@RevalDate", revaluation.RevalutionDate);
                //    cmd.Parameters.AddWithValue("@ServiceEntityId", revaluation.ServiceCompanyId);
                //    cmd.Parameters.AddWithValue("@RevalId", revaluation.Id.ToString());
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //}
                #endregion Delete_Previous_Void_Baseed_on_Date
                revaluation.ModifiedBy = revaluation.UserCreated;
                revaluation.ModifiedDate = DateTime.UtcNow;
                revaluation.DocState = RevaluationConstant.Void;
                revaluation.ObjectState = ObjectState.Modified;
            }
            else
                throw new Exception(RevaluationConstant.Revaluation_already_void);
            RVModel rvm = new RVModel();
            rvm.Id = revaluation.Id;
            rvm.CompanyId = revaluation.CompanyId;
            //FillRevaluationPosting(rvm, revaluation, true);
            SaveRevaluationVoidJournalPosting(rvm);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(RevaluationLoggingValidation.RevaluationApplicationService, ex, ex.Message);
            }
            return revaluation;
        }
        #endregion

        #region Private_Block


        public decimal GetExRateInformation(string documentCurrency, string currency, DateTime Documentdate, long CompanyId)
        {
            decimal exchangeRate = 1;
            try
            {
                string date = Documentdate.ToString("yyyy-MM-dd");

                if (documentCurrency == currency)
                    exchangeRate = 1;
                else
                {
                    //new changes
                    //CommonForex commonForex = _commonForexService.GetForexyByDateAndCurrency(CompanyId, currency, documentCurrency, Convert.ToDateTime(Documentdate));
                    //if (commonForex != null)
                    //{
                    //    exchangeRate = commonForex.FromForexRate.Value;
                    //}
                    //else
                    //{
                    var url = "https://data.fixer.io/api/" + date + "?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + documentCurrency + "&symbols=" + currency;
                    CurrencyModel currencyRates = DownloadSerializedJSONData<CurrencyModel>(url);

                    exchangeRate = currencyRates.Rates.Where(c => c.Key == currency).Select(c => c.Value).FirstOrDefault();
                    FillCommonForexFrom(documentCurrency, Documentdate, exchangeRate, currency);

                    //}
                }
                return exchangeRate;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(RevaluationLoggingValidation.RevaluationApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private void FillCommonForexFrom(string DocumentCurrency, DateTime Documentdate, decimal? exchageRate, string BaseCurrency)
        {
            CommonForex commonForex = new CommonForex();
            commonForex.Id = Guid.NewGuid();
            commonForex.CompanyId = 0;
            commonForex.DateFrom = Convert.ToDateTime(Documentdate);
            commonForex.Dateto = commonForex.DateFrom;
            commonForex.FromForexRate = exchageRate;
            commonForex.ToForexRate = commonForex.FromForexRate;
            commonForex.FromCurrency = BaseCurrency;
            commonForex.ToCurrency = DocumentCurrency;
            commonForex.Status = RecordStatusEnum.Active;
            commonForex.Source = "Fixer";
            commonForex.UserCreated = "System";
            commonForex.CreatedDate = DateTime.UtcNow;
            commonForex.ObjectState = ObjectState.Added;
            _commonForexService.Insert(commonForex);
            _unitOfWorkAsync.SaveChanges();

        }

        public Dictionary<string, decimal> GetExRateInformation(string docCurr, DateTime? Documentdate, long CompanyId, string baseCurr)
        {
            Dictionary<string, decimal> exchangeRates = null;
            try
            {
                BeanForex forex = new BeanForex();
                string date = Documentdate.Value.ToString("yyyy-MM-dd");
                forex.DocumentDate = date;
                forex.Provider = "Fixer";
                var url = "https://data.fixer.io/api/" + date + "?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + docCurr + "&symbols=" + baseCurr;
                CurrencyModel currencyRates = DownloadSerializedJSONData<CurrencyModel>(url);
                //if (currencyRates.Base == null)
                //{
                //    BeanForex forex1 = new BeanForex();
                //    return forex1;
                //}

                //decimal sgdorusdValue;
                //var value = currencyRates.Rates.TryGetValue(BaseCurrency, out sgdorusdValue);
                /*forex.BaseUnitPerUSD*/
                //exchangeRates = currencyRates.Rates.Where(c => c.Key == docCurr).Select(c => new { DocCurr = c.Key, Rate = c.Value }).ToDictionary(docCur => docCur.DocCurr, ExRate => ExRate.Rate);
                exchangeRates = currencyRates.Rates.ToDictionary(curr => curr.Key, val => val.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exchangeRates;
        }
        private decimal GetExRateInformations(string docCurr, DateTime? Documentdate, long CompanyId, string baseCurr)
        {
            decimal exchangeRates = 0;
            try
            {
                BeanForex forex = new BeanForex();
                string date = Documentdate.Value.ToString("yyyy-MM-dd");
                forex.DocumentDate = date;
                forex.Provider = "Fixer";
                var url = "https://data.fixer.io/api/" + date + "?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + docCurr + "&symbols=" + baseCurr;
                CurrencyModel currencyRates = DownloadSerializedJSONData<CurrencyModel>(url);
                exchangeRates = currencyRates.Rates.Select(c => c.Value).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exchangeRates;
        }
        private static T DownloadSerializedJSONData<T>(string url) where T : new()
        {
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
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
                throw ex;
            }

            return DocNumber;
        }
        private string GetNextApplicationNumberForCancel(string sysNumber, bool isFirst, string originalSysNumber)
        {
            string DocNumber = "";
            try
            {
                int DocNo = 0;
                if (!isFirst)
                {
                    DocNo = Convert.ToInt32(sysNumber.Substring(sysNumber.LastIndexOf("-CL") + 3));
                }
                DocNo++;
                DocNumber = originalSysNumber + ("-CL" + DocNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DocNumber;
        }
        private void FillRevaluation(Revaluation revaluation, RevaluationSaveModel revaluationModel)
        {
            revaluation.CompanyId = revaluationModel.CompanyId;
            revaluation.RevalutionDate = revaluationModel.RevaluationDate;
            revaluation.DocState = revaluationModel.DocState;
            revaluation.ServiceCompanyId = revaluationModel.ServiceCompanyId;
            revaluation.IsMultiCurrency = revaluationModel.IsMultiCurrency;
            revaluation.IsNoSupportingDocument = revaluationModel.IsNoSupportingDocument;
            revaluation.DocState = revaluationModel.DocState;
            revaluation.NetAmount = Math.Round((decimal)revaluationModel.NetAmount, 2, MidpointRounding.AwayFromZero);
            revaluationModel.Status = revaluationModel.Status;
        }
        private void FillJVRevaluationDetail(JVVDetailModel detail, RevalutionDetail revalDetail, Revaluation Revaluation, bool? isReversal, decimal? gainLossAmount, string nature, long COAId, string docNo, bool isUnrealized)
        {
            detail.Id = Guid.NewGuid();
            detail.DocType = DocTypeConstant.Journal;
            detail.DocSubType = DocTypeConstant.Revaluation;
            detail.DocCurrency = revalDetail.DocCurrency;
            detail.DocumentId = Revaluation.Id;
            detail.DocDate = Revaluation.RevalutionDate;
            detail.ExchangeRate = Revaluation.ExchangeRate;
            detail.DocNo = docNo;
            detail.PostingDate = Revaluation.RevalutionDate;
            detail.COAId = COAId;
            detail.ServiceCompanyId = revalDetail.ServiceEntityId;
            detail.EntityId = revalDetail.EntityId;
            detail.RecOrder = revalDetail.RecOrder;
            detail.BaseCurrency = revalDetail.BaseCurrency;
            decimal? val = gainLossAmount;
            if (!isUnrealized)
            {
                if (isReversal == true /*&& state == "Posted" || state == "Parked"*/)
                {
                    if (val > 0/* && nature == "Debit"*/)
                    {
                        detail.DocCredit = val == null ? 0 : val.Value;
                        detail.BaseCredit = detail.DocCredit * 1;
                    }

                    else if (val < 0 /*&& nature == "Debit"*/)
                    {
                        detail.DocDebit = val == null ? 0 : -(val.Value);
                        detail.BaseDebit = detail.DocDebit * 1;
                    }
                }
                else
                {
                    if (val > 0)
                    {
                        detail.DocDebit = val == null ? 0 : val.Value;
                        detail.BaseDebit = detail.DocDebit * 1;
                    }
                    else if (val < 0)
                    {
                        detail.DocCredit = val == null ? 0 : -(val.Value);
                        detail.BaseCredit = detail.DocCredit * 1;
                    }
                }
            }
            else
            {
                if (isReversal == true /*&& state == "Posted" || state == "Parked"*/)
                {
                    //if (val > 0 && nature == "Debit")
                    //{
                    //    detail.DocCredit = val == null ? 0 : val.Value;
                    //    detail.BaseCredit = detail.DocCredit * 1;
                    //}
                    if (val > 0 && nature == "Credit")
                    {
                        detail.DocDebit = val == null ? 0 : val.Value;
                        detail.BaseDebit = detail.DocDebit * 1;
                    }
                    //else if (val < 0 && nature == "Debit")
                    //{
                    //    detail.DocDebit = val == null ? 0 : -(val.Value);
                    //    detail.BaseDebit = detail.DocDebit * 1;
                    //}
                    else if (val < 0 && nature == "Credit")
                    {
                        detail.DocCredit = val == null ? 0 : -(val.Value);
                        detail.BaseCredit = detail.DocCredit * 1;
                    }
                }
                else
                {
                    //if (val > 0 && nature == "Debit")
                    //{
                    //    detail.DocDebit = val == null ? 0 : val.Value;
                    //    detail.BaseDebit = detail.DocDebit * 1;
                    //}
                    if (val > 0 && nature == "Credit")
                    {
                        detail.DocCredit = val == null ? 0 : val.Value;
                        detail.BaseCredit = detail.DocCredit * 1;
                    }
                    //else if (val < 0 && nature == "Debit")
                    //{
                    //    detail.DocCredit = val == null ? 0 : -(val.Value);
                    //    detail.BaseCredit = detail.DocCredit * 1;
                    //}
                    else if (val < 0 && nature == "Credit")
                    {
                        detail.DocDebit = val == null ? 0 : -(val.Value);
                        detail.BaseDebit = detail.DocDebit * 1;
                    }
                }
            }
        }
        private void FillJVRevaluationDetail(JVVDetailModel detailNew, JVVDetailModel detailOld, JVModel journal)
        {
            detailNew.Id = Guid.NewGuid();
            detailNew.JournalId = journal.Id;
            detailNew.DocType = DocTypeConstant.Journal;
            detailNew.DocSubType = DocTypeConstant.Revaluation;
            detailNew.DocCurrency = detailOld.DocCurrency;
            detailNew.ExchangeRate = Convert.ToDecimal(1.0000000000);
            detailNew.BaseCurrency = detailOld.BaseCurrency;
            detailNew.DocDate = journal.DocDate;
            detailNew.PostingDate = journal.PostingDate;
            detailNew.DocDebit = detailOld.DocCredit;
            detailNew.DocCredit = detailOld.DocDebit;
            detailNew.BaseDebit = detailOld.BaseCredit;
            detailNew.BaseCredit = detailOld.BaseDebit;
            detailNew.COAId = detailOld.COAId;
            detailNew.EntityId = detailOld.EntityId;
            detailNew.RecOrder = detailOld.RecOrder;
        }

        private void RevaluationDetailModelToEntity(RevalutionDetail revalDetail, RevaluationModel detailModel)
        {
            revalDetail.Id = Guid.NewGuid();
            revalDetail.COAId = detailModel.COAId;
            revalDetail.CreatedDate = DateTime.UtcNow;
            revalDetail.DocCurrency = detailModel.DocCurrency;
            revalDetail.DocumentDate = detailModel.DocDate;
            revalDetail.DocumentType = detailModel.DocType;
            revalDetail.DocumentNumber = detailModel.DocNo;
            revalDetail.EntityId = detailModel.EntityId;
            revalDetail.ServiceEntityId = detailModel.ServiceCompanyId;
            revalDetail.IsChecked = detailModel.IsChecked;
            revalDetail.BaseCurrencyAmount1 = detailModel.BaseBal;
            revalDetail.BaseCurrencyAmount2 = detailModel.RevalBal;
            revalDetail.IsBankData = detailModel.IsBankData;
            revalDetail.ExchangerateNew = detailModel.RevalExchangeRate;
            revalDetail.ExchangerateOld = detailModel.OrgExchangeRate;
            revalDetail.RecOrder = detailModel.RecOrder;
            revalDetail.DocId = detailModel.DocumentId;
            revalDetail.BaseCurrency = detailModel.BaseCurrency;
            //revalDetail.BaseCurrency=detailModel.b
            revalDetail.UnrealisedExchangegainorlose = detailModel.UnrealisedExchangegainorlose;
            revalDetail.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            revalDetail.DocBal = detailModel.DocBal;
        }
        #endregion Private_Block

        #region Auto_Number_Block

        public string GenerateAutoNumberForType(long companyId, string Type, string companyCode, bool isFirst)
        {
            string generatedAutoNumber = "";
            try
            {
                double incrmtValue = 0;
                AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(companyId, Type);
                incrmtValue =/* isFirst == false ? Convert.ToInt32(_autoNo.GeneratedNumber) + 1 : */Convert.ToInt32(_autoNo.GeneratedNumber);
                generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength),
                    incrmtValue, companyId, companyCode);
                if (_autoNo != null)
                {
                    _autoNo.GeneratedNumber = value;
                    _autoNo.IsDisable = true;
                    _autoNo.ObjectState = ObjectState.Modified;
                    _autoNumberService.Update(_autoNo);
                }
                AutoNumberCompanyCompact _autoNumberCompanyNew = _autoNumberService.GetAutoCompany(_autoNo.Id);
                if (_autoNumberCompanyNew != null)
                {
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                    //_autoNumberCompanyService.Update(_autoNumberCompanyNew);
                }
                else
                {
                    _autoNumberCompanyNew = new AutoNumberCompanyCompact();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.Id = Guid.NewGuid();
                    _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                    //_autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(RevaluationLoggingValidation.RevaluationApplicationService, ex, ex.Message);
                throw ex;
            }
            return generatedAutoNumber;
        }
        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, double IncreamentVal,
            long companyId, string Companycode = null)
        {
            List<JournalCompact> lstRev = null;
            int? currentMonth = 0;
            bool ifMonthContains = false;
            string OutputNumber = "";
            try
            {
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
                lstRev = _revaluationService.GetAllJournalByCompanyId(companyId);
                if (lstRev.Any() && ifMonthContains == true)
                {
                    int? lastCretedDate = lstRev.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                    if (DateTime.Now.Year == lstRev.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                    {
                        if (lastCretedDate == currentMonth)
                        {
                            foreach (var reval in lstRev)
                            {
                                if (reval.DocNo != _autoNumberService.GetAutoNumberPreview(companyId, Type))
                                    val = IncreamentVal;
                                else
                                {
                                    val = IncreamentVal + 1;
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

                else if (lstRev.Any() && ifMonthContains == false)
                {
                    foreach (var op in lstRev)
                    {
                        if (op.DocNo != _autoNumberService.GetAutoNumberPreview(companyId, Type))
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

                if (lstRev.Any())
                {
                    OutputNumber = GetNewNumber(lstRev, Type, OutputNumber, counter, companyFormatHere, counterLength);
                }

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(RevaluationLoggingValidation.RevaluationApplicationService, ex, ex.Message);
                throw ex;
            }
            return OutputNumber;
        }
        private string GetNewNumber(List<JournalCompact> lstReval, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstReval.Where(a => a.DocNo == outputNumber).FirstOrDefault();
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
                    var inv = lstReval.Where(c => c.DocNo == val2).FirstOrDefault();
                    if (inv == null)
                        isNotexist = true;
                }
                val1 = val2;
            }
            return val1;
        }
        #endregion Auto_Number_Block

        #region Posting_Block
        private void FillRevaluationPosting(JVModel jvmodel, Revaluation Revaluation, bool isNew, bool? isReversal, string ConnectionString)
        {
            FillJournal(jvmodel, Revaluation, isReversal, ConnectionString);

            #region Auto_Number_Updation_For_Reversal
            //if (isReversal == true)
            //    using (con = new SqlConnection(ConnectionString))
            //    {
            //        string query = $"Update Common.AutoNumber set GeneratedNumber= {value} where CompanyId={Revaluation.CompanyId} and  EntityType='Journal'";
            //        if (con.State != ConnectionState.Open)
            //            con.Open();
            //        cmd = new SqlCommand(query, con);
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
            #endregion Auto_Number_Updation_For_Reversal

            List<JVVDetailModel> lstDetails = new List<JVVDetailModel>();
            JVVDetailModel detail = new JVVDetailModel();
            List<ChartOfAccountCompact> lstCOAs = _masterService.GetAllRevalAccount(Revaluation.CompanyId);
            Dictionary<long, string> unrealisedaccount = _masterService.GetCOAByName(Revaluation.CompanyId, COAConstants.Exchange_gain_loss_Unrealised);
            List<ChartOfAccountCompact> lstRevalCOA = lstCOAs.Where(d => Revaluation.RevalutionDetails.Select(c => c.COAId).Contains(d.Id)).ToList();
            decimal? resValue = 0;
            //foreach (RevalutionDetail revalDetail in Revaluation.RevalutionDetails.Where(c => c.IsBankData == true).OrderBy(c => c.RecOrder).GroupBy(d => d.IsBankData).ToList())
            //{
            if (Revaluation.RevalutionDetails.Any())
            {
                //For All line items with bank account
                List<RevalutionDetail> lstBankRecord = Revaluation.RevalutionDetails.Where(c => c.IsBankData == true && c.IsChecked == true).OrderBy(c => c.RecOrder)/*.GroupBy(d => d.IsBankData).Select(c => c.FirstOrDefault())*/.ToList();
                if (lstBankRecord.Any())
                {
                    var accTypeId = lstRevalCOA.Where(c => c.Id == lstBankRecord.Select(d => d.COAId).FirstOrDefault()).Select(c => new { AccountTypeId = c.AccountTypeId, Nature = c.Nature }).FirstOrDefault();
                    detail = new JVVDetailModel();
                    FillJVRevaluationDetail(detail, lstBankRecord.FirstOrDefault(), Revaluation, isReversal, lstBankRecord.Sum(c => c.UnrealisedExchangegainorlose), accTypeId.Nature, lstCOAs.Where(c => c.AccountTypeId == accTypeId.AccountTypeId && c.IsRevaluation == 1).Select(d => d.Id).FirstOrDefault(), jvmodel.DocNo, false);
                    detail.PostingDate = jvmodel.PostingDate;
                    detail.AccountDescription = jvmodel.DocumentDescription;
                    resValue = (detail.DocDebit != 0 && detail.DocDebit != null) ? detail.DocDebit : detail.DocCredit;
                    if (resValue != 0)
                        lstDetails.Add(detail);
                }
                //  }
                //For All line items without bank account
                foreach (var revalDetail in Revaluation.RevalutionDetails.Where(c => c.IsBankData != true && c.IsChecked == true).OrderBy(c => c.RecOrder).GroupBy(d => d.COAId).ToList())
                {
                    var accTypeId = lstRevalCOA.Where(c => c.Id == revalDetail.Select(d => d.COAId).FirstOrDefault()).Select(c => new { AccountTypeId = c.AccountTypeId, Nature = c.Nature }).FirstOrDefault();
                    detail = new JVVDetailModel();
                    FillJVRevaluationDetail(detail, revalDetail.Select(c => c).FirstOrDefault(), Revaluation, isReversal, revalDetail.Sum(c => c.UnrealisedExchangegainorlose), accTypeId.Nature, lstCOAs.Where(c => c.AccountTypeId == accTypeId.AccountTypeId && c.IsRevaluation == 1).Select(d => d.Id).FirstOrDefault(), jvmodel.DocNo, false);
                    detail.AccountDescription = jvmodel.DocumentDescription;
                    detail.PostingDate = jvmodel.PostingDate;
                    resValue = (detail.DocDebit != 0 && detail.DocDebit != null) ? detail.DocDebit : detail.DocCredit;
                    if (resValue != 0)
                        lstDetails.Add(detail);
                }
                //For Exchange gain/loss - Unrealised account
                detail = new JVVDetailModel();
                FillJVRevaluationDetail(detail, Revaluation.RevalutionDetails.FirstOrDefault(), Revaluation, isReversal, Revaluation.NetAmount, unrealisedaccount.Values.FirstOrDefault(), unrealisedaccount.Keys.FirstOrDefault(), jvmodel.DocNo, true);
                detail.AccountDescription = jvmodel.DocumentDescription;
                detail.PostingDate = jvmodel.PostingDate;
                resValue = (detail.DocDebit != 0 && detail.DocDebit != null) ? detail.DocDebit : detail.DocCredit;
                if (resValue != 0)
                    lstDetails.Add(detail);
            }
            jvmodel.GrandBaseCreditTotal = Math.Round((decimal)lstDetails.Sum(c => c.BaseCredit), 2);
            jvmodel.GrandBaseDebitTotal = Math.Round((decimal)lstDetails.Sum(c => c.BaseDebit), 2);
            jvmodel.GrandDocCreditTotal = Math.Round((decimal)lstDetails.Sum(c => c.DocCredit), 2);
            jvmodel.GrandDocDebitTotal = Math.Round((decimal)lstDetails.Sum(c => c.DocDebit), 2);
            jvmodel.BaseCurrency = detail.BaseCurrency;
            jvmodel.JVVDetailModels = lstDetails.OrderBy(c => c.RecOrder).ToList();
        }
        private void FillJournal(JVModel jvm, Revaluation Revaluation, bool? isReversal, string connectionString)
        {
            jvm.Id = Guid.NewGuid();
            jvm.CompanyId = Revaluation.CompanyId;
            jvm.DocumentId = Revaluation.Id;
            jvm.ServiceCompanyId = Revaluation.ServiceCompanyId;
            jvm.IsMultiCurrency = Revaluation.IsMultiCurrency;
            jvm.IsNoSupportingDocs = Revaluation.IsNoSupportingDocument;
            jvm.Status = RecordStatusEnum.Active;
            jvm.DocType = DocTypeConstant.Journal;
            jvm.DocSubType = DocTypeConstant.Revaluation;
            jvm.UserCreated = Revaluation.UserCreated;
            jvm.ExchangeRate = 1;
            jvm.CreatedDate = DateTime.UtcNow;
            jvm.DocDate = isReversal == true ? Revaluation.RevalutionDate.Value.AddDays(1) : Revaluation.RevalutionDate;
            //jvm.PostingDate = Revaluation.RevalutionDate.Value;
            jvm.PostingDate = jvm.DocDate.Value;
            jvm.DueDate = (DateTime?)null;
            jvm.ReversalDate = Revaluation.RevalutionDate.Value.AddDays(1);
            jvm.DocumentState = isReversal == true ? "Reversed" : "Posted";
            jvm.IsAutoReversalJournal = jvm.DocumentState == "Reversed" ? true : false;
            jvm.DocumentDescription = DocTypeConstant.Revaluation + "-" + Revaluation.RevalutionDate.Value.ToString("dd/MM/yyyy");
            jvm.DocNo = isReversal != true ? Revaluation.SystemRefNo : _autoNumberService.GetAutonumber(Revaluation.CompanyId, DocTypeConstants.JournalVocher, connectionString);
            jvm.SystemReferenceNo = jvm.DocNo;
        }
        //private void FillJournal1(JVModel jvmNew, JVModel jvmOld, Revaluation Revaluation, bool revaluationPeriodDate, DateTime? finalcialLuckDate)
        //{
        //    jvmNew.Id = Guid.NewGuid();
        //    jvmNew.CompanyId = Revaluation.CompanyId;
        //    jvmNew.DocumentId = Revaluation.Id;
        //    jvmNew.ServiceCompanyId = Revaluation.ServiceCompanyId;
        //    jvmNew.IsMultiCurrency = Revaluation.IsMultiCurrency;
        //    jvmNew.IsNoSupportingDocs = Revaluation.IsNoSupportingDocument;
        //    jvmNew.ReverseParentRefId = jvmOld.ReverseParentRefId;
        //    jvmNew.Status = RecordStatusEnum.Active;
        //    jvmNew.DocType = DocTypeConstant.Journal;
        //    jvmNew.DocSubType = DocTypeConstant.Revaluation;
        //    jvmNew.UserCreated = Revaluation.UserCreated;
        //    jvmNew.ExchangeRate = 1;
        //    jvmNew.CreatedDate = Revaluation.CreatedDate;
        //    jvmNew.BaseCurrency = jvmOld.BaseCurrency;
        //    jvmNew.DocCurrency = jvmOld.DocCurrency;//changed by lokanath
        //    jvmNew.DocDate = Revaluation.RevalutionDate.Value.AddDays(1);
        //    jvmNew.PostingDate = Revaluation.RevalutionDate.Value.AddDays(1);
        //    jvmNew.ReversalDate = Revaluation.RevalutionDate.Value.AddDays(1);
        //    if (finalcialLuckDate == null)
        //        jvmNew.DocumentState = "Posted";
        //    else
        //        jvmNew.DocumentState = revaluationPeriodDate == true ? "Posted" : "Parked";
        //    jvmNew.IsAutoReversalJournal = (jvmNew.DocumentState == "Reversed" || jvmNew.DocumentState == "Parked") ? true : false;
        //    jvmNew.DocumentDescription = jvmNew.DocumentState == "Reversed" ? DocTypeConstant.Revaluation + "-" + jvmNew.DocDate.Value.ToString("dd/MM/yyyy") : DocTypeConstant.Revaluation + "-" + jvmNew.DocDate.Value.ToString("dd/MM/yyyy") + "(" + "Reversal" + ")";
        //    jvmNew.DocNo = jvmNew.DocumentState == "Reversed" ? DocTypeConstant.Revaluation + "-" + jvmNew.DocDate.Value.ToString("dd/MM/yyyy") : DocTypeConstant.Revaluation + "-" + jvmNew.DocDate.Value.ToString("dd/MM/yyyy") + "-R";
        //}
        public void SaveRevaluationPosting(JVModel jvModel)
        {
            var json = RestHelper.ConvertObjectToJason(jvModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == RevaluationConstant.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = jvModel;
                //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestHelper.ZPost(url, "api/journal/saveposting", json);
                if (response.ErrorMessage != null)
                {
                    //LoggingHelper.LogError(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(RevaluationLoggingValidation.RevaluationApplicationService, ex, ex.Message);
                string message = ex.Message;
            }
        }

        private void SaveRevaluationVoidJournalPosting(RVModel rvModel)
        {
            var json = RestHelper.ConvertObjectToJason(rvModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == RevaluationConstant.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = rvModel;
                //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestHelper.ZPost(url, "api/journal/saverevaluationjournaleverse", json);
                if (response.ErrorMessage != null)
                {
                    //LoggingHelper.LogError(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(RevaluationLoggingValidation.RevaluationApplicationService, ex, ex.Message);
                string message = ex.Message;
            }
        }
        //private void FillReversalRevaluationPosting(JVModel oldJvm, Revaluation Revaluation, bool revaluationPeriodDate, DateTime? finalcialLuckDate)
        //{

        //    JVModel newJvm = new JVModel();
        //    FillJournal1(newJvm, oldJvm, Revaluation, revaluationPeriodDate, finalcialLuckDate);
        //    List<JVVDetailModel> lstJvModel = new List<JVVDetailModel>();
        //    foreach (var jdetail in oldJvm.JVVDetailModels)
        //    {
        //        JVVDetailModel detailModel = new JVVDetailModel();
        //        FillJVRevaluationDetail(detailModel, jdetail, oldJvm);
        //        detailModel.ServiceCompanyId = newJvm.ServiceCompanyId.Value;
        //        lstJvModel.Add(detailModel);
        //    }
        //    newJvm.JVVDetailModels = lstJvModel;
        //    newJvm.SystemReferenceNo = GetNextApplicationNumber(doc, false, Revaluation.SystemRefNo);
        //    doc = newJvm.SystemReferenceNo;
        //    newJvm.GrandBaseCreditTotal = Math.Round((decimal)lstJvModel.Sum(c => c.BaseCredit), 2);
        //    newJvm.GrandBaseDebitTotal = Math.Round((decimal)lstJvModel.Sum(c => c.BaseDebit), 2);
        //    newJvm.GrandDocCreditTotal = Math.Round((decimal)lstJvModel.Sum(c => c.DocCredit), 2);
        //    newJvm.GrandDocDebitTotal = Math.Round((decimal)lstJvModel.Sum(c => c.DocDebit), 2);
        //    SaveRevaluationPosting(newJvm);
        //}

        #endregion Posting_Block

        #region Get_Manual_Posting
        public string GetManualPosting(long companyId, Guid id, string ConnectionString)
        {
            try
            {
                Revaluation revaluation = _revaluationService.GetAllRevaluationAndDetail(id, companyId);
                if (revaluation != null)
                {
                    JVModel jvm = new JVModel();
                    int count = 1;
                    while (count <= 2)
                    {
                        FillRevaluationPosting(jvm, revaluation, true, count == 2, ConnectionString);
                        SaveRevaluationPosting(jvm);
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return "Successfully completed!!";
        }
        #endregion Get_Manual_Posting

    }
}
