using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppsWorld.RevaluationModule.Models;
using AppsWorld.Framework;
using System.Net;

namespace RevaluationUnitTest
{
    //[TestClass]
    //public class RevaluationUnitTest
    //{
    //    string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();
    //    #region Create Revaluation
    //    [TestMethod]
    //    public void CreateRevaluation_ByComapanyId_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "dateTime", Value = "2016-09-01%2000:00:00.0000000" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetRevaluationUrl + "createrevaluation";
    //        var lstRevalution = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(lstParameter);
    //    }
    //    [TestMethod]
    //    public void CreateRevaluation_ByComapanyId_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "dateTime", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "bdhdnjk" });
    //        var requestUrl = CommonConstant.GetRevaluationUrl + "createrevaluation";
    //        var lstRevalution = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(lstParameter);
    //    }
    //    #endregion

    //    #region Save Revaluation
    //    [TestMethod]
    //    public void SaveRevaluation_PositiveCase()
    //    {
    //        var revaluation = new RevaluationSaveModel();
    //        revaluation = FillRevaluationModel(revaluation);
    //        var json = RestHelper.ConvertObjectToJason(revaluation);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetRevaluationUrl + "saverevaluation", json);
    //        Assert.IsNotNull(response);
    //    }
    //    private RevaluationSaveModel FillRevaluationModel(RevaluationSaveModel revaluationModel)
    //    {
    //        revaluationModel.Id = Guid.NewGuid();
    //        revaluationModel.CompanyId = 5;
    //        revaluationModel.CreatedDate = DateTime.UtcNow;
    //        revaluationModel.UserCreated = "sairam";
    //        revaluationModel.ModifiedDate = DateTime.UtcNow;
    //        revaluationModel.ModifiedBy = "avinash m";
    //        revaluationModel.RevaluationDate = Convert.ToDateTime("2015-02-13T03:30:00");
    //        revaluationModel.Status = RecordStatusEnum.Active;
    //        revaluationModel.DocState = "verified";
    //        //revaluationModel.RevaluationModels = new List<RevaluationModel>
    //        //{
    //        //    new RevaluationModel{Id=Guid.NewGuid(),COAId=3,EntityId=Guid.NewGuid(),DocumentId=Guid.NewGuid(),RevalutionId=Guid.Parse("C64E29BF-088B-4458-8550-0326D9AD2BC6"),Status=RecordStatusEnum.Active}
    //        //};
    //        return revaluationModel;
    //    }
    //    #endregion

    //    #region GetAllRevaluationK
    //    [TestMethod]
    //    public void GetAllRevaluationK_ByComapanyId_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetRevaluationUrl + "getallrevaluationk";
    //        var Revaluation = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Revaluation.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void GetAllRevaluationK_ByComapanyId_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "sdfgs" });
    //        var requestUrl = CommonConstant.GetRevaluationUrl + "getallrevaluationk";
    //        var Revaluation = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Revaluation.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion
    //}
}
