using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.OpeningBalancesModule.Entities
{
    public class BillMap : EntityTypeConfiguration<Bill>
    {
        public BillMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocSubType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SystemReferenceNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(20);

             

            // Table & Column Mappings
            this.ToTable("Bill", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.SystemReferenceNumber).HasColumnName("SystemReferenceNumber");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
        }
    }
}
