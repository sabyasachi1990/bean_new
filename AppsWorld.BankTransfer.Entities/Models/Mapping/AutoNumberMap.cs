using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BankTransferModule.Entities.Models.Mapping
{
    public class AutoNumberMap : EntityTypeConfiguration<AutoNumber>
    {
        public AutoNumberMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.EntityType)
                .IsRequired()
                .HasMaxLength(100);

            //this.Property(t => t.Description)
            //    .HasMaxLength(100);

            this.Property(t => t.Format)
                .HasMaxLength(100);

            this.Property(t => t.GeneratedNumber)
                .HasMaxLength(50);

            //this.Property(t => t.Reset)
            //    .HasMaxLength(20);

            this.Property(t => t.Preview)
                .HasMaxLength(50);

            //this.Property(t => t.Variables)
            // .HasMaxLength(256);

            //this.Property(t => t.UserCreated)
            //    .HasMaxLength(254);

            //this.Property(t => t.ModifiedBy)
            //    .HasMaxLength(254);

            // Table & Column Mappings
            //this.ToTable("AutoNumber", "Common");
            this.ToTable("AutoNumber", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            //this.Property(t => t.ModuleMasterId).HasColumnName("ModuleMasterId");
            this.Property(t => t.EntityType).HasColumnName("EntityType");
            //this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Format).HasColumnName("Format");
            this.Property(t => t.GeneratedNumber).HasColumnName("GeneratedNumber");
            this.Property(t => t.CounterLength).HasColumnName("CounterLength");
            //this.Property(t => t.MaxLength).HasColumnName("MaxLength");
            //this.Property(t => t.StartNumber).HasColumnName("StartNumber");
            //this.Property(t => t.Reset).HasColumnName("Reset");
            this.Property(t => t.Preview).HasColumnName("Preview");
            //this.Property(t => t.Variables).HasColumnName("Variables");
            this.Property(t => t.IsResetbySubsidary).HasColumnName("IsResetbySubsidary");
            //this.Property(t => t.Status).HasColumnName("Status");
            //this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            //this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            //this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            //this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.IsEditable).HasColumnName("IsEditable");
            //this.Property(t => t.IsDisable).HasColumnName("IsDisable");
            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.AutoNumbers)
            //    .HasForeignKey(d => d.CompanyId);
            //this.HasOptional(t => t.ModuleMaster)
            //    .WithMany(t => t.AutoNumbers)
            //    .HasForeignKey(d => d.ModuleMasterId);

        }
    }
}
