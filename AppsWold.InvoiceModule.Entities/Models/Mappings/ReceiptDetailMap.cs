using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.InvoiceModule.Entities.Models.Mappings
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

          

            this.Property(t => t.Currency)
                .IsRequired()
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("ReceiptDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ReceiptId).HasColumnName("ReceiptId");
            this.Property(t => t.DocumentType).HasColumnName("DocumentType");
            this.Property(t => t.Currency).HasColumnName("Currency");
            this.Property(t => t.ReceiptAmount).HasColumnName("ReceiptAmount");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.DocumentNo).HasColumnName("DocumentNo");

            //this.Property(t => t.BankReceiptAmmount).HasColumnName("BankReceiptAmmount");
            //this.Property(t => t.PaymentAmount).HasColumnName("PaymentAmount");


            // Relationships
            //this.HasRequired(t => t.Receipt)
            //    .WithMany(t => t.ReceiptDetails)
            //    .HasForeignKey(d => d.ReceiptId);

        }
    }
}
