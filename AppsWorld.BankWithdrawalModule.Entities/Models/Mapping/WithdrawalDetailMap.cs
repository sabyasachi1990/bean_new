using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BankWithdrawalModule.Entities.Mapping
{
    public class WithdrawalDetailMap : EntityTypeConfiguration<WithdrawalDetail>
    {
        public WithdrawalDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(x => x.DocAmount)
                .HasPrecision(18, 4);
             
            // Properties
            // Table & Column Mappings
            this.ToTable("WithdrawalDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.WithdrawalId).HasColumnName("WithdrawalId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.AllowDisAllow).HasColumnName("AllowDisAllow");
            this.Property(t => t.TaxId).HasColumnName("TaxId");
            this.Property(t => t.TaxRate).HasColumnName("TaxRate");
            this.Property(t => t.DocAmount).HasColumnName("DocAmount");
            this.Property(t => t.DocTaxAmount).HasColumnName("DocTaxAmount");
            this.Property(t => t.DocTotalAmount).HasColumnName("DocTotalAmount");
            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount");
            this.Property(t => t.BaseTaxAmount).HasColumnName("BaseTaxAmount");
            this.Property(t => t.BaseTotalAmount).HasColumnName("BaseTotalAmount");
            this.Property(t => t.Currency).HasColumnName("Currency");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.IsPLAccount).HasColumnName("IsPLAccount");
            this.Property(t => t.TaxIdCode).HasColumnName("TaxIdCode");
            this.Property(t => t.ClearingState).HasColumnName("ClearingState");
            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.WithdrawalDetails)
            //    .HasForeignKey(d => d.COAId);
            //this.HasOptional(t => t.TaxCode)
            //    .WithMany(t => t.WithdrawalDetails)
            //    .HasForeignKey(d => d.TaxId);
            //this.HasRequired(t => t.Withdrawal)
            //    .WithMany(t => t.WithdrawalDetails)
            //    .HasForeignKey(d => d.WithdrawalId);

        }
    }
}
