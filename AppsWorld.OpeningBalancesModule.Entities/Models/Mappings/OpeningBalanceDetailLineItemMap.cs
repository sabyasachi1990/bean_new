
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Entities
{
    public class OpeningBalanceDetailLineItemMap : EntityTypeConfiguration<OpeningBalanceDetailLineItem>
    {
        public OpeningBalanceDetailLineItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Description)
                .HasMaxLength(150);

            this.Property(t => t.BaseCurrency)
                .HasMaxLength(20);

            this.Property(t => t.DocumentCurrency)
                .HasMaxLength(20);

            this.Property(t => t.ExchangeRate)
                .HasPrecision(15, 10);


            // Table & Column Mappings
            this.ToTable("OpeningBalanceDetailLineItem", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OpeningBalanceDetailId).HasColumnName("OpeningBalanceDetailId");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.BaseCredit).HasColumnName("BaseCredit");
            this.Property(t => t.BaseDebit).HasColumnName("BaseDebit");
            this.Property(t => t.DocumentCurrency).HasColumnName("DocumentCurrency");
            this.Property(t => t.DocCredit).HasColumnName("DocCredit");
            this.Property(t => t.DoCDebit).HasColumnName("DoCDebit");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.SegmentMasterid1).HasColumnName("SegmentMasterid1");
            this.Property(t => t.SegmentMasterid2).HasColumnName("SegmentMasterid2");
            this.Property(t => t.DocumentReference).HasColumnName("DocumentReference");
            this.Property(t => t.SegmentCategory1).HasColumnName("SegmentCategory1");
            this.Property(t => t.SegmentCategory2).HasColumnName("SegmentCategory2");
            this.Property(t => t.SegmentDetailid1).HasColumnName("SegmentDetailid1");
            this.Property(t => t.SegmentDetailid2).HasColumnName("SegmentDetailid2");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.IsDisAllow).HasColumnName("IsDisAllow");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.IsEditable).HasColumnName("IsEditable");
            this.Property(t => t.IsProcressed).HasColumnName("IsProcressed");
            this.Property(t => t.ProcressedRemarks).HasColumnName("ProcressedRemarks");
            // Relationships

            //this.HasRequired(t => t.OpeningBalanceDetail)
            //    .WithMany(t => t.OpeningBalanceDetailLineItems)
            //    .HasForeignKey(d => d.OpeningBalanceDetailId);
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.OpeningBalanceDetailLineItems)
            //    .HasForeignKey(d => d.COAId);
            //this.HasOptional(t => t.Entity)
            //    .WithMany(t => t.OpeningBalanceDetailLineItems)
            //    .HasForeignKey(d => d.EntityId);
        }
    }
}
