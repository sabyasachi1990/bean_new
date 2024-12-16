using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Entities
{
    public class CreditMemoMap : EntityTypeConfiguration<CreditMemo>
    {
        public CreditMemoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocSubType)
                .HasMaxLength(20);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("CreditMemo", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
        }
    }
}
