using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BillModule.Entities
{
    public class CreditMemoApplicationDetailMap : EntityTypeConfiguration<CreditMemoApplicationDetail>
    {
        public CreditMemoApplicationDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocumentType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DocCurrency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.SegmentCategory1)
                .HasMaxLength(100);

            this.Property(t => t.SegmentCategory2)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("CreditMemoApplicationDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreditMemoApplicationId).HasColumnName("CreditMemoApplicationId");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.DocumentType).HasColumnName("DocumentType");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.CreditAmount).HasColumnName("CreditAmount");
            this.Property(t => t.BaseCurrencyExchangeRate).HasColumnName("BaseCurrencyExchangeRate");
            this.Property(t => t.SegmentCategory1).HasColumnName("SegmentCategory1");
            this.Property(t => t.SegmentCategory2).HasColumnName("SegmentCategory2");

            // Relationships
            //this.HasRequired(t => t.CreditMemoApplication)
            //    .WithMany(t => t.CreditMemoApplicationDetails)
            //    .HasForeignKey(d => d.CreditMemoApplicationId);

        }
    }
}
