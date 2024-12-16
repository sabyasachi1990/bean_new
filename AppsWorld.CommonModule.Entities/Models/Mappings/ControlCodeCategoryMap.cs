using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CommonModule.Entities.Mapping
{
    public class ControlCodeCategoryMap : EntityTypeConfiguration<ControlCodeCategory>
    {
        public ControlCodeCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ControlCodeCategoryCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ControlCodeCategoryDescription)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.DataType)
                .HasMaxLength(10);

            this.Property(t => t.Format)
                .HasMaxLength(20);

            this.Property(t => t.Remarks)
                .HasMaxLength(254);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.ModuleNamesUsing)
                .HasMaxLength(1000);

            this.Property(t => t.DefaultValue)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ControlCodeCategory", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.ControlCodeCategoryCode).HasColumnName("ControlCodeCategoryCode");
            this.Property(t => t.ControlCodeCategoryDescription).HasColumnName("ControlCodeCategoryDescription");
            this.Property(t => t.DataType).HasColumnName("DataType");
            this.Property(t => t.Format).HasColumnName("Format");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ModuleNamesUsing).HasColumnName("ModuleNamesUsing");
            this.Property(t => t.DefaultValue).HasColumnName("DefaultValue");

            

        }
    }
}
