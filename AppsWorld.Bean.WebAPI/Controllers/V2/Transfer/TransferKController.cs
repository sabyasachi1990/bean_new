﻿using AppsWorld.BankTransferModule.Application.V2;
using AppsWorld.Bean.WebAPI.Utils;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web.Http;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers.V2
{
    [RoutePrefix("api/v2/transfer")]
    public class TransferKController : BaseController
    {
        readonly TransferKApplicationService _transferApplicationService;
        public TransferKController(TransferKApplicationService transferApplicationService)
        {
            this._transferApplicationService = transferApplicationService;
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("banktransferk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult banktransferk(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
         );
                return Ok( _transferApplicationService.GetAllBankTransferK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}