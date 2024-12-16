using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AppsWorld.BankReconciliationModule.Entities;

namespace AppsWorld.BankReconciliationModule.Entities.Models.Mappings
{
    public class BankReconciliationClearingDetailMap : EntityTypeConfiguration<BankReconciliationClearingDetail>
    {
        public BankReconciliationClearingDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("BankReconciliationClearingDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BankReconciliationId).HasColumnName("BankReconciliationId");
            this.Property(t => t.ReceiptId).HasColumnName("ReceiptId");
            this.Property(t => t.PaymentId).HasColumnName("PaymentId");
            this.Property(t => t.IsDeposit).HasColumnName("IsDeposit");
            this.Property(t => t.ClearingDate).HasColumnName("ClearingDate");

            // Relationships
            //this.HasRequired(t => t.BankReconciliation)
            //    .WithMany(t => t.BankReconciliationClearingDetails)
            //    .HasForeignKey(d => d.BankReconciliationId);
            //this.HasOptional(t => t.Receipt)
            //    .WithMany(t => t.BankReconciliationClearingDetails)
            //    .HasForeignKey(d => d.ReceiptId);

        }
    }
}
