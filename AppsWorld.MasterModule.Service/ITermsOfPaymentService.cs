using AppsWorld.CommonModule.Infra;
using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface ITermsOfPaymentService : IService<TermsOfPayment>
    {
        Task<IEnumerable<TermsOfPayment>> GetAllTermsOfPayment(long CompanyId, long CTOPId);
        Task<IEnumerable<TermsOfPayment>> GetAllTermsOfPayments(long CompanyId, long VTOPId);
        TermsOfPayment GetTermsById(long CustTOPId);
        LookUpCategory<string> GetByTOPCustomers(long CompanyId);
        LookUpCategory<string> GetByTOPVendors(long CompanyId);
        TermsOfPayment GetAllTermsOfPaymentsAllPaymentByNameAndCompanyId(long? TermsOfPayment, long CompanyId);
    }
    
}
