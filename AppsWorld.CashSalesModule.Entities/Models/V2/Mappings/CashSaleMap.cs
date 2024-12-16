using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CashSalesModule.Entities.V2
{
    public class CashSaleMap : EntityTypeConfiguration<CashSale>
    {
        public CashSaleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocSubType)
                .HasMaxLength(20);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.PONo)
                .HasMaxLength(20);

            this.Property(t => t.SegmentCategory1)
                .HasMaxLength(50);

            this.Property(t => t.SegmentCategory2)
                .HasMaxLength(50);

            this.Property(t => t.EntityType)                
                .HasMaxLength(50);

            this.Property(t => t.ModeOfReceipt)
                .HasMaxLength(100);

            this.Property(t => t.ReceiptrefNo)
               .HasMaxLength(50);

            this.Property(t => t.CashSaleNumber)
                .HasMaxLength(50);

            this.Property(t => t.DocCurrency)
                .HasMaxLength(5);

            this.Property(t => t.ExCurrency)
                .HasMaxLength(5);

            this.Property(t => t.GSTExCurrency)
                .HasMaxLength(5);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.CashSaleNumber)
                .HasMaxLength(50);

            this.Property(t => t.DocType)
                .HasMaxLength(50);

            this.Property(t => t.ReverseRemarks)
                .HasMaxLength(1000);

            this.Property(t => t.ExtensionType)
                .HasMaxLength(20);

            this.Property(t => t.ExchangeRate)
            .HasPrecision(15, 10);

            this.Property(t => t.GSTExchangeRate)
              .HasPrecision(15, 10);


            // Table & Column Mappings
            this.ToTable("CashSale", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.PONo).HasColumnName("PONo");
            this.Property(t => t.NoSupportingDocs).HasColumnName("NoSupportingDocs");
            this.Property(t => t.SegmentCategory1).HasColumnName("SegmentCategory1");
            this.Property(t => t.SegmentCategory2).HasColumnName("SegmentCategory2");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ModeOfReceipt).HasColumnName("ModeOfReceipt");
            this.Property(t => t.BankClearingDate).HasColumnName("BankClearingDate");
            this.Property(t => t.ReceiptrefNo).HasColumnName("ReceiptrefNo");
            this.Property(t => t.CashSaleNumber).HasColumnName("CashSaleNumber");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");          
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.ExCurrency).HasColumnName("ExCurrency");
            this.Property(t => t.ExDurationFrom).HasColumnName("ExDurationFrom");
            this.Property(t => t.ExDurationTo).HasColumnName("ExDurationTo");
            this.Property(t => t.GSTExchangeRate).HasColumnName("GSTExchangeRate");
            this.Property(t => t.GSTExCurrency).HasColumnName("GSTExCurrency");
            this.Property(t => t.GSTExDurationFrom).HasColumnName("GSTExDurationFrom");
            this.Property(t => t.GSTExDurationTo).HasColumnName("GSTExDurationTo");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.BalanceAmount).HasColumnName("BalanceAmount");
            this.Property(t => t.GSTTotalAmount).HasColumnName("GSTTotalAmount");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.IsGstSettings).HasColumnName("IsGstSettings");
            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");
            this.Property(t => t.IsSegmentReporting).HasColumnName("IsSegmentReporting");
            this.Property(t => t.IsAllowableNonAllowable).HasColumnName("IsAllowableNonAllowable");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            //this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");           
            this.Property(t => t.CashSaleNumber).HasColumnName("CashSaleNumber");
            this.Property(t => t.IsNoSupportingDocument).HasColumnName("IsNoSupportingDocument");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.IsAllowableDisallowableActivated).HasColumnName("IsAllowableDisallowableActivated");
            this.Property(t => t.ReverseDate).HasColumnName("ReverseDate");
            this.Property(t => t.ReverseIsSupportingDocument).HasColumnName("ReverseIsSupportingDocument");
            this.Property(t => t.ReverseRemarks).HasColumnName("ReverseRemarks");
            this.Property(t => t.AllocatedAmount).HasColumnName("AllocatedAmount");
            this.Property(t => t.SegmentMasterid1).HasColumnName("SegmentMasterid1");
            this.Property(t => t.SegmentMasterid2).HasColumnName("SegmentMasterid2");
            this.Property(t => t.SegmentDetailid1).HasColumnName("SegmentDetailid1");
            this.Property(t => t.SegmentDetailid2).HasColumnName("SegmentDetailid2");
            this.Property(t => t.IsBaseCurrencyRateChanged).HasColumnName("IsBaseCurrencyRateChanged");
            this.Property(t => t.IsGSTCurrencyRateChanged).HasColumnName("IsGSTCurrencyRateChanged");
            this.Property(t => t.IsGSTApplied).HasColumnName("IsGSTApplied");
            this.Property(t => t.ItemTotal).HasColumnName("ItemTotal");
            this.Property(t => t.ExtensionType).HasColumnName("ExtensionType");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            //this.HasRequired(t => t.ChartOfAccount)
            // .WithMany(t => t.CashSales)
            // .HasForeignKey(d => d.COAId);
        }
    }
}
