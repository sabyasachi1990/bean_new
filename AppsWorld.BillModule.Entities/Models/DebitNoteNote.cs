using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.BillModule.Entities
{
    public partial class DebitNoteNote : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid DebitNoteId { get; set; }
        public Nullable<System.DateTime> ExpectedPaymentDate { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UserCreated_ { get; set; }

        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        //public virtual DebitNote DebitNote { get; set; }

        [NotMapped]
        public string RecordStatus;
    }
}