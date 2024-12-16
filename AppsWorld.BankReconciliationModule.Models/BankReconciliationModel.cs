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
using AppsWorld.BankReconciliationModule.Entities;

namespace AppsWorld.BankReconciliationModule.Models
{
    public class BankReconciliationModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        [Required(ErrorMessage = "Chartofaccountidrequired")]
        public long COAId { get; set; }
        public long ServiceCompanyId { get; set; }
        public string ServiceCompanyName { get; set; }
        [Required(ErrorMessage = "Date should be in correct format")]
        public System.DateTime BankReconciliationDate { get; set; }
        [Required(ErrorMessage = "Currency field is Required")]
        public string Currency { get; set; }
        [Required(ErrorMessage = "BankAccount is Required")]
        public string BankAccount { get; set; }
        [Required(ErrorMessage = "StatementAmount is Required")]
        public decimal StatementAmount { get; set; }
        //public decimal OutstandingWithdrawals { get; set; }
        public decimal? SubTotal { get; set; }
        //public decimal OutstandingDeposits { get; set; }
        [Required(ErrorMessage = "StatementExpectedAmount is Required")]
        public decimal? StatementExpectedAmount { get; set; }
        public decimal? GLAmount { get; set; }
        [Required(ErrorMessage = "State is Required")]
        public string State { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Version { get; set; }
        public Nullable<System.DateTime> StatementDate { get; set; }
        public Nullable<Boolean> IsDraft { get; set; }
        RecordStatusEnum _status;
        //[Required(ErrorMessage = "Status Required")]

        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
        //public List<BankReconciliationDetail> BankReconciliationDetails { get; set; }
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string PeriodLockPassword { get; set; }

        public List<BankReconciliationDetailModel> BankReconciliationDetailModels { get; set; }
        public Nullable<System.Guid> ClearingId { get; set; }
        public DateTime? LastReconciledDate { get; set; }
        public bool? IsLocked { get; set; }



    }
}
