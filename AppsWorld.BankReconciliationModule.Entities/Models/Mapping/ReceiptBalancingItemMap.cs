using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AppsWorld.BankReconciliationModule.Entities;

namespace AppsWorld.BankReconciliationModule.Entities.Models.Mappings
{
    public class ReceiptBalancingItemMap : EntityTypeConfiguration<ReceiptBalancingItem>
    {
        public ReceiptBalancingItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ReciveORPay)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Account)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TaxCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.TaxType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Currency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("ReceiptBalancingItem", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ReceiptId).HasColumnName("ReceiptId");
            this.Property(t => t.ReciveORPay).HasColumnName("ReciveORPay");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.Account).HasColumnName("Account");
            this.Property(t => t.IsDisAllow).HasColumnName("IsDisAllow");
            this.Property(t => t.TaxId).HasColumnName("TaxId");
            this.Property(t => t.TaxCode).HasColumnName("TaxCode");
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
            this.Property(t => t.TaxType).HasColumnName("TaxType");
            this.Property(t => t.Currency).HasColumnName("Currency");
            this.Property(t => t.DocAmount).HasColumnName("DocAmount");
            this.Property(t => t.DocTaxAmount).HasColumnName("DocTaxAmount");
            this.Property(t => t.DocTotalAmount).HasColumnName("DocTotalAmount");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");

            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.ReceiptBalancingItems)
            //    .HasForeignKey(d => d.COAId);
            //this.HasRequired(t => t.Receipt)
            //    .WithMany(t => t.ReceiptBalancingItems)
            //    .HasForeignKey(d => d.ReceiptId);
            this.HasRequired(t => t.TaxCode1)
                .WithMany(t => t.ReceiptBalancingItems)
                .HasForeignKey(d => d.TaxId);

        }
    }
}
