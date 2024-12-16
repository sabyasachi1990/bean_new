using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Entities.Models.Mappings
{
    public class WithdrawalMap : EntityTypeConfiguration<Withdrawal>
    {
        public WithdrawalMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SystemRefNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DocNo)
                .HasMaxLength(25);

            this.Property(t => t.EntityType)
                .HasMaxLength(25);

            this.Property(t => t.DocCurrency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.SegmentCategory1)
                .HasMaxLength(50);

            this.Property(t => t.SegmentCategory2)
                .HasMaxLength(50);

            this.Property(t => t.ModeOfWithDrawal)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.WithDrawalRefNo)
                .HasMaxLength(50);

            this.Property(t => t.ExCurrency)
                .HasMaxLength(5);

            this.Property(t => t.GSTExCurrency)
                .HasMaxLength(5);

            this.Property(t => t.DocumentState)
                .HasMaxLength(20);

            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.DocDescription)
                .HasMaxLength(253);

            // Table & Column Mappings
            this.ToTable("WithDrawal", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.SystemRefNo).HasColumnName("SystemRefNo");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.BankClearingDate).HasColumnName("BankClearingDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.SegmentCategory1).HasColumnName("SegmentCategory1");
            this.Property(t => t.SegmentCategory2).HasColumnName("SegmentCategory2");
            this.Property(t => t.IsNoSupportingDocumentActivated).HasColumnName("IsNoSupportingDocumentActivated");
            this.Property(t => t.NoSupportingDocs).HasColumnName("NoSupportingDocs");
            this.Property(t => t.ModeOfWithDrawal).HasColumnName("ModeOfWithDrawal");
            this.Property(t => t.WithDrawalRefNo).HasColumnName("WithDrawalRefNo");
            this.Property(t => t.BankWithDrawalAmmount).HasColumnName("BankWithDrawalAmmount");
            this.Property(t => t.BankCharges).HasColumnName("BankCharges");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.ExCurrency).HasColumnName("ExCurrency");
            this.Property(t => t.ExDurationFrom).HasColumnName("ExDurationFrom");
            this.Property(t => t.ExDurationTo).HasColumnName("ExDurationTo");
            this.Property(t => t.IsGstSettingsActivated).HasColumnName("IsGstSettingsActivated");
            this.Property(t => t.IsMultiCurrencyActivated).HasColumnName("IsMultiCurrencyActivated");
            this.Property(t => t.IsAllowableNonAllowableActivated).HasColumnName("IsAllowableNonAllowableActivated");
            this.Property(t => t.IsSegmentReportingActivated).HasColumnName("IsSegmentReportingActivated");
            this.Property(t => t.SegmentMasterid1).HasColumnName("SegmentMasterid1");
            this.Property(t => t.SegmentMasterid2).HasColumnName("SegmentMasterid2");
            this.Property(t => t.SegmentDetailid1).HasColumnName("SegmentDetailid1");
            this.Property(t => t.SegmentDetailid2).HasColumnName("SegmentDetailid2");
            this.Property(t => t.GSTExchangeRate).HasColumnName("GSTExchangeRate");
            this.Property(t => t.GSTExCurrency).HasColumnName("GSTExCurrency");
            this.Property(t => t.GSTExDurationFrom).HasColumnName("GSTExDurationFrom");
            this.Property(t => t.GSTExDurationTo).HasColumnName("GSTExDurationTo");
            this.Property(t => t.GSTTotalAmount).HasColumnName("GSTTotalAmount");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.IsDisAllow).HasColumnName("IsDisAllow");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.IsBaseCurrencyRateChanged).HasColumnName("IsBaseCurrencyRateChanged");
            this.Property(t => t.IsGSTCurrencyRateChanged).HasColumnName("IsGSTCurrencyRateChanged");

            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.WithDrawals)
            //    .HasForeignKey(d => d.COAId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.WithDrawals)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
