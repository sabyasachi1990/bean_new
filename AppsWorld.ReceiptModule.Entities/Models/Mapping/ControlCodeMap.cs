using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.ReceiptModule.Entities.Mapping
{
    public class ControlCodeMap : EntityTypeConfiguration<ControlCode>
    {
        public ControlCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CodeKey)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CodeValue)
                .IsRequired()
                .HasMaxLength(100);

            //this.Property(t => t.IsSystem)
            //    .HasMaxLength(10);

            //this.Property(t => t.Remarks)
            //    .HasMaxLength(254);

            //this.Property(t => t.UserCreated)
            //    .HasMaxLength(254);

            //this.Property(t => t.ModifiedBy)
            //    .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("ControlCode", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ControlCategoryId).HasColumnName("ControlCategoryId");
            this.Property(t => t.CodeKey).HasColumnName("CodeKey");
            this.Property(t => t.CodeValue).HasColumnName("CodeValue");
            //this.Property(t => t.IsSystem).HasColumnName("IsSystem");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            //this.Property(t => t.Remarks).HasColumnName("Remarks");
            //this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            //this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            //this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            //this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            //this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            //this.Property(t => t.IsDefault).HasColumnName("IsDefault");

            // Relationships
            this.HasRequired(t => t.ControlCodeCategory)
                .WithMany(t => t.ControlCodes)
                .HasForeignKey(d => d.ControlCategoryId);

        }
    }
}
