using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    public class InvoiceCompactMap : EntityTypeConfiguration<InvoiceCompact>
    {
        public InvoiceCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.Nature)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(20);

	        this.Property(t => t.ExchangeRate)
		        .HasPrecision(15,10);

            this.Property(t => t.GSTExchangeRate)
                .HasPrecision(15, 10);

            this.Property(t => t.DocDescription)
               .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Invoice", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.Nature).HasColumnName("Nature");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.BalanceAmount).HasColumnName("BalanceAmount");
            this.Property(t => t.AllocatedAmount).HasColumnName("AllocatedAmount");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.InternalState).HasColumnName("InternalState");
            this.Property(t => t.IsOBInvoice).HasColumnName("IsOBInvoice");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.GSTExchangeRate).HasColumnName("GSTExchangeRate");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.IsWorkFlowInvoice).HasColumnName("IsWorkFlowInvoice");
            this.Property(t => t.OpeningBalanceId).HasColumnName("OpeningBalanceId");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.CreditTermsId).HasColumnName("CreditTermsId");
            this.Property(t => t.ExCurrency).HasColumnName("ExCurrency");
            this.Property(t => t.GSTExCurrency).HasColumnName("GSTExCurrency");
            this.Property(t => t.IsGstSettings).HasColumnName("IsGstSettings");
            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.PONo).HasColumnName("PONo");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.BaseBalanceAmount).HasColumnName("BaseBalanceAmount");
            this.Property(t => t.BaseGrandTotal).HasColumnName("BaseGrandTotal");
            this.Property(t => t.RoundingAmount).HasColumnName("RoundingAmount");
        }
    }
}