using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.ReceiptModule.Entities.Mapping
{
    public class CompanyMap : EntityTypeConfiguration<Company>
    {
        public CompanyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //this.Property(t => t.RegistrationNo)
            //    .HasMaxLength(50);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

          //  this.Property(t => t.BaseCurrency)
          //.HasMaxLength(5);

            this.Property(t => t.ShortName)
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("Company", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            //this.Property(t => t.RegistrationNo).HasColumnName("RegistrationNo");
            this.Property(t => t.Name).HasColumnName("Name");
            //this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            //this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.IsGstSetting).HasColumnName("IsGstSetting");
        }
    }
}
