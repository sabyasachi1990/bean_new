﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Entities.Mappings
{
    public class DebitNoteCompactMap : EntityTypeConfiguration<DebitNoteCompact>
    {
        public DebitNoteCompactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.Nature)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.DocNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.DocCurrency)
                .HasMaxLength(5);

            this.Property(t => t.ExCurrency)
                .HasMaxLength(5);

            this.Property(t => t.DocumentState)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UserCreated)
                .HasMaxLength(254);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(254);

            this.Property(t => t.ExchangeRate)
                .HasPrecision(15, 10);

            // Table & Column Mappings
            this.ToTable("DebitNote", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.Nature).HasColumnName("Nature");
            this.Property(t => t.DocDate).HasColumnName("DocDate");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.DocNo).HasColumnName("DocNo");
            this.Property(t => t.DocCurrency).HasColumnName("DocCurrency");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.ExCurrency).HasColumnName("ExCurrency");
            this.Property(t => t.DocumentState).HasColumnName("DocumentState");
            this.Property(t => t.GrandTotal).HasColumnName("GrandTotal");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ServiceCompanyId).HasColumnName("ServiceCompanyId");
            this.Property(t => t.DebitNoteNumber).HasColumnName("DebitNoteNumber");
            this.Property(t => t.DocSubType).HasColumnName("DocSubType");
            this.Property(t => t.NoSupportingDocument).HasColumnName("NoSupportingDocument");
            this.Property(t => t.BalanceAmount).HasColumnName("BalanceAmount");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.BaseGrandTotal).HasColumnName("BaseGrandTotal");
            this.Property(t => t.BaseBalanceAmount).HasColumnName("BaseBalanceAmount");
            this.Property(t => t.RoundingAmount).HasColumnName("RoundingAmount");
        }
    }
}