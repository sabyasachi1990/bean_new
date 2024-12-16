using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class TermsOfPaymentService : Service<TermsOfPayment>, ITermsOfPaymentService
    {
        private readonly IMasterModuleRepositoryAsync<TermsOfPayment> _termsOfPaymentRepository;
        public TermsOfPaymentService(IMasterModuleRepositoryAsync<TermsOfPayment> termsOfPaymentRepository)
            : base(termsOfPaymentRepository)
        {
            _termsOfPaymentRepository = termsOfPaymentRepository;
        }

        public async Task<IEnumerable<TermsOfPayment>> GetAllTermsOfPayment(long CompanyId, long CTOPId)
        {
            return await Task.Run(()=> _termsOfPaymentRepository.Query(a => a.CompanyId == CompanyId && (a.Status == RecordStatusEnum.Active || a.Id == CTOPId) && a.IsCustomer == true).Select().ToList());
        }
        public  async Task<IEnumerable<TermsOfPayment>> GetAllTermsOfPayments(long CompanyId, long VTOPId)
        {
            return await Task.Run(()=> _termsOfPaymentRepository.Query(a => a.CompanyId == CompanyId && (a.Status == RecordStatusEnum.Active || a.Id == VTOPId) && a.IsVendor == true).Select().ToList());
        }       

        public TermsOfPayment GetTermsById(long CustTOPId)
        {
            return _termsOfPaymentRepository.Queryable().Where(c => c.Id == CustTOPId).FirstOrDefault();

        }
        public LookUpCategory<string> GetByTOPCustomers(long CompanyId)
        {
            // return _termsOfPaymentRepository.Query(x => x.CompanyId == CompanyId).Select().ToList();

            var TOPOne = _termsOfPaymentRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active && a.IsCustomer == true).FirstOrDefault();

            var lookUpTOP = new LookUpCategory<string>();

            var TOPAll = _termsOfPaymentRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active && a.IsCustomer == true).OrderBy(c => c.TOPValue);

            if (TOPOne != null)
            {
                //lookUpTOP.CategoryName = CategoryCode;
                lookUpTOP.Lookups = TOPAll.Select(x => new LookUp<string>()
                {
                    Id = x.Id,
                    Code = x.Name,
                    Name = x.Name,
                    RecOrder = x.RecOrder,
                    TOPValue = x.TOPValue
                }).AsEnumerable().OrderBy(c => c.TOPValue).ToList();
            }
            return lookUpTOP;

        }
        public LookUpCategory<string> GetByTOPVendors(long CompanyId)
        {
            var TOPOne = _termsOfPaymentRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active && a.IsVendor == true).FirstOrDefault();
            var lookUpTOP = new LookUpCategory<string>();
            var TOPAll = _termsOfPaymentRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active && a.IsVendor == true).OrderBy(c => c.TOPValue);
            if (TOPOne != null)
            {
                //lookUpTOP.CategoryName = CategoryCode;
                lookUpTOP.Lookups = TOPAll.Select(x => new LookUp<string>()
                {
                    Id = x.Id,
                    Code = x.Name,
                    Name = x.Name,
                    RecOrder = x.RecOrder,
                    TOPValue = x.TOPValue
                }).AsEnumerable().OrderBy(c => c.TOPValue).ToList();
            }
            return lookUpTOP;
        }
        public TermsOfPayment GetAllTermsOfPaymentsAllPaymentByNameAndCompanyId(long? TermsOfPayment, long CompanyId)
        {
            return _termsOfPaymentRepository.Queryable().Where(c => c.Id == TermsOfPayment && c.CompanyId == CompanyId).FirstOrDefault();
        }



    }
}
