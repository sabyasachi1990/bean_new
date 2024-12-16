using AppsWorld.BillModule.Entities;
using AppsWorld.CommonModule.Application;
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.Infra;
using AppsWorld.ReminderModule.Entities.Entities;
using AppsWorld.ReminderModule.Models.Models;
using AppsWorld.ReminderModule.RepositoryPattern;
using AppsWorld.ReminderModule.Service;
using AppsWorld.ReminderModule.Service.V2Service;
using AppsWorld.TemplateModule.Infra;
using AppsWorld.TemplateModule.Models.V2;
using HtmlAgilityPack;
using Mustache;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ziraff.EmailProvider;

namespace AppsWorld.ReminderModule.Application
{
    public class ReminderApplicationService
    {
        #region Constructor 
        private readonly IReminderSaveService _reminderService;
        private readonly IReminderModuleUnitOfWorkAsync _unitOfWorkAysnc;
        private readonly IReminderKService _reminderKService;
        public ReminderApplicationService(IReminderSaveService reminderService, IReminderModuleUnitOfWorkAsync unitOfWork, IReminderKService reminderKService)
        {
            this._reminderService = reminderService;
            this._unitOfWorkAysnc = unitOfWork;
            this._reminderKService = reminderKService;
        }

        #endregion

        public async Task<IQueryable<ReminderVMK>> GetAllReminderskNewOld(long companyId, DateTime? fromDate, DateTime? toDate, string type, string name)
        {
            return await _reminderService.GetReminderskNew(companyId, fromDate, toDate, type, name);
        }
        public async Task<IQueryable<ReminderVMK>> GetAllReminderskNew(long companyId, DateTime? fromDate, DateTime? toDate, string type, string name)
        {
            return await _reminderKService.GetReminderskNew(companyId, fromDate, toDate, type, name);
        }
        public LookUpCategory<string> ReminderLookp(long companyId)
        {
            try
            {
                var controlcodeCategory = _reminderService.GetByCategoryCodeCategoryByCursorName(companyId, "Reminders");
                return controlcodeCategory;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string ReminderSendEmail(ReminderMailModel model, string connectionString, string AzureconnectionString, string fromemail, long companyId)
        {
            try
            {
                //if (model.ToEmail != null)
                //{
                LocalizationCompact localization = _reminderService.GetLocalizationByCompanyId(companyId);
                TemplateMenuModel templatemenuModel = new TemplateMenuModel();
                templatemenuModel.StatementModel = new StatementModel();

                Models.Models.SOAOutstandingAmount invoiceModel = new Models.Models.SOAOutstandingAmount();

                foreach (var id in model.Ids)
                {
                    Guid? RemId = model.Ids.FirstOrDefault();
                    List<Models.Models.SOAOutstandingAmount> lstInvoiceModel = new List<Models.Models.SOAOutstandingAmount>();
                    List<Models.Models.OutstandingTotals> lstoutstanding = new List<Models.Models.OutstandingTotals>();
                    var reminderBatchList = _reminderService.GetReminderBatchList(id);
                    if (reminderBatchList != null)
                    {
                        // reminderBatchList.Recipient = "satyanarayana.ziraff@gmail.com";
                        var generictemplate = _reminderService.GetgenerictemplateById(reminderBatchList.TemplateId);
                        //model.ToEmail = model.ToEmail==null?generictemplate.ToEmailId:null;
                        EmailModelNew emailModel = new EmailModelNew();
                        var generictemplate1 = _reminderService.GetgenerictemplateByIdForPreview(companyId, "Statement Of Account");
                        string htmlHeader = null;
                        string htmlFooter = null;
                        List<Address> address = _reminderService.GetAddress(reminderBatchList.DocumentId);
                        //FormatCompiler compilar = new FormatCompiler();
                        Generator oppGenerator = null;
                        Generator oppGenerator1 = null;
                        string oppResult11 = null;
                        string oppResult1 = null;
                        decimal? OutStandingAmount = 0;
                        decimal? TotalInvoiceFee = 0;
                        Guid invoiceId = Guid.Empty;
                        string invoicenumbers = null;

                        var lstReminderDetails = reminderBatchList.ReminderBatchListDetails.ToList();
                        var lstDistinctCaseIdsWithReminder = lstReminderDetails.DistinctBy(x => x.EntityId).ToList();
                        var lstReminderInvWithReminder = lstReminderDetails.DistinctBy(x => x.DocumentId).ToList();
                        var beanentity = _reminderService.GetEntity(reminderBatchList.DocumentId);

                        if (reminderBatchList.Recipient == null && model.ToEmail == null)
                        {
                            throw new Exception("To send a reminder, please add a reminder recipient for  " + beanentity.Name);
                        }
                        if (generictemplate != null)
                        {
                            generictemplate.Subject = generictemplate.Subject != null && generictemplate.Subject != string.Empty ? generictemplate.Subject.Replace("{{Entity.Entityname}}", beanentity.Name) : null;
                            if (string.IsNullOrEmpty(generictemplate.Subject) && model.Subject == null)
                                throw new Exception("Subject is mandatory for " + generictemplate.Name + " template.");
                        }

                        foreach (var reminderWithCase in lstDistinctCaseIdsWithReminder.OrderBy(a => a.DocDate).ThenBy(c => c.DocNo))
                        {
                            var singlecaselstInvWithReminder = lstReminderDetails.Where(x => x.EntityId == reminderWithCase.EntityId).DistinctBy(v => v.DocumentId).ToList();

                            foreach (var invWithReminder in singlecaselstInvWithReminder.OrderBy(a => a.DocDate).ThenBy(c => c.DocNo))
                            {
                                CompanyModel company = GetCompany(invWithReminder.ServiceCompanyId, address);
                                //var invoice = GetInvoiceData(localization, reminderWithCase);
                                invoiceId = invWithReminder.DocumentId;
                                if (generictemplate != null)
                                {
                                    FormatCompiler compilar = new FormatCompiler();
                                    oppGenerator = compilar.Compile(generictemplate1.TempletContent);
                                    oppGenerator1 = compilar.Compile(generictemplate.TempletContent);
                                    templatemenuModel.ServiceEntity = company;
                                    templatemenuModel.StatementModel.StatementDate = reminderBatchList.JobExecutedOn != null ? reminderBatchList.JobExecutedOn?.ToString("dd/MM/yyyy") : DateTime.UtcNow.ToString("dd/MM/yyyy");
                                    invoiceModel = new Models.Models.SOAOutstandingAmount();
                                    invoiceModel.DocNo = invWithReminder.DocNo;
                                    invoiceModel.DocType = invWithReminder.DocType;
                                    invoiceModel.Currency = invWithReminder.Currency;
                                    invoiceModel.DocDate = invWithReminder.DocDate.ToString(localization != null ? localization.ShortDateFormat : "dd/MM/yyyy");
                                    string doctotamount = Math.Round(invWithReminder.DocumentTotal, 2).ToString("N2");
                                    invoiceModel.DocumentTotal = invWithReminder.DocType == "Credit Note" ? "(" + doctotamount.Replace("-", string.Empty) + ")" : Math.Round(invWithReminder.DocumentTotal, 2).ToString("N2");
                                    string docbalamount = Math.Round(((double)(invWithReminder.DocBalance)), 2).ToString("N2");
                                    invoiceModel.DocBalance = invWithReminder.DocType == "Credit Note" ? "(" + docbalamount.Replace("-", string.Empty) + ")" : Math.Round(((double)(invWithReminder.DocBalance)), 2).ToString("N2");
                                    invoiceModel.CreditNoteBalance = invWithReminder.CreditNoteBalance;
                                    invoiceModel.Remarks = invWithReminder.Remarks;
                                    lstInvoiceModel.Add(invoiceModel);
                                }
                            }
                        }
                        templatemenuModel.SoaDetail = lstInvoiceModel;
                        GetEntityAddressBookInfo(beanentity, templatemenuModel);
                        //templatemenuModel.StatementModel.StatementDate = DateTime.UtcNow.ToString("dd/MM/yyyy");
                        templatemenuModel.Entity = GetEntityInfo(beanentity);
                        var currencyLst = templatemenuModel.SoaDetail.GroupBy(s => new { Currency = s.Currency/*, SubTotal = Convert.ToDecimal(s.DocBalance)*/ }).Select(d => new { d.Key, Sum = d.Sum(s => Convert.ToDecimal(s.CreditNoteBalance)) }).ToList();
                        foreach (var currency in currencyLst)
                        {
                            Models.Models.OutstandingTotals outstandingTotals = new Models.Models.OutstandingTotals();
                            outstandingTotals.Currency = currency.Key.Currency;
                            outstandingTotals.SubTotal = currency.Sum.ToString("N2");
                            decimal subtotal = Convert.ToDecimal(currency.Sum);
                            string total = subtotal < 0M ? "(" + subtotal.ToString("N2") + ")" : subtotal.ToString("N2");
                            outstandingTotals.SubTotal = subtotal < 0M ? total.Replace("-", string.Empty) : subtotal.ToString("N2");
                            lstoutstanding.Add(outstandingTotals);
                        }
                        templatemenuModel.OutstandingTotal = lstoutstanding;
                        templatemenuModel.StatementDate = reminderBatchList.JobExecutedOn != null ? reminderBatchList.JobExecutedOn?.ToString("dd/MM/yyyy") : DateTime.UtcNow.ToString("dd/MM/yyyy");
                        if (oppGenerator != null)
                        {
                            oppResult11 = "\n" + oppGenerator.Render(templatemenuModel);
                            oppResult11 = TemplateEmailFilters(oppResult11);
                        }
                        if (oppGenerator1 != null)
                        {
                            oppResult1 = "\n" + oppGenerator1.Render(templatemenuModel);
                            oppResult1 = TemplateEmailFilters(oppResult1);
                        }
                        emailModel.TempletContent = oppResult1;
                        byte[] pdfBytes = HtmlToPdfConverter.HtmlToPDF(oppResult11, htmlHeader, htmlFooter);
                        string pdfFilename = "Statement_of_Account" + ".pdf";
                        var folderpath = "Entities" + "/" + beanentity.Name.TrimEnd('.') + "/" + "Communication";
                        if (!string.IsNullOrEmpty(reminderBatchList.Recipient))
                        {
                            var lstRecipients = reminderBatchList.Recipient.Split(',');
                            foreach (var toemail in lstRecipients)
                            {
                                Ziraff.EmailProvider.EmailProviderModel emailProviderModel = new Ziraff.EmailProvider.EmailProviderModel();
                                emailProviderModel.FromEmailAddress = fromemail != null ? fromemail : model.UserName;
                                emailProviderModel.ToEmailAddress = model.ToEmail != null ? model.ToEmail : (toemail != null ? toemail + ";" + generictemplate.ToEmailId : generictemplate.ToEmailId);
                                emailProviderModel.Subject = model.Subject != null ? model.Subject : generictemplate.Subject;
                                //generictemplate.Subject != null ? generictemplate.Subject : model.Subject /*"SOA Reminder Email "*/;
                                emailProviderModel.Body = emailModel.TempletContent;
                                emailProviderModel.CCEmailAddress = model.Ccmail == null ? generictemplate.CCEmailIds : model.Ccmail;
                                emailProviderModel.BCCEmailAddress = model.bccmail == null ? generictemplate.BCCEmailIds : model.bccmail;
                                emailProviderModel.EmailPriority = Ziraff.EmailProvider.EmailPriority.High;
                                Ziraff.EmailProvider.Attachment att = new Ziraff.EmailProvider.Attachment()
                                {
                                    Name = pdfFilename,//emailModel.DisplayFileName,
                                    ContentBytes = pdfBytes
                                };
                                emailProviderModel.attachments.Add(att);
                                Ziraff.Configuration.ThirdParty.Configuration configuration = new Ziraff.Configuration.ThirdParty.Configuration();
                                var emailConfig = configuration.GetEmailProvider(companyId, connectionString);
                                emailConfig.Type = "MailGun";
                                EmailAction emailnew = new EmailAction();
                                emailnew.SendEmail(emailConfig, emailProviderModel);
                                var azuremodel = UploadAzureByBytes(pdfFilename, pdfBytes, companyId, folderpath, model.UserFirstName);
                                CommunicationCompact communicationNew = new CommunicationCompact();
                                communicationNew.Id = Guid.NewGuid();
                                communicationNew.CommunicationType = "Email";
                                communicationNew.Date = DateTime.UtcNow;
                                communicationNew.Description = generictemplate.Subject;
                                communicationNew.FromMail = fromemail;
                                communicationNew.SentBy = fromemail;
                                communicationNew.ToMail = model.ToEmail != null ? model.ToEmail : toemail;
                                communicationNew.UserCreated = model.UserFirstName;
                                communicationNew.CreatedDate = DateTime.UtcNow;
                                communicationNew.Subject = generictemplate.Subject;
                                communicationNew.Status = RecordStatusEnum.Active;
                                communicationNew.InvoiceId = RemId;
                                communicationNew.LeadId = beanentity.Id;
                                communicationNew.CompanyId = companyId;
                                communicationNew.InvoiceNumber = invoicenumbers;
                                communicationNew.Category = "SOA Reminder";
                                communicationNew.TemplateName = generictemplate.Name;
                                communicationNew.TemplateId = generictemplate.Id;
                                communicationNew.TemplateCode = generictemplate.Code;
                                communicationNew.TemplateContent = emailModel.TempletContent;
                                communicationNew.CCMail = generictemplate.CCEmailIds;
                                communicationNew.BCCMail = generictemplate.BCCEmailIds;
                                communicationNew.FilePath = folderpath;
                                communicationNew.FileName = azuremodel.FileName;
                                communicationNew.AzurePath = azuremodel != null ? azuremodel.Path.Replace("%20", " ") : null;
                                communicationNew.Remarks = model.UserFirstName + " sent " + (communicationNew.TemplateName != null ? communicationNew.TemplateName.ToLower() : "email") + " to " + communicationNew.ToMail;
                                communicationNew.ObjectState = ObjectState.Added;
                                _reminderService.InsertCommunication(communicationNew);

                                reminderBatchList.Recipient = emailProviderModel.ToEmailAddress + ";" + emailProviderModel.CCEmailAddress + ";" + emailProviderModel.BCCEmailAddress;
                            }

                        }
                        else
                        {
                            Ziraff.EmailProvider.EmailProviderModel emailProviderModel = new Ziraff.EmailProvider.EmailProviderModel();
                            emailProviderModel.FromEmailAddress = fromemail != null ? fromemail : model.UserName;
                            emailProviderModel.ToEmailAddress = model.ToEmail != null ? model.ToEmail : generictemplate.ToEmailId;
                            emailProviderModel.Subject = model.Subject != null ? model.Subject : generictemplate.Subject;
                            emailProviderModel.Body = emailModel.TempletContent;
                            emailProviderModel.CCEmailAddress = model.Ccmail == null ? generictemplate.CCEmailIds : model.Ccmail;
                            emailProviderModel.BCCEmailAddress = model.bccmail == null ? generictemplate.BCCEmailIds : model.bccmail;
                            emailProviderModel.EmailPriority = Ziraff.EmailProvider.EmailPriority.High;
                            Ziraff.EmailProvider.Attachment att = new Ziraff.EmailProvider.Attachment()
                            {
                                Name = pdfFilename,//emailModel.DisplayFileName,
                                ContentBytes = pdfBytes
                            };
                            emailProviderModel.attachments.Add(att);
                            Ziraff.Configuration.ThirdParty.Configuration configuration = new Ziraff.Configuration.ThirdParty.Configuration();
                            var emailConfig = configuration.GetEmailProvider(companyId, connectionString);
                            emailConfig.Type = "MailGun";
                            EmailAction emailnew = new EmailAction();
                            emailnew.SendEmail(emailConfig, emailProviderModel);
                            var azuremodel = UploadAzureByBytes(pdfFilename, pdfBytes, companyId, folderpath, model.UserFirstName);
                            CommunicationCompact communicationNew = new CommunicationCompact();
                            communicationNew.Id = Guid.NewGuid();
                            communicationNew.CommunicationType = "Email";
                            communicationNew.Date = DateTime.UtcNow;
                            communicationNew.Description = generictemplate.Subject;
                            communicationNew.FromMail = fromemail;
                            communicationNew.SentBy = fromemail;
                            communicationNew.ToMail = model.ToEmail;
                            communicationNew.UserCreated = model.UserFirstName;
                            communicationNew.CreatedDate = DateTime.UtcNow;
                            communicationNew.Subject = generictemplate.Subject;
                            communicationNew.Status = RecordStatusEnum.Active;
                            communicationNew.InvoiceId = RemId;
                            communicationNew.InvoiceNumber = invoicenumbers;
                            communicationNew.Category = "SOA Reminder";
                            communicationNew.TemplateName = generictemplate.Name;
                            communicationNew.TemplateId = generictemplate.Id;
                            communicationNew.CompanyId = companyId;
                            communicationNew.LeadId = beanentity.Id;
                            communicationNew.TemplateCode = generictemplate.Code;
                            communicationNew.TemplateContent = emailModel.TempletContent;
                            communicationNew.CCMail = generictemplate.CCEmailIds;
                            communicationNew.BCCMail = generictemplate.BCCEmailIds;
                            communicationNew.FilePath = folderpath;
                            communicationNew.FileName = azuremodel.FileName;
                            communicationNew.AzurePath = azuremodel != null ? azuremodel.Path.Replace("%20", " ") : null;
                            communicationNew.Remarks = model.UserFirstName + " sent " + (communicationNew.TemplateName != null ? communicationNew.TemplateName.ToLower() : "email") + " to " + communicationNew.ToMail;
                            communicationNew.ObjectState = ObjectState.Added;
                            _reminderService.InsertCommunication(communicationNew);
                            reminderBatchList.Recipient = emailProviderModel.ToEmailAddress + ";" + emailProviderModel.CCEmailAddress + ";" + emailProviderModel.BCCEmailAddress;
                        }
                        reminderBatchList.JobStatus = "Sent";
                        reminderBatchList.ModifiedBy = model.UserFirstName;
                        reminderBatchList.ModifiedDate = DateTime.UtcNow;
                        reminderBatchList.ObjectState = ObjectState.Modified;
                        _reminderService.UpdateSOAReminderBatchList(reminderBatchList);
                    }
                    _unitOfWorkAysnc.SaveChanges();
                }
                //}
                //else
                //{
                //    return ("Tomail Is Mandatory For Send Email ");
                //}
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Success";
        }
        public AzureModel UploadAzureByBytes(string FileName, byte[] templatebyte, long companyId, string path, string usercreated)
        {
            try
            {
                string CursorName = "Bean";
                UploadFileModel uploadFileModel = new UploadFileModel();
                uploadFileModel.CompanyId = companyId;
                uploadFileModel.FileBytes = templatebyte;
                uploadFileModel.FileName = FileName;
                uploadFileModel.Path = path;
                uploadFileModel.CursorName = CursorName;
                uploadFileModel.CreatedBy = usercreated;
                var json = RestHelper.ConvertObjectToJason(uploadFileModel);
                try
                {
                    string url = ConfigurationManager.AppSettings["AzureUrl"];
                    var response = RestHelper.ZPost(url, "/api/storage/uploadazurebytes", json);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<AzureModel>(response.Content);
                        return data;
                    }
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PreviewModel GetPreview(Guid id, long companyId, string email)
        {
            PreviewModel previewModel = new PreviewModel();
            try
            {
                string localization = _reminderService.GetLocalizationShotDate(companyId);
                var reminderBatchList = _reminderService.GetReminderBatchList(id);
                var generictemplate = _reminderService.GetgenerictemplateById(reminderBatchList.TemplateId);

                var generictemplate1 = _reminderService.GetgenerictemplateByIdForPreview(companyId, "Statement Of Account");

                EmailModelNew emailModel = new EmailModelNew();

                string htmlHeader = null;
                string htmlFooter = null;
                AttachmnetModel attachmnet = new AttachmnetModel();
                List<Models.Models.SOAOutstandingAmount> lstInvoiceModel = new List<Models.Models.SOAOutstandingAmount>();
                List<Models.Models.OutstandingTotals> lstoutstanding = new List<Models.Models.OutstandingTotals>();
                TemplateMenuModel templatemenuModel = new TemplateMenuModel();
                templatemenuModel.StatementModel = new StatementModel();
                Models.Models.SOAOutstandingAmount invoiceModel;


                FormatCompiler compilar = new FormatCompiler();
                Generator oppGenerator = null;
                Generator oppGenerator1 = null;
                string oppResult11 = null;
                string oppResult12 = null;
                if (reminderBatchList != null)
                {
                    var lstReminderDetails = reminderBatchList.ReminderBatchListDetails.ToList();
                    var lstDistinctCaseIdsWithReminder = lstReminderDetails.DistinctBy(x => x.EntityId).ToList();
                    var lstReminderInvWithReminder = lstReminderDetails.DistinctBy(x => x.DocumentId).ToList();
                    var beanentity = _reminderService.GetEntity(reminderBatchList.DocumentId);
                    List<Address> address = _reminderService.GetAddress(reminderBatchList.DocumentId);
                    List<long?> serviceCompanyIds = lstReminderDetails.DistinctBy(v => v.ServiceCompanyId).Select(d => d.ServiceCompanyId).ToList();
                    List<CompanyCompact> lstCompanies = _reminderService.GetListOfServiceCompanyForSOA(serviceCompanyIds);
                    List<Bank> lstBank = _reminderService.GetListOfBanks(serviceCompanyIds);
                    Dictionary<long?, string> lstGstNumber = _reminderService.GetListGSTnumber(serviceCompanyIds);
                    List<Address> lstCompanyAddress = _reminderService.GetListAddressForCompany(serviceCompanyIds);
                    foreach (var reminderWithCase in lstDistinctCaseIdsWithReminder.OrderBy(a => a.DocDate).ThenBy(c => c.DocNo))
                    {
                        var singlecaselstInvWithReminder = lstReminderDetails.Where(x => x.EntityId == reminderWithCase.EntityId).DistinctBy(v => v.DocumentId).ToList();
                        foreach (var invWithReminder in singlecaselstInvWithReminder.OrderBy(a => a.DocDate).ThenBy(c => c.DocNo))
                        {
                            CompanyCompact companyCompact = lstCompanies.FirstOrDefault(c => c.Id == invWithReminder.ServiceCompanyId);
                            Bank bank = lstBank.FirstOrDefault(c => c.SubcidaryCompanyId == invWithReminder.ServiceCompanyId);
                            string gstNumber = lstGstNumber.FirstOrDefault(c => c.Key == invWithReminder.ServiceCompanyId).Value;

                            CompanyModel company = GetCompanyNew(companyCompact, bank, address, gstNumber, lstCompanyAddress);
                            //var invoice = GetInvoiceData(localization, reminderWithCase);
                            var totalOutStandingAmount = lstReminderInvWithReminder.Select(x => x.DocBalance);
                            if (generictemplate != null)
                            {
                                compilar = new FormatCompiler();
                                oppGenerator = compilar.Compile(generictemplate1.TempletContent);
                                oppGenerator1 = compilar.Compile(generictemplate.TempletContent);
                                templatemenuModel.Entity = GetEntityInfo(beanentity);
                                templatemenuModel.StatementModel.StatementDate = reminderBatchList.JobExecutedOn != null ? reminderBatchList.JobExecutedOn?.ToString("dd/MM/yyyy") : DateTime.UtcNow.ToString("dd/MM/yyyy");
                                GetEntityAddressBookInfo(beanentity, templatemenuModel);
                                templatemenuModel.ServiceEntity = company;
                                //templatemenuModel.Invoice = invoice;
                                invoiceModel = new Models.Models.SOAOutstandingAmount();
                                invoiceModel.DocNo = invWithReminder.DocNo;
                                invoiceModel.DocType = invWithReminder.DocType;
                                invoiceModel.Currency = invWithReminder.Currency;
                                string doctotalamount = Math.Round(invWithReminder.DocumentTotal, 2).ToString("N2");
                                invoiceModel.DocumentTotal = invWithReminder.DocType == "Credit Note" ? "(" + doctotalamount.Replace("-", string.Empty) + ")" : Math.Round(invWithReminder.DocumentTotal, 2).ToString("N2");
                                invoiceModel.DocDate = invWithReminder.DocDate.ToString(localization != null || localization != string.Empty ? localization : "dd/MM/yyyy");
                                string docbalamount = Math.Round(((double)(invWithReminder.DocBalance)), 2).ToString("N2");
                                invoiceModel.DocBalance = invWithReminder.DocType == "Credit Note" ? "(" + docbalamount.Replace("-", string.Empty) + ")" : Math.Round(((double)(invWithReminder.DocBalance)), 2).ToString("N2");
                                //invWithReminder.DocType == "Credit Note" ? "(" + (invWithReminder.DocBalance) + ")" :  invWithReminder.DocBalance.ToString();
                                invoiceModel.CreditNoteBalance = invWithReminder.CreditNoteBalance;
                                invoiceModel.Remarks = invWithReminder.Remarks;
                                lstInvoiceModel.Add(invoiceModel);
                            }
                        }
                    }
                    templatemenuModel.SoaDetail = lstInvoiceModel;
                    templatemenuModel.StatementDate = reminderBatchList.JobExecutedOn != null ? reminderBatchList.JobExecutedOn?.ToString("dd/MM/yyyy") : DateTime.UtcNow.ToString("dd/MM/yyyy");

                    var currencyLst = templatemenuModel.SoaDetail.GroupBy(s => new { Currency = s.Currency/*, SubTotal = Convert.ToDecimal(s.DocBalance)*/ }).Select(d => new { d.Key, Sum = d.Sum(s => Convert.ToDecimal(s.CreditNoteBalance)) }).ToList();

                    foreach (var currency in currencyLst)
                    {
                        Models.Models.OutstandingTotals outstandingTotals = new Models.Models.OutstandingTotals();
                        outstandingTotals.Currency = currency.Key.Currency;
                        outstandingTotals.SubTotal = currency.Sum.ToString("N2");
                        decimal subtotal = Convert.ToDecimal(currency.Sum);
                        string total = subtotal < 0M ? "(" + subtotal.ToString("N2") + ")" : subtotal.ToString("N2");
                        outstandingTotals.SubTotal = subtotal < 0M ? total.Replace("-", string.Empty) : subtotal.ToString("N2");
                        lstoutstanding.Add(outstandingTotals);
                    }

                    templatemenuModel.OutstandingTotal = lstoutstanding;
                    string subject = generictemplate.Subject;
                    if (oppGenerator != null)
                    {
                        oppResult11 = "\n" + oppGenerator.Render(templatemenuModel);
                        oppResult11 = TemplateEmailFilters(oppResult11);
                        oppResult12 = "\n" + oppGenerator1.Render(templatemenuModel);
                        oppResult12 = TemplateEmailFilters(oppResult12);
                        if (subject != null)
                        {
                            Generator subContent = compilar.Compile(subject);
                            subject = subContent.Render(templatemenuModel);
                        }
                    }
                    emailModel.EmailBody = oppResult11;
                    emailModel.TempletContent = oppResult12;
                    byte[] pdfBytes = HtmlToPdfConverter.HtmlToPDF(oppResult11, htmlHeader, htmlFooter);
                    string pdfFilename = "Statement_of_Account" + ".pdf";
                    //var folderpath = null/*"Entities" + "/" + beanentity.Name.TrimEnd('.') + "/" + "Communication"*/;

                    Ziraff.EmailProvider.Attachment att = new Ziraff.EmailProvider.Attachment()
                    {
                        Name = pdfFilename,//emailModel.DisplayFileName,
                        ContentBytes = pdfBytes
                    };

                    var fileURL = UploadAzureByBytesForTemp(pdfFilename, pdfBytes, companyId, string.Empty, email).Path;



                    ////var fileURL = EmailType != "re-sendmail" ? UploadAzureByBytes(item.AttachmentName, pdfBytes, (long)companyId, path, userName).Path : communication.AzurePath;
                    attachmnet.AttachmentFile = fileURL;
                    string delimStr = "/";
                    char[] delimiter = delimStr.ToCharArray();
                    string[] path1 = fileURL.Split(delimiter, 5);
                    var second = path1[4];
                    attachmnet.FileName = second;
                    attachmnet.FilePath = string.Empty;
                    attachmnet.URL = fileURL;
                    attachmnet.DisplayFileName = second;
                    attachmnet.AttachmentName = pdfFilename;

                    previewModel.LstAttachmnets = new List<AttachmnetModel>();
                    previewModel.From = email;
                    previewModel.toEmails = reminderBatchList.Recipient != null ? (reminderBatchList.Recipient + ";" + generictemplate.ToEmailId) : generictemplate.ToEmailId;
                    previewModel.CC = generictemplate.CCEmailIds;
                    previewModel.BCC = generictemplate.BCCEmailIds;
                    previewModel.Subject = subject/*generictemplate.Subject != null && generictemplate.Subject != string.Empty ? generictemplate.Subject.Replace("{{Entity.Entityname}}", beanentity.Name) : null*/;
                    previewModel.TemplateContent = emailModel.TempletContent;
                    previewModel.EmailBody = emailModel.EmailBody;
                    previewModel.LstAttachmnets.Add(attachmnet);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return previewModel;
        }


        public AzureModel UploadAzureByBytesForTemp(string FileName, byte[] templatebyte, long companyId, string path, string usercreated)
        {
            try
            {
                UploadFileModel uploadFileModel = new UploadFileModel();
                uploadFileModel.FileShare = "tempshare";
                uploadFileModel.FileBytes = templatebyte;
                uploadFileModel.FileName = FileName;
                uploadFileModel.Path = path;
                uploadFileModel.CompanyId = companyId;
                uploadFileModel.CursorName = "Bean";

                uploadFileModel.CreatedBy = usercreated;
                var json = RestHelper.ConvertObjectToJason(uploadFileModel);
                try
                {
                    string url = ConfigurationManager.AppSettings["AzureUrl"];
                    var response = RestHelper.ZPost(url, "api/storage/uploadazurebytes", json);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<AzureModel>(response.Content);
                        return data;
                    }
                }
                catch (Exception ex)
                {
                    var message = ex.Message;

                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private AddressBook GetEntityAddressBookInfo(BeanEntity entity, TemplateMenuModel templatemenuModel)
        {
            AddressBook registredAddbook = new AddressBook();
            if (entity != null)
            {
                List<Address> address = _reminderService.GetAddress(entity.Id);
                if (address.Count() > 0 && entity != null)
                {
                    registredAddbook = address.Where(s => s.AddSectionType == "Registered Address").Select(b => b.AddressBook).LastOrDefault();
                    if (registredAddbook != null)
                    {
                        templatemenuModel.RegisteredBlock = ($"{registredAddbook.BlockHouseNo}");
                        templatemenuModel.RegisteredBuilding = ($"{registredAddbook.BuildingEstate}");
                        templatemenuModel.RegisteredCountry = ($"{registredAddbook.Country}");
                        templatemenuModel.RegisteredPostalCode = ($"{registredAddbook.PostalCode}");
                        templatemenuModel.RegisteredStreet = ($"{registredAddbook.Street}");
                        templatemenuModel.RegisteredUnit = ($"{registredAddbook.UnitNo}");
                    }
                }
            }
            return registredAddbook;
        }

        private EntityModel GetEntityInfo(BeanEntity entity)
        {
            EntityModel Entity = new EntityModel();
            if (entity != null)
            {
                List<Address> address = _reminderService.GetAddress(entity.Id);
                Entity.Entityname = entity.Name;
                var EntitymailingAddress = address.Where(x => x.AddType == "Entity" && x.AddSectionType == "Mailing Address").Select(x => x.AddressBook).FirstOrDefault();
                var EntityregisterAddress = address.Where(x => x.AddType == "Entity" && x.AddSectionType == "Registered Address").Select(x => x.AddressBook).FirstOrDefault();

                Entity.MailingAddress = EntitymailingAddress != null ? ((($"{EntitymailingAddress.BlockHouseNo}") != "" ? ($"{EntitymailingAddress.BlockHouseNo}") : "") + " " + (($"{EntitymailingAddress.Street}") != "" ? ($"{EntitymailingAddress.Street}") : "") + "</br> " + (($"{EntitymailingAddress.UnitNo}") != "" ? ($"{EntitymailingAddress.UnitNo}") : "") + " " + (($"{EntitymailingAddress.BuildingEstate}") != "" ? ($"{EntitymailingAddress.BuildingEstate}") : "") + "</br>" + (($"{EntitymailingAddress.Country}") != "" ? ($"{EntitymailingAddress.Country}") : "") + "</br>" + (($"{EntitymailingAddress.Email}") != "" ? ($"{EntitymailingAddress.Email}") : "") + " " + (($"{EntitymailingAddress.PostalCode}") != "" ? ($"{EntitymailingAddress.PostalCode}") : "")) : null;

                Entity.RegisteredAddress = EntityregisterAddress != null ? (($"{EntityregisterAddress.BlockHouseNo}") != "" ? ($"{EntityregisterAddress.BlockHouseNo}") : "") + " " + (($"{EntityregisterAddress.Street}") != "" ? ($"{EntityregisterAddress.Street}") : "") + "</br> " + (($"{EntityregisterAddress.UnitNo}") != "" ? ($"{EntityregisterAddress.UnitNo}") : "") + " " + (($"{EntityregisterAddress.BuildingEstate}") != "" ? ($"{EntityregisterAddress.BuildingEstate}") : "") + "</br>" + (($"{EntityregisterAddress.Country}") != "" ? ($"{EntityregisterAddress.Country}") : "") + "</br>" + (($"{EntityregisterAddress.Email}") != "" ? ($"{EntityregisterAddress.Email}") : "") + "</br>" + (($"{EntityregisterAddress.PostalCode}") != "" ? ($"{EntityregisterAddress.PostalCode}") : "") : null;
            }
            return Entity;
        }

        //private BillModule.Entities.Invoice GetInvoiceData(LocalizationCompact localization, SOAReminderBatchListDetails reminderWithCase)
        //{
        //    BillModule.Entities.Invoice invoicemodel = new BillModule.Entities.Invoice();
        //    invoicemodel.DocNo = reminderWithCase.DocNo;
        //    invoicemodel.DocDate = reminderWithCase.DocDate;
        //    invoicemodel.DocType = reminderWithCase.DocType;
        //    invoicemodel.DocCurrency = reminderWithCase.Currency;
        //    invoicemodel.BalanceAmount = reminderWithCase.DocBalance;
        //    invoicemodel.GrandTotal = reminderWithCase.DocumentTotal;
        //    //invoicemodel.s = reminderWithCase.DocumentTotal;
        //    invoicemodel.Remarks = reminderWithCase.Remarks;
        //    return invoicemodel;
        //}

        private CompanyModel GetCompany(long? companyId, List<Address> address)
        {
            CompanyCompact company = _reminderService.GetServiceCompanyForSOA(companyId);
            Bank bank = company != null ? company.Bank.Where(x => x.CompanyId == companyId).FirstOrDefault() : null;
            //string idType = company != null ? _reminderService.GetIdType(company.idt) : null;
            string idType = " ";

            var Companyaddress = company != null ? _reminderService.GetAddressForCompany(company.Id) : null;
            var EntitymailingAddress = address != null ? (address.Where(x => x.AddType == "Entity" && x.AddSectionType == "Mailing Address").Select(x => x.AddressBook).FirstOrDefault()) : null;
            var EntityregisterAddress = address != null ? (address.Where(x => x.AddType == "Entity" && x.AddSectionType == "Registered Address").Select(x => x.AddressBook).FirstOrDefault()) : null;

            var ServiceEntitymailingAddress = Companyaddress != null ? (Companyaddress.Where(x => x.AddType == "Company" && x.AddSectionType == "Mailing Address").Select(x => x.AddressBook).FirstOrDefault()) : null;
            var ServiceEntityregisterAddress = Companyaddress != null ? (Companyaddress.Where(x => x.AddType == "Company" && x.AddSectionType == "Registered Address").Select(x => x.AddressBook).FirstOrDefault()) : null;
            string gstnumber = _reminderService.GetGSTnumber(companyId);
            CompanyModel companys = new CompanyModel();

            //companys.CompanyLogo = imagtagWithLog;
            companys.CompanyName = company.Name;
            //companys.EntityName = name;
            companys.RegistrationNo = company.RegistrationNo;
            companys.IdentificationType = idType;
            companys.Currency = company.BaseCurrency;
            companys.GSTNumber = gstnumber;
            companys.MailingAddress = ServiceEntitymailingAddress != null ? ((($"{ServiceEntitymailingAddress.BlockHouseNo}") != "" ? ($"{ServiceEntitymailingAddress.BlockHouseNo}") : "") + " " + (($"{ServiceEntitymailingAddress.Street}") != "" ? ($"{ServiceEntitymailingAddress.Street}") : "") + "</br> " + (($"{ServiceEntitymailingAddress.UnitNo}") != "" ? ($"{ServiceEntitymailingAddress.UnitNo}") : "") + " " + (($"{ServiceEntitymailingAddress.BuildingEstate}") != "" ? ($"{ServiceEntitymailingAddress.BuildingEstate}") : "") + "</br>" + (($"{ServiceEntitymailingAddress.Country}") != "" ? ($"{ServiceEntitymailingAddress.Country}") : "") + "</br>" + (($"{ServiceEntitymailingAddress.Email}") != "" ? ($"{ServiceEntitymailingAddress.Email}") : "") + " " + (($"{ServiceEntitymailingAddress.PostalCode}") != "" ? ($"{ServiceEntitymailingAddress.PostalCode}") : "")) : null;
            companys.RegisteredAddress = ServiceEntityregisterAddress != null ? ((($"{ServiceEntityregisterAddress.BlockHouseNo}") != "" ? ($"{ServiceEntityregisterAddress.BlockHouseNo}") : "") + " " + (($"{ServiceEntityregisterAddress.Street}") != "" ? ($"{ServiceEntityregisterAddress.Street}") : "") + "</br> " + (($"{ServiceEntityregisterAddress.UnitNo}") != "" ? ($"{ServiceEntityregisterAddress.UnitNo}") : "") + " " + (($"{ServiceEntityregisterAddress.BuildingEstate}") != "" ? ($"{ServiceEntityregisterAddress.BuildingEstate}") : "") + "</br>" + (($"{ServiceEntityregisterAddress.Country}") != "" ? ($"{ServiceEntityregisterAddress.Country}") : "") + "</br>" + (($"{ServiceEntityregisterAddress.Email}") != "" ? ($"{ServiceEntityregisterAddress.Email}") : "") + " " + (($"{ServiceEntityregisterAddress.PostalCode}") != "" ? ($"{ServiceEntityregisterAddress.PostalCode}") : "")) : null;
            companys.Entityaddress = EntitymailingAddress != null ? ((($"{EntitymailingAddress.BlockHouseNo}") != "" ? ($"{EntitymailingAddress.BlockHouseNo}") : "") + " " + (($"{EntitymailingAddress.Street}") != "" ? ($"{EntitymailingAddress.Street}") : "") + "</br> " + (($"{EntitymailingAddress.UnitNo}") != "" ? ($"{EntitymailingAddress.UnitNo}") : "") + " " + (($"{EntitymailingAddress.BuildingEstate}") != "" ? ($"{EntitymailingAddress.BuildingEstate}") : "") + "</br>" + (($"{EntitymailingAddress.Country}") != "" ? ($"{EntitymailingAddress.Country}") : "") + "</br>" + (($"{EntitymailingAddress.Email}") != "" ? ($"{EntitymailingAddress.Email}") : "") + " " + (($"{EntitymailingAddress.PostalCode}") != "" ? ($"{EntitymailingAddress.PostalCode}") : "")) : EntityregisterAddress != null ? (($"{EntityregisterAddress.BlockHouseNo}") != "" ? ($"{EntityregisterAddress.BlockHouseNo}") : "") + " " + (($"{EntityregisterAddress.Street}") != "" ? ($"{EntityregisterAddress.Street}") : "") + "</br> " + (($"{EntityregisterAddress.UnitNo}") != "" ? ($"{EntityregisterAddress.UnitNo}") : "") + " " + (($"{EntityregisterAddress.BuildingEstate}") != "" ? ($"{EntityregisterAddress.BuildingEstate}") : "") + "</br>" + (($"{EntityregisterAddress.Country}") != "" ? ($"{EntityregisterAddress.Country}") : "") + "</br>" + (($"{EntityregisterAddress.Email}") != "" ? ($"{EntityregisterAddress.Email}") : "") + "</br>" + (($"{EntityregisterAddress.PostalCode}") != "" ? ($"{EntityregisterAddress.PostalCode}") : "") : null;
            if (bank != null)
            {
                companys.BankName = bank.Name;
                companys.BankCode = bank.ShortCode;
                companys.BranchCode = bank.BranchCode;
                companys.BranchName = bank.BranchName;
                companys.Currency = bank.Currency;
                companys.SWIFTCode = bank.SwiftCode;
                companys.BankAddress = bank.BankAddress;
                companys.AccountNumber = bank.AccountNumber;
                companys.AccountName = bank.AccountName;
            }

            return companys;
        }
        private CompanyModel GetCompanyNew(CompanyCompact company, Bank bank, List<Address> address, string gstNumber, List<Address> companyAddress)
        {
            //string idType = company != null ? _reminderService.GetIdType(company.idt) : null;
            string idType = " ";

            //var Companyaddress = company != null ? _reminderService.GetAddressForCompany(company.Id) : null;
            var EntitymailingAddress = address != null ? (address.Where(x => x.AddType == "Entity" && x.AddSectionType == "Mailing Address").Select(x => x.AddressBook).FirstOrDefault()) : null;
            var EntityregisterAddress = address != null ? (address.Where(x => x.AddType == "Entity" && x.AddSectionType == "Registered Address").Select(x => x.AddressBook).FirstOrDefault()) : null;

            var ServiceEntitymailingAddress = companyAddress != null || companyAddress.Any() ? (companyAddress.Where(x => x.AddType == "Company" && x.AddSectionType == "Mailing Address").Select(x => x.AddressBook).FirstOrDefault()) : null;
            var ServiceEntityregisterAddress = companyAddress != null || companyAddress.Any() ? (companyAddress.Where(x => x.AddType == "Company" && x.AddSectionType == "Registered Address").Select(x => x.AddressBook).FirstOrDefault()) : null;
            CompanyModel companys = new CompanyModel();
            if (company != null)
            {
                //companys.CompanyLogo = imagtagWithLog;
                companys.CompanyName = company.Name;
                //companys.EntityName = name;
                companys.RegistrationNo = company.RegistrationNo;
                companys.Currency = company.BaseCurrency;
                companys.GSTNumber = gstNumber;
            }
            companys.IdentificationType = idType;
            companys.MailingAddress = ServiceEntitymailingAddress != null ? ((($"{ServiceEntitymailingAddress.BlockHouseNo}") != "" ? ($"{ServiceEntitymailingAddress.BlockHouseNo}") : "") + " " + (($"{ServiceEntitymailingAddress.Street}") != "" ? ($"{ServiceEntitymailingAddress.Street}") : "") + "</br> " + (($"{ServiceEntitymailingAddress.UnitNo}") != "" ? ($"{ServiceEntitymailingAddress.UnitNo}") : "") + " " + (($"{ServiceEntitymailingAddress.BuildingEstate}") != "" ? ($"{ServiceEntitymailingAddress.BuildingEstate}") : "") + "</br>" + (($"{ServiceEntitymailingAddress.Country}") != "" ? ($"{ServiceEntitymailingAddress.Country}") : "") + "</br>" + (($"{ServiceEntitymailingAddress.Email}") != "" ? ($"{ServiceEntitymailingAddress.Email}") : "") + " " + (($"{ServiceEntitymailingAddress.PostalCode}") != "" ? ($"{ServiceEntitymailingAddress.PostalCode}") : "")) : null;
            companys.RegisteredAddress = ServiceEntityregisterAddress != null ? ((($"{ServiceEntityregisterAddress.BlockHouseNo}") != "" ? ($"{ServiceEntityregisterAddress.BlockHouseNo}") : "") + " " + (($"{ServiceEntityregisterAddress.Street}") != "" ? ($"{ServiceEntityregisterAddress.Street}") : "") + "</br> " + (($"{ServiceEntityregisterAddress.UnitNo}") != "" ? ($"{ServiceEntityregisterAddress.UnitNo}") : "") + " " + (($"{ServiceEntityregisterAddress.BuildingEstate}") != "" ? ($"{ServiceEntityregisterAddress.BuildingEstate}") : "") + "</br>" + (($"{ServiceEntityregisterAddress.Country}") != "" ? ($"{ServiceEntityregisterAddress.Country}") : "") + "</br>" + (($"{ServiceEntityregisterAddress.Email}") != "" ? ($"{ServiceEntityregisterAddress.Email}") : "") + " " + (($"{ServiceEntityregisterAddress.PostalCode}") != "" ? ($"{ServiceEntityregisterAddress.PostalCode}") : "")) : null;
            companys.Entityaddress = EntitymailingAddress != null ? ((($"{EntitymailingAddress.BlockHouseNo}") != "" ? ($"{EntitymailingAddress.BlockHouseNo}") : "") + " " + (($"{EntitymailingAddress.Street}") != "" ? ($"{EntitymailingAddress.Street}") : "") + "</br> " + (($"{EntitymailingAddress.UnitNo}") != "" ? ($"{EntitymailingAddress.UnitNo}") : "") + " " + (($"{EntitymailingAddress.BuildingEstate}") != "" ? ($"{EntitymailingAddress.BuildingEstate}") : "") + "</br>" + (($"{EntitymailingAddress.Country}") != "" ? ($"{EntitymailingAddress.Country}") : "") + "</br>" + (($"{EntitymailingAddress.Email}") != "" ? ($"{EntitymailingAddress.Email}") : "") + " " + (($"{EntitymailingAddress.PostalCode}") != "" ? ($"{EntitymailingAddress.PostalCode}") : "")) : EntityregisterAddress != null ? (($"{EntityregisterAddress.BlockHouseNo}") != "" ? ($"{EntityregisterAddress.BlockHouseNo}") : "") + " " + (($"{EntityregisterAddress.Street}") != "" ? ($"{EntityregisterAddress.Street}") : "") + "</br> " + (($"{EntityregisterAddress.UnitNo}") != "" ? ($"{EntityregisterAddress.UnitNo}") : "") + " " + (($"{EntityregisterAddress.BuildingEstate}") != "" ? ($"{EntityregisterAddress.BuildingEstate}") : "") + "</br>" + (($"{EntityregisterAddress.Country}") != "" ? ($"{EntityregisterAddress.Country}") : "") + "</br>" + (($"{EntityregisterAddress.Email}") != "" ? ($"{EntityregisterAddress.Email}") : "") + "</br>" + (($"{EntityregisterAddress.PostalCode}") != "" ? ($"{EntityregisterAddress.PostalCode}") : "") : null;
            if (bank != null)
            {
                companys.BankName = bank.Name;
                companys.BankCode = bank.ShortCode;
                companys.BranchCode = bank.BranchCode;
                companys.BranchName = bank.BranchName;
                companys.Currency = bank.Currency;
                companys.SWIFTCode = bank.SwiftCode;
                companys.BankAddress = bank.BankAddress;
                companys.AccountNumber = bank.AccountNumber;
                companys.AccountName = bank.AccountName;
            }

            return companys;
        }

        public static string TemplateEmailFilters(string sentEmailBody)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(sentEmailBody);
                var nodes2 = doc.DocumentNode.SelectNodes("//tr[contains(@class, 'hide-foreach')]");
                if (nodes2 != null)
                {
                    foreach (HtmlNode node in nodes2)
                    {
                        node.Remove();
                    }
                    sentEmailBody = doc.DocumentNode.InnerHtml;
                }
                return sentEmailBody;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public BeanEntity GetClientsData(Guid ClientId)
        //{
        //    BeanEntity Clientsmodel = new BeanEntity();
        //    BeanEntity client = _reminderService.GetClientById(ClientId);
        //    if (client != null)
        //    {
        //        Clientsmodel.Name = client.Name;
        //        var lstAddress = _reminderService.GetAddress(client.Id);
        //        if (lstAddress.Count > 0)
        //        {
        //            foreach (var address in lstAddress)
        //            {
        //                if (address.AddSectionType == ValidationMessageConstants.Mailing_Address)
        //                {
        //                    if (address.AddressBook != null)
        //                    {
        //                        //Clientsmodel. = address.AddressBook.UnitNo;
        //                        //Clientsmodel.MaillingBlock = address.AddressBook.BlockHouseNo;
        //                        //Clientsmodel.MaillingBuilding = address.AddressBook.BuildingEstate;
        //                        //Clientsmodel.MaillingStreet = address.AddressBook.Street;
        //                        //Clientsmodel.MaillingCity = address.AddressBook.City;
        //                        //Clientsmodel.MaillingState = address.AddressBook.State;
        //                        //Clientsmodel.MaillingCountry = address.AddressBook.Country;
        //                        //Clientsmodel.MaillingPostalCode = address.AddressBook.PostalCode;
        //                        //Clientsmodel.MaillingAddLine1 = address.AddressBook.Street;
        //                        //Clientsmodel.MaillingAddLine2 = address.AddressBook.UnitNo;

        //                    }
        //                }
        //                if (address.AddSectionType == ValidationMessageConstants.Registered_Address)
        //                {
        //                    if (address.AddressBook != null)
        //                    {
        //                        Clientsmodel.RegisteredUnit = address.AddressBook.UnitNo;
        //                        Clientsmodel.RegisteredBlock = address.AddressBook.BlockHouseNo;
        //                        Clientsmodel.RegisteredBuilding = address.AddressBook.BuildingEstate;
        //                        Clientsmodel.RegisteredStreet = address.AddressBook.Street;
        //                        Clientsmodel.RegisteredCity = address.AddressBook.City;
        //                        Clientsmodel.RegisteredState = address.AddressBook.State;
        //                        Clientsmodel.RegisteredCountry = address.AddressBook.Country;
        //                        Clientsmodel.RegisteredPostalCode = address.AddressBook.PostalCode;
        //                        Clientsmodel.RegisteredAddLine1 = address.AddressBook.Street;
        //                        Clientsmodel.RegisteredAddLine2 = address.AddressBook.UnitNo;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return Clientsmodel;
        //}

        public bool DismissReminderBatch(ReminderMailModel dismisremindermodel)
        {
            try
            {
                foreach (var id in dismisremindermodel.Ids)
                {
                    var ReminderBatch = _reminderService.GetReminderBatchList(id);
                    if (ReminderBatch != null)
                    {
                        ReminderBatch.IsDismiss = true;
                        ReminderBatch.ModifiedBy = dismisremindermodel.UserFirstName;
                        ReminderBatch.ModifiedDate = DateTime.UtcNow;
                        ReminderBatch.ObjectState = ObjectState.Modified;
                        _reminderService.UpdateSOAReminderBatchList(ReminderBatch);
                        _unitOfWorkAysnc.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
