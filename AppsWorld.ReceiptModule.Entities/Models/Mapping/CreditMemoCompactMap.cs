using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.ReceiptModule.Entities.Mapping
{
    public class CreditMemoCompactMap : EntityTypeConfiguration<CreditMemoCompact>
    {
        public CreditMemoCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocSubType)
                .HasMaxLength(20);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);
 
            this.Property(t => t.Nature)
                .HasMaxLength(100);

            this.Property(t => t.DocCurrency)
                .HasMaxLength(5);

            //this.Property(t => t.ExCurrency)
            //    .HasMaxLength(5);
 
            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DocType)
                .HasMaxLength(50);

            this.Property(t => t.ExchangeRate)
              .HasPrecision(15, 10);
 

            // Table & Column Mappings
            this.ToTable("CreditMemo", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.Nature).HasColumnName("Nature");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
           // this.Property(t => t.ExCurrency).HasColumnName("ExCurrency");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.BalanceAmount).HasColumnName("BalanceAmount");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.GSTExchangeRate).HasColumnName("GSTExchangeRate");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            //this.Property(t => t.OpeningBalanceId).HasColumnName("OpeningBalanceId");
            //this.Property(t => t.AllocatedAmount).HasColumnName("AllocatedAmount");
            //this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.BaseBalanceAmount).HasColumnName("BaseBalanceAmount");
            this.Property(t => t.BaseGrandTotal).HasColumnName("BaseGrandTotal");
            this.Property(t => t.RoundingAmount).HasColumnName("RoundingAmount");
            // Relationships
            //this.HasRequired(t => t.Entity)
            //    .WithMany(t => t.CreditMemoes)
            //    .HasForeignKey(d => d.EntityId);

        }
    }
}
