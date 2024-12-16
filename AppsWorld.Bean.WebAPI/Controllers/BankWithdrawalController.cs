using System.Collections.Generic;
using System.Web.Http;
using AppsWorld.BankWithdrawalModule.Application;
using AppsWorld.BankWithdrawalModule.Entities;
using AppsWorld.BankWithdrawalModule.Models;
using System;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Linq;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.Infra;
using WebApi.RedisCache.V2;
using System.Collections.Specialized;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using System.Threading.Tasks;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/bankwithdrawal")]
    public class BankWithdrawalController : BaseController
    {
        BankWithdrawalApplicationService _BankWithdrawalApplicationService;
        public BankWithdrawalController(BankWithdrawalApplicationService BankWithdrawalApplicationService)
        {
            this._BankWithdrawalApplicationService = BankWithdrawalApplicationService;
        }
        [HttpGet]
        [Route("getwithdrawalslu")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetWithdrawalsLookups(Guid id, string type)

        {
            try
            {
                WithdrawalModelLU model = await Task.Run(()=> _BankWithdrawalApplicationService.GetAllWithdrawalLUs(id, AuthInformation.companyId.Value, type, AuthInformation.userName));
                return Ok(model);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("getentityslu")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetEntityLU(Guid id, string entityType, DateTime? docDate)
        {
            try
            {
                return Ok(await Task.Run(()=> _BankWithdrawalApplicationService.GetEntityLU(id, AuthInformation.companyId.Value, entityType, docDate)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("createwithdrawal")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> CreateWithdrawal(Guid withdrawalId, string docType, bool isCopy)
        {
            try
            {
                WithdrawalModel model =await Task.Run(()=> _BankWithdrawalApplicationService.CreateWithdrwal(withdrawalId, AuthInformation.companyId.Value, docType, isCopy,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savewithdrawal")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateWithdrawal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBankWithdrawal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDeposit")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCashPayments")]
        ////[CacheInvalidate(Position = 11, Keys = "GetWithdrawalsLookups")]
        ////[CacheInvalidate(Position = 11, Keys = "GetEntityLU")]

        //[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGSTStatus", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "GSTSettings", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "createwithdrawal,getallbankwithdrawalk,getalldepositk,getallcashpaymentsk,getwithdrawalslu,getentityslu")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult Save(WithdrawalModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                TObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                Withdrawal withdrawal = _BankWithdrawalApplicationService.SaveWithdrawal(TObject,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);

                return Ok(withdrawal);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(BankWithdrawalModule.Infra.WithdrawalLoggingValidation.BankWithdrawalController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallbankwithdrawalk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true, IsUsernameRequired = false)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllBankWithdrawalk(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
           );

                var model =  await _BankWithdrawalApplicationService.GetAllBankWithdrawalk(AuthInformation.userName, AuthInformation.companyId.Value);
                return Ok(model.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getalldepositk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true, IsUsernameRequired = false)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllDepositk(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
           );
                var model = await _BankWithdrawalApplicationService.GetAllDepositK(AuthInformation.userName, AuthInformation.companyId.Value);
                return Ok(model.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallcashpaymentsk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true, IsUsernameRequired = false)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllCashpaymentsk(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
           );
                var model = await  _BankWithdrawalApplicationService.GetAllCashPaymentK(AuthInformation.userName, AuthInformation.companyId.Value);
                return Ok(model.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savewithdrawaldocumentvoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateWithdrawal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBankWithdrawal")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDeposit")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCashPayments")]

        //[CacheInvalidate(Position = 11, Keys = "createwithdrawal,getallbankwithdrawalk,getalldepositk,getallcashpaymentsk")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveWithdrawalDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                Withdrawal model = _BankWithdrawalApplicationService.SaveWithdrawalDocumentVoid(TObject);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getwithdrawaldetail")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetWithdrawalDetail()
        {
            try
            {
                WithdrawalDetailModel model = _BankWithdrawalApplicationService.GetWithdrawalDetail();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}