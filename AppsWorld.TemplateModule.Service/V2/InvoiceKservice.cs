//using AppsWorld.TemplateModule.Entities.Models.V2;
//using AppsWorld.TemplateModule.RepositoryPattern;
//using AppsWorld.TemplateModule.RepositoryPattern.V2;
//using Service.Pattern;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AppsWorld.TemplateModule.Service.V2
//{
//    public class InvoiceKservice : Service<Invoice>, ITemplateService
//    {
//        private readonly ITemplateCompactModuleRepositoryAsync<Invoice> _invoiceRepository;
//        private readonly ITemplateCompactModuleRepositoryAsync<TaxCode> _taxCodeRepository;
//        private readonly ITemplateCompactModuleRepositoryAsync<GenericTemplate> _genericTemplateRepository;
//        private readonly ITemplateCompactModuleRepositoryAsync<Company> _companyKRepository;
//        private readonly ITemplateCompactModuleRepositoryAsync<CompanyTemplateSettings> _companyTemplateSettingKRepository;
//        private readonly ITemplateCompactModuleRepositoryAsync<BeanEntity> _beanEntityRepository;
//        private readonly ITemplateCompactModuleRepositoryAsync<Address> _addressRepository;
//        private readonly ITemplateCompactModuleRepositoryAsync<Journal> _journalRepository;
//        private readonly ITemplateCompactModuleRepositoryAsync<Receipt> _receiptRepository;

//        public InvoiceKservice(ITemplateCompactModuleRepositoryAsync<Invoice> invoiceRepository, ITemplateCompactModuleRepositoryAsync<TaxCode> taxCodeRepository, ITemplateCompactModuleRepositoryAsync<CompanyTemplateSettings> companyTemplateSettingKRepository,
//            ITemplateCompactModuleRepositoryAsync<GenericTemplate> genericTemplateRepository, ITemplateCompactModuleRepositoryAsync<Company> companyKRepository, ITemplateCompactModuleRepositoryAsync<BeanEntity> beanEntityRepository, ITemplateCompactModuleRepositoryAsync<Address> addressRepository, ITemplateCompactModuleRepositoryAsync<Journal> journalRepository, ITemplateCompactModuleRepositoryAsync<Receipt> receiptRepository) : base(invoiceRepository)
//        {
//            _invoiceRepository = invoiceRepository;
//            _taxCodeRepository = taxCodeRepository;
//            _companyTemplateSettingKRepository = companyTemplateSettingKRepository;
//            _genericTemplateRepository = genericTemplateRepository;           
//            _companyKRepository = companyKRepository;      
//            _beanEntityRepository = beanEntityRepository;
//            _addressRepository = addressRepository;
//            _journalRepository = journalRepository;
//            _receiptRepository = receiptRepository;
//        }


//        #region  Commented


//        //public GenericTemplateModel GetTemplateheaderInvoice(long companyId)
//        //{
//        //    //return _genericTemplateRepository.Query(a => a.CompanyId == companyId ).Include(a=>a.TemplateType).Select().FirstOrDefault();
//        //    return (from gt in _genericTemplateRepository.Queryable()
//        //            join tt in _templateTypeRepository.Queryable()
//        //            on gt.TemplateTypeId equals tt.Id
//        //            where (tt.Name == "Invoice" && gt.CompanyId == companyId)
//        //            select new GenericTemplateModel
//        //            {
//        //                Id = gt.Id,
//        //                IsFooterExist = gt.IsFooterExist,
//        //                IsHeaderExist = gt.IsHeaderExist,
//        //                TempletContent = gt.TempletContent

//        //            }).FirstOrDefault();
//        //}
//        //public GenericTemplateModel GetTemplateheaderCreditNote(long companyId)
//        //{
//        //    //return _genericTemplateRepository.Query(a => a.CompanyId == companyId ).Include(a=>a.TemplateType).Select().FirstOrDefault();
//        //    return (from gt in _genericTemplateRepository.Queryable()
//        //            join tt in _templateTypeRepository.Queryable()
//        //            on gt.TemplateTypeId equals tt.Id
//        //            where (tt.Name == "Credit Note" && gt.CompanyId == companyId)
//        //            select new GenericTemplateModel
//        //            {
//        //                Id = gt.Id,
//        //                IsFooterExist = gt.IsFooterExist,
//        //                IsHeaderExist = gt.IsHeaderExist,
//        //                TempletContent = gt.TempletContent

//        //            }).FirstOrDefault();
//        //}

       

