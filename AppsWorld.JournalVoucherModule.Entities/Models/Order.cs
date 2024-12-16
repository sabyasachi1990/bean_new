using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class Order : Entity
    {
        public Guid Id { get; set; }
        public long? CompanyId { get; set; }
        public string LeadSheetType { get; set; }
        public string AccountClass { get; set; }
        public string Recorder { get; set; }
        public Guid? TypeId { get; set; }
        public bool? IsCollapse { get; set; }
    }
}
