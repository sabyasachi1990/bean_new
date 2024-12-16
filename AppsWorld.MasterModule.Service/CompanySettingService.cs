using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Repository.Pattern.Infrastructure;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class CompanySettingService : Service<CompanySetting>, ICompanySettingService
    {
        private readonly IMasterModuleRepositoryAsync<CompanySetting> _companySettingRepository;
        private readonly IMasterModuleUnitOfWorkAsync _unitOfWork;
        public CompanySettingService(IMasterModuleRepositoryAsync<CompanySetting> companySettingRepository)
            : base(companySettingRepository)
        {
            this._companySettingRepository = companySettingRepository;
        }
        public CompanySetting ActivateModule(string moduleName, long companyId)
        {
            return _companySettingRepository.Query(a => a.ModuleName == moduleName && a.CompanyId == companyId && a.IsEnabled == false).Select().FirstOrDefault();
        }
        public CompanySetting ActivateModuleM(string moduleName, long companyId)
        {
            return _companySettingRepository.Query(a => a.ModuleName == moduleName && a.CompanyId == companyId).Select().FirstOrDefault();
        }

        public bool GetModuleStatuss(string moduleName, long companyId)
        {
            CompanySetting setting = _companySettingRepository.Query(a => a.ModuleName == moduleName && a.CompanyId == companyId).Select().FirstOrDefault();
            if (setting != null)
                return setting.IsEnabled;
            return false;
        }
        public bool ActivateModules(string moduleName, long companyId)
        {
            CompanySetting setting = _companySettingRepository.Query(a => a.ModuleName == moduleName && a.CompanyId == companyId && a.IsEnabled == false).Select().FirstOrDefault();
            if (setting != null)
            {
                setting.IsEnabled = true;
                setting.ObjectState = ObjectState.Modified;
                _companySettingRepository.Update(setting);
                _unitOfWork.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
