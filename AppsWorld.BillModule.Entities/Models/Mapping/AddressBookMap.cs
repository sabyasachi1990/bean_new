using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BillModule.Entities
{
    public class AddressBookMap : EntityTypeConfiguration<AddressBook>
    {
        public AddressBookMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.BlockHouseNo)
                .HasMaxLength(256);

            this.Property(t => t.Street)
                .HasMaxLength(256);

            this.Property(t => t.UnitNo)
                .HasMaxLength(256);

            this.Property(t => t.BuildingEstate)
                .HasMaxLength(256);

            this.Property(t => t.City)
                .HasMaxLength(256);

            this.Property(t => t.PostalCode)
                .HasMaxLength(10);

            this.Property(t => t.State)
                .HasMaxLength(256);

            this.Property(t => t.Country)
                .HasMaxLength(256);

            this.Property(t => t.Phone)
                .HasMaxLength(1000);

            this.Property(t => t.Email)
                .HasMaxLength(1000);

            this.Property(t => t.Website)
                .HasMaxLength(1000);

            this.Property(t => t.Remarks)
                .HasMaxLength(256);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("AddressBook", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IsLocal).HasColumnName("IsLocal");
            this.Property(t => t.BlockHouseNo).HasColumnName("BlockHouseNo");
            this.Property(t => t.Street).HasColumnName("Street");
            this.Property(t => t.UnitNo).HasColumnName("UnitNo");
            this.Property(t => t.BuildingEstate).HasColumnName("BuildingEstate");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.PostalCode).HasColumnName("PostalCode");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Country).HasColumnName("Country");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Website).HasColumnName("Website");
            this.Property(t => t.Latitude).HasColumnName("Latitude");
            this.Property(t => t.Longitude).HasColumnName("Longitude");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
        }
    }
}
