using System.Collections.Generic;
using System.Web.Http;
using AppsWorld.JournalVoucherModule.Application;
using AppsWorld.JournalVoucherModule.Models;
using AppsWorld.JournalVoucherModule.Entities;
using System;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Linq;
using System.Net.Http;
using Kendo.DynamicLinq;
using AppsWorld.JournalVoucherModule.Model;
using Newtonsoft.Json;
using WebApi.RedisCache.V2;
using System.Collections.Specialized;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using System.Threading.Tasks;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/journal")]
    public class JournalVoucherController : BaseController
    {
        JournalApplicationService _journalApplicationService;
        //AuthInformation authInfo = GetAuthInfo(System.Web.HttpContext.Current.Request.Headers);

        //private static AuthInformation GetAuthInfo(NameValueCollection headers)
        //{
        //    return headers["AuthInformation"] != null ? JsonConvert.DeserializeObject<AuthInformation>(headers.GetValues("AuthInformation").FirstOrDefault().ToString()) : new Controllers.AuthInformation();
        //}
        public JournalVoucherController(JournalApplicationService journalApplicationService)
        {
            this._journalApplicationService = journalApplicationService;
        }

        #region Grid Calls
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallparkeds")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllParkeds(ODataQueryOptions<JournalModel> options, int pageSize)
        {
            try
            {
                ODataQuerySettings settings = new ODataQuerySettings()
                {
                    PageSize = pageSize
                };

                IQueryable results = options.ApplyTo(_journalApplicationService.GetAllPrakeds(AuthInformation.companyId.Value).AsQueryable(), settings);

                Uri uri = Request.GetNextPageLink();
                long? inlineCount = Request.GetInlineCount();

                PageResult<JournalModel> response = new PageResult<JournalModel>(
                    results as IEnumerable<JournalModel>,
                    uri,
                    inlineCount);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallposted")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllPosteds(ODataQueryOptions<JournalModel> options, int pageSize)
        {
            try
            {
                ODataQuerySettings settings = new ODataQuerySettings()
                {
                    PageSize = pageSize
                };

                IQueryable results = options.ApplyTo(_journalApplicationService.GetAllPosteds(AuthInformation.companyId.Value).AsQueryable(), settings);

                Uri uri = Request.GetNextPageLink();
                long? inlineCount = Request.GetInlineCount();

                PageResult<JournalModel> response = new PageResult<JournalModel>(
                    results as IEnumerable<JournalModel>,
                    uri,
                    inlineCount);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallvoids")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllVoids(ODataQueryOptions<JournalModel> options, int pageSize)
        {
            try
            {
                ODataQuerySettings settings = new ODataQuerySettings()
                {
                    PageSize = pageSize
                };

                IQueryable results = options.ApplyTo(_journalApplicationService.GetAllVoids(AuthInformation.companyId.Value).AsQueryable(), settings);

                Uri uri = Request.GetNextPageLink();
                long? inlineCount = Request.GetInlineCount();

                PageResult<JournalModel> response = new PageResult<JournalModel>(
                    results as IEnumerable<JournalModel>,
                    uri,
                    inlineCount);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallautoreversals")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllAutoReversal(ODataQueryOptions<JournalModel> options, int pageSize)
        {
            try
            {
                ODataQuerySettings settings = new ODataQuerySettings()
                {
                    PageSize = pageSize
                };

                IQueryable results = options.ApplyTo(_journalApplicationService.GetAllAutoReversals(AuthInformation.companyId.Value).AsQueryable(), settings);

                Uri uri = Request.GetNextPageLink();
                long? inlineCount = Request.GetInlineCount();

                PageResult<JournalModel> response = new PageResult<JournalModel>(
                    results as IEnumerable<JournalModel>,
                    uri,
                    inlineCount);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        //[AllowAnonymous]
        [Route("getallrecurrings")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllrecurrings(ODataQueryOptions<JournalModel> options, int pageSize)
        {
            try
            {
                ODataQuerySettings settings = new ODataQuerySettings()
                {
                    PageSize = pageSize
                };

                IQueryable results = options.ApplyTo(_journalApplicationService.GetAllRecurrings(AuthInformation.companyId.Value).AsQueryable(), settings);

                Uri uri = Request.GetNextPageLink();
                long? inlineCount = Request.GetInlineCount();

                PageResult<JournalModel> response = new PageResult<JournalModel>(
                    results as IEnumerable<JournalModel>,
                    uri,
                    inlineCount);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getalljv")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllJournals()
        {
            try
            {
                List<Journal> model = _journalApplicationService.GetAllJournals();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Create Call and lookup call
        [HttpGet]
        //[AllowAnonymous]
        [Route("createjournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> CreateJournalL(Guid id, string documentState)
        {
            try
            {
                JournalModel model = await _journalApplicationService.CreateJournal(id, AuthInformation.companyId.Value, documentState, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        //[AllowAnonymous]
        [Route("journallu")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllJournalLU(Guid journalId, string docSubType = null)
        {
            try
            {
                return Ok(await  _journalApplicationService.GetAllJournalLUs(journalId, AuthInformation.companyId.Value, AuthInformation.userName, docSubType));
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createjournaldetail")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetByDetailId(Guid Id, Guid journalId)
        {
            try
            {
                return Ok(_journalApplicationService.GetByDetailId(Id, journalId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getfrequencylus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetFrequencyLUs(Guid journalId)
        {
            try
            {
                RecurringLU model = _journalApplicationService.GetFrequencyLUs(journalId, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createJournalvoid")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateJournalVoid(Guid id)
        {
            try
            {
                DocumentVoidModel model = _journalApplicationService.CreateJournalVoid(id, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Save Calls

        [HttpPost]
        //[AllowAnonymous]
        [Route("savejournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournals")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkeds")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPosteds")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllVoids")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllAutoReversal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllrecurrings")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRecurringsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetCashSaleJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDocumentJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetRecieptJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetWithdrawalJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetClearingJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetMemoApplicationJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetBankTransferJournal")]

        //[CacheInvalidate(Position = 11, Keys = "CreateClearingDetail", Controller = "ClearingController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateRevaluation", Controller = "RevaluationController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearingDetail", Controller = "ClearingController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBankreconciliation", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetClearingTransactionK", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetClearingRec", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetClearingTransaction", Controller = "BankReconciliationController")]

        //[CacheInvalidate(Position = 11, Keys = "journallu,CreateJournalVoid,GetByDetailId,CreateJournalL,GetAllJournals,GetAllParkeds,GetAllPosteds,GetAllVoids,GetAllAutoReversal,GetAllrecurrings,GetAllPostedsK,GetAllRecurringsK,GetAllParkedsK,GetInvoiceJournal,GetCashSaleJournal,GetDocumentJournal,GetRecieptJournal,GetWithdrawalJournal,GetClearingJournal,GetMemoApplicationJournal,GetBankTransferJournal")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult Savejournal(JournalModel journalObject)
        {
            try
            {
                journalObject.CompanyId = AuthInformation.companyId.Value;
                journalObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                Journal model = _journalApplicationService.SaveJournal(journalObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(journalObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("JournalController", ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savereversal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournals")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkeds")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPosteds")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllVoids")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllAutoReversal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllrecurrings")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRecurringsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetCashSaleJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDocumentJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetRecieptJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetWithdrawalJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetClearingJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetMemoApplicationJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetBankTransferJournal")]

        //[CacheInvalidate(Position = 11, Keys = "journallu,CreateJournalVoid,GetByDetailId,CreateJournalL,GetAllJournals,GetAllParkeds,GetAllPosteds,GetAllVoids,GetAllAutoReversal,GetAllrecurrings,GetAllPostedsK,GetAllRecurringsK,GetAllParkedsK,GetInvoiceJournal,GetCashSaleJournal,GetDocumentJournal,GetRecieptJournal,GetWithdrawalJournal,GetClearingJournal,GetMemoApplicationJournal,GetBankTransferJournal")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveReversal(JournalSaveModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                Journal model = _journalApplicationService.SaveReversal(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("savepostcallforparked")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournals")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkeds")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPosteds")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllVoids")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllAutoReversal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllrecurrings")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRecurringsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetCashSaleJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDocumentJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetRecieptJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetWithdrawalJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetClearingJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetMemoApplicationJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetBankTransferJournal")]

        //[CacheInvalidate(Position = 11, Keys = "journallu,CreateJournalVoid,GetByDetailId,CreateJournalL,GetAllJournals,GetAllParkeds,GetAllPosteds,GetAllVoids,GetAllAutoReversal,GetAllrecurrings,GetAllPostedsK,GetAllRecurringsK,GetAllParkedsK,GetInvoiceJournal,GetCashSaleJournal,GetDocumentJournal,GetRecieptJournal,GetWithdrawalJournal,GetClearingJournal,GetMemoApplicationJournal,GetBankTransferJournal")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SavePostCallforParked(JournalSaveModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                Journal model = _journalApplicationService.SavePostCallforParked(TObject);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savejournalvoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllVoidsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkedsK")]

        //[CacheInvalidate(Position = 11, Keys = "journallu,GetByDetailId,CreateJournalL,GetAllVoidsK,GetAllPostedsK,GetAllParkedsK")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveJournalVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                Journal model = _journalApplicationService.SaveJournalVoid(TObject);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savejournalcopy")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRecurringsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkedsK")]

        //[CacheInvalidate(Position = 11, Keys = "journallu,GetByDetailId,CreateJournalL,GetAllVoidsK,GetAllPostedsK,GetAllParkedsK")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveJournalCopy(JournalSaveModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                JournalModel model = _journalApplicationService.SaveCopy(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Delete call
        [HttpPost]
        //[AllowAnonymous]
        [Route("deletejv")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournals")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllVoidsK")]

        //[CacheInvalidate(Position = 11, Keys = "CreateJournalVoid,GetByDetailId,CreateJournalL,GetAllJournals,GetAllVoidsK")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult DeleteJv(Guid id)
        {
            try
            {
                string msg = _journalApplicationService.DeleteJournal(id, AuthInformation.companyId.Value);
                return Ok(msg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region KendoGrid
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallpostedsK")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllPostedsK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));

                var model = await _journalApplicationService.NewGetAllPostedJournalsK(AuthInformation.companyId.Value, AuthInformation.userName);
                return Ok(model.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        //[AllowAnonymous]
        [Route("getallparkedsK")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllParkedsK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var model = _journalApplicationService.GetAllParkedsK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        //[AllowAnonymous]
        [Route("getallrecurringsK")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllRecurringsK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var model = _journalApplicationService.GetAllRecurringsK(AuthInformation.companyId.Value, AuthInformation.userName).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallvoidsK")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllVoidsK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var model = _journalApplicationService.GetAllVoidedJournalK(AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getallpostedrecurringsK")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllPostedJournalK(HttpRequestMessage requestMessage, Guid? recurringJournalId)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
            var model = _journalApplicationService.GetAllPostedJournal(AuthInformation.companyId.Value, recurringJournalId).OrderByDescending(a => a.CreatedDate).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
            return Ok(model);
        }
        #endregion

        #region Posting calls
        [HttpPost]
        //[AllowAnonymous]
        [Route("saveposting")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournals")]
        //////[CacheInvalidate(Position =11,Keys = "GetAllParkeds")]
        //////[CacheInvalidate(Position =11,Keys = "GetAllPosteds")]
        //////[CacheInvalidate(Position =11,Keys = "GetAllVoids")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllAutoReversal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllrecurrings")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRecurringsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetCashSaleJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDocumentJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetRecieptJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetWithdrawalJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetClearingJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetMemoApplicationJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetBankTransferJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetRevaluationJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetOpeningBalanceJournal")]

        //[CacheInvalidate(Position = 11, Keys = "CreateRevaluation", Controller = "RevaluationController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearingDetail", Controller = "ClearingController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBankreconciliation", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetClearingTransactionK", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetClearingRec", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetClearingTransaction", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetCurrencyLookup", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 11, Keys = "GetCurrencyLookup", Controller = "PaymentController")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU,CreateJournalVoid,GetByDetailId,CreateJournalL,GetAllJournals,GetAllParkeds,GetAllPosteds,GetAllVoids,GetAllAutoReversal,GetAllrecurrings,GetAllPostedsK,GetAllRecurringsK,GetAllParkedsK,GetInvoiceJournal,GetCashSaleJournal,GetDocumentJournal,GetRecieptJournal,GetWithdrawalJournal,GetClearingJournal,GetMemoApplicationJournal,GetBankTransferJournal,GetRevaluationJournal,GetOpeningBalanceJournal")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SavePosting(JVVModel journalModel)
        {


            try
            {
                //journalModel.CompanyId = AuthInformation.companyId.Value;
                //journalModel.ServiceCompanyId = journalModel.DocType == "Transfer" ? journalModel.ServiceCompanyId : Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                Journal journal = _journalApplicationService.SavePosting(journalModel, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(journal);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(journalModel));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("JournalController", ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }


        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("deletevoidposting")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRecurringsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllVoidsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU")]

        //[CacheInvalidate(Position = 11, Keys = "CreateRevaluation", Controller = "RevaluationController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearingDetail", Controller = "ClearingController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBankreconciliation", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetClearingTransactionK", Controller = "BankReconciliationController")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK,GetAllRecurringsK,GetAllParkedsK,CreateJournalL,GetByDetailId,GetAllVoidsK,GetAllJournalLU")]
        //[CommonHeaders(Position = 1)]
        public void DeleteVoidPosting(JournalSaveModel tObject)
        {
            _journalApplicationService.DeletePostVoid(tObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("deletepayrollvoidposting")]
        [CommonHeaders(Position = 1)]
        public void DeletePayrollVoidPosting(JournalSaveModel tObject)
        {
            tObject.CompanyId = AuthInformation.companyId.Value;
            _journalApplicationService.DeletePayrollPostVoid(tObject);
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getinvoicejournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetInvoiceJournal(Guid id, string systemRefNo, string type)
        {
            try
            {
                JVModel model = _journalApplicationService.GetInvoiceJournal(id, systemRefNo, AuthInformation.companyId.Value, type);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        //[AllowAnonymous]
        [Route("getcashsalejournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetCashSaleJournal(Guid id, string systemRefNo, string type)
        {
            try
            {
                DocumentModel model = _journalApplicationService.GetCashSaleJournal(id, systemRefNo, AuthInformation.companyId.Value, type);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getdocumentjournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetDocumentJournal(Guid id, string systemRefNo, string type)
        {
            try
            {
                DocumentModel model = _journalApplicationService.GetDocumentJournal(id, systemRefNo, AuthInformation.companyId.Value, type);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getrecieptjournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetRecieptJournal(Guid id, string systemRefNo, string type)
        {
            try
            {
                DocumentModel model = _journalApplicationService.GetRecieptJournal(id, systemRefNo, AuthInformation.companyId.Value, type);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getwithdrawaljournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetWithdrawalJournal(Guid id, string systemRefNo, string type)
        {
            try
            {
                DocumentModel model = _journalApplicationService.GetWithdrawalJournal(id, systemRefNo, AuthInformation.companyId.Value, type);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getclearingjournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetClearingJournal(Guid id, string systemRefNo, string type)
        {
            try
            {
                DocumentModel model = _journalApplicationService.GetClearingJournal(id, systemRefNo, AuthInformation.companyId.Value, type);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getmemoapplicationjournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetMemoApplicationJournal(Guid id, string systemRefNo, string type)
        {
            try
            {
                DocumentModel model = _journalApplicationService.GetMemoApplicationJournal(id, systemRefNo, AuthInformation.companyId.Value, type);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getdoubtfuldebtjournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetDoubtfuldebtJournal(Guid id, string systemRefNo, string type)
        {
            try
            {
                DocumentModel model = _journalApplicationService.GetDoubtfuldebtJournal(id, systemRefNo, AuthInformation.companyId.Value, type);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getopeningbalancejournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetOpeningBalanceJournal(Guid id)
        {
            try
            {
                DocumentModel model = _journalApplicationService.GetOpeningBalanceJournal(id, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("saverevaluationjournaleverse")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetRevaluationJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllVoidsK")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU,GetByDetailId,CreateJournalL,GetRevaluationJournal,GetAllVoidsK")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult saveRevaluationJournalReverse(RVModel revaluation)
        {
            try
            {
                //revaluation.CompanyId = AuthInformation.companyId.Value;
                string model = _journalApplicationService.GetJournalReverse(revaluation);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        [HttpPost]
        //[AllowAnonymous]
        [Route("updateposting")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDocumentJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetRecieptJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetMemoApplicationJournal")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU,GetByDetailId,CreateJournalL,GetInvoiceJournal,GetDocumentJournal,GetRecieptJournal,GetMemoApplicationJournal")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult UpdatePosting(UpdatePosting up)
        {
            try
            {
                up.CompanyId = AuthInformation.companyId.Value;
                return Ok(_journalApplicationService.UpdatePosting(up));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getbanktransferjournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetBankTransferJournal(Guid id, string systemRefNo, string type)
        {
            try
            {
                BankTransferModel model = _journalApplicationService.GetBankTransferJournal(id, systemRefNo, AuthInformation.companyId.Value, type);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getrevaluationjournal")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetRevaluationJournal(Guid id)
        {
            try
            {
                DocumentModel model = _journalApplicationService.GetAllRevaluation(id, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Recurring Journal
        [HttpGet]
        [Route("getrecurringjournallist")]
        // //[Cache(Position =10,ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetRecurringJournalList(Guid id, DateTime docDate, DateTime? endDate, string docNo, int frequencyValue)
        {
            try
            {
                return Ok(_journalApplicationService.GetDocDateAndDocNo(id, AuthInformation.companyId.Value, docDate, endDate, docNo, frequencyValue));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("saverecurringjournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetByDetailId")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateJournalL")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournals")]
        //////[CacheInvalidate(Position =11,Keys = "GetAllParkeds")]
        //////[CacheInvalidate(Position =11,Keys = "GetAllPosteds")]
        //////[CacheInvalidate(Position =11,Keys = "GetAllVoids")]
        //////[CacheInvalidate(Position =11,Keys = "GetAllAutoReversal")]
        //////[CacheInvalidate(Position =11,Keys = "GetAllrecurrings")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRecurringsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkedsK")]
        //////[CacheInvalidate(Position =11,Keys = "GetInvoiceJournal")]
        //////[CacheInvalidate(Position =11,Keys = "GetCashSaleJournal")]
        //////[CacheInvalidate(Position =11,Keys = "GetDocumentJournal")]
        //////[CacheInvalidate(Position =11,Keys = "GetRecieptJournal")]
        //////[CacheInvalidate(Position =11,Keys = "GetWithdrawalJournal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetClearingJournal")]
        //////[CacheInvalidate(Position =11,Keys = "GetMemoApplicationJournal")]
        //////[CacheInvalidate(Position =11,Keys = "GetBankTransferJournal")]

        //[CacheInvalidate(Position = 11, Keys = "CreateClearingDetail", Controller = "ClearingController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateRevaluation", Controller = "RevaluationController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearingDetail", Controller = "ClearingController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBankreconciliation", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetClearingTransactionK", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetClearingRec", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetClearingTransaction", Controller = "BankReconciliationController")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU,CreateJournalVoid,GetByDetailId,CreateJournalL,GetAllJournals,GetAllParkeds,GetAllPosteds,GetAllVoids,GetAllAutoReversal,GetAllrecurrings,GetAllPostedsK,GetAllRecurringsK,GetAllParkedsK,GetInvoiceJournal,GetCashSaleJournal,GetDocumentJournal,GetRecieptJournal,GetWithdrawalJournal,GetClearingJournal,GetMemoApplicationJournal,GetBankTransferJournal")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveRecurringJournal(JournalModel journalModel)
        {
            try
            {
                journalModel.CompanyId = AuthInformation.companyId.Value;
                journalModel.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(_journalApplicationService.SaveRecurring(journalModel, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(journalModel));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("JournalController", ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        #endregion Recurring Journal

        #region Bean Financials


        [HttpGet]
        [Route("resetfinancialreport")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetTrailBalance(string screenName)
        {
            try
            {
                long companyId = AuthInformation.companyId.Value;
                return Ok(_journalApplicationService.ResetFinancialReport(companyId, screenName, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Route("gettrailbalance")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetTrailBalance(CommonModel commonModel)
        {
            try
            {
                commonModel.CompanyId = AuthInformation.companyId.Value;
                return Ok(_journalApplicationService.GetTrailBalance(commonModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("getincomestatement")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetIncomeStatement(CommonModel commonModel)
        {
            try
            {
                commonModel.CompanyId = AuthInformation.companyId.Value;
                return Ok(_journalApplicationService.GetIncomeStatementByCompanyId(commonModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("getbalancesheet")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetBalanceSheet(CommonModel commonModel)
        {
            try
            {
                commonModel.CompanyId = AuthInformation.companyId.Value;
                return Ok(_journalApplicationService.GetBalanceSheet(commonModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("createcategory")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCategory()
        {
            try
            {
                return Ok(new CategoryModel());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("savecategory")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCategory(CategoryModel categoryModel)
        {
            try
            {
                categoryModel.CompanyId = AuthInformation.companyId.Value;
                return Ok(_journalApplicationService.SaveCategory(categoryModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("savesubtotals")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveSubTotals(LeadSheetTotalModel lstleadSheetTotalModel)
        {
            try
            {
                return Ok(_journalApplicationService.SaveSubTotals(lstleadSheetTotalModel, AuthInformation.companyId.Value));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("saveincomestatement")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveIncomeStatements(IncomeStatementModel leadSheetModels)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(_journalApplicationService.SaveIncomeStatementss(leadSheetModels, AuthInformation.companyId.Value));
                }
                return BadRequest(string.Join(",", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("savebalancesheet")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveBalanceSheetCheck(FinalBalanceSheetModel lstleadSheetTotalModels)
        {
            try
            {
                return Ok(_journalApplicationService.SaveBalanceSheetsCheck(lstleadSheetTotalModels, AuthInformation.companyId.Value));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("getallcompanies")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllCompany()
        {
            try
            {
                //return Ok(_journalApplicationService.GetAllLookUp(AuthInformation.companyId.Value, AuthInformation.userName, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
                return Ok(_journalApplicationService.GetAllLookUp(AuthInformation.companyId.Value, AuthInformation.userName, Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys["SecondaryDbConnection"].ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }






        #endregion Bean Financials





        [HttpPost]
        [Route("getincomestatementK")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetIncomeStatementK(CommonModel commonModel)
        {
            try
            {
                commonModel.CompanyId = AuthInformation.companyId.Value;
                return Ok(_journalApplicationService.GetIncomeStatementByCompanyIdK(commonModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("getbalancesheetK")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult getbalancesheetK(CommonModel commonModel)
        {
            try
            {
                commonModel.CompanyId = AuthInformation.companyId.Value;
                return Ok(_journalApplicationService.GetBalanceSheetKTest(commonModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetDeletedAuditTrail")]
        // //[Cache(Position =10,ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetDeletedAuditTrail(Guid journalId)
        {
            try
            {
                return Ok(_journalApplicationService.GetDeletedAuditTrail(journalId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #region common_jvpopupview_call
        [HttpGet]
        [Route("getjournalpopupview")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetJournalPopUpView(Guid documentId, string docType, Guid? externalId)
        {
            try
            {
                return Ok(_journalApplicationService.GetJvPopupView(AuthInformation.companyId.Value, documentId, docType, externalId, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, AuthInformation.userName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        [HttpPost]
        [Route("beanhtmljournallistingk")]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> Bean_HTMLJournalListingK(HtmlJlParamsVm htmlJlParamsVm)
        {
            try
            {
                htmlJlParamsVm.CompanyId = AuthInformation.companyId.Value;
                return Ok(await Task.Run(() => _journalApplicationService.Bean_HTMLJournalListingK(htmlJlParamsVm, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("journallistingparameters")]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> JournalListingparameters()
        {
            try
            {

                return Ok(await Task.Run(() => _journalApplicationService.JournalListingparameters(AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, AuthInformation.userName)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("journallistingdoctypeparameters")]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> JournalListingDocparameters(string doctrype)
        {
            try
            {

                return Ok(await Task.Run(()=> _journalApplicationService.JournalListingDocparameters(AuthInformation.companyId.Value, doctrype, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }




        #region GST_Analysis_Account_Type_call
        [HttpGet]
        [Route("getcoabyaccounttype")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetCoaByAccounttype(string accounttype)
        {
            try
            {
                return Ok(_journalApplicationService.GetCoabyAccountType(AuthInformation.companyId.Value, accounttype, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion





        #region NewBeanFinancials

        #region Commented V2 Services

        //[HttpPost]
        //[Route("getnewincomestatement")]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetNewIncomestatement(CommonModel commonModel)
        //{
        //    try
        //    {
        //        commonModel.CompanyId = AuthInformation.companyId.Value;
        //        return Ok(Task.Run(() => _journalApplicationService.GetNewIncomestatement(commonModel)).Result);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("getnewbalancesheet")]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetNewBalanceSheet(CommonModel commonModel)
        //{
        //    try
        //    {
        //        commonModel.CompanyId = AuthInformation.companyId.Value;
        //        return Ok(_journalApplicationService.GetNewBalanceSheet(commonModel));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
        #endregion




        #endregion  NewBeanFinancials


    }
}

