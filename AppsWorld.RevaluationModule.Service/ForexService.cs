using Service.Pattern;
using System;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.RevaluationModule.Entities.Models;

namespace AppsWorld.RevaluationModule.Service
{
    public class ForexService : Service<Forex>, IForexService
    {
        private readonly IRevaluationModuleRepositoryAsync<Forex> _forexRepository;
		private readonly IGSTSettingService _gstSettingService;
		private readonly IFinancialSettingService _financialSettingService;

		public ForexService(IRevaluationModuleRepositoryAsync<Forex> forexRepository, IGSTSettingService gstSettingService, IFinancialSettingService financialSettingService)
			: base(forexRepository)
        {
			_forexRepository = forexRepository;
			_gstSettingService = gstSettingService;
			_financialSettingService = financialSettingService;
        }

		public Forex GetMultiCurrencyInformation(string DocumentCurrency, DateTime Documentdate, bool IsBase, long CompanyId)
		{
			Forex forex = null;
			DateTime mydate = Documentdate.Date;
			if (IsBase)
			{
				forex = _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId && Documentdate >= a.DateFrom && Documentdate <= a.Dateto && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
				if (forex == null)
				{
					forex = _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active)
						.Select().OrderByDescending(b => b.Dateto).FirstOrDefault();
				}
			}
			else
			{
				GSTSetting GSTSetting = _gstSettingService.GetGSTSettings(CompanyId);
				if (GSTSetting != null)
				{
					FinancialSetting financialSetting = _financialSettingService.GetFinancialSetting(CompanyId);
					if (financialSetting != null && financialSetting.BaseCurrency == GSTSetting.GSTRepoCurrency)
					{
						forex = _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId && Documentdate >= a.DateFrom && Documentdate <= a.Dateto && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
						if (forex == null)
						{
							forex = _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active)
								.Select().OrderByDescending(b => b.Dateto).FirstOrDefault();
						}
					}
					else
					{
						forex =
							_forexRepository.Query(
								a =>
									a.Currency == DocumentCurrency && a.Type == "GST Currency" &&
									a.CompanyId == CompanyId && (Documentdate >= a.DateFrom && Documentdate <= a.Dateto) && a.Status == RecordStatusEnum.Active)
								.Select()
								.FirstOrDefault();
						if (forex == null)
						{
							forex =
								_forexRepository.Query(
									a =>
										a.Currency == DocumentCurrency && a.Type == "GST Currency" &&
										a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active)
									.Select().OrderByDescending(b => b.Dateto).FirstOrDefault();
						}
					}
				}
			}
			if (forex != null)
			{
				forex.UnitPerUSD = 1 / forex.UnitPerUSD;
				return forex;
			}
			else return null;
		}

    }
}
