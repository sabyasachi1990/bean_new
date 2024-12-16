using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AppsWorld.BankReconciliationModule.Entities;

namespace AppsWorld.BankReconciliationModule.Entities.Models.Mappings
{
    public class BankReconciliationMap : EntityTypeConfiguration<BankReconciliation>
    {
		public BankReconciliationMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Currency)
				.IsRequired()
				.HasMaxLength(5);

			this.Property(t => t.BankAccount)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.State)
				.IsRequired()
				.HasMaxLength(50);

			this.Property(t => t.UserCreated)
				.HasMaxLength(254);

			this.Property(t => t.ModifiedBy)
				.HasMaxLength(254);

			// Table & Column Mappings
			this.ToTable("BankReconciliation", "Bean");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.CompanyId).HasColumnName("CompanyId");
			this.Property(t => t.COAId).HasColumnName("COAId");
			this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
			this.Property(t => t.BankReconciliationDate).HasColumnName("BankReconciliationDate");
			this.Property(t => t.Currency).HasColumnName("Currency");
			this.Property(t => t.BankAccount).HasColumnName("BankAccount");
			this.Property(t => t.StatementAmount).HasColumnName("StatementAmount");
			//this.Property(t => t.OutstandingWithdrawals).HasColumnName("OutstandingWithdrawals");
			this.Property(t => t.SubTotal).HasColumnName("SubTotal");
			//this.Property(t => t.OutstandingDeposits).HasColumnName("OutstandingDeposits");
			this.Property(t => t.StatementExpectedAmount).HasColumnName("StatementExpectedAmount");
			this.Property(t => t.GLAmount).HasColumnName("GLAmount");
			this.Property(t => t.State).HasColumnName("State");
			this.Property(t => t.UserCreated).HasColumnName("UserCreated");
			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
			this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			this.Property(t => t.Version).HasColumnName("Version");
			this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StatementDate).HasColumnName("StatementDate");
            this.Property(t => t.IsDraft).HasColumnName("IsDraft");
            this.Property(t => t.IsReRunBR).HasColumnName("IsReRunBR");
			this.Property(t => t.IsLocked).HasColumnName("IsLocked");

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
