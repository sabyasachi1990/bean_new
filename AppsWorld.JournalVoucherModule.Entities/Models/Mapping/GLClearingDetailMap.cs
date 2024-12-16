using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class GLClearingDetailMap : EntityTypeConfiguration<GLClearingDetail>
    {
        public GLClearingDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.SystemRefNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BaseCurrency)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.CrDr)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("GLClearingDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.GLClearingId).HasColumnName("GLClearingId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.SystemRefNo).HasColumnName("SystemRefNo");
            this.Property(t => t.DocAmount).HasColumnName("DocAmount");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.CrDr).HasColumnName("CrDr");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.IsCheck).HasColumnName("IsCheck");

            // Relationships
            //this.HasRequired(t => t.GLClearing)
            //    .WithMany(t => t.GLClearingDetails)
            //    .HasForeignKey(d => d.GLClearingId);

        }
    }
}
