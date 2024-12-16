
using AppsWorld.TemplateModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Models
{
    public class MustacheModel
    {
        public InvoiceTemplateVM Invoice { get; set; }
        public InvoiceTemplateVM CreditNote { get; set; }
        public ServiceEntity ServiceEntity { get; set; }
        public ReceiptsModel receiptModel { get; set; }
        public StatementModel StatementModel { get; set; }
        public List<ReceiptDetaislModel> ReceiptDetailsModel { get; set; }


    }
    public class InvoiceTemplateVM
    {
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string DueDate { get; set; }
        public decimal Total { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? ExcludeGST { get; set; }
        public decimal? GSTPayable { get; set; }
        public decimal? IncludeGST { get; set; }
        public List<LineItem> LineItem { get; set; }
        public List<TaxCodeVM> TaxCode { get; set; }
    }


}
