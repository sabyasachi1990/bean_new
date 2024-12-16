using System.Collections.Generic;
using System.Web.Http;
using System;
using Newtonsoft.Json;
using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.Infra;
using AppsWorld.GLAccountModule.Application;
using AppsWorld.GLAccountModule.Models;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using System.Linq;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/glaccount")]
    public class GLAccountController : BaseController
    {
        private readonly GLApplicationService _GLApplicationService;
        public GLAccountController(GLApplicationService GLApplicationService)
        {
            this._GLApplicationService = GLApplicationService;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallglaccountlus")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllGLAccountLUs(long companyId)
        {
            try
            {
                var subCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                GlAccountLUs model = _GLApplicationService.GetAccountLUs(companyId, subCompanyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("createclearing")]
        //[CommonHeaders(Position = 1)]   public IHttpActionResult CreateClearing(Guid id, long companyId)
        //{
        //    try
        //    {
        //        ClearingModel model = _ClearingApplicationService.CreateClearing(id, companyId);
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("createclearingdetail")]
        //[CommonHeaders(Position = 1)]   public IHttpActionResult CreateClearingDetail(DateTime date, long coaId, long serviceCompanyId)
        //{
        //    try
        //    {
        //        List<ClearingDetailModel> model = _ClearingApplicationService.CreateClearingDetail(date, coaId, serviceCompanyId);
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

    }
}
