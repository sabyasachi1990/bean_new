using AppsWorld.ReminderModule.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Mappings
{
    public class SOAReminderBatchListMap : EntityTypeConfiguration<SOAReminderBatchList>
    {
        public SOAReminderBatchListMap()
        {
            // Primary Key         
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ReminderType)
                .HasMaxLength(50);

            this.Property(t => t.JobStatus)
                .HasMaxLength(20);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);


            // Table & Column Mappings
            this.ToTable("BeanSOAReminderBatchList", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.ReminderType).HasColumnName("ReminderType");
            this.Property(t => t.Name).HasColumnName("Name");
            //this.Property(t => t.CustBal).HasColumnName("CustBal");
            this.Property(t => t.Recipient).HasColumnName("Recipient");
            this.Property(t => t.JobExecutedOn).HasColumnName("JobExecutedOn");
            this.Property(t => t.JobStatus).HasColumnName("JobStatus");
            this.Property(t => t.IsDismiss).HasColumnName("IsDismiss");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");
        }
    }
}
