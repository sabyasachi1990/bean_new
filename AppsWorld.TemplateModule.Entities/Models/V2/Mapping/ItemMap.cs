using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.V2.Mapping
{
    public class ItemMap : EntityTypeConfiguration<Item>
    {
        public ItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(20);

           

            // Table & Column Mappings
            this.ToTable("Item", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Code).HasColumnName("Code");
            

            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.Items)
            //    .HasForeignKey(d => d.COAId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Items)
            //    .HasForeignKey(d => d.CompanyId);
            //this.HasOptional(t => t.TaxCode)
            //    .WithMany(t => t.Items)
            //    .HasForeignKey(d => d.DefaultTaxcodeId);

        }
    }
}
