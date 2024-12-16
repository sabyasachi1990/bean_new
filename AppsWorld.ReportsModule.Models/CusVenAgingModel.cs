using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReportsModule.Models
{
    //change CusVenAgingModel to CusVenAgingModelNew
    public class CusVenAgingModelNew
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
    //Add NEW CLASS
    public class CusVenAgingModel
    {
        public string Entity { get; set; }
        public string Limit { get; set; }
        public DateTime Date { get; set; }
        public string DocNo { get; set; }
        public string DocCurrency { get; set; }
        public string DocType { get; set; }
        public string ServiceEntity { get; set; }
        public double Current { get; set; }

        [JsonProperty("_1to30")]
        public double _1to30 { get; set; }

        [JsonProperty("_31to60")]
        public double _31to60 { get; set; }

        [JsonProperty("_61to90")]
        public double _61to90 { get; set; }

        [JsonProperty("_91to120")]
        public double _91to120 { get; set; }

        [JsonProperty("_120")]
        public double _120 { get; set; }
        public string DocumentId { get; set; }
        public double BaseBalanceAmount { get; set; }
        public double DocBalanceAmount { get; set; }
        public int ServiceCompanyId { get; set; }
        public string SubType { get; set; }
        public DateTime DueDate { get; set; }
        public int? IsAddNote { get; set; }

    }
    public class CusVenAgingUnCheckInvModel
    {
        public string Entity { get; set; }
        public string Currency { get; set; }
        public decimal? Current { get; set; }
        public decimal? _1to30 { get; set; }
        public decimal? _31to60 { get; set; }
        public decimal? _61to90 { get; set; }
        public decimal? _91to120 { get; set; }
        public decimal? _120 { get; set; }
        public decimal? BaseBalanceAmount { get; set; }
        public decimal? DocBalanceAmount { get; set; }

    }
    public class CustomerViewModel
    {
        public string ServiceEntites { get; set; }
        public string Nature { get; set; }
        public string Entites { get; set; }
        public DateTime? AsOfDate { get; set; }
        public string Currency { get; set; }
        public string CreditLimit { get; set; }
        public bool IsCustomer { get; set; }
        public long? CompanyId { get; set; }
        public string DocCurrency { get; set; }

    }

    public class GeneralLedgerViewModel
    {
        public long? CompanyId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string COA { get; set; }
        public string ServiceCompany { get; set; }
        public long ExcludeClearedItem { get; set; }
        public string Doc_Type { get; set; }

    }
    public class GLOutputVMK
    {
        public string Doc_Type { get; set; }
        public DateTime? Doc_Date { get; set; }
        public string Doc_RefNo { get; set; }
        public string Doc_No { get; set; }
        public string Entity { get; set; }
        public string Doc_Description { get; set; }
        public string Tax_Code { get; set; }
        public string Tax_RateIn_Var { get; set; }
        public string SR_GST { get; set; }
        public string ZR_GST { get; set; }
        public string ES33_GST { get; set; }
        public string ESN33_GST { get; set; }
        public string DS_GST { get; set; }
        public string OS_GST { get; set; }
        public string TOTAL_GST { get; set; }
        public string SR_NET { get; set; }
        public string ZR_NET { get; set; }
        public string ES33_NET { get; set; }
        public string ESN33_NET { get; set; }
        public string DS_NET { get; set; }
        public string OS_NET { get; set; }
        public string TOTAL_NET { get; set; }
        public string Gross_Amount { get; set; }
        public string COA { get; set; }
        public string Account_Type { get; set; }
        public string Service_Company { get; set; }
        public Guid? DocumentId { get; set; }
        public long ServiceCompamyId { get; set; }
        public string DocSubType { get; set; }

    }
    public class GLInputVMK
    {
        public string Doc_Type { get; set; }
        public DateTime? Doc_Date { get; set; }
        public string Doc_RefNo { get; set; }
        public string Doc_No { get; set; }
        public string Entity { get; set; }
        public string Doc_Description { get; set; }
        public string Tax_Code { get; set; }
        public string Tax_RateIn_Var { get; set; }
        public string TX_GST { get; set; }
        public string ZP_GST { get; set; }
        public string IM_GST { get; set; }
        public string ME_GST { get; set; }
        public string IGDS_GST { get; set; }
        public string TX_ESS_GST { get; set; }
        public string TX_N33_GST { get; set; }
        public string TX_RE_GST { get; set; }
        public string TOTAL_GST { get; set; }
        public string TX_NET { get; set; }
        public string ZP_NET { get; set; }
        public string IM_NET { get; set; }
        public string ME_NET { get; set; }
        public string IGDS_NET { get; set; }
        public string TX_ESS_NET { get; set; }
        public string TX_N33_NET { get; set; }
        public string TX_RE_NET { get; set; }
        public string TOTAL_NET { get; set; }
        public string Gross_Amount { get; set; }
        public string COA { get; set; }
        public string Account_Type { get; set; }
        public string Service_Company { get; set; }
        public Guid? DocumentId { get; set; }
        public long ServiceCompamyId { get; set; }
        public string DocSubType { get; set; }

    }
    public class GLViewModel
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
    public class GST
    {
        public string GST_Number { get; set; }
        public long? CompanyId { get; set; }
        public string Total_GST_output_tax { get; set; }//added for GST out put tax hyperlink
        public string Total_GST_input_tax { get; set; }//added for GST in put tax hyperlink
        public List<GSTFirstVM> GSTDetails { get; set; }
    }
    public class GSTFirstVM
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
    public class AuthInformation
    {
        public long? companyId { get; set; }
        public string userName { get; set; }
        public long? moduleDetailId { get; set; }
    }



    public class GSTVM
    {
        public string Doc_Type { get; set; }
        public DateTime? Doc_Date { get; set; }
        public string Doc_RefNo { get; set; }
        public string Doc_No { get; set; }
        public string Entity { get; set; }
        public string Doc_Description { get; set; }
        public string Tax_Code { get; set; }
        public string Tax_RateIn_Var { get; set; }

        public string TX_GST { get; set; }//input GST
        public string ZP_GST { get; set; }
        public string IM_GST { get; set; }
        public string ME_GST { get; set; }
        public string IGDS_GST { get; set; }
        public string BL_GST { get; set; }
        public string NR_GST { get; set; }
        public string EP_GST { get; set; }
        public string OP_GST { get; set; }
        public string TX_ESS_GST { get; set; }
        public string TX_N33_GST { get; set; }
        public string TX_RE_GST { get; set; }
        public string TX_DSPS_GST { get; set; }
        public string TXCA_GST { get; set; }
        public string TX_GMS_GST { get; set; }

        public string TXRC_TS_GST { get; set; }
        public string TXRC_ESS_GST { get; set; }
        public string TXRC_N33_GST { get; set; }
        public string TXRC_RE_GST { get; set; }
        public string IM_ESS_GST { get; set; }
        public string IM_N33_GST { get; set; }
        public string IM_RE_GST { get; set; }

        public string TXRC_TS_NET { get; set; }
        public string TXRC_ESS_NET { get; set; }
        public string TXRC_N33_NET { get; set; }
        public string TXRC_RE_NET { get; set; }
        public string IM_ESS_NET { get; set; }
        public string IM_N33_NET { get; set; }
        public string IM_RE_NET { get; set; }


        public string TX_NET { get; set; }
        public string ZP_NET { get; set; }
        public string IM_NET { get; set; }
        public string ME_NET { get; set; }
        public string IGDS_NET { get; set; }
        public string BL_NET { get; set; }
        public string NR_NET { get; set; }
        public string EP_NET { get; set; }
        public string OP_NET { get; set; }
        public string TX_ESS_NET { get; set; }
        public string TX_N33_NET { get; set; }
        public string TX_RE_NET { get; set; }
        public string TX_DSPS_NET { get; set; }
        public string TXCA_NET { get; set; }
        public string TX_GMS_NET { get; set; }


        public string SR_GST { get; set; }//out put tax
        public string ZR_GST { get; set; }
        public string ES33_GST { get; set; }
        public string ESN33_GST { get; set; }
        public string DS_GST { get; set; }
        public string OS_GST { get; set; }
        public string SRCA_C_GST { get; set; }
        public string SRCA_S_GST { get; set; }
        public string SR_DSPS_GST { get; set; }
        public string SR_GMS_GST { get; set; }
        public string SRRC_GST { get; set; }
        public string SROVR_GST { get; set; }
        public string SRRC_NET { get; set; }
        public string SROVR_NET { get; set; }
        public string SR_NET { get; set; }
        public string ZR_NET { get; set; }
        public string ES33_NET { get; set; }
        public string ESN33_NET { get; set; }
        public string DS_NET { get; set; }
        public string OS_NET { get; set; }
        public string SRCA_C_NET { get; set; }
        public string SRCA_S_NET { get; set; }
        public string SR_DSPS_NET { get; set; }
        public string SR_GMS_NET { get; set; }


        public string TOTAL_GST { get; set; }
        public string TOTAL_NET { get; set; }
        public string Gross_Amount { get; set; }
        public string COA { get; set; }
        public string Account_Type { get; set; }
        public string Service_Company { get; set; }
        public Guid? DocumentId { get; set; }
        public long ServiceCompamyId { get; set; }
        public string DocSubType { get; set; }
        public string BaseTax_Amount { get; set; }
        public string BaseNet_Amount { get; set; }
        public string BaseGross_Amount { get; set; }
    }
}
