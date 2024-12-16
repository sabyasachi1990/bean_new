using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
	public interface ITermsOfPaymentService : IService<TermsOfPayment>
    {
        TermsOfPayment GetById(long Id);

        List<TermsOfPayment> TOPLUEdit(long Id, long companyId);

        List<TermsOfPayment> TOPLUNew(long companyId);

		List<TermsOfPayment> TOPVendorLUEdit(long Id, long companyId);

		List<TermsOfPayment> TOPVendorLUNew(long companyId);


        TermsOfPayment GetTermsOfPayments(int days, long? comapnYId);
    }
}
