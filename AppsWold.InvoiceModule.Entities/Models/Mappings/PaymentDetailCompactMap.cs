using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.InvoiceModule.Entities.Models.Mappings
{
    public class PaymentDetailCompactMap : EntityTypeConfiguration<PaymentDetailCompact>
    {
        public PaymentDetailCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocumentType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DocumentNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.Currency)
                .IsRequired()
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("PaymentDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PaymentId).HasColumnName("PaymentId");
            this.Property(t => t.DocumentType).HasColumnName("DocumentType");
            this.Property(t => t.DocumentNo).HasColumnName("DocumentNo");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.Currency).HasColumnName("Currency");
            this.Property(t => t.PaymentAmount).HasColumnName("PaymentAmount");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            // Relationships
            //this.HasRequired(t => t.Payment)
            //    .WithMany(t => t.PaymentDetails)
            //    .HasForeignKey(d => d.PaymentId);

        }
    }
}
