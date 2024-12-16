using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class AddressMap : EntityTypeConfiguration<Address>
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
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            // Relationships
            //this.HasOptional(t => t.AddressBook)
            //    .WithMany(t => t.Addresses)
            //    .HasForeignKey(d => d.AddressBookId);
        }
    }
}
