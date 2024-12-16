using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Entities.Models.Mapping
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

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(20);
            this.Property(t => t.Nature)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.DocCurrency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.BaseCurrency)
                .HasMaxLength(5);

            this.Property(t => t.ExchangeRate)
                .HasPrecision(15, 10);

            // Table & Column Mappings
            this.ToTable("Bill", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.PostingDate).HasColumnName("PostingDate");
            this.Property(t => t.DocumentDate).HasColumnName("DocumentDate");
            //this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            //this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.Nature).HasColumnName("Nature");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.BalanceAmount).HasColumnName("BalanceAmount");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            //this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            //this.Property(t => t.IsExternal).HasColumnName("IsExternal");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.PayrollId).HasColumnName("PayrollId");
            //this.Property(t => t.OpeningBalanceId).HasColumnName("OpeningBalanceId");
            //this.Property(t => t.DocDescription).HasColumnName("DocDescription");
        }
    }
}
