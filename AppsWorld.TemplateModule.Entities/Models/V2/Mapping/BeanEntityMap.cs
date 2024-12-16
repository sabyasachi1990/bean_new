using AppsWorld.TemplateModule.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.V2.Mapping
{
    public class BeanEntityMap : EntityTypeConfiguration<AppsWorld.TemplateModule.Entities.Models.V2.BeanEntity>
    {
        public BeanEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.IdNo)
                .HasMaxLength(50);

            this.Property(t => t.GSTRegNo)
                .HasMaxLength(50);

           

            // Table & Column Mappings
            this.ToTable("Entity", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IdNo).HasColumnName("IdNo");
            this.Property(t => t.GSTRegNo).HasColumnName("GSTRegNo");
        }
    }
}
