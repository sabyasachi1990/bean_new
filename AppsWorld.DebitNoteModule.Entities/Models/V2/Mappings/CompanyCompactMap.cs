using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.DebitNoteModule.Entities.V2
{
    public class CompanyCompactMap : EntityTypeConfiguration<CompanyCompact>
    {
        public CompanyCompactMap()
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
            this.Property(t => t.IsGstSetting).HasColumnName("IsGstSetting");
        }
    }
}
