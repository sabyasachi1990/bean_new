using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BankTransferModule.Entities.Models.Mapping
{
    public class BankTransferMap : EntityTypeConfiguration<BankTransfer>
    {
        public BankTransferMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocType)
                .IsRequired()
                .HasMaxLength(20);

            //this.Property(t => t.SystemRefNo)
            //    .HasMaxLength(50);

            this.Property(t => t.DocDescription)
                .HasMaxLength(253);

            //this.Property(t => t.DocNo)
            //    .HasMaxLength(25);

            this.Property(t => t.ModeOfTransfer)
                .HasMaxLength(20);

            this.Property(t => t.TransferRefNo)
                .HasMaxLength(50);

            this.Property(t => t.ExCurrency)
                .HasMaxLength(5);

            this.Property(t => t.DocumentState)
                .HasMaxLength(20);

            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.ExchangeRate)
              .HasPrecision(15, 10);

            this.Property(t => t.SystemCalculatedExchangeRate)
               .HasPrecision(15, 10);

            this.Property(t => t.VarianceExchangeRate)
               .HasPrecision(15, 10);

            // Table & Column Mappings
            this.ToTable("BankTransfer", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.SystemRefNo).HasColumnName("SystemRefNo");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.TransferDate).HasColumnName("TransferDate");
            this.Property(t => t.BankClearingDate).HasColumnName("BankClearingDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.IsNoSupportingDocument).HasColumnName("IsNoSupportingDocument");
            this.Property(t => t.IsGstSetting).HasColumnName("IsGstSetting");
            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");
            this.Property(t => t.IsMultiCompany).HasColumnName("IsMultiCompany");
            this.Property(t => t.NoSupportingDocument).HasColumnName("NoSupportingDocument");
            this.Property(t => t.ModeOfTransfer).HasColumnName("ModeOfTransfer");
            this.Property(t => t.TransferRefNo).HasColumnName("TransferRefNo");
            this.Property(t => t.SystemCalculatedExchangeRate).HasColumnName("SystemCalculatedExchangeRate");
            this.Property(t => t.VarianceExchangeRate).HasColumnName("VarianceExchangeRate");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.ExCurrency).HasColumnName("ExCurrency");
            this.Property(t => t.ExDurationFrom).HasColumnName("ExDurationFrom");
            this.Property(t => t.ExDurationTo).HasColumnName("ExDurationTo");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.IsInterCompany).HasColumnName("IsInterCompany");
            this.Property(t => t.IsBaseCurrencyRateChanged).HasColumnName("IsBaseCurrencyRateChanged");
            this.Property(t => t.IsIntercoClearing).HasColumnName("IsIntercoClearing");
            this.Property(t => t.IsIntercoBilling).HasColumnName("IsIntercoBilling");
            this.Property(t => t.ClearCount).HasColumnName("ClearCount");
            this.Property(t => t.IsLocked).HasColumnName("IsLocked");
            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.BankTranfers)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
