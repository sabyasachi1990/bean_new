using AppsWorld.OpeningBalancesModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.OpeningBalancesModule.RepositoryPattern;
using Service.Pattern;
namespace AppsWorld.OpeningBalancesModule.Service
{
    public class BillService : Service<Bill>, IBillService
    {
        private readonly IOpeningBalancesModuleRepositoryAsync<Bill> _billrepository;
        private readonly IOpeningBalancesModuleRepositoryAsync<Invoice> _invoicerepository;
        private readonly IOpeningBalancesModuleRepositoryAsync<CreditMemo> _creditMemoRepository;

        public BillService(IOpeningBalancesModuleRepositoryAsync<Bill> itemRepository, IOpeningBalancesModuleRepositoryAsync<Invoice> invoicerepository, IOpeningBalancesModuleRepositoryAsync<CreditMemo> creditMemoRepository)
            : base(itemRepository)
        {
            _billrepository = itemRepository;
            _invoicerepository = invoicerepository;
            _creditMemoRepository = creditMemoRepository;
        }

        public IDictionary<string, Guid> GetAllInvoiceDocNo(long companyId)
        {
            return _invoicerepository.Queryable().Where(c => c.CompanyId == companyId && c.DocumentState != "Void" && c.DocType == "Invoice").Select(a => new { a.DocNo, a.Id }).ToDictionary(t => t.DocNo, t => t.Id);
        }

        public IDictionary<Guid, string> GetAllInvoiceDocNos(long companyId)
        {
            return _invoicerepository.Queryable().Where(c => c.CompanyId == companyId && c.DocumentState != "Void" && c.DocType == "Invoice").Select(a => new { a.Id, a.DocNo }).ToDictionary(t => t.Id, t => t.DocNo);
        }

        public List<Bill> GetAllBillDocNo(long companyId)
        {
            return _billrepository.Queryable().Where(c => c.CompanyId == companyId && c.DocumentState != "Void").ToList();
        }
        public IDictionary<Guid, string> GetAllCreditNoteDocNo(long companyId)
        {
            return _invoicerepository.Queryable().Where(c => c.CompanyId == companyId && c.DocumentState != "Void" && c.DocType == "Credit Note").Select(c => new { Id = c.Id, DocNo = c.DocNo }).ToDictionary(c => c.Id, DocNo => DocNo.DocNo);
        }
        public IDictionary<Guid, string> GetAllCreditMemoDocNo(long companyId)
        {
            return _creditMemoRepository.Queryable().Where(c => c.CompanyId == companyId && c.DocumentState != "Void" && c.DocType == "Credit Memo").Select(c => new { Id = c.Id, DocNo = c.DocNo }).ToDictionary(Id => Id.Id, DocNo => DocNo.DocNo);
        }
        public List<CreditMemo> GetlistOfCreditMemoDocNos(long companyId)
        {
            return _creditMemoRepository.Queryable().Where(c => c.CompanyId == companyId && c.DocumentState != "Void" && c.DocType == "Credit Memo").ToList();
        }
    }
}
