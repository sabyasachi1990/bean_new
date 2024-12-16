using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class CreditMemoMap : EntityTypeConfiguration<CreditMemo>
    {
        public CreditMemoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocSubType)
                .HasMaxLength(20);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

             
            this.Property(t => t.DocDescription)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("CreditMemo", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.PostingDate).HasColumnName("PostingDate");
            this.Property(t => t.Version).HasColumnName("Version");

            // Relationships
            //this.HasRequired(t => t.Entity)
            //    .WithMany(t => t.CreditMemoes)
            //    .HasForeignKey(d => d.EntityId);

        }
    }
}
