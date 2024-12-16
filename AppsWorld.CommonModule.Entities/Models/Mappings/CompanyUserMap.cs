using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Entities.Models.Mappings
{
    public class CompanyUserMap : EntityTypeConfiguration<CompanyUser>
    {
        public CompanyUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(254);



            // Table & Column Mappings
            this.ToTable("CompanyUser", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.ServiceEntities).HasColumnName("ServiceEntities");
        }
    }
}
