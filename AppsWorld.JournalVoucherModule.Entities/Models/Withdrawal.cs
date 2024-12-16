using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class Withdrawal : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string DocumentState { get; set; }
        public long COAId { get; set; }
        public string ModeOfWithDrawal { get; set; }
        public string WithDrawalRefNo { get; set; }
        public string DocDescription { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
    }
}
