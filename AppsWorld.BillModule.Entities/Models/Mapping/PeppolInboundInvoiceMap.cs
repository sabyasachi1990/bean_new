using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Entities.Models.Mapping
{
    public class PeppolInboundInvoiceMap : EntityTypeConfiguration<PeppolInboundInvoice>
    {
        public PeppolInboundInvoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.UserCreated)
               .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("PeppolInboundInvoice", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DocId).HasColumnName("DocId");
            this.Property(t => t.SenderPeppolId).HasColumnName("SenderPeppolId");
            this.Property(t => t.ReciverPeppolId).HasColumnName("ReciverPeppolId");
            this.Property(t => t.XmlFilepath).HasColumnName("XmlFilepath");
            this.Property(t => t.XMLFileData).HasColumnName("XMLFileData");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage");
        }
    }
}
