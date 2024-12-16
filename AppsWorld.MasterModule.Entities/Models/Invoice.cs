using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.MasterModule.Entities
{
    public partial class Invoice : Entity
    {
        public Invoice()
        {
            //this.CreditNoteApplications = new List<CreditNoteApplication>();
            //this.DoubtfulDebtAllocations = new List<DoubtfulDebtAllocation>();
            this.InvoiceDetails = new List<InvoiceDetail>();
            //this.InvoiceGSTDetails = new List<InvoiceGSTDetail>();
            //this.InvoiceNotes = new List<InvoiceNote>();
        }

        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public System.DateTime DocDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string DocNo { get; set; }
        public string PONo { get; set; }
        public Nullable<bool> NoSupportingDocs { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public bool IsRepeatingInvoice { get; set; }
        public Nullable<int> RepEveryPeriodNo { get; set; }
        public string RepEveryPeriod { get; set; }
        public Nullable<System.DateTime> RepEndDate { get; set; }
        public string EntityType { get; set; }
        public System.Guid EntityId { get; set; }
        public Nullable<long> CreditTermsId { get; set; }
        public string Nature { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public string DocumentState { get; set; }
        public decimal BalanceAmount { get; set; }
        public Nullable<decimal> GSTTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool? IsSegmentReporting { get; set; }
        public bool IsAllowableNonAllowable { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.Guid> ParentInvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<bool> IsNoSupportingDocument { get; set; }
        public Nullable<long> ServiceCompanyId { get; set; }
        public string DocType { get; set; }
        public Nullable<bool> IsAllowableDisallowableActivated { get; set; }
        public Nullable<System.DateTime> ReverseDate { get; set; }
        public Nullable<bool> ReverseIsSupportingDocument { get; set; }
        public string ReverseRemarks { get; set; }
        public Nullable<decimal> AllocatedAmount { get; set; }
        public Nullable<long> SegmentMasterid1 { get; set; }
        public Nullable<long> SegmentMasterid2 { get; set; }
        public Nullable<long> SegmentDetailid1 { get; set; }
        public Nullable<long> SegmentDetailid2 { get; set; }
        public Nullable<bool> IsBaseCurrencyRateChanged { get; set; }
        public Nullable<bool> IsGSTCurrencyRateChanged { get; set; }
        public Nullable<bool> IsGSTApplied { get; set; }
        public Nullable<decimal> ItemTotal { get; set; }
        public string ExtensionType { get; set; }
        public string DocDescription { get; set; }
        public bool? IsOBInvoice { get; set; }

        public bool? IsWorkFlowInvoice { get; set; }
        //public virtual ICollection<CreditNoteApplication> CreditNoteApplications { get; set; }
        //public virtual ICollection<DoubtfulDebtAllocation> DoubtfulDebtAllocations { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        //public virtual ICollection<InvoiceGSTDetail> InvoiceGSTDetails { get; set; }
        //public virtual ICollection<InvoiceNote> InvoiceNotes { get; set; }
        [ForeignKey("EntityId")]
        public virtual BeanEntity BeanEntiity { get; set; }
    }
    }

