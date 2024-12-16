using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class DoctypeModel
    {
        public List<InvoiceDocumentDetailModel> InvoiceDocumentDetailModels { get; set; }
        public decimal? DoubtfulDebitTotalAmount { get; set; }
        public decimal? ReceiptTotalAmount { get; set; }
        public decimal? CreditNoteTotalAmount { get; set; }
        public decimal? AmountDue { get; set; }
        public decimal? PaymentTotalAmount { get; set; }
        public decimal? TransferTotalAmount { get; set; }
        //public  List<InvoiceCreditNoteModel> InvoiceCreditNoteModels { get; set; }

        //public  List<InvoiceDoubtFulDebtModel> InvoiceDoubtFulDebtModels { get; set; }
    }
}
