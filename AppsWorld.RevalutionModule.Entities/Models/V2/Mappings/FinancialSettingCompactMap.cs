using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public class FinancialSettingCompactMap : EntityTypeConfiguration<FinancialSettingCompact>
    {
        public FinancialSettingCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FinancialYearEnd)
                .HasMaxLength(25);

            this.Property(t => t.PeriodLockDatePassword)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FinancialSetting", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.FinancialYearEnd).HasColumnName("FinancialYearEnd");
            this.Property(t => t.PeriodLockDate).HasColumnName("PeriodLockDate");
            this.Property(t => t.PeriodLockDatePassword).HasColumnName("PeriodLockDatePassword");
            this.Property(t => t.EndOfYearLockDate).HasColumnName("EndOfYearLockDate");
            this.Property(t => t.PeriodEndDate).HasColumnName("PeriodEndDate");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
        }
    }
}
