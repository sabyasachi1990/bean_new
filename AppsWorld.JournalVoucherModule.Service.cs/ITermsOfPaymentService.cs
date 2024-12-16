using System.Collections.Generic;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface ITermsOfPaymentService : IService<TermsOfPayment>
    {
        TermsOfPayment GetById(long? Id);
        List<TermsOfPayment> TOPLUEdit(long Id, long companyId);
        List<TermsOfPayment> TOPLUNew(long companyId);
    }
}
