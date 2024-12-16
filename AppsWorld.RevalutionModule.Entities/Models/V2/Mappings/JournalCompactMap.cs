using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public class JournalCompactMap : EntityTypeConfiguration<JournalCompact>
    {
        public JournalCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocType)
                .HasMaxLength(50);

            this.Property(t => t.DocSubType)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Journal", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Journals)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
