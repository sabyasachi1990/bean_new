using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class OpeningBalanceModel
    {
        public OpeningBalanceModel()
        {
            this.Details = new List<OpeningBalanceDetailModel>();
        }
        public Guid Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public long CompanyId { get; set; }
        [Required]
        public long ServiceCompanyId{get;set;}
        public string ServiceCompanyName { get; set; }
        [Required]
        public string BaseCurrency { get; set; }
        public Guid? JournalId { get; set; }
        public bool? IsMultiCurrencyActive { get; set; }
        public bool? IsSegmentActive { get; set; }
        public string DocType { get; set; }
        public string UserCreated { get; set; }
        [Column(TypeName = "datetime2")]
        public Nullable<DateTime> CreatedDate { get; set; }
        public string UserModified { get; set; }
        [Column(TypeName = "datetime2")]
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string SaveType { get; set; }
        public DateTime? FinancialPeriodLockStartDate { get; set; }
        public DateTime? FinancialPeriodLockEndDate { get; set; }
        //public DateTime? FinancialStartDate { get; set; }
        //public DateTime? FinancialEndDate { get; set; }
        public string PeriodLockPassword { get; set; }
        public string SystemRefNo { get; set; }
        public string DocState { get; set; }
        public string Version { get; set; }
        public bool? IsNoSupportingDoc { get; set; }
        public bool? IsGSTActivated { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsTemporary { get; set; }
        public List<OpeningBalanceDetailModel> Details { get; set; }
        public string CurrentState { get; set; }
        //lookup curriencieslu curriencieslu{get;AppDomainSetup;}
    }
}
