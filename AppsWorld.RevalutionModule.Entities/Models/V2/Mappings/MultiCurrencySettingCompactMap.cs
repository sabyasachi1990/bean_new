using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public class MultiCurrencySettingCompactMap : EntityTypeConfiguration<MultiCurrencySettingCompact>
    {
        public MultiCurrencySettingCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            

            // Table & Column Mappings
            this.ToTable("MultiCurrencySetting", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Revaluation).HasColumnName("Revaluation");
        }
    }
}
