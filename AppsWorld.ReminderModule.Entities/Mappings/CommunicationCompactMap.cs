using AppsWorld.ReminderModule.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Mappings
{
    public class CommunicationCompactMap : EntityTypeConfiguration<CommunicationCompact>
    {
        public CommunicationCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.CommunicationType)
            //    .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(1000);

            this.Property(t => t.SentBy)
                .HasMaxLength(256);

            this.Property(t => t.FromMail)
                .HasMaxLength(256);

            this.Property(t => t.Subject)
                .HasMaxLength(1000);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.Remarks)
                .HasMaxLength(10000);

            this.Property(t => t.ReportPath)
                .HasMaxLength(100);

            this.Property(t => t.Category)
                .HasMaxLength(100);

            this.Property(t => t.TemplateName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Communication", "Common");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CommunicationType).HasColumnName("CommunicationType");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.SentBy).HasColumnName("SentBy");
            this.Property(t => t.FromMail).HasColumnName("FromMail");
            this.Property(t => t.ToMail).HasColumnName("ToMail");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.LeadId).HasColumnName("LeadId");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.ReportPath).HasColumnName("ReportPath");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.TemplateName).HasColumnName("TemplateName");
            this.Property(t => t.TemplateCode).HasColumnName("TemplateCode");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.FilePath).HasColumnName("FilePath");
            this.Property(t => t.AzurePath).HasColumnName("AzurePath");
            this.Property(t => t.InvoiceNumber).HasColumnName("InvoiceNumber");
            this.Property(t => t.TemplateContent).HasColumnName("TemplateContent");
            this.Property(t => t.CCMail).HasColumnName("CCMail");
            this.Property(t => t.BCCMail).HasColumnName("BCCMail");
        }
    }
}
