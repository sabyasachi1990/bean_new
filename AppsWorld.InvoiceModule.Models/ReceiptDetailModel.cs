using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
	public class ReceiptDetailModel
	{
		public ReceiptDetailModel()
		{
			ReceiptDetailModels = new List<ReceiptDetailModel>();
		}
		public Guid Id { get; set; }
		public Guid ReceiptId { get; set; }
		public Guid DocumentId { get; set; }
		public System.DateTime DocumentDate { get; set; }
		public string DocumentType { get; set; }
		public string SystemReferenceNumber { get; set; }
		public string DocumentNo { get; set; }
		public String DocumentState { get; set; }
		public String Nature { get; set; }
		public decimal DocumentAmmount { get; set; }
		public decimal AmmountDue { get; set; }
		public string Currency { get; set; }
		public decimal ReceiptAmount { get; set; }
		public string PeriodLockPassword { get; set; }
		public Nullable<DateTime> FinancialStartDate { get; set; }
		public Nullable<DateTime> FinancialEndDate { get; set; }
		public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
		public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
		public Nullable<decimal> BaseExchangeRate { get; set; }
        public string ServiceCompanyName { get; set; }
        public long? ServiceCompanyId { get; set; }

        public List<ReceiptDetailModel> ReceiptDetailModels { get; set; } 



	}
}
