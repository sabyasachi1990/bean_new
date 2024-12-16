
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class InvoiceCreditNoteModel
    {
      
        public System.Guid CreditNoteId { get; set; }
       
        public System.DateTime DocDate { get; set; }
        
        public string DocNo { get; set; }

		public string SystemRefNo { get; set; }

		public decimal? Ammount { get; set; }
        public string DocType { get; set; }
    }
}
