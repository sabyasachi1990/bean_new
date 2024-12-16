using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Entities.Models.Mapping
{
    public class BankTransferDetailMap : EntityTypeConfiguration<BankTransferDetail>
    {
        public BankTransferDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Currency)
                .HasMaxLength(5);

            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("BankTransferDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BankTransferId).HasColumnName("BankTransferId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.Currency).HasColumnName("Currency");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.BankClearingDate).HasColumnName("BankClearingDate");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
        }
    }
}
