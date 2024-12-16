using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.RevaluationModule.Service
{
    public class BillService:Service<Bill>,IBillService
    {
        private readonly IRevaluationModuleRepositoryAsync<Bill> _billRepository;
        public BillService(IRevaluationModuleRepositoryAsync<Bill> billRepository)
            : base(billRepository)
        {
            _billRepository = billRepository;
        }
        public List<Bill> lstBills(long companyId)
        {
            return _billRepository.Query(c => c.CompanyId == companyId && (c.DocumentState == DebitNoteStates.NotPaid || c.DocumentState == DebitNoteStates.PartialPaid)).Select().ToList();
        }

    }
}
