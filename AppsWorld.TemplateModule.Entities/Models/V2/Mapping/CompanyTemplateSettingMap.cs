using AppsWorld.TemplateModule.Entities.Models.V2;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Entities.Models.V2.Mapping
{
    public class CompanyTemplateSettingMap : EntityTypeConfiguration<CompanyTemplateSettings>
    {
        public CompanyTemplateSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.HeaderContent)
                .HasMaxLength(4000);

            this.Property(t => t.FooterContent)
                .HasMaxLength(4000);

            this.Property(t => t.Remarks)
                .HasMaxLength(254);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("CompanyTemplateSettings", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.HeaderContent).HasColumnName("HeaderContent");
            this.Property(t => t.FooterContent).HasColumnName("FooterContent");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");

            //// Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.CompanyTemplateSettings)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
