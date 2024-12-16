//using AppsWorld.CommonModule.Models;
//using AppsWorld.CreditMemoModule.Models;
//using CreditMemoUnitTest.CommonResource;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace CreditMemoUnitTest
//{
//    [TestClass]
//    public class CreditMemoUnitTest
//    {
//        string UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"].ToString();

//        #region GetCreditMemoK
//        [TestMethod]
//        public void GetAllCreditMemoK_ByCompanyId_PositiveCase()
//        {
//            List<List<string, string>> lstParameter = new List<List<string, string>>();
//            lstParameter.Add(new List<string, string>() { Name = "{take:10,skip:0}" });
//            lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "8" });
//            var requestUrl = CommonConstant.GetCreditMemoUrl + "getallcreditmemok";
//            var creditMemo = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
//            Assert.AreNotEqual(creditMemo.StatusCode, HttpStatusCode.BadRequest);
//        }
//        [TestMethod]
//        public void GetAllCreditMemoK_ByCompanyId_NegativeCase()
//        {
//            List<List<string, string>> lstParameter = new List<List<string, string>>();
//            lstParameter.Add(new List<string, string>() { Name = null });
//            lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "fghjhj" });
//            var requestUrl = CommonConstant.GetCreditMemoUrl + "getallcreditmemok";
//            var creditMemo = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
//            Assert.AreNotEqual(creditMemo.StatusCode, HttpStatusCode.BadRequest);
//        }

//        #endregion

//        #region Create
//        [TestMethod]
//        public void CreateCreditMemo_ById_PositiveCase()
//        {
//            List<List<string, string>> lstParameter = new List<List<string, string>>();
//            lstParameter.Add(new List<string, string>() { Name = "Id", Value = "86D40B07-393E-441A-8598-27066DB9630C" });
//            lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "8" });
//            var requestUrl = CommonConstant.GetCreditMemoUrl + "createcreditmemo";
//            var creditMemo = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
//            Assert.AreNotEqual(creditMemo.StatusCode, HttpStatusCode.BadRequest);
//        }
//        [TestMethod]
//        public void CreateCreditMemo_ById_NegativeCase()
//        {
//            List<List<string, string>> lstParameter = new List<List<string, string>>();
//            lstParameter.Add(new List<string, string>() { Name = "Id", Value = null });
//            lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "9" });
//            var requestUrl = CommonConstant.GetCreditMemoUrl + "createcreditmemo";
//            var creditMemo = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
//            Assert.IsNull(creditMemo.StatusCode);
//        }

//        [TestMethod]
//        public void CreateCreditMemoApplication_ById_PositiveCase()
//        {
//            List<List<string, string>> lstParameter = new List<List<string, string>>();
//            lstParameter.Add(new List<string, string>() { Name = "creditMemoId", Value = "86D40B07-393E-441A-8598-27066DB9630C" });
//            lstParameter.Add(new List<string, string>() { Name = "cmApplicationId", Value = "1EF7DDD2-8B95-4272-BD92-81F469F7CD79" });
//            lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "8" });
//            lstParameter.Add(new List<string, string>() { Name = "isView", Value = "true" });
//            lstParameter.Add(new List<string, string>() { Name = "applicationDate", Value = "2017-03-13 14:17:08.2820000" });
//            var requestUrl = CommonConstant.GetCreditMemoUrl + "createcreditmemoapplication";
//            var creditMemo = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
//            Assert.AreNotEqual(creditMemo.StatusCode, HttpStatusCode.BadRequest);
//        }
//        [TestMethod]
//        public void CreateCreditMemoApplication_ById_NegativeCase()
//        {
//            List<List<string, string>> lstParameter = new List<List<string, string>>();
//            lstParameter.Add(new List<string, string>() { Name = "creditMemoId", Value = "null" });
//            lstParameter.Add(new List<string, string>() { Name = "cmApplicationId", Value = "null" });
//            lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "8" });
//            lstParameter.Add(new List<string, string>() { Name = "isView", Value = "true" });
//            lstParameter.Add(new List<string, string>() { Name = "applicationDate", Value = "2017-03-13 14:17:08.2820000" });
//            var requestUrl = CommonConstant.GetCreditMemoUrl + "createcreditmemoapplication";
//            var creditMemo = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
//            Assert.AreNotEqual(creditMemo.StatusCode, HttpStatusCode.BadRequest);
//        }

