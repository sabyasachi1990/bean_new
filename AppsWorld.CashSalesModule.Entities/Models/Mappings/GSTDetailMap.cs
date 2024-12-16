using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CashSalesModule.Entities.Models.Mappings
{
    //public class GSTDetailMap : EntityTypeConfiguration<GSTDetail>
    //{
    //    public GSTDetailMap()
    //    {
    //        // Primary Key
    //        this.HasKey(t => t.Id);

    //        // Properties
    //        this.Property(t => t.DocType)
    //            .HasMaxLength(50);

    //        // Table & Column Mappings
    //        this.ToTable("GSTDetail", "Common");
    //        this.Property(t => t.Id).HasColumnName("Id");
    //        this.Property(t => t.DocType).HasColumnName("DocType");
    //        this.Property(t => t.DocId).HasColumnName("DocId");
    //        this.Property(t => t.ModuleMasterId).HasColumnName("ModuleMasterId");
    //        this.Property(t => t.Amount).HasColumnName("Amount");
    //        this.Property(t => t.TaxAmount).HasColumnName("TaxAmount");
    //        this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
    //        this.Property(t => t.TaxId).HasColumnName("TaxId");

    //        // Relationships
    //        //this.HasOptional(t => t.TaxCode)
    //        //    .WithMany(t => t.GSTDetails)
    //        //    .HasForeignKey(d => d.TaxId);
    //        //this.HasRequired(t => t.ModuleMaster)
    //        //    .WithMany(t => t.GSTDetails)
    //        //    .HasForeignKey(d => d.ModuleMasterId);

    //    }
    //}
}
