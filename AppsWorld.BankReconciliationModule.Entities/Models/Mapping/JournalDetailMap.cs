using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Entities.Models.Mappings
{
    public class JournalDetailMap : EntityTypeConfiguration<JournalDetail>
    {
        public JournalDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.AccountDescription)
                .HasMaxLength(254);

            this.Property(t => t.TaxType)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("JournalDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.JournalId).HasColumnName("JournalId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.AccountDescription).HasColumnName("AccountDescription");
            this.Property(t => t.AllowDisAllow).HasColumnName("AllowDisAllow");
            this.Property(t => t.TaxId).HasColumnName("TaxId");
            this.Property(t => t.TaxType).HasColumnName("TaxType");
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
            this.Property(t => t.DocDebit).HasColumnName("DocDebit");
            this.Property(t => t.DocCredit).HasColumnName("DocCredit");
            this.Property(t => t.DocTaxDebit).HasColumnName("DocTaxDebit");
            this.Property(t => t.DocTaxCredit).HasColumnName("DocTaxCredit");
            this.Property(t => t.BaseDebit).HasColumnName("BaseDebit");
            this.Property(t => t.BaseCredit).HasColumnName("BaseCredit");
            this.Property(t => t.BaseTaxDebit).HasColumnName("BaseTaxDebit");
            this.Property(t => t.BaseTaxCredit).HasColumnName("BaseTaxCredit");
            this.Property(t => t.DocDebitTotal).HasColumnName("DocDebitTotal");
            this.Property(t => t.DocCreditTotal).HasColumnName("DocCreditTotal");
            this.Property(t => t.BaseDebitTotal).HasColumnName("BaseDebitTotal");
            this.Property(t => t.BaseCreditTotal).HasColumnName("BaseCreditTotal");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.ItemCode).HasColumnName("ItemCode");
            this.Property(t => t.ItemDescription).HasColumnName("ItemDescription");
            this.Property(t => t.Qty).HasColumnName("Qty");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.DiscountType).HasColumnName("DiscountType");
            this.Property(t => t.Discount).HasColumnName("Discount");
			this.Property(t => t.DocumentId).HasColumnName("DocumentId");
			this.Property(t => t.DocumentDetailId).HasColumnName("DocumentDetailId");
			this.Property(t => t.DocTaxableAmount).HasColumnName("DocTaxableAmount");
			this.Property(t => t.DocTaxAmount).HasColumnName("DocTaxAmount");
			this.Property(t => t.BaseTaxableAmount).HasColumnName("BaseTaxableAmount");
			this.Property(t => t.BaseTaxAmount).HasColumnName("BaseTaxAmount");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.GSTExCurrency).HasColumnName("GSTExCurrency");
            this.Property(t => t.GSTExchangeRate).HasColumnName("GSTExchangeRate");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.Nature).HasColumnName("Nature");
            this.Property(t => t.OffsetDocument).HasColumnName("OffsetDocument");
            this.Property(t => t.IsTax).HasColumnName("IsTax");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.SettlementMode).HasColumnName("SettlementMode");
            this.Property(t => t.SettlementRefNo).HasColumnName("SettlementRefNo");
            this.Property(t => t.SettlementDate).HasColumnName("SettlementDate");
            this.Property(t => t.SystemRefNo).HasColumnName("SystemRefNo");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.PONo).HasColumnName("PONo");
            this.Property(t => t.CreditTermsId).HasColumnName("CreditTermsId");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.SegmentMasterid1).HasColumnName("SegmentMasterid1");
            this.Property(t => t.SegmentMasterid2).HasColumnName("SegmentMasterid2");
            this.Property(t => t.SegmentDetailid1).HasColumnName("SegmentDetailid1");
            this.Property(t => t.SegmentDetailid2).HasColumnName("SegmentDetailid2");
            this.Property(t => t.NoSupportingDocs).HasColumnName("NoSupportingDocs");
            this.Property(t => t.SegmentCategory1).HasColumnName("SegmentCategory1");
            this.Property(t => t.SegmentCategory2).HasColumnName("SegmentCategory2");
            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.PostingDate).HasColumnName("PostingDate");
            this.Property(t => t.RecieptType).HasColumnName("RecieptType");
			this.Property(t => t.GSTDebit).HasColumnName("GSTDebit");
			this.Property(t => t.GSTCredit).HasColumnName("GSTCredit");
			this.Property(t => t.GSTTaxableAmount).HasColumnName("GSTTaxableAmount");
			this.Property(t => t.GSTTaxAmount).HasColumnName("GSTTaxAmount");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ReconciliationDate).HasColumnName("ReconciliationDate");
            this.Property(t => t.IsBankReconcile).HasColumnName("IsBankReconcile");
            this.Property(t => t.ClearingDate).HasColumnName("ClearingDate");
            this.Property(t => t.ClearingStatus).HasColumnName("ClearingStatus");
            this.Property(t => t.ReconciliationId).HasColumnName("ReconciliationId");

            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.JournalDetails)
            //    .HasForeignKey(d => d.COAId);
            ////this.HasRequired(t => t.Journal)
            ////    .WithMany(t => t.JournalDetails)
            //    .HasForeignKey(d => d.JournalId);
            //this.HasOptional(t => t.TaxCode)
            //    .WithMany(t => t.JournalDetails)
            //    .HasForeignKey(d => d.TaxId);

        }
    }
}
