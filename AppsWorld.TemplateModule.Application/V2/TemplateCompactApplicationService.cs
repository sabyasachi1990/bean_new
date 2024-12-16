using AppsWorld.TemplateModule.Entities.Models.V2;
using AppsWorld.TemplateModule.Infra;
using AppsWorld.TemplateModule.Infra.V2;
using AppsWorld.TemplateModule.Models;
using AppsWorld.TemplateModule.Models.V2;
using AppsWorld.TemplateModule.Service.V2;
using Microsoft.WindowsAzure.Storage;
using Mustache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoice = AppsWorld.TemplateModule.Entities.Models.V2.Invoice;
//using InvoiceTemplateVM = AppsWorld.TemplateModule.Models.V2.InvoiceTemplateVM;
//using LineItem = AppsWorld.TemplateModule.Models.V2.LineItem;
using MustacheModel = AppsWorld.TemplateModule.Models.V2.MustacheModel;
//using ReceiptDetaislModel = AppsWorld.TemplateModule.Models.V2.ReceiptDetaislModel;
//using ReceiptsModel = AppsWorld.TemplateModule.Models.V2.ReceiptsModel;
//using ServiceEntity = AppsWorld.TemplateModule.Models.V2.ServiceEntity;
//using StatementModel = AppsWorld.TemplateModule.Models.V2.StatementModel;
//using TaxCodeVM = AppsWorld.TemplateModule.Models.V2.TaxCodeVM;
using EmailModel = AppsWorld.TemplateModule.Models.EmailModel;
using System.Globalization;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Ziraff.FrameWork.SingleTon;
using System.Data;
using System.Text.RegularExpressions;
//using Templates;

namespace AppsWorld.TemplateModule.Application.V2
{
    public class TemplateCompactApplicationService
    {
        ITemplateService _templateService;
        public TemplateCompactApplicationService(ITemplateService templateService)
        {
            _templateService = templateService;
        }


        #region Commented 
        //public async Task<EmailModel> GetInvoiceEmailSending1(Guid invoiceId, long companyId, string userName, string azureConnectionString)
        //{
        //    AzureFileManager pdfFileManager =
        //                      new AzureFileManager(CloudStorageAccount.Parse(azureConnectionString),
        //                        ConfigurationManager.AppSettings["AzurePDFContainer"]);
        //    EmailModel emailModel = new EmailModel();
        //    try
        //    {
        //        string templateData = null;

        //        string htmlHeader = null;
        //        string htmlFooter = null;
        //        var invoices = _invoiceKService.GetClientsByInvoice(invoiceId);
        //        var generictemplate = _invoiceKService.GetTemplateheaderInvoice(companyId);
        //        //var generictemplate1 = _invoiceKService.GetTemplateheaderCreditNote(companyId);

        //        //Communication communication = _communicationService.GetCommunication(invoiceId);
        //        // Template template = _invoiceService.GetEmailTemplate(companyId);
        //        if (generictemplate != null)
        //        {
        //            if (generictemplate.IsFooterExist == true || generictemplate.IsHeaderExist == true)
        //            {
        //                if (invoices != null)
        //                {
        //                    CompanyTemplateSettings companyTemplateSetting = _invoiceKService.GetCompanyTemplateSettings(invoices.ServiceCompanyId);
        //                    if (companyTemplateSetting != null)
        //                    {
        //                        htmlHeader = companyTemplateSetting.HeaderContent;
        //                        htmlFooter = companyTemplateSetting.FooterContent;
        //                    }
        //                }
        //            }
        //            templateData = generictemplate.TempletContent;
        //            //templateData1 = generictemplate1.TempletContent;
        //            //emailModel.GenericTemplateId = generictemplate.Id;
        //        }
        //        FormatCompiler compilar = new FormatCompiler();
        //        Generator oppGenerator = null;

        //        Guid clientId = Guid.Empty;

        //        //Localization localization = _invoiceService.GetLocalizationByCompanyId(companyId);
        //        //var company = GetCompany(invoiceId);
        //        //var invoice = GetInvoice(invoiceId);
        //        //var clients = GetClients(invoiceId);
        //        //var caseGroup = GetCaseGroup(invoiceId);
        //        //clients.ScopeofWork = caseGroup.ScopeofWork;
        //        //var contactAddress = GetContactAddress(invoiceId);
        //        //	var contact = GetContact(invoiceId);

        //        if (templateData != null)
        //        {
        //            compilar = new FormatCompiler();
        //            oppGenerator = compilar.Compile(templateData);

        //            string oppResult = null;
        //            TemplateMenuModel templatemenuModel = new TemplateMenuModel();
        //            templatemenuModel.ServiceEntity = GetCompany(invoiceId);
        //            templatemenuModel.Invoice = GetInvoice(invoiceId);
        //            templatemenuModel.Entity = GetEntity(invoiceId);
        //            //templatemenuModel.Item= GetAllItems(invoiceId);

        //            //	templatemenuModel.ContactAddress = contactAddress;
        //            if (oppGenerator != null)
        //                oppResult = "\n" + oppGenerator.Render(templatemenuModel);
        //            //emailModel.TempletContent = oppResult;

        //            byte[] pdfBytes = HtmlToPdfConverter.HtmlToPDF(oppResult, htmlHeader, htmlFooter);
        //            //string pdfFilename = string.Format("Invoice_" + caseGroup.CaseRefNumber + "_" + invoice.InvoiceType + ".pdf");//previous
        //            //kskwf
        //            string pdfFilename = "Invoice_" + templatemenuModel.Invoice.DocNo + ".pdf";
        //            //AzureFileManager pdfFileManager = new AzureFileManager(CloudStorageAccount.Parse(
        //            //      ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString),
        //            //    ConfigurationManager.AppSettings["AzurePDFContainer"]);
        //            string url = await pdfFileManager.UploadPDF(pdfBytes, pdfFilename);

        //            // emailModel.ReportPath = url; //Need to implement mongo data sending in Byte[] formats 
        //            // emailModel.Attachement = string.Format("{0},{1}", url,"");
        //            //emailModel.Attachement = url;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return emailModel;
        //}
        //public CompanyModel GetCompany(Guid invoiceId)
        //{
        //    CompanyModel companyModel = new CompanyModel();
        //    try
        //    {
        //        var company = _invoiceKService.GetInvoicesandsubsudaryCompanyById(invoiceId);
        //        if (company != null)
        //        {
        //            companyModel.CompanyName = company.Item2.Name;
        //            companyModel.RegistrationNo = company.Item2.RegistrationNo;
        //            companyModel.BankCode = company.Item2.Bank.Select(s => s.ShortCode).FirstOrDefault();
        //            companyModel.BankName = company.Item2.Bank.Select(s => s.Name).FirstOrDefault();
        //            companyModel.BranchCode = company.Item2.Bank.Select(s => s.BranchCode).FirstOrDefault();
        //            companyModel.BranchName = company.Item2.Bank.Select(s => s.BranchName).FirstOrDefault(); companyModel.AccountNumber = company.Item2.Bank.Select(s => s.AccountNumber).FirstOrDefault();
        //            companyModel.BankAddress = company.Item2.Bank.Select(s => s.BankAddress).FirstOrDefault();
        //            companyModel.AccountName = company.Item2.Bank.Select(s => s.AccountName).FirstOrDefault();
        //            companyModel.SWIFTCode = company.Item2.Bank.Select(s => s.SwiftCode).FirstOrDefault();
        //            companyModel.Currency = company.Item2.Bank.Select(s => s.Currency).FirstOrDefault();
        //            companyModel.IdentificationType = _invoiceKService.GetIdentificationType(company.Item1.CompanyId);
        //            companyModel.GSTNumber = _invoiceKService.GetGSTNumber(company.Item1.CompanyId, company.Item1.ServiceCompanyId);


        //            //companyModel.RegisteredAddress = company.Item2.Bank.Select(s => s.Currency).FirstOrDefault();
        //            //companyModel.Entityaddress = company.Item2.Bank.Select(s => s.Currency).FirstOrDefault();
        //            //companyModel.MailingAddress = company.Item2.Bank.Select(s => s.Currency).FirstOrDefault();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return companyModel;
        //}

        //public Models.V2.Invoice GetInvoice(Guid invoiceId)
        //{
        //    Models.V2.Invoice invoicemodel = new Models.V2.Invoice();
        //    try
        //    {

        //        var invoice = _invoiceKService.GetInvoicesById(invoiceId);
        //        if (invoice != null)
        //        {
        //            invoicemodel.DocNo = invoice.DocNo;
        //            invoicemodel.DocDate = invoice.DocDate.ToString("dd/MM/yyyy");
        //            invoicemodel.CreditTerms = _invoiceKService.GetCreditTerms(invoice.CompanyId, invoice.CreditTermsId);
        //            invoicemodel.DueDate = invoice.DueDate != null ? invoice.DueDate.Value.ToString("dd/MM/yyyy") : "";
        //            invoicemodel.PoNo = invoice.PONo;
        //            invoicemodel.Currency = invoice.DocCurrency;
        //            invoicemodel.ExchangeRate = invoice.ExchangeRate;
        //            invoicemodel.DocumentDescription = invoice.DocDescription;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return invoicemodel;
        //}

        //public Models.V2.Entitys GetEntity(Guid invoiceId)
        //{
        //    Models.V2.Entitys entityModel = new Models.V2.Entitys();
        //    try
        //    {

        //        var invoice = _invoiceKService.GetInvoicesById(invoiceId);
        //        if (invoice != null)
        //        {

        //            entityModel.EntityName = _invoiceKService.GetEntityName(invoice.CompanyId, invoice.EntityId);
        //            //entityModel.RegisteredAddress = invoice.DocNo;
        //            //entityModel.MailingAddress = invoice.DocNo;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return entityModel;
        //}

        //public Models.V2.Items GetAllItems(Guid invoiceId)
        //{
        //    Models.V2.Items itemModel = new Models.V2.Items();
        //    try
        //    {

        //        var invoice = _invoiceKService.GetInvoicesById(invoiceId);
        //        if (invoice != null)
        //        {

        //            //itemModel.EntityName = _invoiceKService.GetEntityName(invoice.CompanyId, invoice.EntityId);
        //            //entityModel.RegisteredAddress = invoice.DocNo;
        //            //entityModel.MailingAddress = invoice.DocNo;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return itemModel;
        //}
        #endregion

