using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class InterCompanySettingDetailMap : EntityTypeConfiguration<InterCompanySettingDetail>
    {
        public InterCompanySettingDetailMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
       

            

            // Table & Column Mappings
            this.ToTable("InterCompanySettingDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.InterCompanySettingId).HasColumnName("InterCompanySettingId");
            this.Property(t => t.ServiceEntityId).HasColumnName("ServiceEntityId");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Status).HasColumnName("Status");
        }

    }
}
