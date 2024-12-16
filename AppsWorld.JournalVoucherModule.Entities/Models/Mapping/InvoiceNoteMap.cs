using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class InvoiceNoteMap : EntityTypeConfiguration<InvoiceNote>
    {
        public InvoiceNoteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Notes)
                .HasMaxLength(254);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("InvoiceNote", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
            this.Property(t => t.ExpectedPaymentDate).HasColumnName("ExpectedPaymentDate");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");


            // Relationships
            //this.HasRequired(t => t.Invoice)
            //    .WithMany(t => t.InvoiceNotes)
            //    .HasForeignKey(d => d.InvoiceId);

        }
    }
}
