using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Entities.Models
{
    public class Journal : Entity
    {
        //public Journal()
        //{
        //    this.JournalDetails = new List<JournalDetail>();
        //}

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocumentState { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Guid? DocumentId { get; set; }
        public decimal? BalanceAmount { get; set; }
        //public virtual List<JournalDetail> JournalDetails { get; set; }
    }
}
