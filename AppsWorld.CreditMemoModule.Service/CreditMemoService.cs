using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CreditMemoModule.RepositoryPattern;
using AppsWorld.CreditMemoModule.Entities;
using Service.Pattern;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CreditMemoModule.Infra;
using AppsWorld.CreditMemoModule.Models;
using AppsWorld.CommonModule.Entities;

namespace AppsWorld.CreditMemoModule.Service
{
    public class CreditMemoService : Service<CreditMemo>, ICreditMemoService
    {
        private readonly ICreditMemoModuleRepositoryAsync<CreditMemo> _creditMemoRepository;
        private readonly ICreditMemoModuleRepositoryAsync<BeanEntity> _beanEntityRepository;
        private readonly ICreditMemoModuleRepositoryAsync<CompanyUser> _compUserRepo;
        private readonly ICreditMemoModuleRepositoryAsync<Company> _companyRepo;
        private readonly ICreditMemoModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUserDetailRepo;
        public CreditMemoService(ICreditMemoModuleRepositoryAsync<CreditMemo> creditMemoRepository, ICreditMemoModuleRepositoryAsync<BeanEntity> beanEntityRepository, ICreditMemoModuleRepositoryAsync<CompanyUser> compUserRepo, ICreditMemoModuleRepositoryAsync<Company> companyRepo, ICreditMemoModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> companyUserDetailRepo) : base(creditMemoRepository)
        {
            _creditMemoRepository = creditMemoRepository;
            _beanEntityRepository = beanEntityRepository;
            _compUserRepo = compUserRepo;
            _companyRepo = companyRepo;
            _compUserDetailRepo = companyUserDetailRepo;
        }

