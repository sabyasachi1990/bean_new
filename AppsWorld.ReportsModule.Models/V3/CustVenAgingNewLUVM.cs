using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReportsModule.Models.V3
{
    public class CustVenAgingNewLUVM
    {
        public CustVenAgingNewLUVM()
        {
            DocCurrencies = new List<string>();
            ServiceEntities = new List<ServiceEntityNew>();
            //Entities = new List<Entity>();

        }
        public List<string> Natures { get; set; }
        public List<string> Currency { get; set; }
        public List<string> CreditLimits { get; set; }
        public List<string> DocCurrencies { get; set; }
        public List<ServiceEntityNew> ServiceEntities { get; set; }
      //  public List<Entity> Entities { get; set; }
        public DateTime? LastRefreshedDate { get; set; }
    }
    public class GeneralLegderNewLUVM
    {
        public GeneralLegderNewLUVM()
        {
            ServiceCompany = new List<ServiceEntityNew>();
            ChartofAccounts = new List<ChartOfAccountNew>();
            Doc_Type = new List<string>();
        }
        public List<ServiceEntityNew> ServiceCompany { get; set; }
        public List<ChartOfAccountNew> ChartofAccounts { get; set; }
        public List<string> Doc_Type { get; set; }
    }
    public class FinancialYearLUVMNew
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
    public class ServiceEntityNew
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
    public class ChartOfAccountNew
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public long? ServiceCompanyId { get; set; }
        public bool? IsBank { get; set; }

    }
}
