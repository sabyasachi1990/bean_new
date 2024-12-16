using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.Mapping
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

            this.Property(t => t.RegistrationNo)
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(254);

            this.Property(t => t.CssSprite)
                .HasMaxLength(50);

            this.Property(t => t.BaseCurrency)
                .HasMaxLength(5);

            this.Property(t => t.Remarks)
                .HasMaxLength(254);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.ShortName)
                .HasMaxLength(5);

            this.Property(t => t.Communication)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Company", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.RegistrationNo).HasColumnName("RegistrationNo");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.LogoId).HasColumnName("LogoId");
            this.Property(t => t.CssSprite).HasColumnName("CssSprite");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.AddressBookId).HasColumnName("AddressBookId");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Communication).HasColumnName("Communication");
            this.Property(t => t.IsGstSetting).HasColumnName("IsGstSetting");


        }
    }
}
