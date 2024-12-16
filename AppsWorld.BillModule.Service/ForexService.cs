using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.BillModule.Service
{
	//public class ForexService : Service<Forex>, IForexService
	//{
	//	private readonly IBillModuleRepositoryAsync<Forex> _forexRepository;
	//	private readonly IGSTSettingService _gstSettingService;
	//	private readonly IFinancialSettingService _financialSettingService;

	//	public ForexService(IBillModuleRepositoryAsync<Forex> forexRepository, IGSTSettingService gstSettingService, IFinancialSettingService financialSettingService)
	//		: base(forexRepository)
	//	{
	//		_forexRepository = forexRepository;
	//		_gstSettingService = gstSettingService;
	//		_financialSettingService = financialSettingService;
	//	}

	//	public Forex GetMultiCurrencyInformation(string DocumentCurrency, DateTime Documentdate, bool IsBase, long CompanyId)
	//	{
	//		Forex forex = null;
	//		DateTime mydate = Documentdate.Date;
	//		if (IsBase)
	//		{
	//			forex = _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId && Documentdate >= a.DateFrom && Documentdate <= a.Dateto && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
	//			if (forex == null)
	//			{
	//				forex = _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId)
	//					.Select().OrderByDescending(b => b.Dateto).FirstOrDefault();
	//			}
	//		}
	//		else
	//		{
	//			GSTSetting GSTSetting = _gstSettingService.GetGSTSettings(CompanyId);
	//			if (GSTSetting != null)
	//			{
	//				FinancialSetting financialSetting = _financialSettingService.GetFinancialSetting(CompanyId);
	//				if (financialSetting != null && financialSetting.BaseCurrency == GSTSetting.GSTRepoCurrency)
	//				{
	//					forex = _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId && Documentdate >= a.DateFrom && Documentdate <= a.Dateto && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
	//					if (forex == null)
	//					{
	//						forex = _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active)
	//							.Select().OrderByDescending(b => b.Dateto).FirstOrDefault();
	//					}
	//				}
	//				else
	//				{
	//					forex =
	//						_forexRepository.Query(
	//							a =>
	//								a.Currency == DocumentCurrency && a.Type == "GST Currency" &&
	//								a.CompanyId == CompanyId && (Documentdate >= a.DateFrom && Documentdate <= a.Dateto) && a.Status == RecordStatusEnum.Active)
	//							.Select()
	//							.FirstOrDefault();
	//					if (forex == null)
	//					{
	//						forex =
	//							_forexRepository.Query(
	//								a =>
	//									a.Currency == DocumentCurrency && a.Type == "GST Currency" &&
	//									a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active)
	//								.Select().OrderByDescending(b => b.Dateto).FirstOrDefault();
	//					}
	//				}
	//			}
	//		}
	//		if (forex != null)
	//			forex.UnitPerUSD = 1 / forex.UnitPerUSD;
	//		return forex;
	//	}

	//}
}
