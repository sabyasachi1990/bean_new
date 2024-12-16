using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;

namespace AppsWorld.ReceiptModule.Service
{
	//public class BankReconciliationSettingService : Service<BankReconciliationSetting>, IBankReconciliationSettingService
 //   {
	//	private readonly IReceiptModuleRepositoryAsync<BankReconciliationSetting> _bankReconciliationRepository;

	//	public BankReconciliationSettingService(IReceiptModuleRepositoryAsync<BankReconciliationSetting> bankreconcilationRepository)
	//		: base(bankreconcilationRepository)
 //       {
	//		_bankReconciliationRepository = bankreconcilationRepository;
 //       }
	//	public BankReconciliationSetting GetByCompanyId(long companyId)
	//	{
	//		return _bankReconciliationRepository.Query(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
	//	}
	//	//public GSTSetting GetGSTSettings(long companyId)
	//	//{
	//	//	return _bankReconciliationRepository.Query(a => a.CompanyId == companyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
	//	//}
	//	//public bool IsGSTSettingActivated(long companyId)
	//	//{
	//	//	return GetGSTSettings(companyId) != null;
	//	//}

		
 //   }
}
