using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class BankReconciliationMap : EntityTypeConfiguration<BankReconciliation>
    {
		public BankReconciliationMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			 

			// Table & Column Mappings
			this.ToTable("BankReconciliation", "Bean");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.CompanyId).HasColumnName("CompanyId");
			this.Property(t => t.COAId).HasColumnName("COAId");
			this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
			this.Property(t => t.BankReconciliationDate).HasColumnName("BankReconciliationDate");
            this.Property(t => t.IsReRunBR).HasColumnName("IsReRunBR");
            this.Property(t => t.State).HasColumnName("State");

            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.BankReconciliations)
            //    .HasForeignKey(d => d.COAId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.BankReconciliations)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
