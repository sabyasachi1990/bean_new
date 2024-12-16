using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.DebitNoteModule.Entities.V2
{
    public class InvoiceCompactMap : EntityTypeConfiguration<InvoiceCompact>
    {
        public InvoiceCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("Invoice", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
        }
    }
}
