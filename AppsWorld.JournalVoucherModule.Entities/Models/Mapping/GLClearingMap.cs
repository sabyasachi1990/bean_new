using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class GLClearingMap : EntityTypeConfiguration<GLClearing>
    {
        public GLClearingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DocType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DocNo)
                .HasMaxLength(25);

            this.Property(t => t.DocDescription)
                .HasMaxLength(253);

            this.Property(t => t.SystemRefNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("GLClearing", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.DocType).HasColumnName("DocType");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.DocDescription).HasColumnName("DocDescription");
            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");
            this.Property(t => t.SystemRefNo).HasColumnName("SystemRefNo");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CrDr).HasColumnName("CrDr");
            this.Property(t => t.CheckAmount).HasColumnName("CheckAmount");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.COAId2).HasColumnName("COAId2");
            // Relationships
            //this.HasRequired(t => t.ChartOfAccount)
            //    .WithMany(t => t.GLClearings)
            //    .HasForeignKey(d => d.COAId);
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.GLClearings)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
