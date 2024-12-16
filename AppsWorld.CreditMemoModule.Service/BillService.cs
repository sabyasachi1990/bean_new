using AppsWorld.CommonModule.Infra;
using AppsWorld.CreditMemoModule.Entities;
using AppsWorld.CreditMemoModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Service
{
    public class BillService : Service<Bill>, IBillService
    {
        private readonly ICreditMemoModuleRepositoryAsync<Bill> _billRepository;
        public BillService(ICreditMemoModuleRepositoryAsync<Bill> billRepository) : base(billRepository)
        {
            _billRepository = billRepository;
        }
        public Bill GetCrediMemoByDocId(Guid id)
        {
            return _billRepository.Query(x => x.Id == id).Include(c => c.BillDetails).Select().FirstOrDefault();
        }
        public List<Bill> GetAllCreditMemoById(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime date)
        {
            return _billRepository.Query(c => c.CompanyId == companyId && c.EntityId == EntityId && c.PostingDate <= date && c.DocCurrency == DocCurrency && c.ServiceCompanyId == ServiceCompanyId && (c.DocumentState == InvoiceStates.NotPaid || c.DocumentState == InvoiceStates.PartialPaid) && c.DocSubType != DocTypeConstants.PayrollBill).Select().ToList();
        }
        public Bill GetCrediMemoByEntity(Guid id)
        {
            return _billRepository.Query(x => x.EntityId == id).Select().FirstOrDefault();
        }
        public List<Bill> GetAllBills(List<Guid?> ids, long companyId)
        {
            return _billRepository.Query(x => ids.Contains(x.Id)).Select().ToList();
        }
        public List<decimal?> GetBillStatusByIds(List<Guid> Ids)
        {
            return _billRepository.Queryable().Where(c => Ids.Contains(c.Id)).Select(c => c.BalanceAmount).ToList();
        }
        public List<Bill> GetAllBillsByDocIds(List<Guid> ids, long companyId)
        {
            return _billRepository.Query(x => ids.Contains(x.Id)).Select().ToList();
        }
    }
}


