using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class DebitNoteModel
    {
        public System.Guid? EntityId { get; set; }
        public string EntityName { get; set; }
		public string EntityType { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string CreditTermsName { get; set; }
        public string Nature { get; set; }
        public string PONo { get; set; }
        public virtual List<DebitNoteCreditNoteModel> DebitNoteCreditNoteModel { get; set; }
        public virtual List<DebitNoteDoubtFulDebitModel> DebitNoteDoubtFulDebitModel { get; set; }
    }
}
