using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;


namespace AppsWorld.DebitNoteModule.Entities.Mapping
{
    public class DoubtfulDebtAllocationDetailMap : EntityTypeConfiguration<DoubtfulDebtAllocationDetail>
    {
        public DoubtfulDebtAllocationDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocumentType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DocCurrency)
                .IsRequired()
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("DoubtfulDebtAllocationDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DoubtfulDebtAllocationId).HasColumnName("DoubtfulDebtAllocationId");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.DocumentType).HasColumnName("DocumentType");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.AllocateAmount).HasColumnName("AllocateAmount");

            // Relationships
            //this.HasRequired(t => t.DoubtfulDebtAllocation)
            //    .WithMany(t => t.DoubtfulDebtAllocationDetails)
            //    .HasForeignKey(d => d.DoubtfulDebtAllocationId);

        }
    }
}
