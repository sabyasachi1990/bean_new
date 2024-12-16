using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class OpeningBalance : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public long ServiceCompanyId { get; set; }
        public string DocType { get; set; }
        public Guid? PostedId { get; set; }
    }
}
