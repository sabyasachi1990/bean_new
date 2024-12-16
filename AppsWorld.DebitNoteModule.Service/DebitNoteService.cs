using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.DebitNoteModule.RepositoryPattern;
using AppsWorld.DebitNoteModule.Models;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Entities;

namespace AppsWorld.DebitNoteModule.Service
{
    public class DebitNoteService : Service<DebitNote>, IDebitNoteService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<DebitNote> _debitNoteRepository;
        private readonly IDebitNoteMoluleRepositoryAsync<BeanEntity> _beanEntityRepository;
        private readonly IDebitNoteMoluleRepositoryAsync<TermsOfPayment> _termsOfPaymentRepository;
        private readonly IDebitNoteMoluleRepositoryAsync<Company> _companyRepository;
        private readonly IDebitNoteMoluleRepositoryAsync<CompanyUser> _compUserRepo;
        private readonly IDebitNoteMoluleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUserDetailRepo;
        public DebitNoteService(IDebitNoteMoluleRepositoryAsync<DebitNote> debitNoteRepository, IDebitNoteMoluleRepositoryAsync<TermsOfPayment> termsOfPaymentRepository, IDebitNoteMoluleRepositoryAsync<BeanEntity> beanEntityRepository, IDebitNoteMoluleRepositoryAsync<Company> companyRepository, IDebitNoteMoluleRepositoryAsync<CompanyUser> compUserRepo, IDebitNoteMoluleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepo)
            : base(debitNoteRepository)
        {
            _debitNoteRepository = debitNoteRepository;
            _beanEntityRepository = beanEntityRepository;
            _termsOfPaymentRepository = termsOfPaymentRepository;
            _companyRepository = companyRepository;
            _compUserRepo = compUserRepo;
            _compUserDetailRepo = compUserDetailRepo;
        }
        public DebitNote GetDebitNoteById(Guid id, long companyId)
        {
            return _debitNoteRepository.Query(x => x.Id == id && x.CompanyId == companyId).Include(x => x.DebitNoteDetails).Select().FirstOrDefault();
        }
        public DebitNote GetDebitNote(Guid id, long companyId)
        {
            return _debitNoteRepository.Query(x => x.Id == id && x.CompanyId == companyId).Include(x => x.DebitNoteDetails).Include(x => x.DebitNoteNotes).Select().FirstOrDefault();
        }
        public DebitNote CreateDebitNote(long companyId)
        {
            return _debitNoteRepository.Query(e => e.CompanyId == companyId).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public DebitNote CreateDebitNoteForDocNo(long companyId)
        {
            return _debitNoteRepository.Query(x => x.CompanyId == companyId && x.DocumentState != "Void").Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public DebitNote GetDocNo(string docNo, long companyId)
        {
            return _debitNoteRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public DebitNote GetDebitNoteDocNo(Guid id, string docNo, long companyId)
        {
            return _debitNoteRepository.Query(x => x.Id != id && x.DocNo == docNo && x.CompanyId == companyId && x.DocumentState != "Void").Select().FirstOrDefault();
        }
        public async Task<IQueryable<DebitNoteKModel>> GetAllDebitNotesK(string userName, long companyId)
        {

            try
            {
                IQueryable<DebitNote> lstDebitNotes = await Task.Run(() => _debitNoteRepository.Queryable().Where(c => c.CompanyId == companyId && c.Status < RecordStatusEnum.Disable));
                IQueryable<BeanEntity> beanEntityRepository = await Task.Run(() => _beanEntityRepository.Queryable().Where(a => a.CompanyId == companyId));

                IQueryable<DebitNoteKModel> debitNoteKModel =
                                                     from debitnote in lstDebitNotes
                                                     from beanentity in beanEntityRepository
                                                     where debitnote.EntityId == beanentity.Id
                                                     join company in await Task.Run(() => _companyRepository.Queryable()) on debitnote.ServiceCompanyId equals company.Id
                                                     join compUser in await Task.Run(() => _compUserRepo.Queryable()) on company.ParentId equals compUser.CompanyId
                                                     join cud in await Task.Run(() => _compUserDetailRepo.Queryable()) on compUser.Id equals cud.CompanyUserId
                                                     where company.Id == cud.ServiceEntityId
                                                     where debitnote.CompanyId == companyId && compUser.Username == userName
                                                     select new DebitNoteKModel()
                                                     {
                                                         Id = debitnote.Id,
                                                         CompanyId = debitnote.CompanyId,
                                                         DocDate = debitnote.DocDate,
                                                         DueDate = debitnote.DueDate,
                                                         DocNo = debitnote.DocNo,
                                                         EntityName = beanentity.Name,
                                                         EntityId = beanentity.Id,
                                                         GrandTotal = (double)(debitnote.GrandTotal),
                                                         DocumentState = debitnote.DocumentState,
                                                         Nature = debitnote.Nature,
                                                         Status = debitnote.Status,
                                                         BalanceAmount = (double)(debitnote.BalanceAmount),
                                                         CreatedDate = debitnote.CreatedDate,
                                                         UserCreated = debitnote.UserCreated,
                                                         ModifiedBy = debitnote.ModifiedBy,
                                                         ModifiedDate = debitnote.ModifiedDate,
                                                         DocCurrency = debitnote.DocCurrency,
                                                         ServiceCompanyName = company.ShortName,
                                                         ServiceCompanyId = company.Id,
                                                         ExchangeRate = (debitnote.ExchangeRate).ToString(),
                                                         BaseBal = Math.Round((double)(debitnote.BalanceAmount * debitnote.ExchangeRate), 2),
                                                         BaseTotal = Math.Round((double)(debitnote.GrandTotal * debitnote.ExchangeRate), 2),
                                                         ScreenName = "Debit Note",
                                                         IsLocked = debitnote.IsLocked
                                                     };
                return debitNoteKModel.OrderByDescending(x => x.CreatedDate).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DebitNote> GetAllDebitModel(long companyId)
        {
            return _debitNoteRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).ToList();
        }
        public DebitNote GetDebittNote(Guid id)
        {
            return _debitNoteRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
        public DateTime? GetDNLastPostedDate(long companyId)
        {
            return _debitNoteRepository.Query(e => e.CompanyId == companyId).Select().OrderByDescending(a => a.CreatedDate).Select(a => a.CreatedDate).FirstOrDefault();
        }
        public bool IsVoid(long companyId, Guid id)
        {
            return _debitNoteRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => a.DocumentState == "Void").FirstOrDefault() == true;
        }
    }
}
