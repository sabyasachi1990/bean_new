using AppsWorld.BillModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using AppsWorld.BillModule.RepositoryPattern;

namespace AppsWorld.BillModule.Service
{
    public class LocalizationService : Service<Localization>, ILocalizationService
    {
        IBillModuleRepositoryAsync<Localization> _localizationRepository;
        public LocalizationService(IBillModuleRepositoryAsync<Localization> localizationRepository) : base(localizationRepository)
        {
            this._localizationRepository = localizationRepository;
        }

        public string GetLocalization(long companyId)
        {
            return _localizationRepository.Query(a => a.CompanyId == companyId).Select(e=>e.BaseCurrency).FirstOrDefault();
        }
    }
}
