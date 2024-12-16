using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Entities.Mapping
{
    public class JournalDetailMap : EntityTypeConfiguration<JournalDetail>
    {
        public JournalDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.AccountDescription)
            //    .HasMaxLength(254);
            // Table & Column Mappings
            this.ToTable("JournalDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.JournalId).HasColumnName("JournalId");
            //this.Property(t => t.COAId).HasColumnName("COAId");
            //this.Property(t => t.AccountDescription).HasColumnName("AccountDescription");
            //this.Property(t => t.DocDebit).HasColumnName("DocDebit");
            //this.Property(t => t.DocCredit).HasColumnName("DocCredit");
            //this.Property(t => t.DocTaxDebit).HasColumnName("DocTaxDebit");
            //this.Property(t => t.DocTaxCredit).HasColumnName("DocTaxCredit");
            //this.Property(t => t.BaseDebit).HasColumnName("BaseDebit");
            //this.Property(t => t.BaseCredit).HasColumnName("BaseCredit");
            //this.Property(t => t.BaseTaxDebit).HasColumnName("BaseTaxDebit");
            //this.Property(t => t.BaseTaxCredit).HasColumnName("BaseTaxCredit");
            //this.Property(t => t.DocDebitTotal).HasColumnName("DocDebitTotal");
            //this.Property(t => t.DocCreditTotal).HasColumnName("DocCreditTotal");
            //this.Property(t => t.BaseDebitTotal).HasColumnName("BaseDebitTotal");
            //this.Property(t => t.BaseCreditTotal).HasColumnName("BaseCreditTotal");
          
			this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            //this.Property(t => t.DocType).HasColumnName("DocType");
            //this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            //this.Property(t => t.DocNo).HasColumnName("DocNo");
            //this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            //this.Property(t => t.SystemRefNo).HasColumnName("SystemRefNo");
            //this.Property(t => t.DocDate).HasColumnName("DocDate");
            //this.Property(t => t.Type).HasColumnName("Type");
            //this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            //this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.DocumentDetailId).HasColumnName("DocumentDetailId");
            //this.Property(t => t.IsTax).HasColumnName("IsTax");
            //this.Property(t => t.AmountDue).HasColumnName("AmountDue");

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
