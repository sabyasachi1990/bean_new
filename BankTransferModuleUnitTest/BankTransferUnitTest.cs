



using AppsWorld.BankTransferModule.Models;
using BankTransferModuleUnitTest.CommonResource;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BankTransferModuleUnitTest
{
    //[TestClass]
    //public class BankTransferUnitTest
    //{
    //    string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();



    //    #region GetAllBankTransferK
    //    [TestMethod]
    //    public void GetAllBankTransferK_ByComapanyId_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:20,skip:0}&companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBankTransferUrl + "banktransferk";
    //        var banktransfer = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(banktransfer.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void GetBankTransferK_ByComapanyId_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:20,skip:0}&companyId", Value = null });
    //        var requestUrl = CommonConstant.GetBankTransferUrl + "banktransferk";
    //        var banktransfer = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(banktransfer.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion

    //    #region GetAllBankTransferLUs

    //    [TestMethod]
    //    public void GetAllBankTransferLUs_ByCompanyId()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "banktransferId", Value = "CCA9A1DE-6F67-4989-B8FD-EE5D28CF1407" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBankTransferUrl + "getallbanktransferlu";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(lstEmployees);
    //    }

    //    [TestMethod]
    //    public void GetAllBankTransferLUs_ByCompanyId_NegativeTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "banktransferId", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBankTransferUrl + "getallbanktransferlu";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(lstEmployees.StatusCode, HttpStatusCode.BadRequest);
    //    }

    //    #endregion

    //    #region CreateBankTransfer
    //    [TestMethod]
    //    public void CreateBankTransfer_ByCompanyId()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "CCA9A1DE-6F67-4989-B8FD-EE5D28CF1407" });
    //        var requestUrl = CommonConstant.GetBankTransferUrl + "createbankransfer";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(lstEmployees);
    //    }

    //    [TestMethod]
    //    public void CreateBankTransfer_ByCompanyId_NegativeTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = null });
    //        var requestUrl = CommonConstant.GetBankTransferUrl + "createbankransfer";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(lstEmployees.StatusCode, HttpStatusCode.BadRequest);
    //    }

    //    #endregion

    //    #region Save Call


    //    [TestMethod]
    //    public void BankTransfer_Positive_Test()
    //    {
    //        BankTransferModel bankTransferModel = new BankTransferModel();
    //        bankTransferModel = FillBankModel(bankTransferModel);
    //        var json = RestHelper.ConvertObjectToJason(bankTransferModel);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetBankTransferUrl + "savebanktransfer", json);
    //        Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
    //    }
    //    public BankTransferModel FillBankModel(BankTransferModel bankTransferModel)
    //    {
    //        bankTransferModel.Id = Guid.NewGuid();
    //        bankTransferModel.CompanyId = 1;
    //        bankTransferModel.DocType = "Transfer";
    //        bankTransferModel.SystemRefNo = "BT-2017-00005";
    //        bankTransferModel.TransferDate = Convert.ToDateTime("2016-09-23 05:30:00.000");
    //        bankTransferModel.DocNo = "Doc-1234";
    //        bankTransferModel.ModeOfTransfer = "Cash";
    //        bankTransferModel.DocDescription = "Transfer Discription";
    //        bankTransferModel.TransferDate = Convert.ToDateTime("2017-01-01 05:30:00.000");
    //        bankTransferModel.IsGstSetting = true;
    //        bankTransferModel.IsMultiCompany = true;
    //        bankTransferModel.IsMultiCurrency = true;
    //        bankTransferModel.Status = AppsWorld.Framework.RecordStatusEnum.Active;
    //        bankTransferModel.BankTransferDetailsModel = new List<BankTransferDetailModel> { new BankTransferDetailModel { Id = Guid.NewGuid(), BankTransferId = bankTransferModel.Id, COAId = 1, ServiceCompanyId = 2, Amount = 120, Type = "Wi" } };
    //        return bankTransferModel;
    //    }
    //    [TestMethod]
    //    public void BankTransfer_Negative_Test()
    //    {
    //        BankTransferModel bankTransferModel = new BankTransferModel();
    //        bankTransferModel = FillBakTransfer_Negative_Test(bankTransferModel);
    //        var json = RestHelper.ConvertObjectToJason(bankTransferModel);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetBankTransferUrl + "savebanktransfer", json);
    //        Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
    //    }
    //    private BankTransferModel FillBakTransfer_Negative_Test(BankTransferModel bankTransferModel)
    //    {
    //        bankTransferModel.Id = Guid.NewGuid();
    //       // bankTransferModel.CompanyId = 1;
    //        bankTransferModel.DocType = "Transfer";
    //        bankTransferModel.SystemRefNo = "BT-2017-00004";
    //        bankTransferModel.TransferDate = Convert.ToDateTime("2016-09-23 05:30:00.000");
    //        bankTransferModel.DocNo = "TT_1";
    //        bankTransferModel.ModeOfTransfer = "Cash";
    //        bankTransferModel.DocDescription = "Transfer Discription";
    //        bankTransferModel.TransferDate= Convert.ToDateTime("2017-01-01 05:30:00.000");
    //        bankTransferModel.IsGstSetting= true;
    //        bankTransferModel.IsMultiCompany = true;
    //        bankTransferModel.IsMultiCurrency = true;
    //        bankTransferModel.Status = AppsWorld.Framework.RecordStatusEnum.Active;
    //        bankTransferModel.BankTransferDetailsModel = new List<BankTransferDetailModel> { new BankTransferDetailModel { Id = Guid.NewGuid(), BankTransferId = bankTransferModel.Id, COAId = 1, ServiceCompanyId = 2, Amount = 500, Type = "wit"} };
    //        return bankTransferModel;
    //    }
    //    #endregion
    //}
}
