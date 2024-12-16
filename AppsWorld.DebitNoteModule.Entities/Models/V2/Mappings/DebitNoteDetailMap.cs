using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;


namespace AppsWorld.DebitNoteModule.Entities.V2
{
    public class DebitNoteDetailMap : EntityTypeConfiguration<DebitNoteDetail>
    {
        public DebitNoteDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TaxType)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("DebitNoteDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DebitNoteId).HasColumnName("DebitNoteId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.AllowDisAllow).HasColumnName("AllowDisAllow");
            this.Property(t => t.TaxId).HasColumnName("TaxId");
            this.Property(t => t.TaxType).HasColumnName("TaxType");
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
            this.Property(t => t.DocAmount).HasColumnName("DocAmount");
            this.Property(t => t.DocTaxAmount).HasColumnName("DocTaxAmount");
            this.Property(t => t.DocTotalAmount).HasColumnName("DocTotalAmount");
            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount");
            this.Property(t => t.BaseTaxAmount).HasColumnName("BaseTaxAmount");
            this.Property(t => t.BaseTotalAmount).HasColumnName("BaseTotalAmount");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.AccountDescription).HasColumnName("AccountDescription");
            this.Property(t => t.IsPLAccount).HasColumnName("IsPLAccount");
            this.Property(t => t.TaxIdCode).HasColumnName("TaxIdCode");
            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //     .WithMany(t => t.DebitNoteDetails)
            //     .HasForeignKey(d => d.COAId);
            //this.HasRequired(t => t.DebitNote)
            //    .WithMany(t => t.DebitNoteDetails)
            //    .HasForeignKey(d => d.DebitNoteId);
            //this.HasOptional(t => t.TaxCode)
            //    .WithMany(t => t.DebitNoteDetails)
            //    .HasForeignKey(d => d.TaxId);

        }
    }
}
