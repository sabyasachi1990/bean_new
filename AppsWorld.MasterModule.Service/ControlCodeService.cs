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
    public class ControlCodeService : Service<ControlCode>, IControlCodeService
    {
        private readonly IMasterModuleRepositoryAsync<ControlCode> _controlCodeRepository;
        public ControlCodeService(IMasterModuleRepositoryAsync<ControlCode> controlCodeRepository)
            : base(controlCodeRepository)
        {
            _controlCodeRepository = controlCodeRepository;
        }

        public List<ControlCode> GetControlCodesByCatId(long CatId)
        {
            return _controlCodeRepository.Queryable().Where(c => c.ControlCategoryId == CatId).ToList();
        }

    }
}
