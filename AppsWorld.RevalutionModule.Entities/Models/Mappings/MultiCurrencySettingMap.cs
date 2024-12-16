using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Entities.Models.Mappings
{
    public class MultiCurrencySettingMap : EntityTypeConfiguration<MultiCurrencySetting>
    {
        public MultiCurrencySettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BaseCurrency)
                .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("MultiCurrencySetting", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.Revaluation).HasColumnName("Revaluation");      
        }
    }
}
