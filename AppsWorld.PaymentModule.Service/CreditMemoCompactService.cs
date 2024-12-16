using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Infra;
using AppsWorld.PaymentModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Service
{
    public class CreditMemoCompactService : Service<CreditMemoCompact>, ICreditMemoCompactService
    {
        private readonly IPaymentModuleRepositoryAsync<CreditMemoCompact> _creditMemoRepository;
        private readonly IPaymentModuleRepositoryAsync<CreditMemoApplicationCompact> _creditMemoAppRepository;

        public CreditMemoCompactService(IPaymentModuleRepositoryAsync<CreditMemoCompact> creditMemoRepository, IPaymentModuleRepositoryAsync<CreditMemoApplicationCompact> creditMemoAppRepository) : base(creditMemoRepository)
        {
            this._creditMemoRepository = creditMemoRepository;
            this._creditMemoAppRepository = creditMemoAppRepository;
        }
        public List<CreditMemoCompact> GetListOfCreditMemos(long companyId, string docType, Guid entityId, List<Guid> lstDocumentIds)
        {
            return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.DocType == docType && a.EntityId == entityId && lstDocumentIds.Contains(a.Id)).Select().ToList();
        }
        public List<CreditMemoCompact> GetListOfCreditMemoWithOutInter(long companyId, long serviceCompanyId, string currency, Guid entityId, DateTime docDate)
        {
            if (currency != null)
                return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.DocCurrency == currency && a.EntityId == entityId && a.DocDate <= docDate && a.DocumentState != "Void" && a.DocumentState != "Fully Applied").Select().ToList();
            else
                return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.EntityId == entityId && a.DocDate <= docDate && a.DocumentState != "Void" && a.DocumentState != "Fully Applied").Select().ToList();
        }
        public List<CreditMemoCompact> GetListOfCreditMemoWithInter(long companyId, string currency, Guid entityId, DateTime docDate)
        {
            if (currency != null)
                return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.DocCurrency == currency && a.EntityId == entityId && a.DocDate <= docDate && a.DocumentState != "Void" && a.DocumentState != "Fully Applied").Select().ToList();
            else
                return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.EntityId == entityId && a.DocDate <= docDate && a.DocumentState != "Void" && a.DocumentState != "Fully Applied").Select().ToList();
        }
        public List<CreditMemoCompact> GetListOfCreditMemoCompacts(long companyId, List<Guid> ids)
        {
            return _creditMemoRepository.Query(a => a.CompanyId == companyId && ids.Contains(a.Id)).Select().ToList();
        }
        public List<Guid?> GetListOfCreditMemoApps(long companyId, List<Guid> lstDocIds)
        {
            return _creditMemoAppRepository.Query(a => a.CompanyId == companyId && lstDocIds.Contains(a.Id)).Select(a => a.DocumentId).ToList();
        }

        public List<CreditMemoCompact> GetAllCMsByDocId(List<Guid> docIds, string docCurrency, Guid entityId, long companyId)
        {
            return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.DocCurrency == docCurrency && a.EntityId == entityId && docIds.Contains(a.Id)).Select().ToList();
        }
        public List<CreditMemoApplicationCompact> GetListOfCreditMemoAppsByCMIds(List<Guid> creditMemoIds, long companyId)
        {
            return _creditMemoAppRepository.Query(a => a.CompanyId == companyId && creditMemoIds.Contains(a.CreditMemoId) && a.DocumentId != null && a.Status == Framework.RecordStatusEnum.Active).Select().ToList();
        }
        public List<CreditMemoCompact> GetAllCMByDocId(List<Guid> ids, long companyId)
        {
            return _creditMemoRepository.Query(c => ids.Contains(c.Id) && c.CompanyId == companyId).Select().ToList();
        }
        public CreditMemoApplicationCompact GetCMApplicationByDocId(Guid detailId)
        {
            return _creditMemoAppRepository.Query(c => c.DocumentId == detailId).Select().FirstOrDefault();
        }
        public decimal? GetExchangeRateByDocId(long companyId, Guid documentdId)
        {
            return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.Id == documentdId).Select(a => a.ExchangeRate).FirstOrDefault();
        }
        public List<string> GetByCreditMemoId(Guid entityId, long companyId)
        {
            return _creditMemoRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Credit Memo" && (c.DocumentState == CreditMemoState.Not_Applied || c.DocumentState == CreditMemoState.Partial_Applied)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetByStateandCreditMemoEntity(Guid entityId, long companyId)
        {
            return _creditMemoRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Credit Memo" && (c.DocumentState != InvoiceState.Void || c.DocumentState != InvoiceState.Deleted)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetAllCreditMemoByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _creditMemoRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Credit Memo" && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState == CreditMemoState.Not_Applied || c.DocumentState == CreditMemoState.Partial_Applied)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetAllCreditMemoEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _creditMemoRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocType == "Credit Memo" && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState != InvoiceState.Void || c.DocumentState != InvoiceState.Deleted)).Select(c => c.DocCurrency).ToList();
        }
    }
}
