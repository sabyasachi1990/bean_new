using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.ReportsModule.Application;
using AppsWorld.ReportsModule.Application.V3;
using AppsWorld.ReportsModule.Models;
using AppsWorld.ReportsModule.Models.V3;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers
{
	[RoutePrefix("api/htmlreports")]
	[CommonHeaders(Position = 1)]
	public class ReportsController : BaseController
	{
		private readonly ReportsApplicationService _reportsService;

        private readonly ReportsReadOnlyApplicationService _ReportsReadOnlyApplicationService;

        HttpActionContext actionContex;
		public ReportsController(ReportsApplicationService reportsService, ReportsReadOnlyApplicationService ReportsReadOnlyApplicationService)
		{
			actionContex = new HttpActionContext();
			this._reportsService = reportsService;
			this._ReportsReadOnlyApplicationService= ReportsReadOnlyApplicationService;
		}

		//ReportsApplicationService _reportsService = new ReportsApplicationService();

		[HttpPost]
		//[AllowAnonymous]
		[Route("getcutomerandvendoraging")]
		//[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 4500)]
		public IHttpActionResult GetCutomerAndVendorAging(CustomerViewModel customerViewMode)
		{
			try
			{
				//return Ok(await Task.Run(() => _reportsService.GetCutomerAndVendorAging(customerViewMode, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)));

                return Ok( _ReportsReadOnlyApplicationService.GetCutomerAndVendorAging(customerViewMode, Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys["SecondaryDbConnection"].ToString()));
            }
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpGet]
		//[AllowAnonymous]
		[Route("getallcustvenlusdata")]
		//[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 4500)]
		public IHttpActionResult GetAllCustVenLusData(long? tenantId, bool? isCustomer)
		{
			try
			{
				//return Ok(await Task.Run(()=>_reportsService.CustVenAgingLU(tenantId, isCustomer, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, AuthInformation.userName)));

                return Ok( _ReportsReadOnlyApplicationService.CustVenAgingLU(tenantId, isCustomer, AuthInformation.userName, Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys["SecondaryDbConnection"].ToString()));

            }
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}


		[HttpPost]
		//[AllowAnonymous]
		[Route("getcutomerandvendoraginguncheckinv")]
		//[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 4500)]
		//[CommonHeaders(Position = 1)]
		public IHttpActionResult GetCutomerAndVendorAgingUnCheckInv(CustomerViewModel customerViewMode)
		{
			try
			{
				//return Ok(_reportsService.GetCutomerAndVendorAgingUnCheckInv(customerViewMode, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
				return Ok(_ReportsReadOnlyApplicationService.GetCutomerAndVendorAgingUnCheckInv(customerViewMode, Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys["SecondaryDbConnection"].ToString()));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpGet]
		//[AllowAnonymous]
		[Route("getgeneralledgerLu")]
		//[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 4500)]
		public IHttpActionResult GetGeneralLedgerLu(long companyId)
		{
			try
			{
				//return Ok(Task.Run(()=>_reportsService.GetGeneralLedgerLu(companyId, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, AuthInformation.userName)).Result);
                return Ok( _ReportsReadOnlyApplicationService.GetGeneralLedgerLu(companyId, AuthInformation.userName, Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys["SecondaryDbConnection"].ToString()));
            }
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost]
		//[AllowAnonymous]
		[Route("getgeneralledger")]
		//[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 4500)]
		public IHttpActionResult GetGeneralLedger(GeneralLedgerViewModelNew generalLedgerViewModel)
		{
			try
			{
                //return Ok(Task.Run(()=>_reportsService.GetGeneralLedger(generalLedgerViewModel, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)).Result);

                return Ok( _ReportsReadOnlyApplicationService.GetGeneralLedger(generalLedgerViewModel, Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys["SecondaryDbConnection"].ToString()));
            }
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}


		[HttpGet]
		//[AllowAnonymous]
		[Route("getgstfirstlevel")]
		//[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 4500)]
		public async Task<IHttpActionResult> GetGSTFirstLevel(long companyId, DateTime? fromDate, DateTime? toDate, long serviceCompanyId)
		{
			try
			{
				return Ok(await _reportsService.GetGSTFirstLevel(companyId, fromDate, toDate, serviceCompanyId, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}


		[HttpGet]
		[Route("getGSTOutputTaxK")]
		public IHttpActionResult GetGSTOutputK(HttpRequestMessage requestMessage, long companyId, DateTime? fromDate, DateTime? toDate, string serviceCompany, string GSTNumber)
		{
			try
			{
				DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
								requestMessage.RequestUri.ParseQueryString().GetKey(0)
					  );
				var model = _reportsService.GetGSTOutputK(companyId, fromDate, toDate, serviceCompany, GSTNumber, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
				return Ok(model);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("getGSTInputTaxK")]
		public IHttpActionResult getGSTInputTaxK(HttpRequestMessage requestMessage, long companyId, DateTime? fromDate, DateTime? toDate, string serviceCompany, string GSTNumber)
		{
			try
			{
				DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
								requestMessage.RequestUri.ParseQueryString().GetKey(0)
					  );
				var model = _reportsService.getGSTInputTaxK(companyId, fromDate, toDate, serviceCompany, GSTNumber, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
				return Ok(model);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		//[AllowAnonymous]
		[Route("getfinancialyear")]
		public IHttpActionResult GetFinancialYear(long companyId, DateTime toDate)
		{
			try
			{
				//return Ok(_reportsService.GetFinancialYear(companyId, toDate, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
                return Ok(_reportsService.GetFinancialYear(companyId, toDate, Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys["SecondaryDbConnection"].ToString()));
            }
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpGet]
		[Route("getgstsecondlevel")]
		public IHttpActionResult GetGStSecondLevel(HttpRequestMessage requestMessage, long companyId, DateTime? fromDate, DateTime? toDate, long serviceCompanyId, string GSTNumber, string lstOfTaxCodes, string taxType)
		{
			try
			{
				DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
								requestMessage.RequestUri.ParseQueryString().GetKey(0)
					  );
				var model = Task.Run(()=>_reportsService.GetGSTSecondLevel(companyId, fromDate, toDate, serviceCompanyId, GSTNumber, lstOfTaxCodes, taxType, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter)).Result;
				return Ok(model);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		//[AllowAnonymous]
		[Route("getfinancialLu")]
		public IHttpActionResult GetFinancialLu(long companyId, DateTime? toDate)
		{
			try
			{
				//return Ok(await Task.Run(()=> _reportsService.GetFinancialLu(companyId, toDate, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)));

                return Ok( _ReportsReadOnlyApplicationService.GetFinancialLu(companyId, toDate, Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys["SecondaryDbConnection"].ToString()));
            }
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

    }
}
