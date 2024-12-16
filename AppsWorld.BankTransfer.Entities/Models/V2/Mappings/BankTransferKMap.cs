using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.BankTransferModule.Entities.V2
{
    public class BankTransferKMap : EntityTypeConfiguration<BankTransferK>
    {
        public BankTransferKMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.ModeOfTransfer)
                .HasMaxLength(20);

            this.Property(t => t.TransferRefNo)
                .HasMaxLength(50);

            this.Property(t => t.DocumentState)
                .HasMaxLength(20);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.ExchangeRate)
              .HasPrecision(15, 10);

            // Table & Column Mappings
            this.ToTable("BankTransfer", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.TransferDate).HasColumnName("TransferDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.ModeOfTransfer).HasColumnName("ModeOfTransfer");
            this.Property(t => t.TransferRefNo).HasColumnName("TransferRefNo");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            // Relationships
            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.BankTranfers)
            //    .HasForeignKey(d => d.CompanyId);

        }
    }
}
