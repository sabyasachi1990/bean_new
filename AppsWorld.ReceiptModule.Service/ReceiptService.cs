using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;
using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.Infra;
using Ziraff.FrameWork.Logging;
using System.Data.Entity;

namespace AppsWorld.ReceiptModule.Service
{
    public class ReceiptService : Service<Receipt>, IReceiptService
    {
        private readonly IReceiptModuleRepositoryAsync<Receipt> _receiptRepository;
        private readonly IReceiptModuleRepositoryAsync<Company> _companyRepository;
        private readonly IReceiptModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;
        private readonly IReceiptModuleRepositoryAsync<CompanyUser> _compUserRepo;
        private readonly IReceiptModuleRepositoryAsync<BillCompact> _billCompactRepo;
        private readonly IReceiptModuleRepositoryAsync<CreditMemoCompact> _creditMemoCompactRepo;
        private readonly IReceiptModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUserDetailRepo;

        public ReceiptService(IReceiptModuleRepositoryAsync<Receipt> receiptRepository, IReceiptModuleRepositoryAsync<Company> companyRepository, IReceiptModuleRepositoryAsync<ChartOfAccount> chartOfAccountRepository, IReceiptModuleRepositoryAsync<CompanyUser> compUserRepo, IReceiptModuleRepositoryAsync<BillCompact> billCompactRepo, IReceiptModuleRepositoryAsync<CreditMemoCompact> creditMemoCompactRepo,IReceiptModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepo)
            : base(receiptRepository)
        {
            this._receiptRepository = receiptRepository;
            this._companyRepository = companyRepository;
            this._chartOfAccountRepository = chartOfAccountRepository;
            _compUserRepo = compUserRepo;
            _billCompactRepo = billCompactRepo;
            _creditMemoCompactRepo = creditMemoCompactRepo;
            _compUserDetailRepo = compUserDetailRepo;
        }
        public List<Receipt> GetAllReceiptModel(long companyId)
        {
            return _receiptRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }
        public Receipt GetReceipt(Guid id, long companyId)
        {
            return _receiptRepository.Query(c => c.Id == id && c.CompanyId == companyId).Include(c => c.ReceiptDetails).Include(x => x.ReceiptBalancingItems).Select().FirstOrDefault();
        }
        public Receipt GetReceipts(Guid id, long companyId)
        {
            return _receiptRepository.Query(c => c.Id == id && c.CompanyId == companyId).Include(c => c.ReceiptDetails).Select().FirstOrDefault();
        }
        public Receipt CreateReceipt(long companyId)
        {
            return _receiptRepository.Query(c => c.CompanyId == companyId && c.DocumentState != "Void").Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public DateTime CreateReceiptNew(long companyId)
        {
            return _receiptRepository.Queryable().Where(c => c.CompanyId == companyId && c.DocumentState != "Void").OrderByDescending(c => c.CreatedDate).Select(c=>c.DocDate).FirstOrDefault();
        }

        public Receipt GetDocNo(string docNo, long companyId)
        {
            return _receiptRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public Receipt CheckDocNo(Guid id, string docNo, long companyId)
        {
            return _receiptRepository.Query(c => c.Id != id && c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public Receipt CheckReceiptById(Guid id)
        {
            return
                _receiptRepository.Query(c => c.Id == id)
                    .Include(c => c.ReceiptDetails)
                    .Include(c => c.ReceiptBalancingItems)
                    //.Include(c => c.ReceiptGSTDetails)
                    .Select()
                    .FirstOrDefault();
        }
        public void UpdateReceipt(Receipt receipt)
        {
            _receiptRepository.Update(receipt);
        }
        public void InsertReceipt(Receipt receipt)
        {
            _receiptRepository.Insert(receipt);
        }

        public IQueryable<ReceiptModelK> GetAllReceiptsK(string userName, long companyId)
        {
            try
            {
                LoggingHelper.LogMessage(ReceiptLoggingValidation.ReceiptService, ReceiptLoggingValidation.Log_Receipts_GetReceiptsK_Request_Message);
                IQueryable<BeanEntity> beanEntityRepository =  _receiptRepository.GetRepository<BeanEntity>().Queryable().Where(x=>x.CompanyId==companyId);
                IQueryable<Receipt> receiptRepository = _receiptRepository.Queryable().Where(c=>c.CompanyId==companyId);

                IQueryable<ReceiptModelK> receiptModelKDetails = from b in receiptRepository
                                                                 join e in beanEntityRepository on b.EntityId equals e.Id
                                                                 join c in  _chartOfAccountRepository.Queryable().AsNoTracking() on b.COAId equals c.Id
                                                                 join company in  _companyRepository.Queryable().AsNoTracking() on b.ServiceCompanyId equals company.Id
                                                                 join compUser in  _compUserRepo.Queryable().AsNoTracking() on company.ParentId equals compUser.CompanyId
                                                                 where b.CompanyId == companyId && compUser.Username == userName
                                                                 select new ReceiptModelK()
                                                                 {
                                                                     Id = b.Id,
                                                                     CompanyId = b.CompanyId,
                                                                     DocNo = b.DocNo,
                                                                     DocDate = b.DocDate,
                                                                     EntityName = e.Name,
                                                                     EntityId = e.Id,
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
                                                                     ScreenName = "Receipt",
                                                                     IsLocked = b.IsLocked
                                                                 };
                return receiptModelKDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();
               
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(ReceiptLoggingValidation.ReceiptService, ex, ex.Message);
                LoggingHelper.LogMessage(ReceiptLoggingValidation.ReceiptService, ReceiptLoggingValidation.Log_Receipts_GetReceiptsK_Exeception_Message);
                throw;
            }
        }

        public Receipt GetReceiptById(Guid receiptId, Guid entityId, long companyId)
        {
            return _receiptRepository.Query(a => a.CompanyId == companyId && a.Id == receiptId && a.EntityId == entityId).Select().FirstOrDefault();
        }


        public List<string> GetAllBillByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _billCompactRepo.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Bill" && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetAllBillEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _billCompactRepo.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Bill" && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState != InvoiceState.Void || c.DocumentState != InvoiceState.Deleted)).Select(c => c.DocCurrency).ToList();
        }



        public List<string> GetAllCreditMemoByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _creditMemoCompactRepo.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Credit Memo" && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetAllCreditMemoEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _creditMemoCompactRepo.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Credit Memo" && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState != InvoiceState.Void || c.DocumentState != InvoiceState.Deleted)).Select(c => c.DocCurrency).ToList();
        }


        public List<string> GetByBillId(Guid entityId, long companyId)
        {
            return _billCompactRepo.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Bill" && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetByStateandBillEntity(Guid entityId, long companyId)
        {
            return _billCompactRepo.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Bill" && (c.DocumentState != InvoiceState.Void || c.DocumentState != InvoiceState.Deleted)).Select(c => c.DocCurrency).ToList();
        }

        public List<string> GetByCreditMemoId(Guid entityId, long companyId)
        {
            return _creditMemoCompactRepo.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Credit Memo" && (c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetByStateandCreditMemoEntity(Guid entityId, long companyId)
        {
            return _creditMemoCompactRepo.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Credit Memo" && (c.DocumentState != InvoiceState.Void || c.DocumentState != InvoiceState.Deleted)).Select(c => c.DocCurrency).ToList();
        }
        public bool IsVoid(long companyId, Guid id)
        {
            return _receiptRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => (a.DocumentState == ReceiptState.Void || a.DocumentState == ReceiptState.Cleared)).FirstOrDefault();
        }
        public bool IsVoidNew(long companyId, Guid id)
        {
            return _receiptRepository.Queryable().Where(a => a.CompanyId == companyId && a.Id == id).Select(a => (a.DocumentState == ReceiptState.Void || a.DocumentState == ReceiptState.Cleared)).FirstOrDefault() == true;
        }
        public async Task<Receipt> CreateReceiptAsync(long companyId)
        {
            return await Task.Run(()=> _receiptRepository.Query(c => c.CompanyId == companyId && c.DocumentState != "Void").Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault());
        }
        public async Task<Receipt> GetReceiptAsync(Guid id, long companyId)
        {
            return await Task.Run(()=> _receiptRepository.Query(c => c.Id == id && c.CompanyId == companyId).Include(c => c.ReceiptDetails).Include(x => x.ReceiptBalancingItems).Select().FirstOrDefault());
        }
    }
}
