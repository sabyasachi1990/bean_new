using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CreditMemoModule.Entities.Mapping
{
    public class JournalMap : EntityTypeConfiguration<Journal>
    {
        public JournalMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.SystemReferenceNo)
                .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("Journal", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            //this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.SystemReferenceNo).HasColumnName("SystemReferenceNo");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.IsWithdrawal).HasColumnName("IsWithdrawal");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.BalanceAmount).HasColumnName("BalanceAmount");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Journals)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
