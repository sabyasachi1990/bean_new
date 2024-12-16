using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class OpeningBalanceDetailMap : EntityTypeConfiguration<OpeningBalanceDetail>
    {
        public OpeningBalanceDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.BaseCurrency)
                .HasMaxLength(20);

            this.Property(t => t.DocCurrency)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("OpeningBalanceDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OpeningBalanceId).HasColumnName("OpeningBalanceId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.BaseCredit).HasColumnName("BaseCredit");
            this.Property(t => t.BaseDebit).HasColumnName("BaseDebit");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.DocCredit).HasColumnName("DocCredit");
            this.Property(t => t.DocDebit).HasColumnName("DocDebit");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.IsOrginalAccount).HasColumnName("IsOrginalAccount");
            this.Property(t => t.ClearingState).HasColumnName("ClearingState");

            // Relationships

            //this.HasRequired(t => t.OpeningBalance)
            //    .WithMany(t => t.OpeningBalanceDetails)
            //    .HasForeignKey(d => d.OpeningBalanceId);
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.OpeningBalanceDetails)
            //    .HasForeignKey(d => d.COAId);
        }
    }
}