        public CreditMemo GetCreditMemoByCompanyId(long companyId, Guid id)
        {
            return _creditMemoRepository.Query(e => e.CompanyId == companyId && e.Id == id && e.DocType == DocTypeConstants.BillCreditMemo).Include(a => a.CreditMemoDetails).Select().FirstOrDefault();
        }
        public CreditMemo GetCreditMemoByCompanyId(long companyId)
        {
            return _creditMemoRepository.Query(e => e.DocType == DocTypeConstants.BillCreditMemo && e.CompanyId == companyId && e.DocSubType != DocTypeConstants.OpeningBalance).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public CreditMemo GetAllMemoByDoctypeAndCompanyId(string docType, long companyId)
        {
            return _creditMemoRepository.Query(a => a.DocType == docType && a.CompanyId == companyId && a.DocumentState != CreditMemoState.Void).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }
        public CreditMemo GetMemos(string strNewDocNo, string docType, long CompanyId)
        {
            return _creditMemoRepository.Query(a => a.DocNo == strNewDocNo && a.DocType == docType && a.CompanyId == CompanyId).Select().FirstOrDefault();
        }
        public CreditMemo GetCreditMemoById(Guid id)
        {
            return _creditMemoRepository.Query(e => e.Id == id).Include(a => a.CreditMemoDetails).Include(c => c.BeanEntity).Select().FirstOrDefault();
        }
        public IQueryable<CreditMemoModelK> GetAllCreditMemoK(string username, long companyId)
        {
            try
            {
                IQueryable<BeanEntity> beanEntityRepository = _beanEntityRepository.Queryable();
                IQueryable<CreditMemo> creditMemoRepository = _creditMemoRepository.Queryable();
                IQueryable<CreditMemoModelK> creditMemoModelK = from memo in creditMemoRepository
                                                                join beanEntity in beanEntityRepository on memo.EntityId equals beanEntity.Id
                                                                join company in _companyRepo.Queryable() on memo.ServiceCompanyId equals company.Id
                                                                join compUser in _compUserRepo.Queryable() on company.ParentId equals compUser.CompanyId
                                                                join cud in _compUserDetailRepo.Queryable() on compUser.Id equals cud.CompanyUserId where company.Id == cud.ServiceEntityId
                                                                //where (memo.EntityId == beanEntity.Id)
                                                                where memo.CompanyId == companyId
                                                                where memo.DocType == DocTypeConstants.BillCreditMemo
                                                                && compUser.Username == username
                                                                select new CreditMemoModelK()
                                                                {
                                                                    Id = memo.Id,
                                                                    CompanyId = memo.CompanyId,
                                                                    DocNo = memo.DocNo,
                                                                    DueDate = memo.DueDate,
                                                                    DocDate = memo.DocDate,
                                                                    DocumentState = memo.DocumentState,
                                                                    EntityName = beanEntity.Name,
                                                                    GrandTotal = (double)(memo.GrandTotal),
                                                                    // CreditMemoNumber = memo.CreditMemoNumber,
                                                                    CreatedDate = memo.CreatedDate,
                                                                    DocCurrency = memo.DocCurrency,
                                                                    ExCurrency = memo.ExCurrency,
                                                                    BaseAmount = Math.Round((double)(memo.GrandTotal * memo.ExchangeRate), 2),
                                                                    Nature = memo.Nature,
                                                                    ExchangeRate = (memo.ExchangeRate).ToString(),
                                                                    ServiceCompanyName = company.ShortName,
                                                                    BalanceAmount = (double?)memo.BalanceAmount,
                                                                    DocSubType = memo.DocSubType,
                                                                    BaseBalance = Math.Round((double)(memo.BalanceAmount * memo.ExchangeRate), 2),
                                                                    UserCreated = memo.UserCreated,
                                                                    ModifiedBy = memo.ModifiedBy,
                                                                    ModifiedDate = memo.ModifiedDate,
                                                                    IsExternal = /*memo.ExtensionType == "OBCM" &&*/ (memo.DocSubType == DocTypeConstants.OpeningBalance||memo.DocSubType==DocTypeConstants.Interco) ? true : false,
                                                                    PostingDate = memo.PostingDate,
                                                                    IsLocked = memo.IsLocked,
                                                                    DocType = memo.DocType
                                                                };
                return creditMemoModelK.OrderByDescending(a => a.CreatedDate).AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CreditMemo GetMemoLU(long companyId, Guid creditId)
        {
            return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.Id == creditId && a.DocType == DocTypeConstants.BillCreditMemo).Include(a => a.CreditMemoDetails).Select().FirstOrDefault();
        }
        public CreditMemo GetMemo(Guid creditMemoId)
        {
            return _creditMemoRepository.Query(x => x.Id == creditMemoId).Select().FirstOrDefault();
        }
        public CreditMemo CheckDocExists(Guid Id, string docType, string docNo, long companyId, Guid entityId)
        {
            return _creditMemoRepository.Queryable().Where(c => c.Id != Id && c.DocType == docType && c.DocNo == docNo && c.CompanyId == companyId && c.DocumentState != "Void" && c.EntityId == entityId).FirstOrDefault();
        }
        public List<CreditMemo> GetCompanyIdAndDocType(long companyId)
        {
            return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.DocType == DocTypeConstants.BillCreditMemo).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }
        public List<CreditMemo> GetTaggedMemoByCustomerAndCurrency(Guid customerId, string currency, long companyId)
        {
            return _creditMemoRepository.Query(a => a.EntityId == customerId && a.DocCurrency == currency && a.CompanyId == companyId && a.AllocatedAmount > 0).Select().ToList();
        }
        public CreditMemo GetByCompanyId(long companyId)
        {
            return _creditMemoRepository.Query(e => e.CompanyId == companyId).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public CreditMemo GetMemoByDocId(long companyId, Guid? docId)
        {
            return _creditMemoRepository.Query(e => e.CompanyId == companyId && e.Id == docId).Select().FirstOrDefault();
        }

        public bool IsDocumentNumberExists(Guid Id, string docType, string docNo, long companyId, Guid entityId)
        {
            return _creditMemoRepository.Queryable().Where(c => c.Id != Id && c.DocType == docType && c.DocNo == docNo && c.CompanyId == companyId && c.DocumentState != "Void" && c.EntityId == entityId).Any();
        }

        public bool IsVoid(long companyId, Guid id)
        {
            return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => a.DocumentState == InvoiceStates.Void).FirstOrDefault() == true;
        }


    }
}
