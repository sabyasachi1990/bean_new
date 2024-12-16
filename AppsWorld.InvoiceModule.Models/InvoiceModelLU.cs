
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.Entities.Models;
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
    public class InvoiceModelLU
    {
        public long CompanyId { get; set; }

        //public LookUpCategory<string> SegmentCategory1LU { get; set; }
        //public LookUpCategory<string> SegmentCategory2LU { get; set; }

        //public List<LookUpCategory<string>> ItemLU { get; set; }
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public LookUpCategory<string> CurrencyLU { get; set; }
        //public LookUpCategory<string> NatureLU { get; set; }
        public List<string> NatureLU { get; set; }
        //public LookUpCategory<string> AllowableNonallowableLU { get; set; }
        //public LookUpCategory<string> DocumentTypeLU { get; set; }
        public List<LookUp<string>> TermsOfPaymentLU { get; set; }

		public List<COALookup<string>> ChartOfAccountLU { get; set; }

        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        //public LookUpCategory<string> DocumentSubType { get; set; }
        //[ForeignKey("EntityId")]
        //public virtual BeanEntity BeanEntity { get; set; }
        //[ForeignKey("CreditTermsId")]
        //public virtual TermsOfPayment TermsOfPayments { get; set; }
        //public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }

    }
}
