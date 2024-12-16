using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CommonModule.Entities.V2
{
    public class CompanyKMap : EntityTypeConfiguration<CompanyK>
    {
        public CompanyKMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(254);

            this.Property(t => t.ShortName)
                .HasMaxLength(5);

             
            // Table & Column Mappings
            this.ToTable("Company", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
        }
    }
}
