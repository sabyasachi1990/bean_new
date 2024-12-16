using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public class CompanyUserCompactMap : EntityTypeConfiguration<CompanyUserCompact>
    {
        public CompanyUserCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(254);



            // Table & Column Mappings
            this.ToTable("CompanyUser", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.ServiceEntities).HasColumnName("ServiceEntities");
        }
    }
}
