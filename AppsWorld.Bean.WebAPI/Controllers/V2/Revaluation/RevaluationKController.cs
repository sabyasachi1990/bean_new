using System.Web.Http;
using System;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using AppsWorld.RevaluationModule.Application.V2;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/v2/revaluation")]
    public class RevaluationKController : BaseController
    {
        readonly RevaluationKApplicationService _revaluationApplicationService;
        public RevaluationKController(RevaluationKApplicationService revaluationApplicationService)
        {
            this._revaluationApplicationService = revaluationApplicationService;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallrevaluationk")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllRevaluationK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
               requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );
                return Ok(_revaluationApplicationService.GetAllRevaluationK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

