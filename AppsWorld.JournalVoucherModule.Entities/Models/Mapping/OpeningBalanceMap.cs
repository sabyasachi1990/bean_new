using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Entities.Models.Mapping
{
    public class OpeningBalanceMap : EntityTypeConfiguration<OpeningBalance>
    {
        public OpeningBalanceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.DocType)
                .HasMaxLength(50);

             
            // Table & Column Mappings
            this.ToTable("OpeningBalance", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.PostedId).HasColumnName("PostedId");
        }
    }
}
