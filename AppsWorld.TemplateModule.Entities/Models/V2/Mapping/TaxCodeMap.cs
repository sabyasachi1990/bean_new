using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Entities.Models.V2.Mapping
{
    public class TaxCodeMap : EntityTypeConfiguration<AppsWorld.TemplateModule.Entities.Models.V2.TaxCode>
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

         

            // Table & Column Mappings
            this.ToTable("TaxCode", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
      
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
         
            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.TaxCodes)
            //    .HasForeignKey(d => d.CompanyId);
        }
    }
}
