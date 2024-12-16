using System.Web.Http;
using System;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using AppsWorld.RevaluationModule.Application.V2;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using System.Linq;
using AppsWorld.RevaluationModule.Models;
using System.Collections.Generic;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/v2/revaluation")]
    public class RevaluationMainController : BaseController
    {
        readonly RevaluationApplicationService _revaluationApplicationService;
        public RevaluationMainController(RevaluationApplicationService revaluationApplicationService)
        {
            this._revaluationApplicationService = revaluationApplicationService;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createrevaluation")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateRevaluation(DateTime dateTime, Guid id)
        {
            try
            {
                return Ok(_revaluationApplicationService.CreateRevaluation(dateTime, AuthInformation.companyId.Value, id, AuthInformation.Values.FirstOrDefault(c => c.Key == "ServiceCompanyId").Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("revaluationlus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetRevaluationLookup(Guid id)
        {
            try
            {
                return Ok(_revaluationApplicationService.RevaluationLUs(AuthInformation.companyId.Value, id, AuthInformation.userName, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createrevaluationmodel")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateRevaluationModel()
        {
            try
            {
                return Ok(_revaluationApplicationService.CreateRevaluationModel(AuthInformation.companyId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("saverevaluation")]
        //[CacheInvalidate(Position = 11, Keys = "getallrevaluationk,createrevaluation,revaluationlus,createrevaluationmodel")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveRevalution(RevaluationSaveModel revaluationModel)
        {
            try
            {
                revaluationModel.CompanyId = AuthInformation.companyId.Value;
                revaluationModel.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(_revaluationApplicationService.SaveRevaluation(revaluationModel, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(revaluationModel));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("RevaluationController", ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savevoidrevaluation")]
        //[CacheInvalidate(Position = 11, Keys = "getallrevaluationk,createrevaluation,revaluationlus,createrevaluationmodel")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCancelRevalution(RevaluationCancelModel revCancelModel)
        {
            try
            {
                revCancelModel.CompanyId = AuthInformation.companyId.Value;
                return Ok(_revaluationApplicationService.SaveRevaluationVoid(revCancelModel, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getrevaluationposting")]
        public string GetRevaluationPosting(Guid id, long companyId)
        {
            string model = string.Empty;
            try
            {
                model = _revaluationApplicationService.GetManualPosting(companyId, id, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
            }
            return model;
        }
    }
}

