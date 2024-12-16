using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class CCAccountTypeService : Service<CCAccountType>, ICCAccountTypeService
    {
        private readonly IMasterModuleRepositoryAsync<CCAccountType> _ccAccountTypeRepository;
        public CCAccountTypeService(IMasterModuleRepositoryAsync<CCAccountType> ccAccountTypeService)
            : base(ccAccountTypeService)
        {
            _ccAccountTypeRepository = ccAccountTypeService;
        }
        public async Task<IEnumerable<CCAccountType>> GetAllCCAccountType(long companyid)
        { 
            return await Task.Run(()=> _ccAccountTypeRepository.Query(a => a.CompanyId == companyid).Include(c=>c.AccountTypeIdTypes).Select().ToList());
        }
    }
}
