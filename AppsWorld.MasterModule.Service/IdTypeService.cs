using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class IdTypeService : Service<IdType>, IIdTypeService
    {
        private readonly IMasterModuleRepositoryAsync<IdType> _idTypeRepository;
        public IdTypeService(IMasterModuleRepositoryAsync<IdType> idTypeRepository)
            : base(idTypeRepository)
        {
            _idTypeRepository = idTypeRepository;
        }
        public async Task<IEnumerable<IdType>> GetAllIdTypes(long tempId, long CompanyId)
        {
            return await Task.Run(()=> _idTypeRepository.Query(a => (a.Status == RecordStatusEnum.Active || a.Id == tempId) && a.CompanyId == CompanyId).Select().ToList());
        }

    }
}
