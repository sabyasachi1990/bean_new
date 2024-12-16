using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
   // public class InvoiceGSTDetailMap : EntityTypeConfiguration<InvoiceGSTDetail>
   // {
   //     public InvoiceGSTDetailMap()
   //     {
   //         // Primary Key
   //         this.HasKey(t => t.Id);

   //         // Properties
   //         // Table & Column Mappings
   //         this.ToTable("InvoiceGSTDetail", "Bean");
   //         this.Property(t => t.Id).HasColumnName("Id");
   //         this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
   //         this.Property(t => t.TaxId).HasColumnName("TaxId");
   //         this.Property(t => t.Amount).HasColumnName("Amount");
   //         this.Property(t => t.TaxAmount).HasColumnName("TaxAmount");
   //         this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");

   //         //Relationships
   //         //this.HasRequired(t => t.Invoice)
   //         //    .WithMany(t => t.InvoiceGSTDetails)
   //         //    .HasForeignKey(d => d.InvoiceId);

			////this.HasRequired(t => t.TaxCode)
			////	.WithMany(t => t.InvoiceGSTDetails)
			////	.HasForeignKey(d => d.TaxId);

   //     }
   // }
}
