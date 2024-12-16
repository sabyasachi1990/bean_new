using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JVPopupViewModel
    {
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string ServiceEntityName { get; set; }
        public DateTime? DocDate { get; set; }
        public string DocNo { get; set; }
        public int? RecOrder { get; set; }
        public List<JVPopupViewDetailModel> JVPopupViewDetailModels { get; set; }
        public List<JVPopupMaster> JvPopupMasters { get; set; }
    }
    public class JVPopupViewDetailModel
    {
        public string DocDescription { get; set; }
        public string AccountName { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? DocCredit { get; set; }
        public decimal? BaseDebit { get; set; }
        public decimal? BaseCredit { get; set; }
        public string DocCurrency { get; set; }
        public string EntityName { get; set; }
        public int? RecOrder { get; set; }
    }
    public class JVPopupMaster
    {
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string ServiceEntityName { get; set; }
        public DateTime? DocDate { get; set; }
        public string DocNo { get; set; }
        public int? RecOrder { get; set; }
        public decimal? TotalDocDebit { get; set; }
        public decimal? TotalDocCredit { get; set; }
        public decimal? TotalBaseDebit { get; set; }
        public decimal? TotalBaseCredit { get; set; }
        public string SystemRefNo { get; set; }
        public List<JVPopupViewDetailModel> JVPopupViewDetailModels { get; set; }
    }

}
