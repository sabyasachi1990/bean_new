using Service.Pattern;
using System.Collections.Generic;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class TermsOfPaymentService : Service<TermsOfPayment>, ITermsOfPaymentService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<TermsOfPayment> _termsOfPaymentRepository;

        public TermsOfPaymentService(IJournalVoucherModuleRepositoryAsync<TermsOfPayment> termsOfPaymentRepository)
			: base(termsOfPaymentRepository)
        {
			_termsOfPaymentRepository = termsOfPaymentRepository;
        }
        public TermsOfPayment GetById(long? Id)
        {
            return _termsOfPaymentRepository.Query(x => x.Id == Id).Select().FirstOrDefault();
        }
        public List<TermsOfPayment> TOPLUEdit(long id, long companyId)
        {
            return _termsOfPaymentRepository.Query(a => (a.Status == RecordStatusEnum.Active || a.Id == id) && a.CompanyId == companyId).Select().ToList();
        }
        public List<TermsOfPayment> TOPLUNew(long companyId)
        {
            return _termsOfPaymentRepository.Query(a => a.Status == RecordStatusEnum.Active && a.CompanyId == companyId).Select().ToList();
        }
    }
}
