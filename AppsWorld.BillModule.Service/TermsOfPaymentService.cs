using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.BillModule.Service
{
    public class TermsOfPaymentService : Service<TermsOfPayment>, ITermsOfPaymentService
    {
        private readonly IBillModuleRepositoryAsync<TermsOfPayment> _termsOfPaymentRepository;

        public TermsOfPaymentService(IBillModuleRepositoryAsync<TermsOfPayment> termsOfPaymentRepository)
			: base(termsOfPaymentRepository)
        {
			_termsOfPaymentRepository = termsOfPaymentRepository;
        }

        public TermsOfPayment GetById(long Id)
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
		public List<TermsOfPayment> TOPVendorLUEdit(long id, long companyId)
		{
			return _termsOfPaymentRepository.Query(a => (a.Status == RecordStatusEnum.Active || a.Id == id) && a.CompanyId == companyId && a.IsVendor == true).Select().ToList();
		}
		public List<TermsOfPayment> TOPVendorLUNew(long companyId)
		{
			return _termsOfPaymentRepository.Query(a => a.Status == RecordStatusEnum.Active && a.CompanyId == companyId && a.IsVendor == true).Select().ToList();
		}


        public TermsOfPayment GetTermsOfPayments(int days, long? comapnYId)
        {
            return _termsOfPaymentRepository.Query(x=>x.CompanyId == comapnYId && x.TOPValue == days && x.IsVendor== true).Select().FirstOrDefault();
        }

    }
}