        public async Task<EmailTemplatesVM> GenerateTemplates(EmailModel emailModel, string azureConnectionString)
        {
            EmailTemplatesVM templateVM = new EmailTemplatesVM();
            try
            {
                //AzureFileManager pdfFileManager =
                //                  new AzureFileManager(CloudStorageAccount.Parse(azureConnectionString),
                //                    ConfigurationManager.AppSettings["AzurePDFContainer"]);
                string htmlHeader = null;
                string htmlFooter = null;
                //var invoices = _templateService.GetClientsByInvoice(emailModel.ScreenId);
                string result = null;
                List<Address> address = null;
                BeanEntity entity = null;
                Localization localization = _templateService.GetLocalizationByCompanyId(emailModel.CompanyId.Value);

                Invoice invoice = null;
                DebitNote debitNote = null;
                MediaRepository mediaRepository = null;
                Guid entityId = Guid.Empty;

                Company company = null;
                Bank bank = null;
                BeanEntity beanentity = null;
                string idType = null;
                string docNo = string.Empty;
                string gstnumber = string.Empty;
                if (emailModel.ScreenName == "Invoice" || emailModel.ScreenName == "Credit Note")
                {

                    invoice = _templateService.GetInvoiceById(emailModel.ScreenId, emailModel.ScreenName);
                    docNo = invoice.DocNo;
                    entityId = invoice.EntityId;
                    company = _templateService.GetServiceCompany(invoice.ServiceCompanyId);
                    idType = _templateService.GetIdType(company.IdTypeId);
                    bank = _templateService.GetInvoiceBank(invoice.ServiceCompanyId);
                    gstnumber = _templateService.GetGSTnumber(invoice.ServiceCompanyId);
                }
                else if (emailModel.ScreenName == "Receipt")
                {

                    var receipt = _templateService.GetReceiptById(emailModel.ScreenId);
                    docNo = receipt.DocNo;
                    entityId = receipt.EntityId;
                    company = _templateService.GetServiceCompany(receipt.ServiceCompanyId);
                    idType = _templateService.GetIdType(company.IdTypeId);
                    bank = bank = _templateService.GetBank(receipt.ServiceCompanyId);
                    gstnumber = _templateService.GetGSTnumber(receipt.ServiceCompanyId);
                }
                else if (emailModel.ScreenName == "Statement Of Account")
                {
                    beanentity = _templateService.GetEntity(emailModel.ScreenId);
                    entityId = beanentity.Id;
                }
                else if (emailModel.ScreenName == "Debit Note")
                {
                    debitNote = _templateService.GetDebitNoteById(emailModel.ScreenId, emailModel.ScreenName);
                    docNo = debitNote.DocNo;
                    entityId = debitNote.EntityId;
                    company = _templateService.GetServiceCompany(debitNote.ServiceCompanyId.Value);
                    idType = _templateService.GetIdType(company.IdTypeId);
                    //mediaRepository = _templateService.GetPhoto(company.Id, company.LogoId);
                    bank = _templateService.GetBank(debitNote.ServiceCompanyId.Value);
                    gstnumber = _templateService.GetGSTnumber(debitNote.ServiceCompanyId.Value);
                }
                //string gstnumber = _templateService.GetGSTnumber(emailModel.CompanyId.Value);

                templateVM.ScreenId = emailModel.ScreenId;
                templateVM.ScreenName = emailModel.ScreenName;
                templateVM.CursorName = emailModel.CursorName;
                templateVM.TemplateName = emailModel.TemplateName;
                if (entityId != null)
                {
                    address = _templateService.GetAddress(entityId);
                    entity = _templateService.GetEntity(entityId);
                }
                var lstEmails = _templateService.GetContactByClienId(entity.Id);
                var contactNames = _templateService.GetContactsById(entity.Id);
                if (lstEmails != null)
                {
                    templateVM.ToEmails = lstEmails.Select(a => new FrameWork.LookUps.LookUp<string>()
                    {
                        Code = contactNames.Where(x => x.Id == a.ContactId).Select(x => x.FirstName).FirstOrDefault() + " " + "<" + a.Email + ">",
                        Name = contactNames.Where(x => x.Id == a.ContactId).Select(x => x.FirstName).FirstOrDefault() + " " + "<" + a.Email + ">",
                        RecOrder = 1
                    }
                    ).OrderBy(a => a.Name).ToList();
                }
                PrimaryContact contact = new PrimaryContact();
                contact.ContactName = contactNames.Select(s => s.FirstName).FirstOrDefault();
                contact.ContactSalutation = contactNames.Select(s => s.Salutation).FirstOrDefault();
                contact.Contactemail = contactNames.Select(s => s.Communication).FirstOrDefault();
                FormatCompiler compiler = new FormatCompiler();
                MustacheModel mustacheModel = new MustacheModel();
                //templateVM.ToEmail = lstEmails.Where(s => s.IsPrimary == true).Select(s => s.Email).FirstOrDefault();
                templateVM.ToEmail = lstEmails.Where(a => a.IsPrimary == true).Select(a => (contactNames.Where(x => x.Id == a.ContactId).Select(x => x.FirstName).FirstOrDefault()) + " " + "<" + a.Email + ">").FirstOrDefault();
                templateVM.CcMail = /*emailModel.CompanyId.Value == 1 ? "accounts@precursor.com.sg" :*/ "";



                mustacheModel.Contact = contact;
                mustacheModel.PrimaryContact = contact;
                mustacheModel.StatementDate = DateTime.UtcNow.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
                mustacheModel.UserName = _templateService.GetFirstName(emailModel.CompanyId.Value, emailModel.UserName);
                string EmailBody = "";

                FillServiceEntitiesEntity2MustacheModel(company, entity, address, bank, mustacheModel, localization, gstnumber, idType);
                // FillCreditNoteTemplateEntity2Model(invoice, company, mustacheModel, localization, gstnumber);
                switch (emailModel.ScreenName)
                {
                    case "Invoice":
                        mustacheModel.Invoice = FillInvoiceTemplateEntity2Model(invoice, mustacheModel, localization, gstnumber);
                        EmailBody = _templateService.GetGenericEmailBody(invoice.CompanyId, "Bean Invoice Email");
                        break;
                    case "Credit Note":
                        mustacheModel.CreditNote = FillCreditNoteTemplateEntity2Model(invoice, company, mustacheModel, localization, gstnumber);
                        EmailBody = _templateService.GetGenericEmailBody(invoice.CompanyId, "Bean Credit Note Email");
                        break;
                    case "Debit Note":
                        mustacheModel.CreditNote = FillDebitNoteTemplateEntity2Model(debitNote, company, mustacheModel, localization, gstnumber);
                        EmailBody = _templateService.GetGenericEmailBody(debitNote.CompanyId, "Bean Debit Note Email");
                        break;
                    case "Receipt":
                        FillReceiptEntit2MustacheModel(emailModel, mustacheModel, localization);
                        EmailBody = _templateService.GetGenericEmailBody(emailModel.CompanyId.Value, "Bean Receipt Email");
                        break;
                    case "Statement Of Account":
                        FillSoaEntity2MustacheModel(emailModel, mustacheModel, localization);
                        EmailBody = _templateService.GetGenericEmailBody(emailModel.CompanyId.Value, "SOA Email");
                        break;
                }
                templateVM.mustacheModel = mustacheModel;
                string pdfFilename = string.Empty;
                Generator content = compiler.Compile(emailModel.TemplateContent);
                result = content.Render(mustacheModel);
                templateVM.TemplateContent = result;
                byte[] pdfBytes = HtmlToPdfConverter.HtmlToPDF(result, htmlHeader, htmlFooter);
                //if (docNo != null || docNo == string.Empty)
                pdfFilename = emailModel.TemplateName + "_" + docNo + ".pdf";
                string fileName = pdfFilename.Replace(" ", "_");
                string screenName = emailModel.ScreenName == "Debit Note" ? emailModel.ScreenName + " (s)" : emailModel.ScreenName;
                templateVM.Subject = "Our Services for " + entity.Name + " - " + screenName;
                templateVM.AttachmentName = fileName;
                //templateVM.Attachments = await pdfFileManager.UploadPDF(pdfBytes, fileName);

                string result1 = null;
                Generator content1 = compiler.Compile(EmailBody);
                result1 = content1.Render(mustacheModel);
                templateVM.EmailBody = result1;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return templateVM;
        }


        #region Multiple Email 
        public List<EmailTemplatesVM> GenerateMultipleTemplates(List<EmailModel> emailModel,  string username)
        {

            List<EmailTemplatesVM> lstTemplates = new List<EmailTemplatesVM>();
            List<Invoices> lstDebits = new List<Invoices>();
            List<AppsWorld.TemplateModule.Models.V2.Receipt> lstReceipts = new List<AppsWorld.TemplateModule.Models.V2.Receipt>();
            foreach (var email in emailModel)
            {

                EmailTemplatesVM template = new EmailTemplatesVM();
                try
                {
                    
                    List<Address> address = null;
                    BeanEntity entity = null;
                    Localization localization = _templateService.GetLocalizationByCompanyId(email.CompanyId.Value);

                    Invoice invoice = null;
                    DebitNote debitNote = null;
                    CashSale cashSale = null;
                    Guid entityId = Guid.Empty;

                    Company company = null;
                    Bank bank = null;
                    BeanEntity beanEntity = null;
                    string idType = null;
                    string docNo = string.Empty;
                    string gstNumber = string.Empty;
                    if (email.ScreenName == "Invoice" || email.ScreenName == "Credit Note")
                    {

                        invoice = _templateService.GetInvoiceById(email.ScreenId, email.ScreenName);
                        docNo = StringCharactersReplaceFunction(invoice.DocNo);
                        entityId = invoice.EntityId;
                        company = _templateService.GetServiceCompany(invoice.ServiceCompanyId);
                        idType = _templateService.GetIdType(company.IdTypeId);
                        bank = _templateService.GetBank(invoice.ServiceCompanyId, email.ScreenName);
                        gstNumber = _templateService.GetGSTnumber(invoice.ServiceCompanyId);
                    }
                    else if (email.ScreenName == "Receipt")
                    {

                        var receipt = _templateService.GetReceiptById(email.ScreenId);
                        docNo = StringCharactersReplaceFunction(receipt.DocNo);
                        entityId = receipt.EntityId;
                        company = _templateService.GetServiceCompany(receipt.ServiceCompanyId);
                        idType = _templateService.GetIdType(company.IdTypeId);
                        bank = _templateService.GetBank(receipt.ServiceCompanyId, email.ScreenName);
                        gstNumber = _templateService.GetGSTnumber(receipt.ServiceCompanyId);
                    }
                    else if (email.ScreenName == "Statement Of Account")
                    {
                        beanEntity = _templateService.GetEntity(email.ScreenId);
                        company = _templateService.GetServiceCompanyForSOA(beanEntity.CompanyId);
                        bank = company != null ? _templateService.GetBank(company.Id, email.ScreenName) : null;
                        idType = company != null ? _templateService.GetIdType(company.IdTypeId) : null;
                        gstNumber = company != null ? _templateService.GetGSTnumber(company.Id) : null;
                        entityId = beanEntity.Id;
                    }
                    else if (email.ScreenName == "Debit Note")
                    {
                        debitNote = _templateService.GetDebitNoteById(email.ScreenId, email.ScreenName);
                        docNo = StringCharactersReplaceFunction(debitNote.DocNo);
                        entityId = debitNote.EntityId;
                        company = _templateService.GetServiceCompany(debitNote.ServiceCompanyId.Value);
                        idType = _templateService.GetIdType(company.IdTypeId);
                        bank = _templateService.GetBank(debitNote.ServiceCompanyId.Value, email.ScreenName);
                        gstNumber = _templateService.GetGSTnumber(debitNote.ServiceCompanyId.Value);
                    }
                    else if (email.ScreenName == "Cash Sale")
                    {
                        cashSale = _templateService.GetCashSaleById(email.ScreenId, email.ScreenName);
                        docNo = StringCharactersReplaceFunction(cashSale.DocNo);
                        entityId = cashSale.EntityId != null ? cashSale.EntityId.Value : Guid.Empty;
                        company = _templateService.GetServiceCompany(cashSale.ServiceCompanyId.Value);
                        idType = _templateService.GetIdType(company.IdTypeId);
                        bank = _templateService.GetBank(cashSale.ServiceCompanyId.Value, email.ScreenName);
                        bool? isGstActive = _templateService.GetServiceCompanyByGst(cashSale.ServiceCompanyId.Value);
                        if (isGstActive == true)
                            gstNumber = _templateService.GetGSTnumber(cashSale.ServiceCompanyId.Value);
                    }
                    template.ScreenId = email.ScreenId;
                    template.ScreenName = email.ScreenName;
                    template.CursorName = email.CursorName;
                    template.TemplateName = email.TemplateName;
                    if (entityId != null)
                    {
                        address = _templateService.GetAddress(entityId);
                        entity = _templateService.GetEntity(entityId);
                    }
                    var lstEmails = entity != null ? _templateService.GetContactByClienId(entity.Id) : null;
                    var contactNames = entity != null ? _templateService.GetContactsById1(entity.Id) : null;
                    if (lstEmails != null)
                    {
                        template.ToEmails = lstEmails.Select(a => new FrameWork.LookUps.LookUp<string>()
                        {
                            Code = contactNames.Where(x => x.Id == a.ContactId).Select(x => x.FirstName).FirstOrDefault() + " " + "<" + a.Email + ">",
                            Name = contactNames.Where(x => x.Id == a.ContactId).Select(x => x.FirstName).FirstOrDefault() + " " + "<" + a.Email + ">",
                            RecOrder = 1
                        }
                        ).OrderBy(a => a.Name).ToList();
                    }
                    PrimaryContact contact = new PrimaryContact();
                    contact.ContactName = contactNames != null ? contactNames.Select(s => s.FirstName).FirstOrDefault() : null;
                    contact.ContactSalutation = contactNames != null ? contactNames.Select(s => s.Salutation).FirstOrDefault() : null;
                    contact.Contactemail = lstEmails != null ? lstEmails.Select(s => s.Email).FirstOrDefault() : null;
                    MustacheModel mustacheModel = new MustacheModel();
                    template.ToEmail = contactNames != null ? (lstEmails.Where(a => a.IsPrimary == true).Select(a => (contactNames.Where(x => x.Id == a.ContactId).Select(x => x.FirstName).FirstOrDefault()) + " " + "<" + a.Email + ">").FirstOrDefault()) : null;
                    string genericTemplateCcMail = _templateService.GetGenerictemplateForCcMail(email.CompanyId.Value, template.TemplateName);
                    template.CcMail = genericTemplateCcMail;
                    mustacheModel.Contact = contact;
                    mustacheModel.PrimaryContact = contact;
                    mustacheModel.StatementDate = DateTime.UtcNow.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
                    mustacheModel.UserName = _templateService.GetFirstName(email.CompanyId.Value, email.UserName);
                    string EmailBody = "";

                    FillServiceEntitiesEntity2MustacheModel(company, entity, address, bank, mustacheModel, localization, gstNumber, idType);
                    switch (email.ScreenName)
                    {
                        case CommonConstant.Invoice:
                            mustacheModel.Invoice = FillInvoiceTemplateEntity2Model(invoice, mustacheModel, localization, gstNumber);
                            lstDebits.Add(mustacheModel.Invoice);
                            mustacheModel.LstDebitNotes = lstDebits.OrderBy(s => s.DocDate).ThenBy(s => s.CreatedDate).ToList();
                            mustacheModel.OutstandingBalance = Math.Round(lstDebits.Select(v => Convert.ToDecimal(v.BalanceAmount)).Sum(), 2).ToString("N2");
                            EmailBody = email.TemplateName == "Invoice" ? _templateService.GetGenericEmailBody1(invoice.CompanyId, /*email.TemplateName*/ "Bean Invoice Email", email.TemplateName) : _templateService.GetGenericEmailBody1(invoice.CompanyId, "Bean Invoice Email", email.TemplateName /*"Bean Invoice Email"*/);
                            break;
                        case CommonConstant.Credit_Note:
                            mustacheModel.CreditNote = FillCreditNoteTemplateEntity2Model(invoice, company, mustacheModel, localization, gstNumber);
                            lstDebits.Add(mustacheModel.CreditNote);
                            mustacheModel.LstDebitNotes = lstDebits.OrderBy(s => s.DocDate).ThenBy(s => s.CreatedDate).ToList();
                            mustacheModel.OutstandingBalance = Math.Round(lstDebits.Select(v => Convert.ToDecimal(v.BalanceAmount)).Sum(), 2).ToString("N2");
                            EmailBody = _templateService.GetGenericEmailBody1(invoice.CompanyId, "Bean Credit Note Email", email.TemplateName);
                            break;
                        case CommonConstant.Debit_Note:
                          
                            mustacheModel.CreditNote = FillDebitNoteTemplateEntity2Model(debitNote, company, mustacheModel, localization, gstNumber);
                            lstDebits.Add(mustacheModel.CreditNote);
                            mustacheModel.LstDebitNotes = lstDebits.OrderBy(s => s.DocDate).ThenBy(s => s.CreatedDate).ToList();
                            mustacheModel.OutstandingBalance = Math.Round(lstDebits.Select(v => Convert.ToDecimal(v.BalanceAmount)).Sum(), 2).ToString("N2");
                            if (emailModel.Count > 1)
                            { EmailBody = email.TemplateName == "Debit Note" ? _templateService.GetGenericEmailBody1(debitNote.CompanyId, "Bean Debit Note Email", email.TemplateName) : _templateService.GetGenericEmailBody1(debitNote.CompanyId, "Bean Debit Note Multiple Email", email.TemplateName /*"Debit Note Multiple Email"*/); }
                            else { EmailBody = _templateService.GetGenericEmailBody1(debitNote.CompanyId, "Bean Debit Note Email", email.TemplateName); }
                            break;
                        case CommonConstant.Cash_Sale:
                           
                            mustacheModel.CashSale = FillCashSaleTemplateEntity2Model(cashSale, company, mustacheModel, localization, gstNumber);
                            lstDebits.Add(mustacheModel.CashSale);
                            mustacheModel.LstCashSales = lstDebits.OrderBy(s => s.DocDate).ThenBy(s => s.CreatedDate).ToList();
                            mustacheModel.OutstandingBalance = Math.Round(lstDebits.Select(v => Convert.ToDecimal(v.BalanceAmount)).Sum(), 2).ToString("0,0.00",
                CultureInfo.InvariantCulture);
                            EmailBody =_templateService.GetGenericEmailBody1(cashSale.CompanyId, "Cash Sale Email", email.TemplateName);
                            break;
                        case CommonConstant.Receipt:
                            FillReceiptEntit2MustacheModel(email, mustacheModel, localization);

                            lstReceipts.Add(mustacheModel.Receipt);
                            mustacheModel.BRFADetail = lstReceipts.SelectMany(c => c.BRFApplication).ToList();
                            mustacheModel.Receipt.BRAmount = Math.Round(lstReceipts.Sum(s => Convert.ToDecimal(s.BRAmount)), 2).ToString("0,0.00",
                CultureInfo.InvariantCulture);
                            mustacheModel.Receipt.RAAmmount = Math.Round(lstReceipts.Sum(s => Convert.ToDecimal(s.RAAmmount)), 2).ToString("0,0.00",
               CultureInfo.InvariantCulture);
                            mustacheModel.BRFApplications = lstReceipts.SelectMany(c => c.BRFApplication).ToList();
                            mustacheModel.ReceiptDetailsModel = lstReceipts.OrderBy(s => s.RecieptDate).ToList();
                            EmailBody =  _templateService.GetGenericEmailBody1(email.CompanyId.Value, "Bean Receipt Email", email.TemplateName);
                            break;
                        case CommonConstant.Statement_Of_Account:
                            FillSoaEntity2MustacheModel(email, mustacheModel, localization, username);
                            EmailBody =  _templateService.GetGenericEmailBody1(email.CompanyId.Value, "SOA Email", email.TemplateName);
                            break;
                    }

                    string pdfFilename = string.Empty;
                    template.mustacheModel = mustacheModel;
                    pdfFilename = email.ScreenName + "_" + docNo + ".pdf";
                    string fileName = pdfFilename.Replace(" ", "_");
                    string screenName = (email.ScreenName == "Debit Note" || email.ScreenName == "Cash Sale") ? email.ScreenName + "(s)" : email.ScreenName;
                    template.Subject = entity != null ? ("Our Services for " + entity.Name + " - " + screenName) : null;
                    template.AttachmentName = fileName;
                    template.EmailBody = EmailBody;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                lstTemplates.Add(template);
            }
            return lstTemplates;
        }
        #endregion Multiple Email 
        private void FillServiceEntitiesEntity2MustacheModel(Company company, BeanEntity entity, List<Address> address, Bank bank, MustacheModel mustacheModel, Localization localization, string gstNumber, string idType)
        {
            var Companyaddress = company != null ? _templateService.GetAddressForCompany(company.Id) : null;
            var EntitymailingAddress = address.Where(x => x.AddType == "Entity" && x.AddSectionType == "Mailing Address").Select(x => x.AddressBook).FirstOrDefault();
            var EntityregisterAddress = address.Where(x => x.AddType == "Entity" && x.AddSectionType == "Registered Address").Select(x => x.AddressBook).FirstOrDefault();

            var ServiceEntitymailingAddress = Companyaddress.Where(x => x.AddType == "Company" && x.AddSectionType == "Mailing Address").Select(x => x.AddressBook).FirstOrDefault();
            var ServiceEntityregisterAddress = Companyaddress.Where(x => x.AddType == "Company" && x.AddSectionType == "Registered Address").Select(x => x.AddressBook).FirstOrDefault();


            //ServiceEntity Company = new ServiceEntity();
            ServiceEntitys companys = new ServiceEntitys();
            //Templates.Template.Entity Entity = new Templates.Template.Entity();
            BeanEntityModel Entity = new BeanEntityModel();
            if (company != null)
            {
                MediaRepository media = _templateService.GetPhoto(company.LogoId);
                var companyLogo = media != null ? (media.Medium != null ? media.Medium : media.Large) : null;
                var imagtagWithLog = companyLogo != null ? string.Format(@"<img src='{0}'>", companyLogo) : null;
                companys.CompanyLogo = imagtagWithLog;
                companys.CompanyName = company.Name;
                companys.EntityName = entity != null ? entity.Name : string.Empty;
                companys.RegistrationNo = company.RegistrationNo;
                companys.IdentificationType = idType;
                companys.Currency = company.BaseCurrency;
                companys.GSTNumber = gstNumber;

                companys.MailingAddress = ServiceEntitymailingAddress != null ? ((($"{ServiceEntitymailingAddress.BlockHouseNo}") != "" ? ($"{ServiceEntitymailingAddress.BlockHouseNo}") : "") + " " + (($"{ServiceEntitymailingAddress.Street}") != "" ? ($"{ ServiceEntitymailingAddress.Street}") : "") + "</br> " + (($"{ServiceEntitymailingAddress.UnitNo}") != "" ? ($"{ServiceEntitymailingAddress.UnitNo}") : "") + " " + (($"{ServiceEntitymailingAddress.BuildingEstate}") != "" ? ($"{ServiceEntitymailingAddress.BuildingEstate}") : "") + "</br>" + (($"{ServiceEntitymailingAddress.Country}") != "" ? ($"{ServiceEntitymailingAddress.Country}") : "") + "</br>" + (($"{ServiceEntitymailingAddress.Email}") != "" ? ($"{ServiceEntitymailingAddress.Email}") : "") + " " + (($"{ServiceEntitymailingAddress.PostalCode}") != "" ? ($"{ServiceEntitymailingAddress.PostalCode}") : "")) : null;

                companys.RegisteredAddress = ServiceEntityregisterAddress != null ? ((($"{ServiceEntityregisterAddress.BlockHouseNo}") != "" ? ($"{ServiceEntityregisterAddress.BlockHouseNo}") : "") + " " + (($"{ServiceEntityregisterAddress.Street}") != "" ? ($"{ ServiceEntityregisterAddress.Street}") : "") + "</br> " + (($"{ServiceEntityregisterAddress.UnitNo}") != "" ? ($"{ServiceEntityregisterAddress.UnitNo}") : "") + " " + (($"{ServiceEntityregisterAddress.BuildingEstate}") != "" ? ($"{ServiceEntityregisterAddress.BuildingEstate}") : "") + "</br>" + (($"{ServiceEntityregisterAddress.Country}") != "" ? ($"{ServiceEntityregisterAddress.Country}") : "") + "</br>" + (($"{ServiceEntityregisterAddress.Email}") != "" ? ($"{ServiceEntityregisterAddress.Email}") : "") + " " + (($"{ServiceEntityregisterAddress.PostalCode}") != "" ? ($"{ServiceEntityregisterAddress.PostalCode}") : "")) : null;

                companys.Entityaddress = EntitymailingAddress != null ? ((($"{EntitymailingAddress.BlockHouseNo}") != "" ? ($"{EntitymailingAddress.BlockHouseNo}") : "") + " " + (($"{EntitymailingAddress.Street}") != "" ? ($"{ EntitymailingAddress.Street}") : "") + "</br> " + (($"{EntitymailingAddress.UnitNo}") != "" ? ($"{EntitymailingAddress.UnitNo}") : "") + " " + (($"{EntitymailingAddress.BuildingEstate}") != "" ? ($"{EntitymailingAddress.BuildingEstate}") : "") + "</br>" + (($"{EntitymailingAddress.Country}") != "" ? ($"{EntitymailingAddress.Country}") : "") + "</br>" + (($"{EntitymailingAddress.Email}") != "" ? ($"{EntitymailingAddress.Email}") : "") + " " + (($"{EntitymailingAddress.PostalCode}") != "" ? ($"{EntitymailingAddress.PostalCode}") : "")) : EntityregisterAddress != null ? (($"{EntityregisterAddress.BlockHouseNo}") != "" ? ($"{EntityregisterAddress.BlockHouseNo}") : "") + " " + (($"{EntityregisterAddress.Street}") != "" ? ($"{ EntityregisterAddress.Street}") : "") + "</br> " + (($"{EntityregisterAddress.UnitNo}") != "" ? ($"{EntityregisterAddress.UnitNo}") : "") + " " + (($"{EntityregisterAddress.BuildingEstate}") != "" ? ($"{EntityregisterAddress.BuildingEstate}") : "") + "</br>" + (($"{EntityregisterAddress.Country}") != "" ? ($"{EntityregisterAddress.Country}") : "") + "</br>" + (($"{EntityregisterAddress.Email}") != "" ? ($"{EntityregisterAddress.Email}") : "") + "</br>" + (($"{EntityregisterAddress.PostalCode}") != "" ? ($"{EntityregisterAddress.PostalCode}") : "") : null;


                mustacheModel.Company = companys;
                mustacheModel.ServiceEntity = companys;

            }
            if (entity != null)
            {
                Entity.Entityname = entity.Name;
                Entity.MailingAddress = EntitymailingAddress != null ? ((($"{EntitymailingAddress.BlockHouseNo}") != "" ? ($"{EntitymailingAddress.BlockHouseNo}") : "") + " " + (($"{EntitymailingAddress.Street}") != "" ? ($"{ EntitymailingAddress.Street}") : "") + "</br> " + (($"{EntitymailingAddress.UnitNo}") != "" ? ($"{EntitymailingAddress.UnitNo}") : "") + " " + (($"{EntitymailingAddress.BuildingEstate}") != "" ? ($"{EntitymailingAddress.BuildingEstate}") : "") + " " + "</br>" + (($"{EntitymailingAddress.Country}") != "" ? ($"{EntitymailingAddress.Country}") : "") + "</br>" + (($"{EntitymailingAddress.Email}") != "" ? ($"{EntitymailingAddress.Email}") : "") + " " + (($"{EntitymailingAddress.PostalCode}") != "" ? ($"{EntitymailingAddress.PostalCode}") : "")) : null;

                Entity.RegisteredAddress = EntityregisterAddress != null ? (($"{EntityregisterAddress.BlockHouseNo}") != "" ? ($"{EntityregisterAddress.BlockHouseNo}") : "") + " " + (($"{EntityregisterAddress.Street}") != "" ? ($"{EntityregisterAddress.Street}") : "") + "</br> " + (($"{EntityregisterAddress.UnitNo}") != "" ? ($"{EntityregisterAddress.UnitNo}") : "") + " " + (($"{EntityregisterAddress.BuildingEstate}") != "" ? ($"{EntityregisterAddress.BuildingEstate}") : "") + "</br>" + (($"{EntityregisterAddress.Country}") != "" ? ($"{EntityregisterAddress.Country}") : "") + "</br>" /*+ (($"{EntityregisterAddress.Email}") != "" ? ($"{EntityregisterAddress.Email}") : "")*/ + " " + (($"{EntityregisterAddress.PostalCode}") != "" ? ($"{EntityregisterAddress.PostalCode}") : "") : null;

                if (EntityregisterAddress != null)
                {
                    Entity.RegisteredBlock = EntityregisterAddress.BlockHouseNo != null || EntityregisterAddress.BlockHouseNo != string.Empty ? EntityregisterAddress.BlockHouseNo : null;
                    Entity.RegisteredUnit = EntityregisterAddress.UnitNo != null || EntityregisterAddress.UnitNo != string.Empty ? EntityregisterAddress.UnitNo : null;
                    Entity.RegisteredPostalCode = EntityregisterAddress.PostalCode != null || EntityregisterAddress.PostalCode != string.Empty ? EntityregisterAddress.PostalCode : null;
                    Entity.RegisteredBuilding = EntityregisterAddress.BuildingEstate != null || EntityregisterAddress.BuildingEstate != string.Empty ? EntityregisterAddress.BuildingEstate : null;
                    Entity.RegisteredCountry = EntityregisterAddress.Country != null || EntityregisterAddress.Country != string.Empty ? EntityregisterAddress.Country : null;
                    Entity.RegisteredEmail = EntityregisterAddress.Email != null || EntityregisterAddress.Email != string.Empty ? EntityregisterAddress.Email : null;
                    Entity.RegisteredStreet = EntityregisterAddress.Street != null || EntityregisterAddress.Street != string.Empty ? EntityregisterAddress.Street : null;
                }
                if (EntitymailingAddress != null)
                {
                    Entity.MailingBlock = EntitymailingAddress.BlockHouseNo != null || EntitymailingAddress.BlockHouseNo != string.Empty ? EntitymailingAddress.BlockHouseNo : null;
                    Entity.MailingStreet = EntitymailingAddress.Street != null || EntitymailingAddress.Street != string.Empty ? EntitymailingAddress.Street : null;
                    Entity.MailingUnit = EntitymailingAddress.UnitNo != null || EntitymailingAddress.UnitNo != string.Empty ? EntitymailingAddress.UnitNo : null;
                    Entity.MailingBuilding = EntitymailingAddress.BuildingEstate != null || EntitymailingAddress.BuildingEstate != string.Empty ? EntitymailingAddress.BuildingEstate : null;
                    Entity.MailingCountry = EntitymailingAddress.Country != null || EntitymailingAddress.Country != string.Empty ? EntitymailingAddress.Country : null;
                    Entity.MailingPostalCode = EntitymailingAddress.PostalCode != null || EntitymailingAddress.PostalCode != string.Empty ? EntitymailingAddress.PostalCode : null;
                    Entity.MailingEmail = EntitymailingAddress.Email != null || EntitymailingAddress.Email != string.Empty ? EntitymailingAddress.Email : null;
                }

                mustacheModel.Entity = Entity;
            }
            else
            {
                Entity.Entityname = string.Empty;
                Entity.MailingAddress = string.Empty;
                Entity.RegisteredAddress = string.Empty;

                mustacheModel.Entity = Entity;
            }
            if (address.Count() > 0 && entity != null)
            {
                AddressBook registredAddbook = null;
                registredAddbook = address.Where(s => s.AddSectionType == "Registered Address").Select(b => b.AddressBook).LastOrDefault();



                #region Old Code
                ////Templates.Template.Entity Entity = new Templates.Template.Entity();
                ////string AddSectionType = address.Where(s => s.AddSectionType == "Mailing Address").Select(s => s.AddSectionType).LastOrDefault();
                ////if (AddSectionType== "Mailing Address")
                ////{
                //AddressBook registredAddbook = null;
                //registredAddbook = address.Where(s => s.AddSectionType == "Mailing Address").Select(b => b.AddressBook).FirstOrDefault();
                ////if (mailingAddbook != null)
                ////    Entity.MailingAddress = ($" {mailingAddbook.Street},{mailingAddbook.City},{mailingAddbook.Country}");
                ////}
                //if (registredAddbook == null)
                //{
                //    registredAddbook = address.Where(s => s.AddSectionType == "Registered Address").Select(b => b.AddressBook).LastOrDefault();
                //}
                //if (registredAddbook == null)
                //{
                //    registredAddbook = address.Where(s => s.AddSectionType == "Residential Address").Select(b => b.AddressBook).LastOrDefault();
                //}
                //else if (registredAddbook == null)
                //{
                //    registredAddbook = address.Where(s => s.AddSectionType == "Residential Address").Select(b => b.AddressBook).LastOrDefault();
                //}
                ////registredAddbook = address.Where(s => s.AddSectionType == "Residential Address").Select(b => b.AddressBook).LastOrDefault();
                ////if (registredAddbook != null)
                ////Entity.RegisteredAddress = ($" {registredAddbook.Street},{registredAddbook.City},{registredAddbook.Country}");
                //Entity.RegisteredAddress = registredAddbook.BlockHouseNo != null? registredAddbook.BlockHouseNo : "" +" "+registredAddbook.Street != null? registredAddbook.Street : ""+"</br>"+ registredAddbook.UnitNo != null ? registredAddbook.UnitNo : "" + " "+ registredAddbook.BuildingEstate != null ? registredAddbook.BuildingEstate : "" + "</br>"+registredAddbook.Country != null ? registredAddbook.Country : "" + " " + registredAddbook.PostalCode != null ? registredAddbook.PostalCode : "";
                ////mustacheModel.Entity = Entity;
                //companys.MailingAddress = Entity.MailingAddress;
                //companys.RegisteredAddress = Entity.RegisteredAddress;
                #endregion Old Code

                if (registredAddbook != null)
                {
                    mustacheModel.RegisteredBlock = ($"{ registredAddbook.BlockHouseNo}");
                    mustacheModel.RegisteredBuilding = ($"{ registredAddbook.BuildingEstate}");
                    mustacheModel.RegisteredCountry = ($"{ registredAddbook.Country}");
                    mustacheModel.RegisteredPostalCode = ($"{ registredAddbook.PostalCode}");
                    mustacheModel.RegisteredStreet = ($"{registredAddbook.Street}");
                    mustacheModel.RegisteredUnit = ($"{registredAddbook.UnitNo}");
                    //Entity.RegisteredAddress = ($"{registredAddbook.BlockHouseNo}") != null ? ($"{registredAddbook.BlockHouseNo}") : "" + " " + ($"{registredAddbook.Street}") != null ? ($"{ registredAddbook.Street}") : "" + "</br> " + ($"{registredAddbook.UnitNo}") != null ? ($"{registredAddbook.UnitNo}") : "" + " " + ($"{registredAddbook.BuildingEstate}") != null ? ($"{registredAddbook.BuildingEstate}") : "" + "</br>" + ($"{registredAddbook.Country}") != null ? ($"{registredAddbook.Country}") : "" + " " + ($"{registredAddbook.PostalCode}") != null ? ($"{registredAddbook.PostalCode}") : "";
                    //Entity.MailingAddress=registredAddbook.Street != null?($" {registredAddbook.Street},{registredAddbook.City},{registredAddbook.Country}"):"";
                }
                mustacheModel.Entity = Entity;
                mustacheModel.Company = companys;
                mustacheModel.ServiceEntity = companys;
            }
            mustacheModel.IsBankDetailHide = bank != null ? true : false;
            mustacheModel.IsBankDetailNotHide = bank == null ? true : false;
            if (bank != null)
            {
                //Templates.Template.ServiceEntity banks = new Templates.Template.ServiceEntity();
                companys.BankName = bank.Name;
                companys.BankCode = bank.ShortCode;
                companys.BranchCode = bank.BranchCode;
                companys.BranchName = bank.BranchName;
                companys.Currency = bank.Currency;
                //companys.GSTNumber= company.IsGstSetting=true?company.g
                companys.SWIFTCode = bank.SwiftCode;
                companys.BankAddress = bank.BankAddress;
                companys.AccountNumber = bank.AccountNumber;
                companys.AccountName = bank.AccountName;
                mustacheModel.Company = companys;
                mustacheModel.ServiceEntity = companys;
            }

        }

        private void FillSoaEntity2MustacheModel(EmailModel templateVM, MustacheModel mustacheModel, Localization localization, string username)
        {
            var journelLst = _templateService.GetJournal(templateVM.ScreenId, templateVM.CompanyId.Value);
            var lstinvoices = _templateService.GetAllInvoice(templateVM.ScreenId, templateVM.CompanyId.Value);

            var listOfInvoic = GetSpRecords(templateVM.CompanyId.Value, DateTime.Now, templateVM.ScreenId, username);


            List<SOAOutstandingAmount> dd = new List<SOAOutstandingAmount>();
            ServiceEntitys companys = new ServiceEntitys();

            foreach (var invoice in listOfInvoic.OrderBy(s => s.DocDate)/*.ThenBy(s => s.CreatedDate)*/)
            {

                SOAOutstandingAmount statementModel = new SOAOutstandingAmount();
                statementModel.DocNo = invoice.DocNo;
                statementModel.ServiceCompanyId = invoice.ServiceCompanyId;
                statementModel.DocDate = invoice.DocDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
                statementModel.DocType = invoice.DocType;
                statementModel.Currency = invoice.Currency;
                string doctotal = Math.Round(invoice.GrandTotal, 2).ToString("N2") != null ? Math.Round(invoice.GrandTotal, 2).ToString("N2") : Math.Round(invoice.GrandTotal, 2).ToString("N2");
                statementModel.DocumentTotal = invoice.DocType == "Credit Note" ? "(" + doctotal + ")" : doctotal;
                decimal docAmount = Math.Round(invoice.DocBalanceAmount, 2);
                string docbal = docAmount < 0 ? (docAmount * -1).ToString() : docAmount.ToString();
                statementModel.DocBalance = invoice.DocType == "Credit Note" ? "(" + docbal + ")" : docbal;
                statementModel.CreditNoteBalance = Math.Round(invoice.DocBalanceAmount, 2);
                statementModel.BaseAmount = Math.Round(invoice.GrandTotal, 2).ToString("N2") != null ? Math.Round(invoice.GrandTotal, 2).ToString("N2") : Math.Round(invoice.GrandTotal, 2).ToString("N2");
                statementModel.ReceivedAmount = invoice.DocType == "Credit Note" ? (Math.Round(invoice.GrandTotal, 2) + docAmount).ToString() : (Math.Round(invoice.GrandTotal, 2) - docAmount).ToString();
                statementModel.Aging = ((DateTime.Now.Date - invoice.DocDate.Date).TotalDays).ToString();
                mustacheModel.StatementModel = statementModel;
                dd.Add(statementModel);
            }
            SOAOutstanding outstanding = new SOAOutstanding();
            //mustacheModel.ReceiptDetailsModel = new List<Templates.Template.Receipt>();
            string str = localization != null ? localization.ShortDateFormat : DateTime.UtcNow.ToString("dd/MM/yyyy");
            string duplicateDocBal = string.Empty;

            #region Comment By Satya
            //foreach (var journal in journelLst.OrderBy(s => s.DocDate).ThenBy(s => s.CreatedDate))
            //{
            //    serviceCompanyId = journal.ServiceCompanyId.Value;
            //    SOAOutstandingAmount statementModel = new SOAOutstandingAmount();
            //    statementModel.DocNo = journal.DocNo;
            //    statementModel.ServiceCompanyId = journal.ServiceCompanyId.Value;
            //    statementModel.DocDate = journal.DocDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
            //    statementModel.DocType = journal.DocType;
            //    statementModel.Currency = journal.DocCurrency;
            //    statementModel.Remarks = journal.RefNo;
            //    string doctotal = Math.Round(journal.GrandDocDebitTotal.Value, 2).ToString("N2") != null ? Math.Round(journal.GrandDocDebitTotal.Value, 2).ToString("N2") : Math.Round(journal.GrandDocCreditTotal.Value, 2).ToString("N2");
            //    statementModel.DocumentTotal = journal.DocType == "Credit Note" ? "(" + doctotal + ")" : doctotal;
            //    string docbal = journal.BalanceAmount != null ? Math.Round(journal.BalanceAmount.Value, 2).ToString("N2") : 0.ToString("N2");
            //    statementModel.DocBalance = journal.DocType == "Credit Note" ? "(" + docbal + ")" : docbal;
            //    statementModel.CreditNoteBalance = journal.DocType == "Credit Note" ? (journal.BalanceAmount != null ? Math.Round(-journal.BalanceAmount.Value, 2) : 0) : Math.Round(journal.BalanceAmount.Value, 2);
            //    mustacheModel.StatementModel = statementModel;
            //    dd.Add(statementModel);
            //}

            #endregion

            mustacheModel.SoaDetail = dd.Where(s => s.DocBalance != 0.ToString("N2")).ToList();
            List<Templates.Template.ServiceEntity> lstserviceEntity = new List<Templates.Template.ServiceEntity>();
            foreach (var serviceCompanys in dd.Select(s => s.ServiceCompanyId).Distinct())
            {
                Templates.Template.ServiceEntity serviceEntity = new Templates.Template.ServiceEntity();
                // var serviceCompany = _templateService.GetServiceCompany(serviceCompanys);
                var banks = _templateService.GetBank(serviceCompanys);

                if (banks != null)
                {
                    serviceEntity.BankName = banks.Name;
                    serviceEntity.SWIFTCode = banks.SwiftCode;
                    serviceEntity.BankAddress = banks.BankAddress;
                    serviceEntity.AccountNumber = banks.AccountNumber;
                    serviceEntity.AccountName = banks.AccountName;
                }
                lstserviceEntity.Add(serviceEntity);
            }

            mustacheModel.Banks = lstserviceEntity;
            List<OutstandingTotals> lstoutstanding = new List<OutstandingTotals>();
            var currencyLst = mustacheModel.SoaDetail.GroupBy(s => new { Currency = s.Currency/*, SubTotal = Convert.ToDecimal(s.DocBalance)*/ }).Select(d => new { d.Key, Sum = d.Sum(s => Convert.ToDecimal(s.CreditNoteBalance)) }).ToList();

            foreach (var currency in currencyLst)
            {
                OutstandingTotals outstandingTotals = new OutstandingTotals();
                outstandingTotals.Currency = currency.Key.Currency;
                outstandingTotals.SubTotal = currency.Sum.ToString("N2");
                decimal subtotal = Convert.ToDecimal(currency.Sum);
                string total = subtotal < 0M ? "(" + subtotal.ToString("N2") + ")" : subtotal.ToString("N2");
                outstandingTotals.SubTotal = subtotal < 0M ? total.Replace("-", string.Empty) : subtotal.ToString("N2");
                //outstandingTotals.SubTotal =s.Replace("-","");
                lstoutstanding.Add(outstandingTotals);
            }
            mustacheModel.OutstandingTotal = lstoutstanding;
            //mustacheModel.OutstandingBalance = (Math.Round(dd.Sum(a => Convert.ToDecimal(a.DocBalance)))).ToString("N2");
            outstanding.DocNo = string.Join("<br />", dd.Select(s => s.DocNo).ToList());
            //taxCodeName.TaxRate = string.Join("<br />", lstTaxCode.Select(a => a.TaxRate).ToList());
            outstanding.DocType = string.Join("<br />", dd.Select(a => a.DocType).ToList());
            outstanding.DocDate = string.Join("<br />", dd.Select(s => s.DocDate).ToList());
            outstanding.DocumentTotal = string.Join("<br />", dd.Select(s => s.DocumentTotal).ToList());
            outstanding.Currency = string.Join("<br />", dd.Select(s => s.Currency).ToList());
            outstanding.DocBalance = string.Join("<br />", dd.Select(s => s.DocBalance).ToList());
            outstanding.BaseAmount = string.Join("<br />", dd.Select(s => s.BaseAmount).ToList());
            outstanding.ReceivedAmount = string.Join("<br />", dd.Select(s => s.ReceivedAmount).ToList());
            outstanding.Aging = string.Join("<br />", dd.Select(s => s.Aging).ToList());
            mustacheModel.Outstanding = outstanding;
        }
        private void FillSoaEntity2MustacheModel(EmailModel templateVM, MustacheModel mustacheModel, Localization localization)
        {
            var journelLst = _templateService.GetJournal(templateVM.ScreenId, templateVM.CompanyId.Value);
            var lstinvoices = _templateService.GetAllInvoice(templateVM.ScreenId, templateVM.CompanyId.Value);

            var listOfInvoic = GetSpRecords(templateVM.CompanyId.Value, DateTime.Now, templateVM.ScreenId);


            List<SOAOutstandingAmount> dd = new List<SOAOutstandingAmount>();
            ServiceEntitys companys = new ServiceEntitys();

            foreach (var invoice in listOfInvoic.OrderBy(s => s.DocDate)/*.ThenBy(s => s.CreatedDate)*/)
            {

                SOAOutstandingAmount statementModel = new SOAOutstandingAmount();
                statementModel.DocNo = invoice.DocNo;
                statementModel.ServiceCompanyId = invoice.ServiceCompanyId;
                statementModel.DocDate = invoice.DocDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
                statementModel.DocType = invoice.DocType;
                statementModel.Currency = invoice.Currency;
                string doctotal = Math.Round(invoice.GrandTotal, 2).ToString("N2") != null ? Math.Round(invoice.GrandTotal, 2).ToString("N2") : Math.Round(invoice.GrandTotal, 2).ToString("N2");
                statementModel.DocumentTotal = invoice.DocType == "Credit Note" ? "(" + doctotal + ")" : doctotal;
                string docbal = invoice.BaseBalanceAmount != null ? Math.Round(invoice.BaseBalanceAmount, 2).ToString("N2") : 0.ToString("N2");
                statementModel.DocBalance = invoice.DocType == "Credit Note" ? "(" + docbal + ")" : docbal;
                statementModel.CreditNoteBalance = invoice.DocType == "Credit Note" ? (invoice.BaseBalanceAmount != null ? Math.Round(invoice.BaseBalanceAmount, 2) : 0) : Math.Round(invoice.BaseBalanceAmount, 2);
                mustacheModel.StatementModel = statementModel;
                dd.Add(statementModel);
            }
            long serviceCompanyId = 0;
            SOAOutstanding outstanding = new SOAOutstanding();
            //mustacheModel.ReceiptDetailsModel = new List<Templates.Template.Receipt>();
            string str = localization != null ? localization.ShortDateFormat : DateTime.UtcNow.ToString("dd/MM/yyyy");
            string duplicateDocBal = string.Empty;

            #region Comment By Satya
            //foreach (var journal in journelLst.OrderBy(s => s.DocDate).ThenBy(s => s.CreatedDate))
            //{
            //    serviceCompanyId = journal.ServiceCompanyId.Value;
            //    SOAOutstandingAmount statementModel = new SOAOutstandingAmount();
            //    statementModel.DocNo = journal.DocNo;
            //    statementModel.ServiceCompanyId = journal.ServiceCompanyId.Value;
            //    statementModel.DocDate = journal.DocDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
            //    statementModel.DocType = journal.DocType;
            //    statementModel.Currency = journal.DocCurrency;
            //    statementModel.Remarks = journal.RefNo;
            //    string doctotal = Math.Round(journal.GrandDocDebitTotal.Value, 2).ToString("N2") != null ? Math.Round(journal.GrandDocDebitTotal.Value, 2).ToString("N2") : Math.Round(journal.GrandDocCreditTotal.Value, 2).ToString("N2");
            //    statementModel.DocumentTotal = journal.DocType == "Credit Note" ? "(" + doctotal + ")" : doctotal;
            //    string docbal = journal.BalanceAmount != null ? Math.Round(journal.BalanceAmount.Value, 2).ToString("N2") : 0.ToString("N2");
            //    statementModel.DocBalance = journal.DocType == "Credit Note" ? "(" + docbal + ")" : docbal;
            //    statementModel.CreditNoteBalance = journal.DocType == "Credit Note" ? (journal.BalanceAmount != null ? Math.Round(-journal.BalanceAmount.Value, 2) : 0) : Math.Round(journal.BalanceAmount.Value, 2);
            //    mustacheModel.StatementModel = statementModel;
            //    dd.Add(statementModel);
            //}

            #endregion

            mustacheModel.SoaDetail = dd.Where(s => s.DocBalance != 0.ToString("N2")).ToList();
            List<Templates.Template.ServiceEntity> lstserviceEntity = new List<Templates.Template.ServiceEntity>();
            foreach (var serviceCompanys in dd.Select(s => s.ServiceCompanyId).Distinct())
            {
                Templates.Template.ServiceEntity serviceEntity = new Templates.Template.ServiceEntity();
                // var serviceCompany = _templateService.GetServiceCompany(serviceCompanys);
                var banks = _templateService.GetBank(serviceCompanys);

                if (banks != null)
                {
                    serviceEntity.BankName = banks.Name;
                    serviceEntity.SWIFTCode = banks.SwiftCode;
                    serviceEntity.BankAddress = banks.BankAddress;
                    serviceEntity.AccountNumber = banks.AccountNumber;
                    serviceEntity.AccountName = banks.AccountName;
                }
                lstserviceEntity.Add(serviceEntity);
            }

            mustacheModel.Banks = lstserviceEntity;
            List<OutstandingTotals> lstoutstanding = new List<OutstandingTotals>();
            var currencyLst = mustacheModel.SoaDetail.GroupBy(s => new { Currency = s.Currency/*, SubTotal = Convert.ToDecimal(s.DocBalance)*/ }).Select(d => new { d.Key, Sum = d.Sum(s => Convert.ToDecimal(s.CreditNoteBalance)) }).ToList();

            foreach (var currency in currencyLst)
            {
                OutstandingTotals outstandingTotals = new OutstandingTotals();
                outstandingTotals.Currency = currency.Key.Currency;
                outstandingTotals.SubTotal = currency.Sum.ToString("N2");
                decimal subtotal = Convert.ToDecimal(currency.Sum);
                string total = subtotal < 0M ? "(" + subtotal.ToString("N2") + ")" : subtotal.ToString("N2");
                outstandingTotals.SubTotal = subtotal < 0M ? total.Replace("-", string.Empty) : subtotal.ToString("N2");
                //outstandingTotals.SubTotal =s.Replace("-","");
                lstoutstanding.Add(outstandingTotals);
            }
            mustacheModel.OutstandingTotal = lstoutstanding;
            //mustacheModel.OutstandingBalance = (Math.Round(dd.Sum(a => Convert.ToDecimal(a.DocBalance)))).ToString("N2");
            outstanding.DocNo = string.Join("<br />", dd.Select(s => s.DocNo).ToList());
            //taxCodeName.TaxRate = string.Join("<br />", lstTaxCode.Select(a => a.TaxRate).ToList());
            outstanding.DocType = string.Join("<br />", dd.Select(a => a.DocType).ToList());
            outstanding.DocDate = string.Join("<br />", dd.Select(s => s.DocDate).ToList());
            outstanding.DocumentTotal = string.Join("<br />", dd.Select(s => s.DocumentTotal).ToList());
            outstanding.Currency = string.Join("<br />", dd.Select(s => s.Currency).ToList());
            outstanding.DocBalance = string.Join("<br />", dd.Select(s => s.DocBalance).ToList());
            mustacheModel.Outstanding = outstanding;
        }

        private List<InvoiceModel> GetSpRecords(long CompanyId, DateTime date, Guid EntityId, string username)
        {
            string connectionString = CommonConnection.DBConnection;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[BC_SOA_Template]", con);
                cmd.Parameters.AddWithValue("@Tenantid", CompanyId);
                cmd.Parameters.AddWithValue("@AsOf", date);
                cmd.Parameters.AddWithValue("@EntitiId", EntityId);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);
                string JsonINVTbl = JsonConvert.SerializeObject(dt);
                List<InvoiceModel> details = JsonConvert.DeserializeObject<List<InvoiceModel>>(JsonINVTbl).ToList();
                con.Close();
                return details;
            }
        }
        private List<InvoiceModel> GetSpRecords(long CompanyId, DateTime date, Guid EntityId)
        {
            string connectionString = CommonConnection.DBConnection;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[BC_SOA_Template12]", con);
                cmd.Parameters.AddWithValue("@Tenantid", CompanyId);
                cmd.Parameters.AddWithValue("@AsOf", date);
                cmd.Parameters.AddWithValue("@EntitiId", EntityId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);
                string JsonINVTbl = JsonConvert.SerializeObject(dt);
                List<InvoiceModel> details = JsonConvert.DeserializeObject<List<InvoiceModel>>(JsonINVTbl).ToList();
                con.Close();
                return details;
            }
        }


