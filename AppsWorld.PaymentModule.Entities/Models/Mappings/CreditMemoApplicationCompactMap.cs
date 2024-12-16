using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Entities.Mappings
{
    public class CreditMemoApplicationCompactMap : EntityTypeConfiguration<CreditMemoApplicationCompact>
    {
        public CreditMemoApplicationCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.CreditMemoApplicationNumber)
                .HasMaxLength(50);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("CreditMemoApplication", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreditMemoId).HasColumnName("CreditMemoId");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.CreditMemoApplicationDate).HasColumnName("CreditMemoApplicationDate");
            this.Property(t => t.CreditMemoApplicationResetDate).HasColumnName("CreditMemoApplicationResetDate");
            this.Property(t => t.IsNoSupportingDocumentActivated).HasColumnName("IsNoSupportingDocumentActivated");
            this.Property(t => t.IsNoSupportingDocument).HasColumnName("IsNoSupportingDocument");
            this.Property(t => t.CreditAmount).HasColumnName("CreditAmount");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.CreditMemoApplicationNumber).HasColumnName("CreditMemoApplicationNumber");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");

        }
    }
}
