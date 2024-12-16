using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class SSICCodesMap : EntityTypeConfiguration<SSICCodes>
    {
        public SSICCodesMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(200);
            this.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(200);
            this.Property(t => t.Industry)
           .IsRequired()
           .HasMaxLength(8000);
            this.Property(t => t.Status);
            this.Property(t => t.CreatedDate);
            this.Property(t => t.UserCreated);
            this.Property(t => t.ModifiedDate);
            this.Property(t => t.ModifiedBy);

            // Table & Column Mappings
            this.ToTable("SSICCodes", "Boardroom");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Industry).HasColumnName("Industry");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
        }
    }
}
