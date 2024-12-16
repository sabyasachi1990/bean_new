using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.ReminderModule.Entities.V2Entities.V2Mappings
{
    public class SOAReminderBatchListDetailsMapping : EntityTypeConfiguration<SOAReminderBatchListDetailsEntity>
    {
        public SOAReminderBatchListDetailsMapping()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties


            // Table & Column Mappings
            this.ToTable("BeanSOAReminderBatchListDetails", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MasterId).HasColumnName("MasterId");
            this.Property(t => t.CreditNoteBalance).HasColumnName("CreditNoteBalance");

        }
    }
}
