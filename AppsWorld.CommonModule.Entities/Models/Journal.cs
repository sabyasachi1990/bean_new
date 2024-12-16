using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;using FrameWork;
using AppsWorld.Framework;
using Newtonsoft.Json.Converters;

namespace AppsWorld.CommonModule.Entities
{
    public partial class Journal:Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.Guid? DocumentId { get; set; }
        //public long? ServiceCompanyId { get; set; }
        public string SystemReferenceNo { get; set; }
        public string DocSubType { get; set; }
        public bool? IsWithdrawal { get; set; }
        public bool? IsAddNote { get; set; }
        public string DocumentState { get; set; }
        public string DocType { get; set; }
    }
}
