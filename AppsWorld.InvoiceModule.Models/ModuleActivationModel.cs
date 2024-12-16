using AppsWorld.BeanCursor.Entities.Models;
using AppsWorld.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.InvoiceModule.Models
{
	public class ModuleActivationModel
    {
		public bool IsGSTActive { get; set; }
		public bool? IsMultiCurrencyActive { get; set; }
		public bool IsNoSupportingDocumentActive { get; set; }
		public bool IsFinancialActive { get; set; }

		public bool IsAllowableNonAllowable { get; set; }
		public string BaseCurrency { get; set; }






    }
}
