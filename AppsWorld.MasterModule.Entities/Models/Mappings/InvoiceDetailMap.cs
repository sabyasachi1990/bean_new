using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class InvoiceDetailMap : EntityTypeConfiguration<InvoiceDetail>
    {
        public InvoiceDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            // Table & Column Mappings
            this.ToTable("InvoiceDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
            this.Property(t => t.DocAmount).HasColumnName("DocAmount");
            this.Property(t => t.DocTotalAmount).HasColumnName("DocTotalAmount");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.InvoiceDetails)
            //    .HasForeignKey(d => d.COAId);
            //this.HasRequired(t => t.Invoice)
            //    .WithMany(t => t.InvoiceDetails)
            //    .HasForeignKey(d => d.InvoiceId);
            //this.HasRequired(t => t.Item)
            //    .WithMany(t => t.InvoiceDetails)
            //    .HasForeignKey(d => d.ItemId);
            //this.HasRequired(t => t.TaxCode)
            //   .WithMany(t => t.InvoiceDetails)
            //   .HasForeignKey(d => d.TaxId);
        }
    }
}
