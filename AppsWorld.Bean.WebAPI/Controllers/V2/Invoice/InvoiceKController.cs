using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.CommonModule.Application;
using AppsWorld.InvoiceModule.Application.V2;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web.Http;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers.V2
{
    [RoutePrefix("api/v2/invoice")]
    public class InvoiceKController : BaseController
    {
        private readonly InvoiceKApplicationService _invoiceKApplicationService;
        public InvoiceKController(InvoiceKApplicationService invoiceKApplicationService)
        {
            this._invoiceKApplicationService = invoiceKApplicationService;
        }

        #region Invoice

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallinvoicesk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllInvoicesK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var querData = _invoiceKApplicationService.GetAllInvoicesK(AuthInformation.userName, AuthInformation.companyId.Value);

                //var model = _commonAppService.LimitUserPermissionBySe(System.Web.HttpContext.Current.Request.Headers, companyId, querData).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                var model = querData.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallparkedinvoicek")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllParkedInvoiceK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
           );
                var model = _invoiceKApplicationService.GetAllParkedInvoicesK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallrecurringinvoicek")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllRecurringInvoiceK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
           );
                var model = _invoiceKApplicationService.GetAllRecurringInvoicesK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallpostedrecurringsK")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllPostedRecurring(HttpRequestMessage requestMessage, Guid id)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var model = _invoiceKApplicationService.GetAllRecurringPostedInvoicesK(AuthInformation.companyId.Value, id).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region CreditNote
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallcreditnoteK")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllCreditNoteK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
           );
                var model = _invoiceKApplicationService.GetAllCreditNoteK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion CreditNote

        #region DebitProvision
        [HttpGet]
        //[AllowAnonymous]
        [Route("getalldebitfulldebitK")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public DataSourceResult GetAllDebitfulldebitK(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
               requestMessage.RequestUri.ParseQueryString().GetKey(0));

            return _invoiceKApplicationService.GetAllDebitfulldebitK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);

        }
        #endregion DebitProvision
    }
}
