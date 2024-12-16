using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CommonModule.Entities.Mapping
{
    public class EmployeeMap : EntityTypeConfiguration<Employee>
    {
        public EmployeeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            this.Property(t => t.FirstName)
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .HasMaxLength(100);
 
            // Table & Column Mappings
            this.ToTable("Employee", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Employees)
            //    .HasForeignKey(d => d.CompanyId);
            //this.HasOptional(t => t.MediaRepository)
            //    .WithMany(t => t.Employees)
            //    .HasForeignKey(d => d.PhotoId);

        }
    }
}
