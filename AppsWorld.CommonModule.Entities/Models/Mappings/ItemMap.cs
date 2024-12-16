using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CommonModule.Entities.Mapping
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

            this.Property(t => t.Description)
                .HasMaxLength(200);

            this.Property(t => t.UOM)
                .HasMaxLength(20);

            this.Property(t => t.Currency)
                .HasMaxLength(5);

            //this.Property(t => t.AllowableDis)
            //    .HasMaxLength(20);

            
            this.Property(t => t.Notes)
                .HasMaxLength(1000);

            this.Property(t => t.Remarks)
                .HasMaxLength(256);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("Item", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.UOM).HasColumnName("UOM");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.Currency).HasColumnName("Currency");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.DefaultTaxcodeId).HasColumnName("DefaultTaxcodeId");
            this.Property(t => t.AllowableDis).HasColumnName("AllowableDis");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.IsSaleItem).HasColumnName("IsSaleItem");

            this.Property(t => t.IsPurchaseItem).HasColumnName("IsPurchaseItem");
            this.Property(t => t.IsAccountEditable).HasColumnName("IsAccountEditable");

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
