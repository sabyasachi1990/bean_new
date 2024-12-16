using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CashSalesModule.Entities.Models.Mappings
{
    public class AutoNumberCompanyMap : EntityTypeConfiguration<AutoNumberCompany>
    {
        public AutoNumberCompanyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.GeneratedNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AutoNumberCompany", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SubsideryCompanyId).HasColumnName("SubsideryCompanyId");
            this.Property(t => t.AutonumberId).HasColumnName("AutonumberId");
            this.Property(t => t.GeneratedNumber).HasColumnName("GeneratedNumber");

            // Relationships
            this.HasOptional(t => t.AutoNumber)
                .WithMany(t => t.AutoNumberCompanies)
                .HasForeignKey(d => d.AutonumberId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.AutoNumberCompanies)
            //    .HasForeignKey(d => d.SubsideryCompanyId);

        }
    }
}
