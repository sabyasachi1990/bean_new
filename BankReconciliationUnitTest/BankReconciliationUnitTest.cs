using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System;
using AppsWorld.BankReconciliationModule.Models;
using AppsWorld.Framework;

namespace BankReconciliationUnitTest
{
    //[TestClass]
    //public class BankReconciliationUnitTest
    //{
    //    string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();

    //    #region Get_All_Bank_Reconciliation

    //    [TestMethod]
    //    public void getallbankreconciliationsK_ByComapanyId_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "203" });
    //        var requestUrl = CommonConstant.GetBankReconciliationUrl + "getallbankreconciliationsK";
    //        var BankRecon = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(BankRecon.StatusCode, HttpStatusCode.OK);
    //    }

    //    [TestMethod]
    //    public void getallbankreconciliationsK_ByComapanyId_NegtiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "sai" });
    //        var requestUrl = CommonConstant.GetBankReconciliationUrl + "getallbalancessk";
    //        var BankRecon = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(BankRecon.StatusCode, HttpStatusCode.OK);
    //    }
    //    #endregion

    //    #region bank_Reconciliationllu

    //    [TestMethod]
    //    public void bankreconciliationllu_ByComapanyId_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "CompanyId", Value = "15" });
    //        var requestUrl = CommonConstant.GetBankReconciliationUrl + "bankreconciliationllu";
    //        var BankRecon = RestHelper.ZGetK(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(BankRecon.StatusCode, HttpStatusCode.OK);
    //    }

    //    [TestMethod]
    //    public void bankreconciliationllu_ByComapanyId_NegtiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "CompanyId", Value = "o0" });
    //        var requestUrl = CommonConstant.GetBankReconciliationUrl + "bankreconciliationllu";
    //        var BankRecon = RestHelper.ZGetK(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(BankRecon.StatusCode, HttpStatusCode.BadRequest);
    //    }


    //    #endregion

    //    #region Getclearingtransaction
    //    [TestMethod]
    //    public void Getclearingtransaction_PositiveCase()
    //    {
    //        List<List<string, string>> Parameters = new List<List<string, string>>();
    //        Parameters.Add(new List<string, string>() { Name = "companyId", Value = "203" });
    //        Parameters.Add(new List<string, string>() { Name = "subcompanyId", Value = "204" });
    //        Parameters.Add(new List<string, string>() { Name = "chartid", Value = "12747" });
    //        Parameters.Add(new List<string, string>() { Name = "fromDate", Value = "null" });
    //        Parameters.Add(new List<string, string>() { Name = "toDate", Value = "2017-01-18%2000:00:00.0000000" });
    //        var requestUrl = CommonConstant.GetBankReconciliationUrl + "getclearingtransaction";
    //        var BankRecon = RestHelper.ZGet(UnitTestUrl, requestUrl, Parameters);
    //        Assert.AreEqual(BankRecon.StatusCode, HttpStatusCode.OK);
    //    }

    //    [TestMethod]
    //    public void Getclearingtransaction_NegtiveCase()
    //    {
    //        List<List<string, string>> Parameters = new List<List<string, string>>();
    //        Parameters.Add(new List<string, string>() { Name = "companyId", Value = "203" });
    //        Parameters.Add(new List<string, string>() { Name = "subcompanyId", Value = "204" });
    //        Parameters.Add(new List<string, string>() { Name = "chartid", Value = "127470" });
    //        Parameters.Add(new List<string, string>() { Name = "fromDate", Value = "null" });
    //        Parameters.Add(new List<string, string>() { Name = "toDate", Value = "2017-01-60" });
    //        var requestUrl = CommonConstant.GetBankReconciliationUrl + "getclearingtransaction";
    //        var BankRecon = RestHelper.ZGet(UnitTestUrl, requestUrl, Parameters);
    //        Assert.AreEqual(BankRecon.StatusCode, HttpStatusCode.OK);
    //    }

    //    #endregion

    //    #region Createbankreconciliation
    //    [TestMethod]
    //    public void Createbankreconciliation_NegtiveCase()
    //    {
    //        List<List<string, string>> Parameters = new List<List<string, string>>();
    //        Parameters.Add(new List<string, string>() { Name = "id", Value = "00000000-0000-0000-0000-000000000000" });
    //        Parameters.Add(new List<string, string>() { Name = "companyId", Value = "203" });
    //        Parameters.Add(new List<string, string>() { Name = "subcompanyId", Value = "204" });
    //        Parameters.Add(new List<string, string>() { Name = "chartid", Value = "12747" });
    //        Parameters.Add(new List<string, string>() { Name = "fromDate", Value = "null" });
    //        Parameters.Add(new List<string, string>() { Name = "toDate", Value = "2017-01-18%2000:00:00.0000000" });
    //        var requestUrl = CommonConstant.GetBankReconciliationUrl + "createbankreconciliation";
    //        var bankrec = RestHelper.ZGet(UnitTestUrl, requestUrl, Parameters);
    //        Assert.AreEqual(bankrec.StatusCode, HttpStatusCode.OK);
    //    }