//        #endregion

//        #region Void
//        [TestMethod]
//        public void SaveCreditMemoDocumentVoid()
//        {
//            DocumentVoidModel creditMemo = new DocumentVoidModel();
//            creditMemo = FillVoidCreditMemo(creditMemo);
//            var json = RestHelper.ConvertObjectToJason(creditMemo);
//            var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetCreditMemoUrl + "savepaymentdocumentvoid", json);
//            Assert.IsNotNull(response);
//        }
//        public DocumentVoidModel FillVoidCreditMemo(DocumentVoidModel documentvoidmodel)
//        {
//            documentvoidmodel.Id = Guid.NewGuid();
//            documentvoidmodel.CompanyId = 8;
//            documentvoidmodel.PeriodLockPassword = "123456";
//            return documentvoidmodel;
//        }
//        [TestMethod]
//        public void CreateCreditMemoDocumentVoid()
//        {
//            List<List<string, string>> lstParameter = new List<List<string, string>>();
//            lstParameter.Add(new List<string, string>() { Name = "Id", Value = "7F2272C3-B946-42CE-B875-52CC16EDEB89" });
//            lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "8" });
//            var requestUrl = CommonConstant.GetCreditMemoUrl + "createcreditmemodocumentvoid";
//            var creditMemo = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
//            Assert.AreNotEqual(creditMemo.StatusCode, HttpStatusCode.BadRequest);

//        }
//        #endregion Void

//        #region GetAllCreditMemoLookups
//        [TestMethod]
//        public void GetAllCreditMemoLookups_ByCompanyId_PositiveCase()
//        {
//            List<List<string, string>> lstParameter = new List<List<string, string>>();
//            lstParameter.Add(new List<string, string>() { Name = "id", Value = "7F2272C3-B946-42CE-B875-52CC16EDEB89" });
//            lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "17" });
//            var requestUrl = CommonConstant.GetCreditMemoUrl + "getallcreditmemolus";
//            var creditMemo = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
//            Assert.AreNotEqual(creditMemo.StatusCode, HttpStatusCode.BadRequest);
//        }
//        [TestMethod]
//        public void GetAllCreditMemoLookups_ByCompanyId_NagativeCase()
//        {
//            List<List<string, string>> lstParameter = new List<List<string, string>>();
//            lstParameter.Add(new List<string, string>() { Name = null });
//            lstParameter.Add(new List<string, string>() { Name = "companyId", Value = "vgghh" });
//            var requestUrl = CommonConstant.GetCreditMemoUrl + "getallcreditmemolus";
//            var creditMemo = RestHelper.ZGet(UnitTestUrl, requestUrl, lstParameter);
//            Assert.AreNotEqual(creditMemo.StatusCode, HttpStatusCode.BadRequest);
//        }
//        #endregion

//        #region Save
//        [TestMethod]
//        public void SaveCreditMemo()
//        {
//            CreditMemoModel creditMemo = new CreditMemoModel();
//            creditMemo = FillCreditMemo(creditMemo);
//            var json = RestHelper.ConvertObjectToJason(creditMemo);
//            var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetCreditMemoUrl + "savecreditmemo", json);
//            Assert.IsNotNull(response);
//        }

