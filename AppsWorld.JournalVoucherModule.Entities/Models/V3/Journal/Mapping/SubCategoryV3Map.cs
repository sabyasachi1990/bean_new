using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal.Mapping
{
    public class SubCategoryV3Map : EntityTypeConfiguration<SubCategoryV3>
    {
        public SubCategoryV3Map()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.Type)
                .HasMaxLength(50);

            //this.Property(t => t.ColorCode)
            //    .HasMaxLength(100);

            //this.Property(t => t.AccountClass)
            //    .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("SubCategory", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
           // this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.TypeId).HasColumnName("TypeId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.IsIncomeStatement).HasColumnName("IsIncomeStatement");
            //this.Property(t => t.ColorCode).HasColumnName("ColorCode");
            //this.Property(t => t.AccountClass).HasColumnName("AccountClass");
            this.Property(t => t.Recorder).HasColumnName("Recorder");

            //this.HasOptional(t => t.CategoryV3)
            //   .WithMany(t => t.SubCategorys)
            //   .HasForeignKey(d => d.CategoryId);
        }
    }
}
