using System;
using System.Collections.Generic;
using AppsWorld.BankWithdrawalModule.Entities;
using AppsWorld.BankWithdrawalModule.Models;
using AppsWorld.BankWithdrawalModule.RepositoryPattern;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using System.Linq;
using AppsWorld.BankWithdrawalModule.Infra;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Service
{
    public class WithdrawalService : Service<Withdrawal>, IWithdrawalService
    {
        private readonly IBankWithdrawalModuleRepositoryAsync<Withdrawal> _bankWithdrawalRepository;
        private readonly IBankWithdrawalModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ChartOfAccount> _chartOfAccountRepository;
        private readonly IBankWithdrawalModuleRepositoryAsync<AppsWorld.CommonModule.Entities.BeanEntity> _beanEntityRepository;
        private readonly IBankWithdrawalModuleRepositoryAsync<CompanyUser> _compUserRepo;
        private readonly IBankWithdrawalModuleRepositoryAsync<Company> _companyRepo;
        private readonly IBankWithdrawalModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUserDetailRepo;
        public WithdrawalService(IBankWithdrawalModuleRepositoryAsync<Withdrawal> bankWithdrwalRepository
            , IBankWithdrawalModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ChartOfAccount> chartOfAccountRepository, IBankWithdrawalModuleRepositoryAsync<AppsWorld.CommonModule.Entities.BeanEntity> beanEntityRepository, IBankWithdrawalModuleRepositoryAsync<CompanyUser> compUserRepo, IBankWithdrawalModuleRepositoryAsync<Company> CompanyRepo, IBankWithdrawalModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepo)
            : base(bankWithdrwalRepository)
        {
            _bankWithdrawalRepository = bankWithdrwalRepository;
            _chartOfAccountRepository = chartOfAccountRepository;
            _beanEntityRepository = beanEntityRepository;
            _compUserRepo = compUserRepo;
            _companyRepo = CompanyRepo;
            _compUserDetailRepo = compUserDetailRepo;
        }
        public Withdrawal GetWithdrawal(Guid id, long companyId, string docType)
        {
            return _bankWithdrawalRepository.Query(x => x.Id == id && x.CompanyId == companyId && x.DocType == docType).Select().FirstOrDefault();
        }
        public async Task<Withdrawal> GetWithdrawalById(Guid withdrawalId, long companyId)
        {
            return await Task.Run(()=> _bankWithdrawalRepository.Query(x => x.Id == withdrawalId && x.CompanyId == companyId).Include(c => c.WithdrawalDetails).Select().FirstOrDefault());
        }
        public async Task<Withdrawal> GetWithdrawalByIdAsync(Guid withdrawalId, long companyId)
        {
            return await Task.Run(()=> _bankWithdrawalRepository.Query(x => x.Id == withdrawalId && x.CompanyId == companyId).Include(c => c.WithdrawalDetails).Select().FirstOrDefault());
        }
        public async Task<Withdrawal> CreateWithdrawal(long companyId, string docType)
        {
            return await Task.Run(()=> _bankWithdrawalRepository.Query(x => x.CompanyId == companyId && x.DocType == docType).Select().OrderByDescending(d => d.CreatedDate).FirstOrDefault());
        }
       
        public Withdrawal GetDocNo(string docNo, long companyId, string docType)
        {
            return _bankWithdrawalRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId && c.DocType == docType).Select().FirstOrDefault();
        }
        public Withdrawal GetWithdrawalDocNo(Guid id, string docNo, long companyId, string docType)
        {
            var lst = _bankWithdrawalRepository.Query(x => x.Id != id && x.DocNo != string.Empty && x.CompanyId == companyId && x.DocType == docType && x.DocumentState != WithdrawalState.Void).Select().ToList();
            return lst.Where(x => x.DocNo == docNo).FirstOrDefault();
        }
        public Withdrawal GetWithdraw(Guid id, long companyId, string docType)
        {
            return _bankWithdrawalRepository.Query(x => x.Id == id && x.CompanyId == companyId && x.DocType == docType).Include(x => x.WithdrawalDetails).Select().FirstOrDefault();
        }
        public async Task<Withdrawal> GetWithdrawAsync(Guid id, long companyId, string docType)
        {
            return await Task.Run(()=> _bankWithdrawalRepository.Query(x => x.Id == id && x.CompanyId == companyId && x.DocType == docType).Include(x => x.WithdrawalDetails).Select().FirstOrDefault());
        }
        public List<string> GetAutoNumber(long companyId, string docType)
        {
            return _bankWithdrawalRepository.Queryable().Where(x => x.CompanyId == companyId && x.DocType == docType).Select(x => x.SystemRefNo).ToList();
        }
        public  async Task<IQueryable<BankWithdrawalModelK>> GetAllBankWithdralK(string username, long companyId, string docType)
        {
            
            IQueryable<Withdrawal> withdrawalRepository = await Task.Run(()=> _bankWithdrawalRepository.Queryable());
            IQueryable<Company> companyRepository = await Task.Run(()=> _bankWithdrawalRepository.GetRepository<Company>().Queryable());
           
            IQueryable<BankWithdrawalModelK> withdrawalModelKDetails =await Task.Run(()=>  from b in withdrawalRepository
                                                                       join coa in _chartOfAccountRepository.Queryable() on b.COAId equals coa.Id
                                                                       join be in _beanEntityRepository.Queryable() on b.EntityId equals be.Id into grp
                                                                       from d in grp.DefaultIfEmpty()
                                                                           
                                                                       join c in companyRepository on b.ServiceCompanyId equals c.Id
                                                                       join compUser in _compUserRepo.Queryable() on c.ParentId equals compUser.CompanyId
                                                                       join cud in _compUserDetailRepo.Queryable() on compUser.Id equals cud.CompanyUserId where c.Id == cud.ServiceEntityId
                                                                       where b.CompanyId == companyId
                                                                      
                                                                       && b.DocType == docType
                                                                       && b.EntityId == d.Id
                                                                       && b.COAId == coa.Id
                                                                       && compUser.Username == username
                                                                       select new BankWithdrawalModelK()
                                                                       {
                                                                           Id = b.Id,
                                                                           CompanyId = b.CompanyId,
                                                                           DocNo = b.DocNo,
                                                                           DocDate = b.DocDate,
                                                                           SystemRefNo = b.SystemRefNo,
                                                                          
                                                                           EntityId = b.EntityId,
                                                                           EntityName = d.Name,
                                                                           UserCreated = b.UserCreated,
                                                                           ServiceCompanyName = c.ShortName,
                                                                           DocumentState = b.DocumentState,
                                                                           ModeOfWithdrawal = b.ModeOfWithDrawal,
                                                                           CreatedDate = b.CreatedDate,
                                                                           ModifiedBy = b.ModifiedBy,
                                                                           ModifiedDate = b.ModifiedDate,                     
                                                                           BaseAmount = Math.Round((double)(b.GrandTotal * b.ExchangeRate),2),
                                                                           GrandTotal = (double)b.GrandTotal,
                                                                           BankClearingDate = b.BankClearingDate,
                                                                           WithdrawalRefNo = b.WithDrawalRefNo,
                                                                           ExchangeRate = (b.ExchangeRate).ToString(),
                                                                           COAId = b.COAId,
                                                                           DocCurrency = b.DocCurrency,
                                                                           CashBankAccount = coa.Name,
                                                                           IsLocked = b.IsLocked,
                                                                           DocType = b.DocType
                                                                       });
            return withdrawalModelKDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }
        public Withdrawal CreateWithdrawalforDocNo(long companyId, string docType)
        {
            return _bankWithdrawalRepository.Query(x => x.CompanyId == companyId && x.DocType == docType && x.DocumentState != WithdrawalState.Void).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public List<Withdrawal> GetAllPaymentModel(long companyId)
        {
            return _bankWithdrawalRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).ToList();
        }
        //public void Insert(Withdrawal withdrawal)
        //{
        //    _bankWithdrawalRepository.Insert(withdrawal);
        //}
        //public void Update(Withdrawal withdrawal)
        //{
        //    _bankWithdrawalRepository.Update(withdrawal);
        //}


        public bool IsVoid(long companyId, Guid id)
        {
            return _bankWithdrawalRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => a.DocumentState == WithdrawalState.Void).FirstOrDefault() == true;
        }
    }
}
