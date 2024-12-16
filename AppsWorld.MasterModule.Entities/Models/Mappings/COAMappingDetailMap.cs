using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class COAMappingDetailMap : EntityTypeConfiguration<COAMappingDetail>
    {
        public COAMappingDetailMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
       

            

            // Table & Column Mappings
            this.ToTable("COAMappingDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.COAMappingId).HasColumnName("COAMappingId");
            this.Property(t => t.CustCOAId).HasColumnName("CustCOAId");
            this.Property(t => t.VenCOAId).HasColumnName("VenCOAId");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Status).HasColumnName("Status");
        }

    }
}
