using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BillModule.Entities
{
    public class LocalizationMap : EntityTypeConfiguration<Localization>
    {
        public LocalizationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LongDateFormat)
                .HasMaxLength(50);

            this.Property(t => t.ShortDateFormat)
                .HasMaxLength(50);

            this.Property(t => t.TimeFormat)
                .HasMaxLength(50);

            this.Property(t => t.BaseCurrency)
                .HasMaxLength(5);

            this.Property(t => t.BusinessYearEnd)
                .HasMaxLength(20);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.TimeZone)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Localization", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.LongDateFormat).HasColumnName("LongDateFormat");
            this.Property(t => t.ShortDateFormat).HasColumnName("ShortDateFormat");
            this.Property(t => t.TimeFormat).HasColumnName("TimeFormat");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.BusinessYearEnd).HasColumnName("BusinessYearEnd");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.TimeZone).HasColumnName("TimeZone");
            this.Property(t => t.DefaultWorkingHours).HasColumnName("DefaultWorkingHours");
        }
    }
}
