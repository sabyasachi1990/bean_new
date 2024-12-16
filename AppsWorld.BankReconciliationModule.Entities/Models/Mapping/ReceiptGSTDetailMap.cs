using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AppsWorld.BankReconciliationModule.Entities;

namespace AppsWorld.BankReconciliationModule.Entities.Models.Mappings
{
    //public class ReceiptGSTDetailMap : EntityTypeConfiguration<ReceiptGSTDetail>
    //{
    //    public ReceiptGSTDetailMap()
    //    {
    //        // Primary Key
    //        this.HasKey(t => t.Id);

    //        // Properties
    //        this.Property(t => t.TaxCode)
    //            .IsRequired()
    //            .HasMaxLength(20);

    //        // Table & Column Mappings
    //        this.ToTable("ReceiptGSTDetail", "Bean");
    //        this.Property(t => t.Id).HasColumnName("Id");
    //        this.Property(t => t.ReceiptId).HasColumnName("ReceiptId");
    //        this.Property(t => t.TaxId).HasColumnName("TaxId");
    //        this.Property(t => t.TaxCode).HasColumnName("TaxCode");
    //        this.Property(t => t.Amount).HasColumnName("Amount");
    //        this.Property(t => t.TaxAmount).HasColumnName("TaxAmount");
    //        this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");

    //        // Relationships
    //        //this.HasRequired(t => t.Receipt)
    //        //    .WithMany(t => t.ReceiptGSTDetails)
    //        //    .HasForeignKey(d => d.ReceiptId);
    //        //this.HasRequired(t => t.TaxCode1)
    //        //    .WithMany(t => t.ReceiptGSTDetails)
    //        //    .HasForeignKey(d => d.TaxId);

    //    }
    //}
}
