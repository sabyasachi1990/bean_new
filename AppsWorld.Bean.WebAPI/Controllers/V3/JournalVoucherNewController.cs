using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.JournalVoucherModule.Application;
using AppsWorld.JournalVoucherModule.Application.V3;
using AppsWorld.JournalVoucherModule.Models.V3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers.V3
{
    [RoutePrefix("api/V3/JournalVoucherNew")]
    public class JournalVoucherNewController : BaseController
    {
        JournalApplicationServiceV3 _journalApplicationServiceV3;
        public JournalVoucherNewController(JournalApplicationServiceV3 journalApplicationServiceV3)
        {
            this._journalApplicationServiceV3= journalApplicationServiceV3;
        }
        [HttpPost]
        [Route("getnewincomestatement")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetNewIncomestatement(CommonModel commonModel)
        {
            try
            {
                commonModel.CompanyId = AuthInformation.companyId.Value;
                return Ok( _journalApplicationServiceV3.GetNewIncomestatement(commonModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("getnewbalancesheet")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetNewBalanceSheet(CommonModel commonModel)
        {
            try
            {
                commonModel.CompanyId = AuthInformation.companyId.Value;
                return Ok(_journalApplicationServiceV3.GetNewBalanceSheet(commonModel));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
