using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using FrameWork;
using AppsWorld.JournalVoucherModule.Models;
using AppsWorld.JournalVoucherModule.Infra;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class JournalService : Service<Journal>, IJournalService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Journal> _journalRepository;
        private readonly IJournalVoucherModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        private readonly IJournalVoucherModuleRepositoryAsync<CompanyUser> _compUserRepo;
        private readonly IJournalVoucherModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUserDetailRepo;
        public JournalService(IJournalVoucherModuleRepositoryAsync<Journal> journalRepository, IJournalVoucherModuleRepositoryAsync<JournalDetail> journalDetailRepository, IJournalVoucherModuleRepositoryAsync<CompanyUser> compUserRepo, IJournalVoucherModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepo)
            : base(journalRepository)
        {
            this._journalRepository = journalRepository;
            this._journalDetailRepository = journalDetailRepository;
            _compUserRepo = compUserRepo;
            _compUserDetailRepo = compUserDetailRepo;
        }

        public Journal CreateJournalDocNo(long companyId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.ReverseParentRefId == null && x.DocType == DocTypeConstants.JournalVocher && x.DocumentState != "Void" && x.Status == RecordStatusEnum.Active && (x.DocSubType == DocSubTypeConstants.Auto_Reversal || x.DocSubType == DocSubTypeConstants.General || x.DocSubType == DocSubTypeConstants.Recurring)).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }
        public List<Journal> GetaAllJournalParkedById(long companyId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocumentState == JournalState.Parked && x.Status == RecordStatusEnum.Active).Select().ToList();
        }
        //public List<Journal> GetAllJVPostedByCompanyId(long companyId)
        //{
        //	return _journalRepository.Query(x => x.CompanyId == companyId && (x.DocumentState == JournalState.Posted || x.DocumentState == JournalState.Reversed)).Select().ToList();
        //}
        public List<Journal> GetAllJVPostedByCompanyId(long companyId)//&& x.IsShow == true (Pradhan removed)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.Status == RecordStatusEnum.Active && (x.DocumentState != JournalState.Parked && x.DocumentState != JournalState.Recurring && x.DocumentState != JournalState.Void && x.DocumentState != "Cancelled")).Select().ToList();
        }
        public List<Journal> GetAllAutoReversalByid(long companyId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocSubType == DocSubTypeConstants.Auto_Reversal && x.Status == RecordStatusEnum.Active).Select().ToList();
        }
        public List<Journal> GetAllJVVoidByCompanyId(long companyId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && (x.DocumentState == JournalState.Void || x.DocumentState == "Cancelled") && x.Status == RecordStatusEnum.Active).Select().ToList();
        }
        public List<Journal> GetAllJournals()
        {
            return _journalRepository.Queryable().ToList();
        }

        public List<Journal> GetAllRecurringsById(long companyId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocSubType == DocSubTypeConstants.Recurring && x.DocumentState == DocSubTypeConstants.Recurring && x.Status == RecordStatusEnum.Active).Select().ToList();
        }

        public Journal GetByAllJournal(Guid id)
        {
            return _journalRepository.Query(x => x.Id == id && x.Status == RecordStatusEnum.Active).Include(c => c.JournalDetails).Select().FirstOrDefault();
        }
        public Journal GetAllReversal(Guid id)
        {
            return _journalRepository.Query(x => x.ReverseParentId == id && x.Status == RecordStatusEnum.Active).Include(c => c.JournalDetails).Select().FirstOrDefault();
        }
        public Journal GetByDocTypeId(Guid id, string DocType, string DocNo, long companyId, string documentState)
        {
            return _journalRepository.Query(c => c.Id != id && c.DocNo == DocNo && c.DocType == DocType && c.CompanyId == companyId && c.DocumentState != "Void" && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public async Task<Journal> GetCompanyId(long companyId)
        {
            return await Task.Run(()=> _journalRepository.Query(c => c.CompanyId == companyId && c.DocType == DocTypeConstants.JournalVocher && c.DocSubType == DocSubTypeConstants.General && c.DocumentState == "Posted" && c.Status == RecordStatusEnum.Active).Select().OrderByDescending(d => d.CreatedDate).FirstOrDefault());
        }
        public async Task<Journal> GetJournalDateBySubType(long companyId, string docSubType) => await Task.Run(()=> _journalRepository.Query(c => c.CompanyId == companyId && c.DocSubType == DocSubTypeConstants.Recurring && c.DocType == DocTypeConstants.JournalVocher && c.Status == RecordStatusEnum.Active && (c.DocumentState == DocSubTypeConstants.Recurring || c.InternalState != null)).Select().OrderByDescending(s => s.CreatedDate).FirstOrDefault());

        public Journal GetDocNo(string docNo, long companyId)
        {
            return _journalRepository.Query(x => x.DocNo == docNo && x.CompanyId == companyId && x.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public List<Journal> GetJournalByCompanyId(long companyId, string documentState, string docSubType)
        {
            return _journalRepository.Query(c => c.CompanyId == companyId && c.DocType == DocTypeConstants.JournalVocher && c.DocSubType == DocSubTypeConstants.Recurring && (c.DocumentState == documentState || c.DocumentState == JournalState.Void) && c.Status == RecordStatusEnum.Active).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }
        public Journal GetJournalById(Guid id, long companyId)
        {
            return _journalRepository.Query(x => x.Id == id && x.CompanyId == companyId && x.Status == RecordStatusEnum.Active).Include(a => a.JournalDetails).Select().FirstOrDefault();
        }
        public async Task<bool?> GetIsLockedByReverseParentId(Guid id, long companyId)
        {
            return await Task.Run(()=> _journalRepository.Query(x => x.ReverseParentId == id && x.CompanyId == companyId).Select(x => x.IsLocked).FirstOrDefault());
        }
        public Journal GetJournalByReverseParentId(Guid id, long companyId)
        {
            return _journalRepository.Query(x => x.ReverseParentId == id && x.CompanyId == companyId && x.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public List<Journal> GetJVByCompanyId(Guid id, long companyId)
        {
            return _journalRepository.Query(x => x.Id == id && x.CompanyId == companyId && x.Status == RecordStatusEnum.Active).Select().ToList();
        }
        public IQueryable<JournalModelK> GetaAllJournalPostedById(long companyId)
        {
            IQueryable<Company> serviceCompanyRepository = _journalRepository.GetRepository<Company>().Queryable();
            IQueryable<Journal> journalRepository = _journalRepository.Queryable();
            //IQueryable<JournalDetail> journalDetailRepository = _journalDetailRepository.Queryable();
            IQueryable<JournalModelK> journalModelkDetails =
                                            from jv in journalRepository
                                            from e in serviceCompanyRepository
                                            where (jv.ServiceCompanyId == e.ParentId)
                                            where jv.CompanyId == companyId
                                            where (jv.DocumentState != JournalState.Parked) && jv.Status == RecordStatusEnum.Active
                                            select new JournalModelK()
                                            {
                                                Id = jv.Id,
                                                CompanyId = jv.CompanyId,
                                                DocCurrency = jv.DocCurrency,
                                                CreatedDate = jv.CreatedDate,
                                                DocDate = jv.DocDate,
                                                DocNo = jv.DocNo,
                                                //DocDescription = jv.DocumentDescription,
                                                DocSubType = jv.DocSubType,
                                                DocType = jv.DocType,
                                                DocumentState = jv.DocumentState,
                                                ModifiedBy = jv.ModifiedBy,
                                                ModifiedDate = jv.ModifiedDate,
                                                SystemReferenceNumber = jv.SystemReferenceNo,
                                                SystemORManual = "Manual",
                                                //IsMultiCurrency = jv.IsMultiCurrency,
                                                //CompanyName = e.ShortName
                                                //PostingDate=jv.po
                                                //TotalCreditBC = (double)(jv.GrandBaseCreditTotal),
                                                //TotalCreditDc = (double)(jv.GrandDocCreditTotal),
                                                //TotalDebitBc = (double)(jv.GrandBaseDebitTotal),
                                                //TotalDebitDc = (double)(jv.GrandDocDebitTotal)
                                            };
            return journalModelkDetails.OrderBy(a => a.CreatedDate).AsQueryable();
        }
        public IQueryable<JournalModelParkedK> GetaAllJVParkedById(string username, long companyId)
        {
            IQueryable<Company> serviceCompanyRepository = _journalRepository.GetRepository<Company>().Queryable();
            IQueryable<Journal> journalRepository = _journalRepository.Queryable();
            //IQueryable<JournalDetail> journalDetailRepository = _journalDetailRepository.Queryable();
            IQueryable<JournalModelParkedK> journalModelParkedKDetails =
                                            from journal in journalRepository
                                            join e in serviceCompanyRepository on journal.ServiceCompanyId equals e.Id
                                            join compUser in _compUserRepo.Queryable() on e.ParentId equals compUser.CompanyId
                                            join cud in _compUserDetailRepo.Queryable() on compUser.Id equals cud.CompanyUserId
                                            where e.Id == cud.ServiceEntityId
                                            //where (journal.ServiceCompanyId == e.Id)
                                            where journal.CompanyId == companyId
                                            && (journal.DocumentState == JournalState.Parked) && compUser.Username == username
                                            select new JournalModelParkedK()
                                            {
                                                Id = journal.Id,
                                                CompanyId = journal.CompanyId,
                                                CreatedDate = journal.CreatedDate,
                                                DocCurrency = journal.DocSubType == DocTypeConstants.Revaluation ? journal.ExCurrency : journal.DocCurrency,
                                                DocDate = journal.DocDate,
                                                PostingDate = journal.PostingDate,
                                                DocDescription = journal.DocumentDescription,
                                                SystemReferenceNumber = journal.SystemReferenceNo,
                                                DocNo = journal.DocNo,
                                                IsLocked = journal.IsLocked,
                                                DocumentState = journal.DocumentState,
                                                NoSupportingDocument = journal.NoSupportingDocument,
                                                DocType = journal.DocType,
                                                DocSubType = journal.DocSubType,
                                                BaseCurrency = journal.ExCurrency,
                                                ExchangeRate = (journal.ExchangeRate).ToString(),
                                                ServiceCompanyName = e.ShortName,
                                                UserCreated = journal.UserCreated,
                                                ModifiedBy = journal.ModifiedBy,
                                                ModifiedDate = journal.ModifiedDate,
                                                GrandDocCreditTotal = journal.GrandDocCreditTotal != null ? (double)journal.GrandDocCreditTotal : 0,
                                                GrandDocDebitTotal = journal.GrandDocDebitTotal != null ? (double)journal.GrandDocDebitTotal : 0,
                                                GrandBaseCreditTotal = journal.GrandDocCreditTotal != null ? (double)journal.GrandDocCreditTotal : 0,
                                                GrandBaseDebitTotal = journal.GrandDocDebitTotal != null ? (double)journal.GrandDocDebitTotal : 0,
                                                RowVersion = journal.Version
                                            };
            return journalModelParkedKDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }
        public IQueryable<JournalModelRecurringK> GetaAllJVRecurringById(long companyId)
        {
            IQueryable<Company> serviceCompanyRepository = _journalRepository.GetRepository<Company>().Queryable();
            IQueryable<Journal> journalRepository = _journalRepository.Queryable();
            IQueryable<JournalDetail> journalDetailRepository = _journalDetailRepository.Queryable();
            IQueryable<JournalModelRecurringK> journalModelParkedKDetails =
                                            from jv in journalRepository
                                            from e in serviceCompanyRepository
                                            where (jv.ServiceCompanyId == e.Id)
                                            where jv.CompanyId == companyId
                                            where (jv.DocSubType == DocSubTypeConstants.Recurring) && jv.Status == RecordStatusEnum.Active
                                            select new JournalModelRecurringK()
                                            {
                                                Id = jv.Id,
                                                CompanyId = jv.CompanyId,
                                                DocCurrency = jv.DocCurrency,
                                                CreatedDate = jv.CreatedDate,
                                                DocDescription = jv.DocumentDescription,
                                                DocSubType = jv.DocSubType,
                                                DocType = jv.DocType,
                                                ModifiedBy = jv.ModifiedBy,
                                                ModifiedDate = jv.ModifiedDate,
                                                ServiceCompanyName = e.ShortName,
                                                FrequencyValue = jv.FrequencyValue,
                                                FrequencyEndDate = jv.FrequencyEndDate,
                                                UserCreated = jv.UserCreated,
                                                FrequencyType = jv.FrequencyType,
                                                Status = e.Status

                                            };
            return journalModelParkedKDetails.OrderBy(a => a.CreatedDate).AsQueryable();
        }

        public IQueryable<JournalModelParkedK> GetaAllVoidedK(long companyId)
        {
            IQueryable<Company> serviceCompanyRepository = _journalRepository.GetRepository<Company>().Queryable();
            IQueryable<Journal> journalRepository = _journalRepository.Queryable();
            //IQueryable<JournalDetail> journalDetailRepository = _journalDetailRepository.Queryable();
            IQueryable<JournalModelParkedK> journalModelParkedKDetails =
                                            from journal in journalRepository
                                            from e in serviceCompanyRepository
                                            where (journal.ServiceCompanyId == e.Id)
                                            && journal.CompanyId == companyId
                                            && (journal.DocumentState == JournalState.Void || journal.DocumentState == "Cancelled") && journal.Status == RecordStatusEnum.Active
                                            select new JournalModelParkedK()
                                            {
                                                Id = journal.Id,
                                                CompanyId = journal.CompanyId,
                                                CreatedDate = journal.CreatedDate,
                                                DocCurrency = journal.DocSubType == DocTypeConstants.Revaluation ? journal.ExCurrency : journal.DocCurrency,
                                                DocDate = journal.DocDate,
                                                PostingDate = journal.PostingDate,
                                                DocDescription = journal.DocumentDescription,
                                                SystemReferenceNumber = journal.SystemReferenceNo,
                                                DocNo = journal.DocNo,
                                                DocumentState = journal.DocumentState,
                                                NoSupportingDocument = journal.NoSupportingDocument,
                                                DocType = journal.DocType,
                                                DocSubType = journal.DocSubType,
                                                BaseCurrency = journal.ExCurrency,
                                                ExchangeRate = (journal.ExchangeRate).ToString(),
                                                ServiceCompanyName = e.ShortName,
                                                UserCreated = journal.UserCreated,
                                                ModifiedDate = journal.ModifiedDate,
                                                ModifiedBy = journal.ModifiedBy,
                                                GrandDocCreditTotal = journal.GrandDocCreditTotal != null ? (double)journal.GrandDocCreditTotal : 0,
                                                GrandDocDebitTotal = journal.GrandDocDebitTotal != null ? (double)journal.GrandDocDebitTotal : 0,
                                                GrandBaseCreditTotal = journal.GrandDocCreditTotal != null ? (double)journal.GrandDocCreditTotal : 0,
                                                GrandBaseDebitTotal = journal.GrandDocDebitTotal != null ? (double)journal.GrandDocDebitTotal : 0,
                                                RowVersion = journal.Version
                                            };
            return journalModelParkedKDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }

        public Journal GetBydocumentId(Guid documentId, long companyId)
        {
            return _journalRepository.Query(c => c.DocumentId == documentId && c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Include(c => c.JournalDetails).Select().FirstOrDefault();
        }
        public Journal GetByTransferdocumentId(Guid documentId, long companyId, bool? isWithdrawal)
        {
            return _journalRepository.Query(c => c.DocumentId == documentId && c.CompanyId == companyId && c.IsWithdrawal == isWithdrawal && c.Status == RecordStatusEnum.Active).Include(c => c.JournalDetails).Select().FirstOrDefault();
        }

        public List<Journal> GetListBydocumentId(Guid? documentId, long companyId)
        {
            return _journalRepository.Query(c => c.DocumentId == documentId && c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Include(c => c.JournalDetails).Select().ToList();
        }
        public List<Journal> GetAllRevalJournalByDocId(Guid? documentId, long companyId)
        {
            return _journalRepository.Query(c => c.DocumentId == documentId && c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Select().ToList();
        }
        public Journal GetByReceiptdocumentId(Guid documentId, long companyId)
        {
            return _journalRepository.Query(c => c.DocumentId == documentId && c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Include(c => c.JournalDetails).Select().FirstOrDefault();
        }
        public string GetJournalRefNo(Guid id, long companyId)
        {
            return _journalRepository.Query(x => x.Id == id && x.CompanyId == companyId && x.Status == RecordStatusEnum.Active).Select(c => c.DocNo).FirstOrDefault();
        }
        public List<Journal> GetAllRecurringJournal(long companyId, Guid journalId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.Id == journalId && x.IsRecurringJournal == true /*&& x.EndDate >= DateTime.UtcNow*/ && x.Status == AppsWorld.Framework.RecordStatusEnum.Active).Include(x => x.JournalDetails).Select().ToList();
        }
        public bool? GetNosupporting(long companyId)
        {
            var nos = _journalRepository.Query(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Select(c => c.IsNoSupportingDocs).FirstOrDefault();

            return nos != null ? true : false;
        }
        public bool? Getallowablenonallowable(long companyId)
        {
            bool? aallowdi = _journalRepository.Query(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Select(c => c.IsAllowableNonAllowable).FirstOrDefault();
            return aallowdi;
        }
        public Journal GetJournalByid(Guid id, long companyId)
        {
            return _journalRepository.Query(a => a.DocumentId == id && a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Include(c => c.JournalDetails).Select().FirstOrDefault();
        }
        public Journal CheckOpeningbalance(string docType, long companyId, long? serviceCompanyId)
        {
            return _journalRepository.Query(a => a.DocSubType == docType && a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public List<Journal> GetJournalReferenceNo(Guid? id, long companyId)
        {
            return _journalRepository.Query(x => x.ReverseParentRefId == id && x.CompanyId == companyId && x.Status == RecordStatusEnum.Active).Select().ToList();
        }

        public Journal GetJournalByCIDandDocSubType(long companyId, string docType, string docSubtype, string docNo)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.DocType == docType && a.DocSubType == docSubtype && a.DocNo == docNo && a.Status == RecordStatusEnum.Active && a.DocumentState == "Recurring" && a.Status == RecordStatusEnum.Active).Include(a => a.JournalDetails).Select().FirstOrDefault();
        }
        public Journal GetReccuringJournalByDocNoAndCompanyId(string docNo, long companyId)
        {
            return _journalRepository.Query(a => a.DocNo == docNo && a.CompanyId == companyId && a.DocSubType == DocSubTypeConstants.Recurring && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public Journal GetJournalByCIDandDocSubTypeAndId(Guid id, long companyId, string docType, string docSubtype, string docNo)
        {
            return _journalRepository.Query(a => a.Id == id && a.CompanyId == companyId && a.DocType == docType && a.DocSubType == docSubtype && a.DocNo == docNo && a.Status == RecordStatusEnum.Active).Include(a => a.JournalDetails).Select().FirstOrDefault();
        }

        public List<Journal> GetJournalByCompanyIdAndDocSubType(long companyId, string docSubType)
        {
            return _journalRepository.Query(c => c.CompanyId == companyId && c.DocType == DocTypeConstants.JournalVocher/* && c.DocSubType == docSubType || c.DocSubType == DocSubTypeConstants.Recurring*/ && c.DocumentState == "Parked" && c.Status == RecordStatusEnum.Active).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }

        public Journal GetJournalByIdandRecurringId(long companyId, Guid recurringId)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.RecurringJournalId == recurringId && a.DocumentState == "Parked" && a.Status == RecordStatusEnum.Active).Include(a => a.JournalDetails).Select().FirstOrDefault();
        }
        public Journal GetRecurringJournalByReccuringId(long companyId, Guid recurringId)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.RecurringJournalId == recurringId && a.DocumentState == DocSubTypeConstants.Recurring && a.Status == RecordStatusEnum.Active).Include(a => a.JournalDetails).Select().FirstOrDefault();
        }
        public List<string> GetDocNoByCompanyId(long companyId, string documentSatate, string docNo)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.DocumentState == documentSatate && a.DocNo == docNo && a.Status == RecordStatusEnum.Active).Select(a => a.DocNo).ToList();
        }
        public List<string> GetPostedJournalDocNo(long companyId, string documentState)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.DocumentState == documentState && a.Status == RecordStatusEnum.Active).Select(a => a.DocNo).ToList();
        }
        public IQueryable<JournalModelK> GetAllPostedJournal(long companyId, Guid? recurringJournalId)
        {
            IQueryable<JournalModelK> journalModelK = from jr in _journalRepository.Queryable()
                                                      join com in _journalRepository.GetRepository<Company>().Queryable()
                                                      on jr.CompanyId equals com.Id
                                                      where (jr.CompanyId == companyId && jr.RecurringJournalId == recurringJournalId && (jr.DocumentState == JournalState.Posted || jr.DocumentState == JournalState.Deleted|| jr.DocumentState == JournalState.Void) /*&& a.Status == RecordStatusEnum.Active*/)
                                                      select new JournalModelK()
                                                      {
                                                          Id = jr.Id,

                                                          CreatedDate = jr.CreatedDate,
                                                          DocCurrency = jr.DocCurrency,
                                                          PostingDate = jr.PostingDate,
                                                          DocNo = jr.DocNo,
                                                          DocumentState = jr.DocumentState,
                                                          //DocDescription = journal.DocumentDescription,
                                                          //DocDate = jr.DocDate,
                                                          //CompanyId = jr.CompanyId,
                                                          //DocType = jr.DocType,
                                                          //DocSubType = jr.DocSubType,
                                                          //BaseCurrency = journal.ExCurrency,
                                                          //SystemReferenceNumber = jr.SystemReferenceNo,
                                                          //CreationType = journal.CreationType,
                                                          UserCreated = jr.UserCreated,
                                                          ServiceCompanyName = com.Name,
                                                          InternalState = jr.DocumentState,
                                                          //GrandDocCreditTotal = jr.GrandDocCreditTotal != null ? (double)jr.GrandDocCreditTotal : 0,
                                                          GrandDocDebitTotal = jr.GrandDocDebitTotal != null ? (double)jr.GrandDocDebitTotal : 0,
                                                          GrandBaseCreditTotal = jr.GrandDocCreditTotal != null ? (double)jr.GrandDocCreditTotal : 0,
                                                          GrandBaseDebitTotal = jr.GrandDocDebitTotal != null ? (double)jr.GrandDocDebitTotal : 0,
                                                          RowVersion = jr.Version,
                                                          IsLocked = jr.IsLocked
                                                      };
            return journalModelK.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }

        public List<Journal> GetJournalByCidandRecurringId(long companyId, Guid recurringId)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.RecurringJournalId == recurringId && a.Status == RecordStatusEnum.Active && a.DocumentState != JournalState.Posted).Include(c => c.JournalDetails).Select().ToList();
        }

        public Journal GetAllJournalById(Guid id, long companyId)
        {
            return _journalRepository.Query(a => a.Id == id && a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Include(a => a.JournalDetails)/**/.Select().FirstOrDefault();
        }
        public async Task<Journal> GetAllJournalByIdAsync(Guid id, long companyId)
        {
            return await Task.Run(()=> _journalRepository.Query(a => a.Id == id && a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Include(a => a.JournalDetails).Select().FirstOrDefault());
        }
        public List<Journal> GetAllPostedJournalByCid(long companyId)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.DocType == DocTypeConstants.JournalVocher && (a.DocSubType == DocTypeConstants.General || a.DocSubType == DocSubTypeConstants.Auto_Reversal || a.DocSubType == DocSubTypeConstants.Revaluation) && ((a.DocumentState == JournalState.Posted || a.DocumentState == JournalState.Reversed || a.DocumentState == JournalState.Void) && a.Status == RecordStatusEnum.Active)).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }
        public Journal GetAllJournalByRecurringId(Guid id, long comapnyId)
        {
            return _journalRepository.Query(a => a.RecurringJournalId == id && a.CompanyId == a.CompanyId && a.Status == RecordStatusEnum.Active).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }

        //Update
        public List<string> GetPostedJournalsDocNo(long companyId, string documentState)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.Status == RecordStatusEnum.Active && (a.DocumentState == documentState || a.DocumentState == JournalState.Reversed) && a.Status == RecordStatusEnum.Active && a.DocType == DocTypeConstants.JournalVocher).Select(a => a.DocNo).ToList();
        }

        public Journal CreateJournalDocNo(long companyId, string documentState)
        {

            if (documentState == JournalState.Posted)
            {
                return _journalRepository.Query(x => x.CompanyId == companyId && x.Status == RecordStatusEnum.Active /*&& x.ReverseParentRefId == null*/ && x.DocType == DocTypeConstants.JournalVocher && x.DocumentState != "Void" && x.DocSubType == DocSubTypeConstants.General && x.DocumentState == documentState).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
            }

            else /*if(documentState==JournalState.Recurring)*/
            {
                return _journalRepository.Query(x => x.CompanyId == companyId && x.Status == RecordStatusEnum.Active/*&& x.ReverseParentRefId == null*/ && x.DocType == DocTypeConstants.JournalVocher && x.DocumentState != "Void" && x.DocumentState == documentState && x.DocSubType == DocSubTypeConstants.Recurring).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
            }

        }

        public Journal GetDocNoByDocumentSate(string docNo, long companyId, string documentState)
        {
            return _journalRepository.Query(x => x.DocNo == docNo && x.CompanyId == companyId && x.DocumentState == documentState && x.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public Journal GetDocumentByDIdandCompanyId(long companyId, Guid documentId, string docType)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.DocumentId == documentId && a.DocType == docType && a.Status == RecordStatusEnum.Active).Include(a => a.JournalDetails).Select().FirstOrDefault();
        }
        public IQueryable<InvoiceAuditTrailModel> GetDeletedAuditTrail(Guid journalId)
        {
            IQueryable<InvoiceAuditTrailModel> result = from invoice in _journalRepository.Queryable()
                                                        where invoice.RecurringJournalId == journalId && invoice.InternalState == "Deleted" && invoice.Status == RecordStatusEnum.Disable
                                                        select new InvoiceAuditTrailModel()
                                                        {
                                                            Status = invoice.Status,
                                                            DocNumber = invoice.DocNo,
                                                            Amount = invoice.GrandDocDebitTotal
                                                        };
            return result;
        }
        public List<Guid> GetJournalsIdByDocumentId(long companyId, Guid documentId, string docType)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.DocumentId == documentId && a.DocType == docType).Select(a => a.Id).ToList();
        }
        public List<Journal> GetLstOfJournals(List<Guid> journalIds)
        {
            return _journalRepository.Query(a => journalIds.Contains(a.Id)).Include(a => a.JournalDetails).Select().ToList();
        }
        public Journal GetDocumentIdByDIDAndDocSubType(long companyId, Guid documentId, string docSubType)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.DocumentId == documentId && a.DocSubType == docSubType && a.Status == RecordStatusEnum.Active).Include(a => a.JournalDetails).Select().FirstOrDefault();
        }

        public List<Journal> GetAllPostedJournals(long companyId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocType == DocTypeConstants.JournalVocher && x.Status == RecordStatusEnum.Active && x.InternalState == null && (x.DocumentState != JournalState.Parked && x.DocumentState != JournalState.Recurring /*&& x.DocumentState != JournalState.Void*/ && x.DocumentState != "Cancelled" && x.DocumentState != JournalState.Deleted)).Select().ToList();
        }
        public List<Journal> GetLstOFJournalsByDocumentId(Guid documentId, string docSubType, long companyId)
        {
            return _journalRepository.Query(a => a.DocumentId == documentId && a.CompanyId == companyId && a.DocType == docSubType && a.Status == RecordStatusEnum.Active).Include(a => a.JournalDetails).Select().ToList();
        }
        public async Task<DateTime> GetReversalDate(Guid? reversalId, long companyId)
        {
            return await Task.Run(()=> _journalRepository.Query(a => a.Id == reversalId && a.CompanyId == companyId && a.Status == RecordStatusEnum.Active)/**/.Select(a => a.DocDate).FirstOrDefault());
        }
        public async Task<Journal> GetJournalByIdAndCid(Guid id, long companyId)
        {
            return await Task.Run(()=> _journalRepository.Query(x => x.Id == id && x.CompanyId == companyId/* && x.Status == RecordStatusEnum.Active*/).Include(a => a.JournalDetails).Select().FirstOrDefault());
        }
        public Journal GetRecurringJournal(long companyId, Guid? recurringId)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.Id == recurringId && a.DocumentState == DocSubTypeConstants.Recurring).Select().FirstOrDefault();
        }
        public async Task<Journal> GetLastJournal(long companyId, string documentState)
        {
            return  await Task.Run(()=> _journalRepository.Query(c => c.CompanyId == companyId && c.DocType == DocTypeConstants.JournalVocher && c.Status == RecordStatusEnum.Active && (documentState == DocSubTypeConstants.Recurring ? (c.DocSubType == DocSubTypeConstants.Recurring && (c.DocumentState == DocSubTypeConstants.Recurring || c.InternalState != null)) : c.DocSubType == DocTypeConstants.General || c.DocSubType == DocTypeConstants.Revaluation)).Select().OrderByDescending(d => d.CreatedDate).FirstOrDefault());
        }

        //public List<Journal> GetAllRecurringsByCompanyId(long companyId)
        //{
        //    return _journalRepository.Query(x => x.CompanyId == companyId && x.DocSubType == DocSubTypeConstants.Recurring && (x.DocumentState == JournalState.Void || x.DocumentState == DocSubTypeConstants.Recurring) && x.InternalState != null && x.Status == RecordStatusEnum.Active).Select().ToList();
        //}
        public IQueryable<JournalModelRecurringK> GetAllRecurringsByCompanyId(long companyId, string username)
        {
            try
            {
                IQueryable<Company> companyRepo = _journalRepository.GetRepository<Company>().Queryable();
                IQueryable<Journal> journalAllRecurring = _journalRepository.Queryable();
                IQueryable<JournalModelRecurringK> journalModelRecurrings = from j in journalAllRecurring
                                                                            from c in companyRepo
                                                                            where (j.CompanyId == companyId)
                                                                            where (j.ServiceCompanyId == c.Id)
                                                                            join cu in _compUserRepo.Queryable() on c.ParentId equals cu.CompanyId
                                                                            join cud in _compUserDetailRepo.Queryable() on cu.Id equals cud.CompanyUserId
                                                                            where c.Id == cud.ServiceEntityId
                                                                            where (j.DocSubType == DocSubTypeConstants.Recurring &&
                                                                            (j.DocumentState == JournalState.Void || j.DocumentState == DocSubTypeConstants.Recurring)
                                                                            && j.InternalState != null && j.Status == RecordStatusEnum.Active) && cu.Username == username
                                                                            select new JournalModelRecurringK()
                                                                            {
                                                                                Id = j.Id,
                                                                                CreatedDate = j.CreatedDate,
                                                                                DocCurrency = j.DocSubType == DocTypeConstants.Revaluation ? j.ExCurrency : j.DocCurrency,
                                                                                DocDescription = j.DocumentDescription,
                                                                                UserCreated = j.UserCreated,
                                                                                ModifiedBy = j.ModifiedBy,
                                                                                ModifiedDate = j.ModifiedDate,
                                                                                ServiceCompanyName = c.Name,
                                                                                GrandDocDebitTotal = j.GrandDocDebitTotal,
                                                                                LastPosted = j.LastPosted,
                                                                                NextDue = j.NextDue,
                                                                                FrequencyEndDate = j.FrequencyEndDate,
                                                                                FrequencyValue = j.FrequencyValue,
                                                                                DocNo = j.DocNo,
                                                                                InternalState = j.InternalState,
                                                                                DocDate = j.DocDate,
                                                                                IsLocked = j.IsLocked,
                                                                                RowVersion = j.Version
                                                                            };
                return journalModelRecurrings.OrderByDescending(c => c.CreatedDate).AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IQueryable<JournalModelK>> NewGetAllPostedjournals(long companyId, string userName)
        {
           
            IQueryable<Company> companyRepository = await Task.Run(()=> _journalRepository.GetRepository<Company>().Queryable());
            IQueryable<JournalModelK> journals = from j in await Task.Run(()=> _journalRepository.Queryable().Where(a => a.CompanyId == companyId))
                                                 join c in companyRepository on j.ServiceCompanyId equals c.Id
                                                 join user in await Task.Run(()=> _compUserRepo.Queryable()) on c.ParentId equals user.CompanyId
                                                 join cud in await Task.Run(()=> _compUserDetailRepo.Queryable()) on user.Id equals cud.CompanyUserId
                                                 where c.Id == cud.ServiceEntityId
                                                 where j.CompanyId == companyId && user.Username == userName && j.DocType == DocTypeConstants.JournalVocher && j.Status == RecordStatusEnum.Active && j.InternalState == null && (j.DocumentState != JournalState.Parked && j.DocumentState != JournalState.Recurring  && j.DocumentState != "Cancelled" && j.DocumentState != JournalState.Deleted)
                                                
                                                 select new JournalModelK()
                                                 {
                                                     Id = j.Id,
                                                     CompanyId = j.CompanyId,
                                                     CreatedDate = j.CreatedDate,
                                                     DocCurrency = j.DocSubType == DocTypeConstants.Revaluation ? j.ExCurrency : j.DocCurrency,
                                                     DocDate = j.DocDate,
                                                     DocNo = j.DocNo,
                                                     DocumentState = (j.ClearingStatus != null && j.ClearingStatus != string.Empty) ? j.ClearingStatus : j.DocumentState,
                                                     DocType = j.DocType,
                                                     DocSubType = j.DocSubType,
                                                     DocumentId = j.DocumentId,
                                                     IsSystem = j.CreationType == "System",
                                                     IsLocked = j.IsLocked,
                                                     GrandBaseDebitTotal = j.GrandBaseDebitTotal != null ? (double)j.GrandBaseDebitTotal : 0,
                                                     GrandBaseCreditTotal = j.GrandBaseCreditTotal != null ? (double)j.GrandBaseCreditTotal : 0,
                                                     GrandDocCreditTotal = j.GrandDocCreditTotal != null ? (double)j.GrandDocCreditTotal : 0,
                                                     GrandDocDebitTotal = j.GrandDocDebitTotal != null ? (double)j.GrandDocDebitTotal : 0,
                                                     ServiceCompanyId = j.ServiceCompanyId,
                                                     ServiceCompanyName = c.ShortName,
                                                     SystemReferenceNumber = j.SystemReferenceNo,
                                                     UserCreated = j.UserCreated,
                                                     ModifiedBy = j.ModifiedBy,
                                                     ModifiedDate = j.ModifiedDate,
                                                     IsModify = j.JournalDetails.Any(d => d.ClearingDate != null && d.ClearingStatus == "Cleared"),
                                             
                                                     BaseAmount = (double)(j.GrandBaseCreditTotal ?? j.GrandBaseDebitTotal),
                                                     ExchangeRate = (j.ExchangeRate).ToString(),
                                                     Status = j.Status,
                                                     DocDescription = j.DocumentDescription,
                                                     RowVersion = j.Version,
                                                     ReversalDate = j.ReversalDate
                                                 };
            return journals.OrderByDescending(a => a.CreatedDate).AsQueryable();

        }
        public bool IsVoid(long companyId, Guid id)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => a.DocumentState == JournalState.Void).FirstOrDefault() == true;
        }
    }
}

