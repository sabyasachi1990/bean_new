using AppsWorld.BankWithdrawalModule.Models;
using AppsWorld.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WithdrawalUnitTest
{
    //[TestClass]
    public class WithdrawalUnitTest
    {
        //string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();

        //#region GetAllWithdrawalK
        //[TestMethod]
        //public void GetAllWithdrawalK_ByComapanyId_PositiveCase()
        //{
        //    List<List<string, string>> lstParameter = new List<List<string, string>>();
        //    lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}" });
        //    lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
        //    lstParameter.Add(new List<string, string>() { Name = "isWithdrawal", Value ="true"});
        //    var requestUrl = CommonConstant.GetWithdrawalUrl + "getallbankwithdrawalk";
        //    var Withdraw = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
        //    Assert.AreNotEqual(Withdraw.StatusCode, HttpStatusCode.BadRequest);
        //}
        //[TestMethod]
        //public void GetAllWithdrawalK_ByComapanyId_NegativeCase()
        //{
        //    List<List<string, string>> lstParameter = new List<List<string, string>>();
        //    lstParameter.Add(new List<string, string>() { Name = null });
        //    lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "sdfgs" });
        //    lstParameter.Add(new List<string, string>() { Name = "isWithdrawal", Value = "1" });
        //    var requestUrl = CommonConstant.GetWithdrawalUrl + "getallbankwithdrawalk";
        //    var Withdraw = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
        //    Assert.AreNotEqual(Withdraw.StatusCode, HttpStatusCode.BadRequest);
        //}
        //#endregion

        //#region GetAllPaymentLookups
        //[TestMethod]
        //public void GetAllWithdrawalLookups_ByCompanyId_PositiveCase()
        //{
        //    List<List<string, string>> lstParameter = new List<List<string, string>>();
        //    lstParameter.Add(new List<string, string>() { Name = "id", Value = "9E0290CE-4F22-48C0-903C-0F7C69030537" });
        //    lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
        //    lstParameter.Add(new List<string,string>() { Name = "isWithdrawal",Value="true"});
        //    var requestUrl = CommonConstant.GetWithdrawalUrl + "withdrawalslu";
        //    var Withdraw = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
        //    Assert.AreNotEqual(Withdraw.StatusCode, HttpStatusCode.BadRequest);
        //}
        //[TestMethod]
        //public void GetAllWithdrawalLookups_ByCompanyId_NegativeCase()
        //{
        //    List<List<string, string>> lstParameter = new List<List<string, string>>();
        //    lstParameter.Add(new List<string, string>() { Name = "id",Value=null });
        //    lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "gssvj" });
        //    lstParameter.Add(new List<string, string>() { Name = "isWithdrawal", Value = "gssvj" });
        //    var requestUrl = CommonConstant.GetWithdrawalUrl + "withdrawalslu";
        //    var Withdraw = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
        //    Assert.AreNotEqual(Withdraw.StatusCode, HttpStatusCode.BadRequest);
        //}
        //[TestMethod]
        //public void GetEntityLookup_ByEntityId_PositiveCase()
        //{
        //    List<List<string, string>> lstParameter = new List<List<string, string>>();
        //    lstParameter.Add(new List<string, string>() { Name = "id", Value = "A0A602B4-318D-A58D-4BAE-06C0F9968A51" });
        //    lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "18" });
        //    lstParameter.Add(new List<string, string>() { Name = "type", Value = "Employee" });
        //    lstParameter.Add(new List<string, string>() { Name = "isWithdrawal", Value = "true" });
        //    var requestUrl = CommonConstant.GetWithdrawalUrl + "entityslu";
        //    var Withdraw = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
        //    Assert.AreNotEqual(Withdraw.StatusCode, HttpStatusCode.BadRequest);
        //}
        //[TestMethod]
        //public void GetCurrencyLookup_ByEntityId_NegativeCase()
        //{
        //    List<List<string, string>> lstParameter = new List<List<string, string>>();
        //    lstParameter.Add(new List<string, string>() { Name = "id", Value = null });
        //    lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
        //    lstParameter.Add(new List<string, string>() { Name = "type", Value = "ddjvv" });
        //    lstParameter.Add(new List<string, string>() { Name = "isWithdrawal", Value = "false" });
        //    var requestUrl = CommonConstant.GetWithdrawalUrl + "entityslu";
        //    var Withdraw = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
        //    Assert.AreNotEqual(Withdraw.StatusCode, HttpStatusCode.BadRequest);
        //}
        //#endregion

        //#region CreateWithdrawal
        //[TestMethod]
        //public void CreatePayment_ById_PositiveCase()
        //{
        //    List<List<string, string>> lstParameter = new List<List<string, string>>();
        //    lstParameter.Add(new List<string, string>() { Name = "withdrawalId", Value = "9E0290CE-4F22-48C0-903C-0F7C69030537" });
        //    lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
        //    lstParameter.Add(new List<string, string>() { Name = "isWithdrawal", Value = "true" });
        //    var requestUrl = CommonConstant.GetWithdrawalUrl + "createwithdrawal";
        //    var Withdraw = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
        //    Assert.AreNotEqual(Withdraw.StatusCode, HttpStatusCode.BadRequest);
        //}
        //[TestMethod]
        //public void CreatePayment_ById_NegativeCase()
        //{
        //    List<List<string, string>> lstParameter = new List<List<string, string>>();
        //    lstParameter.Add(new List<string, string>() { Name = "withdrawalId", Value = null });
        //    lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "dhfjskdf" });
        //    lstParameter.Add(new List<string, string>() { Name = "isWithdrawal", Value = "hvd" });
        //    var requestUrl = CommonConstant.GetWithdrawalUrl + "createwithdrawal";
        //    var Withdraw = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
        //    Assert.IsNull(Withdraw.StatusCode);
        //}
        //#endregion

        //#region Save Withdrawal
        //[TestMethod]
        //public void SaveWithdrawal()
        //{
        //    WithdrawalModel model = new WithdrawalModel();
        //    //model = FillWithdrawalModel(model);
        //    var json = RestHelper.ConvertObjectToJason(model);
        //    var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetWithdrawalUrl + "savewithdrawal", json);
        //    Assert.IsNotNull(response);
        //}
        ////public WithdrawalModel FillWithdrawalModel(WithdrawalModel wModel)
        ////{
        ////    wModel.COAModels = new COAModel { COAId = 678, COAName = "Check" };
        ////    wModel.EntityModels = new EntityModel { EntityId = Guid.Parse("C8DD0A63-83BE-5734-4BAF-0AEAA83B975E"), EntityName = "Bank", EntityType = "CUST" };
        ////    wModel.ModeOfWithdrawalModels = new ModeOfWithdrawalModel { code = "D-001", Name = "WID" };
        ////    wModel.ServiceCompanyModels = new ServiceCompanyModel { ServiceCompanyId = 4, ServiceCompanyName = "Vector" };
        ////    wModel.Id = Guid.NewGuid();
        ////    wModel.CompanyId = 5;
        ////    wModel.DocType = "Doc-1";
        ////    wModel.SystemRefNo = "SYS-1";
        ////    wModel.DocDate = Convert.ToDateTime("2016-09-23 05:30:00.000");
        ////    //wModel.DueDate = Convert.ToDateTime("2016-12-01 02:25:09.000");
        ////    wModel.DocNo = "Doc-123";
        ////    wModel.IsMultiCurrencyActivated = true;
        ////    wModel.IsAllowableNonAllowableActivated = true;
        ////    wModel.IsGstSettingsActivated = true;
        ////    wModel.IsNoSupportingDocument = true;
        ////    wModel.IsSegmentReportingActivated = true;
        ////    wModel.ISGstDeRegistered = false;
        ////    wModel.BankWithdrawalAmmount = 1000;
        ////    wModel.IsDisAllow = false;
        ////    wModel.GrandTotal = 52000;
        ////    wModel.DocCurrency = "USD";
        ////    wModel.BaseCurrency = "SGD";
        ////    wModel.Status = RecordStatusEnum.Active;
        ////    wModel.WithdrawalDetailModels = new List<WithdrawalDetailModel> { new WithdrawalDetailModel { Id = Guid.NewGuid(), WithdrawalId = wModel.Id, COAId = wModel.COAId, TaxId = 7, DocAmount = 120, DocTotalAmount = 500, BaseTaxAmount = 130 } };
        ////    wModel.GSTDetailModels = new List<GSTDetailModel> { new GSTDetailModel { Id = Guid.NewGuid(), DocId = Guid.NewGuid(), DocType = "Withdraw", ModuleMasterId = 4, Amount = 150, TotalAmount = 600, TaxId = 7 } };
        ////    return wModel;
        ////}
        //#endregion
    }
}
