using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CashSalesModule.Entities.V2
{
    public class ChartOfAccountCompactMap : EntityTypeConfiguration<ChartOfAccountCompact>
    {
        public ChartOfAccountCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Code)
                .HasMaxLength(100);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            

            // Table & Column Mappings
            this.ToTable("ChartOfAccount", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.IsRealCOA).HasColumnName("IsRealCOA");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.Class).HasColumnName("Class");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.AccountTypeId).HasColumnName("AccountTypeId");
            // Relationships
            //this.HasRequired(t => t.AccountType)
            //    .WithMany(t => t.ChartOfAccounts)
            //    .HasForeignKey(d => d.AccountTypeId);

        }
    }
}
