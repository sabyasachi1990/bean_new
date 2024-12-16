using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.ReminderModule.Entities.V2Entities.V2Mappings
{
    public class SOAReminderBatchListMapping : EntityTypeConfiguration<SOAReminderBatchListEntity>
    {
        public SOAReminderBatchListMapping()
        {
            // Primary Key         
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ReminderType)
                .HasMaxLength(50);

            this.Property(t => t.JobStatus)
                .HasMaxLength(20);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);


            // Table & Column Mappings
            this.ToTable("BeanSOAReminderBatchList", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.ReminderType).HasColumnName("ReminderType");
            this.Property(t => t.Name).HasColumnName("Name");
            //this.Property(t => t.CustBal).HasColumnName("CustBal");
            this.Property(t => t.Recipient).HasColumnName("Recipient");
            this.Property(t => t.JobExecutedOn).HasColumnName("JobExecutedOn");
            this.Property(t => t.JobStatus).HasColumnName("JobStatus");
            this.Property(t => t.IsDismiss).HasColumnName("IsDismiss");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Custbalance).HasColumnName("Custbalance");
        }
    }
}
