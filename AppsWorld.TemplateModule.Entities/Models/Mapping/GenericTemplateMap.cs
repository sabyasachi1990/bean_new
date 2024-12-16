using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.Mapping
{
    public class GenericTemplateMap : EntityTypeConfiguration<GenericTemplate>
    {
        public GenericTemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Remarks)
                .HasMaxLength(256);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);
            this.Property(t => t.Conditions)
                .HasMaxLength(600);
            this.Property(t => t.TempletContent)
                .IsMaxLength();

            // Table & Column Mappings
            this.ToTable("GenericTemplate", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.TemplateTypeId).HasColumnName("TemplateTypeId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.TempletContent).HasColumnName("TempletContent");
            this.Property(t => t.IsSystem).HasColumnName("IsSystem");
            this.Property(t => t.IsFooterExist).HasColumnName("IsFooterExist");
            this.Property(t => t.IsHeaderExist).HasColumnName("IsHeaderExist");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Conditions).HasColumnName("Conditions");
            this.Property(t => t.IsUsed).HasColumnName("IsUsed");
            this.Property(t => t.BCCEmailIds).HasColumnName("BCCEmailIds");
            this.Property(t => t.FromEmailId).HasColumnName("FromEmailId");
            this.Property(t => t.ToEmailId).HasColumnName("ToEmailId");
            this.Property(t => t.CCEmailIds).HasColumnName("CCEmailIds");
            this.Property(t => t.TemplateType).HasColumnName("TemplateType");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.IsPartnerTemplate).HasColumnName("IsPartnerTemplate");




           
           

        }
    }
}
