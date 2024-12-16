using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.MasterModule.Entities
{
    //using Repository.Pattern.Ef6;
    //using System;
    //using System.Collections.Generic;
    //using System.ComponentModel.DataAnnotations;
    //using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    // [Table("Bean.InvoiceDetail")]
    public partial class InvoiceDetail : Entity
    {
        public InvoiceDetail()
        {
            //this.Item = new Item();
            // this.TaxCode = new TaxCode();
            //this.ChartOfAccount = new ChartOfAccount();
        }
        public System.Guid Id { get; set; }
        public System.Guid InvoiceId { get; set; }
        public decimal DocAmount { get; set; }
        public decimal? DocTotalAmount { get; set; }
        //[ForeignKey("ItemId")]
        public System.Guid ItemId { get; set; }
    }
}
