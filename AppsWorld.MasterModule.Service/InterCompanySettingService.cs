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
    public class InterCompanySettingService : Service<InterCompanySetting>, IInterCompanySettingService
    {
        private readonly IMasterModuleRepositoryAsync<InterCompanySetting> _InterCompanySettingRepository;

        public InterCompanySettingService(IMasterModuleRepositoryAsync<InterCompanySetting> InterCompanySettingRepository)
            : base(InterCompanySettingRepository)
        {
            _InterCompanySettingRepository = InterCompanySettingRepository;

        }

        public InterCompanySetting GetInterCoCompanyIdAndType(long companyId, string interType)
        {
            return _InterCompanySettingRepository.Query(a => a.CompanyId == companyId && a.InterCompanyType == interType).Select().FirstOrDefault();
        }

        public InterCompanySetting GetInterCompanyClearingById(Guid Id)
        {
            return _InterCompanySettingRepository.Queryable().Where(s => s.Id == Id).Include(a => a.InterCompanySettingDetails).FirstOrDefault();
        }


        public bool? GetIBIsActivatedOrNot(long companyId, string InterType)
        {
            return _InterCompanySettingRepository.Query(a => a.CompanyId == companyId && a.InterCompanyType == InterType).Select(c => c.IsInterCompanyEnabled).FirstOrDefault();
        }

    }
}