        private void FillReceiptEntit2MustacheModel(EmailModel templateVM, MustacheModel mustacheModel, Localization localization)
        {
            //var receipt = _receiptService.Query(v => v.Id == templateVM.ScreenId).Include(v => v.ReceiptDetails).Select().FirstOrDefault();
            var receipt = _templateService.GetReceipt(templateVM.ScreenId);
            Templates.Template.Receipt receipts = new Templates.Template.Receipt();
            BankReceiptForApplications detail = new BankReceiptForApplications();
            List<BankReceiptForApplications> lstdetail = new List<BankReceiptForApplications>();
            mustacheModel.receipt = new AppsWorld.TemplateModule.Models.V2.Receipt();
            mustacheModel.receipt.RecieptDate = receipt.CreatedDate?.ToString(localization != null ? localization.ShortDateFormat : "yyyy-MM-dd");
            mustacheModel.receipt.ModeOfReceipt = receipt.ModeOfReceipt;
            mustacheModel.receipt.BRCurrency = receipt.BankReceiptAmmountCurrency;
            mustacheModel.receipt.RRNumber = receipt.SystemRefNo;
            mustacheModel.receipt.DocNo = receipt.DocNo;
            mustacheModel.receipt.DocumentDescription = receipt.Remarks;
            mustacheModel.receipt.DocDate = receipt.DocDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
            //mustacheModel.receiptModel.BankReceiptCurrency = receipt.DocCurrency;
            mustacheModel.receipt.BRAmount = (Math.Round(Convert.ToDecimal(receipt.BankReceiptAmmount), 2)).ToString("N2");
            mustacheModel.receipt.RAAmmount = (Math.Round(Convert.ToDecimal(receipt.ReceiptApplicationAmmount), 2)).ToString("N2");
            //mustacheModel.receipt.DocumentDescription = receipt.DocumentDescription;
            mustacheModel.receipt.BRFApplication = new List<BankReceiptForApplications>();
            mustacheModel.Receipt = mustacheModel.receipt;
            var ServiceCompanyDetails = _templateService.GetServiceEntityNameById(receipt.CompanyId);
            foreach (var receptDetail in receipt.ReceiptDetails.Where(s => s.ReceiptAmount != 0).ToList())
            {
                BankReceiptForApplications rDetailmodel = new BankReceiptForApplications();
                rDetailmodel.DocNo = receptDetail.DocumentNo;
                rDetailmodel.DocType = receptDetail.DocumentType;
                rDetailmodel.DocTotal = Math.Round(receipt.GrandTotal, 2).ToString("N2");
                rDetailmodel.Amount = receptDetail.DocumentType == "Credit Note" ? "(" + Math.Round(receptDetail.DocumentAmmount, 2).ToString("N2") + ")" : receptDetail.DocumentType == "Bill" ? "(" + Math.Round(receptDetail.DocumentAmmount, 2).ToString("N2") + ")" : Math.Round(receptDetail.DocumentAmmount, 2).ToString("N2");
                rDetailmodel.Balance = receptDetail.DocumentType == "Credit Note" ? "(" + Math.Round(receptDetail.AmmountDue, 2).ToString("N2") + ")" : receptDetail.DocumentType == "Bill" ? "(" + Math.Round(receptDetail.AmmountDue, 2).ToString("N2") + ")" : Math.Round(receptDetail.AmmountDue, 2).ToString("N2");
                rDetailmodel.Receipt = receptDetail.DocumentType == "Credit Note" ? "(" + Math.Round(receptDetail.ReceiptAmount, 2).ToString("N2") + ")" : receptDetail.DocumentType == "Bill" ? "(" + Math.Round(receptDetail.ReceiptAmount, 2).ToString("N2") + ")" : Math.Round(receptDetail.ReceiptAmount, 2).ToString("N2");
                rDetailmodel.DocCurrency = receptDetail.Currency;
                //company = ServiceCompanyDetails.Where(x => x.Id == receptDetail.ServiceCompanyId).FirstOrDefault();
                rDetailmodel.Co = ServiceCompanyDetails != null ? ServiceCompanyDetails.Where(x => x.Id == receptDetail.ServiceCompanyId).FirstOrDefault().Name.ToString() : null;
                rDetailmodel.DocDate = receptDetail.DocumentDate.Date.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
                mustacheModel.receipt.BRFApplication.Add(rDetailmodel);

                lstdetail.Add(rDetailmodel);

            }

            //mustacheModel.receiptModel.BankReceiptAmount = (mustacheModel.receiptModel.BankReceiptForApplication.Select(q => Convert.ToDecimal(q.Amount)).Sum()).ToString("N2");
            mustacheModel.TotalReceiptApplicationAmount = (receipt.ReceiptDetails.Where(s => s.ReceiptAmount != 0).ToList().Select(q => Convert.ToDecimal(q.ReceiptAmount)).Sum()).ToString("N2");

            detail.DocNo = string.Join("<br />", mustacheModel.receipt.BRFApplication.Select(a => a.DocNo).ToList());
            //taxCodeName.TaxRate = string.Join("<br />", lstTaxCode.Select(a => a.TaxRate).ToList());
            detail.DocType = string.Join("<br />", mustacheModel.receipt.BRFApplication.Select(a => a.DocType).ToList());
            detail.DocTotal = string.Join("<br />", (mustacheModel.receipt.BRFApplication.Select(q => Convert.ToDecimal(q.DocTotal).ToString("N2"))));
            detail.Amount = string.Join("<br />", mustacheModel.receipt.BRFApplication.Select(a => a.Amount).ToList());
            detail.DocCurrency = string.Join("<br />", mustacheModel.receipt.BRFApplication.Select(s => s.DocCurrency));
            detail.DocDate = string.Join("<br />", mustacheModel.receipt.BRFApplication.Select(s => s.DocDate));
            detail.Co = string.Join("<br />", mustacheModel.receipt.BRFApplication.Select(s => s.Co));
            mustacheModel.BankReceiptForApplication = detail;
            //mustacheModel.BRFADetail = lstdetail;
        }

