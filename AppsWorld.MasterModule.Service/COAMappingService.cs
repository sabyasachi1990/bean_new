using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class COAMappingService : Service<COAMapping>, ICOAMappingService
    {
        private readonly IMasterModuleRepositoryAsync<COAMapping> _cOAMappingServiceRepository;

        public COAMappingService(IMasterModuleRepositoryAsync<COAMapping> cOAMappingServiceRepository)
            : base(cOAMappingServiceRepository)
        {
            _cOAMappingServiceRepository = cOAMappingServiceRepository;

        }
        public COAMapping GetCOAMappingById(long companyId)
        {
            return _cOAMappingServiceRepository.Queryable().Where(s => s.CompanyId == companyId).Include(s => s.COAMappingDetails).FirstOrDefault();
        }
       
    }
}
