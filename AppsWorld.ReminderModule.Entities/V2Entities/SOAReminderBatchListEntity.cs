using AppsWorld.Framework;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.ReminderModule.Entities.V2Entities
{
    public class SOAReminderBatchListEntity : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.Guid DocumentId { get; set; }
        public string ReminderType { get; set; }
        public string Name { get; set; }
        public string Recipient { get; set; }
        public Nullable<System.DateTime> JobExecutedOn { get; set; }
        public string JobStatus { get; set; }
        public bool? IsDismiss { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public decimal Custbalance { get; set; }
    }
}
