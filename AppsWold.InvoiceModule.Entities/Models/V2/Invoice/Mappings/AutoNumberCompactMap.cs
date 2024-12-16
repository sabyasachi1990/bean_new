using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    public class AutoNumberCompactMap : EntityTypeConfiguration<AutoNumberCompact>
    {
        public AutoNumberCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.EntityType)
                .IsRequired()
                .HasMaxLength(100);

             
            this.Property(t => t.Format)
                .HasMaxLength(100);

            this.Property(t => t.GeneratedNumber)
                .HasMaxLength(50);

            this.Property(t => t.Reset)
                .HasMaxLength(20);

            this.Property(t => t.Preview)
                .HasMaxLength(50);
            
            // Table & Column Mappings
            //this.ToTable("AutoNumber", "Common");
            this.ToTable("AutoNumber", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.Format).HasColumnName("Format");
            this.Property(t => t.GeneratedNumber).HasColumnName("GeneratedNumber");
            this.Property(t => t.CounterLength).HasColumnName("CounterLength");
            this.Property(t => t.Reset).HasColumnName("Reset");
            this.Property(t => t.Preview).HasColumnName("Preview");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.IsEditable).HasColumnName("IsEditable");
            this.Property(t => t.IsDisable).HasColumnName("IsDisable");

        }
    }
}
