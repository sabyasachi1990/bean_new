using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal.Mapping
{
    public class AccountTypeV3Map : EntityTypeConfiguration<AccountTypeV3>
    {
        public AccountTypeV3Map()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Class)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Category)
                .HasMaxLength(100);

            this.Property(t => t.SubCategory)

                .HasMaxLength(100);

            //this.Property(t => t.Nature)

            //    .HasMaxLength(100);

            //this.Property(t => t.AppliesTo)
            //    .HasMaxLength(50);

            //this.Property(t => t.Remarks)
            //    .HasMaxLength(256);

            //this.Property(t => t.UserCreated)
            //    .HasMaxLength(254);

            //this.Property(t => t.ModifiedBy)
            //    .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("AccountType", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Class).HasColumnName("Class");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.SubCategory).HasColumnName("SubCategory");
            //this.Property(t => t.Nature).HasColumnName("Nature");
            //this.Property(t => t.AppliesTo).HasColumnName("AppliesTo");
            //this.Property(t => t.IsSystem).HasColumnName("IsSystem");
            //this.Property(t => t.ShowCurrency).HasColumnName("ShowCurrency");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            //this.Property(t => t.Remarks).HasColumnName("Remarks");
            //this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            //this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            //this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            //this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            //this.Property(t => t.Version).HasColumnName("Version");
            //this.Property(t => t.Status).HasColumnName("Status");
            //this.Property(t => t.ShowAllowable).HasColumnName("ShowAllowable");
            //this.Property(t => t.ShowCashflowType).HasColumnName("ShowCashflowType");
            //this.Property(t => t.ShowRevaluation).HasColumnName("ShowRevaluation");
            //this.Property(t => t.Indexs).HasColumnName("Indexs");
            //this.Property(t => t.ModuleType).HasColumnName("ModuleType");
            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.AccountTypes)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
