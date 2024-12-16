using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.InvoiceModule.Entities.Models.Mappings
{
   // public class ProvisionMap : EntityTypeConfiguration<Provision>
   // {
   //     public ProvisionMap()
   //     {
   //         // Primary Key
   //         this.HasKey(t => t.Id);

   //         // Properties
   //         this.Property(t => t.DocumentType)
   //             .IsRequired()
   //             .HasMaxLength(20);

   //         this.Property(t => t.DocNo)
   //             .IsRequired()
   //             .HasMaxLength(25);

   //         this.Property(t => t.Remarks)
   //             .HasMaxLength(1000);

   //         this.Property(t => t.Currency)
   //             .IsRequired()
   //             .HasMaxLength(5);

   //         this.Property(t => t.SystemRefNo)
   //             .IsRequired()
   //             .HasMaxLength(25);

   //         this.Property(t => t.UserCreated)
   //             .HasMaxLength(254);

   //         this.Property(t => t.ModifiedBy)
   //             .HasMaxLength(254);

   //         // Table & Column Mappings
   //         this.ToTable("Provision", "Bean");
   //         this.Property(t => t.Id).HasColumnName("Id");
			//this.Property(t => t.RefDocumentId).HasColumnName("RefDocumentId");
			//this.Property(t => t.CompanyId).HasColumnName("CompanyId");
   //         this.Property(t => t.DocumentType).HasColumnName("DocumentType");
   //         this.Property(t => t.DocumentDate).HasColumnName("DocumentDate");
   //         this.Property(t => t.DocNo).HasColumnName("DocNo");
   //         this.Property(t => t.Remarks).HasColumnName("Remarks");
   //         this.Property(t => t.IsNoSupportingDocument).HasColumnName("IsNoSupportingDocument");
   //         this.Property(t => t.NoSupportingDocument).HasColumnName("NoSupportingDocument");
   //         this.Property(t => t.Currency).HasColumnName("Currency");
   //         this.Property(t => t.Provisionamount).HasColumnName("Provisionamount");
   //         this.Property(t => t.IsAllowableDisallowable).HasColumnName("IsAllowableDisallowable");
   //         this.Property(t => t.IsDisAllow).HasColumnName("IsDisAllow");
   //         this.Property(t => t.SystemRefNo).HasColumnName("SystemRefNo");
   //         this.Property(t => t.UserCreated).HasColumnName("UserCreated");
   //         this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
   //         this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
   //         this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
   //         this.Property(t => t.Status).HasColumnName("Status");
			//this.Property(t => t.RefDocType).HasColumnName("RefDocType");

   //         // Relationships
   //         //this.HasRequired(t => t.Invoice)
   //         //    .WithMany(t => t.Provisions)
   //         //    .HasForeignKey(d => d.InvoiceId);

   //     }
   // }
}
