using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.ReminderModule.Application;
using AppsWorld.ReminderModule.Models.Models;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers.V2.Reminder
{
    [RoutePrefix("api/Reminder")]
    public class ReminderController : BaseController
    {
        #region Constructor
        private readonly ReminderApplicationService _reminderApplicationService;

        public ReminderController(ReminderApplicationService reminderApplicationService)
        {
            this._reminderApplicationService = reminderApplicationService;
        }
        #endregion
        [HttpGet]
        [Route("getallremindersK")]
        [Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        [RolePermission(Position = 3)]
        public async Task<IHttpActionResult> GetAllReminderskNew(HttpRequestMessage requestMessage, DateTime? fromDate, DateTime? toDate, string type, string Name)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));

                return Ok(( await _reminderApplicationService.GetAllReminderskNew(AuthInformation.companyId.Value, fromDate, toDate, type, Name)).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("reminderlookup")]
        [Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult ReminderLookp()
        {
            try
            {
                return Ok(_reminderApplicationService.ReminderLookp(AuthInformation.companyId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPreview")]
        [Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetPreview(Guid id)
        {
            try
            {
                var email = AuthInformation.userName;
                return Ok(_reminderApplicationService.GetPreview(id, AuthInformation.companyId.Value, email));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("Remindersendemail")]
        [CommonHeaders(Position = 1)]
        //[RolePermission(Position = 3)]
        public IHttpActionResult SaveInvoiceEmail(ReminderMailModel emailmodel)
        {
            try
            {
                var email = AuthInformation.Values.FirstOrDefault(t => t.Key == "Email").Value;
                emailmodel.UserName = AuthInformation.userName;
                return Ok(_reminderApplicationService.ReminderSendEmail(emailmodel, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, Ziraff.FrameWork.SingleTon.CommonConnection.AzureStorage, email, AuthInformation.companyId.Value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route("dismissreminderbatch")]
        //[CacheInvalidate(Position = 10, Keys = "getreminderbatch")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult DismissReminderBatch(ReminderMailModel dismisremindermodel)
        {
            try
            {
                return Ok(_reminderApplicationService.DismissReminderBatch(dismisremindermodel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
