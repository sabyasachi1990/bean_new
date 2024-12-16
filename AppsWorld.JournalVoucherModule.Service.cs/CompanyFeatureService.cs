using System;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
 
namespace AppsWorld.JournalVoucherModule.Service
{
    public class CompanyFeatureService : Service<CompanyFeature>, ICompanyFeatureService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<CompanyFeature> _companyFeatureRepository;

        public CompanyFeatureService(IJournalVoucherModuleRepositoryAsync<CompanyFeature> companyFeatureRepository)
            : base(companyFeatureRepository)
        {
            this._companyFeatureRepository = companyFeatureRepository;
        }

    }
}
