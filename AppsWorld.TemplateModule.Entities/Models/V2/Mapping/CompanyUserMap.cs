 using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.V2.Mapping
{
    public class CompanyUserMap : EntityTypeConfiguration<CompanyUser>
    {
        public CompanyUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(254);

            this.Property(t => t.FirstName)
                .HasMaxLength(128);

            this.Property(t => t.LastName)
                .HasMaxLength(100);

            this.Property(t => t.Salutation)
                .HasMaxLength(100);

            this.Property(t => t.Remarks)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("CompanyUser", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Salutation).HasColumnName("Salutation");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserId).HasColumnName("UserId");
         

            //  this.Property(t => t.IsFavourite).HasColumnName("IsFavourite");


           

        }
    }
}
