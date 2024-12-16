using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
	public class ReceiptMap : EntityTypeConfiguration<Receipt>
	{
		public ReceiptMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			

			this.Property(t => t.Remarks)
				.HasMaxLength(1000);

			

			// Table & Column Mappings
			this.ToTable("Receipt", "Bean");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.CompanyId).HasColumnName("CompanyId");			
			this.Property(t => t.Remarks).HasColumnName("Remarks");			
			this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.ModeOfReceipt).HasColumnName("ModeOfReceipt");
            this.Property(t => t.ReceiptRefNo).HasColumnName("ReceiptRefNo");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.Version).HasColumnName("Version");
        }
	}
}
