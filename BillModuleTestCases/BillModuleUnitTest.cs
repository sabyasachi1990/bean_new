using System;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrameWork;
using System.Net;
using AppsWorld.BillModule.Models;
using AppsWorld.Framework;

namespace BillModuleTestCases
{
    //[TestClass]
    //public class BillModuleUnitTest
    //{
    //    string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();

    //    #region GetAllBills
        
    //    [TestMethod]
    //    public void GetAllBills_ByCompanyId_ValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBillurl + "bill";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(lstEmployees);
    //    }

    //    [TestMethod]
    //    public void GetAllBills_ByCompanyId_InValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "dfgdfgdfg" });
    //        var requestUrl = CommonConstant.GetBillurl + "bill";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(lstEmployees.StatusCode, HttpStatusCode.BadRequest);
    //    }

    //    #endregion

    //    #region Createbil
    //    [TestMethod]
    //    public void Createbil_GetByID_ValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "4248178A-2985-4AF1-8408-6BF086E1AF16" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "15" });
    //        var requestUrl = CommonConstant.GetBillurl + "createbill";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(lstParameter);

    //    }
    //    [TestMethod]
    //    public void Createbil_GetById_NegitiveTest()
    //    {

    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "strinfds" });
    //        var requestUrl = CommonConstant.GetBillurl + "createbill";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNull(Employees.StatusCode);

    //    }
    //    #endregion

    //    #region GetAllBillLU
    //    [TestMethod]
    //    public void GetAllBillLU_GetById_Validtest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "billId", Value = "BA133852-9C0A-4E67-B9A1-D3D418BE0F70" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "15" });
    //        var requestUrl = CommonConstant.GetBillurl + "billalllu";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(Employees.StatusCode);
    //    }



    //    [TestMethod]
    //    public void GetAllBillLU_GetById_InValidtest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "billId", Value = "dsfdsfdsfdsfds" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBillurl + "billalllu";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(Employees.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion

    //    #region GetAllBillDetailModel
    //    [TestMethod]
    //    public void GetAllBillDetailModel_GetById_ValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "billId", Value = "B2C41DFF-208F-44DB-B608-186B4F98F1A0" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "15" });
    //        var requestUrl = CommonConstant.GetBillurl + "billalllu";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(Employees.StatusCode);
    //    }



    //    [TestMethod]
    //    public void GetAllBillDetailModel_GetById_InValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "billId", Value = "stringsfs" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBillurl + "billalllu";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(Employees.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion

    //    #region GetGstBillDetailById
    //    [TestMethod]
    //    public void GetGstBillDetailById_GetById_ValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "Id", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "billId", Value = "gdgdsdf" });
    //        var requestUrl = CommonConstant.GetBillurl + "createbillgstdetail";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(Employees.StatusCode);
    //    }

    //    [TestMethod]
    //    public void GetGstBillDetailById_GetById_InValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "Id", Value = "gdfgd" });
    //        lstParameter.Add(new List<string, string>() { Name = "billId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBillurl + "createbillgstdetail";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNull(Employees.StatusCode);
    //    }
    //    #endregion

    //    #region CreateCreditNoteDocumentVoid
    //    [TestMethod]
    //    public void CreateCreditNoteDocumentVoid_GetById_ValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "Id", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBillurl + "createbilldocumentvoid";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(Employees.StatusCode);
    //    }

    //    [TestMethod]
    //    public void CreateCreditNoteDocumentVoid_GetById_InValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "Id", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBillurl + "createbilldocumentvoid";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNull(Employees.StatusCode);
    //    }
    //    #endregion

    //    #region VendorLu
    //    [TestMethod]
    //    public void VendorLu_GetById_ValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "entityId", Value = "1" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "fgfghffk" });
    //        var requestUrl = CommonConstant.GetBillurl + "vendorlu";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreEqual(Employees.StatusCode,HttpStatusCode.BadRequest);
    //    }

    //    [TestMethod]
    //    public void VendorLu_GetById_InValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "entityId", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBillurl + "vendorlu";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNull(Employees.StatusCode);
    //    }
    //    #endregion

    //    #region GetAllBillsK
    //    [TestMethod]
    //    public void GetAllBillsK_GetById_ValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBillurl + "GetAllBillsK";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Employees.StatusCode, HttpStatusCode.BadRequest);
    //    }

    //    [TestMethod]
    //    public void GetAllBillsK_GetById_InValidTest()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //        var requestUrl = CommonConstant.GetBillurl + "GetAllBillsK";
    //        var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNull(Employees.StatusCode);
    //    }
    //    #endregion

    //    #region GetAllBillssK
        
    //    //[TestMethod]
    //    //public void GetAllBillssK_GetById_ValidTest()
    //    //{
    //    //    List<List<string, string>> lstParameter = new List<List<string, string>>();
    //    //    lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}", Value = "1" });
    //    //    lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "1" });
    //    //    var requestUrl = CommonConstant.GetBillurl + "GetAllBillssK";
    //    //    var Employees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //    //    Assert.IsNull(Employees.StatusCode);
    //    //}
        
    //    #endregion
        
    //    #region IsGSTAllowed
    //    [TestMethod]
    //    public void IsGSTAllowed_ById_validModel()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "docDate", Value = "01-02-2016" });
    //        var requestUrl = CommonConstant.GetBillurl + "isgstallowed";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNotNull(lstEmployees.StatusCode);
    //    }

    //    [TestMethod]
    //    public void IsGSTAllowed_ById_InvalidModel()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "kdfjlskfs" });
    //        lstParameter.Add(new List<string, string>() { Name = "docDate", Value = "01-02-2016" });
    //        var requestUrl = CommonConstant.GetBillurl + "isgstallowed";
    //        var lstEmployees = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.Equals(lstEmployees.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion

    //    #region SaveBill
    //    //[TestMethod]

    //    //public void SaveBill()
    //    //{
    //    //    var bill = new BillModel();
    //    //    bill = FillBillModel(bill);
    //    //    var json = RestHelper.ConvertObjectToJason(bill);
    //    //    var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetBillurl + "savebill", json);
    //    //    Assert.IsNotNull(response);
    //    //}
    //    //private BillModel FillBillModel(BillModel billModel)
    //    //{
    //    //    billModel.Id = System.Guid.NewGuid();
    //    //    billModel.DocNo = "1213";
    //    //    billModel.SystemReferenceNumber = "123";
    //    //    billModel.DocumentState = "Active";
    //    //    billModel.Nature = "sdfdsf";
    //    //    billModel.DueDate = Convert.ToDateTime("01-12-2016");
    //    //    billModel.ServiceCompany.ServiceCompanyId = 1;
    //    //    billModel.CreatedDate = Convert.ToDateTime("01-12-2016");
    //    //    billModel.IsNoSupportingDocument = true;
    //    //    billModel.IsGstSettings = false;
    //    //    billModel.ISMultiCurrency = true;
    //    //    billModel.ISSegmentReporting = true;
    //    //    billModel.ISAllowDisAllow = true;
    //    //    billModel.IsGSTCurrencyRateChanged = false;
    //    //    billModel.IsBaseCurrencyRateChanged = true;
    //    //    billModel.ISGstDeRegistered = false;
    //    //    billModel.Status = RecordStatusEnum.Active;
    //    //    billModel.PostingDate = Convert.ToDateTime("10-06-2016");
    //    //    billModel.CreditTerm.CreditTermId = 1;
    //    //    billModel.CompanyId = 1;
    //    //    billModel.GrandTotal = Convert.ToDecimal("10.253");
    //    //    return billModel;
    //    //}
    //    //#endregion

    //    //#region SaveCreditNoteDocumentVoid
    //    //[TestMethod]
    //    //public void SaveCreditNoteDocumentVoid()
    //    //{
    //    //    var savecreditbill = new DocumentVoidModel();
    //    //    savecreditbill = SavebillModel(savecreditbill);
    //    //    var json = RestHelper.ConvertObjectToJason(savecreditbill);
    //    //    var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetBillurl + "savebilldocumentvoid", json);
    //    //    Assert.IsNotNull(response);
    //    //}


    //    //private DocumentVoidModel SavebillModel(DocumentVoidModel Docmodel)
    //    //{
    //    //    Docmodel.Id = System.Guid.NewGuid();
    //    //    Docmodel.CompanyId = 2;
    //    //    Docmodel.PeriodLockPassword = "Suresh.1264";
    //    //    return Docmodel;

    //    //}
    //    #endregion

        
        
    //}
}
