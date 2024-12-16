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
    public class TaxCodeMappingDetailService : Service<TaxCodeMappingDetail>, ITaxCodeMappingDetailService
    {
        private readonly IMasterModuleRepositoryAsync<TaxCodeMappingDetail> _taxCodeMappingDetailServiceRepository;
     
        public TaxCodeMappingDetailService(IMasterModuleRepositoryAsync<TaxCodeMappingDetail> taxCodeMappingDetailServiceRepository)
            : base(taxCodeMappingDetailServiceRepository)
        {
            _taxCodeMappingDetailServiceRepository = taxCodeMappingDetailServiceRepository;
            
        }
        public TaxCodeMappingDetail GetTaxCodeDetailById(Guid Id)
        {
            return _taxCodeMappingDetailServiceRepository.Queryable().Where(s => s.Id == Id).FirstOrDefault();
        }
        public List<TaxCodeMappingDetail> GetAllTaxCodeDetailById(List<Guid> Ids)
        {
            return _taxCodeMappingDetailServiceRepository.Queryable().Where(s =>Ids.Contains(s.Id)).ToList();
        }
    }
}
