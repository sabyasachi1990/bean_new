using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BillModule.Entities
{
    public class JournalEntryMap : EntityTypeConfiguration<JournalEntry>
    {
        public JournalEntryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SystemReferenceNumber)
                .HasMaxLength(50);

            this.Property(t => t.DocType)
                .HasMaxLength(50);

            this.Property(t => t.DocSubType)
                .HasMaxLength(20);

            this.Property(t => t.ServiceCompany)
                .HasMaxLength(5);

            this.Property(t => t.Nature)
                .HasMaxLength(100);

            this.Property(t => t.DocNo)
                .HasMaxLength(25);

            this.Property(t => t.PONo)
                .HasMaxLength(20);

            this.Property(t => t.SegmentCategory1)
                .HasMaxLength(50);

            this.Property(t => t.SegmentCategory2)
                .HasMaxLength(50);

            this.Property(t => t.EntityType)
                .HasMaxLength(50);

            this.Property(t => t.EntityRefNo)
                .HasMaxLength(25);

            this.Property(t => t.EntityName)
                .HasMaxLength(100);

            this.Property(t => t.AccountCode)
                .HasMaxLength(100);

            this.Property(t => t.AccountName)
                .HasMaxLength(100);

            this.Property(t => t.ItemCode)
                .HasMaxLength(30);

            this.Property(t => t.ItemDescription)
                .HasMaxLength(200);

            this.Property(t => t.Unit)
                .HasMaxLength(20);

            this.Property(t => t.DiscountType)
                .HasMaxLength(1);

            this.Property(t => t.TaxCode)
                .HasMaxLength(20);

            this.Property(t => t.TaxType)
                .HasMaxLength(20);

            this.Property(t => t.DocCurrency)
                .HasMaxLength(5);

            this.Property(t => t.BaseCurrency)
                .HasMaxLength(5);

            this.Property(t => t.GSTReportingCurrency)
                .HasMaxLength(5);

            this.Property(t => t.SettlementMode)
                .HasMaxLength(20);

            this.Property(t => t.OffsetDocument)
                .HasMaxLength(25);

            this.Property(t => t.Reversed)
                .HasMaxLength(5);

            this.Property(t => t.ReversalDocRef)
                .HasMaxLength(25);

            this.Property(t => t.DocumentState)
                .HasMaxLength(25);

            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("JournalEntry", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.PostingDate).HasColumnName("PostingDate");
            this.Property(t => t.SystemReferenceNumber).HasColumnName("SystemReferenceNumber");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.ServiceCompany).HasColumnName("ServiceCompany");
            this.Property(t => t.ServiceId).HasColumnName("ServiceId");
            this.Property(t => t.Nature).HasColumnName("Nature");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.PONo).HasColumnName("PONo");
            this.Property(t => t.CreditTerms).HasColumnName("CreditTerms");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.SegmentCategory1).HasColumnName("SegmentCategory1");
            this.Property(t => t.SegmentCategory2).HasColumnName("SegmentCategory2");
            this.Property(t => t.NoSupportingDocs).HasColumnName("NoSupportingDocs");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.EntityRefNo).HasColumnName("EntityRefNo");
            this.Property(t => t.EntityName).HasColumnName("EntityName");
            this.Property(t => t.AccountCode).HasColumnName("AccountCode");
            this.Property(t => t.AccountName).HasColumnName("AccountName");
            this.Property(t => t.ItemCode).HasColumnName("ItemCode");
            this.Property(t => t.ItemDescription).HasColumnName("ItemDescription");
            this.Property(t => t.Qty).HasColumnName("Qty");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.DiscountType).HasColumnName("DiscountType");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.AllowDisAllow).HasColumnName("AllowDisAllow");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.TaxCode).HasColumnName("TaxCode");
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
            this.Property(t => t.TaxType).HasColumnName("TaxType");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.DebitDC).HasColumnName("DebitDC");
            this.Property(t => t.CreditDC).HasColumnName("CreditDC");
            this.Property(t => t.TaxableamountDC).HasColumnName("TaxableamountDC");
            this.Property(t => t.TaxAmountDC).HasColumnName("TaxAmountDC");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.ExchangeRateBc).HasColumnName("ExchangeRateBc");
            this.Property(t => t.DebitBC).HasColumnName("DebitBC");
            this.Property(t => t.CreditBC).HasColumnName("CreditBC");
            this.Property(t => t.TaxableamountBC).HasColumnName("TaxableamountBC");
            this.Property(t => t.TaxAmountBC).HasColumnName("TaxAmountBC");
            this.Property(t => t.GSTReportingCurrency).HasColumnName("GSTReportingCurrency");
            this.Property(t => t.ExchangeRateGSTR).HasColumnName("ExchangeRateGSTR");
            this.Property(t => t.DebitGSTR).HasColumnName("DebitGSTR");
            this.Property(t => t.CreditGSTR).HasColumnName("CreditGSTR");
            this.Property(t => t.TaxableamountGSTR).HasColumnName("TaxableamountGSTR");
            this.Property(t => t.TaxAmountGSTR).HasColumnName("TaxAmountGSTR");
            this.Property(t => t.Subledgerrequired).HasColumnName("Subledgerrequired");
            this.Property(t => t.SettlementMode).HasColumnName("SettlementMode");
            this.Property(t => t.SettlementRefNO).HasColumnName("SettlementRefNO");
            this.Property(t => t.SettlementDate).HasColumnName("SettlementDate");
            this.Property(t => t.OffsetDocument).HasColumnName("OffsetDocument");
            this.Property(t => t.Cleared).HasColumnName("Cleared");
            this.Property(t => t.ClearingDate).HasColumnName("ClearingDate");
            this.Property(t => t.BankRecon).HasColumnName("BankRecon");
            this.Property(t => t.Reversed).HasColumnName("Reversed");
            this.Property(t => t.ReversalDocRef).HasColumnName("ReversalDocRef");
            this.Property(t => t.ReversalDate).HasColumnName("ReversalDate");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.DocumentStateDT).HasColumnName("DocumentStateDT");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            //this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");

            // Relationships
            //this.HasOptional(t => t.ChartOfAccount)
            //    .WithMany(t => t.JournalEntries)
            //    .HasForeignKey(d => d.COAId);
            //this.HasOptional(t => t.Entity)
            //    .WithMany(t => t.JournalEntries)
            //    .HasForeignKey(d => d.EntityId);
            //this.HasOptional(t => t.Item)
            //    .WithMany(t => t.JournalEntries)
            //    .HasForeignKey(d => d.ItemId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.JournalEntries)
            //    .HasForeignKey(d => d.CompanyId);
            //this.HasOptional(t => t.Service)
            //    .WithMany(t => t.JournalEntries)
            //    .HasForeignKey(d => d.ServiceId);

        }
    }
}
