using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class CashSaleService : Service<CashSale>, ICashSaleService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<CashSale> _cashSalerepository;
        private readonly IJournalVoucherModuleRepositoryAsync<BankReconciliation> _bankReconRepository;

        public CashSaleService(IJournalVoucherModuleRepositoryAsync<CashSale> cashSalerepository, IJournalVoucherModuleRepositoryAsync<BankReconciliation> bankReconRepository)
             : base(cashSalerepository)
        {
            _cashSalerepository = cashSalerepository;
            this._bankReconRepository = bankReconRepository;
        }
        public CashSale GetCashSaleDetail(Guid cashSaleId)
        {
            return _cashSalerepository.Query(x => x.Id == cashSaleId).Select().SingleOrDefault();
        }
        public CashSale GetCashSaleId(Guid? Id)
        {
            return _cashSalerepository.Query(x => x.Id == Id).Select().FirstOrDefault();
        }
        public BankReconciliation GetBRByCOAID(long? coaId, long? serviceCompanyId, long? companyId)
        {
            return _bankReconRepository.Query(c => c.COAId == coaId && c.ServiceCompanyId == serviceCompanyId && c.CompanyId == companyId && c.State == "Reconciled" && c.IsReRunBR != true /*&& c.BankReconciliationDate > docDate*/).Select().OrderByDescending(x => x.BankReconciliationDate).FirstOrDefault();
        }
    }
}
