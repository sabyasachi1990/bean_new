﻿using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.MasterModule.Entities
{
    public partial class Currency : Entity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public Nullable<int> RecOrder { get; set; }
        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string DefaultValue { get; set; }
        //  public virtual Company Company { get; set; }
        [NotMapped]
        public string DefaultCurrency { get; set; }
    }
}