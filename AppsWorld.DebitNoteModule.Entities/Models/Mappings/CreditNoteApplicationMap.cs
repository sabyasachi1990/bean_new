using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;


namespace AppsWorld.DebitNoteModule.Entities.Mapping
{
    public class CreditNoteApplicationMap : EntityTypeConfiguration<CreditNoteApplication>
    {
        public CreditNoteApplicationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.CreditNoteApplicationNumber)
                .HasMaxLength(50);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("CreditNoteApplication", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.CreditNoteApplicationDate).HasColumnName("CreditNoteApplicationDate");
            this.Property(t => t.CreditNoteApplicationResetDate).HasColumnName("CreditNoteApplicationResetDate");
            this.Property(t => t.IsNoSupportingDocumentActivated).HasColumnName("IsNoSupportingDocumentActivated");
            this.Property(t => t.IsNoSupportingDocument).HasColumnName("IsNoSupportingDocument");
            this.Property(t => t.CreditAmount).HasColumnName("CreditAmount");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.CreditNoteApplicationNumber).HasColumnName("CreditNoteApplicationNumber");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");
			this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");

            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.CreditNoteApplications)
            //    .HasForeignKey(d => d.CompanyId);
            //this.HasRequired(t => t.Invoice)
            //    .WithMany(t => t.CreditNoteApplications)
            //    .HasForeignKey(d => d.InvoiceId);

        }
    }
}
