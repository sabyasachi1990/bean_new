using System.Web.Http;
using ElasticClientWrapper;
using System;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/elasticsearch")]
    public class ElasticSearchController : BaseController
    {

        private readonly IESOperations _esOperations;

        public ElasticSearchController()
        {
            _esOperations = new ESOperations();
        }

        public ElasticSearchController(IESOperations esOperations)
        {
            _esOperations = esOperations;
        }

        [HttpGet]
        //[AllowAnonymous]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SmartSearch(string searchCriteria, string context)
        {
            try
            {
                object model = _esOperations.SmartSearch(searchCriteria, context);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GlobalSearch(string searchCriteria, int startIndex, int noOfRecords)
        {
            try
            {
                object model = _esOperations.GlobalSearch(searchCriteria, startIndex, noOfRecords);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("autocomplete")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult AutoComplete(string indexName, string input, string indexField, int noOfRecords)
        {
            try
            {
                object model = _esOperations.AutoComplete(indexName, input, indexField, noOfRecords);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
