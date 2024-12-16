using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BillModule.Entities
{
    public class UserAccountMap : EntityTypeConfiguration<UserAccount>
    {
        public UserAccountMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FirstName)
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .HasMaxLength(100);

            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(254);

            this.Property(t => t.Email)
                .HasMaxLength(254);

            this.Property(t => t.Role)
                .HasMaxLength(254);

            this.Property(t => t.Title)
               .HasMaxLength(10);

            this.Property(t => t.Gender)
                .HasMaxLength(10);

            this.Property(t => t.PhoneNo)
                .HasMaxLength(100);

            this.Property(t => t.Remarks)
                .HasMaxLength(256);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("UserAccount", "Auth");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.DOB).HasColumnName("DOB");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Role).HasColumnName("Role");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.PhotoId).HasColumnName("PhotoId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.PhoneNo).HasColumnName("PhoneNo");
            this.Property(t => t.DeactivationDate).HasColumnName("DeactivationDate");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");

            // Relationships


        }
    }
}
