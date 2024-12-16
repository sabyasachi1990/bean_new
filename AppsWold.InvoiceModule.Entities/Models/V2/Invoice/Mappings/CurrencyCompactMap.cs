using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    public class CurrencyCompactMap : EntityTypeConfiguration<CurrencyCompact>
    {
        public CurrencyCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DefaultValue)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Currency", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DefaultValue).HasColumnName("DefaultValue");



        }
    }
}
