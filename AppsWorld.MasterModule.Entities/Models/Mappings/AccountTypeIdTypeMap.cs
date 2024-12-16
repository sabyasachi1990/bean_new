using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class AccountTypeIdTypeMap : EntityTypeConfiguration<AccountTypeIdType>
    {
        public AccountTypeIdTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("AccountTypeIdType", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AccountTypeId).HasColumnName("AccountTypeId");
            this.Property(t => t.IdTypeId).HasColumnName("IdTypeId");
            this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            

            // Relationships
            //this.HasRequired(t => t.IdType)
            //   .WithMany(t => t.AccountTypeIdTypes)
            //   .HasForeignKey(d => d.AccountTypeId);
            //this.HasRequired(t => t.IdType)
            //    .WithMany(t => t.AccountTypeIdTypes)
            //    .HasForeignKey(d => d.IdTypeId);

        }
    }
}
