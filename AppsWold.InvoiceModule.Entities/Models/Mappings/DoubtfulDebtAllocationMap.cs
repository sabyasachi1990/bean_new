using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;


namespace AppsWorld.InvoiceModule.Entities.Models.Mappings
{
    public class DoubtfulDebtAllocationMap : EntityTypeConfiguration<DoubtfulDebtAllocation>
    {
        public DoubtfulDebtAllocationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.DoubtfulDebtAllocationNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DoubtfulDebtAllocation", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DoubtfulDebtAllocationDate).HasColumnName("DoubtfulDebtAllocationDate");
            this.Property(t => t.DoubtfulDebtAllocationResetDate).HasColumnName("DoubtfulDebtAllocationResetDate");
            this.Property(t => t.IsNoSupportingDocumentActivated).HasColumnName("IsNoSupportingDocumentActivated");
            this.Property(t => t.IsNoSupportingDocument).HasColumnName("IsNoSupportingDocument");
            this.Property(t => t.AllocateAmount).HasColumnName("AllocateAmount");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DoubtfulDebtAllocationNumber).HasColumnName("DoubtfulDebtAllocationNumber");
            this.Property(t => t.IsRevExcess).HasColumnName("IsRevExcess");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.IsLocked).HasColumnName("IsLocked");

            // Relationships
            //this.HasRequired(t => t.Invoice)
            //    .WithMany(t => t.DoubtfulDebtAllocations)
            //    .HasForeignKey(d => d.InvoiceId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.DoubtfulDebtAllocations)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
