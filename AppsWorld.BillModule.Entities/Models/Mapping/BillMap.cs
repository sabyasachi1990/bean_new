using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BillModule.Entities
{
    public class BillMap : EntityTypeConfiguration<Bill>
    {
        public BillMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocSubType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SystemReferenceNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DocDescription)
                .HasMaxLength(256);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.EntityType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Nature)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.DocCurrency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.BaseCurrency)
                .HasMaxLength(5);

            this.Property(t => t.SegmentCategory1)
                .HasMaxLength(50);

            this.Property(t => t.SegmentCategory2)
                .HasMaxLength(50);

            this.Property(t => t.GSTExCurrency)
                .HasMaxLength(5);

            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.ExchangeRate)
                .HasPrecision(15, 10);

            this.Property(t => t.GSTExchangeRate)
                .HasPrecision(15, 10);

            // Table & Column Mappings
            this.ToTable("Bill", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.SystemReferenceNumber).HasColumnName("SystemReferenceNumber");
            this.Property(t => t.PostingDate).HasColumnName("PostingDate");
            this.Property(t => t.DocumentDate).HasColumnName("DocumentDate");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.NoSupportingDocument).HasColumnName("NoSupportingDocument");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.CreditTermsId).HasColumnName("CreditTermsId");
            this.Property(t => t.CreditTermValue).HasColumnName("CreditTermValue");
            this.Property(t => t.Nature).HasColumnName("Nature");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.ExDurationFrom).HasColumnName("ExDurationFrom");
            this.Property(t => t.ExDurationTo).HasColumnName("ExDurationTo");
            this.Property(t => t.SegmentCategory1).HasColumnName("SegmentCategory1");
            this.Property(t => t.SegmentCategory2).HasColumnName("SegmentCategory2");
            this.Property(t => t.SegmentMasterid1).HasColumnName("SegmentMasterid1");
            this.Property(t => t.SegmentMasterid2).HasColumnName("SegmentMasterid2");
            this.Property(t => t.SegmentDetailid1).HasColumnName("SegmentDetailid1");
            this.Property(t => t.SegmentDetailid2).HasColumnName("SegmentDetailid2");
            this.Property(t => t.GSTExchangeRate).HasColumnName("GSTExchangeRate");
            this.Property(t => t.GSTExCurrency).HasColumnName("GSTExCurrency");
            this.Property(t => t.GSTExDurationFrom).HasColumnName("GSTExDurationFrom");
            this.Property(t => t.GSTExDurationTo).HasColumnName("GSTExDurationTo");
            this.Property(t => t.GSTTotalAmount).HasColumnName("GSTTotalAmount");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.IsNoSupportingDocument).HasColumnName("IsNoSupportingDocument");
            this.Property(t => t.IsGstSettings).HasColumnName("IsGstSettings");
            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");
            this.Property(t => t.IsSegmentReporting).HasColumnName("IsSegmentReporting");
            this.Property(t => t.IsAllowableDisallowable).HasColumnName("IsAllowableDisallowable");
            this.Property(t => t.IsGSTCurrencyRateChanged).HasColumnName("IsGSTCurrencyRateChanged");
            this.Property(t => t.IsBaseCurrencyRateChanged).HasColumnName("IsBaseCurrencyRateChanged");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.VendorType).HasColumnName("VendorType");
            this.Property(t => t.BalanceAmount).HasColumnName("BalanceAmount");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.PayrollId).HasColumnName("PayrollId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.IsExternal).HasColumnName("IsExternal");
            this.Property(t => t.OpeningBalanceId).HasColumnName("OpeningBalanceId");
            this.Property(t => t.SyncHRPayrollId).HasColumnName("SyncHRPayrollId");
            this.Property(t => t.SyncHRPayrollDate).HasColumnName("SyncHRPayrollDate");
            this.Property(t => t.SyncHRPayrollRemarks).HasColumnName("SyncHRPayrollRemarks");
            this.Property(t => t.SyncHRPayrollStatus).HasColumnName("SyncHRPayrollStatus");
            this.Property(t => t.BaseGrandTotal).HasColumnName("BaseGrandTotal");
            this.Property(t => t.BaseGrandTotal).HasColumnName("BaseGrandTotal");
            this.Property(t => t.ClearCount).HasColumnName("ClearCount");
            this.Property(t => t.IsLocked).HasColumnName("IsLocked");
            //this.Property(t => t.IsOBBill).HasColumnName("IsOBBill");
            //this.Property(t => t.IsGSTDeRegistration).HasColumnName("IsGSTDeRegistration");
            //this.Property(t => t.GSTDeRegistrationDate).HasColumnName("GSTDeRegistrationDate");

            this.Property(t => t.roundingamount).HasColumnName("roundingamount");
            this.Property(t => t.PeppolDocumentId).HasColumnName("PeppolDocumentId");



            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Bills)
            //    .HasForeignKey(d => d.CompanyId);
            //this.HasRequired(t => t.TermsOfPayment)
            //    .WithMany(t => t.Bills)
            //    .HasForeignKey(d => d.CreditTermsId);
            //this.HasRequired(t => t.Entity)
            //    .WithMany(t => t.Bills)
            //.HasForeignKey(d => d.EntityId);

        }
    }
}
