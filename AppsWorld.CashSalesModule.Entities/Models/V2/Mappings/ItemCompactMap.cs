using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CashSalesModule.Entities.V2
{
    public class ItemCompactMap : EntityTypeConfiguration<ItemCompact>
    {
        public ItemCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Description)
                .HasMaxLength(200);

            

            // Table & Column Mappings
            this.ToTable("Item", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Status).HasColumnName("Status");
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
