using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.OpeningBalancesModule.Entities
{
    public class InvoiceMap : EntityTypeConfiguration<Invoice>
    {
        public InvoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocSubType)
                .HasMaxLength(20);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(20);

             

            // Table & Column Mappings
            this.ToTable("Invoice", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.Status).HasColumnName("Status");
            //this.Property(t => t.IsGSTDeRegistration).HasColumnName("IsGSTDeRegistration");
            //this.Property(t => t.GSTDeRegistrationDate).HasColumnName("GSTDeRegistrationDate");



            //// Relationships
            //this.HasRequired(t => t.BeanEntity)
            //    .WithMany(t => t.Invoices)
            //    .HasForeignKey(d => d.EntityId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Invoices)
            //    .HasForeignKey(d => d.CompanyId);
            //this.HasRequired(t => t.TermsOfPayment)
            //    .WithMany(t => t.Invoices)
            //    .HasForeignKey(d => d.CreditTermsId);

        }
    }
}
