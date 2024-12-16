using AppsWorld.TemplateModule.Entities.Models.V2;
using AppsWorld.TemplateModule.Models;
using AppsWorld.TemplateModule.RepositoryPattern.V2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Service.V2
{
    public class TemplateService : Service<Invoice>, ITemplateService
    {
        private readonly ITemplateCompactModuleRepositoryAsync<Invoice> _invoiceRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<TaxCode> _taxCodeRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<GenericTemplate> _genericTemplateRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<Company> _companyKRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<CompanyTemplateSettings> _companyTemplateSettingKRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<BeanEntity> _beanEntityRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<Address> _addressRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<Journal> _journalRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<Receipt> _receiptRepository; 
        private readonly ITemplateCompactModuleRepositoryAsync<Template> _templateRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<Localization> _localizationRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<Contact> _contactRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<ContactDetail> _contactDetailRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<CompanyUser> _companyUserRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<GSTSetting> _gSTSettingRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<TermsOfPayment> _termsOfPaymentRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<Bank> _bankRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<IdType> _idTypeRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<DebitNote> _debitNoteRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<Item> _itemRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<CashSale> _cashSaleRepository;
        private readonly ITemplateCompactModuleRepositoryAsync<MediaRepository> _photoRepository;

        public TemplateService(ITemplateCompactModuleRepositoryAsync<Invoice> invoiceRepository, ITemplateCompactModuleRepositoryAsync<TaxCode> taxCodeRepository, ITemplateCompactModuleRepositoryAsync<CompanyTemplateSettings> companyTemplateSettingKRepository,
            ITemplateCompactModuleRepositoryAsync<GenericTemplate> genericTemplateRepository, ITemplateCompactModuleRepositoryAsync<Company> companyKRepository, ITemplateCompactModuleRepositoryAsync<BeanEntity> beanEntityRepository, ITemplateCompactModuleRepositoryAsync<Address> addressRepository, ITemplateCompactModuleRepositoryAsync<Journal> journalRepository, ITemplateCompactModuleRepositoryAsync<Receipt> receiptRepository, ITemplateCompactModuleRepositoryAsync<Template> templateRepository, ITemplateCompactModuleRepositoryAsync<Localization> localizationRepository, ITemplateCompactModuleRepositoryAsync<Contact> contactRepository, ITemplateCompactModuleRepositoryAsync<ContactDetail> contactDetailRepository, ITemplateCompactModuleRepositoryAsync<CompanyUser> companyUserRepository, ITemplateCompactModuleRepositoryAsync<ChartOfAccount> chartOfAccountRepository, ITemplateCompactModuleRepositoryAsync<GSTSetting> gSTSettingRepository, ITemplateCompactModuleRepositoryAsync<TermsOfPayment> termsOfPaymentRepository, ITemplateCompactModuleRepositoryAsync<Bank> bankRepository, ITemplateCompactModuleRepositoryAsync<IdType> idTypeRepository, ITemplateCompactModuleRepositoryAsync<DebitNote> debitNoteRepository, ITemplateCompactModuleRepositoryAsync<Item> itemRepository, ITemplateCompactModuleRepositoryAsync<CashSale> cashSaleRepository, ITemplateCompactModuleRepositoryAsync<MediaRepository> photoRepository) : base(invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
            _taxCodeRepository = taxCodeRepository;
            _companyTemplateSettingKRepository = companyTemplateSettingKRepository;
            _genericTemplateRepository = genericTemplateRepository;
            _companyKRepository = companyKRepository;
            _beanEntityRepository = beanEntityRepository;
            _addressRepository = addressRepository;
            _journalRepository = journalRepository;
            _receiptRepository = receiptRepository;
            _templateRepository = templateRepository;
            _localizationRepository = localizationRepository;
            _contactRepository = contactRepository;
            _contactDetailRepository = contactDetailRepository;
            _companyUserRepository = companyUserRepository;
            _chartOfAccountRepository = chartOfAccountRepository;
            _gSTSettingRepository = gSTSettingRepository;
            _termsOfPaymentRepository = termsOfPaymentRepository;
            _bankRepository = bankRepository;
            _idTypeRepository = idTypeRepository;
            _debitNoteRepository = debitNoteRepository;
            _itemRepository = itemRepository;
            _cashSaleRepository = cashSaleRepository;
            _photoRepository = photoRepository;
        }


        #region  Commented


        //public GenericTemplateModel GetTemplateheaderInvoice(long companyId)
        //{
        //    //return _genericTemplateRepository.Query(a => a.CompanyId == companyId ).Include(a=>a.TemplateType).Select().FirstOrDefault();
        //    return (from gt in _genericTemplateRepository.Queryable()
        //            join tt in _templateTypeRepository.Queryable()
        //            on gt.TemplateTypeId equals tt.Id
        //            where (tt.Name == "Invoice" && gt.CompanyId == companyId)
        //            select new GenericTemplateModel
        //            {
        //                Id = gt.Id,
        //                IsFooterExist = gt.IsFooterExist,
        //                IsHeaderExist = gt.IsHeaderExist,
        //                TempletContent = gt.TempletContent

        //            }).FirstOrDefault();
        //}
        //public GenericTemplateModel GetTemplateheaderCreditNote(long companyId)
        //{
        //    //return _genericTemplateRepository.Query(a => a.CompanyId == companyId ).Include(a=>a.TemplateType).Select().FirstOrDefault();
        //    return (from gt in _genericTemplateRepository.Queryable()
        //            join tt in _templateTypeRepository.Queryable()
        //            on gt.TemplateTypeId equals tt.Id
        //            where (tt.Name == "Credit Note" && gt.CompanyId == companyId)
        //            select new GenericTemplateModel
        //            {
        //                Id = gt.Id,
        //                IsFooterExist = gt.IsFooterExist,
        //                IsHeaderExist = gt.IsHeaderExist,
        //                TempletContent = gt.TempletContent

        //            }).FirstOrDefault();
        //}



        //public Tuple<Entities.Models.V2.Invoice, Entities.Models.V2.Company> GetInvoicesandsubsudaryCompanyById(Guid invoiceId)
        //{
        //    var data = (from iv in _invoiceRepository.Queryable()
        //                join c in _companyKRepository.Queryable()
        //                on iv.ServiceCompanyId equals c.Id
        //                where (iv.Id == invoiceId)
        //                select new { iv, c }).ToList();

        //    var datas = data.AsQueryable().FirstOrDefault();
        //    return new Tuple<Entities.Models.V2.Invoice, Entities.Models.V2.Company>(datas.iv, datas.c);
        //}
        //public string GetIdentificationType(long companyId)
        //{
        //    return _idTypeKRepository.Queryable().Where(a => a.CompanyId == companyId).Select(s => s.Name).FirstOrDefault();
        //}

        //public string GetGSTNumber(long companyId, long serviceCompanyId)
        //{
        //    return _gSTSettingKRepository.Queryable().Where(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId).Select(s => s.Number).FirstOrDefault();
        //}
        //public Invoice GetInvoicesById(Guid invoiceid)
        //{
        //    return _invoiceRepository.Query(a => a.Id == invoiceid).Select().FirstOrDefault();
        //}
        //public string GetCreditTerms(long companyId, long? creditTermId)
        //{
        //    return _termsOfPaymentRepository.Queryable().Where(a => a.CompanyId == companyId && a.Id == creditTermId).Select(s => s.Name).FirstOrDefault();
        //}

        //public string GetEntityName(long companyId, Guid? entityId)
        //{
        //    return _beanEntityRepository.Queryable().Where(a => a.CompanyId == companyId && a.Id == entityId).Select(s => s.Name).FirstOrDefault();
        //}
        #endregion



        public Invoice GetClientsByInvoice(Guid invoiceid)
        {
            return _invoiceRepository.Queryable().Where(a => a.Id == invoiceid).FirstOrDefault();
        }
        public CompanyTemplateSettings GetCompanyTemplateSettings(long companyId)
        {
            return _companyTemplateSettingKRepository.Queryable().Where(a => a.ServiceCompanyId == companyId).FirstOrDefault();
        }
        public List<Address> GetAddress(Guid id)
        {
            return _addressRepository.Query(b => b.AddTypeId == id).Include(b => b.AddressBook).Select().ToList();
        }
        public List<Address> GetAddressForCompany(long id)
        {
            return _addressRepository.Query(b => b.AddType == "company" && b.AddTypeIdInt==id).Include(b => b.AddressBook).Select().ToList();
        }
        public string GetIdType(long? id)
        {
            return _idTypeRepository.Query(v => v.Id == id).Select(s => s.Name).FirstOrDefault();
        }

        public BeanEntity GetEntity(Guid entityId)
        {
            return _beanEntityRepository.Query(v => v.Id == entityId).Include(v => v.Invoices).Select().FirstOrDefault();
        }
        public Invoice GetInvoiceById(Guid invoiceId, string Type)
        {
            return _invoiceRepository.Query(b => b.Id == invoiceId && b.DocType == Type).Include(s => s.InvoiceDetails).Select().FirstOrDefault();
        }
        public Receipt GetReceiptById(Guid receipt)
        {
            return _receiptRepository.Query(b => b.Id == receipt).Select().FirstOrDefault();
        }

        public Company GetServiceCompany(long companyId)
        {
            return _companyKRepository.Query(b => b.Id == companyId).Include(b => b.Bank).Select().FirstOrDefault();
        }
        public Company GetServiceCompanyForSOA(long companyId)
        {
            return _companyKRepository.Query(b => b.ParentId == companyId).Select().FirstOrDefault();
        }
        public Bank GetBank(long companyId)
        {
            return _bankRepository.Query(b => b.SubcidaryCompanyId == companyId).Select().LastOrDefault();
        }
        public Bank GetBank(long companyId,string screenName)
        {
            Bank bank;
            List<string> purposeValues = new List<string>();
            List<Bank> lstBank = _bankRepository.Query(b => b.SubcidaryCompanyId == companyId).Select().ToList();
            bank = lstBank.FirstOrDefault(x => x.Purpose != null && x.Purpose != string.Empty && x.Purpose != "" && x.Purpose == screenName);
            if(bank != null)
                purposeValues = bank.Purpose.Split(',').Select(x => x.Trim()).ToList();
            if (purposeValues.Contains(screenName))
            {
                return bank;
            }
            else
            {
                bank = lstBank.LastOrDefault();
            }
            return bank;
        }
        public Bank GetInvoiceBank(long companyId)
        {
            List<Bank> lstBanks = _bankRepository.Queryable().Where(b => b.SubcidaryCompanyId == companyId).ToList();
            Bank bank = lstBanks.FirstOrDefault(x => x.Purpose != null || x.Purpose != string.Empty);
            List<string> purposeValues = bank.Purpose.Split(',').Select(x => x.Trim()).ToList();
            if (purposeValues.Contains("Invoice"))
            {
                return bank;
            }
            else
            {
                return lstBanks.LastOrDefault();
            }
        }
        //public List<Bank> GetALlBanks(long companyId, string screenName)
        //{
        //    List<Bank> lstbank = _bankRepository.Queryable().Where(b => b.CompanyId == companyId).ToList();
        //    List<Bank> bankList = new List<Bank>();
        //    foreach (var bank in lstbank)
        //    {
        //        Bank bank1 = new Bank();
        //        string[] purpose = bank.Purpose.Split(',').Select(sValue => sValue.Trim()).ToArray();
        //        foreach (var item in purpose)
        //        {
        //            if (item == screenName)
        //            {
        //                bank1.SwiftCode = bank.SwiftCode;
        //                bank1.BankAddress = bank.BankAddress;
        //                bank1.Name = bank.Name;
        //                bank1.AccountName = bank.AccountName;
        //                bank1.AccountNumber = bank.AccountNumber;
        //                bankList.Add(bank1);
        //            }
        //        }
        //    }
        //    if (bankList.Count > 0)
        //        return bankList;
        //    return lstbank;
        //}

        public List<Journal> GetJournal(Guid entityId, long companyId)
        {
            List<Journal> journal = _journalRepository.Queryable().Where(v => v.EntityId == entityId && v.CompanyId == companyId && ((v.DocType == "Invoice" || v.DocType == "Debit Note" || v.DocType == "Credit Note") && (v.DocumentState == "Partial Paid" || v.DocumentState == "Not Paid" || v.DocumentState == "Partial Applied" || v.DocumentState == "Not Applied"))).ToList();
            return journal;
        }
        public List<Invoice> GetAllInvoice(Guid entityId, long companyId)
        {
            List<Invoice> journal = _invoiceRepository.Queryable().Where(v => v.EntityId == entityId && v.CompanyId == companyId && v.DocSubType == "Opening Bal" && v.IsOBInvoice == true && (v.DocumentState != "Fully Paid" && v.DocumentState != "Fully Applied")).ToList();
            return journal;
        }

        public TaxCode GetTaxCode(long? taxId)
        {
            TaxCode taxCode = _taxCodeRepository.Query(v => v.Id == taxId).Select().FirstOrDefault();
            return taxCode;
        }
        public ChartOfAccount GetChartOfAccount(long? coaId)
        {
            ChartOfAccount taxCode = _chartOfAccountRepository.Query(v => v.Id == coaId).Select().FirstOrDefault();
            return taxCode;
        }
        public GenericTemplate GetGenerictemplate(long companyId, string templateType)
        {
            GenericTemplate GenericTemplate = _genericTemplateRepository.Queryable().Where(b => b.CompanyId == companyId && b.TemplateType == templateType).FirstOrDefault();
            return GenericTemplate;
        }

        public Receipt GetReceipt(Guid Id)
        {
            return _receiptRepository.Query(v => v.Id == Id).Include(v => v.ReceiptDetails).Select().FirstOrDefault();
        }

        public Receipt GetReceiptDetails(long? companyId)
        {

            return _receiptRepository.Query(v => v.CompanyId == companyId).Include(v => v.ReceiptDetails).Select().FirstOrDefault();
        }

        public Guid GetEntityNameById(string entityName)
        {
            return _beanEntityRepository.Queryable().Where(s => s.Name == entityName).Select(s => s.Id).FirstOrDefault();
        }
        public Template GetTemplateById(long companyId, string menuType)
        {
            return _templateRepository.Queryable().Where(c => c.CompanyId == companyId && c.TemplateMenu == "Invoice" && c.Status == Framework.RecordStatusEnum.Active && c.CursorName == "Bean Cursor").FirstOrDefault();
        }
        public Localization GetLocalizationByCompanyId(long companyId)
        {
            return _localizationRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
        }

        public List<ContactEmailModel> GetContactByClienId(Guid id) //check
        {
            try
            {
                List<ContactEmailModel> data = new List<ContactEmailModel>();
                string email = "";
                var sa = _contactDetailRepository.Query(a => a.EntityId == id).Include(a => a.Contact).Select().ToList();
                //string str = null;
                foreach (var item in sa)
                {
                    if (item.Communication == null || item.Communication == string.Empty)
                        email = "";
                    if (item.Communication != null && item.Communication != "]" && item.Communication != string.Empty)
                    {
                        JArray a = JArray.Parse(item.Communication);
                        foreach (JObject o in a.Children<JObject>())
                        {
                            string comm = o.ToString();
                            var jsonMetaResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(comm);
                            if (jsonMetaResult.ContainsValue("Email"))
                            {
                                ContactEmailModel model = new ContactEmailModel();
                                email = jsonMetaResult["value"];
                                model.Email = email;
                                model.IsPrimary = item.IsPrimaryContact;
                                model.ContactId = item.ContactId;
                                data.Add(model);
                            }

                        }
                        //str = str + ',' + email;
                    }
                    if (email == string.Empty)
                    {
                        if (item.Contact.Communication != null && item.Contact.Communication != "]")
                        {
                            JArray ja = JArray.Parse(item.Contact.Communication);
                            foreach (JObject jo in ja.Children<JObject>())
                            {
                                string com = jo.ToString();
                                var jsonMetaResult1 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(com);
                                if (jsonMetaResult1.ContainsValue("Email"))
                                {
                                    ContactEmailModel model = new ContactEmailModel();
                                    email = jsonMetaResult1["value"];
                                    model.Email = email;
                                    model.IsPrimary = item.IsPrimaryContact;
                                    model.ContactId = item.ContactId;
                                    //email = "avinash1422@gmail.com";
                                    data.Add(model);
                                }
                            }
                        }
                    }

                }
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Contact> GetContactsById(Guid id)
        {
            return _contactDetailRepository.Query(x => x.EntityId == id).Include(x => x.Contact).Select(x => x.Contact).ToList();
        }
        public List<Contact> GetContactsById1(Guid id)
        {
            return _contactDetailRepository.Query(x => x.EntityId == id && x.IsPrimaryContact == true).Include(x => x.Contact).Select(x => x.Contact).ToList();
        }
        public string GetGenericEmailBody(long companyId, string templateName)
        {
            return _genericTemplateRepository.Queryable().Where(s => s.CompanyId == companyId && s.Name == templateName).Select(s => s.TempletContent).FirstOrDefault();
        }
        public string GetGenericEmailBody1(long companyId, string templateType, string templateName)
        {
            string genericTemplate = null;
            List<GenericTemplate> lstGenericTemplate = _genericTemplateRepository.Queryable().Where(s => s.CompanyId == companyId && s.TemplateType == templateType).ToList();
            if (templateName == "Invoice" || templateName == "Credit Note" || templateName == "Debit Note" || templateName == "Cash Sale" || templateName == "Receipt" || templateName == "Statement Of Account")
                genericTemplate = lstGenericTemplate.Where(s => s.CompanyId == companyId && s.TemplateType == templateType && s.IsSystem == true && s.Status == Framework.RecordStatusEnum.Active).Select(s => s.TempletContent).FirstOrDefault();
            if (genericTemplate == null && (templateName != "Invoice" || templateName != "Credit Note" || templateName != "Debit Note" || templateName != "Cash Sale" || templateName != "Receipt" || templateName != "Statement Of Account"))
                genericTemplate = lstGenericTemplate.Where(s => s.CompanyId == companyId && s.TemplateType == templateType && s.Name == templateName).Select(s => s.TempletContent).FirstOrDefault();
            if (genericTemplate == null)
                genericTemplate = lstGenericTemplate.Where(s => s.CompanyId == companyId && s.TemplateType == templateType && s.Status == Framework.RecordStatusEnum.Active).Select(s => s.TempletContent).FirstOrDefault();
            return genericTemplate;

        }
        public string GetGenericEmailBody1(long companyId, string templateType, string templateName,long serviceCompanyId)
        {
            string genericTemplate = null;
            List<GenericTemplate> lstGenericTemplate = _genericTemplateRepository.Queryable().Where(s => s.CompanyId == companyId && s.TemplateType == templateType && s.ServiceCompanyIds.Contains(serviceCompanyId.ToString())).ToList();
            if (templateName == "Invoice" || templateName == "Credit Note" || templateName == "Debit Note" || templateName == "Cash Sale" || templateName == "Receipt" || templateName == "Statement Of Account")
                genericTemplate = lstGenericTemplate.Where(s => s.CompanyId == companyId && s.TemplateType == templateType && s.IsSystem == true && s.Status == Framework.RecordStatusEnum.Active && s.ServiceCompanyIds.Contains(serviceCompanyId.ToString())).Select(s => s.TempletContent).FirstOrDefault();
            if (genericTemplate == null && (templateName != "Invoice" || templateName != "Credit Note" || templateName != "Debit Note" || templateName != "Cash Sale" || templateName != "Receipt" || templateName != "Statement Of Account"))
                genericTemplate = lstGenericTemplate.Where(s => s.CompanyId == companyId && s.TemplateType == templateType && s.Name == templateName && s.ServiceCompanyIds.Contains(serviceCompanyId.ToString())).Select(s => s.TempletContent).FirstOrDefault();
            if (genericTemplate == null)
                genericTemplate = lstGenericTemplate.Where(s => s.CompanyId == companyId && s.TemplateType == templateType && s.Status == Framework.RecordStatusEnum.Active && s.ServiceCompanyIds.Contains(serviceCompanyId.ToString())).Select(s => s.TempletContent).FirstOrDefault();
            return genericTemplate;

        }
        public string GetFirstName(long companyId, string UserName)
        {
            return _companyUserRepository.Queryable().Where(s => s.CompanyId == companyId && s.Username == UserName).Select(s => s.FirstName).FirstOrDefault();
        }
        public string GetGSTnumber(long companyId)
        {
            return _gSTSettingRepository.Queryable().Where(s => s.ServiceCompanyId == companyId).Select(s => s.Number).FirstOrDefault();
        }
        public string GetTermsOfPaymentById(long? id, long companyId)
        {
            return _termsOfPaymentRepository.Query(a => a.Id == id && a.CompanyId == companyId).Select(a => a.Name).FirstOrDefault();
        }

        public GSTSetting GetIsGst(long companyId)
        {
            return _gSTSettingRepository.Queryable().Where(s => s.CompanyId == companyId).FirstOrDefault();
        }
        public DebitNote GetDebitNoteById(Guid invoiceId, string Type)
        {
            return _debitNoteRepository.Query(b => b.Id == invoiceId && b.DocSubType == Type).Include(s => s.DebitNoteDetails).Select().FirstOrDefault();
        }
        public CashSale GetCashSaleById(Guid invoiceId, string Type)
        {
            return _cashSaleRepository.Query(b => b.Id == invoiceId && b.DocType == Type).Include(s => s.CashSaleDetails).Select().FirstOrDefault();
        }
        public List<Item> GetAllItems(long companyId)
        {
            return _itemRepository.Query(x => x.CompanyId == companyId).Select().ToList();
        }
        public bool? GetServiceCompanyByGst(long companyId)
        {
            return _companyKRepository.Query(b => b.Id == companyId).Select(s => s.IsGstSetting).FirstOrDefault();
        }

        public MediaRepository GetPhoto( Guid? logoId)
        {
            return _photoRepository.Query(p=>p.Id== logoId).Select().FirstOrDefault();
        }

        public List<Company> GetServiceEntityNameById(long id)
        {
          return  _companyKRepository.Query(x=>x.Status==Framework.RecordStatusEnum.Active && x.ParentId==id).Select().ToList();
        }

        public string GetGenerictemplateForCcMail(long value, string templateName)
        {
            return _genericTemplateRepository.Queryable().Where(b => b.CompanyId == value && b.Name == templateName && b.CursorName!= "WorkFlow Cursor").Select(x=>x.CCEmailIds).FirstOrDefault();
        }
    }
}
