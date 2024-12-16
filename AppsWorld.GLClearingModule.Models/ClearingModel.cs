using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.Models
{
    public class ClearingModel
    {
        public ClearingModel()
        {
            this.ClearingDetailModel = new List<Models.ClearingDetailModel>();
            //this.BeanEntity = new EntityModel();
        }
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public System.DateTime? DocDate { get; set; }
        public System.DateTime? FromDate { get; set; }
        public System.DateTime? ToDate { get; set; }
        public bool? IsClearingChecked { get; set; }
        public string DocNo { get; set; }
        public long? ServiceCompanyId { get; set; }
        public long? COAId { get; set; }
        //public string DocDescription { get; set; }
        //public bool IsMultiCurrency { get; set; }
        //public string SystemRefNo { get; set; }
        //public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        //public string CrDr { get; set; }
        //public decimal? CheckAmount { get; set; }
        public string DocumentState { get; set; }
        //public long COAId2 { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> Status { get; set; }
        public virtual List<ClearingDetailModel> ClearingDetailModel { get; set; }
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string PeriodLockPassword { get; set; }
        public string ExCurrency { get; set; }
        //public Guid? JournalId { get; set; }
        public bool? IsDocNoEditable { get; set; }
        
        //public Guid? EntityId { get; set; }
        //public string EntityName { get; set; }
        //public string EntityType { get; set; }
        //public System.DateTime? BankClearingDate { get; set; }
        //public string DocDescription { get; set; }
        //public EntityModel BeanEntity { get; set; }
        public string Version { get; set; }
        public bool? IsLocked { get; set; }
    }
}
