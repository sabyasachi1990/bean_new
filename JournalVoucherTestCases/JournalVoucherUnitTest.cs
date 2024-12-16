using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.Model;
using AppsWorld.JournalVoucherModule.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JournalVoucherTestCases;



namespace JournalVoucherTestCases
{
    //[TestClass]
    //public class JournalVoucherUnitTest
    //{
    //    string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();

    //    #region CreateJournalL
    //    [TestMethod]
    //    public void CreateJournal_GetByIdAndCompanyID_Validtest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "132840BE-D422-4994-80FB-004F27D5E67F" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetJournalUrl + "createjournal";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(Employees.StatusCode);
    //    }

    //    [TestMethod]
    //    public void CreateJournal_GetByIdAndCompanyID_InValidtest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "dsfdsfdsfdsfds" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetJournalUrl + "createjournal";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(Employees.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion

    //    #region GetAllJournalLU
    //    [TestMethod]
    //    public void GetAllJournalLU_GetByIdAndCompanyID_Validtest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "journalId", Value = "132840BE-D422-4994-80FB-004F27D5E67F" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetJournalUrl + "journallu";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(Employees.StatusCode);
    //    }
    //    [TestMethod]
    //    public void GetAllJournalLU_GetByIdAndCompanyID_InValidtest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "journalId", Value = "null" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "2" });
    //        var requestUrl = CommonConstant.GetJournalUrl + "journallu";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNull(Employees.StatusCode);
    //    }
    //    #endregion

    //    #region GetByDetailId
    //    [TestMethod]
    //    public void GetByDetailId_GetByIdAndCompanyID_Validtest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "DE5E5EA3-3EDC-434B-B97A-0001B1A6525C" });
    //        lstParameter.Add(new List<string, string>() { Name = "journalId", Value = "15322770-52E8-4127-959D-2DBE0916FAC0" });
    //        var requestUrl = CommonConstant.GetJournalUrl + "createjournaldetail";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(Employees.StatusCode);
    //    }
    //    [TestMethod]
    //    public void GetByDetailId_GetByIdAndCompanyID_InValidtest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "12" });
    //        lstParameter.Add(new List<string, string>() { Name = "journalId", Value = "15322770-52E8-4127-959D-2DBE0916FAC0" });
    //        var requestUrl = CommonConstant.GetJournalUrl + "createjournaldetail";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(Employees.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion

    //    #region CreateJournalVoid

    //    [TestMethod]
    //    public void CreateJournalVoid_GetByIdAndCompanyID_Validtest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "132840BE-D422-4994-80FB-004F27D5E67F" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetJournalUrl + "createJournalvoid";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(Employees.StatusCode);
    //    }
    //    [TestMethod]
    //    public void CreateJournalVoid_GetByIdAndCompanyID_InValidtest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "null" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetJournalUrl + "createJournalvoid";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(Employees.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion


    //    [TestMethod]

    //    public void SaveJournal()
    //    {
    //        JournalModel custjournal = new JournalModel();
    //        custjournal = Filljournalmodule(custjournal);
    //        var json = RestHelper.ConvertObjectToJason(custjournal);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetJournalUrl + "savejournal", json);
    //        Assert.IsNotNull(response);
    //    }
    //    private JournalModel Filljournalmodule(JournalModel journalmodule)
    //    {
    //        //journalmodule.ServiceCompanyMOdels = new ServiceCompanyModel { ServiceCompanyId = 12, ServiceCompanyName = "fgdfgd" };
    //        //journalmodule.SegmentCategory = new SegmentCategoryModel { SegmentMasterId1 = 101, SegmentCategoryId1 = 12, SegmentCategoryName1 = "clock", SegmentMasterId2 = 102, SegmentCategoryId2 = 13, SegmentCategoryName2 = "Book" };
    //        journalmodule.Id = Guid.NewGuid();
    //        journalmodule.CompanyId = 3;
    //        journalmodule.Nature = "Treade";
    //        journalmodule.DocNo = "RC_2015-07-888";
    //        journalmodule.PostingDate = DateTime.UtcNow;
    //        journalmodule.DocDate = DateTime.UtcNow;
    //        journalmodule.DueDate = DateTime.UtcNow;
    //        journalmodule.ExchangeRate = Convert.ToDecimal("521.0100");
    //        journalmodule.BaseCurrency = "dsfjs";
    //        journalmodule.DocumentState ="Not Paid";
    //        journalmodule.GSTTotalAmount =Convert.ToDecimal("0.00");
    //        journalmodule.SystemORManual = "jhsfhjs";
    //        journalmodule.ISMultiCurrency = false;
    //        journalmodule.IsGstSettings = false;
    //        //journalmodule.IsNoSupportingDocument = true;
    //        journalmodule.Status = RecordStatusEnum.Active;
    //        //journalmodule.NoSupportingDocument = true;
    //        //journalmodule.NoSupportingDocument = true;
    //        //journalmodule.ISSegmentReporting = true;
    //        //journalmodule.ISGstDeRegistered = true;
    //        journalmodule.Remarks = "nothing";
    //        //journalmodule.FinancialEndDate = Convert.ToDateTime("08-11-2016");
    //        journalmodule.FinancialPeriodLockStartDate = Convert.ToDateTime("30/06/2016");
    //        journalmodule.FinancialPeriodLockEndDate = Convert.ToDateTime("18/08/2016");
    //        //journalmodule.FinancialStartDate = Convert.ToDateTime("20-10-2015");
    //        journalmodule.UserCreated = "megasoft@mailinator.com";
    //        journalmodule.CreatedDate = Convert.ToDateTime("2016-09-01 12:13:01.1347526");
    //        journalmodule.ModifiedBy = null;
    //        journalmodule.ModifiedDate = null;
    //        journalmodule.Version = null;
    //        journalmodule.IsGSTCurrencyRateChanged = null;
    //        journalmodule.PeriodLockPassword = "123456";
    //        journalmodule.IsAutoReversalJournal = null;
    //        journalmodule.IsRecurringJournal = null;
    //        CurrencyModel CurrencyModel = new CurrencyModel();
    //        CurrencyModel.CurrencyName = "Cash";
    //        journalmodule.CreationType ="journal";
    //        //journalmodule.ISSegmentReporting =true;
    //        journalmodule.IsGstSettings =true;
    //        journalmodule.IsAllowableNonAllowable =true;
    //        journalmodule.JournalDetailModels = new List<JournalDetailModel>()
    //        {
    //            new JournalDetailModel {Id = Guid.NewGuid(), JournalId = Guid.NewGuid(), COAId = 2,DocDebit = Convert.ToDecimal("52.43"),DocCredit =Convert.ToDecimal("0.00") ,DocTaxCredit =Convert.ToDecimal("0.00") ,DocTaxDebit =Convert.ToDecimal("0.00") ,DocDebitTotal = Convert.ToDecimal("0.00"),DocCreditTotal = Convert.ToDecimal("0.00")}
    //        };
    //        //journalmodule.JournalGSTDetails = new List<JournalGSTDetail>(){
    //        //    new JournalGSTDetail{Id = Guid.NewGuid(),JournalId=Guid.NewGuid(),TaxId =17,Amount = Convert.ToDecimal("1.00"),TaxAmount =Convert.ToDecimal("0.00") ,TotalAmount =Convert.ToDecimal("0.00") }
    //        //};
    //        return journalmodule;
    //    }
       
    //}
}
