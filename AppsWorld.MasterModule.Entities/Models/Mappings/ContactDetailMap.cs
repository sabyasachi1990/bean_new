using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class ContactDetailMap : EntityTypeConfiguration<ContactDetail>
    {
        public ContactDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.EntityType)
                .HasMaxLength(100);

            this.Property(t => t.Designation)
                .HasMaxLength(100);

            this.Property(t => t.Communication)
                .HasMaxLength(1000);

            this.Property(t => t.Matters)
                .HasMaxLength(1000);

            this.Property(t => t.OtherDesignation)
                .HasMaxLength(100);

            this.Property(t => t.CursorShortCode)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("ContactDetails", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ContactId).HasColumnName("ContactId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            this.Property(t => t.Designation).HasColumnName("Designation");
            this.Property(t => t.Communication).HasColumnName("Communication");
            this.Property(t => t.Matters).HasColumnName("Matters");
            this.Property(t => t.IsPrimaryContact).HasColumnName("IsPrimaryContact");
            this.Property(t => t.IsReminderReceipient).HasColumnName("IsReminderReceipient");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.OtherDesignation).HasColumnName("OtherDesignation");
            this.Property(t => t.IsPinned).HasColumnName("IsPinned");
            this.Property(t => t.IsCopy).HasColumnName("IsCopy");
            this.Property(t => t.CursorShortCode).HasColumnName("CursorShortCode");
            this.Property(t => t.DocId).HasColumnName("DocId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.Status).HasColumnName("Status");

            // Relationships
            this.HasRequired(t => t.Contact)
                .WithMany(t => t.ContactDetails)
                .HasForeignKey(d => d.ContactId);

        }
    }
}
