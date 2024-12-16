using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.DebitNoteModule.Entities.V2;
using AppsWorld.DebitNoteModule.RepositoryPattern.V2;
using Service.Pattern;
using System;
using System.Linq;
using AppsWorld.DebitNoteModule.Models;

namespace AppsWorld.DebitNoteModule.Service.V2
{
    public class DebitNoteKService : Service<DebitNoteK>, IDebitNoteKService
    {
        private readonly IDebitNoteKRepositoryAsync<DebitNoteK> _debitNoteKRepository;
        private readonly IDebitNoteKRepositoryAsync<BeanEntityCompact> _beanEntityRepository;
        private readonly IDebitNoteKRepositoryAsync<CompanyCompact> _companyRepository;
        private readonly IDebitNoteKRepositoryAsync<CompanyUserCompact> _companyUserRepository;


        public DebitNoteKService(IDebitNoteKRepositoryAsync<DebitNoteK> debitNoteKRepository, IDebitNoteKRepositoryAsync<BeanEntityCompact> beanEntityRepository, IDebitNoteKRepositoryAsync<CompanyCompact> companyRepository, IDebitNoteKRepositoryAsync<CompanyUserCompact> companyUserRepository)
            : base(debitNoteKRepository)
        {
            this._debitNoteKRepository = debitNoteKRepository;
            this._beanEntityRepository = beanEntityRepository;
            this._companyRepository = companyRepository;
            this._companyUserRepository = companyUserRepository;
        }

        public IQueryable<DebitNoteKModel> GetAllDebitNotesK(string username, long companyId)
        {

            //IQueryable<DebitNoteK> lstDebitNotes = _debitNoteKRepository.Queryable().Where(c => c.CompanyId == companyId && c.Status < RecordStatusEnum.Disable).AsQueryable();
            IQueryable<BeanEntityCompact> beanEntityRepository = _beanEntityRepository.Queryable();
            IQueryable<DebitNoteKModel> debitNoteKModel =
                                                 from debitnote in _debitNoteKRepository.Queryable()
                                                 from beanentity in beanEntityRepository
                                                 where debitnote.EntityId == beanentity.Id
                                                 join company in _companyRepository.Queryable() on debitnote.ServiceCompanyId equals company.Id
                                                 join compUser in _companyUserRepository.Queryable() on company.ParentId equals compUser.CompanyId
                                                 where debitnote.CompanyId == companyId
                                                 && (compUser.ServiceEntities != null ? compUser.ServiceEntities.Contains(company.Id.ToString()) : true) && compUser.Username == username
                                                 select new DebitNoteKModel()
                                                 {
                                                     Id = debitnote.Id,
                                                     CompanyId = debitnote.CompanyId,
                                                     DocDate = debitnote.DocDate,
                                                     DueDate = debitnote.DueDate,
                                                     DocNo = debitnote.DocNo,
                                                     EntityName = beanentity.Name,
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
                                                     ExchangeRate = (debitnote.ExchangeRate).ToString(),
                                                     BaseBal = Math.Round((double)(debitnote.BalanceAmount * debitnote.ExchangeRate), 2),
                                                     BaseTotal = Math.Round((double)(debitnote.GrandTotal * debitnote.ExchangeRate), 2),
                                                     ScreenName = "Debit Note"
                                                 };
            return debitNoteKModel.OrderByDescending(x => x.CreatedDate).AsQueryable();
        }
    }
}
