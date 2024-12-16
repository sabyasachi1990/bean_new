using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.V2.Mapping
{
    public class AddressMap : EntityTypeConfiguration<AppsWorld.TemplateModule.Entities.Models.V2.Address>
    {
        public AddressMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.AddSectionType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AddType)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Addresses", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
          
            this.Property(t => t.AddSectionType).HasColumnName("AddSectionType");
            this.Property(t => t.AddType).HasColumnName("AddType");
            this.Property(t => t.AddTypeId).HasColumnName("AddTypeId");
            this.Property(t => t.AddTypeIdInt).HasColumnName("AddTypeIdInt");
            this.Property(t => t.AddressBookId).HasColumnName("AddressBookId");
            this.Property(t => t.Status).HasColumnName("Status");

            // Relationships
            this.HasOptional(t => t.AddressBook)
                .WithMany(t => t.Addresses)
                .HasForeignKey(d => d.AddressBookId);


        }
    }
}
