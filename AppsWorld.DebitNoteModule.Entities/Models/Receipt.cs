using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.DebitNoteModule.Entities
{
    public partial class Receipt : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public string SystemRefNo { get; set; }
        public string DocumentState { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
