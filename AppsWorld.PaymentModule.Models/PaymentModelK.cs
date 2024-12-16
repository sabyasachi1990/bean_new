using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Models
{
    public class PaymentModelK
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        //public string SystemRefNo { get; set; }
        public DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public double? BankPaymentAmmount { get; set; }
        public string BankPaymentAmmountCurrency { get; set; }
        public string PaymentRefNo { get; set; }
        public string ServiceCompanyName { get; set; }
        public string DocumentState { get; set; }
        //public string DocDescription { get; set; }
        public string CashBankAccount { get; set; }
        public string PaymentApplicationCurrency { get; set; }
        public double? PaymentApplicationAmmount { get; set; }
        public string ExchangeRate { get; set; }
        //public Nullable<bool> NoSupportingDocument { get; set; }
        public string EntityName { get; set; }
        public string ModeOfPayment { get; set; }
        public double GrandTotal { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string UserCreated { get; set; }
        public DateTime? BankClearingDate { get; set; }
        public bool? IsLocked { get; set; }
        public string DocType { get; set; }
    }
}
