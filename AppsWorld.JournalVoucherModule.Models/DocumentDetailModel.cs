using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class DocumentDetailModel
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double? Qty { get; set; }
        public string Unit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? BalanceAmount { get; set; }
        public decimal? DoubtfulDebitTotalAmount { get; set; }
        public Nullable<Guid> Item { get; set; }
        public string TaxIdCode { get; set; }
        public long? COAId { get; set; }
        public decimal? DocAmount { get; set; }
        public decimal? DocTotalAmount { get; set; }
        public decimal? DocTaxAmount { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? BaseTaxAmount { get; set; }
        public decimal? BaseTotalAmount { get; set; }
        public string AmtCurrency { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public string TaxName { get; set; }
        public decimal? TaxDocDebit { get; set; }
        public decimal? TaxDocCredit { get; set; }
        public decimal? TaxBaseDebit { get; set; }
        public decimal? TaxBaseCredit { get; set; }
        public string AccountName { get; set; }
        public string AccountDescription { get; set; }
        public bool? Disallowable { get; set; }
        public long TaxId { get; set; }
        public string TaxCode { get; set; }
        public string TaxType { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? DocCredit { get; set; }
        public decimal? BaseDebit { get; set; }
        public decimal? BaseCredit { get; set; }
        public decimal? DocTaxDebit { get; set; }
        public decimal? DocTaxCredit { get; set; }
        public decimal? BaseTaxDebit { get; set; }
        public decimal? BaseTaxCredit { get; set; }
        public bool? ISDiSAllowShow { get; set; }
        public int? RecOrder { get; set; }
        public string DocNo { get; set; }
        public long? SegmentMasterid1 { get; set; }
        public long? SegmentMasterid2 { get; set; }
        public long? SegmentDetailid1 { get; set; }
        public long? SegmentDetailid2 { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }

        public string SegmentDetailidName1 { get; set; }
        public string SegmentDetailidName2 { get; set; }
        public string SegmentMasteridName1 { get; set; }
        public string SegmentMasteridName2 { get; set; }
        public DateTime? PostingDate { get; set; }
        public string OffsetDocument { get; set; }
        public string SystemRefNo { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string ServiceCompanyName { get; set; }
        public string Nature { get; set; }
        public string DocDescription { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string EntityName { get; set; }
        public string  AccountCode { get; set; }
        public string  DocCurrency { get; set; }
        public Guid? DetailId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string BaseCurrency { get; set; }
    }
}
