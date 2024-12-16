using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;

using AppsWorld.ReceiptModule.Entities.Models.V2.Receipt;
using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern.V2;
using Service.Pattern;
using System;
using System.Linq;

namespace AppsWorld.ReceiptModule.Service.V2.Receipt.K_Service
{
    public class ReceiptKService : Service<ReceiptK>, IReceiptKService
    {
        private readonly IReceiptKModuleRepositoryAsync<ReceiptK> _receiptRepository;
        private readonly IReceiptKModuleRepositoryAsync<BeanEntityCompact> _beanEntityRepository;
        private readonly IReceiptKModuleRepositoryAsync<CompanyCompact> _companyRepository;
        private readonly IReceiptKModuleRepositoryAsync<CompanyUserCompact> _companyUserRepository;
        private readonly IReceiptKModuleRepositoryAsync<ChartOfAccountCompact> _chartOfAccountRepository;
        public ReceiptKService(IReceiptKModuleRepositoryAsync<ReceiptK> receiptKRepository, IReceiptKModuleRepositoryAsync<BeanEntityCompact> beanEntityRepository, IReceiptKModuleRepositoryAsync<CompanyCompact> companyRepository, IReceiptKModuleRepositoryAsync<CompanyUserCompact> companyUserRepository, IReceiptKModuleRepositoryAsync<ChartOfAccountCompact> chartOfAccountRepository)
            : base(receiptKRepository)
        {
            this._receiptRepository = receiptKRepository;
            this._beanEntityRepository = beanEntityRepository;
            this._companyRepository = companyRepository;
            this._companyUserRepository = companyUserRepository;
            this._chartOfAccountRepository = chartOfAccountRepository;
        }

        public IQueryable<ReceiptModelK> GetAllReceiptsK(string username, long companyId)
        {
            IQueryable<BeanEntityCompact> beanEntityRepository = _receiptRepository.GetRepository<BeanEntityCompact>().Queryable();
            IQueryable<ReceiptK> receiptRepository = _receiptRepository.Queryable();
            IQueryable<ReceiptModelK> receiptModelKDetails = from b in receiptRepository
                                                             join e in beanEntityRepository on b.EntityId equals e.Id
                                                             join c in _chartOfAccountRepository.Queryable() on b.COAId equals c.Id
                                                             join company in _companyRepository.Queryable() on b.ServiceCompanyId equals company.Id
                                                             join compUser in _companyUserRepository.Queryable() on company.ParentId equals compUser.CompanyId
                                                            
                                                             where b.CompanyId == companyId
                                                             && (compUser.ServiceEntities != null ? compUser.ServiceEntities.Contains(company.Id.ToString()) : true) && compUser.Username == username
                                                             select new ReceiptModelK()
                                                             {
                                                                 Id = b.Id,
                                                                 CompanyId = b.CompanyId,
                                                                 DocNo = b.DocNo,
                                                                 DocDate = b.DocDate,
                                                                 EntityName = e.Name,
                                                                 EntityId= e.Id,
                                                                 DocumentState = b.DocumentState,
                                                                 BankReceiptAmmount = (double)(b.BankReceiptAmmount),
                                                                 BankReceiptAmmountCurrency = b.BankReceiptAmmountCurrency,
                                                                 ExchangeRate = (b.ExchangeRate).ToString(),
                                                                 ReceiptRefNo = b.ReceiptRefNo,
                                                                 DocCurrency = b.DocCurrency,
                                                                 ReceiptApplicationAmmount = (double)(b.ReceiptApplicationAmmount),
                                                                 ServiceCompanyName = company.ShortName,
                                                                 ModeOfReceipt = b.ModeOfReceipt,
                                                                 CreatedDate = b.CreatedDate,
                                                                 UserCreated = b.UserCreated,
                                                                 ModifiedBy = b.ModifiedBy,
                                                                 CashBankAccount = c.Name,
                                                                 ModifiedDate = b.ModifiedDate,
                                                                 BankClearingDate = b.BankClearingDate,
                                                                 ScreenName = "Receipt"
                                                             };
            return receiptModelKDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }

    }
}
