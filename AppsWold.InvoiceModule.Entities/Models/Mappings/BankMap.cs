using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.InvoiceModule.Entities.Models.Mappings
{
   
    public class BankMap : EntityTypeConfiguration<Bank>
    {
        public BankMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ShortCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.BranchCode)
                .HasMaxLength(50);

            this.Property(t => t.BranchName)
                .HasMaxLength(100);

            this.Property(t => t.AccountNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AccountName)
                .HasMaxLength(100);

            this.Property(t => t.SwiftCode)
                .HasMaxLength(50);

            this.Property(t => t.Currency)
                .HasMaxLength(5);

            this.Property(t => t.Remarks)
                .HasMaxLength(256);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.BankAddress)
                .HasMaxLength(4000);
            

            // Table & Column Mappings
            this.ToTable("Bank", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.SubcidaryCompanyId).HasColumnName("SubcidaryCompanyId");
            this.Property(t => t.ShortCode).HasColumnName("ShortCode");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.BranchCode).HasColumnName("BranchCode");
            this.Property(t => t.BranchName).HasColumnName("BranchName");
            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber");
            this.Property(t => t.AccountName).HasColumnName("AccountName");
            this.Property(t => t.SwiftCode).HasColumnName("SwiftCode");
            this.Property(t => t.AddressBookId).HasColumnName("AddressBookId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.Currency).HasColumnName("Currency");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.BankAddress).HasColumnName("BankAddress");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.Purpose).HasColumnName("Purpose");

            // Relationships
            //this.HasOptional(t => t.ChartOfAccount)
            //    .WithMany(t => t.Banks)
            //    .HasForeignKey(d => d.COAId);
            //this.HasOptional(t => t.AddressBook)
            //    .WithMany(t => t.Banks)
            //    .HasForeignKey(d => d.AddressBookId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Banks)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
