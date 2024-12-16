using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;


namespace AppsWorld.BillModule.Entities
{
    public class DebitNoteNoteMap : EntityTypeConfiguration<DebitNoteNote>
    {
        public DebitNoteNoteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Notes)
                .HasMaxLength(256);

            this.Property(t => t.UserCreated_)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("DebitNoteNote", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DebitNoteId).HasColumnName("DebitNoteId");
            this.Property(t => t.ExpectedPaymentDate).HasColumnName("ExpectedPaymentDate");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UserCreated_).HasColumnName("UserCreated ");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");


            // Relationships
            //this.HasRequired(t => t.DebitNote)
            //      .WithMany(t => t.DebitNoteNotes)
            //      .HasForeignKey(d => d.DebitNoteId);

        }
    }
}
