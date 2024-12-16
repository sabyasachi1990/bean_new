using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.CommonModule.Application;
using AppsWorld.DebitNoteModule.Application.V2;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web.Http;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers.V2
{
    [RoutePrefix("api/v2/debitnote")]
    public class DebitNoteKController : BaseController
    {
        DebitNoteKApplicationService _debitNoteKApplicationService;
        public DebitNoteKController(DebitNoteKApplicationService debiNoteKApplicationService)
        {
            this._debitNoteKApplicationService = debiNoteKApplicationService;
        }

        #region DebitNote

        [HttpGet]
        //[AllowAnonymous]
        [Route("getalldebitnotek")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllInvoicesK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                return Ok(_debitNoteKApplicationService.GetAllDebitNotesK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


    }
}
