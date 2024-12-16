using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using AppsWorld.OpeningBalancesModule.Models;


namespace OpeningBalancesUnitTest
{
    //[TestClass]
    //public class OpeningBalancesUnitTest
    //{
    //    string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();

    //    #region Kendo_GetAllOpeningBalancessK
    //    [TestMethod]
    //    public void GetAllOpeningBalancessK_ByComapanyId_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "58" });
    //        var requestUrl = CommonConstant.GetOpeningBalanceUrl + "getallbalancessk";
    //        var OpeningBalance = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(OpeningBalance.StatusCode, HttpStatusCode.OK);
    //    }

    //    [TestMethod]
    //    public void GetAllOpeningBalancessK_ByComapanyId_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "10s" });
    //        var requestUrl = CommonConstant.GetOpeningBalanceUrl + "getallbalancessk";
    //        var OpeningBalance = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(OpeningBalance.StatusCode, HttpStatusCode.OK);
    //    }

    //    #endregion

    //    #region Get_AllOpeningBalanceLookup
    //    [TestMethod]
    //    public void GetAllOpeningBalanceLookups_ByCompanyId_ServiceCompanyid_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "58" });
    //        lstParameter.Add(new List<string, string>() { Name = "ServiceCompanyId", Value = "59" });
    //        var requestUrl = CommonConstant.GetOpeningBalanceUrl + "getopeningbalancelu";
    //        var OpeningBalance = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(OpeningBalance.StatusCode, HttpStatusCode.OK);
    //    }

    //    [TestMethod]
    //    public void GetAllOpeningBalanceLookups_ByCompanyId_ServiceCompanyid_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "58" });
    //        lstParameter.Add(new List<string, string>() { Name = "ServiceCompanyId", Value = "sa" });
    //        var requestUrl = CommonConstant.GetOpeningBalanceUrl + "getopeningbalancelu";
    //        var OpeningBalance = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(OpeningBalance.StatusCode, HttpStatusCode.OK);
    //    }


    //    #endregion

    //    #region Get_OpeningBalance
    //    [TestMethod]
    //    public void Get_OpeningBalance_By_CompanyId_ServiceCompanyid_Positivecase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "58" });
    //        lstParameter.Add(new List<string, string>() { Name = "ServiceCompanyId", Value = "59" });
    //        var requestUrl = CommonConstant.GetOpeningBalanceUrl + "getservicecompanyopeningbalance";
    //        var OpeningBalance = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(OpeningBalance.StatusCode, HttpStatusCode.OK);
    //    }

    //    [TestMethod]
    //    public void Get_OpeningBalance_By_CompanyId_ServiceCompanyid_Negativecase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "58" });
    //        lstParameter.Add(new List<string, string>() { Name = "ServiceCompanyId", Value = "1bc" });
    //        var requestUrl = CommonConstant.GetOpeningBalanceUrl + "getservicecompanyopeningbalance";
    //        var OpeningBalance = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(OpeningBalance.StatusCode, HttpStatusCode.OK);

    //    }

    //    #endregion

    //    #region Get_LineItemsForCOA
    //    [TestMethod]
    //    public void Get_LineItems_ForCOA_Positive_TestCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "OpeningBalanceDetailId", Value = "A02EF52E-EDFE-4692-80AD-B4AE8637B878" });
    //        var requestUrl = CommonConstant.GetOpeningBalanceUrl + "getlineitemsforcoa";
    //        var OpeningBalance = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(OpeningBalance.StatusCode, HttpStatusCode.OK);
    //    }

    //    [TestMethod]
    //    public void Get_LineItems_ForCOA_Negative_TestCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "OpeningBalanceDetailId", Value = "aa2EF52E-EDFE-4692-80AD-B4AE8637B878sasaasasasa" });
    //        var requestUrl = CommonConstant.GetOpeningBalanceUrl + "getlineitemsforcoa";
    //        var OpeningBalance = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(OpeningBalance.StatusCode, HttpStatusCode.OK);
    //    }

