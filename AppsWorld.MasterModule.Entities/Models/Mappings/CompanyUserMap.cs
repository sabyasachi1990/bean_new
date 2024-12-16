using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
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

            // Table & Column Mappings
            this.ToTable("CompanyUser", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            // Relationships
            //this.HasRequired(t => t.UserAccount)
            //    .WithMany(t => t.CompanyUsers)
            //    .HasForeignKey(d => d.UserId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.CompanyUsers)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
