using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class CommonForexMap: EntityTypeConfiguration<CommonForex>
    {
        public CommonForexMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
                      

            this.Property(t => t.FromForexRate)
               .IsRequired()
               .HasPrecision(15, 10);

            this.Property(t => t.ToForexRate)
                .HasPrecision(15, 10);
            // Table & Column Mappings
            this.ToTable("Forex", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Source).HasColumnName("Source");
            this.Property(t => t.DateFrom).HasColumnName("DateFrom");
            this.Property(t => t.Dateto).HasColumnName("Dateto");
            this.Property(t => t.FromCurrency).HasColumnName("FromCurrency");
            this.Property(t => t.ToCurrency).HasColumnName("ToCurrency");
            this.Property(t => t.FromForexRate).HasColumnName("FromForexRate");
            this.Property(t => t.ToForexRate).HasColumnName("ToForexRate");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");


            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.Forexes)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
