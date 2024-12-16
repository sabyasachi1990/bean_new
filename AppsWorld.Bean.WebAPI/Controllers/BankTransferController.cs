using AppsWorld.BankTransferModule.Application;
using AppsWorld.BankTransferModule.Entities;
using AppsWorld.BankTransferModule.Models;
using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.CommonModule.Models;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/transfer")]
    public class BankTransferController : BaseController
    {
        BankTransferApplicationService _bankTransferApplicationService;
        //AuthInformation authInfo = GetAuthInfo(System.Web.HttpContext.Current.Request.Headers);

        //private static AuthInformation GetAuthInfo(NameValueCollection headers)
        //{
        //    return headers["AuthInformation"] != null ? JsonConvert.DeserializeObject<AuthInformation>(headers.GetValues("AuthInformation").FirstOrDefault().ToString()) : new Controllers.AuthInformation();
        //}

        public BankTransferController(BankTransferApplicationService bankTransferApplicationService)
        {
            this._bankTransferApplicationService = bankTransferApplicationService;
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("banktransferk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true,IsUsernameRequired =true)]
         [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> banktransferk(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                    requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var data = await  _bankTransferApplicationService.GetAllBankTransferK(AuthInformation.userName, AuthInformation.companyId.Value);
                return Ok(data.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        //[AllowAnonymous]
        [Route("createbankransfer")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
         [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateBankTransfer(Guid id)
        {
            try
            {
                BankTransferModel model = _bankTransferApplicationService.CreateBankTransfer(id, AuthInformation.companyId.Value,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection,AuthInformation.userName);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallbanktransferlu")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true, IsUsernameRequired = true)]
         [CommonHeaders(Position = 1)]    public IHttpActionResult GetAllBankTransferLU(Guid banktransferId)
        {
            try
            {
                BankTransferLU model = _bankTransferApplicationService.GetAllBankTransferLU(banktransferId, AuthInformation.companyId.Value, AuthInformation.userName, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savebanktransfer")]
        ////[CacheInvalidate(Position =11,Keys = "banktransferk")]
        ////[CacheInvalidate(Position =11,Keys = "CreateBankTransfer")]
        ////[CacheInvalidate(Position =11,Keys = "GetAllBankTransferLU")]

        //[CacheInvalidate(Position =11,Keys = "banktransferk,CreateBankTransfer,GetAllBankTransferLU")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveBankTransfer(BankTransferModel TObject)
        {
            try
            {
                TObject.CompanyId=AuthInformation.companyId.Value;                
                BankTransfer bankTransfer = _bankTransferApplicationService.SaveBankTransfer(TObject,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(bankTransfer);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(BankTransferModule.Infra.Resources.BankTransferLoggingValidation.BankTransferController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savebanktransferdocumentvoid")]
        ////[CacheInvalidate(Position =11,Keys = "banktransferk")]
        ////[CacheInvalidate(Position =11,Keys = "CreateBankTransfer")]
        ////[CacheInvalidate(Position =11,Keys = "GetAllBankTransferLU")]

        //[CacheInvalidate(Position =11,Keys = "banktransferk,CreateBankTransfer,GetAllBankTransferLU")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveWithdrawalDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId=AuthInformation.companyId.Value;
                BankTransfer model = _bankTransferApplicationService.SaveBankTransferDocumentVoid(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #region Intercobilling
        [HttpGet]
        [Route("getsettlementdetails")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetSettlementDetails(Guid transferId, string docCurrency, DateTime transferDate)
        {
            try
            {
                //Dictionary<string, string> dictonary = new Dictionary<string, string>();
                //AuthInformation.Values.Add("WithdrawalServiceEntityId", "810");
                //AuthInformation.Values.Add("DepositServiceEntityId", "809");
                return Ok(_bankTransferApplicationService.GetListOfSettlementDetails(AuthInformation.companyId.Value, Convert.ToInt64(AuthInformation.Values.Where(a => a.Key == "WithdrawalServiceEntityId").Select(a => a.Value).FirstOrDefault()), Convert.ToInt64(AuthInformation.Values.Where(a => a.Key == "DepositServiceEntityId").Select(a => a.Value).FirstOrDefault()), transferId, transferDate, docCurrency));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
