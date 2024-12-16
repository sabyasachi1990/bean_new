using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;

namespace AppsWorld.RevaluationModule.Entities.Models
{
    public partial class DebitNote : Entity
    {
        
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<int> Status { get; set; }
        public string DocSubType { get; set; }
        //public string DocType { get; set; }
        public string DocumentState { get; set; }

    }
}
