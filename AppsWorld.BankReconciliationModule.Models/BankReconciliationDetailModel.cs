using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Net.Configuration;

namespace AppsWorld.BankReconciliationModule.Models
{
    public class BankReconciliationDetailModel
    {
        public BankReconciliationDetailModel()
        {
            Id = Guid.NewGuid();
        }
        public System.Guid Id { get; set; }
        public System.Guid JournalId { get; set; }
        public System.Guid? BankReconciliationId { get; set; }
        [Required(ErrorMessage = "DocumentDate is Required")]
        public Nullable<System.DateTime> DocumentDate { get; set; }
        [Required(ErrorMessage = "DocumentType is Required")]
        public string DocumentType { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        public string EntityName { get; set; }
        public string DocRefNo { get; set; }
        public long CompanyId { get; set; }
        public Nullable<decimal> Ammount { get; set; }
        public string ClearingStatus { get; set; }
        public Nullable<System.DateTime> ClearingDate { get; set; }
        [Required(ErrorMessage = "DocumentId is Required")]
        public Nullable<System.Guid> DocumentId { get; set; }
        public Nullable<bool> isWithdrawl { get; set; }
        public bool? Withdrawal { get; set; }
        public string DocSubType { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsReconcile { get; set; }
        public bool? IsDisable { get; set; }
        public DateTime? ReconciliationDate { get; set; }
        public DateTime? LastReconciliationDate { get; set; }
        public long? COAId { get; set; }
        public long? ServiceEntityId { get; set; }
        public bool? IsChecked { get; set; }
        public bool? IsUncleared { get; set; }
        public string RecordStatus { get; set; }
        public bool? isClearedTab { get; set; }
        public string Mode { get; set; }
        public string RefNo { get; set; }
        public int RowNumber { get; set; }
        public Guid? JournalDetailId { get; set; }
    }
}
