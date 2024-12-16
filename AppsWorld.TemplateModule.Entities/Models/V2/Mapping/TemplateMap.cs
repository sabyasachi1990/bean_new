using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.V2.Mapping
{
    public class TemplateMap : EntityTypeConfiguration<Template>
    {
        public TemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Code)
                .IsRequired();

            this.Property(t => t.FromEmailId)
                .HasMaxLength(100);

            this.Property(t => t.CCEmailIds)
                .HasMaxLength(500);

            this.Property(t => t.BCCEmailIds)
                .HasMaxLength(500);

            this.Property(t => t.TemplateType)
                .HasMaxLength(30);

            this.Property(t => t.Remarks)
                .HasMaxLength(256);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.Subject)
                .HasMaxLength(256);

            this.Property(t => t.TemplateMenu)
                .HasMaxLength(100);

            this.Property(t => t.ToEmailId)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Template", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.FromEmailId).HasColumnName("FromEmailId");
            this.Property(t => t.CCEmailIds).HasColumnName("CCEmailIds");
            this.Property(t => t.BCCEmailIds).HasColumnName("BCCEmailIds");
            this.Property(t => t.TemplateType).HasColumnName("TemplateType");
            this.Property(t => t.TempletContent).HasColumnName("TempletContent");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.TemplateMenu).HasColumnName("TemplateMenu");
            this.Property(t => t.ToEmailId).HasColumnName("ToEmailId");
            this.Property(t => t.IsUnique).HasColumnName("IsUnique");
            this.Property(t => t.CursorName).HasColumnName("CursorName");

            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Templates)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
