using AppsWorld.ReminderModule.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Mappings
{
    public class SOAReminderBatchListDetailsMap : EntityTypeConfiguration<SOAReminderBatchListDetails>
    {
        public SOAReminderBatchListDetailsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties


            // Table & Column Mappings
            this.ToTable("BeanSOAReminderBatchListDetails", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MasterId).HasColumnName("MasterId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            //this.Property(t => t.CustBal).HasColumnName("CustBal");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.DocumentTotal).HasColumnName("DocumentTotal");
            this.Property(t => t.CreditNoteBalance).HasColumnName("CreditNoteBalance");
            this.Property(t => t.DocBalance).HasColumnName("DocBalance");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.Currency).HasColumnName("Currency");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.Status).HasColumnName("Status");

            //RelationShip
            this.HasRequired(t => t.SOAReminderBatchList)
            .WithMany(t => t.ReminderBatchListDetails)
            .HasForeignKey(d => d.MasterId);
        }
    }
}
