using AppsWorld.BillModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Service
{
    public interface IPaymentService : IService<Payment>
    {
        Payment GetPaymentById(Guid pymentid);
        Payment GetPaymentByComapnyId(long companyId, string docType);
        Payment GetDocNo(string docNo, long companyId);
        Payment GetLastPayement(long companyId, string docType);
    }
}
