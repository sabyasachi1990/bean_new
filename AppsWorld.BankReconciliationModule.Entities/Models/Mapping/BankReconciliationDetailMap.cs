using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Entities.Models.Mappings
{
    public class BankReconciliationDetailMap : EntityTypeConfiguration<BankReconciliationDetail>
    {
        public BankReconciliationDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocumentType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DocRefNo)
                .IsRequired()
                .HasMaxLength(25);

            //this.Property(t => t.RefernceNo)
            //   .HasMaxLength(25);

            this.Property(t => t.ClearingStatus)
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("BankReconciliationDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BankReconciliationId).HasColumnName("BankReconciliationId");
            this.Property(t => t.ClearingDate).HasColumnName("ClearingDate");
            this.Property(t => t.DocumentDate).HasColumnName("DocumentDate");
            this.Property(t => t.DocumentType).HasColumnName("DocumentType");
            this.Property(t => t.DocRefNo).HasColumnName("DocRefNo");
            //this.Property(t => t.RefernceNo).HasColumnName("RefernceNo");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.Ammount).HasColumnName("Ammount");
            this.Property(t => t.ClearingStatus).HasColumnName("ClearingStatus");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.isWithdrawl).HasColumnName("isWithdrawl");
            this.Property(t => t.IsDisable).HasColumnName("IsDisable");
            this.Property(t => t.IsChecked).HasColumnName("IsChecked");
            this.Property(t => t.Mode).HasColumnName("Mode");
            this.Property(t => t.RefNo).HasColumnName("RefNo");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.JournalDetailId).HasColumnName("JournalDetailId");
            // Relationships
            //this.HasRequired(t => t.BankReconciliation)
            //    .WithMany(t => t.BankReconciliationDetails)
            //    .HasForeignKey(d => d.BankReconciliationId);
            //this.HasRequired(t => t.Entity)
            //    .WithMany(t => t.BankReconciliationDetails)
            //    .HasForeignKey(d => d.EntityId);

        }

    }
}
