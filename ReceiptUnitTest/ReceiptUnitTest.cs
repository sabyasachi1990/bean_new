using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppsWorld.ReceiptModule.Models;
using ReceiptUnitTest.CommonResource;
using System.Configuration;
using System.Net;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.Framework;
using System.Collections.Generic;
namespace ReceiptUnitTest
{
    //[TestClass]
    //public class ReceiptUnitTest
    //{
    //    string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();

    //    [TestMethod]

    //    public void Save()
    //    {
    //        ReceiptModel custreceipt = new ReceiptModel();
    //        custreceipt = Fillreceiptmodule(custreceipt);
    //        var json = RestHelper.ConvertObjectToJason(custreceipt);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonContest.ApiReceptUrl + "savereceipt", json);
    //        Assert.AreEqual(response, HttpStatusCode.OK);
    //    }
    //    private ReceiptModel Fillreceiptmodule(ReceiptModel receiptmodule)
    //    {
    //        //receiptmodule.EntityModels = new EntityModel {EntityId=Guid.NewGuid(),EntityName="fgdfgd" };

    //        receiptmodule.DocDate = DateTime.UtcNow;
    //        //receiptmodule.DocDate = DateTime.UtcNow;

    //        receiptmodule.DocNo = "RC_2015-07-888";


    //        //receiptmodule.COAModels = new COAModel { COAId = 101, COAName = "user" };           
    //        receiptmodule.Id = Guid.NewGuid();
    //        receiptmodule.CompanyId = 4;
    //        //receiptmodule.DocDate=Convert.ToDateTime("12-05-2016");
    //        receiptmodule.ReceiptRefNo = "TT9721616";
    //        //receiptmodule.Status = RecordStatusEnum.Active;
    //        receiptmodule.Remarks = "Receipt from Cust Z";
    //        receiptmodule.IsNoSupportingDocument = true;
    //        receiptmodule.IsGstSettings = true;
    //        receiptmodule.ISMultiCurrency = true;
    //        //receiptmodule.IsDisAllow = true;
    //        //receiptmodule.IsAllowDisAllow = false;
    //        receiptmodule.PeriodLockPassword="54545454";
    //        receiptmodule.Status = RecordStatusEnum.Active;
    //        receiptmodule.GrandTotal = 1000;
    //        receiptmodule.BankReceiptAmmount = Convert.ToDecimal("4234");
    //        receiptmodule.EntityId = Guid.NewGuid(); ;
    //        receiptmodule.ServiceCompanyId = 2;
    //        //receiptmodule.ReceiptBalancingItems= df;
    //        receiptmodule.ISGstDeRegistered = false;
    //        receiptmodule.ExchangeRate = Convert.ToDecimal("23");
    //        receiptmodule.FinancialPeriodLockStartDate = Convert.ToDateTime("2016-06-21 04:25:28");
    //        receiptmodule.FinancialPeriodLockEndDate = Convert.ToDateTime("2016-06-24 00:00:00");
    //        //receiptmodule.FinancialPeriodLockEndDate = Convert.ToDateTime("2016-12-07 00:00:00");
    //        //receiptmodule.FinancialPeriodLockStartDate = Convert.ToDateTime("2016-05-27 00:00:00");
    //        receiptmodule.BankReceiptAmmountCurrency = "1500";
    //        receiptmodule.BankChargesCurrency = "23423";
    //        receiptmodule.UserCreated = "Created";
    //        receiptmodule.DocCurrency = "SUD";
          

    //        ModeOfReceiptModel modeOfReceiptModel = new ModeOfReceiptModel();
    //        modeOfReceiptModel.Name = "Cash";
    //        //receiptmodule.ModeOfReceiptModels = modeOfReceiptModel;
    //        receiptmodule.ReceiptBalancingItems = new List<ReceiptBalancingItem>(){
    //            new ReceiptBalancingItem{Status=RecordStatusEnum.Active,DocAmount=Convert.ToDecimal("9999"),TaxId=12,ReciveORPay="Pay",Account="Savings"}
    //        };
    //        receiptmodule.ReceiptDetailModels = new List<ReceiptDetailModel>()
    //        {
    //            new ReceiptDetailModel{ ReceiptAmount=Convert.ToDecimal("2434"),Currency="SUD"}
    //        };
    //        //receiptmodule.ReceiptGSTDetails = new List<ReceiptGSTDetail>(){
    //        //new ReceiptGSTDetail { Id=Guid.NewGuid(),ReceiptId=Guid.NewGuid(),TaxId=1,TaxCode="sfwer342424",}
    //        //};
    //        //receiptmodule.ValidateFinancialOpenPeriod=new List<FinancialSetting>(){
            
    //        //}

    //        //new List<ReceiptDetail>{new ReceiptDetail{ }}
    //        return receiptmodule;

    //        //receiptmodule.Remarks = Convert.ToDateTime("12-01-2016");
    //        //receiptmodule.IsNoSupportingDocument = true;
    //        //receiptmodule.IsGstSettings = true;
    //        //receiptmodule.IsMultiCurrency = true;
    //        //receiptmodule.IsSegmentReporting = true;
    //        //receiptmodule.IsAllowableNonAllowable = false;
    //        //receiptmodule.Status = RecordStatusEnum.Active;
    //        //receiptmodule.GrandTotal = 10;
    //        //billModel.SystemReferenceNumber = "123";
    //        //billModel.DocumentState = "Active";
    //        //billModel.Nature = "sdfdsf";
    //        //billModel.DueDate = Convert.ToDateTime("01-12-2016");
    //        //billModel.ServiceCompany.ServiceCompanyId = 1;
    //        //billModel.CreatedDate = Convert.ToDateTime("01-12-2016");
    //        //billModel.IsNoSupportingDocument = true;
    //        //billModel.IsGstSettings = false;
    //        //billModel.ISMultiCurrency = true;
    //        //billModel.ISSegmentReporting = true;
    //        //billModel.ISAllowDisAllow = true;
    //        //billModel.IsGSTCurrencyRateChanged = false;
    //        //billModel.IsBaseCurrencyRateChanged = true;
    //        //billModel.ISGstDeRegistered = false;
    //        //billModel.Status = RecordStatusEnum.Active;
    //        //billModel.PostingDate = Convert.ToDateTime("10-06-2016");
    //        //billModel.CreditTerm.CreditTermId = 1;
    //        //billModel.CompanyId = 1;
    //        //billModel.GrandTotal = Convert.ToDecimal("10.253");

    //    }
    //}
}
