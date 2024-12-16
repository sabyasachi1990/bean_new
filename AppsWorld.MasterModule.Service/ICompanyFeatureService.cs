using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface ICompanyFeatureService : IService<CompanyFeature>
    {
        CompanyFeature GetFeature(string name, long companyId, long moduleId);
        List<CompanyFeature> GetFeaturesByCidandFid(long companyid, List<Guid> listFeaturesId);
    }
}
