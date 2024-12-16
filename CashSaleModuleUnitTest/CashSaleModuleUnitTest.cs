using AppsWorld.CashSalesModule.Models;
using AppsWorld.Framework;
using CashSaleModuleUnitTest.CommonSource;
using FrameWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CashSaleModuleUnitTest
{
    //[TestClass]
    //public class CashSaleModuleUnitTest
    //{
    //    string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();


    //    #region Save Call


    //    [TestMethod]
    //    public void SaveCashSale()
    //    {
    //        CashSaleModel cashSaleModel = new CashSaleModel();
    //        cashSaleModel = FillCashSaleModel(cashSaleModel);
    //        var json = RestHelper.ConvertObjectToJason(cashSaleModel);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonContest.GetCashSaleUrl + "savecashsale", json);
    //        Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
    //    }


    //    [TestMethod]
    //    public void SaveCashSale_Negative_Test()
    //    {
    //        CashSaleModel cashSaleModel = new CashSaleModel();
    //        cashSaleModel = FillSaveCashSale_Negative_Test(cashSaleModel);
    //        var json = RestHelper.ConvertObjectToJason(cashSaleModel);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonContest.GetCashSaleUrl + "savecashsale", json);
    //        Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
    //    }

    //    private CashSaleModel FillSaveCashSale_Negative_Test(CashSaleModel cashSaleModel)
    //    {
    //        cashSaleModel.Id = Guid.NewGuid();
    //        //cashSaleModel.CompanyId = 4;
    //        cashSaleModel.DocSubType = "Doc-1";
    //        cashSaleModel.CashSaleNumber = "SYS-1";
    //        cashSaleModel.DocDate = Convert.ToDateTime("2016-09-23 05:30:00.000");
    //        cashSaleModel.DocNo = "Doc-123";
    //        cashSaleModel.ModeOfReceipt = "Cash";
    //        cashSaleModel.EntityType = "Customer";
    //        cashSaleModel.EntityId = Guid.Parse("84D08D81-50FD-9211-FD5E-0B9B05A81EEC");
    //        cashSaleModel.DocumentState = "Active";         
    //        cashSaleModel.GrandTotal = 52000;
    //        cashSaleModel.IsGstSettings = true;
    //        cashSaleModel.IsMultiCurrency = true;
    //        cashSaleModel.COAId = 11692;
    //        //cashSaleModel.IsRepeatingInvoice = true;
    //        cashSaleModel.IsSegmentReporting = true;
    //        cashSaleModel.IsAllowableNonAllowable = true;
    //        cashSaleModel.BaseCurrency = "UDS";
    //        cashSaleModel.BalanceAmount = 125500;
    //        cashSaleModel.ServiceCompanyId = 63;
    //        cashSaleModel.IsNoSupportingDocument = true;
    //        cashSaleModel.IsGSTApplied = false;
    //        cashSaleModel.DocCurrency = "USD";           
    //        cashSaleModel.Status = RecordStatusEnum.Active;
    //        cashSaleModel.CashSaleDetails = new List<CashSaleDetailModel> { new CashSaleDetailModel { Id = Guid.NewGuid(), CashSaleId = cashSaleModel.Id, COAId = 1, TaxId = 7, DocAmount = 120, DocTotalAmount = 500, BaseTaxAmount = 130} };
    //        //cashSaleModel.GSTDetailModels = new List<GSTDetailModel> { new GSTDetailModel { Id = Guid.NewGuid(), DocId = Guid.NewGuid(), DocType = "CashSales", ModuleMasterId = 4, Amount = 150, TotalAmount = 600, TaxId = 7 } };
    //        return cashSaleModel;
    //    }

    //    public CashSaleModel FillCashSaleModel(CashSaleModel csModel)
    //    {
    //        csModel.Id = Guid.NewGuid();
    //        csModel.CompanyId = 5;
    //        csModel.DocSubType = "Doc-1";
    //        csModel.CashSaleNumber = "SYS-1";
    //        csModel.DocDate = Convert.ToDateTime("2016-09-23 05:30:00.000");
    //        csModel.DocNo = "Doc-123";
    //        csModel.ModeOfReceipt = "Cash";
    //        csModel.EntityType = "Customer";
    //        csModel.EntityId = Guid.Parse("84D08D81-50FD-9211-FD5E-0B9B05A81EEC");
    //        csModel.DocumentState = "Active";
    //        //csModel.BalanceAmount = 3000;
    //        csModel.GrandTotal = 52000;
    //        csModel.IsGstSettings = true;
    //        csModel.IsMultiCurrency = true;
    //        csModel.COAId = 11692;
    //        //csModel.IsRepeatingInvoice = true;
    //        csModel.IsSegmentReporting = true;
    //        csModel.IsAllowableNonAllowable = true;
    //        csModel.BaseCurrency = "UDS";
    //        csModel.BalanceAmount = 125500;
    //        csModel.ServiceCompanyId = 63;            
    //        csModel.IsNoSupportingDocument = true;           
    //        csModel.IsGSTApplied = false;
    //        csModel.DocCurrency = "USD";
    //       // csModel.BaseCurrency = "SGD";
    //        csModel.Status = RecordStatusEnum.Active;
    //        csModel.CashSaleDetails = new List<CashSaleDetailModel> { new CashSaleDetailModel { Id = Guid.NewGuid(), CashSaleId = csModel.Id, COAId =1, TaxId = 7, DocAmount = 120, DocTotalAmount = 500, BaseTaxAmount = 130 ,AmtCurrency="USD"} };
    //        //csModel.GSTDetailModels = new List<GSTDetailModel> { new GSTDetailModel { Id = Guid.NewGuid(), DocId = Guid.NewGuid(), DocType = "CashSales", ModuleMasterId = 4, Amount = 150, TotalAmount = 600, TaxId = 7 } };
    //        return csModel;
    //    }
    //    #endregion

    //    #region CreateCashSales
    //    [TestMethod]
    //    public void CreateCashSales_ByCompanyId()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "955D6E92-E154-47C2-9FF4-17DB3B77C809" });
    //        var requestUrl = CommonContest.GetCashSaleUrl + "createcashsales";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(lstEmployees);
    //    }

    //    [TestMethod]
    //    public void CreateCashSales_ByCompanyId_NegativeTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = null });
    //        var requestUrl = CommonContest.GetCashSaleUrl + "createcashsales";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(lstEmployees.StatusCode, HttpStatusCode.BadRequest);
    //    }

    //    #endregion

    //    #region GetAllCashSalesK

    //    [TestMethod]
    //    public void GetAllCashSalesK_ByCompanyId()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:20,skip:0}&companyId", Value = "1" });
    //        var requestUrl = CommonContest.GetCashSaleUrl + "getallcashsalesk";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(lstEmployees);
    //    }

    //    [TestMethod]
    //    public void GetAllCashSalesK_ByCompanyId_NegativeTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:20,skip:0}&companyId", Value = null });
    //        var requestUrl = CommonContest.GetCashSaleUrl + "getallcashsalesk";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(lstEmployees.StatusCode, HttpStatusCode.BadRequest);
    //    }

    //    #endregion

    //    #region GetAllCashSalesLUs

    //    [TestMethod]
    //    public void GetAllCashSalesLUs_ByCompanyId()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "cashsaleId", Value = "DEEB6B94-D87D-4221-9770-866661BCC5B8" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonContest.GetCashSaleUrl + "getallcashsaleslu";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(lstEmployees);
    //    }

    //    [TestMethod]
    //    public void GetAllCashSalesLUs_ByCompanyId_NegativeTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonContest.GetCashSaleUrl + "getallcashsalesk";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(lstEmployees.StatusCode, HttpStatusCode.BadRequest);
    //    }

    //    #endregion
    //}
}
