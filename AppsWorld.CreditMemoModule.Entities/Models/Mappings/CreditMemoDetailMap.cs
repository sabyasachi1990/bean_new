using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CreditMemoModule.Entities.Mapping
{
    public class CreditMemoDetailMap : EntityTypeConfiguration<CreditMemoDetail>
    {
        public CreditMemoDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.TaxCurrency)
                .HasMaxLength(10);

            //this.Property(t => t.AmtCurrency)
            //    .IsRequired()
            //    .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("CreditMemoDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreditMemoId).HasColumnName("CreditMemoId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.AllowDisAllow).HasColumnName("AllowDisAllow");
            this.Property(t => t.TaxId).HasColumnName("TaxId");
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
            this.Property(t => t.DocTaxAmount).HasColumnName("DocTaxAmount");
            this.Property(t => t.TaxCurrency).HasColumnName("TaxCurrency");
            this.Property(t => t.DocAmount).HasColumnName("DocAmount");
           // this.Property(t => t.AmtCurrency).HasColumnName("AmtCurrency");
            this.Property(t => t.DocTotalAmount).HasColumnName("DocTotalAmount");
            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount");
            this.Property(t => t.BaseTaxAmount).HasColumnName("BaseTaxAmount");
            this.Property(t => t.BaseTotalAmount).HasColumnName("BaseTotalAmount");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsPLAccount).HasColumnName("IsPLAccount");
            this.Property(t => t.TaxIdCode).HasColumnName("TaxIdCode");
            this.Property(t => t.ClearingState).HasColumnName("ClearingState");

            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.CreditMemoDetails)
            //    .HasForeignKey(d => d.COAId);
            //this.HasRequired(t => t.CreditMemo)
            //    .WithMany(t => t.CreditMemoDetails)
            //    .HasForeignKey(d => d.CreditMemoId);
            //this.HasOptional(t => t.TaxCode)
            //    .WithMany(t => t.CreditMemoDetails)
            //    .HasForeignKey(d => d.TaxId);

        }
    }
}
