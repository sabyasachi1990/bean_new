using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Entities.Mapping
{
   public  class DocRepositoryMap: EntityTypeConfiguration<DocRepository>
    {
        public DocRepositoryMap()
    {
        // Primary Key
        this.HasKey(t => t.Id);

        // Properties
        this.Property(t => t.Type)
            .HasMaxLength(50);

        this.Property(t => t.TypeKey)
            .HasMaxLength(50);

        this.Property(t => t.ModuleName)
            .HasMaxLength(50);

        this.Property(t => t.DisplayFileName)
            .HasMaxLength(500);

        this.Property(t => t.Description)
            .HasMaxLength(500);

        this.Property(t => t.FileExt)
            .HasMaxLength(7);

        this.Property(t => t.FileSize)
            .HasPrecision(15, 8);

        this.Property(t => t.UserCreated)
            .HasMaxLength(254);

        this.Property(t => t.ModifiedBy)
            .HasMaxLength(254);

        // Table & Column Mappings
        this.ToTable("DocRepository", "Common");
        this.Property(t => t.Id).HasColumnName("Id");
        this.Property(t => t.CompanyId).HasColumnName("CompanyId");
        this.Property(t => t.TypeId).HasColumnName("TypeId");
        this.Property(t => t.Type).HasColumnName("Type");
        this.Property(t => t.TypeKey).HasColumnName("TypeKey");
        this.Property(t => t.ModuleName).HasColumnName("ModuleName");
        this.Property(t => t.FilePath).HasColumnName("FilePath");
        this.Property(t => t.DisplayFileName).HasColumnName("DisplayFileName");
        this.Property(t => t.Description).HasColumnName("Description");
        this.Property(t => t.FileSize).HasColumnName("FileSize");
        this.Property(t => t.FileExt).HasColumnName("FileExt");
        this.Property(t => t.RecOrder).HasColumnName("RecOrder");
        this.Property(t => t.UserCreated).HasColumnName("UserCreated");
        this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
        this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
        this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
        this.Property(t => t.Version).HasColumnName("Version");
        this.Property(t => t.Status).HasColumnName("Status");
        this.Property(t => t.FilePath).HasColumnName("FilePath");
        this.Property(t => t.TypeIntId).HasColumnName("TypeIntId");
        this.Property(t => t.NameofApprovalAuthority).HasColumnName("NameofApprovalAuthority");
        this.Property(t => t.MongoFilesId).HasColumnName("MongoFilesId");

		  //Relationships
		  //this.HasRequired(t => t.Company)
		  //    .WithMany(t => t.DocRepositories)
		  //    .HasForeignKey(d => d.CompanyId);

	   }

    }
}
