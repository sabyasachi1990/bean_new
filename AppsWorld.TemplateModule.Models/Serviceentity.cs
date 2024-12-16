using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Models
{
   

    public class ServiceEntity
    {


        public string CompanyName { get; set; }
        public string RegistrationNo { get; set; }
        public string IdentificationType { get; set; }
        public string RegisteredAddress { get; set; }
        public string MailingAddress { get; set; }
        public string Entityaddress { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string SWIFTCode { get; set; }
        public string AccountName { get; set; }
        public string Currency { get; set; }
        public string GSTNumber { get; set; }
        public string EntityName { get; set; }

    }

    //public class PrimaryContact
    //{
    //    public string ContactSalutation { get; set; }
    //    public string ContactName { get; set; }
    //    public string Contactemail { get; set; }
    //}

    //public class Entity
    //{
    //    public string Entityname { get; set; }
    //    public string RegisteredAddress { get; set; }
    //    public string MailingAddress { get; set; }
    //}

    public class LineItem
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double? Quantity { get; set; }
        public string Unit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string TaxAmount { get; set; }
        // public decimal Total { get; set; }
        public string Discount { get; set; }
        public string Currency { get; set; }

        public string TaxCode { get; set; }
    }

    public class TaxCodeVM
    {
        public string TaxName { get; set; }
        public double? TaxRate { get; set; }


        public decimal? SubTotal { get; set; }
    }


}
