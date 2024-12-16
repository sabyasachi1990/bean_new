using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.RevaluationModule.Entities.Models.Mappings
{
    public class RevalutionDetailMap : EntityTypeConfiguration<RevalutionDetail>
    {
        public RevalutionDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SystemReferenceNumber)
                .HasMaxLength(100);

            this.Property(t => t.DocumentType)
                .HasMaxLength(100);

            this.Property(t => t.DocumentSubType)
                .HasMaxLength(100);

            this.Property(t => t.DocumentNumber)
                .HasMaxLength(100);

            this.Property(t => t.DocumentDescription)
                .HasMaxLength(254);

            this.Property(t => t.SegmentCategory1)
                .HasMaxLength(100);

            this.Property(t => t.SegmentCategory2)
                .HasMaxLength(100);

            this.Property(t => t.BaseCurrency)
                .HasMaxLength(5);

            this.Property(t => t.DocCurrency)
                .HasMaxLength(5);


            this.Property(t => t.UserCreated)
                .HasMaxLength(254);


            // Table & Column Mappings
            this.ToTable("RevalutionDetail", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RevalutionId).HasColumnName("RevalutionId");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.SystemReferenceNumber).HasColumnName("SystemReferenceNumber");
            this.Property(t => t.DocumentType).HasColumnName("DocumentType");
            this.Property(t => t.DocumentSubType).HasColumnName("DocumentSubType");
            this.Property(t => t.DocumentNumber).HasColumnName("DocumentNumber");
            this.Property(t => t.DocumentDescription).HasColumnName("DocumentDescription");
            this.Property(t => t.SegmentCategory1).HasColumnName("SegmentCategory1");
            this.Property(t => t.SegmentCategory2).HasColumnName("SegmentCategory2");
            this.Property(t => t.DocumentDate).HasColumnName("DocumentDate");
            this.Property(t => t.DocId).HasColumnName("DocId");
            this.Property(t => t.ExchangerateOld).HasColumnName("ExchangerateOld");
            this.Property(t => t.ExchangerateNew).HasColumnName("ExchangerateNew");
            this.Property(t => t.BaseCurrency).HasColumnName("BaseCurrency");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.DocCurrencyAmount).HasColumnName("DocCurrencyAmount");
            this.Property(t => t.BaseCurrencyAmount1).HasColumnName("BaseCurrencyAmount1");
            this.Property(t => t.BaseCurrencyAmount2).HasColumnName("BaseCurrencyAmount2");
            this.Property(t => t.UnrealisedExchangegainorlose).HasColumnName("UnrealisedExchangegainorlose");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.PostingDate).HasColumnName("PostingDate");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.DocBal).HasColumnName("DocBal");
            this.Property(t => t.ServiceEntityId).HasColumnName("ServiceEntityId");
            this.Property(t => t.IsChecked).HasColumnName("IsChecked");
            // Relationships
            //this.HasRequired(t => t.Revalution)
            //    .WithMany(t => t.RevalutionDetails)
            //    .HasForeignKey(d => d.RevalutionId);
        }
    }
}
