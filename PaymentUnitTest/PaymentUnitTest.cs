using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppsWorld.PaymentModule.Models;
using AppsWorld.Framework;
using System.Net;
using AppsWorld.CommonModule.Models;

namespace PaymentUnitTest
{
    //[TestClass]
    //public class PaymentUnitTest
    //{
    //    string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();

    //    #region GetAllPaymentK
    //    [TestMethod]
    //    public void GetAllPaymentK_ByComapanyId_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "getallpaymentsk";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void GetAllPaymentK_ByComapanyId_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "sdfgs" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "getallpaymentsk";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion

    //    #region GetAllPaymentLookups
    //    [TestMethod]
    //    public void GetAllPaymentLookups_ByCompanyId_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "id", Value = "B91A424D-1103-4E89-B8C0-4CE937E1DD1A" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "paymentslu";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void GetAllPaymentLookups_ByCompanyId_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "gssvj" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "paymentslu";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void GetCurrencyLookup_ByEntityId_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "entityId", Value = "9ABCD3D2-A327-4F20-B7F2-CC39D8D1F02F" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "getcurrencylu";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void GetCurrencyLookup_ByEntityId_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "entityId",Value="sjsj"});
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "getcurrencylu";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    #endregion

    //    [TestMethod]
    //    public void CreatePayment_ById_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "paymentId", Value = "9ABCD3D2-A327-4F20-B7F2-CC39D8D1F02F" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "createpayment";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void CreatePayment_ById_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "paymentId", Value = null });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "createpayment";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.IsNull(Payment.StatusCode);
    //    }
    //    [TestMethod]
    //    public void SavePayment()
    //    {
    //        PaymentModel custpayment= new PaymentModel();
    //      //  custpayment = FillPaymentModule(custpayment);
    //        var json = RestHelper.ConvertObjectToJason(custpayment);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetPaymentUrl + "savepayment", json);
    //        Assert.IsNotNull(response);
    //    }
    //    //public PaymentModel FillPaymentModule(PaymentModel paymentmodule)
    //    //{
    //    //    paymentmodule.COAModels = new COAModel { COAId=101,COAName="Check"};   
    //    //    paymentmodule.CurrencyModels=new CurrencyModel{CurrencyCode="CC_1",CurrencyName="Singapur Doller"};
    //    //    paymentmodule.CreditTermsModels=new CreditTermModel{CreditTermId=12, CreditTermName="CT_Payment",TOPValue=12};
    //    //    paymentmodule.EntityModels=new EntityModel{EntityId=Guid.NewGuid() ,EntityName="Bank",EntityType="COA"};
    //    //    paymentmodule.ModeOfPaymentModels=new ModeOfPaymentModel{code="12",Name="Hai"};
    //    //    paymentmodule.ServiceCompanyMOdels=new ServiceCompanyModel{ServiceCompanyId=1264,ServiceCompanyName="Vector"};
    //    //    paymentmodule.Id = Guid.Parse("587B654E-A27B-4612-B7FA-5104AAE9393C");
    //    //    paymentmodule.CompanyId=3;
    //    //    paymentmodule.DocSubType="Doc_Sub_1";
    //    //    paymentmodule.SystemRefNo="SYS_1";
    //    //    //paymentmodule.EntityType=;
    //    //    //paymentmodule.EntityId=;
    //    //    paymentmodule.DocDate=Convert.ToDateTime("2016-09-23 05:30:00.000");
    //    //    paymentmodule.DueDate=Convert.ToDateTime("2016-12-01 02:25:09.000");
    //    //    paymentmodule.DocNo="Doc_123";
    //    //    paymentmodule.ModeOfPayment="Cash";
    //    //    paymentmodule.BankPaymentAmmountCurrency="UDS";
    //    //    paymentmodule.BankPaymentAmmount=125500;
    //    //    paymentmodule.BankChargesCurrency="USD";
    //    //    paymentmodule.ISMultiCurrency=true;
    //    //    paymentmodule.IsAllowDisAllow=true;
    //    //    paymentmodule.IsDisAllow=false;
    //    //    paymentmodule.GrandTotal=52000;
    //    //    paymentmodule.DocCurrency="SUD";
    //    //    paymentmodule.BaseCurrency="SUD";
    //    //    paymentmodule.Status = RecordStatusEnum.Active;
    //    //    paymentmodule.PaymentDetailModels = new List<PaymentDetailModel>()
    //    //    {
    //    //        new PaymentDetailModel {Id=Guid.Parse("1524D158-B1B5-4716-8E83-BB91197C9818"),PaymentId=Guid.Parse("587B654E-A27B-4612-B7FA-5104AAE9393C"),DocumentDate=Convert.ToDateTime("2015-08-03 02:53:12.0000000"),DocumentType="Payment Bill",SystemReferenceNumber="RC_1264",DocumentNo="Doc_369",DocumentState="Completed",Nature="Average",DocumentAmmount=5300,Currency="USD",PaymentAmount=6320,DocumentId=Guid.Parse("41161FE8-BE0D-4088-B551-57442402DF3C")}
            
