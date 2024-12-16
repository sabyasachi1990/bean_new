
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
   public class ModuleMasterService:Service<ModuleMaster>,IModuleMosterService
    {
       private readonly IMasterModuleRepositoryAsync<ModuleMaster> _moduleMasterRepository;
       public ModuleMasterService(IMasterModuleRepositoryAsync<ModuleMaster> moduleMasterRepository)
           : base(moduleMasterRepository)
       {
           this._moduleMasterRepository = moduleMasterRepository;
       }
       public ModuleMaster GetModuleMaster(long companyId, string name)
       {
           return _moduleMasterRepository.Query(c => c.CompanyId == 0 && c.Name == name).Select().FirstOrDefault();
       }

    }
}
