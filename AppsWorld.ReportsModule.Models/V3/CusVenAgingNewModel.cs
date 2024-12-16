using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReportsModule.Models.V3
{
    public class CusVenAgingNewModel
    {
        public string Entity { get; set; }
        public string Limit { get; set; }
        public DateTime? Date { get; set; }
        public string DocNo { get; set; }
        public decimal? Current { get; set; }
        public decimal? _1to30 { get; set; }
        public decimal? _31to60 { get; set; }
        public decimal? _61to90 { get; set; }
        public decimal? _91to120 { get; set; }
        public decimal? _120 { get; set; }
        public decimal? BaseBalanceAmount { get; set; }
        public decimal? DocBalanceAmount { get; set; }
        public string DocCurrency { get; set; }
        public string DocType { get; set; }
        public string ServiceEntity { get; set; }
        public Guid? DocumentId { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string SubType { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? IsAddNote { get; set; }
    }
    public class GeneralLedgerViewModelNew
    {
        public long? CompanyId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string COA { get; set; }
        public string ServiceCompany { get; set; }
        public long ExcludeClearedItem { get; set; }
        public string Doc_Type { get; set; }

    }
    public class GLViewModelNew
    {
        public string COA_Name { get; set; }
        public string Type { get; set; }
        public string Sub_Type { get; set; }
        public string DocNo { get; set; }
        public string Entity { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Balance { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? DocCredit { get; set; }
        public decimal? DocBalance { get; set; }
        public decimal? Exch_Rate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? Bank_Clearing { get; set; }
        public string Item { get; set; }
        public long? Quantity { get; set; }
        public decimal? Unit_Price { get; set; }
        public string Tax_Code { get; set; }
        public string Mode { get; set; }
        public string Ref_No { get; set; }
        public string Cleared { get; set; }
        public DateTime? Date { get; set; }
        public Guid? DocumentId { get; set; }
        public long RowId { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string ServiceEntity { get; set; }

    }
   
}
