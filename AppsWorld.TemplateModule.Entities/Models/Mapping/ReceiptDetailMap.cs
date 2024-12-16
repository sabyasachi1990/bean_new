using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.Mapping
{
    public class ReceiptDetailMap : EntityTypeConfiguration<ReceiptDetail>
    {
        public ReceiptDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocumentType)
                .IsRequired()
                .HasMaxLength(20);

            //this.Property(t => t.SystemReferenceNumber)
            //    .IsRequired()
            //    .HasMaxLength(50);

            this.Property(t => t.DocumentNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(25);


            //this.Property(t => t.Nature)
            //    .IsRequired()
            //    .HasMaxLength(10);
            this.Property(t => t.Currency)
                .IsRequired()
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("ReceiptDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ReceiptId).HasColumnName("ReceiptId");
           // this.Property(t => t.DocumentDate).HasColumnName("DocumentDate");
            this.Property(t => t.DocumentType).HasColumnName("DocumentType");
          //  this.Property(t => t.SystemReferenceNumber).HasColumnName("SystemReferenceNumber");
            this.Property(t => t.DocumentNo).HasColumnName("DocumentNo");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
           // this.Property(t => t.Nature).HasColumnName("Nature");
           // this.Property(t => t.DocumentAmmount).HasColumnName("DocumentAmmount");
           // this.Property(t => t.AmmountDue).HasColumnName("AmmountDue");
            this.Property(t => t.Currency).HasColumnName("Currency");
            this.Property(t => t.ReceiptAmount).HasColumnName("ReceiptAmount");
           // this.Property(t => t.RecOrder).HasColumnName("RecOrder");

            // Relationships
            //this.HasRequired(t => t.Receipt)
            //    .WithMany(t => t.ReceiptDetails)
            //    .HasForeignKey(d => d.ReceiptId);

        }
    }
}
