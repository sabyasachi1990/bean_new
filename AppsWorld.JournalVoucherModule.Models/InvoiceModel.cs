using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class InvoiceModel
    {
       public System.Guid? EntityId { get; set; }
       public string EntityName { get; set; }

	   public string EntityType { get; set; }
       public Nullable<System.DateTime> DueDate { get; set; }
       public string CreditTermsName { get; set; }
       public string Nature { get; set; }
       public string PONo { get; set; }
       public bool? IsRepeatingInvoice { get; set; }
       public Nullable<int> RepEveryPeriodNo { get; set; }
       public string RepEveryPeriod { get; set; }
       public Nullable<System.DateTime> EndDate { get; set; }
	   public virtual List<InvoiceCreditNoteModel> InvoiceCreditNoteModel { get; set; }
	   public virtual List<InvoiceDoubtFulDebitModel> InvoiceDoubtFulDebitModel { get; set; }
    }
}
