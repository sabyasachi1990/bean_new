using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BillModule.Entities
{
    public class GSTSettingMap : EntityTypeConfiguration<GSTSetting>
    {
        public GSTSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Number)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ReportingYearEnd)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.ReportingInterval)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.GSTRepoCurrency)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("GSTSetting", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Number).HasColumnName("Number");
            this.Property(t => t.DateOfRegistration).HasColumnName("DateOfRegistration");
            this.Property(t => t.DeRegistration).HasColumnName("DeRegistration");
            this.Property(t => t.IsDeregistered).HasColumnName("IsDeregistered");
            this.Property(t => t.ReportingYearEnd).HasColumnName("ReportingYearEnd");
            this.Property(t => t.ReportingInterval).HasColumnName("ReportingInterval");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.GSTRepoCurrency).HasColumnName("GSTRepoCurrency");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");

            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.GSTSettings)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
