using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.V2.Mapping
{
    public class CashSaleDetailMap : EntityTypeConfiguration<CashSaleDetail>
    {
        public CashSaleDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ItemCode)
                .HasMaxLength(50);

            this.Property(t => t.ItemDescription)
                .HasMaxLength(200);

            this.Property(t => t.Unit)
                .HasMaxLength(20);

            this.Property(t => t.DiscountType)
                .HasMaxLength(1);

            this.Property(t => t.TaxCurrency)
                .HasMaxLength(10);

            this.Property(t => t.AmtCurrency)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("CashSaleDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CashSaleId).HasColumnName("CashSaleId");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.ItemCode).HasColumnName("ItemCode");
            this.Property(t => t.ItemDescription).HasColumnName("ItemDescription");
            this.Property(t => t.Qty).HasColumnName("Qty");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.DiscountType).HasColumnName("DiscountType");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.AllowDisAllow).HasColumnName("AllowDisAllow");
            this.Property(t => t.TaxId).HasColumnName("TaxId");
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
            this.Property(t => t.DocTaxAmount).HasColumnName("DocTaxAmount");
            this.Property(t => t.TaxCurrency).HasColumnName("TaxCurrency");
            this.Property(t => t.DocAmount).HasColumnName("DocAmount");
            this.Property(t => t.AmtCurrency).HasColumnName("AmtCurrency");
            this.Property(t => t.DocTotalAmount).HasColumnName("DocTotalAmount");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount");
            this.Property(t => t.BaseTaxAmount).HasColumnName("BaseTaxAmount");
            this.Property(t => t.BaseTotalAmount).HasColumnName("BaseTotalAmount");            
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.IsPLAccount).HasColumnName("IsPLAccount");
            this.Property(t => t.TaxIdCode).HasColumnName("TaxIdCode");
            this.Property(t => t.ClearingState).HasColumnName("ClearingState");
            // Relationships
            //this.HasRequired(t => t.CashSale)
            //    .WithMany(t => t.CashSaleDetails)
            //    .HasForeignKey(d => d.CashSaleId);
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.CashSaleDetails)
            //    .HasForeignKey(d => d.COAId);
            //this.HasOptional(t => t.Item)
            //    .WithMany(t => t.CashSaleDetails)
            //    .HasForeignKey(d => d.ItemId);
            //this.HasOptional(t => t.TaxCode)
            //    .WithMany(t => t.CashSaleDetails)
            //    .HasForeignKey(d => d.TaxId);

        }
    }
}
