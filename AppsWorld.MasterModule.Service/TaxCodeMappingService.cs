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
    public class TaxCodeMappingService : Service<TaxCodeMapping>, ITaxCodeMappingService
    {
        private readonly IMasterModuleRepositoryAsync<TaxCodeMapping> _taxCodeMappingServiceRepository;

        public TaxCodeMappingService(IMasterModuleRepositoryAsync<TaxCodeMapping> taxCodeMappingServiceRepository)
            : base(taxCodeMappingServiceRepository)
        {
            _taxCodeMappingServiceRepository = taxCodeMappingServiceRepository;

        }
        public TaxCodeMapping GetTaxCodeById(long? companyId)
        {
            return _taxCodeMappingServiceRepository.Queryable().Where(s => s.CompanyId == companyId).Include(s=>s.TaxCodeMappingDetails).FirstOrDefault();
        }
    }
}
