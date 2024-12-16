using AppsWorld.BillModule.Entities;
using AppsWorld.Framework;
using AppsWorld.ReminderModule.Entities.Entities;
using AppsWorld.ReminderModule.Models.Models;
using AppsWorld.ReminderModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Service
{
    public class ReminderSaveService : Service<SOAReminderBatchList>, IReminderSaveService
    {
        private readonly IReminderModuleRepositoryAsync<SOAReminderBatchList> _soaReminderBatchListRepository;
        private readonly IReminderModuleRepositoryAsync<SOAReminderBatchListDetails> _soaReminderBatchListDetailsRepository;
        private readonly IReminderModuleRepositoryAsync<BeanEntity> _beanEntityRepository;
        private readonly IReminderModuleRepositoryAsync<BillModule.Entities.ControlCodeCategory> _controlCodeCategoryRepository;
        private readonly IReminderModuleRepositoryAsync<LocalizationCompact> _localizationCompactRepository;
        private readonly IReminderModuleRepositoryAsync<GenericTemplateCompact> _genericTemplateCompactRepository;
        private readonly IReminderModuleRepositoryAsync<Address> _addressRepository;
        private readonly IReminderModuleRepositoryAsync<CompanyCompact> _companyKRepository;
        private readonly IReminderModuleRepositoryAsync<GSTSetting> _GSTSettingRepository;
        private readonly IReminderModuleRepositoryAsync<IdType> _idTypeRepository;
        private readonly IReminderModuleRepositoryAsync<CommunicationCompact> _communicationCompactRepository;
        private readonly IReminderModuleRepositoryAsync<Bank> _bankRepository;
        public ReminderSaveService(IReminderModuleRepositoryAsync<SOAReminderBatchList> soaReminderBatchListRepository,
            IReminderModuleRepositoryAsync<SOAReminderBatchListDetails> soaReminderBatchListDetailsRepository,
            IReminderModuleRepositoryAsync<BeanEntity> beanEntityRepository,
            IReminderModuleRepositoryAsync<BillModule.Entities.ControlCodeCategory> controlCodeCategoryRepository,
            IReminderModuleRepositoryAsync<LocalizationCompact> localizationCompactRepository, IReminderModuleRepositoryAsync<Address> addressRepository,
            IReminderModuleRepositoryAsync<CompanyCompact> companyKRepository, IReminderModuleRepositoryAsync<GSTSetting> GSTSettingRepository,
            IReminderModuleRepositoryAsync<IdType> idTypeRepository, IReminderModuleRepositoryAsync<GenericTemplateCompact> genericTemplateCompactRepository, IReminderModuleRepositoryAsync<CommunicationCompact> communicationCompactRepository, IReminderModuleRepositoryAsync<Bank> bankRepository
            ) : base(soaReminderBatchListRepository)
        {
            _soaReminderBatchListRepository = soaReminderBatchListRepository;
            _soaReminderBatchListDetailsRepository = soaReminderBatchListDetailsRepository;
            _beanEntityRepository = beanEntityRepository;
            _controlCodeCategoryRepository = controlCodeCategoryRepository;
            _localizationCompactRepository = localizationCompactRepository;
            _addressRepository = addressRepository;
            _companyKRepository = companyKRepository;
            _GSTSettingRepository = GSTSettingRepository;
            _idTypeRepository = idTypeRepository;
            _genericTemplateCompactRepository = genericTemplateCompactRepository;
            _communicationCompactRepository = communicationCompactRepository;
            _bankRepository = bankRepository;
        }
        public async Task<IQueryable<ReminderVMK>> GetReminderskNew(long companyId, DateTime? fromDate, DateTime? toDate, string type, string name)
        {

            IQueryable<ReminderVMK> lstReminders = null;
            if (name == "Dismiss")
            {
                lstReminders = (from r in _soaReminderBatchListRepository.Queryable().Where(s => s.CompanyId == companyId)
                                join rd in _soaReminderBatchListDetailsRepository.Queryable() on r.Id equals rd.MasterId
                                into rds
                                from rd in rds.DefaultIfEmpty()
                                join client in await Task.Run(()=> _beanEntityRepository.Queryable()) on r.DocumentId equals client.Id
                                where r.CompanyId == companyId && r.IsDismiss == true || r.JobStatus == "Sent" && r.JobExecutedOn >= fromDate && r.JobExecutedOn <= toDate
                                select new ReminderVMK()
                                {
                                    Id = r.Id,
                                    EntityId = client.Id,
                                    EntityName = client.Name,
                                    CompanyId = client.CompanyId,
                                    BalanceAmount =/* rd.DocBalance*/(rds.Where(x => x.MasterId == r.Id).Sum(c => c.CreditNoteBalance))/*+ (rds.Where(x => x.MasterId == r.Id).Select(c => c.CreditNoteBalance).Sum())*/,
                                    Recipient = r.Recipient,
                                    ReminderType = r.ReminderType,
                                    ReminderName = r.Name,
                                    CreatedDate = r.ModifiedDate,
                                    RemainderDate = r.JobExecutedOn,
                                    DismissOrSentDate = r.ModifiedDate,
                                    UserCreated = r.ModifiedBy,
                                    Status = (r.IsDismiss == null || r.IsDismiss == false) ? r.JobStatus : "Dismiss"
                                }).Distinct().AsQueryable().OrderByDescending(x => (x.CreatedDate));
            }
            else if (name == "Send")
            {
                lstReminders = (from r in _soaReminderBatchListRepository.Queryable().Where(s => s.CompanyId == companyId)
                                join rd in _soaReminderBatchListDetailsRepository.Queryable() on r.Id equals rd.MasterId
                                into rds
                                from rd in rds.DefaultIfEmpty()
                                join client in await Task.Run(()=> _beanEntityRepository.Queryable()) on r.DocumentId equals client.Id
                                where r.CompanyId == companyId && r.IsDismiss != true && r.JobStatus != "Sent" && r.JobExecutedOn >= fromDate && r.JobExecutedOn <= toDate
                                select new ReminderVMK()
                                {
                                    Id = r.Id,
                                    EntityId = client.Id,
                                    EntityName = client.Name,
                                    CompanyId = client.CompanyId,
                                    BalanceAmount = (rds.Where(x => x.MasterId == r.Id).Sum(c => c.CreditNoteBalance))/*+ (rds.Where(x => x.MasterId == r.Id).Select(c => c.CreditNoteBalance).Sum())*/,
                                    Recipient = r.Recipient,
                                    ReminderType = r.ReminderType,
                                    ReminderName = r.Name,
                                    RemainderDate = r.JobExecutedOn,
                                    CreatedDate = r.CreatedDate,
                                    DismissOrSentDate = r.ModifiedDate,
                                    //Status=r.Status
                                }).Distinct().AsQueryable();
            }
   
            return lstReminders.Where(x => x.CompanyId == companyId && x.BalanceAmount > 0);
        }


        public CommonModule.Infra.LookUpCategory<string> GetByCategoryCodeCategoryByCursorName(long companyId, string categoryCode)
        {
            var controlcodeCategory = _controlCodeCategoryRepository.Query(a => a.CompanyId == companyId && a.ControlCodeCategoryCode == categoryCode && a.ModuleNamesUsing.Contains("Bean Cursor") && a.Status == RecordStatusEnum.Active).Include(a => a.ControlCodes).Select().FirstOrDefault();
            var lookUpCategory = new CommonModule.Infra.LookUpCategory<string>();
            if (controlcodeCategory != null)
            {
                lookUpCategory.CategoryName = controlcodeCategory.ControlCodeCategoryDescription;
                lookUpCategory.DefaultValue = controlcodeCategory.DefaultValue;
                lookUpCategory.Code = controlcodeCategory.ControlCodeCategoryCode;
                lookUpCategory.Id = controlcodeCategory.Id;
                lookUpCategory.Lookups = controlcodeCategory.ControlCodes.Where(x => x.ControlCategoryId == controlcodeCategory.Id).Select(x => new CommonModule.Infra.LookUp<string>()
                {
                    Code = x.CodeKey,
                    Name = x.CodeValue,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    //DefaultValue = x.ModuleNamesUsing
                }).OrderBy(x => x.RecOrder).ToList();
            }
            return lookUpCategory;
        }
        public void UpdateSOAReminderBatchList(SOAReminderBatchList reminderBatchList)
        {
            _soaReminderBatchListRepository.Update(reminderBatchList);
        }
        public SOAReminderBatchList GetReminderBatchList(Guid id)
        {
            return _soaReminderBatchListRepository.Query(x => x.Id == id).Include(c => c.ReminderBatchListDetails).Select().FirstOrDefault();
        }
        public LocalizationCompact GetLocalizationByCompanyId(long companyId)
        {
            return _localizationCompactRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public string GetLocalizationShotDate(long companyId)
        {
            return _localizationCompactRepository.Query(d => d.CompanyId == companyId).Select(d => d.ShortDateFormat).FirstOrDefault();
        }
        public GenericTemplateCompact GetgenerictemplateById(Guid templateId)
        {
            return _genericTemplateCompactRepository.Query(x => x.Id == templateId).Select().FirstOrDefault();
        }
        public BeanEntity GetClientById(Guid clientId)
        {
            return _beanEntityRepository.Query(x => x.Id == clientId).Select().FirstOrDefault();
        }
        //public BeanEntity GetClientById(Guid entityId)
        //{
        //    BeanEntity beanEntity = _beanEntityRepository.Query(v => v.Id == entityId).Include(v => v.Invoices).Select().FirstOrDefault();
        //    return beanEntity;
        //}
        public List<Address> GetAddress(Guid id)
        {
            List<Address> addresses = _addressRepository.Query(b => b.AddTypeId == id).Include(b => b.AddressBook).Select().ToList();
            return addresses;
        }
        public CompanyCompact GetServiceCompanyForSOA(long? documentId)
        {
            CompanyCompact company = _companyKRepository.Query(b => b.Id == documentId).Include(b => b.Bank).Select().FirstOrDefault();
            return company;
        }
        public List<CompanyCompact> GetListOfServiceCompanyForSOA(List<long?> serCompIds) => _companyKRepository.Queryable().Where(b => serCompIds.Contains(b.Id)).ToList();
        public List<Bank> GetListOfBanks(List<long?> serviceCompIds) => _bankRepository.Queryable().Where(d => serviceCompIds.Contains(d.SubcidaryCompanyId)).ToList();

        public List<Address> GetAddressForCompany(long id)
        {
            List<Address> addresses = _addressRepository.Query(b => b.AddType == "company" && b.AddTypeIdInt == id).Include(b => b.AddressBook).Select().ToList();
            return addresses;
        }
        public List<Address> GetListAddressForCompany(List<long?> serviceCompanyIds)
        {
            return _addressRepository.Query(b => b.AddType == "company" && serviceCompanyIds.Contains(b.AddTypeIdInt)).Include(b => b.AddressBook).Select().ToList();
        }
        public string GetGSTnumber(long? companyId)
        {
            return _GSTSettingRepository.Queryable().Where(s => s.ServiceCompanyId == companyId).Select(s => s.Number).FirstOrDefault();
        }
        public Dictionary<long?, string> GetListGSTnumber(List<long?> serviceCompIds)
        {
            return _GSTSettingRepository.Queryable().Where(s => serviceCompIds.Contains(s.ServiceCompanyId)).ToDictionary(ServiceCompanyId => ServiceCompanyId.ServiceCompanyId, Number => Number.Number);
        }
        public string GetIdType(long? id)
        {
            string idType = _idTypeRepository.Query(v => v.Id == id).Select(s => s.Name).FirstOrDefault();
            return idType;
        }
        public BeanEntity GetEntity(Guid entityId)
        {
            BeanEntity beanEntity = _beanEntityRepository.Query(v => v.Id == entityId).Select().FirstOrDefault();
            return beanEntity;
        }
        public void InsertCommunication(CommunicationCompact communicationNew)
        {
            _communicationCompactRepository.Insert(communicationNew);
        }

        public GenericTemplateCompact GetgenerictemplateByIdForPreview(long companyId, string templateName)
        {
            return (_genericTemplateCompactRepository.Query(x => x.CompanyId == companyId && x.Name == templateName && x.Code == templateName).Select().FirstOrDefault());
        }
    }
}
