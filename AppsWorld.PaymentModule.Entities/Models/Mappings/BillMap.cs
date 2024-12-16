using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.PaymentModule.Entities.Mapping
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

            this.Property(t => t.SystemReferenceNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

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

            this.Property(t => t.GSTExchangeRate)
               .HasPrecision(15, 10);


            // Table & Column Mappings
            this.ToTable("Bill", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.SystemReferenceNumber).HasColumnName("SystemReferenceNumber");
            this.Property(t => t.PostingDate).HasColumnName("PostingDate");
            this.Property(t => t.DocumentDate).HasColumnName("DocumentDate");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.Nature).HasColumnName("Nature");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.GSTExchangeRate).HasColumnName("GSTExchangeRate");
            this.Property(t => t.OpeningBalanceId).HasColumnName("OpeningBalanceId");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.IsExternal).HasColumnName("IsExternal");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.BaseGrandTotal).HasColumnName("BaseGrandTotal");
            this.Property(t => t.BaseBalanceAmount).HasColumnName("BaseBalanceAmount");
            this.Property(t => t.RoundingAmount).HasColumnName("RoundingAmount");

            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Bills)
            //    .HasForeignKey(d => d.CompanyId);
            //this.HasRequired(t => t.TermsOfPayment)
            //    .WithMany(t => t.Bills)
            //    .HasForeignKey(d => d.CreditTermsId);
            //this.HasRequired(t => t.Entity)
            //    .WithMany(t => t.Bills)
            //    .HasForeignKey(d => d.EntityId);

        }
    }
}
