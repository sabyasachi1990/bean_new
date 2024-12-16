using AppsWorld.ReminderModule.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Mappings
{
    public class LocalizationMap : EntityTypeConfiguration<LocalizationCompact>
    {
        public LocalizationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ShortDateFormat)
                .HasMaxLength(50);

            this.Property(t => t.TimeFormat)
                .HasMaxLength(50);

            this.Property(t => t.BaseCurrency)
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("Localization", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.ShortDateFormat).HasColumnName("ShortDateFormat");
            this.Property(t => t.TimeFormat).HasColumnName("TimeFormat");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
        }
    }
}
