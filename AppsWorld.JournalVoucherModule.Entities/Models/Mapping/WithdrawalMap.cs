using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class WithdrawalMap : EntityTypeConfiguration<Withdrawal>
    {
        public WithdrawalMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocType)
                .IsRequired()
                .HasMaxLength(20);
            // Table & Column Mappings
            this.ToTable("Withdrawal", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.ModeOfWithDrawal).HasColumnName("ModeOfWithDrawal");
            this.Property(t => t.WithDrawalRefNo).HasColumnName("WithDrawalRefNo");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.Version).HasColumnName("Version");
        }
    }
}
