using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.Mappings
{
    public class ReceiptKMap : EntityTypeConfiguration<ReceiptK>
    {
        public ReceiptKMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
           

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.ModeOfReceipt)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ReceiptRefNo)
                .HasMaxLength(50);

            this.Property(t => t.BankReceiptAmmountCurrency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("Receipt", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.BankClearingDate).HasColumnName("BankClearingDate");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.ModeOfReceipt).HasColumnName("ModeOfReceipt");
            this.Property(t => t.ReceiptRefNo).HasColumnName("ReceiptRefNo");
            this.Property(t => t.BankReceiptAmmountCurrency).HasColumnName("BankReceiptAmmountCurrency");
            this.Property(t => t.BankReceiptAmmount).HasColumnName("BankReceiptAmmount");
            this.Property(t => t.ReceiptApplicationAmmount).HasColumnName("ReceiptApplicationAmmount");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
        }
    }
}
