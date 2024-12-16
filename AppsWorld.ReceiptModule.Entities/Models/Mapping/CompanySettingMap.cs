using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.ReceiptModule.Entities.Mapping
{
    public class CompanySettingMap : EntityTypeConfiguration<CompanySetting>
    {
        public CompanySettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //this.Property(t => t.CursorName)
            //    .HasMaxLength(100);

            this.Property(t => t.ModuleName)
                .HasMaxLength(100);

            //this.Property(t => t.UserCreated)
            //    .HasMaxLength(254);

            //this.Property(t => t.ModifiedBy)
            //    .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("CompanySetting", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            //this.Property(t => t.CursorName).HasColumnName("CursorName");
            this.Property(t => t.ModuleName).HasColumnName("ModuleName");
            //this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            //this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            //this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            //this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.IsEnabled).HasColumnName("IsEnabled");

            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.CompanySettings)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
