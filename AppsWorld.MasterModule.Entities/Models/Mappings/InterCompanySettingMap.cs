﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities.Models.Mappings
{
    public class InterCompanySettingMap : EntityTypeConfiguration<InterCompanySetting>
    {
        public InterCompanySettingMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
       

            

            // Table & Column Mappings
            this.ToTable("InterCompanySetting", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.InterCompanyType).HasColumnName("InterCompanyType");
            this.Property(t => t.IsInterCompanyEnabled).HasColumnName("IsInterCompanyEnabled");
            this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
          

        }

    }
}