        private AppsWorld.TemplateModule.Models.V2.Invoices FillInvoiceTemplateEntity2Model(Invoice invoice, MustacheModel mustacheModel, Localization localization, string gstnumber)
        {
            AppsWorld.TemplateModule.Models.V2.Invoices templateVm = new AppsWorld.TemplateModule.Models.V2.Invoices();
            templateVm.DocNo = invoice.DocNo;
            templateVm.CreditTerms = _templateService.GetTermsOfPaymentById(invoice.CreditTermsId, invoice.CompanyId);
            templateVm.PO = invoice.PONo;
            templateVm.DocumentDescription = invoice.DocDescription;
            templateVm.Currency = invoice.DocCurrency;
            templateVm.CreatedDate = invoice.CreatedDate;
            templateVm.BaseCurrency = localization.BaseCurrency;
            //string s= string.Format("{0.0000000000}", invoice.ExchangeRate);
            templateVm.ExchangeRate = (invoice.ExchangeRate).ToString();
            templateVm.DocumentDescription = invoice.DocDescription;
            templateVm.BalanceAmount = string.Format("{0:n}", Math.Round(invoice.GrandTotal, 2));
            templateVm.BalanceAmount = Math.Round(invoice.GrandTotal, 2).ToString("0,0.00",
          CultureInfo.InvariantCulture);
            templateVm.DocDate = invoice.DocDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
            templateVm.DueDate = invoice.DueDate.Value.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
            List<AppsWorld.TemplateModule.Models.V2.LineItems> lstLineItem = new List<AppsWorld.TemplateModule.Models.V2.LineItems>();
            List<TaxCodess> lstTaxCode = new List<TaxCodess>();
            //Templates.Template.LineItem LineItem = new Templates.Template.LineItem();
            LineItems LineItem = new LineItems();
            Templates.Template.TaxCode taxCodeName = new Templates.Template.TaxCode();
            GSTReporting gstreporting = new GSTReporting();
            List<GSTReporting> lstgst = new List<GSTReporting>();
            List<Item> Lstitems = _templateService.GetAllItems(invoice.CompanyId);
            decimal TotalTax = 0;
            foreach (var invoiceItem in invoice.InvoiceDetails.Where(s => s.DocAmount != 0).OrderBy(s => s.RecOrder))
            {
                TaxCodess taxCode = new TaxCodess();
                AppsWorld.TemplateModule.Models.V2.LineItems Item = new AppsWorld.TemplateModule.Models.V2.LineItems();
                GSTReporting gst = new GSTReporting();
                Entities.Models.V2.TaxCode taxtcodendName = _templateService.GetTaxCode(invoiceItem.TaxId);
                //Templates.Template.LineItem LineItem = new Templates.Template.LineItem();
                Item.ItemCode = Lstitems.Where(s => s.Id == invoiceItem.ItemId).Select(s => s.Code).FirstOrDefault();
                Item.ItemDescription = invoiceItem.ItemDescription;
                Item.Quantity = invoiceItem.Qty.ToString();
                Item.UnitPrice = Math.Round(invoiceItem.UnitPrice.Value, 2).ToString("N2");
                LineItem.Unit = invoiceItem.Unit;
                Item.TaxCode = taxtcodendName != null ? invoiceItem.TaxIdCode == "NA" ? taxtcodendName.Code : invoiceItem.TaxIdCode : "";
                Item.BaseAmount = Math.Round(invoiceItem.BaseAmount.Value, 2).ToString("N2");
                Item.BaseTotal = Math.Round(invoiceItem.BaseTotalAmount.Value, 2).ToString("N2");
                Item.Amount = Math.Round(invoiceItem.DocAmount, 2).ToString("N2");
                Item.Currency = invoiceItem.AmtCurrency;

                Item.TaxAmount = invoiceItem.DocTaxAmount != null ? (Decimal.Round(invoiceItem.DocTaxAmount.Value, 2).ToString("N2")) : "";
                Item.Total = invoiceItem.DocTotalAmount != null ? (Math.Round(invoiceItem.DocTotalAmount, 2).ToString("N2")) : "";
                Item.Discount = invoiceItem.Discount != null ? (Math.Round(invoiceItem.Discount.Value, 2).ToString("N2")) : "";
                Item.DocNo = invoice.DocNo;
                Item.DocDate = invoice.DocDate.ToString();
                Item.Unit = invoiceItem.Unit != null ? invoiceItem.Unit : "";

                lstLineItem.Add(Item);

                TotalTax += invoiceItem.DocTaxAmount != null ? (Decimal.Round(invoiceItem.DocTaxAmount.Value, 2)) : 0;

                //Templates.Template.TaxCode taxCodeName = new Templates.Template.TaxCode();
                taxCode.TaxCodes = invoiceItem.TaxIdCode != null ? invoiceItem.TaxIdCode : "";
                taxCode.TaxName = taxtcodendName != null ? taxtcodendName.Name : "";
                taxCode.TaxRate = (invoiceItem.TaxRate != null ? Math.Round(invoiceItem.TaxRate.Value, 2) : (double?)null);
                if (invoiceItem.DocTaxAmount != null)
                    taxCode.SubTotal = Math.Round(invoiceItem.DocTaxAmount.Value, 2).ToString("N2");
                lstTaxCode.Add(taxCode);

                //gst.Amount = invoiceItem.BaseAmount != null ? Math.Round(invoiceItem.BaseAmount.Value, 2).ToString("N2") : "0";
                //gst.TaxAmount = invoiceItem.BaseTaxAmount != null ? Math.Round(invoiceItem.BaseTaxAmount.Value, 2).ToString("N2") : "0";
                //gst.TotalAmount = (Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)).ToString("N2");
                //gst.ExchangeRate =(invoice.GSTExchangeRate).ToString();

                //gst.Amount = Convert.ToString(invoiceItem.BaseAmount);
                //gst.TaxAmount = invoiceItem.BaseTaxAmount != null ? Math.Round(invoiceItem.BaseTaxAmount.Value, 2).ToString("N2") : "0";
                //gst.TotalAmount = (Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)).ToString("N2");
                //need  to ccheck

                gst.Amount = invoiceItem.DocAmount != null ? Decimal.Round((invoiceItem.DocAmount) * (invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value) : 1), 2, MidpointRounding.AwayFromZero).ToString("N2") : "0";
                gst.TaxAmount = invoiceItem.DocTaxAmount != null ? (Decimal.Round((invoiceItem.DocTaxAmount.Value) * (invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value) : 1), 2, MidpointRounding.AwayFromZero).ToString("N2")) : "0";
                //gst.TotalAmount = ((invoiceItem.DocAmount) * invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value)) + ((invoiceItem.DocTaxAmount.Value) * invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value) : 1;

