using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    public class CreditNoteApplicationDetailMap : EntityTypeConfiguration<CreditNoteApplicationDetail>
    {
        public CreditNoteApplicationDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocumentType)
                .HasMaxLength(20);

            this.Property(t => t.DocCurrency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.BaseCurrencyExchangeRate)
                .HasPrecision(15,10);

            // Table & Column Mappings
            this.ToTable("CreditNoteApplicationDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreditNoteApplicationId).HasColumnName("CreditNoteApplicationId");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.DocumentType).HasColumnName("DocumentType");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.CreditAmount).HasColumnName("CreditAmount");
            this.Property(t => t.BaseCurrencyExchangeRate).HasColumnName("BaseCurrencyExchangeRate");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.TaxId).HasColumnName("TaxId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.TaxAmount).HasColumnName("TaxAmount");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.TaxIdCode).HasColumnName("TaxIdCode");
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.ServiceEntityId).HasColumnName("ServiceEntityId");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.RoundingAmount).HasColumnName("RoundingAmount");

        }
    }
}
