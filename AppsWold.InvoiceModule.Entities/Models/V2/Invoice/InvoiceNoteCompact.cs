using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    public partial class InvoiceNoteCompact : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid InvoiceId { get; set; }
        public Nullable<System.DateTime> ExpectedPaymentDate { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UserCreated { get; set; }

        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        //public virtual Invoice Invoice { get; set; }

        [NotMapped]
        public string RecordStatus;
    }
}
