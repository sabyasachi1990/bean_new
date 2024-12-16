using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;

namespace AppsWorld.ReceiptModule.Service
{
    public class TermsOfPaymentService : Service<TermsOfPayment>, ITermsOfPaymentService
    {
        private readonly IReceiptModuleRepositoryAsync<TermsOfPayment> _termsOfPaymentRepository;

        public TermsOfPaymentService(IReceiptModuleRepositoryAsync<TermsOfPayment> termsOfPaymentRepository)
            : base(termsOfPaymentRepository)
        {
            _termsOfPaymentRepository = termsOfPaymentRepository;
        }
        public long GetCreditTermById(long companyId)
        {
            return _termsOfPaymentRepository.Queryable().Where(s => s.CompanyId == companyId && s.Name == "Credit - 0").Select(s => s.Id).FirstOrDefault();
        }

    }
}
