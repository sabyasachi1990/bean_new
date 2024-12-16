using System.Data.Entity.ModelConfiguration;
namespace AppsWorld.InvoiceModule.Entities.Models.Mappings
{
    public class PaymentCompactMap : EntityTypeConfiguration<PaymentCompact>
    {
        public PaymentCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.PaymentApplicationCurrency)
                .HasMaxLength(5);

            this.Property(t => t.DocCurrency)
                .IsRequired()
                .HasMaxLength(5);
            // Table & Column Mappings
            this.ToTable("Payment", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.PaymentApplicationCurrency).HasColumnName("PaymentApplicationCurrency");
            this.Property(t => t.PaymentApplicationAmmount).HasColumnName("PaymentApplicationAmmount");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
        }
    }
}
