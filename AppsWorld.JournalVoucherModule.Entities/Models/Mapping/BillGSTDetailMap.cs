using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
    //public class BillGSTDetailMap : EntityTypeConfiguration<BillGSTDetail>
    //{
    //    public BillGSTDetailMap()
    //    {
    //        // Primary Key
    //        this.HasKey(t => t.Id);

    //        // Properties
    //        this.Property(t => t.TaxCode)
    //            .IsRequired()
    //            .HasMaxLength(20);

    //        // Table & Column Mappings
    //        this.ToTable("BillGSTDetail", "Bean");
    //        this.Property(t => t.Id).HasColumnName("Id");
    //        this.Property(t => t.BillId).HasColumnName("BillId");
    //        this.Property(t => t.TaxId).HasColumnName("TaxId");
    //        this.Property(t => t.TaxCode).HasColumnName("TaxCode");
    //        this.Property(t => t.Amount).HasColumnName("Amount");
    //        this.Property(t => t.TaxAmount).HasColumnName("TaxAmount");
    //        this.Property(t => t.Total).HasColumnName("Total");

    //        // Relationships
    //        //this.HasRequired(t => t.Bill)
    //        //    .WithMany(t => t.BillGSTDetails)
    //        //    .HasForeignKey(d => d.BillId);
    //        //this.HasRequired(t => t.TaxCode1)
    //        //    .WithMany(t => t.BillGSTDetails)
    //        //    .HasForeignKey(d => d.TaxId);

    //    }
    //}
}
