using System.Collections.Generic;
using System.Web.Http;
using AppsWorld.CommonModule.Application;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Models;
using System;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Linq;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/common")]
    public class CommonController : BaseController
    {
        CommonApplicationService _CommonApplicationService;
        public CommonController(CommonApplicationService CommonApplicationService)
        {
            this._CommonApplicationService = CommonApplicationService;
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("generateautonumberfortype")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GenerateAutoNumberForTypes(AutoNumberViewModel autoNumberViewModel)
        {
            try
            {
                autoNumberViewModel.CompanyId = AuthInformation.companyId.Value;
                string autonumeber = _CommonApplicationService.GenerateAutoNumberForType(autoNumberViewModel);
                return Ok(autonumeber);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("getallglaccounts")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllGLAccounts(GLAccountM accounts)
        {
            try
            {
                accounts.CompanyId = AuthInformation.companyId.Value.ToString();
                List<RepotringModel> lstReport = _CommonApplicationService.GetGLAccounts(accounts, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection



);
                return Ok(lstReport);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("limituserpermissionbyse")]
        //[CommonHeaders(Position = 1)]   public IHttpActionResult LimitUserPermissionBySe(string username, long companyId)
        //{
        //    try
        //    {
        //        var lstsre = _CommonApplicationService.LimitUserPermissionBySe(username, companyId);
        //        return Ok(lstsre);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpGet]
        [Route("getnextsequencenumber")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetNextSequenceNo(long companyId, string entityType)
        {
            try
            {
                //accounts.CompanyId = AuthInformation.companyId.Value.ToString();
                return Ok(_CommonApplicationService.GetNextSequenceNo(companyId, entityType, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getdocumenthistory")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetDocumentHistory(string docType, Guid id)
        {
            try
            {
                //accounts.CompanyId = AuthInformation.companyId.Value.ToString();

                return Ok(_CommonApplicationService.GetAllDocumentHistory(AuthInformation.companyId.Value, docType, id, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // Practice 
        [HttpPost]
        //[AllowAnonymous]
        [Route("savecommoneditdata")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCommonEditData(CommonDocUpdateModel model)
        {
            try
            {
                model.CompanyId = AuthInformation.companyId;
                return Ok(_CommonApplicationService.SaveCommonEditData(model, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("savecommonlockeddata")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCommonLockedData(CommonLockModel model)
        {
            try
            {
                model.CompanyId = AuthInformation.companyId.Value;
                return Ok(_CommonApplicationService.SaveCommonLockedCall(model, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("SaveCommonDeletedata")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCommonDeleteData(CommonDeleteModel model)
        {
            try
            {
                model.CompanyId = AuthInformation.companyId.Value;
                return Ok(_CommonApplicationService.DeleteTransaction(model, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}