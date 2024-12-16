using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class ActivityHistoryMap : EntityTypeConfiguration<ActivityHistory>
    {
        public ActivityHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.CreatedBy)
                .HasMaxLength(254);

            this.ToTable("ActivityHistory", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Activity).HasColumnName("Activity");
            this.Property(t => t.Action).HasColumnName("Action");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.DocumentId).HasColumnName("DocId");
        }

    }
}
