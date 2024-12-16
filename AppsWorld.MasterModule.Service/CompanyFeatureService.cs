
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
   public class CompanyFeatureService:Service<CompanyFeature>,ICompanyFeatureService
    {
       private readonly IMasterModuleRepositoryAsync<CompanyFeature> _companyFeatureRepository;
       public CompanyFeatureService(IMasterModuleRepositoryAsync<CompanyFeature> companyFeatureRepository)
           : base(companyFeatureRepository)
       {
           this._companyFeatureRepository = companyFeatureRepository;
       }
       public CompanyFeature GetFeature(string name, long companyId, long moduleId)
       {
           return _companyFeatureRepository.Query(a => a.Feature.Name == name && a.CompanyId == companyId && a.Feature.ModuleId == moduleId).Select().FirstOrDefault();
       }

        public List<CompanyFeature> GetFeaturesByCidandFid(long companyid, List<Guid> listFeaturesId)
        {
            return _companyFeatureRepository.Query(z => z.CompanyId == companyid && z.Status == RecordStatusEnum.Active && listFeaturesId.Contains(z.FeatureId)).Select().ToList();
        }
    }
}
