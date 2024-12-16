using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    //public class BankReconciliationSettingService : Service<BankReconciliationSetting>, IBankReconciliationSettingService
    //{
    //    private readonly IMasterModuleRepositoryAsync<BankReconciliationSetting> _bankReconciliationSettingRepository;
    //    public BankReconciliationSettingService(IMasterModuleRepositoryAsync<BankReconciliationSetting> bankReconciliationSettingRepository)
    //        : base(bankReconciliationSettingRepository)
    //    {
    //        this._bankReconciliationSettingRepository = bankReconciliationSettingRepository;
    //    }
    //    public BankReconciliationSetting GetByCompanyId(long companyId)
    //    {
    //        return _bankReconciliationSettingRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
    //    }
    //    public BankReconciliationSetting GetByCompanyIdAndId(long Id, long companyId)
    //    {
    //        return _bankReconciliationSettingRepository.Query(e => e.Id == Id && e.CompanyId == companyId).Select().FirstOrDefault();
    //    }

    //}
}
