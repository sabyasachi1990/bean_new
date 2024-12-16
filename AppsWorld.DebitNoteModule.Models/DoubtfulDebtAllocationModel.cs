using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Models
{
    public class DoubtfulDebtAllocationModel
    {
        public DoubtfulDebtAllocationModel()
        {
            this.DoubtfulDebtAllocationDetailModels = new List<DoubtfulDebtAllocationDetailModel>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Guid InvoiceId { get; set; }
        public string DocNo { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DoubtfulDebtAllocationNumber { get; set; }
        public string DocCurrency { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public bool? NoSupportingDocument { get; set; }
        public System.DateTime DoubtfulDebitAllocationDate { get; set; }
        public Nullable<System.DateTime> DoubtfulDebitAllocationResetDate { get; set; }
        public decimal AllocateAmount { get; set; }
        public decimal DoubtfulDebtAmount { get; set; }
        public decimal DoubtfulDebtBalanceAmount { get; set; }
        public string Remarks { get; set; }
        DoubtfulDebtAllocationStatus _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public DoubtfulDebtAllocationStatus Status
        {
            get
            {
                return _status;
            }
            set { _status = (DoubtfulDebtAllocationStatus)value; }
        }
        public string PeriodLockPassword { get; set; }
        public Nullable<DateTime> FinancialStartDate { get; set; }
        public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }

        public virtual List<DoubtfulDebtAllocationDetailModel> DoubtfulDebtAllocationDetailModels { get; set; }
    }
}
