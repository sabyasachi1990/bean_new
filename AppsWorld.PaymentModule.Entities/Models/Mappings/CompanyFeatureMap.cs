using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Entities.Mapping
{
    public class CompanyFeatureMap : EntityTypeConfiguration<CompanyFeature>
    {
        public CompanyFeatureMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CompanyFeatures", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.FeatureId).HasColumnName("FeatureId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Remarks).HasColumnName("Remarks");

            // Relationships
            this.HasRequired(t => t.Feature)
                .WithMany(t => t.CompanyFeatures)
                .HasForeignKey(d => d.FeatureId);

        }
    }
}
