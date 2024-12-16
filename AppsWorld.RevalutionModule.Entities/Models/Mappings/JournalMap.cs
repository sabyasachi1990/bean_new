using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Entities.Models.Mappings
{
    public class JournalMap : EntityTypeConfiguration<Journal>
    {
        public JournalMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocType)
                .HasMaxLength(50);

            this.Property(t => t.DocSubType)
                .HasMaxLength(20);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.SystemReferenceNo)
                .HasMaxLength(50);

            this.Property(t => t.DocCurrency)
                .HasMaxLength(5);

            this.Property(t => t.SegmentCategory1)
                .HasMaxLength(50);

            this.Property(t => t.SegmentCategory2)
                .HasMaxLength(50);

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

            this.Property(t => t.DocumentDescription)
                .HasMaxLength(50);

            this.Property(t => t.RecurringJournalName)
                .HasMaxLength(30);

            this.Property(t => t.FrequencyType)
                .HasMaxLength(2);

            this.Property(t => t.CreationType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.EntityType)
                .HasMaxLength(50);

            this.Property(t => t.PoNo)
                .HasMaxLength(20);

            this.Property(t => t.ModeOfReceipt)
                .HasMaxLength(20);

            this.Property(t => t.BankReceiptAmmountCurrency)
                .HasMaxLength(5);

            this.Property(t => t.BankChargesCurrency)
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

            this.Property(t => t.Nature)
                .HasMaxLength(100);

            this.Property(t => t.RepEveryPeriod)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Journal", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.SystemReferenceNo).HasColumnName("SystemReferenceNo");
            this.Property(t => t.IsNoSupportingDocs).HasColumnName("IsNoSupportingDocs");
            this.Property(t => t.NoSupportingDocument).HasColumnName("NoSupportingDocument");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.SegmentCategory1).HasColumnName("SegmentCategory1");
            this.Property(t => t.SegmentCategory2).HasColumnName("SegmentCategory2");
            this.Property(t => t.IsSegmentReporting).HasColumnName("IsSegmentReporting");
            this.Property(t => t.SegmentMasterid1).HasColumnName("SegmentMasterid1");
            this.Property(t => t.SegmentMasterid2).HasColumnName("SegmentMasterid2");
            this.Property(t => t.SegmentDetailid1).HasColumnName("SegmentDetailid1");
            this.Property(t => t.SegmentDetailid2).HasColumnName("SegmentDetailid2");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.ExCurrency).HasColumnName("ExCurrency");
            this.Property(t => t.ExDurationFrom).HasColumnName("ExDurationFrom");
            this.Property(t => t.ExDurationTo).HasColumnName("ExDurationTo");
            this.Property(t => t.GSTExchangeRate).HasColumnName("GSTExchangeRate");
            this.Property(t => t.GSTExCurrency).HasColumnName("GSTExCurrency");
            this.Property(t => t.GSTExDurationFrom).HasColumnName("GSTExDurationFrom");
            this.Property(t => t.GSTExDurationTo).HasColumnName("GSTExDurationTo");
            this.Property(t => t.GSTTotalAmount).HasColumnName("GSTTotalAmount");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.IsNoSupportingDocument).HasColumnName("IsNoSupportingDocument");
            this.Property(t => t.IsGstSettings).HasColumnName("IsGstSettings");
            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");
            this.Property(t => t.IsAllowableNonAllowable).HasColumnName("IsAllowableNonAllowable");
            this.Property(t => t.IsBaseCurrencyRateChanged).HasColumnName("IsBaseCurrencyRateChanged");
            this.Property(t => t.IsGSTCurrencyRateChanged).HasColumnName("IsGSTCurrencyRateChanged");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            //this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DocumentDescription).HasColumnName("DocumentDescription");
            this.Property(t => t.IsRecurringJournal).HasColumnName("IsRecurringJournal");
            this.Property(t => t.RecurringJournalName).HasColumnName("RecurringJournalName");
            this.Property(t => t.FrequencyValue).HasColumnName("FrequencyValue");
            this.Property(t => t.FrequencyType).HasColumnName("FrequencyType");
            this.Property(t => t.FrequencyEndDate).HasColumnName("FrequencyEndDate");
            this.Property(t => t.IsAutoReversalJournal).HasColumnName("IsAutoReversalJournal");
            this.Property(t => t.ReversalDate).HasColumnName("ReversalDate");
            this.Property(t => t.ReverseParentRefId).HasColumnName("ReverseParentRefId");
            this.Property(t => t.ReverseChildRefId).HasColumnName("ReverseChildRefId");
            this.Property(t => t.CreationType).HasColumnName("CreationType");
            this.Property(t => t.GrandDocDebitTotal).HasColumnName("GrandDocDebitTotal");
            this.Property(t => t.GrandDocCreditTotal).HasColumnName("GrandDocCreditTotal");
            this.Property(t => t.GrandBaseDebitTotal).HasColumnName("GrandBaseDebitTotal");
            this.Property(t => t.GrandBaseCreditTotal).HasColumnName("GrandBaseCreditTotal");
            this.Property(t => t.IsCopy).HasColumnName("IsCopy");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.PoNo).HasColumnName("PoNo");
            this.Property(t => t.PostingDate).HasColumnName("PostingDate");
            this.Property(t => t.IsGSTApplied).HasColumnName("IsGSTApplied");
            this.Property(t => t.IsGSTDeRegistration).HasColumnName("IsGSTDeRegistration");
            this.Property(t => t.GSTDeRegistrationDate).HasColumnName("GSTDeRegistrationDate");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.ModeOfReceipt).HasColumnName("ModeOfReceipt");
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
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.ClearingDate).HasColumnName("ClearingDate");
            this.Property(t => t.ClearingStatus).HasColumnName("ClearingStatus");
            this.Property(t => t.Nature).HasColumnName("Nature");
            this.Property(t => t.IsRepeatingInvoice).HasColumnName("IsRepeatingInvoice");
            this.Property(t => t.RepEveryPeriodNo).HasColumnName("RepEveryPeriodNo");
            this.Property(t => t.RepEveryPeriod).HasColumnName("RepEveryPeriod");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.CreditTermsId).HasColumnName("CreditTermsId");
        }
    }
}
