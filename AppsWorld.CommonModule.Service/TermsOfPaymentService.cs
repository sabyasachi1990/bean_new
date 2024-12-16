using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Service
{
    public class TermsOfPaymentService : Service<TermsOfPayment>, ITermsOfPaymentService
    {
        private readonly ICommonModuleRepositoryAsync<TermsOfPayment> _termsOfPaymentRepository;
        public TermsOfPaymentService(ICommonModuleRepositoryAsync<TermsOfPayment> termsOfPaymentRepository)
            : base(termsOfPaymentRepository)
        {
            _termsOfPaymentRepository = termsOfPaymentRepository;
        }

        public TermsOfPayment GetTermsOfPaymentById(long? id)
        {
            return _termsOfPaymentRepository.Query(a => a.Id == id).Select().FirstOrDefault();
        }
        public List<TermsOfPayment> GetAllTOPByCid(long companyId)
        {
            return _termsOfPaymentRepository.Query(a => a.CompanyId == companyId).Select().ToList();
        }
    }
}
