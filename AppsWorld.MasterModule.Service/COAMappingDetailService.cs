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
    public class COAMappingDetailService : Service<COAMappingDetail>, ICOAMappingDetailService
    {
        private readonly IMasterModuleRepositoryAsync<COAMappingDetail> _cOAMappingDetailServiceRepository;
     
        public COAMappingDetailService(IMasterModuleRepositoryAsync<COAMappingDetail> cOAMappingDetailServiceRepository)
            : base(cOAMappingDetailServiceRepository)
        {
            _cOAMappingDetailServiceRepository = cOAMappingDetailServiceRepository;
            
        }
        public COAMappingDetail GetCOAById(Guid Id)
        {
            return _cOAMappingDetailServiceRepository.Queryable().Where(s => s.Id == Id).FirstOrDefault();
        }
        public List<COAMappingDetail> GetAllMappingSDetailById(List<Guid> Ids)
        {
            return _cOAMappingDetailServiceRepository.Query(c => Ids.Contains(c.Id)).Select().ToList();
        }
    }
}
