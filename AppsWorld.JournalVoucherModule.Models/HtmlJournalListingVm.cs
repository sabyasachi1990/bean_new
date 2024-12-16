
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class HtmlJournalListingVm
    {
        public string Type { get; set; }
        public string SubType { get; set; }
        public string SvcEntity { get; set; }
        public DateTime? Date { get; set; }
        public string DocNo { get; set; }
        public string Entity { get; set; }
        public string Description { get; set; }
        public string Account { get; set; }
        public string Curr { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? DocCredit { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public Guid? JournalId { get; set; }
        public Guid? DocumentId { get; set; }
        public long? ServiceCompanyId { get; set; }
    }
    public class HtmlJlParamsVm
    {
        public string QueryOptions { get; set; }
        public long CompanyId { get; set; }
        public string serviceEntity { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string DocType { get; set; }
        public string SubType { get; set; }
        public string DocNumber { get; set; }
    }
    public class JournalListingParamVm
    {
        public JournalListingParamVm()
        {
            //ServiceCompany = new List<string>();
            ServiceCompany = new List<ServiceEntity>();
            DocType = new List<string>();
            DocSubType = new List<string>();
        }

        //public List<string> ServiceCompany { get; set; }
        public List<ServiceEntity> ServiceCompany { get; set; }
        public List<string> DocType { get; set; }
        public List<string> DocSubType { get; set; }

    }
    public class JournalListingDocSubTypeParamVm
    {
        public JournalListingDocSubTypeParamVm()
        {
            SubType = new List<string>();

        }
        //public string Doctype { get; set; }
        public List<string> SubType { get; set; }
    }
    public class ServiceEntity
    {
        public long? Id { get; set; }
        public string Name { get; set; }
    }
}
