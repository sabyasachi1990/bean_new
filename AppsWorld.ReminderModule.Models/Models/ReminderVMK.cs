using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Models.Models
{
    public  class ReminderVMK
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string ReminderType { get; set; }
        public string ReminderName { get; set; }
        public Nullable<DateTime> RemainderDate { get; set; }
        public Guid EntityId { get; set; }
        public string EntityName { get; set; }
        //public int NoOfCases { get; set; }
        //public Decimal? CasesFee { get; set; }
        public decimal? BalanceAmount { get; set; }
        //public Decimal? UnPaidAmount { get; set; }
        public string Recipient { get; set; }
        public string User { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> LaseSent { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string AzurePath { get; set; }
        public Nullable<DateTime> DismissOrSentDate { get; set; }
        public string Status { get; set; }
    }
}
