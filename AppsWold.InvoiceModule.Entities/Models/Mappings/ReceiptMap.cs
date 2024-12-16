using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.InvoiceModule.Entities.Models.Mappings
{
	public class ReceiptMap : EntityTypeConfiguration<Receipt>
	{
		public ReceiptMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.DocSubType)
				.IsRequired()
				.HasMaxLength(20);
			this.Property(t => t.SystemRefNo)
				.IsRequired()
				.HasMaxLength(50);
			this.Property(t => t.DocNo)
				.IsRequired()
				.HasMaxLength(25);
			// Table & Column Mappings
			this.ToTable("Receipt", "Bean");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.CompanyId).HasColumnName("CompanyId");
			this.Property(t => t.DocSubType).HasColumnName("DocSubType");
			this.Property(t => t.SystemRefNo).HasColumnName("SystemRefNo");
			this.Property(t => t.DocDate).HasColumnName("DocDate");
			this.Property(t => t.DocNo).HasColumnName("DocNo");
		    this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.ReceiptApplicationAmmount).HasColumnName("ReceiptApplicationAmmount");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.BankReceiptAmmountCurrency).HasColumnName("BankReceiptAmmountCurrency");
            this.Property(t => t.BankReceiptAmmount).HasColumnName("BankReceiptAmmount");
            this.Property(t => t.ModeOfReceipt).HasColumnName("ModeOfReceipt");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
			this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");




		}
    }
}