    //    [TestMethod]
    //    public void Createbankreconciliation_PositiveCase()
    //    {
    //        List<List<string, string>> Parameters = new List<List<string, string>>();
    //        Parameters.Add(new List<string, string>() { Name = "id", Value = "00000000-000-0000-0000-000000000000" });
    //        Parameters.Add(new List<string, string>() { Name = "companyId", Value = "203" });
    //        Parameters.Add(new List<string, string>() { Name = "subcompanyId", Value = "204" });
    //        Parameters.Add(new List<string, string>() { Name = "chartid", Value = "12747" });
    //        Parameters.Add(new List<string, string>() { Name = "fromDate", Value = "null" });
    //        Parameters.Add(new List<string, string>() { Name = "toDate", Value = "2017-01-18%2000:00:00.0000000" });
    //        var requestUrl = CommonConstant.GetBankReconciliationUrl + "createbankreconciliation";
    //        var bankrec = RestHelper.ZGet(UnitTestUrl, requestUrl, Parameters);
    //        Assert.AreEqual(bankrec.StatusCode, HttpStatusCode.OK);
    //    }

    //    #endregion

    //    #region GetclearingRec

    //    [TestMethod]
    //    public void GetclearingRec_PositiveCase()
    //    {
    //        List<List<string, string>> Parameters = new List<List<string, string>>();
    //        Parameters.Add(new List<string, string>() { Name = "id", Value = "00000000-0000-0000-0000-000000000000" });
    //        Parameters.Add(new List<string, string>() { Name = "companyId", Value = "203" });
    //        Parameters.Add(new List<string, string>() { Name = "subcompanyId", Value = "204" });
    //        Parameters.Add(new List<string, string>() { Name = "chartid", Value = "12747" });
    //        Parameters.Add(new List<string, string>() { Name = "fromDate", Value = "null" });
    //        Parameters.Add(new List<string, string>() { Name = "toDate", Value = "2017-01-18%2000:00:00.0000000" });
    //        var requestUrl = CommonConstant.GetBankReconciliationUrl + "getclearingrec";
    //        var bankr = RestHelper.ZGet(UnitTestUrl, requestUrl, Parameters);
    //        Assert.AreEqual(bankr.StatusCode, HttpStatusCode.OK);
    //    }

    //    [TestMethod]
    //    public void GetclearingRec_NegtiveCase()
    //    {
    //        List<List<string, string>> Parameters = new List<List<string, string>>();
    //        Parameters.Add(new List<string, string>() { Name = "id", Value = "00000000-0000-000-0000-000000000000" });
    //        Parameters.Add(new List<string, string>() { Name = "companyId", Value = "203" });
    //        Parameters.Add(new List<string, string>() { Name = "subcompanyId", Value = "204" });
    //        Parameters.Add(new List<string, string>() { Name = "chartid", Value = "12747" });
    //        Parameters.Add(new List<string, string>() { Name = "fromDate", Value = "null" });
    //        Parameters.Add(new List<string, string>() { Name = "toDate", Value = "2017-01-18%2000:00:00.0000000" });
    //        var requestUrl = CommonConstant.GetBankReconciliationUrl + "getclearingrec";
    //        var bankr = RestHelper.ZGet(UnitTestUrl, requestUrl, Parameters);
    //        Assert.AreEqual(bankr.StatusCode, HttpStatusCode.BadRequest);

    //    }

    //    #endregion

    //    #region Savebankreconciliation
    //    [TestMethod]
    //    public void savebankreconciliation_Positive()
    //    {
    //        BankReconciliationModel bankreconciliation = new BankReconciliationModel();
    //        bankreconciliation = FillBankReconciliationDetail_Positive(bankreconciliation);
    //        var json = RestHelper.ConvertObjectToJason(bankreconciliation);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetBankReconciliationUrl + "Savebankreconciliation", json);
    //        Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

    //    }

    //    [TestMethod]
    //    public void savebankreconciliation_Negitive()
    //    {
    //        BankReconciliationModel bankreconciliation = new BankReconciliationModel();
    //        bankreconciliation = FillBankReconciliationDetail_Negative(bankreconciliation);
    //        var json = RestHelper.ConvertObjectToJason(bankreconciliation);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetBankReconciliationUrl + "Savebankreconciliation", json);
    //        Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

    //    }

    //    [TestMethod]
    //    public void savebankreconciliation_Positive_Update()
    //    {
    //        BankReconciliationModel bankreconciliation = new BankReconciliationModel();
    //        bankreconciliation = FillBankReconciliationDetail_Positive_Update(bankreconciliation);
    //        var json = RestHelper.ConvertObjectToJason(bankreconciliation);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetBankReconciliationUrl + "Savebankreconciliation", json);
    //        Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

    //    }



