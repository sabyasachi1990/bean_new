﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
	public interface IControlCodeService : IService<ControlCode>
    {
		ControlCode GetControlCodeCode(string code);

		List<ControlCode> GetControlCodesByCatId(long CatId);
    }
}