using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Entities
{
    public class OpeningBalanceMap : EntityTypeConfiguration<OpeningBalance>
    {
        public OpeningBalanceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.BaseCurrency)
                .HasMaxLength(5);

            //this.Property(t => t.CreatedBy)
            //    .HasMaxLength(20);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(50);

            this.Property(t => t.DocType)
                .HasMaxLength(50);

            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("OpeningBalance", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            ////this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.SaveType).HasColumnName("SaveType");
            this.Property(t => t.SystemRefNo).HasColumnName("SystemRefNo");
            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");
            this.Property(t => t.IsNoSupportingDoc).HasColumnName("IsNoSupportingDoc");
            this.Property(t => t.IsSegmentReporting).HasColumnName("IsSegmentReporting");
            this.Property(t => t.IsEditable).HasColumnName("IsEditable");
            this.Property(t => t.PostedId).HasColumnName("PostedId");
            this.Property(t => t.IsLocked).HasColumnName("IsLocked");
            this.Property(t => t.IsTemporary).HasColumnName("IsTemporary");
            // this.Property(t => t.Recorder).HasColumnName("Recorder");

            // Relationships

            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.OpeningBalances)
            //    .HasForeignKey(d => d.CompanyId);
        }
    }
}
