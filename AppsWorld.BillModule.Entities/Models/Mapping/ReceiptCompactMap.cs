using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BillModule.Entities
{
	public class ReceiptCompactMap : EntityTypeConfiguration<ReceiptCompact>
	{
		public ReceiptCompactMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.DocNo)
				.IsRequired()
				.HasMaxLength(25);
			// Table & Column Mappings
			this.ToTable("Receipt", "Bean");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.CompanyId).HasColumnName("CompanyId");
			this.Property(t => t.DocDate).HasColumnName("DocDate");
			this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.ReceiptApplicationAmmount).HasColumnName("ReceiptApplicationAmmount");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
			this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
		}
    }
}
