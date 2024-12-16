using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.TemplateModule.Entities.Models.V2.Mapping
{
   
    public class BankMap : EntityTypeConfiguration<AppsWorld.TemplateModule.Entities.Models.V2.Bank>
    {
        public BankMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
           

            this.Property(t => t.Name)
                .HasMaxLength(100);

           
            

            // Table & Column Mappings
            this.ToTable("Bank", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Purpose).HasColumnName("Purpose");


            // Relationships
            //this.HasOptional(t => t.ChartOfAccount)
            //    .WithMany(t => t.Banks)
            //    .HasForeignKey(d => d.COAId);
            //this.HasOptional(t => t.AddressBook)
            //    .WithMany(t => t.Banks)
            //    .HasForeignKey(d => d.AddressBookId);

        }
    }
}