//        public CreditMemoModel FillCreditMemo(CreditMemoModel creditMemoModel)
//        {
//            creditMemoModel.Id = Guid.NewGuid();
//            creditMemoModel.CompanyId = 8;
//            creditMemoModel.BalanceAmount = 1225;
//            creditMemoModel.BaseCurrency = "SGD";
//            creditMemoModel.CreatedDate = Convert.ToDateTime("2017-03-09 08:23:45.3129875");
//            creditMemoModel.CreditMemoNumber = "INV - 2017 - 00003";
//            creditMemoModel.CreditTermsId = 15121032;
//            creditMemoModel.CreditTermsName = "scott";
//            creditMemoModel.DeRegistrationDate = Convert.ToDateTime("2017-03-11 10:56:06.1980000");
//            creditMemoModel.DocCurrency = "SGD";
//            creditMemoModel.DocDate = Convert.ToDateTime("2017-03-10 00:00:00.000");
//            creditMemoModel.DocNo = "9";
//            creditMemoModel.DocSubType = "Credit Memo";
//            creditMemoModel.DocumentState = "Not Paid";
//            creditMemoModel.DueDate = Convert.ToDateTime("2017-03-11 00:00:00.000");
//            creditMemoModel.EntityId = Guid.NewGuid();
//            creditMemoModel.EntityName = "bean";
//            creditMemoModel.EntityType = "Vendor";
//            creditMemoModel.EventStatus = null;
//            creditMemoModel.ExchangeRate = 88;
//            creditMemoModel.ExDurationFrom = null;
//            creditMemoModel.ExDurationTo = null;
//            creditMemoModel.ExtensionType = null;
//           // creditMemoModel.FinancialEndDate = null; 
//            creditMemoModel.FinancialPeriodLockEndDate = null;
//           // creditMemoModel.FinancialStartDate = null;
//            creditMemoModel.FinancialPeriodLockStartDate = null;
//            creditMemoModel.GrandTotal = 45562156;
//            creditMemoModel.GSTExchangeRate = 1132;
//            creditMemoModel.GSTExCurrency = "5212";
//            creditMemoModel.GSTExDurationFrom = null;
//            creditMemoModel.GSTExDurationTo = null;
//            creditMemoModel.GSTTotalAmount = null;
//            creditMemoModel.ModifiedBy = null;
//            creditMemoModel.ModifiedDate = null;
//            creditMemoModel.Nature = "Trade";
//            creditMemoModel.NoSupportingDocument = null;
//            creditMemoModel.ParentInvoiceId = null;
//            //creditMemoModel.SegmentCategory1 = null;
//            //creditMemoModel.SegmentCategory2 = null;
//            //creditMemoModel.SegmentDetailid1 = null;
//            //creditMemoModel.SegmentDetailid2 = null;
//            //creditMemoModel.SegmentMasterid1 = null;
//            //creditMemoModel.SegmentMasterid2 = null;
//          //  creditMemoModel.Status = AppsWorld.Framework.RecordStatusEnum.Active;
//            creditMemoModel.UserCreated = "Malathi@gmail.com";
//            creditMemoModel.ServiceCompanyId = 85;


//            //creditMemoModel.CreditMemoDetailModels = new List<CreditMemoDetailModel>()
//            //{
//            //    new CreditMemoDetailModel {Id=Guid.Parse("267C1CB3-E0FD-4E10-B024-203367DA4F6B"),CreditMemoId=creditMemoModel.Id,Qty=1,Unit="Pieces",UnitPrice=1000,AccountName="Bank1",AllowDisAllow=true,AmtCurrency="SGD",BaseAmount=1000,BaseTaxAmount=null,BaseTotalAmount=null,COAId=123,Discount=0,DiscountType="$",DocAmount=1000,DocTaxAmount=0,DocTotalAmount=0,RecOrder=1,RecordStatus="1",Remarks=null,TaxCode="NA",TaxId=1,TaxIdCode="1234",TaxCurrency="SGD",TaxRate=1213,TaxType="OutPut"

//            //    }
//            //};


//            creditMemoModel.CreditMemoApplicationModels = new List<CreditMemoApplicationModel>()
//            {
//                new CreditMemoApplicationModel {Id=Guid.Parse("1EF7DDD2-8B95-4272-BD92-81F469F7CD79"),CompanyId=8,CreditMemoId=creditMemoModel.Id, DocSubType="Credit Memo",DocDate=Convert.ToDateTime("2017-03-10 00:00:00.000"),DocNo="8",DocCurrency="SGD",CreatedDate=Convert.ToDateTime("2017-03-13 14:17:13.3463945"),CreditAmount=10,CreditMemoAmount=4566,CreditMemoApplicationDate=Convert.ToDateTime("2017-03-13 14:17:08.2820000"),CreditMemoApplicationNumber="CN-2017-00002-A1",CreditMemoApplicationResetDate=null,CreditMemoBalanceAmount=32,FinancialPeriodLockEndDate=null,FinancialPeriodLockStartDate=null, IsNoSupportingDocument=true,ModifiedBy=null,ModifiedDate=null,NoSupportingDocument=null,PeriodLockPassword="123456",Remarks=null,UserCreated="Malathi@gmail.com",