    //    //    };
    //    //    return paymentmodule;    
    //    //}
    //    [TestMethod]
    //    public void SaveCreditNoteDocumentVoid()
    //    {
    //        DocumentVoidModel custvoidpayment = new DocumentVoidModel();
    //        custvoidpayment = FillVoidPaymentModule(custvoidpayment);
    //        var json = RestHelper.ConvertObjectToJason(custvoidpayment);
    //        var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetPaymentUrl + "savepaymentdocumentvoid", json);
    //        Assert.IsNotNull(response);
    //    }
    //    public DocumentVoidModel FillVoidPaymentModule(DocumentVoidModel documentvoidmodel)
    //    {
    //        documentvoidmodel.Id = Guid.NewGuid();
    //        documentvoidmodel.CompanyId = 3;
    //        documentvoidmodel.PeriodLockPassword = "123456";
    //        return documentvoidmodel;
    //    }
    //    [TestMethod]
    //    public void GetPaymentDetails_ById_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "receiptId", Value = "B91A424D-1103-4E89-B8C0-4CE937E1DD1A" });
    //        lstParameter.Add(new List<string, string>() { Name = "entityId", Value = "FEDC00F7-528E-FFEF-B69D-07D4D2BD1A5B" });
    //        lstParameter.Add(new List<string, string>() { Name = "currency", Value = "INR" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        lstParameter.Add(new List<string, string>() { Name = "serviceCompanyId", Value = "11" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "getpaymentdetails";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void GetPaymentDetails_ById_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "paymentId", Value = "dfdfdsdfs" });
    //        lstParameter.Add(new List<string, string>() { Name = "entityId", Value = "FEDC00F7-528E-FFEF-B69D-07D4D2BD1A5B" });
    //        lstParameter.Add(new List<string, string>() { Name = "currency", Value = "INR" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        lstParameter.Add(new List<string, string>() { Name = "serviceCompanyId", Value = "11" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "getpaymentdetails";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void CreatePaymentDetails_ById_PositiveCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "paymentId", Value = "B91A424D-1103-4E89-B8C0-4CE937E1DD1A" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "createpaymentdetails";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //    [TestMethod]
    //    public void CreatePaymentDetails_ById_NegativeCase()
    //    {
    //        List<List<string, string>> lstParameter = new List<List<string, string>>();
    //        lstParameter.Add(new List<string, string>() { Name = "paymentId", Value = "2342323" });
    //        lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "3" });
    //        var requestUrl = CommonConstant.GetPaymentUrl + "createpaymentdetails";
    //        var Payment = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
    //        Assert.AreNotEqual(Payment.StatusCode, HttpStatusCode.BadRequest);
    //    }
    //}
}
