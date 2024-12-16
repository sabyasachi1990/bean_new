using System;
using AppsWorld.CashSalesModule.Models;
using AppsWorld.CashSalesModule.RepositoryPattern.V2;
using Service.Pattern;
using System.Linq;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CashSalesModule.Entities.V2;

namespace AppsWorld.CashSalesModule.Service.V2
{
    public class CashSalesKService : Service<CashSaleK>, ICashSalesKService
    {
        private readonly ICashSalesKModuleRepositoryAsync<CashSaleK> _cashSaleRepository;
        private readonly ICashSalesKModuleRepositoryAsync<CompanyK> _companyRepository;
        private readonly ICashSalesKModuleRepositoryAsync<ChartOfAccountK> _coaRepository;
        private readonly ICashSalesKModuleRepositoryAsync<CompanyUserK> _compUserRepo;
        private readonly ICashSalesKModuleRepositoryAsync<BeanEntityK> _entityRepository;

        public CashSalesKService(ICashSalesKModuleRepositoryAsync<CashSaleK> cashSaleRepository, ICashSalesKModuleRepositoryAsync<CompanyK> companyRepository, ICashSalesKModuleRepositoryAsync<ChartOfAccountK> coaRepository, ICashSalesKModuleRepositoryAsync<CompanyUserK> compUserRepo, ICashSalesKModuleRepositoryAsync<BeanEntityK> entityRepository)
            : base(cashSaleRepository)
        {
            _cashSaleRepository = cashSaleRepository;
            _companyRepository = companyRepository;
            _coaRepository = coaRepository;
            _compUserRepo = compUserRepo;
            this._entityRepository = entityRepository;
        }
        public IQueryable<CashSaleModelK> GetAllCashSalesK(string username, long companyId)
        {
            //IQueryable<BeanEntity> beanEntityRepository = _cashSaleRepository.GetRepository<BeanEntity>().Queryable();
            IQueryable<CashSaleK> cashSaleRepository = _cashSaleRepository.Queryable();
            IQueryable<CashSaleModelK> cashSaleModelK = from b in cashSaleRepository
                                                        join e in _entityRepository.Queryable() on b.EntityId equals e.Id into grp
                                                        from d in grp.DefaultIfEmpty()
                                                        join company in _companyRepository.Queryable() on b.ServiceCompanyId equals company.Id
                                                        join coa in _coaRepository.Queryable() on b.COAId equals coa.Id
                                                        join compUser in _compUserRepo.Queryable() on company.ParentId equals compUser.CompanyId // newly added for svcentity permissions
                                                        where b.CompanyId == companyId
                                                        && (compUser.ServiceEntities != null ? compUser.ServiceEntities.Contains(company.Id.ToString()) : true) && compUser.Username == username// newly added for svcentity permissions
                                                        select new CashSaleModelK()
                                                        {
                                                            Id = b.Id,
                                                            CompanyId = b.CompanyId,
                                                            DocNo = b.DocNo,
                                                            EntityName = d.Name,
                                                            DocumentState = b.DocumentState,
                                                            DocDate = b.DocDate,
                                                            GrandTotal = (double)(b.GrandTotal),
                                                            CreatedDate = b.CreatedDate,
                                                            BaseAmount = Math.Round((double)(b.GrandTotal * b.ExchangeRate), 2),
                                                            DocCurrency = b.DocCurrency,
                                                            ModeOfReceipt = b.ModeOfReceipt,
                                                            ReceiptrefNo = b.ReceiptrefNo,
                                                            PONo = b.PONo,
                                                            ExchangeRate = (b.ExchangeRate).ToString(),
                                                            CashBankAccount = coa.Name,
                                                            ServiceCompanyName = company.ShortName,
                                                            BankClearingDate = b.BankClearingDate,
                                                            UserCreated = b.UserCreated,
                                                            ModifiedDate = b.ModifiedDate,
                                                            ModifiedBy = b.ModifiedBy,
                                                            ScreenName = "Cash Sale"
                                                        };
            return cashSaleModelK.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }
    }
}
