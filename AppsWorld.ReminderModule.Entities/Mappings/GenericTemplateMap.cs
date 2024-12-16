using AppsWorld.ReminderModule.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Mappings
{
    public class GenericTemplateMap : EntityTypeConfiguration<GenericTemplateCompact>
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
            this.Property(t => t.CompanyId);
            this.Property(t => t.UserCreated)
                .HasMaxLength(254);
            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);
            this.Property(t => t.TempletContent)
                .IsMaxLength();

            // Table & Column Mappings
            this.ToTable("GenericTemplate", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.TemplateTypeId).HasColumnName("TemplateTypeId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.TempletContent).HasColumnName("TempletContent");
            this.Property(t => t.IsFooterExist).HasColumnName("IsFooterExist");
            this.Property(t => t.IsHeaderExist).HasColumnName("IsHeaderExist");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.BCCEmailIds).HasColumnName("BCCEmailIds");
            this.Property(t => t.FromEmailId).HasColumnName("FromEmailId");
            this.Property(t => t.ToEmailId).HasColumnName("ToEmailId");
            this.Property(t => t.CCEmailIds).HasColumnName("CCEmailIds");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.CursorName).HasColumnName("CursorName");
        }
    }
}
