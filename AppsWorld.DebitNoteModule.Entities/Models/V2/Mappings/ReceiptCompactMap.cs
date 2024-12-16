using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.DebitNoteModule.Entities.V2
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
			this.Property(t => t.DocNo).HasColumnName("DocNo");
		    this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
