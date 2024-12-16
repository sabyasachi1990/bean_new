using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BillModule.Entities
{
    public class BillDetailMap : EntityTypeConfiguration<BillDetail>
    {
        public BillDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Account)
            //    .IsRequired()
            //    .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(256);

            this.Property(t => t.TaxCode)
                
                .HasMaxLength(20);

            this.Property(t => t.TaxType)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("BillDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BillId).HasColumnName("BillId");
            //this.Property(t => t.Account).HasColumnName("Account");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.IsDisallow).HasColumnName("IsDisallow");
            this.Property(t => t.TaxId).HasColumnName("TaxId");
            this.Property(t => t.TaxCode).HasColumnName("TaxCode");
            this.Property(t => t.TaxType).HasColumnName("TaxType");
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
            this.Property(t => t.DocAmount).HasColumnName("DocAmount");
            this.Property(t => t.DocTaxAmount).HasColumnName("DocTaxAmount");
            this.Property(t => t.DocTotalAmount).HasColumnName("DocTotalAmount");
            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount");
            this.Property(t => t.BaseTaxAmount).HasColumnName("BaseTaxAmount");
            this.Property(t => t.BaseTotalAmount).HasColumnName("BaseTotalAmount");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.IsPLAccount).HasColumnName("IsPLAccount");
            this.Property(t => t.TaxIdCode).HasColumnName("TaxIdCode");
            this.Property(t => t.ClearingState).HasColumnName("ClearingState");

            // Relationships
            //this.HasRequired(t => t.Bill)
            //    .WithMany(t => t.BillDetails)
            //    .HasForeignKey(d => d.BillId);
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.BillDetails)
            //    .HasForeignKey(d => d.COAId);
            //this.HasOptional(t => t.TaxCode1)
            //    .WithMany(t => t.BillDetails)
            //    .HasForeignKey(d => d.TaxId);

        }
    }
}
