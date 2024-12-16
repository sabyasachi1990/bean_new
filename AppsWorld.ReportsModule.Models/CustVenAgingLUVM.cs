using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReportsModule.Models
{
    public class CustVenAgingLUVM
    {
        public CustVenAgingLUVM()
        {
            //ServiceEntities = new List<string>();
            //Entities = new List<string>();
            DocCurrencies = new List<string>();
            ServiceEntities = new List<ServiceEntity>();
            Entities = new List<Entity>();
           // Entities = new List<Entitys>();
        }
        //public List<string> ServiceEntities { get; set; }
        public List<string> Natures { get; set; }
        //public List<string> Entities { get; set; }
        public List<string> Currency { get; set; }
        //public List<string> Customers { get; set; }
        public List<string> CreditLimits { get; set; }
        public List<string> DocCurrencies { get; set; }
        public List<ServiceEntity> ServiceEntities { get; set; }

        public List<Entity> Entities { get; set; }
        //public List<Entitys> entities { get; set; }
        public DateTime? LastRefreshedDate { get; set; }
    }
    public class GeneralLegderLUVM
    {
        public GeneralLegderLUVM()
        {
            ServiceCompany = new List<ServiceEntity>();
            ChartofAccounts = new List<ChartOfAccount>();
            Doc_Type = new List<string>();
        }
        public List<ServiceEntity> ServiceCompany { get; set; }
        //public List<string> ServiceCompany { get; set; }
        public List<ChartOfAccount> ChartofAccounts { get; set; }
        public List<string> Doc_Type { get; set; }
    }
    public class FinancialYearLUVM
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
    public class ServiceEntity
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
    public class CommonClass
    {
        public string TableName { get; set; }
        public long? Id { get; set; }
        public Guid? EntityId { get; set; }
        public string Name { get; set; }
        public string SName { get; set; }
    }
    public class CompanySettings
    {
        public bool IsInterCompanyEnabled { get; set; }
    }

    public class currency
    {
        public string Name { get; set; }
    }
    public class Entity
    {
        //public Guid EntityId { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
    }
    public class ChartOfAccount
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public long? ServiceCompanyId { get; set; }
        public bool? IsBank { get; set; }

    }
    public class Entitys
    {
        public string EntityId { get; set; }
        //  public string Name { get; set; }
        public string EntityName { get; set; }
    }
    public class ServiceCompany
    {

        public long? Id { get; set; }
        public string Name { get; set; }
        // [JsonProperty("SName")]
        public string ShortName { get; set; }
    }
}
