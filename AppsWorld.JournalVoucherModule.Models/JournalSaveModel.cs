using System;
using System.Collections.Generic;
using AppsWorld.JournalVoucherModule.Entities;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using FrameWork;
using AppsWorld.Framework;
using Newtonsoft.Json.Converters;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JournalSaveModel
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string PeriodLockPassword { get; set; }
        public DateTime ReversalDate { get; set; }
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public bool? IsDocNoEditable { get; set; }
        public bool? IsCopyReversal { get; set; }
        public string ModifiedBy { get; set; }
        //public Guid? DocumentId { get; set; }
    }
}
