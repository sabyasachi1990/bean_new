using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Entities.Models.Mappings
{
    public class CompanyUserDetailMap : EntityTypeConfiguration<CompanyUserDetail>
    {
        public CompanyUserDetailMap()
        {
            //Primary Key
            this.HasKey(t => t.Id);

            this.ToTable("CompanyUserDetail", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyUserId).HasColumnName("CompanyUserId");
            this.Property(t => t.ServiceEntityId).HasColumnName("ServiceEntityId");
            //this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