    //    #region Private
    //    public BankReconciliationModel FillBankReconciliationDetail_Positive(BankReconciliationModel br)
    //    {
    //        br.Id = Guid.NewGuid();
    //        br.CompanyId = 203;
    //        br.ServiceCompanyId = 204;
    //        br.BankReconciliationDate = DateTime.UtcNow;
    //        br.COAId = 12747;
    //        br.Currency = "USD";
    //        br.BankAccount = "Suresh";
    //        br.StatementAmount = 200;
    //        //br.OutstandingDeposits = 200;
    //        //br.OutstandingWithdrawals = 300;
    //        br.SubTotal = 200;
    //        br.StatementExpectedAmount = 200;
    //        br.GLAmount = 200;
    //        br.State = "Paid";
    //        br.Status = RecordStatusEnum.Active;
    //        br.UserCreated = "Saikiran@ziraff.in";
    //        br.CreatedDate = DateTime.UtcNow;
    //        br.BankReconciliationDetailModels = new List<BankReconciliationDetailModel>{ new BankReconciliationDetailModel {
    //        Id = Guid.NewGuid(),
    //        BankReconciliationId = br.Id,
    //        ClearingDate = DateTime.UtcNow,
    //        DocumentDate = DateTime.UtcNow,
    //        DocumentType = "Deposit",
    //        DocRefNo = "UOB-001",
    //        //RefNo = "001",
    //        EntityId = Guid.Parse("316FBED0-9B57-A893-7DF4-B2BD304B1389"),
    //        ClearingStatus = "Cleared",
    //        DocumentId =Guid.Parse("49DF162F-10C3-4735-AD17-70B9C06704A3"),
    //        isWithdrawl = true,
    //        Ammount=2000
    //        } };

    //        return br;
    //    }

    //    private BankReconciliationModel FillBankReconciliationDetail_Negative(BankReconciliationModel br)
    //    {
    //        br.Id = Guid.NewGuid();
    //        br.CompanyId = 203;
    //        br.ServiceCompanyId = 204;
    //        br.BankReconciliationDate = DateTime.UtcNow;
    //        br.COAId = 12747;
    //        br.Currency = "";
    //        br.BankAccount = "Suresh";
    //        br.StatementAmount = 200;
    //        //br.OutstandingDeposits = 200;
    //        //br.OutstandingWithdrawals = 300;
    //        br.SubTotal = 200;
    //        br.StatementExpectedAmount = 200;
    //        br.GLAmount = 200;
    //        br.State = "Paid";
    //        br.Status = RecordStatusEnum.Active;
    //        br.UserCreated = "Saikiran@ziraff.in";
    //        br.CreatedDate = DateTime.UtcNow;
    //        br.BankReconciliationDetailModels = new List<BankReconciliationDetailModel>{ new BankReconciliationDetailModel {
    //        Id = Guid.NewGuid(),
    //        BankReconciliationId = br.Id,
    //        ClearingDate = DateTime.UtcNow,
    //        DocumentDate = DateTime.UtcNow,
    //        DocumentType = "Deposit",
    //        DocRefNo = "UOB-001",
    //        //RefNo = "001",
    //        EntityId = Guid.Parse("316FBED0-9B57-A893-7DF4-B2BD304B138"),
    //        ClearingStatus = "Cleared",
    //        DocumentId =Guid.Parse("49DF162F-10C3-4735-AD17-70B9C06704A3"),
    //        isWithdrawl = true,
    //        Ammount=2000
    //        } };

    //        return br;
    //    }

    //    public BankReconciliationModel FillBankReconciliationDetail_Positive_Update(BankReconciliationModel br)
    //    {
    //        br.Id = Guid.Parse("B6811E40-6C46-4739-B474-8D9C44B2F6D5");
    //        br.CompanyId = 203;
    //        br.ServiceCompanyId = 204;
    //        br.BankReconciliationDate = DateTime.UtcNow;
    //        br.COAId = 12747;
    //        br.Currency = "USD";
    //        br.BankAccount = "Suresh";
    //        br.StatementAmount = 200;
    //        //br.OutstandingDeposits = 200;
    //        //br.OutstandingWithdrawals = 300;
    //        br.SubTotal = 200;
    //        br.StatementExpectedAmount = 200;
    //        br.GLAmount = 200;
    //        br.State = "Paid";
    //        br.Status = RecordStatusEnum.Active;
    //        br.UserCreated = "Saikiran@ziraff.in";
    //        br.CreatedDate = DateTime.UtcNow;
    //        br.BankReconciliationDetailModels = new List<BankReconciliationDetailModel>{ new BankReconciliationDetailModel {
    //        Id = Guid.Parse("B4C5BC82-CD2D-48CD-892A-0B1D99676F84"),
    //        BankReconciliationId = br.Id,
    //        ClearingDate = DateTime.UtcNow,
    //        DocumentDate = DateTime.UtcNow,
    //        DocumentType = "Deposit",
    //        DocRefNo = "UOB-001",
    //        //RefNo = "001",
    //        EntityId = Guid.Parse("316FBED0-9B57-A893-7DF4-B2BD304B1389"),
    //        ClearingStatus = "Cleared",
    //        DocumentId =Guid.Parse("49DF162F-10C3-4735-AD17-70B9C06704A3"),
    //        isWithdrawl = true,
    //        Ammount=2000
    //        } };

    //        return br;
    //    }


    //    #endregion
    //    #endregion
    //}
}
