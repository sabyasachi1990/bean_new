﻿using System;
using System.Collections.Generic;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;
using System.ComponentModel.DataAnnotations;
using AppsWorld.MasterModule.Entities;

namespace AppsWorld.MasterModule.Models
{
    public class ContactDetailModel
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public Guid? EntityId { get; set; }
        public string EntityType { get; set; }
        public string Designation { get; set; }
        public string CompanyName { get; set; }
        public string Communication { get; set; }
        public string Matters { get; set; }
        public bool? IsPrimaryContact { get; set; }        
        public bool? IsCopy { get; set; }        
        public string Remarks { get; set; }
        public Guid? DocId { get; set; }
        public string DocType { get; set; }
        public string UserCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsReminderReceipient { get; set; }
        public string OtherDesignation { get; set; }

        RecordStatusEnum _status = RecordStatusEnum.Active;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != null)
                    _status = value;
            }
        }
        public List<Address> Addresses { get; set; }
        public bool IsAssociate { get; set; }
    }
}