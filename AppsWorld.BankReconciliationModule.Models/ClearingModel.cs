using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;
using AppsWorld.Framework;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.BankReconciliationModule.Models
{
    public class ClearingModel
    {
		public long CompanyId { get; set; }
        public virtual List<BankReconciliationDetailModel> Details { get; set; }
    }
}
