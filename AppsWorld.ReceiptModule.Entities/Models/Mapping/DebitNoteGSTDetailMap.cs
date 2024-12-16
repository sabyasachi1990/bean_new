using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;


namespace AppsWorld.ReceiptModule.Entities.Mapping
{
    //public class DebitNoteGSTDetailMap : EntityTypeConfiguration<DebitNoteGSTDetail>
    //{
    //    public DebitNoteGSTDetailMap()
    //    {
    //        // Primary Key
    //        this.HasKey(t => t.Id);

    //        // Properties
    //        // Table & Column Mappings
    //        this.ToTable("DebitNoteGSTDetail", "Bean");
    //        this.Property(t => t.Id).HasColumnName("Id");
    //        this.Property(t => t.DebitNoteId).HasColumnName("DebitNoteId");
    //        this.Property(t => t.TaxId).HasColumnName("TaxId");
    //        this.Property(t => t.Amount).HasColumnName("Amount");
    //        this.Property(t => t.TaxAmount).HasColumnName("TaxAmount");
    //        this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");

    //        // Relationships
    //        //this.HasRequired(t => t.DebitNote)
    //        //   .WithMany(t => t.DebitNoteGSTDetails)
    //        //   .HasForeignKey(d => d.DebitNoteId);
    //        this.HasRequired(t => t.TaxCode)
    //            .WithMany(t => t.DebitNoteGSTDetails)
    //            .HasForeignKey(d => d.TaxId);

    //    }
    //}
}