    //    #endregion

    //    #region Savecall_SaveOpeningBalance

    //    [TestMethod]
    //    public void SaveOpeningBalance()
    //    {
    //        OpeningBalanceModel openingBalanceModel = new OpeningBalanceModel();
    //        openingBalanceModel = FillOpeningBalanceModel_Positive(openingBalanceModel);
    //        var json = RestHelper.ConvertObjectToJason(openingBalanceModel);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetOpeningBalanceUrl + "SaveOpeningBalance", json);
    //        Assert.AreEqual(response.StatusCode,HttpStatusCode.OK);
    //    }
    //    [TestMethod]
    //    public void SaveOpeningBalance_Negative_Test()
    //    {
    //        OpeningBalanceModel openingBalanceModel = new OpeningBalanceModel();
    //        openingBalanceModel = FillOpeningBalanceModel_Negative(openingBalanceModel);
    //        var json = RestHelper.ConvertObjectToJason(openingBalanceModel);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetOpeningBalanceUrl + "SaveOpeningBalance", json);
    //        Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
    //    }

    //    #region FillOpeningBalanceModel
    //    public OpeningBalanceModel FillOpeningBalanceModel_Positive(OpeningBalanceModel openingBalanceModels)
    //    {
    //        Guid id = Guid.NewGuid();
    //        openingBalanceModels.Id = Guid.NewGuid();
    //        openingBalanceModels.CompanyId = 58;
    //        openingBalanceModels.ServiceCompanyId = 59;
    //        openingBalanceModels.BaseCurrency = "SGD";
    //        openingBalanceModels.Date = Convert.ToDateTime("2016-09-23 05:30:00.000");
    //        openingBalanceModels.Details = new List<OpeningBalanceDetailModel> { new OpeningBalanceDetailModel{ 
    //       AccountCode="saikiran",
    //       OpeningBalanceId=openingBalanceModels.Id,
    //       Id=id,
    //       COAId=5748,
    //       LineItems=new List<OpeningBalanceLineItemModel>{new OpeningBalanceLineItemModel{Id=Guid.NewGuid(),
    //       OpeningBalanceDetailId=id,
    //       COAId=5748,
    //       EntityId=Guid.Parse("0FEA9AF8-0371-2AB3-D70D-188509476283"),
    //       Date=DateTime.UtcNow}}}};
    //        return openingBalanceModels;
    //    }

    //    public OpeningBalanceModel FillOpeningBalanceModel_Negative(OpeningBalanceModel openingBalanceModels)
    //    {
    //        //Guid id = Guid.NewGuid();
    //        openingBalanceModels.Id = Guid.NewGuid();
    //        openingBalanceModels.CompanyId = 58;
    //        openingBalanceModels.ServiceCompanyId = 59;
    //        openingBalanceModels.BaseCurrency = "SGD";
    //        openingBalanceModels.Date = Convert.ToDateTime("2016-09-23 05:30:00.000");
    //        openingBalanceModels.Details = new List<OpeningBalanceDetailModel> { new OpeningBalanceDetailModel{ 
    //       AccountCode="saikiran",
    //       OpeningBalanceId=openingBalanceModels.Id,
    //       Id=Guid.Parse("0FEA9AF8-0371-2AB3-D70D-1885094762833"),
    //       COAId=5748,
    //       LineItems=new List<OpeningBalanceLineItemModel>{new OpeningBalanceLineItemModel{Id=Guid.NewGuid(),
    //       OpeningBalanceDetailId=Guid.Parse("9FEA9AF8-0371-2AB3-D70D-188509476283"),
    //       COAId=5748,
    //       EntityId=Guid.Parse("0FEA9AF8-0371-2AB3-D70D-188509476283"),
    //       Date=DateTime.UtcNow}}}};
    //        return openingBalanceModels;
    //    }

    //    #endregion

    //    #endregion
    //}
}
