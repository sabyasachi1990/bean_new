using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class FinancialSettingMap : EntityTypeConfiguration<FinancialSetting>
    {
        public FinancialSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FinancialYearEnd)
                .HasMaxLength(25);

            this.Property(t => t.TimeZone)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PeriodLockDatePassword)
                .HasMaxLength(50);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.BaseCurrency)
                .HasMaxLength(50);

            this.Property(t => t.LongDateFormat)
                .HasMaxLength(50);

            this.Property(t => t.ShortDateFormat)
                .HasMaxLength(50);

            this.Property(t => t.TimeFormat)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FinancialSetting", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.FinancialYearEnd).HasColumnName("FinancialYearEnd");
            this.Property(t => t.TimeZone).HasColumnName("TimeZone");
            this.Property(t => t.PeriodLockDate).HasColumnName("PeriodLockDate");
            this.Property(t => t.PeriodLockDatePassword).HasColumnName("PeriodLockDatePassword");
            this.Property(t => t.EndOfYearLockDate).HasColumnName("EndOfYearLockDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.LongDateFormat).HasColumnName("LongDateFormat");
            this.Property(t => t.ShortDateFormat).HasColumnName("ShortDateFormat");
            this.Property(t => t.TimeFormat).HasColumnName("TimeFormat");
            this.Property(t => t.PeriodEndDate).HasColumnName("PeriodEndDate");
            this.Property(t => t.IsbaseCurrency).HasColumnName("IsbaseCurrency");
            this.Property(t => t.IsPosted).HasColumnName("IsPosted");

            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.FinancialSettings)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
