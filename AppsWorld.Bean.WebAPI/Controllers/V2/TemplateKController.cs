
using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.TemplateModule.Application;
using AppsWorld.TemplateModule.Application.V2;
using AppsWorld.TemplateModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/templates")]
    public class TemplateKController : BaseController
    { 
        TemplateCompactApplicationService _templateCompactApplicationService;
        public TemplateKController(TemplateCompactApplicationService templateCompactApplicationService)
        {
            _templateCompactApplicationService = templateCompactApplicationService;
        }

        #region Templates
        [HttpPost]
        [Route("generatetemplates")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GenerateTemplates(EmailModel templateVM)
        {
            try
            {
                //templateVM.CompanyId = AuthInformation.companyId.Value;
                return Ok(_templateCompactApplicationService.GenerateTemplates(templateVM, KeyVaultProperty.AzureStorage));
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("generatemultipletemplates")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GenerateMultipleTemplates(List<EmailModel> templateVM)
        {
            try
            {
                return Ok(_templateCompactApplicationService.GenerateMultipleTemplates(templateVM,  AuthInformation.userName));
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("getsentemail")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetSentEmail(EmailModel templateVM)
        {
            try
            {
                //templateVM.CompanyId = AuthInformation.companyId.Value;
                return Ok(_templateCompactApplicationService.GenerateTemplates(templateVM, KeyVaultProperty.AzureStorage));
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion Templates

    }
}
