using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;


namespace AppsWorld.DebitNoteModule.Entities.Mapping
{
    public class CreditNoteApplicationDetailMap : EntityTypeConfiguration<CreditNoteApplicationDetail>
    {
        public CreditNoteApplicationDetailMap()
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
            this.ToTable("CreditNoteApplicationDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreditNoteApplicationId).HasColumnName("CreditNoteApplicationId");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.DocumentType).HasColumnName("DocumentType");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.CreditAmount).HasColumnName("CreditAmount");
            this.Property(t => t.BaseCurrencyExchangeRate).HasColumnName("BaseCurrencyExchangeRate");
            this.Property(t => t.SegmentCategory1).HasColumnName("SegmentCategory1");
            this.Property(t => t.SegmentCategory2).HasColumnName("SegmentCategory2");

            // Relationships
            //this.HasRequired(t => t.CreditNoteApplication)
            //    .WithMany(t => t.CreditNoteApplicationDetails)
            //    .HasForeignKey(d => d.CreditNoteApplicationId);

        }
    }
}
