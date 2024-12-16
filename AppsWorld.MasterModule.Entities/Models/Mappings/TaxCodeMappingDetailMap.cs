using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class TaxCodeMappingDetailMap : EntityTypeConfiguration<TaxCodeMappingDetail>
    {
        public TaxCodeMappingDetailMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties




            // Table & Column Mappings
            this.ToTable("TaxCodeMappingDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TaxCodeMappingId).HasColumnName("TaxCodeMappingId");
            this.Property(t => t.CustTaxId).HasColumnName("CustTaxId");
            this.Property(t => t.VenTaxId).HasColumnName("VenTaxId");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CustTaxCode).HasColumnName("CustTaxCode");
            this.Property(t => t.VenTaxCode).HasColumnName("VenTaxCode");
        }

    }
}
