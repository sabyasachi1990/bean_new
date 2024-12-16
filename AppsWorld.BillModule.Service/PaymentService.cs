using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Service
{
    public class PaymentService : Service<Payment>, IPaymentService
    {
        private readonly IBillModuleRepositoryAsync<Payment> _paymentRepository;
        public PaymentService(IBillModuleRepositoryAsync<Payment> paymentRepository)
            : base(paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public Payment GetDocNo(string docNo, long companyId)
        {
            return _paymentRepository.Query(a => a.CompanyId == companyId && a.DocNo == docNo).Select().FirstOrDefault();
        }

        public Payment GetPaymentByComapnyId(long companyId, string docType)
        {
            return _paymentRepository.Query(c => c.CompanyId == companyId && docType == "Payroll" ? c.DocSubType == "Payroll" : c.DocSubType == "General" && c.DocumentState != "Void").Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }

        public Payment GetPaymentById(Guid pymentid)
        {
            return _paymentRepository.Query(c => c.Id == pymentid).Select().FirstOrDefault();
        }
        public Payment GetLastPayement(long companyId, string docType)
        {
            return _paymentRepository.Query(c => c.CompanyId == companyId && c.DocSubType == docType).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
    }
}