                gst.TotalAmount = Math.Round((Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)), 2, MidpointRounding.AwayFromZero).ToString("N2");

                //(Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)).ToString("N2");
                lstgst.Add(gst);

                //gst.TotalAmount = (Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(invoiceItem.crr)).ToString("N2");

                //gst.Amount = invoiceItem.DocAmount!=null?(Convert.ToString(invoiceItem.DocAmount)):"";
                //gst.TaxAmount = (invoiceItem.TaxRate != null ? (invoiceItem.DocAmount / (Convert.ToDecimal(invoiceItem.TaxRate) != null ? Convert.ToDecimal(invoiceItem.TaxRate) : 0M)) : 0).ToString("N2");
                //gst.TotalAmount = (Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)).ToString("N2");
            }

            templateVm.TaxTotal = TotalTax.ToString();

            mustacheModel.IsGSTActive = invoice.IsGstSettings == true ? true : false;
            mustacheModel.IsGSTNotActive = invoice.IsGstSettings == false ? true : false;

            mustacheModel.ISFoexCurrency = localization.BaseCurrency != invoice.DocCurrency ? true : false;
            mustacheModel.ISNotFoexCurrency = localization.BaseCurrency == invoice.DocCurrency ? true : false;

            mustacheModel.IsGST = (invoice.IsGstSettings == true && invoice.DocCurrency != "SGD") ? true : false;
            mustacheModel.ItemDetail = lstLineItem.OrderBy(x => x.DocDate).ToList();
            templateVm.Total = Math.Round(lstLineItem.Select(b => Convert.ToDecimal(b.Amount)).Sum(), 2).ToString("N2");
            templateVm.SubTotal = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("N2");

            string total = Math.Round(lstLineItem.Select(b => Convert.ToDecimal(b.Amount)).Sum(), 2).ToString("N2"); ;
            string subtotal = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("N2");
            templateVm.SubTotal = (Convert.ToDecimal(total) + (subtotal != null ? Convert.ToDecimal(subtotal) : 0)).ToString("N2");

            templateVm.GSTPayable = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("N2");
            templateVm.ExcludeGST = Math.Round(lstLineItem.Select(v => Convert.ToDecimal(v.Amount)).Sum(), 2).ToString("N2");
            templateVm.IncludeGST = Math.Round(invoice.InvoiceDetails.Select(b => b.BaseTotalAmount).Sum().Value, 2).ToString("N2");

            gstreporting.Amount = lstgst.Sum(s => Convert.ToDouble(s.Amount)).ToString("N2");
            gstreporting.TaxAmount = lstgst.Sum(s => Convert.ToDouble(s.TaxAmount)).ToString("N2");
            gstreporting.TotalAmount = lstgst.Sum(s => Convert.ToDouble(s.TotalAmount)).ToString("N2");
            gstreporting.ExchangeRate = (invoice.GSTExchangeRate).ToString();

            mustacheModel.InvoiceAmountDue = invoice.BalanceAmount.ToString("N2");
            mustacheModel.GSTReporting = gstreporting;
            taxCodeName.TaxName = string.Join("<br />", lstTaxCode.Select(a => a.TaxName).ToList());
            //need tocheck
            //taxCodeName.TaxRate = string.Join("<br />", lstTaxCode.Select(a => Convert.ToString( a.TaxRate)));
            LineItem.Amount = string.Join("<br />", lstLineItem.Select(a => a.Amount).ToList());
            LineItem.Currency = string.Join("<br />", lstLineItem.Select(a => a.Currency).ToList());
            LineItem.Discount = string.Join("<br />", lstLineItem.Select(a => a.Discount).ToList());
            LineItem.ItemCode = string.Join("<br />", lstLineItem.Select(a => a.ItemCode).ToList());
            LineItem.ItemDescription = string.Join("<br />", lstLineItem.Select(a => a.ItemDescription).ToList());
            LineItem.Quantity = string.Join("<br />", (lstLineItem.Select(a => a.Quantity)).ToList());
            LineItem.TaxAmount = string.Join("<br />", lstLineItem.Select(a => a.TaxAmount).ToList());
            LineItem.TaxCode = string.Join("<br />", lstLineItem.Select(a => a.TaxCode).ToList());
            LineItem.Total = string.Join("<br />", lstLineItem.Select(a => a.Total).ToList());
            LineItem.UnitPrice = string.Join("<br />", lstLineItem.Select(a => a.UnitPrice).ToList());
            LineItem.DocNo = string.Join("<br />", lstLineItem.Select(a => a.DocNo).ToList());
            //LineItem.Quantity = Double.Parse(string.Join("<br />", lstLineItem.Select(a => a.Quantity).ToList()));
            LineItem.DocDate = string.Join("<br />", lstLineItem.Select(a => a.DocDate).ToList());
            LineItem.BaseAmount = string.Join("<br />", lstLineItem.Select(a => a.BaseAmount).ToList());
            LineItem.BaseTotal = string.Join("<br />", lstLineItem.Select(a => a.BaseTotal).ToList());

            List<AppsWorld.TemplateModule.Models.V2.TaxCode> lsttaxcodes = new List<AppsWorld.TemplateModule.Models.V2.TaxCode>();


            //var taxLst = lstTaxCode.GroupBy(s => new { Taxcode = s.TaxName/*, SubTotal = Convert.ToDecimal(s.DocBalance)*/ }).Select(d => new { d.Key, Sum = d.Sum(s => Convert.ToDecimal(s.SubTotal)) }).ToList();

            var query = (from t in lstTaxCode
                         group t by new { t.TaxCodes, t.TaxName, t.TaxRate }
                         into grp
                         select new
                         {
                             grp.Key.TaxCodes,
                             grp.Key.TaxName,
                             grp.Key.TaxRate,
                             SubTotal = grp.Sum(t => Convert.ToDecimal(t.SubTotal))
                         }).ToList();

            foreach (var currency in query)
            {
                AppsWorld.TemplateModule.Models.V2.TaxCode outstandingTotals = new AppsWorld.TemplateModule.Models.V2.TaxCode();
                outstandingTotals.TaxName = currency.TaxName;
                outstandingTotals.TaxCodes = currency.TaxCodes;
                outstandingTotals.SubTotal = currency.SubTotal.ToString("N2");
                decimal subtotal1 = Convert.ToDecimal(currency.SubTotal);
                string total1 = subtotal1 < 0M ? "(" + subtotal1.ToString("N2") + ")" : subtotal1.ToString("N2");
                outstandingTotals.SubTotal = subtotal1 < 0M ? total1.Replace("-", string.Empty) : subtotal1.ToString("N2");
                //outstandingTotals.SubTotal =s.Replace("-","");
                outstandingTotals.TaxRate = currency.TaxRate;
                lsttaxcodes.Add(outstandingTotals);
            }
            //mustacheModel.LstTaxCode = lstoutstanding;



            mustacheModel.TaxCode = lsttaxcodes.Where(s => !s.TaxCodes.Contains("NA")).ToList();
            mustacheModel.IsTaxCodeHide = mustacheModel.TaxCode.Count > 0 ? true : false;
            mustacheModel.LineItem = LineItem;

            if (invoice.DocType == "Invoice" || invoice.DocType == "Credit Note")
            {
                mustacheModel.Invoice = new AppsWorld.TemplateModule.Models.V2.Invoices();
                mustacheModel.Invoice.LineItem = new List<LineItems>();
                mustacheModel.Invoice.TaxCode = new List<TaxCodess>();
                mustacheModel.Invoice = templateVm;
                mustacheModel.Invoice.LineItem = lstLineItem;
                mustacheModel.Invoice.TaxCode = lstTaxCode;
                if (invoice.DocType == "Credit Note")
                {
                    mustacheModel.CreditNote = new AppsWorld.TemplateModule.Models.V2.Invoices();
                    mustacheModel.CreditNote.LineItem = new List<LineItems>();
                    mustacheModel.CreditNote.TaxCode = new List<TaxCodess>();
                    mustacheModel.CreditNote = templateVm;
                    mustacheModel.CreditNote.LineItem = lstLineItem;
                    mustacheModel.CreditNote.TaxCode = lstTaxCode;
                }

            }
            //else
            //{
            //    mustacheModel.CreditNote = new Templates.Template.Invoice();
            //    mustacheModel.CreditNote.LineItem = new List<Templates.Template.LineItem>();
            //    mustacheModel.CreditNote.TaxCode = new List<Templates.Template.TaxCode>();
            //    mustacheModel.CreditNote = templateVm;
            //    mustacheModel.CreditNote.LineItem = lstLineItem;
            //    mustacheModel.CreditNote.TaxCode = lstTaxCode;
            //}

            return templateVm;
        }
        private AppsWorld.TemplateModule.Models.V2.Invoices FillCreditNoteTemplateEntity2Model(Invoice invoice, Company company, MustacheModel mustacheModel, Localization localization, string gstnumber)
        {
            AppsWorld.TemplateModule.Models.V2.Invoices templateVm = new AppsWorld.TemplateModule.Models.V2.Invoices();
            templateVm.DocNo = invoice.DocNo;
            templateVm.Currency = invoice.DocCurrency;
            templateVm.DocumentDescription = invoice.DocDescription;
            templateVm.CreatedDate = invoice.CreatedDate;
            templateVm.CreditTerms = _templateService.GetTermsOfPaymentById(invoice.CreditTermsId, invoice.CompanyId);
            templateVm.DocDate = invoice.DocDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
            templateVm.DueDate = invoice.DueDate.Value.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
            templateVm.BalanceAmount = string.Format("{0:n}", Math.Round(invoice.GrandTotal, 2));
            templateVm.BalanceAmount = Math.Round(invoice.GrandTotal, 2).ToString("0,0.00",
          CultureInfo.InvariantCulture);
            templateVm.DocumentDescription = invoice.DocDescription;
            templateVm.ExchangeRate = invoice.ExchangeRate.ToString();
            templateVm.BaseCurrency = localization.BaseCurrency;
            List<LineItems> lstLineItem = new List<LineItems>();
            List<TaxCodess> lstTaxCode = new List<TaxCodess>();
            LineItems LineItem = new LineItems();
            //Templates.Template.TaxCode taxCodeName = new Templates.Template.TaxCode();
            TaxCodess taxCodeName = new TaxCodess();

            GSTReporting gstreporting = new GSTReporting();
            List<Templates.Template.GSTReporting> lstgst = new List<Templates.Template.GSTReporting>();
            decimal TotalTax = 0;
            foreach (var invoiceItem in invoice.InvoiceDetails.Where(s => s.DocAmount != 0).OrderBy(s => s.RecOrder))
            {

                ChartOfAccount taxtcodendName = _templateService.GetChartOfAccount(invoiceItem.COAId);
                Entities.Models.V2.TaxCode taxtcodendName1 = _templateService.GetTaxCode(invoiceItem.TaxId);
                TaxCodess taxCode = new TaxCodess();
                LineItems Item = new LineItems();

                Templates.Template.GSTReporting gst = new Templates.Template.GSTReporting();

                Item.ItemCode = taxtcodendName.Name;
                Item.ItemDescription = invoiceItem.ItemDescription;
                // Item.Quantity = invoiceItem.Qty.ToString();
                Item.UnitPrice = invoiceItem.UnitPrice != null ? Math.Round(invoiceItem.UnitPrice.Value, 2).ToString("N2") : "";
                //LineItem.Unit = invoiceItem.Unit;
                Item.TaxAmount = invoiceItem.DocTaxAmount != null ? (Decimal.Round(invoiceItem.DocTaxAmount.Value, 2).ToString("N2")) : "";
                Item.Total = invoiceItem.DocTotalAmount != null ? (Math.Round(invoiceItem.DocTotalAmount, 2).ToString("N2")) : "";
                Item.TaxCode = taxtcodendName1 != null ? invoiceItem.TaxIdCode == "NA" ? taxtcodendName1.Code : invoiceItem.TaxIdCode : "";
                Item.Amount = Math.Round(invoiceItem.DocAmount, 2).ToString("N2");
                Item.Currency = invoiceItem.AmtCurrency;
                Item.BaseAmount = Math.Round(invoiceItem.BaseAmount.Value, 2).ToString("N2");
                Item.BaseTotal = Math.Round(invoiceItem.BaseTotalAmount.Value, 2).ToString("N2");
                lstLineItem.Add(Item);


                taxCode.TaxCodes = invoiceItem.TaxIdCode != null ? invoiceItem.TaxIdCode : "";
                taxCode.TaxName = taxtcodendName1 != null ? taxtcodendName1.Name : "";
                taxCode.TaxRate = (invoiceItem.TaxRate != null ? Math.Round(invoiceItem.TaxRate.Value, 2) : (double?)null);
                taxCodeName.TaxRate = (invoiceItem.TaxRate != null ? Math.Round(invoiceItem.TaxRate.Value, 2) : (double?)null);
                taxCode.SubTotal = invoiceItem.DocTaxAmount != null ? Math.Round(invoiceItem.DocTaxAmount.Value, 2).ToString("N2") : 0.ToString();
                lstTaxCode.Add(taxCode);

                TotalTax += invoiceItem.DocTaxAmount != null ? (Decimal.Round(invoiceItem.DocTaxAmount.Value, 2)) : 0;

                //gst.Amount = Convert.ToString(invoiceItem.BaseAmount);
                ////need  to ccheck

                //gst.TaxAmount = invoiceItem.BaseTaxAmount != null ? Math.Round(invoiceItem.BaseTaxAmount.Value, 2).ToString("N2") : "0";
                //gst.TotalAmount = (Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)).ToString("N2");

                gst.Amount = invoiceItem.DocAmount != null ? Decimal.Round((invoiceItem.DocAmount) * (invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value) : 1), 2, MidpointRounding.AwayFromZero).ToString("N2") : "0";
                gst.TaxAmount = invoiceItem.DocTaxAmount != null ? (Decimal.Round((invoiceItem.DocTaxAmount.Value) * (invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value) : 1), 2, MidpointRounding.AwayFromZero).ToString("N2")) : "0";
                //gst.TotalAmount = ((invoiceItem.DocAmount) * invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value)) + ((invoiceItem.DocTaxAmount.Value) * invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value) : 1;

                gst.TotalAmount = Math.Round((Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)), 2, MidpointRounding.AwayFromZero).ToString("N2");

                lstgst.Add(gst);

            }
            templateVm.TaxTotal = TotalTax.ToString();
            List<AppsWorld.TemplateModule.Models.V2.TaxCode> lsttaxcodes = new List<AppsWorld.TemplateModule.Models.V2.TaxCode>();


            var query = (from t in lstTaxCode
                         group t by new { t.TaxCodes, t.TaxName, t.TaxRate }
                         into grp
                         select new
                         {
                             grp.Key.TaxCodes,
                             grp.Key.TaxName,
                             grp.Key.TaxRate,
                             SubTotal = grp.Sum(t => Convert.ToDecimal(t.SubTotal))
                         }).ToList();

            foreach (var currency in query)
            {
                AppsWorld.TemplateModule.Models.V2.TaxCode outstandingTotals = new AppsWorld.TemplateModule.Models.V2.TaxCode();
                outstandingTotals.TaxName = currency.TaxName;
                outstandingTotals.TaxCodes = currency.TaxCodes;
                outstandingTotals.TaxRate = currency.TaxRate;
                outstandingTotals.SubTotal = currency.SubTotal.ToString("N2");
                decimal subtotal1 = Convert.ToDecimal(currency.SubTotal);
                string total1 = subtotal1 < 0M ? "(" + subtotal1.ToString("N2") + ")" : subtotal1.ToString("N2");
                outstandingTotals.SubTotal = subtotal1 < 0M ? total1.Replace("-", string.Empty) : subtotal1.ToString("N2");
                //outstandingTotals.SubTotal =s.Replace("-","");
                lsttaxcodes.Add(outstandingTotals);
            }
            mustacheModel.TaxCode = lsttaxcodes.Where(s => !s.TaxCodes.Contains("NA")).ToList();

            mustacheModel.ItemDetail = lstLineItem;
            //mustacheModel.IsGST = gstnumber != null ? true : false;
            //mustacheModel.IsGST = invoice.IsGstSettings == true ? true : false;
            mustacheModel.IsTaxCodeHide = mustacheModel.TaxCode.Count > 0 ? true : false;
            mustacheModel.IsGSTActive = invoice.IsGstSettings == true ? true : false;
            mustacheModel.IsGSTNotActive = invoice.IsGstSettings == false ? true : false;
            mustacheModel.IsGST = (invoice.IsGstSettings == true && invoice.DocCurrency != "SGD") ? true : false;

            mustacheModel.ISFoexCurrency = localization.BaseCurrency != invoice.DocCurrency ? true : false;
            mustacheModel.ISNotFoexCurrency = localization.BaseCurrency == invoice.DocCurrency ? true : false;

            gstreporting.Amount = lstgst.Sum(s => Convert.ToDouble(s.Amount)).ToString("N2");
            gstreporting.TaxAmount = lstgst.Sum(s => Convert.ToDouble(s.TaxAmount)).ToString("N2");
            gstreporting.TotalAmount = lstgst.Sum(s => Convert.ToDouble(s.TotalAmount)).ToString("N2");
            gstreporting.ExchangeRate = (invoice.GSTExchangeRate).ToString();

            mustacheModel.GSTReporting = gstreporting;
            templateVm.Total = Math.Round(lstLineItem.Select(b => Convert.ToDecimal(b.Amount)).Sum(), 2).ToString("N2");
            templateVm.SubTotal = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("N2");
            string total = Math.Round(lstLineItem.Select(b => Convert.ToDecimal(b.Amount)).Sum(), 2).ToString("N2"); ;
            string subtotal = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("N2");
            templateVm.SubTotal = (Convert.ToDecimal(total) + (subtotal != null ? Convert.ToDecimal(subtotal) : 0)).ToString("N2");

            templateVm.GSTPayable = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("N2");
            templateVm.ExcludeGST = Math.Round(lstLineItem.Select(v => Convert.ToDecimal(v.Amount)).Sum(), 2).ToString("N2");
            templateVm.IncludeGST = Math.Round(invoice.InvoiceDetails.Select(b => b.BaseTotalAmount).Sum().Value, 2).ToString("N2");

            if (invoice.DocType == "Invoice" || invoice.DocType == "Credit Note")
            {
                mustacheModel.Invoice = new AppsWorld.TemplateModule.Models.V2.Invoices();
                mustacheModel.Invoice.LineItem = new List<LineItems>();
                mustacheModel.Invoice.TaxCode = new List<TaxCodess>();
                mustacheModel.Invoice = templateVm;
                mustacheModel.Invoice.LineItem = lstLineItem;
                mustacheModel.Invoice.TaxCode = lstTaxCode;
                if (invoice.DocType == "Credit Note")
                {
                    mustacheModel.CreditNote = new AppsWorld.TemplateModule.Models.V2.Invoices();
                    mustacheModel.CreditNote.LineItem = new List<LineItems>();
                    mustacheModel.CreditNote.TaxCode = new List<TaxCodess>();
                    mustacheModel.CreditNote = templateVm;
                    mustacheModel.CreditNote.LineItem = lstLineItem;
                    mustacheModel.CreditNote.TaxCode = lstTaxCode;
                }

            }

            taxCodeName.TaxName = string.Join("<br />", lstTaxCode.Select(a => a.TaxName).ToList());
            //taxCodeName.TaxRate = string.Join("<br />", lstTaxCode.Select(a => a.TaxRate).ToList());
            LineItem.Amount = string.Join("<br />", lstLineItem.Select(a => a.Amount).ToList());
            LineItem.Currency = string.Join("<br />", lstLineItem.Select(a => a.Currency).ToList());
            LineItem.Discount = string.Join("<br />", lstLineItem.Select(a => a.Discount).ToList());
            LineItem.ItemCode = string.Join("<br />", lstLineItem.Select(a => a.ItemCode).ToList());
            LineItem.ItemDescription = string.Join("<br />", lstLineItem.Select(a => a.ItemDescription).ToList());
            LineItem.Quantity = (string.Join("<br />", lstLineItem.Select(a => a.Quantity).ToList()));
            LineItem.TaxAmount = string.Join("<br />", lstLineItem.Select(a => a.TaxAmount).ToList());
            LineItem.TaxCode = string.Join("<br />", lstLineItem.Select(a => a.TaxCode).ToList());
            LineItem.Total = string.Join("<br />", lstLineItem.Select(a => a.Total).ToList());
            LineItem.UnitPrice = string.Join("<br />", lstLineItem.Select(a => a.UnitPrice).ToList());
            LineItem.BaseAmount = string.Join("<br />", lstLineItem.Select(a => a.BaseAmount).ToList());
            LineItem.BaseTotal = string.Join("<br />", lstLineItem.Select(a => a.BaseTotal).ToList());
            mustacheModel.LineItem = LineItem;
            return templateVm;
        }


        private AppsWorld.TemplateModule.Models.V2.Invoices FillDebitNoteTemplateEntity2Model(DebitNote debitnote, Company company, MustacheModel mustacheModel, Localization localization, string gstnumber)
        {
            try
            {


                AppsWorld.TemplateModule.Models.V2.Invoices templateVm = new AppsWorld.TemplateModule.Models.V2.Invoices();
                templateVm.DocNo = debitnote.DocNo;
                templateVm.DocDate = debitnote.DocDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
                templateVm.DueDate = debitnote.DueDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
                templateVm.DocumentDescription = debitnote.Remarks;
                templateVm.Currency = debitnote.DocCurrency;
                templateVm.CreatedDate = debitnote.CreatedDate;
                templateVm.BalanceAmount = Math.Round(debitnote.GrandTotal, 2).ToString("N2");

                templateVm.CreditTerms = _templateService.GetTermsOfPaymentById(debitnote.CreditTermsId, debitnote.CompanyId);
                templateVm.ExchangeRate = debitnote.ExchangeRate.ToString();
                templateVm.DocumentDescription = debitnote.Remarks != null ? debitnote.Remarks.ToString() : null;

                List<LineItems> lstLineItem = new List<LineItems>();
                List<TaxCodess> lstTaxCode = new List<TaxCodess>();
                LineItems LineItem = new LineItems();
                TaxCodess taxCodeName = new TaxCodess();

                GSTReporting gstreporting = new GSTReporting();
                List<Templates.Template.GSTReporting> lstgst = new List<Templates.Template.GSTReporting>();

                foreach (var invoiceItem in debitnote.DebitNoteDetails.Where(s => s.DocAmount != 0).OrderBy(s => s.RecOrder))
                {

                    ChartOfAccount taxtcodendName = _templateService.GetChartOfAccount(invoiceItem.COAId);
                    Entities.Models.V2.TaxCode taxtcodendName1 = _templateService.GetTaxCode(invoiceItem.TaxId);
                    TaxCodess taxCode = new TaxCodess();
                    LineItems Item = new LineItems();
                    Templates.Template.GSTReporting gst = new Templates.Template.GSTReporting();
                    Item.ItemCode = taxtcodendName.Name;
                    Item.ItemDescription = invoiceItem.AccountDescription;
                    Item.DocNo = templateVm.DocNo;
                    Item.DocDate = templateVm.DocDate;
                    //Item.Quantity = invoiceItem.Qty;
                    //Item.UnitPrice = invoiceItem.UnitPrice != null ? Math.Round(invoiceItem.UnitPrice.Value, 2).ToString("N2") : "";
                    //LineItem.Unit = invoiceItem.Unit;
                    Item.TaxCode = taxtcodendName1 != null ? invoiceItem.TaxIdCode == "NA" ? taxtcodendName1.Code : invoiceItem.TaxIdCode : "";
                    Item.TaxAmount = invoiceItem.DocTaxAmount != null ? (Decimal.Round(invoiceItem.DocTaxAmount.Value, 2).ToString("N2")) : "";
                    Item.Amount = Math.Round(invoiceItem.DocAmount, 2).ToString("N2");
                    Item.Total = invoiceItem.DocTotalAmount != null ? (Decimal.Round(invoiceItem.DocTotalAmount, 2).ToString("N2")) : "";
                    //Item.Currency = invoiceItem.DocAmount;
                    lstLineItem.Add(Item);


                    taxCode.TaxCodes = invoiceItem.TaxIdCode != null ? invoiceItem.TaxIdCode : "";
                    taxCode.TaxName = taxtcodendName1 != null ? taxtcodendName1.Name : "";
                    //taxCode.TaxCodes = Item.TaxCode.ToString();
                    taxCode.TaxRate = (invoiceItem.TaxRate != null ? Math.Round(invoiceItem.TaxRate.Value, 2) : (double?)null);
                    //taxCodeName.TaxRate = (invoiceItem.TaxRate != null ? Math.Round(invoiceItem.TaxRate.Value, 2) : (double?)null);
                    taxCode.SubTotal = invoiceItem.DocTaxAmount != null ? Math.Round(invoiceItem.DocTaxAmount.Value, 2).ToString("N2") : 0.ToString();
                    lstTaxCode.Add(taxCode);
                    //gst.Amount = Convert.ToString(invoiceItem.BaseAmount);
                    ////need  to ccheck
                    //gst.TaxAmount = invoiceItem.BaseTaxAmount != null ? Math.Round(invoiceItem.BaseTaxAmount.Value, 2).ToString("N2") : "0";
                    //gst.TotalAmount = (Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)).ToString("N2");

                    gst.Amount = invoiceItem.DocAmount != null ? Decimal.Round((invoiceItem.DocAmount) * (debitnote.GSTExchangeRate != null ? (debitnote.GSTExchangeRate.Value) : 1), 2, MidpointRounding.AwayFromZero).ToString("N2") : "0";
                    gst.TaxAmount = invoiceItem.DocTaxAmount != null ? (Decimal.Round((invoiceItem.DocTaxAmount.Value) * (debitnote.GSTExchangeRate != null ? (debitnote.GSTExchangeRate.Value) : 1), 2, MidpointRounding.AwayFromZero).ToString("N2")) : "0";
                    //gst.TotalAmount = ((invoiceItem.DocAmount) * invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value)) + ((invoiceItem.DocTaxAmount.Value) * invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value) : 1;

                    gst.TotalAmount = Math.Round((Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)), 2, MidpointRounding.AwayFromZero).ToString("N2");
                    lstgst.Add(gst);

                }

                List<AppsWorld.TemplateModule.Models.V2.TaxCode> lsttaxcodes = new List<AppsWorld.TemplateModule.Models.V2.TaxCode>();
                //var taxLst = lstTaxCode.GroupBy(s => new { Taxcode = s.TaxName/*, SubTotal = Convert.ToDecimal(s.DocBalance)*/ }).Select(d => new { d.Key, Sum = d.Sum(s => Convert.ToDecimal(s.SubTotal)) }).ToList();

                var query = (from t in lstTaxCode
                             group t by new { t.TaxCodes, t.TaxName, t.TaxRate }
                          into grp
                             select new
                             {
                                 grp.Key.TaxCodes,
                                 grp.Key.TaxName,
                                 grp.Key.TaxRate,
                                 SubTotal = grp.Sum(t => Convert.ToDecimal(t.SubTotal))
                             }).ToList();

                foreach (var currency in query)
                {
                    AppsWorld.TemplateModule.Models.V2.TaxCode outstandingTotals = new AppsWorld.TemplateModule.Models.V2.TaxCode();
                    outstandingTotals.TaxName = currency.TaxName;
                    outstandingTotals.TaxRate = currency.TaxRate;
                    outstandingTotals.TaxCodes = currency.TaxCodes;
                    outstandingTotals.SubTotal = currency.SubTotal.ToString("N2");
                    decimal subtotal1 = Convert.ToDecimal(currency.SubTotal);
                    string total1 = subtotal1 < 0M ? "(" + subtotal1.ToString("N2") + ")" : subtotal1.ToString("N2");
                    outstandingTotals.SubTotal = subtotal1 < 0M ? total1.Replace("-", string.Empty) : subtotal1.ToString("N2");
                    //outstandingTotals.SubTotal =s.Replace("-","");
                    lsttaxcodes.Add(outstandingTotals);
                }
                mustacheModel.TaxCode = lsttaxcodes.Where(s => !s.TaxCodes.Contains("NA")).ToList();
                mustacheModel.LineItem = LineItem;
                mustacheModel.ItemDetail = lstLineItem.OrderBy(x => x.DocDate).ToList();
                mustacheModel.IsTaxCodeHide = mustacheModel.TaxCode.Count > 0 ? true : false;
                //mustacheModel.IsTaxCodeNotHide = mustacheModel.TaxCode.Count < 0 ? true : false;
                mustacheModel.IsGSTActive = debitnote.IsGstSettings == true ? true : false;
                mustacheModel.IsGSTNotActive = debitnote.IsGstSettings == false ? true : false;
                mustacheModel.IsGST = (debitnote.IsGstSettings == true && debitnote.DocCurrency != "SGD") ? true : false;
                gstreporting.Amount = lstgst.Sum(s => Convert.ToDouble(s.Amount)).ToString("N2");
                ////now   
                gstreporting.TaxAmount = lstgst.Sum(s => Convert.ToDouble(s.TaxAmount)).ToString("N2");
                gstreporting.TotalAmount = lstgst.Sum(s => Convert.ToDouble(s.TotalAmount)).ToString("N2");
                gstreporting.ExchangeRate = (debitnote.GSTExchangeRate).ToString(); ;
                mustacheModel.GSTReporting = gstreporting;
                templateVm.Total = Math.Round(lstLineItem.Select(b => Convert.ToDecimal(b.Amount)).Sum(), 2).ToString("N2");
                templateVm.SubTotal = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("N2");
                string total = Math.Round(lstLineItem.Select(b => Convert.ToDecimal(b.Amount)).Sum(), 2).ToString("N2"); ;
                string subtotal = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("N2");
                templateVm.SubTotal = (Convert.ToDecimal(total) + (subtotal != null ? Convert.ToDecimal(subtotal) : 0)).ToString("N2");

                templateVm.GSTPayable = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("N2");
                templateVm.ExcludeGST = Math.Round(lstLineItem.Select(v => Convert.ToDecimal(v.Amount)).Sum(), 2).ToString("N2");
                templateVm.IncludeGST = Math.Round(debitnote.DebitNoteDetails.Select(b => b.BaseTotalAmount).Sum().Value, 2).ToString("N2");

                if (debitnote.DocSubType == "Debit Note")
                {
                    mustacheModel.DebitNote = new AppsWorld.TemplateModule.Models.V2.Invoices();
                    mustacheModel.DebitNote.LineItem = new List<LineItems>();
                    mustacheModel.DebitNote.TaxCode = new List<TaxCodess>();
                    mustacheModel.DebitNote = templateVm;
                    mustacheModel.DebitNote.LineItem = lstLineItem;
                    mustacheModel.DebitNote.TaxCode = lstTaxCode;

                }


                taxCodeName.TaxName = string.Join("<br />", lstTaxCode.Select(a => a.TaxName).ToList());
                //taxCodeName.TaxRate = string.Join("<br />", lstTaxCode.Select(a => a.TaxRate).ToList());
                LineItem.Amount = string.Join("<br />", lstLineItem.Select(a => a.Amount).ToList());
                LineItem.Currency = string.Join("<br />", lstLineItem.Select(a => a.Currency).ToList());
                LineItem.Discount = string.Join("<br />", lstLineItem.Select(a => a.Discount).ToList());
                LineItem.ItemCode = string.Join("<br />", lstLineItem.Select(a => a.ItemCode).ToList());
                LineItem.ItemDescription = string.Join("<br />", lstLineItem.Select(a => a.ItemDescription).ToList());
                LineItem.Quantity = string.Join("<br />", lstLineItem.Select(a => a.Quantity).ToList());
                LineItem.TaxAmount = string.Join("<br />", lstLineItem.Select(a => a.TaxAmount).ToList());
                LineItem.TaxCode = string.Join("<br />", lstLineItem.Select(a => a.TaxCode).ToList());
                LineItem.Total = string.Join("<br />", lstLineItem.Select(a => a.Total).ToList());
                LineItem.UnitPrice = string.Join("<br />", lstLineItem.Select(a => a.UnitPrice).ToList());


                return templateVm;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private AppsWorld.TemplateModule.Models.V2.Invoices FillCashSaleTemplateEntity2Model(CashSale debitnote, Company company, MustacheModel mustacheModel, Localization localization, string gstnumber)
        {
            try
            {


                AppsWorld.TemplateModule.Models.V2.Invoices templateVm = new AppsWorld.TemplateModule.Models.V2.Invoices();
                templateVm.DocNo = debitnote.DocNo;
                templateVm.DocDate = debitnote.DocDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
                //templateVm.DueDate = debitnote.DueDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
                templateVm.DocumentDescription = debitnote.DocDescription;
                templateVm.Currency = debitnote.DocCurrency;
                templateVm.CreatedDate = debitnote.CreatedDate;
                //templateVm.BalanceAmount = Math.Round(debitnote.GrandTotal, 2).ToString("N2");
                templateVm.BalanceAmount = string.Format("{0:n}", Math.Round(debitnote.GrandTotal, 2));
                templateVm.BalanceAmount = Math.Round(debitnote.GrandTotal, 2).ToString("0,0.00",
              CultureInfo.InvariantCulture);

                templateVm.PO = debitnote.PONo;
                templateVm.ExchangeRate = debitnote.ExchangeRate.ToString();
                templateVm.DocumentDescription = debitnote.DocDescription;

                List<LineItems> lstLineItem = new List<LineItems>();
                List<TaxCodess> lstTaxCode = new List<TaxCodess>();
                LineItems LineItem = new LineItems();
                TaxCodess taxCodeName = new TaxCodess();

                GSTReporting gstreporting = new GSTReporting();
                List<Templates.Template.GSTReporting> lstgst = new List<Templates.Template.GSTReporting>();
                List<Item> Lstitems = _templateService.GetAllItems(debitnote.CompanyId);
                foreach (var invoiceItem in debitnote.CashSaleDetails.Where(s => s.DocAmount != 0).OrderBy(s => s.RecOrder))
                {

                    Entities.Models.V2.TaxCode taxtcodendName1 = _templateService.GetTaxCode(invoiceItem.TaxId);
                    TaxCodess taxCode = new TaxCodess();
                    LineItems Item = new LineItems();
                    Templates.Template.GSTReporting gst = new Templates.Template.GSTReporting();
                    Item.ItemCode = Lstitems.Where(s => s.Id == invoiceItem.ItemId).Select(s => s.Code).FirstOrDefault();
                    Item.ItemDescription = invoiceItem.ItemDescription;
                    Item.DocNo = templateVm.DocNo;
                    Item.DocDate = templateVm.DocDate;
                    Item.Quantity = invoiceItem.Qty != null ? invoiceItem.Qty.ToString() : "";
                    Item.Discount = invoiceItem.Discount != null ? invoiceItem.Discount.ToString() : "";
                    Item.TaxAmount = invoiceItem.DocTaxAmount != null ? (Decimal.Round(invoiceItem.DocTaxAmount.Value, 2).ToString("N2")) : "";
                    Item.Total = invoiceItem.DocTotalAmount != null ? (Math.Round(invoiceItem.DocTotalAmount, 2).ToString("N2")) : "";

                    Item.UnitPrice = Math.Round(invoiceItem.UnitPrice.Value, 2).ToString("0,0.00",
                 CultureInfo.InvariantCulture) == "00.00" ? "0.00" : Math.Round(invoiceItem.UnitPrice.Value, 2).ToString("0,0.00",
                 CultureInfo.InvariantCulture);
                    //LineItem.Unit = invoiceItem.Unit;
                    Item.TaxCode = taxtcodendName1 != null ? invoiceItem.TaxIdCode == "NA" ? taxtcodendName1.Code : invoiceItem.TaxIdCode : "";
                    Item.Amount = Math.Round(invoiceItem.DocAmount, 2).ToString("0,0.00",
                 CultureInfo.InvariantCulture) == "00.00" ? "0.00" : Math.Round(invoiceItem.DocAmount, 2).ToString("0,0.00",
                 CultureInfo.InvariantCulture);
                    //Item.Currency = invoiceItem.DocAmount;
                    lstLineItem.Add(Item);


                    taxCode.TaxCodes = invoiceItem.TaxIdCode != null ? invoiceItem.TaxIdCode : "";
                    taxCode.TaxName = taxtcodendName1 != null ? taxtcodendName1.Name : "";
                    //taxCode.TaxCodes = Item.TaxCode.ToString();
                    taxCode.TaxRate = (invoiceItem.TaxRate != null ? Math.Round(invoiceItem.TaxRate.Value, 2) : (double?)null);
                    //taxCodeName.TaxRate = (invoiceItem.TaxRate != null ? Math.Round(invoiceItem.TaxRate.Value, 2) : (double?)null);
                    taxCode.SubTotal = invoiceItem.DocTaxAmount != null ? Math.Round(invoiceItem.DocTaxAmount.Value, 2).ToString("N2") : 0.ToString();

                    lstTaxCode.Add(taxCode);
                    //gst.Amount = Convert.ToString(invoiceItem.BaseAmount);
                    ////need  to ccheck
                    //gst.TaxAmount = invoiceItem.BaseTaxAmount != null ? Math.Round(invoiceItem.BaseTaxAmount.Value, 2).ToString("N2") : "0";
                    //gst.TotalAmount = (Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)).ToString("N2");

                    gst.Amount = invoiceItem.DocAmount != null ? Decimal.Round((invoiceItem.DocAmount) * (debitnote.GSTExchangeRate != null ? (debitnote.GSTExchangeRate.Value) : 1), 2, MidpointRounding.AwayFromZero).ToString("N2") : "0";
                    gst.TaxAmount = invoiceItem.DocTaxAmount != null ? (Decimal.Round((invoiceItem.DocTaxAmount.Value) * (debitnote.GSTExchangeRate != null ? (debitnote.GSTExchangeRate.Value) : 1), 2, MidpointRounding.AwayFromZero).ToString("N2")) : "0";
                    //gst.TotalAmount = ((invoiceItem.DocAmount) * invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value)) + ((invoiceItem.DocTaxAmount.Value) * invoice.GSTExchangeRate != null ? (invoice.GSTExchangeRate.Value) : 1;

                    gst.TotalAmount = Math.Round((Convert.ToDecimal(gst.Amount) + Convert.ToDecimal(gst.TaxAmount)), 2, MidpointRounding.AwayFromZero).ToString("N2");
                    lstgst.Add(gst);

                }

                List<AppsWorld.TemplateModule.Models.V2.TaxCode> lsttaxcodes = new List<AppsWorld.TemplateModule.Models.V2.TaxCode>();
                //var taxLst = lstTaxCode.GroupBy(s => new { Taxcode = s.TaxName/*, SubTotal = Convert.ToDecimal(s.DocBalance)*/ }).Select(d => new { d.Key, Sum = d.Sum(s => Convert.ToDecimal(s.SubTotal)) }).ToList();

                var query = (from t in lstTaxCode
                             group t by new { t.TaxCodes, t.TaxName, t.TaxRate }
                          into grp
                             select new
                             {
                                 grp.Key.TaxCodes,
                                 grp.Key.TaxName,
                                 grp.Key.TaxRate,
                                 SubTotal = grp.Sum(t => Convert.ToDecimal(t.SubTotal))
                             }).ToList();

                foreach (var currency in query)
                {
                    AppsWorld.TemplateModule.Models.V2.TaxCode outstandingTotals = new AppsWorld.TemplateModule.Models.V2.TaxCode();
                    outstandingTotals.TaxName = currency.TaxName;
                    outstandingTotals.TaxRate = currency.TaxRate;
                    outstandingTotals.TaxCodes = currency.TaxCodes;
                    outstandingTotals.SubTotal = currency.SubTotal.ToString("0,0.00",
               CultureInfo.InvariantCulture) == "00.00" ? "0.00" : currency.SubTotal.ToString("0,0.00",
               CultureInfo.InvariantCulture);
                    decimal subtotal1 = Convert.ToDecimal(currency.SubTotal);
                    string total1 = subtotal1 < 0M ? "(" + subtotal1.ToString("0,0.00",
                CultureInfo.InvariantCulture) == "00.00" ? "0.00" : subtotal1.ToString("0,0.00",
                CultureInfo.InvariantCulture) + ")" : subtotal1.ToString("0,0.00",
                CultureInfo.InvariantCulture) == "00.00" ? "0.00" : subtotal1.ToString("0,0.00",
                CultureInfo.InvariantCulture);
                    outstandingTotals.SubTotal = subtotal1 < 0M ? total1.Replace("-", string.Empty) : subtotal1.ToString("N2") == "00.00" ? "0.00" : subtotal1.ToString("N2");
                    //outstandingTotals.SubTotal =s.Replace("-","");
                    lsttaxcodes.Add(outstandingTotals);
                }
                mustacheModel.TaxCode = lsttaxcodes.Where(s => !s.TaxCodes.Contains("NA")).ToList();
                mustacheModel.LineItem = LineItem;
                mustacheModel.ItemDetail = lstLineItem;
                mustacheModel.IsTaxCodeHide = mustacheModel.TaxCode.Count > 0 ? true : false;
                mustacheModel.IsGSTActive = debitnote.IsGstSettings == true ? true : false;
                mustacheModel.IsGSTNotActive = debitnote.IsGstSettings == false ? true : false;
                mustacheModel.IsGST = (debitnote.IsGstSettings == true && debitnote.DocCurrency != "SGD") ? true : false;
                gstreporting.Amount = lstgst.Sum(s => Convert.ToDouble(s.Amount)).ToString("N2");
                ////now   
                gstreporting.TaxAmount = lstgst.Sum(s => Convert.ToDouble(s.TaxAmount)).ToString("N2");
                gstreporting.TotalAmount = lstgst.Sum(s => Convert.ToDouble(s.TotalAmount)).ToString("N2");
                gstreporting.ExchangeRate = (debitnote.GSTExchangeRate).ToString();
                mustacheModel.GSTReporting = gstreporting;
                templateVm.Total = Math.Round(lstLineItem.Select(b => Convert.ToDecimal(b.Amount)).Sum(), 2).ToString("0,0.00",
                 CultureInfo.InvariantCulture);
                templateVm.SubTotal = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("0,0.00",
                CultureInfo.InvariantCulture);
                string total = Math.Round(lstLineItem.Select(b => Convert.ToDecimal(b.Amount)).Sum(), 2).ToString("0,0.00",
                CultureInfo.InvariantCulture);
                string subtotal = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("0,0.00",
                CultureInfo.InvariantCulture);
                templateVm.SubTotal = (Convert.ToDecimal(total) + (subtotal != null ? Convert.ToDecimal(subtotal) : 0)).ToString("0,0.00",
                CultureInfo.InvariantCulture);

                templateVm.GSTPayable = Math.Round(lstTaxCode.Select(v => Convert.ToDecimal(v.SubTotal)).Sum(), 2).ToString("0,0.00",
                CultureInfo.InvariantCulture);
                templateVm.ExcludeGST = Math.Round(lstLineItem.Select(v => Convert.ToDecimal(v.Amount)).Sum(), 2).ToString("0,0.00",
                CultureInfo.InvariantCulture);
                templateVm.IncludeGST = Math.Round(debitnote.CashSaleDetails.Select(b => b.BaseTotalAmount).Sum().Value, 2).ToString("0,0.00",
                CultureInfo.InvariantCulture);

                if (debitnote.DocType == "Cash Sale")
                {
                    mustacheModel.CashSale = new AppsWorld.TemplateModule.Models.V2.Invoices();
                    mustacheModel.CashSale.LineItem = new List<LineItems>();
                    mustacheModel.CashSale.TaxCode = new List<TaxCodess>();
                    mustacheModel.CashSale = templateVm;
                    mustacheModel.CashSale.LineItem = lstLineItem;
                    mustacheModel.CashSale.TaxCode = lstTaxCode;

                }


                taxCodeName.TaxName = string.Join("<br />", lstTaxCode.Select(a => a.TaxName).ToList());
                //taxCodeName.TaxRate = string.Join("<br />", lstTaxCode.Select(a => a.TaxRate).ToList());
                LineItem.Amount = string.Join("<br />", lstLineItem.Select(a => a.Amount).ToList());
                LineItem.Currency = string.Join("<br />", lstLineItem.Select(a => a.Currency).ToList());
                LineItem.Discount = string.Join("<br />", lstLineItem.Select(a => a.Discount).ToList());
                LineItem.ItemCode = string.Join("<br />", lstLineItem.Select(a => a.ItemCode).ToList());
                LineItem.ItemDescription = string.Join("<br />", lstLineItem.Select(a => a.ItemDescription).ToList());
                LineItem.Quantity = string.Join("<br />", lstLineItem.Select(a => a.Quantity).ToList());
                LineItem.TaxAmount = string.Join("<br />", lstLineItem.Select(a => a.TaxAmount).ToList());
                LineItem.TaxCode = string.Join("<br />", lstLineItem.Select(a => a.TaxCode).ToList());
                LineItem.Total = string.Join("<br />", lstLineItem.Select(a => a.Total).ToList());
                LineItem.UnitPrice = string.Join("<br />", lstLineItem.Select(a => a.UnitPrice).ToList());


                return templateVm;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static string StringCharactersReplaceFunction(string name)
        {
            name = name.Replace('"', '_').Replace('\\', '_').Replace('/', '_')
                  .Replace(':', '_').Replace('|', '_').Replace('<', '_').Replace('>', '_').Replace('*', '_').Replace('?', '_').Replace("'", "_");
            Regex re = new Regex("[.]*(?=[.]$)");
            name = re.Replace(name, "");
            name = name.EndsWith(".") ? name.Remove(name.Length - 1) : name;
            name = name.Trim();
            return name;
        }
    }
}
