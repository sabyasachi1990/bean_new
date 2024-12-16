using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
   public interface IInvoiceService:IService<Invoice>
    {
        Invoice GetbyIdAndCompanyId(Guid guid, long CompanyId);
        List<Invoice> GetAllByEntityId(Guid id);
        List<Invoice> GetAllCnByEntityId(Guid id);
        Task<Invoice> GetALlinvocesByItems(Guid? DocumentId);

        Guid? GetEntityIdById(Guid guid, long CompanyId);
        //List<Guid> GetALlCashSaleByItems(Guid? DocumentId);
    }
}
