
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.V2.Mapping
{
    public class TermsOfPaymentMap : EntityTypeConfiguration<TermsOfPayment>
    {
        public TermsOfPaymentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.TermsType)
                .HasMaxLength(20);
                      

            // Table & Column Mappings
            this.ToTable("TermsOfPayment", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.TermsType).HasColumnName("TermsType");
            this.Property(t => t.TOPValue).HasColumnName("TOPValue");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");          
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.IsCustomer).HasColumnName("IsCustomer");
            this.Property(t => t.IsVendor).HasColumnName("IsVendor");

            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.TermsOfPayments)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
