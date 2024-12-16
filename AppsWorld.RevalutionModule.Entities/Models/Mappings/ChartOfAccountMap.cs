using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Entities.Models.Mappings
{
    public class ChartOfAccountMap : EntityTypeConfiguration<ChartOfAccount>
    {
        public ChartOfAccountMap()
        {
            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CompanyId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Code)
                .HasMaxLength(100);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            //this.Property(t => t.AccountTypeId)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);          
            //this.Property(t => t.ModuleType)
            //    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ChartOfAccount", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IsRevaluation).HasColumnName("IsRevaluation");
            this.Property(t => t.Revaluation).HasColumnName("Revaluation");
            this.Property(t => t.ShowRevaluation).HasColumnName("ShowRevaluation");
            this.Property(t => t.IsBank).HasColumnName("IsBank");
            this.Property(t => t.AccountTypeId).HasColumnName("AccountTypeId");
            this.Property(t => t.Nature).HasColumnName("Nature");
        }
    }
}
