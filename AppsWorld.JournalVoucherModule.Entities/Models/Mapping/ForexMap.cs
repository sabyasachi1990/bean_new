using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.JournalVoucherModule.Entities
{
    //public class ForexMap : EntityTypeConfiguration<Forex>
    //{
    //    public ForexMap()
    //    {
    //        // Primary Key
    //        this.HasKey(t => t.Id);

    //        // Properties
    //        this.Property(t => t.Id)
    //            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

    //        this.Property(t => t.Type)
    //            .IsRequired()
    //            .HasMaxLength(20);

    //        this.Property(t => t.Currency)
    //            .IsRequired()
    //            .HasMaxLength(10);

    //        this.Property(t => t.Remarks)
    //            .HasMaxLength(256);

    //        this.Property(t => t.UserCreated)
    //            .HasMaxLength(254);

    //        this.Property(t => t.ModifiedBy)
    //            .HasMaxLength(254);

    //        this.Property(t => t.UnitPerUSD)
    //            .IsRequired()
    //            .HasPrecision(15,10);

          

    //        // Table & Column Mappings
    //        this.ToTable("Forex", "Bean");
    //        this.Property(t => t.Id).HasColumnName("Id");
    //        //this.Property(t => t.CompanyId).HasColumnName("CompanyId");
    //        this.Property(t => t.Type).HasColumnName("Type");
    //        this.Property(t => t.DateFrom).HasColumnName("DateFrom");
    //        this.Property(t => t.Dateto).HasColumnName("Dateto");
    //        this.Property(t => t.Currency).HasColumnName("Currency");
    //        this.Property(t => t.UnitPerUSD).HasColumnName("UnitPerUSD");
    //        this.Property(t => t.USDPerUnit).HasColumnName("USDPerUnit");
    //        this.Property(t => t.UnitPerCal).HasColumnName("UnitPerCal");
    //        this.Property(t => t.Notes).HasColumnName("Notes");
    //        this.Property(t => t.RecOrder).HasColumnName("RecOrder");
    //        this.Property(t => t.Remarks).HasColumnName("Remarks");
    //        this.Property(t => t.UserCreated).HasColumnName("UserCreated");
    //        this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
    //        this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
    //        this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
    //        this.Property(t => t.Version).HasColumnName("Version");
    //        this.Property(t => t.Status).HasColumnName("Status");


    //        // Relationships
    //        //this.HasRequired(t => t.Company)
    //        //    .WithMany(t => t.Forexes)
    //        //    .HasForeignKey(d => d.CompanyId);

    //    }
    //}
}
