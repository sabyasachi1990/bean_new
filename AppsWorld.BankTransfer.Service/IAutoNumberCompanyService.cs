
using AppsWorld.BankTransferModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public interface IAutoNumberCompanyService : IService<AutoNumberCompany>
    {
        //List<AutoNumberCompany> GetAutoNumberCompany(Guid AutoNumberId);
    }
}
