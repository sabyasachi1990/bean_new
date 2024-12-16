using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CashSalesModule.Entities.V2
{
    public class CashSaleKMap : EntityTypeConfiguration<CashSaleK>
    {
        public CashSaleKMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.PONo)
                .HasMaxLength(20);

            this.Property(t => t.ModeOfReceipt)
                .HasMaxLength(100);

            this.Property(t => t.ReceiptrefNo)
               .HasMaxLength(50);
          
            this.Property(t => t.DocCurrency)
                .HasMaxLength(5);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);
            
            this.Property(t => t.DocType)
                .HasMaxLength(50);

            this.Property(t => t.ExchangeRate)
            .HasPrecision(15, 10);


            // Table & Column Mappings
            this.ToTable("CashSale", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.PONo).HasColumnName("PONo");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ModeOfReceipt).HasColumnName("ModeOfReceipt");
            this.Property(t => t.BankClearingDate).HasColumnName("BankClearingDate");
            this.Property(t => t.ReceiptrefNo).HasColumnName("ReceiptrefNo");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");          
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
        }
    }
}
