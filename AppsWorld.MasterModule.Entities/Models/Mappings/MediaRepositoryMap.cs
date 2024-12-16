using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class MediaRepositoryMap : EntityTypeConfiguration<MediaRepository>
    {
        public MediaRepositoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SourceType)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.MediaType)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Original)
                .HasMaxLength(1000);

            this.Property(t => t.Small)
                .HasMaxLength(1000);

            this.Property(t => t.Medium)
                .HasMaxLength(1000);

            this.Property(t => t.Large)
                .HasMaxLength(1000);

            this.Property(t => t.CssSprite)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MediaRepository", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.SourceType).HasColumnName("SourceType");
            this.Property(t => t.MediaType).HasColumnName("MediaType");
            this.Property(t => t.Original).HasColumnName("Original");
            this.Property(t => t.Small).HasColumnName("Small");
            this.Property(t => t.Medium).HasColumnName("Medium");
            this.Property(t => t.Large).HasColumnName("Large");
            this.Property(t => t.CssSprite).HasColumnName("CssSprite");
            this.Property(t => t.Status).HasColumnName("Status");
        }
    }
}
