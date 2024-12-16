using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AppsWorld.BankReconciliationModule.Entities;

namespace AppsWorld.BankReconciliationModule.Entities.Models.Mappings
{
    public class ReceiptMap : EntityTypeConfiguration<Receipt>
    {
        public ReceiptMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocSubType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SystemRefNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EntityType)
                .IsRequired()
                .HasMaxLength(50);

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

            this.Property(t => t.BankChargesCurrency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.ExcessPaidByClient)
                .HasMaxLength(50);

            this.Property(t => t.ExcessPaidByClientCurrency)
                .HasMaxLength(5);

            this.Property(t => t.BalancingItemReciveCRCurrency)
                .HasMaxLength(5);

            this.Property(t => t.BalancingItemPayDRCurrency)
                .HasMaxLength(5);

            this.Property(t => t.ReceiptApplicationCurrency)
                .HasMaxLength(5);

            this.Property(t => t.ExCurrency)
                .HasMaxLength(5);

            this.Property(t => t.GSTExCurrency)
                .HasMaxLength(5);

            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.DocCurrency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.BaseCurrency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.DocumentState)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Receipt", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.SystemRefNo).HasColumnName("SystemRefNo");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.BankClearingDate).HasColumnName("BankClearingDate");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.IsNoSupportingDocument).HasColumnName("IsNoSupportingDocument");
            this.Property(t => t.NoSupportingDocs).HasColumnName("NoSupportingDocs");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.ModeOfReceipt).HasColumnName("ModeOfReceipt");
            this.Property(t => t.ReceiptRefNo).HasColumnName("ReceiptRefNo");
            this.Property(t => t.BankReceiptAmmountCurrency).HasColumnName("BankReceiptAmmountCurrency");
            this.Property(t => t.BankReceiptAmmount).HasColumnName("BankReceiptAmmount");
            this.Property(t => t.BankChargesCurrency).HasColumnName("BankChargesCurrency");
            this.Property(t => t.BankCharges).HasColumnName("BankCharges");
            this.Property(t => t.ExcessPaidByClient).HasColumnName("ExcessPaidByClient");
            this.Property(t => t.ExcessPaidByClientCurrency).HasColumnName("ExcessPaidByClientCurrency");
            this.Property(t => t.ExcessPaidByClientAmmount).HasColumnName("ExcessPaidByClientAmmount");
            this.Property(t => t.BalancingItemReciveCRCurrency).HasColumnName("BalancingItemReciveCRCurrency");
            this.Property(t => t.BalancingItemReciveCRAmount).HasColumnName("BalancingItemReciveCRAmount");
            this.Property(t => t.BalancingItemPayDRCurrency).HasColumnName("BalancingItemPayDRCurrency");
            this.Property(t => t.BalancingItemPayDRAmount).HasColumnName("BalancingItemPayDRAmount");
            this.Property(t => t.ReceiptApplicationCurrency).HasColumnName("ReceiptApplicationCurrency");
            this.Property(t => t.ReceiptApplicationAmmount).HasColumnName("ReceiptApplicationAmmount");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.ExCurrency).HasColumnName("ExCurrency");
            this.Property(t => t.ExDurationFrom).HasColumnName("ExDurationFrom");
            this.Property(t => t.ExDurationTo).HasColumnName("ExDurationTo");
            this.Property(t => t.IsGstSettings).HasColumnName("IsGstSettings");
            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");
            this.Property(t => t.IsAllowableDisallowable).HasColumnName("IsAllowableDisallowable");
            this.Property(t => t.IsDisAllow).HasColumnName("IsDisAllow");
            this.Property(t => t.GSTExchangeRate).HasColumnName("GSTExchangeRate");
            this.Property(t => t.GSTExCurrency).HasColumnName("GSTExCurrency");
            this.Property(t => t.GSTExDurationFrom).HasColumnName("GSTExDurationFrom");
            this.Property(t => t.GSTExDurationTo).HasColumnName("GSTExDurationTo");
            this.Property(t => t.GSTTotalAmount).HasColumnName("GSTTotalAmount");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.IsGSTCurrencyRateChanged).HasColumnName("IsGSTCurrencyRateChanged");
            this.Property(t => t.IsBaseCurrencyRateChanged).HasColumnName("IsBaseCurrencyRateChanged");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.SystemCalculatedExchangeRate).HasColumnName("SystemCalculatedExchangeRate");
            this.Property(t => t.VarianceExchangeRate).HasColumnName("VarianceExchangeRate");

            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.Receipts)
            //    .HasForeignKey(d => d.COAId);
            //this.HasRequired(t => t.Entity)
            //    .WithMany(t => t.Receipts)
            //    .HasForeignKey(d => d.EntityId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Receipts)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
