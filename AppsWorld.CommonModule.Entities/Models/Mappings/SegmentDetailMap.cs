using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CommonModule.Entities.Mapping
{
    //public class SegmentDetailMap : EntityTypeConfiguration<SegmentDetail>
    //{
    //    public SegmentDetailMap()
    //    {
    //        // Primary Key
    //        this.HasKey(t => t.Id);

    //        // Properties
    //        this.Property(t => t.Id)
    //            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

    //        this.Property(t => t.Name)
    //            .IsRequired()
    //            .HasMaxLength(100);

    //        this.Property(t => t.Remarks)
    //            .HasMaxLength(256);

    //        this.Property(t => t.UserCreated)
    //            .HasMaxLength(254);

    //        this.Property(t => t.ModifiedBy)
    //            .HasMaxLength(254);

    //        // Table & Column Mappings
    //        this.ToTable("SegmentDetail", "Bean");
    //        this.Property(t => t.Id).HasColumnName("Id");
    //        this.Property(t => t.SegmentMasterId).HasColumnName("SegmentMasterId");
    //        this.Property(t => t.Name).HasColumnName("Name");
    //        this.Property(t => t.ParentId).HasColumnName("ParentId");
    //        this.Property(t => t.IsSystem).HasColumnName("IsSystem");
    //        this.Property(t => t.RecOrder).HasColumnName("RecOrder");
    //        this.Property(t => t.Remarks).HasColumnName("Remarks");
    //        this.Property(t => t.UserCreated).HasColumnName("UserCreated");
    //        this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
    //        this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
    //        this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
    //        this.Property(t => t.Version).HasColumnName("Version");
    //        this.Property(t => t.Status).HasColumnName("Status");

    //        // Relationships
    //        this.HasRequired(t => t.SegmentMaster)
    //            .WithMany(t => t.SegmentDetails)
    //            .HasForeignKey(d => d.SegmentMasterId);

    //    }
    //}
}
