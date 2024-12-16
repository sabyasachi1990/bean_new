using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class CurrencyMap : EntityTypeConfiguration<Currency>
    {
        public CurrencyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("Currency", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Currencies)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