//                  CreditMemoApplicationDetailModels=new List<CreditMemoApplicationDetailModel>()
//                  { 
//                new CreditMemoApplicationDetailModel { Id=Guid.NewGuid(),BalanceAmount=1223,BaseCurrencyExchangeRate=15562,CreditAmount=5451,CreditMemoApplicationId=Guid.Parse("1EF7DDD2-8B95-4272-BD92-81F469F7CD79"),DocAmount=5523,DocCurrency="21022",DocDate=Convert.ToDateTime("2017-03-10 00:00:00.000"),DocNo="9",DocType="Credit Memo",DocumentId=creditMemoModel.Id} } } };
//            return creditMemoModel;
//        }
//        [TestMethod]
//        public void SaveCreditMemoApplication()
//        {
//            CreditMemoApplicationModel creditMemoApp = new CreditMemoApplicationModel();
//            FillCreditMemoApplication(creditMemoApp);
//            var json = RestHelper.ConvertObjectToJason(creditMemoApp);
//            var response = RestHelper.ZPost(UnitTestUrl, CommonConstant.GetCreditMemoUrl + "savecreditmemoapplication", json);
//            Assert.IsNotNull(response);
//        }
//        private void FillCreditMemoApplication(CreditMemoApplicationModel creditMemoApplicationModel)
//        {

//            creditMemoApplicationModel.Id = Guid.Parse("1EF7DDD2-8B95-4272-BD92-81F469F7CD79");
//            creditMemoApplicationModel.CompanyId = 8;
//            creditMemoApplicationModel.CreditMemoId = Guid.Parse("86D40B07-393E-441A-8598-27066DB9630C");
//            creditMemoApplicationModel.DocSubType = "Credit Memo";
//            creditMemoApplicationModel.DocDate = Convert.ToDateTime("2017-03-10 00:00:00.000");
//            creditMemoApplicationModel.DocNo = "Dco-1";
//            creditMemoApplicationModel.DocCurrency = "SGD";
//            creditMemoApplicationModel.CreatedDate = Convert.ToDateTime("2017-03-13 14:17:13.3463945");
//            creditMemoApplicationModel.CreditAmount = 10;
//            creditMemoApplicationModel.CreditMemoAmount = 4566;
//            creditMemoApplicationModel.CreditMemoApplicationDate = Convert.ToDateTime("2017-03-13 14:17:08.2820000");
//            creditMemoApplicationModel.CreditMemoApplicationNumber = "CN-2017-00002-A1";
//            creditMemoApplicationModel.CreditMemoApplicationResetDate = null;
//            creditMemoApplicationModel.CreditMemoBalanceAmount = 32;
//            //creditMemoApplicationModel.FinancialEndDate = null;
//            creditMemoApplicationModel.FinancialPeriodLockEndDate = null;
//            creditMemoApplicationModel.FinancialPeriodLockStartDate = null;
//            //creditMemoApplicationModel.FinancialStartDate = null;
//            creditMemoApplicationModel.IsNoSupportingDocument = true;
//            creditMemoApplicationModel.ModifiedBy = null;
//            creditMemoApplicationModel.ModifiedDate = null;
//            creditMemoApplicationModel.NoSupportingDocument = null;
//            creditMemoApplicationModel.PeriodLockPassword = "123456";
//            creditMemoApplicationModel.Remarks = null;
//            creditMemoApplicationModel.UserCreated = "Malathi@gmail.com";

//            creditMemoApplicationModel.CreditMemoApplicationDetailModels = new List<CreditMemoApplicationDetailModel>()
//                  {
//                new CreditMemoApplicationDetailModel { Id=Guid.NewGuid(),BalanceAmount=1223,BaseCurrencyExchangeRate=15562,CreditAmount=5451,CreditMemoApplicationId=Guid.Parse("1EF7DDD2-8B95-4272-BD92-81F469F7CD79"),DocAmount=5523,DocCurrency="21022",DocDate=Convert.ToDateTime("2017-03-10 00:00:00.000"),DocNo="9",DocType="Credit Memo",DocumentId=Guid.Parse("0E971710-0779-4E89-96FD-13301C65AD57")} };
//            }


//        }
       
//        #endregion
//    }







