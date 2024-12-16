using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Service
{
    public interface ITermsOfPaymentService:IService<TermsOfPayment>
    {
        TermsOfPayment GetTermsOfPaymentById(long? id);
        List<TermsOfPayment> GetAllTOPByCid(long companyId);
    }
}
