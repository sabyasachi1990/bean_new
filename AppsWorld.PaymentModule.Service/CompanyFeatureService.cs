using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.CommonModule.Entities;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Models;
using AppsWorld.PaymentModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.PaymentModule.Infra;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using AppsWorld.PaymentModule.Infra.Resources;
namespace AppsWorld.PaymentModule.Service
{
    public class CompanyFeatureService : Service<CompanyFeature>, ICompanyFeatureService
    {
        private readonly IPaymentModuleRepositoryAsync<CompanyFeature> _companyFeatureRepository;

        public CompanyFeatureService(IPaymentModuleRepositoryAsync<CompanyFeature> companyFeatureRepository)
            : base(companyFeatureRepository)
        {
            this._companyFeatureRepository = companyFeatureRepository;
        }
        public List<CompanyFeature> GetCompanyFeature(long companyId)
        {
            return _companyFeatureRepository.Query(a => a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Include(c => c.Feature).Select().ToList();
        }
    }
}
