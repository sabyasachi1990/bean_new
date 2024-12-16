using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class ContactMap : EntityTypeConfiguration<Contact>
    {
        public ContactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Salutation)
                .HasMaxLength(100);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .HasMaxLength(100);

            this.Property(t => t.IdType)
                .HasMaxLength(25);

            this.Property(t => t.IdNo)
                .HasMaxLength(100);

            this.Property(t => t.CountryOfResidence)
                .HasMaxLength(100);

            this.Property(t => t.Remarks)
                .HasMaxLength(256);


            // Table & Column Mappings
            this.ToTable("Contact", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Salutation).HasColumnName("Salutation");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.PhotoId).HasColumnName("PhotoId");
            this.Property(t => t.DOB).HasColumnName("DOB");
            this.Property(t => t.IdType).HasColumnName("IdType");
            this.Property(t => t.IdNo).HasColumnName("IdNo");
            this.Property(t => t.CountryOfResidence).HasColumnName("CountryOfResidence");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Remarks).HasColumnName("Remarks"); 
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Communication).HasColumnName("Communication");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

        }
    }
}
