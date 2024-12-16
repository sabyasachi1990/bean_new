using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
    public interface ICompanySettingService : IService<CompanySetting>
    {
        bool GetModuleStatus(string moduleName,  long companyId);
        
    }
}
