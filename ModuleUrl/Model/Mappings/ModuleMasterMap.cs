using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleUrl.Model.Mappings
{
	public class ModuleMasterMap : EntityTypeConfiguration<ModuleMaster>
	{
		public ModuleMasterMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(100);

			this.Property(t => t.Description)
				.HasMaxLength(1000);

			this.Property(t => t.Heading)
				.HasMaxLength(100);

			this.Property(t => t.CssSprite)
				.HasMaxLength(50);

			this.Property(t => t.FontAwesome)
				.HasMaxLength(50);

			this.Property(t => t.Url)
				.HasMaxLength(1000);

			this.Property(t => t.Remarks)
				.HasMaxLength(256);

			// Table & Column Mappings
			this.ToTable("ModuleMaster", "Common");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.ParentId).HasColumnName("ParentId");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.CompanyId).HasColumnName("CompanyId");
			this.Property(t => t.Description).HasColumnName("Description");
			this.Property(t => t.Heading).HasColumnName("Heading");
			this.Property(t => t.LogoId).HasColumnName("LogoId");
			this.Property(t => t.CssSprite).HasColumnName("CssSprite");
			this.Property(t => t.FontAwesome).HasColumnName("FontAwesome");
			this.Property(t => t.Url).HasColumnName("Url");
			this.Property(t => t.RecOrder).HasColumnName("RecOrder");
			this.Property(t => t.Remarks).HasColumnName("Remarks");
			this.Property(t => t.Status).HasColumnName("Status");

			// Relationships


		}
	}
}