using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.RevaluationModule.Entities.Models.Mappings
{
    public class DebitNoteMap : EntityTypeConfiguration<DebitNote>
    {
        public DebitNoteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(20);          

            this.Property(t => t.DocSubType)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("DebitNote", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            //this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");


          

        }
    }
}
