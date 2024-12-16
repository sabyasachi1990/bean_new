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

    public class FeatureService : Service<Feature>, IFeatureService
    {
        private readonly IMasterModuleRepositoryAsync<Feature> _featureRepository;
        public FeatureService(IMasterModuleRepositoryAsync<Feature> featureRepository)
            : base(featureRepository)
        {
            _featureRepository = featureRepository;
        }
        public List<Guid> GetFeatureIdsByMId(long moduleId)
        {
            return _featureRepository.Queryable().Where(c => c.ModuleId == moduleId).Select(c => c.Id).ToList();
        }
    }
}
