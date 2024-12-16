using System;
using AppsWorld.CashSalesModule.Entities;
using AppsWorld.CashSalesModule.Models;
using AppsWorld.CashSalesModule.RepositoryPattern;
using Service.Pattern;
using System.Linq;
using AppsWorld.CommonModule.Entities;
using FrameWork;
using System.Collections.Generic;
using AppsWorld.CashSalesModule.Entities.Models;
using AppsWorld.Framework;
using AppsWorld.CashSalesModule.Infra;

namespace AppsWorld.CashSalesModule.Service
{
    public class CashSalesService : Service<CashSale>, ICashSalesService
    {
        private readonly ICashSalesModuleRepositoryAsync<CashSale> _cashSaleRepository;
        private readonly ICashSalesModuleRepositoryAsync<CashSaleDetail> _cashSaleDetailRepository;
        private readonly ICashSalesModuleRepositoryAsync<Company> _companyRepository;
        private readonly ICashSalesModuleRepositoryAsync<ChartOfAccount> _coaRepository;
        private readonly ICashSalesModuleRepositoryAsync<CompanyUser> _compUserRepo;
        private readonly ICashSalesModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUserDetailRepo;

        public CashSalesService(ICashSalesModuleRepositoryAsync<CashSale> cashSaleRepository, ICashSalesModuleRepositoryAsync<Company> companyRepository, ICashSalesModuleRepositoryAsync<ChartOfAccount> coaRepository, ICashSalesModuleRepositoryAsync<CompanyUser> compUserRepo, ICashSalesModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepo)
            : base(cashSaleRepository)
        {
            _cashSaleRepository = cashSaleRepository;
            _companyRepository = companyRepository;
            _coaRepository = coaRepository;
            _compUserRepo = compUserRepo;
            _compUserDetailRepo = compUserDetailRepo;
        }

        public List<CashSale> GetAllCashSale(long companyId)
        {
            return _cashSaleRepository.Queryable().Where(c => c.CompanyId == companyId).AsEnumerable().OrderByDescending(c => c.CreatedDate).ToList();
        }

        public List<string> GetAutoNumber(long companyId)
        {
            return _cashSaleRepository.Queryable().Where(x => x.CompanyId == companyId).Select(x => x.CashSaleNumber).ToList();
        }
        public CashSale GetCashSaleDocNo(Guid id, string docNo, long companyId)
        {
            return _cashSaleRepository.Query(x => x.Id != id && x.DocNo == docNo && x.CompanyId == companyId && x.DocumentState != CashSaleStatus.Void).Select().FirstOrDefault();
        }

        public CashSale CreateCashSales(long companyId, Guid id)
        {
            return _cashSaleRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select().FirstOrDefault();
        }

        public CashSale GetAllCashSalesLUs(Guid cashsaleId, long companyId)
        {
            return _cashSaleRepository.Query(c => c.Id == cashsaleId && c.CompanyId == companyId).Include(c => c.CashSaleDetails).Select().FirstOrDefault();
        }

        public IQueryable<CashSaleModelK> GetAllCashSalesK(string username, long companyId)
        {
            IQueryable<BeanEntity> beanEntityRepository = _cashSaleRepository.GetRepository<BeanEntity>().Queryable();
            IQueryable<CashSale> cashSaleRepository = _cashSaleRepository.Queryable();
            IQueryable<CashSaleModelK> cashSaleModelK = from b in cashSaleRepository
                                                        join e in beanEntityRepository on b.EntityId equals e.Id into grp
                                                        from d in grp.DefaultIfEmpty()
                                                        join company in _companyRepository.Queryable() on b.ServiceCompanyId equals company.Id
                                                        join coa in _coaRepository.Queryable() on b.COAId equals coa.Id

                                                        join compUser in _compUserRepo.Queryable() on company.ParentId equals compUser.CompanyId // newly added for svcentity permissions
                                                        join CUD in _compUserDetailRepo.Queryable() on compUser.Id equals CUD.CompanyUserId where company.Id == CUD.ServiceEntityId
                                                        //where (b.EntityId == e.Id)
                                                        //where b.CompanyId == companyId
                                                        where b.CompanyId == companyId && compUser.Username == username// newly added for svcentity permissions
                                                        select new CashSaleModelK()
                                                        {
                                                            Id = b.Id,
                                                            CompanyId = b.CompanyId,
                                                            DocNo = b.DocNo,
                                                            EntityName = d.Name,
                                                            EntityId = d.Id,
                                                            DocumentState = b.DocumentState,
                                                            DocDate = b.DocDate,
                                                            GrandTotal = (double)(b.GrandTotal),
                                                            CreatedDate = b.CreatedDate,
                                                            BaseAmount = Math.Round((double)(b.GrandTotal * b.ExchangeRate), 2),
                                                            DocCurrency = b.DocCurrency,
                                                            //ExCurrency = b.ExCurrency,
                                                            ModeOfReceipt = b.ModeOfReceipt,
                                                            ReceiptrefNo = b.ReceiptrefNo,
                                                            // CashSaleNumber = b.CashSaleNumber,
                                                            // DocDescription = b.DocDescription,
                                                            PONo = b.PONo,
                                                            ExchangeRate = (b.ExchangeRate).ToString(),
                                                            //NoSupportingDocument = b.NoSupportingDocs,
                                                            CashBankAccount = coa.Name,
                                                            ServiceCompanyName = company.ShortName,
                                                            ServiceCompanyId = company.Id,
                                                            BankClearingDate = b.BankClearingDate,
                                                            UserCreated = b.UserCreated,
                                                            ModifiedDate = b.ModifiedDate,
                                                            ModifiedBy = b.ModifiedBy,
                                                            ScreenName = "Cash Sale",
                                                            IsLocked = b.IsLocked
                                                        };
            return cashSaleModelK.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }

        public CashSale GetCashSaleByIdAndCompanyId(Guid id, long companyid)
        {
            return _cashSaleRepository.Query(e => e.Id == id && e.CompanyId == companyid).Include(a => a.CashSaleDetails).Select().FirstOrDefault();
        }

        public CashSale GetCashSaleLU(long companyId, Guid Id)
        {
            return _cashSaleRepository.Query(a => a.CompanyId == companyId && a.Id == Id).Include(z => z.CashSaleDetails).Select().FirstOrDefault();
        }

        public CashSale GetDocTypeAndCompanyid(string DocType, long companyId)
        {
            return _cashSaleRepository.Query(a => a.DocType == DocType && a.CompanyId == companyId && a.DocumentState != CashSaleStatus.Void).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }

        public CashSale DuplicateCashsale(string DocNo, string docType, long companyId)
        {
            return _cashSaleRepository.Query(a => a.DocNo == DocNo && a.DocType == docType && a.CompanyId == companyId).Select().FirstOrDefault();
        }
        public CashSale GetCashsaleByCompanyId(long companyId)
        {
            return _cashSaleRepository.Query(a => a.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public bool IsVoid(long companyId, Guid id)
        {
            return _cashSaleRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => (a.DocumentState == InvoiceState.Void || a.DocumentState == CashSaleStatus.Cleared)).FirstOrDefault() == true;
        }
    }
}