//        //public Tuple<Entities.Models.V2.Invoice, Entities.Models.V2.Company> GetInvoicesandsubsudaryCompanyById(Guid invoiceId)
//        //{
//        //    var data = (from iv in _invoiceRepository.Queryable()
//        //                join c in _companyKRepository.Queryable()
//        //                on iv.ServiceCompanyId equals c.Id
//        //                where (iv.Id == invoiceId)
//        //                select new { iv, c }).ToList();

//        //    var datas = data.AsQueryable().FirstOrDefault();
//        //    return new Tuple<Entities.Models.V2.Invoice, Entities.Models.V2.Company>(datas.iv, datas.c);
//        //}
//        //public string GetIdentificationType(long companyId)
//        //{
//        //    return _idTypeKRepository.Queryable().Where(a => a.CompanyId == companyId).Select(s => s.Name).FirstOrDefault();
//        //}

//        //public string GetGSTNumber(long companyId, long serviceCompanyId)
//        //{
//        //    return _gSTSettingKRepository.Queryable().Where(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId).Select(s => s.Number).FirstOrDefault();
//        //}
//        //public Invoice GetInvoicesById(Guid invoiceid)
//        //{
//        //    return _invoiceRepository.Query(a => a.Id == invoiceid).Select().FirstOrDefault();
//        //}
//        //public string GetCreditTerms(long companyId, long? creditTermId)
//        //{
//        //    return _termsOfPaymentRepository.Queryable().Where(a => a.CompanyId == companyId && a.Id == creditTermId).Select(s => s.Name).FirstOrDefault();
//        //}

//        //public string GetEntityName(long companyId, Guid? entityId)
//        //{
//        //    return _beanEntityRepository.Queryable().Where(a => a.CompanyId == companyId && a.Id == entityId).Select(s => s.Name).FirstOrDefault();
//        //}
//        #endregion



//        public Invoice GetClientsByInvoice(Guid invoiceid)
//        {
//            var data = _invoiceRepository.Query(a => a.Id == invoiceid).Select().FirstOrDefault();
//            return data;

//        }
//        public CompanyTemplateSettings GetCompanyTemplateSettings(long companyId)
//        {
//            return _companyTemplateSettingKRepository.Queryable().Where(a => a.ServiceCompanyId == companyId).FirstOrDefault();
//        }
//        public List<Address> GetAddress(Guid id)
//        {
//            List<Address> addresses = _addressRepository.Query(b => b.AddTypeId == id).Include(b => b.AddressBook).Select().ToList();
//            return addresses;
//        }

//        public BeanEntity GetEntity(Guid entityId)
//        {
//            BeanEntity beanEntity = _beanEntityRepository.Query(v => v.Id == entityId).Include(v => v.Invoices).Include(v => v.Invoices.Select(b => b.InvoiceDetails)).Select().FirstOrDefault();
//            return beanEntity;
//        }



//        public Company GetServiceCompany(long companyId)
//        {
//            Company company = _companyKRepository.Query(b => b.Id == companyId).Include(b => b.Bank).Select().FirstOrDefault();
//            return company;
//        }

//        public List<Journal> GetJournal(Guid entityId)
//        {
//            List<Journal> journal = _journalRepository.Query(v => v.EntityId == entityId).Select().ToList();
//            return journal;


//        }

//        public TaxCode GetTaxCode(long? taxId)
//        {
//            TaxCode taxCode = _taxCodeRepository.Query(v => v.Id == taxId).Select().FirstOrDefault();
//            return taxCode;
//        }
//        public GenericTemplate GetGenerictemplate(long companyId, string templateType)
//        {
//            GenericTemplate GenericTemplate = _genericTemplateRepository.Query(b => b.CompanyId == companyId && b.TemplateType == templateType).Select().FirstOrDefault();
//            return GenericTemplate;
//        }

//        public Receipt GetReceipt(Guid Id)
//        {
//            return _receiptRepository.Query(v => v.Id == Id).Include(v => v.ReceiptDetails).Select().FirstOrDefault();
//        }

//        public Receipt GetReceiptDetails(long? companyId)
//        {

//         return   _receiptRepository.Query(v => v.CompanyId == companyId).Include(v => v.ReceiptDetails).Select().FirstOrDefault();
//        }

//        public Guid GetEntityNameById(string entityName)
//        {
//            return _beanEntityRepository.Queryable().Where(s => s.Name == entityName).Select(s=>s.Id).FirstOrDefault();
//        }
//    }
//}
