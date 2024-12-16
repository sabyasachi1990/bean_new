
using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.TemplateModule.Application;
using AppsWorld.TemplateModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/template")]
    public class TemplateController : BaseController
    {
        TemplateApplicationService _templateApplicationService;
        public TemplateController(TemplateApplicationService templateApplicationService)
        {
            _templateApplicationService = templateApplicationService;
        }
        #region Templates
        [HttpPost]
        [Route("generatetemplates")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GenerateTemplates(TemplatesVM templateVM)
        {
            try
            {
                templateVM.CompanyId = AuthInformation.companyId.Value;
                return Ok(_templateApplicationService.GenerateTemplates(templateVM));
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        #endregion Templates

    }
}
