using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class JournalDetailMap : EntityTypeConfiguration<JournalDetail>
    {
        public JournalDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
          

            // Table & Column Mappings
            this.ToTable("JournalDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.JournalId).HasColumnName("JournalId");
            this.Property(t => t.COAId).HasColumnName("COAId");
			this.Property(t => t.DocumentId).HasColumnName("DocumentId");
			this.Property(t => t.DocumentDetailId).HasColumnName("DocumentDetailId");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.JournalDetails)
            //    .HasForeignKey(d => d.COAId);
            ////this.HasRequired(t => t.Journal)
            ////    .WithMany(t => t.JournalDetails)
            //    .HasForeignKey(d => d.JournalId);
            //this.HasOptional(t => t.TaxCode)
            //    .WithMany(t => t.JournalDetails)
            //    .HasForeignKey(d => d.TaxId);

        }
    }
}
