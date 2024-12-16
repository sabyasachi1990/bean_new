using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.ReceiptModule.Entities.Mapping
{
   // public class BankReconciliationSettingMap : EntityTypeConfiguration<BankReconciliationSetting>
   // {
   //     public BankReconciliationSettingMap()
   //     {
   //         // Primary Key
   //         this.HasKey(t => t.Id);

   //         // Properties
   //         this.Property(t => t.Id)
   //             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

   //         //this.Property(t => t.UserCreated)
   //         //    .HasMaxLength(254);

   //         //this.Property(t => t.ModifiedBy)
   //         //    .HasMaxLength(254);

   //         // Table & Column Mappings
   //         this.ToTable("BankReconciliationSetting", "Bean");
   //         this.Property(t => t.Id).HasColumnName("Id");
   //         this.Property(t => t.CompanyId).HasColumnName("CompanyId");
   //         //this.Property(t => t.BankClearingDate).HasColumnName("BankClearingDate");
   //         this.Property(t => t.Status).HasColumnName("Status");
   //         //this.Property(t => t.UserCreated).HasColumnName("UserCreated");
   //         //this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
   //         //this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
   //         //this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

   //         // Relationships
			////this.HasRequired(t => t.Company)
			////	.WithMany(t => t.BankReconciliations)
			////	.HasForeignKey(d => d.CompanyId);

   //     }
   // }
}
