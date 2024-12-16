using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Entities.Models.Mapping
{
    public class TaxCodeMap : EntityTypeConfiguration<TaxCode>
    {
        public TaxCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1000);

            this.Property(t => t.TaxType)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TaxCode", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.TaxType).HasColumnName("TaxType");
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.EffectiveTo).HasColumnName("EffectiveTo");
            this.Property(t => t.EffectiveFrom).HasColumnName("EffectiveFrom");
            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.TaxCodes)
            //    .HasForeignKey(d => d.CompanyId);
        }
    }
}
