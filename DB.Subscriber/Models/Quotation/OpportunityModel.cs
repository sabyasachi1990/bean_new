namespace DB.Subscriber.Models.Quotation
{
    using System;
    //using System.Data.Entity.Spatial;

    public class OpportunityModel 
    {
        public OpportunityModel()
        {
            //this.FileRepoModel = new List<FileRepositoryModel>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public long ServiceCompanyId { get; set; }
        public string OpportunityName { get; set; }
        public long ServiceGroupId { get; set; }
        public string OpportunityRefNo { get; set; }
        public long ServiceId { get; set; }
        public string Type { get; set; }
        public string Stage { get; set; }
        public string Nature { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCompanyName { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public DateTime? ReOpen { get; set; }

        public string Frequency { get; set; }

        public string FeeType { get; set; }

        public decimal? Fee { get; set; }

        public double? TargettedRecovery { get; set; }

        public string SpecialRemarks { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string UserCreated { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public bool? IsRecurring { get; set; }
        public bool? IsAdHoc { get; set; }
        public bool? IsConfirmed { get; set; }

        public string OpportunityNumber { get; set; }

        public string RecurringScopeofWork { get; set; }
        public string ReportPath { get; set; }

        public Nullable<bool> IsModified { get; set; }

        public Nullable<bool> IsChangeHappen { get; set; }
        public string SummaryAttachment { get; set; }

        public Guid QuotationId { get; set; }

        public string KeyTemplateContent { get; set; }

        public string StandardTemplateContent { get; set; }

        public bool? IsModify { get; set; }
        public Guid? StandardTerms { get; set; }
        public Guid? KeyTerms { get; set; }

    }
}

