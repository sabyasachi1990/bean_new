﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.ReceiptModule.Service
{
	public interface ICurrencyService : IService<Currency>
    {
		Currency GetCurrencyByCode(long companyId, string code);

		LookUpCategory<string> GetByCurrencies(long CompanyId, string CategoryCode);
		LookUpCategory<string> GetByCurrenciesEdit(long CompanyId, string code, string CategoryCode);

    }
}