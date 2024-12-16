using AppsWorld.GLClearingModule.Models;
using ClearingUnitTest.CommonResource;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClearingUnitTest
{
    //[TestClass]
    //public class ClearingUnitTest
    //{
    //    string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();


    //    #region GetAllClearingK
    //    [TestMethod]
    //    public void GetAllClearingK_ByCompanyId_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "8" });
    //        var requestUrl = CommonConstant.GetClearingUrl + "getallclearingk";
    //        var allclearing = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(allclearing.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void GetAllClearingK_ByCompanyId_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "fghjhj" });
    //        var requestUrl = CommonConstant.GetClearingUrl + "getallclearingk";
    //        var allclearing = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(allclearing.StatusCode, HttpStatusCode.BadRequest);
    //    }

    //    #endregion


    //    #region CreateClearing
    //    [TestMethod]
    //    public void CreateClearing_ById_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "45C18F95-C5E6-47F8-8BD9-035967443A8A" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "13" });
    //        var requestUrl = CommonConstant.GetClearingUrl + "createclearing";
    //        var clearing = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(clearing.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void CreateClearing_ById_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "Id", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "abc" });
    //        var requestUrl = CommonConstant.GetClearingUrl + "createclearing";
    //        var clearing = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNull(clearing.StatusCode);
    //    }
    //    [TestMethod]
    //    public void CreateClearingDetail_ById_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "date", Value = "2017-03-13 14:17:08.2820000" });
    //        lstParameter.Add(new List<string, string>() { Name = "coaId", Value = "9" });
    //        lstParameter.Add(new List<string, string>() { Name = "serviceCompanyId", Value = "8" });
    //        var requestUrl = CommonConstant.GetClearingUrl + "createclearingdetail";
    //        var Clearingdetail = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Clearingdetail.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void CreateClearingDetail_ById_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "date", Value = "2017-03-13 14:17:08.2820000" });
    //        lstParameter.Add(new List<string, string>() { Name = "coaId", Value = "abc" });
    //        lstParameter.Add(new List<string, string>() { Name = "serviceCompanyId", Value = "abc" });
    //        var requestUrl = CommonConstant.GetClearingUrl + "createclearingdetail";
    //        var Clearingdetail = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Clearingdetail.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion

    //    #region SaveClearing
    //    [TestMethod]
    //    public void SaveClearing()
    //    {
    //        ClearingModel clearingModel = new ClearingModel();
    //        clearingModel = FillClearingModel(clearingModel);
    //        var json = RestHelper.ConvertObjectToJason(clearingModel);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetClearingUrl + "saveclearing", json);
    //        Assert.IsNotNull(response);
    //    }

    //    public ClearingModel FillClearingModel(ClearingModel clearingModel)
    //    {
    //        clearingModel.Id = Guid.NewGuid();
    //        //clearingModel.CheckAmount = 1111;
    //        clearingModel.COAId = 10659;
    //        //clearingModel.COAId2 = 4541;
    //        clearingModel.CompanyId = 174;
    //        //clearingModel.CrDr = "Cr";
    //        clearingModel.CreatedDate = DateTime.UtcNow; ;
    //        clearingModel.DocDate = DateTime.UtcNow;
    //        //clearingModel.DocDescription = "Testing 123 Umang";
    //        clearingModel.DocNo = "CL_12345";
    //        clearingModel.DocType = "Clearing";
    //        clearingModel.DocumentState = "Created";
    //        //clearingModel.IsMultiCurrency = true;
    //        //clearingModel.Remarks = "Hyderabad258";
    //        clearingModel.ServiceCompanyId = 183;
    //        clearingModel.Status = 1;
    //        //clearingModel.SystemRefNo = "CL - 2017 - 00017";
    //        clearingModel.UserCreated = "abc@republic.com";
            
    //        //clearingModel.ClearingDetailModel = new List<ClearingDetailModel>()
    //        //{
    //        //    //new ClearingDetailModel { Id=Guid.Parse("D4AEE8F1-9C2B-4A90-A623-01E3503A5E08"), CrDr="Cr", BaseAmount=1111, BaseCurrency="USD", DocAmount=4700, DocDate=DateTime.Parse("2017-04-15 00:00:00.0000000"), DocCurrency="USD", DocNo= "CL_002-R", DocType="Clearing", DocumentId=Guid.Parse("E642EBFB-28C2-4454-9BFF-F6D64C90AC5E"), GLClearingId=clearingModel.Id, IsCheck=true, RecOrder=2, RecordStatus="Added", SystemRefNo="CL-2017-00017" }
    //        //};
    //        return clearingModel;
    //    }
    //    #endregion
    //}
}

