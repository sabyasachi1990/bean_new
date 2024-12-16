using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface ICompanyService : IService<Company>
    {
        Company GetByNameByServiceCompany(long? parentId);

        Task<List<Company>> GetCompany(long companyId, long companyIdCheck ,string userName);

        Company GetById(long? id);

        List<FrameWork.LookUps.LookUp<long>> GetAllLookup(long CompanyId, string Username);
        string GetCompanyName(long? id);
    }


}

