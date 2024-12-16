using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
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
                .HasMaxLength(25);

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
