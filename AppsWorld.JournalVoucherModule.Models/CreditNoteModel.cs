using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class CreditNoteModel
    {
        public System.Guid? EntityId { get; set; }
        public string EntityName { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string CreditTermsName { get; set; }
        public string Nature { get; set; }
		public string EntityType { get; set; }
	}
}
