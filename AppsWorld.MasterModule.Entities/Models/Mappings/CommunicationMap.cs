﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
   public class CommunicationMap: EntityTypeConfiguration<Communication>
    {
        public CommunicationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Remarks)
               .HasMaxLength(256);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            // Table & Column Mappings
            this.ToTable("Communication", "Common");
            this.Property(t => t.Id).HasColumnName("Id");            
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.CommunicationType).HasColumnName("CommunicationType");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.SentBy).HasColumnName("SentBy");
            this.Property(t => t.FromMail).HasColumnName("FromMail");
            this.Property(t => t.ToMail).HasColumnName("ToMail");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.TemplateName).HasColumnName("TemplateName");
            this.Property(t => t.TemplateCode).HasColumnName("TemplateCode");
            this.Property(t => t.ReportPath).HasColumnName("ReportPath");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
        }

    }
}