using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CommonModule.Entities.Mapping
{
    public class DocumentHistoryMap : EntityTypeConfiguration<DocumentHistory>
    {
        public DocumentHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Remarks)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("DocumentHistory", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DocState).HasColumnName("DocState");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.TransactionId).HasColumnName("TransactionId");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount");
            this.Property(t => t.DocAmount).HasColumnName("DocAmount");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.StateChangedBy).HasColumnName("StateChangedBy");
            this.Property(t => t.DocBalanaceAmount).HasColumnName("DocBalanaceAmount");
            this.Property(t => t.BaseBalanaceAmount).HasColumnName("BaseBalanaceAmount");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
        }
    }
}
