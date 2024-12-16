using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Entities.Models.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);


            // Table & Column Mappings
            this.ToTable("Order", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.LeadSheetType).HasColumnName("LeadSheetType");
            this.Property(t => t.AccountClass).HasColumnName("AccountClass");
            this.Property(t => t.Recorder).HasColumnName("Recorder");
            this.Property(t => t.TypeId).HasColumnName("TypeId");
            this.Property(t => t.IsCollapse).HasColumnName("IsCollapse");
        }
    }
}
