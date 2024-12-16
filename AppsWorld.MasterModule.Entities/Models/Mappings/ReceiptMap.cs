using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
	public class ReceiptMap : EntityTypeConfiguration<Receipt>
	{
		public ReceiptMap()
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
			this.Property(t => t.DocNo).HasColumnName("DocNo");
			this.Property(t => t.COAId).HasColumnName("COAId");
			this.Property(t => t.BankReceiptAmmount).HasColumnName("BankReceiptAmmount");
			this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ReceiptApplicationAmmount).HasColumnName("ReceiptApplicationAmmount");
            this.Property(t => t.Version).HasColumnName("Version");
        }
	}
}
