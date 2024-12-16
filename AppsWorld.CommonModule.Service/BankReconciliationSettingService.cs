using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Entities;
//using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.RepositoryPattern;

namespace AppsWorld.CommonModule.Service
{
	//public class BankReconciliationSettingService : Service<BankReconciliationSetting>, IBankReconciliationSettingService
 //   {
	//	private readonly ICommonModuleRepositoryAsync<BankReconciliationSetting> _bankReconciliationRepository;

	//	public BankReconciliationSettingService(ICommonModuleRepositoryAsync<BankReconciliationSetting> bankreconcilationRepository)
	//		: base(bankreconcilationRepository)
 //       {
	//		_bankReconciliationRepository = bankreconcilationRepository;
 //       }
	//	public BankReconciliationSetting GetByCompanyId(long companyId)
	//	{
	//		return _bankReconciliationRepository.Query(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
	//	}
 //       public DateTime? GetByCompanyIdByDate(long companyId)
 //       {
 //           return _bankReconciliationRepository.Query(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Select(x=>x.BankClearingDate).FirstOrDefault();
 //       }
 //       //public GSTSetting GetGSTSettings(long companyId)
 //       //{
 //       //	return _bankReconciliationRepository.Query(a => a.CompanyId == companyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
 //       //}
 //       //public bool IsGSTSettingActivated(long companyId)
 //       //{
 //       //	return GetGSTSettings(companyId) != null;
 //       //}


 //   }
}
