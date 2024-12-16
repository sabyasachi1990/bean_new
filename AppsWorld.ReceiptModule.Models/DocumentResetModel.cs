using System;
 
namespace AppsWorld.ReceiptModule.Models
{
    public class DocumentResetModel
    {
        public System.Guid Id { get; set; }
        public System.Guid CreditMemoId { get; set; }
        public System.Guid InvoiceId { get; set; }
        public long CompanyId { get; set; }
        public Nullable<System.DateTime> ResetDate { get; set; }
        public string PeriodLockPassword { get; set; }
        public string Version { get; set; }
    }
}
