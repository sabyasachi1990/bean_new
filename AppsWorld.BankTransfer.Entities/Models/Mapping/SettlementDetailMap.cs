using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Entities.Models.Mapping
{
    public class SettlementDetailMap : EntityTypeConfiguration<SettlementDetail>
    {
        public SettlementDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.ExchangeRate)
             .HasPrecision(15, 10);
            // Table & Column Mappings
            this.ToTable("SettlementDetail", "Bean");
            this.Property(a => a.Id).HasColumnName("Id");
            this.Property(a => a.BankTransferId).HasColumnName("BankTransferId");
            this.Property(a => a.SettlemetType).HasColumnName("SettlemetType");
            this.Property(a => a.DocumentId).HasColumnName("DocumentId");
            this.Property(a => a.DocumentType).HasColumnName("DocumentType");
            this.Property(a => a.DocumentDate).HasColumnName("DocumentDate");
            this.Property(a => a.DocumentNo).HasColumnName("DocumentNo");
            this.Property(a => a.DocumentState).HasColumnName("DocumentState");
            this.Property(a => a.Currency).HasColumnName("Currency");
            this.Property(a => a.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(a => a.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(a => a.DocumentAmmount).HasColumnName("DocumentAmmount");
            this.Property(a => a.AmmountDue).HasColumnName("AmmountDue");
            this.Property(a => a.RecOrder).HasColumnName("RecOrder");
        }
    }
}
