using System;
using System.Collections.Generic;
using AppsWorld.JournalVoucherModule.Entities;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;using FrameWork;
using AppsWorld.Framework;
using Newtonsoft.Json.Converters;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class DocumentVoidModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string PeriodLockPassword { get; set; }
        public Guid? RecurringJournalId { get; set; }
        public string DocumentState { get; set; }
        public string Version { get; set; }
        public bool? IsDelete { get; set; }
    }
}
