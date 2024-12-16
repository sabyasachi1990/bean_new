using AppsWorld.BillModule.Entities;
using AppsWorld.InvoiceModule.Entities.V2;
using AppsWorld.TemplateModule.Models.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Models.Models
{
    public class InvoiceModel
    {
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string DocType { get; set; }
        public string Currency { get; set; }
        public string Remarks { get; set; }
        public string DocumentTotal { get; set; }
        public string DocBalance { get; set; }
        public string SubTotal { get; set; }
    }

    public class TemplateMenuModel
    {
        public TemplateMenuModel()
        {
            this.SoaDetail = new List<Models.SOAOutstandingAmount>();
            this.OutstandingTotal = new List<Models.OutstandingTotals>();
        }
        public StatementModel StatementModel { get; set; }
        public BillModule.Entities.Invoice Invoice { get; set; }
        public EntityModel Entity { get; set; }
        public CompanyModel ServiceEntity { get; set; }
        public CommonVariablesModel Common { get; set; }
        public Nullable<decimal> TotalOutstandingBalanceFee { get; set; }
        public List<SOAOutstandingAmount> SoaDetail { get; set; }
        public List<OutstandingTotals> OutstandingTotal { get; set; }
        public bool? IsGSTActive { get; set; }
        public bool? IsGSTNotActive { get; set; }
        public string StatementDate { get; set; }
        public string RegisteredBlock { get; set; }
        public string RegisteredBuilding { get; set; }
        public string RegisteredCountry { get; set; }
        public string RegisteredPostalCode { get; set; }
        public string RegisteredStreet { get; set; }
        public string RegisteredUnit { get; set; }
    }
    public class CommonVariablesModel
    {
        public string Date { get; set; }
        public string UserName { get; set; }
        public string Stripe { get; set; }
    }

    public class EntityModel
    {
        public string Entityname { get; set; }
        public string RegisteredAddress { get; set; }
        public string MailingAddress { get; set; }
        public string RegisteredBlock { get; set; }
        public string RegisteredStreet { get; set; }
        public string RegisteredUnit { get; set; }
        public string RegisteredBuilding { get; set; }
        public string RegisteredCountry { get; set; }
        public string RegisteredPostalCode { get; set; }
    }
}
