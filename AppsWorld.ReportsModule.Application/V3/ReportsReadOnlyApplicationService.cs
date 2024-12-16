using AppsWorld.ReportsModule.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReportsModule.Models.V3;
using System.Security.Policy;
using AppsWorld.ReportsModule.Infra;
using Ziraff.FrameWork.Logging;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.IdentityModel.Metadata;

namespace AppsWorld.ReportsModule.Application.V3
{
    public class ReportsReadOnlyApplicationService
    {
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        DataTable dt = null;
        #region 
        public object GetGeneralLedgerLu(long companyId, string Username, string connection)
        {
            var res = connection.Split(';');
            var serverName = res[0].Split('=')[1];
            var AppicationIntest = res[2].Split('=')[1];
            string message = $"[{companyId}] {Username} {serverName} {AppicationIntest}";

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add(Username, companyId.ToString());
            var serializeparms = JsonConvert.SerializeObject(parms);
            CommonObjModel commonObjModel = new CommonObjModel
            {
                CompanyId = companyId,
                Params = serializeparms,
                ServerName = serverName,
                ApplicationIntest = AppicationIntest,
                MethodName = "GetGeneralLedgerLu",
            };
            string objname1 = JsonConvert.SerializeObject(commonObjModel);
            GeneralLegderNewLUVM glLu = new GeneralLegderNewLUVM();
            LoggingHelper.LogMessage(BeanLogConstant.log_GetGeneralLedgerLu_Entering_ApplicationService, objname1);

            List<ServiceEntityNew> lstServiceEntity = new List<ServiceEntityNew>();
            List<ChartOfAccountNew> lstCOA = new List<ChartOfAccountNew>();

            try
            {
                using (con = new SqlConnection(connection))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("[dbo].[GLparameter]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@UserName", Username);
                    using (dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ServiceEntityNew serviceEntity = new ServiceEntityNew();
                            serviceEntity.Name = dr["Name"] != null ? Convert.ToString(dr["Name"]) : null;
                            serviceEntity.Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : (long?)null;
                            lstServiceEntity.Add(serviceEntity);
                        }
                        glLu.ServiceCompany = lstServiceEntity.OrderBy(x => x.ShortName).ToList();
                        dr.NextResult();
                        dt = new DataTable();
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        while (dr.Read())
                        {
                            ChartOfAccountNew coa = new ChartOfAccountNew();
                            //glLu.ServiceCompany.Add(Convert.ToString(dr[0]));
                            coa.Name = dr["COAName"] != null ? Convert.ToString(dr["COAName"]) : null;
                            coa.Id = dr["COAId"] != DBNull.Value ? Convert.ToInt64(dr["COAId"]) : (long?)null;
                            coa.ServiceCompanyId = dr["ServiceCompany"] != DBNull.Value ? Convert.ToInt64(dr["ServiceCompany"]) : (long?)null;
                            coa.IsBank = dr["IsBank"] != DBNull.Value ? Convert.ToBoolean(dr["IsBank"]) : (bool?)null;
                            lstCOA.Add(coa);
                        }
                        dr.NextResult();
                        glLu.ChartofAccounts = lstCOA;
                        DataTable dt1 = new DataTable();
                        dt1.Load(dr);
                        glLu.Doc_Type = dt1.AsEnumerable().Select(a => a.Field<string>(0)).Distinct().ToList();
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
                LoggingHelper.LogMessage(BeanLogConstant.log_GetGeneralLedgerLu_Completed_ApplicationService, objname1);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(BeanLogConstant.log_GetGeneralLedgerLu_Failed_ApplicationService, ex, message);
                throw ex;
            }

            return glLu;
        }


        public FinancialYearLUVMNew GetFinancialLu(long companyId, DateTime? toDate, string connection)
        {
            FinancialYearLUVMNew glLu = new FinancialYearLUVMNew();
            try
            {
                var res = connection.Split(';');
                var serverName = res[0].Split('=')[1];
                var AppicationIntest = res[2].Split('=')[1];

                Dictionary<DateTime?, string> parms = new Dictionary<DateTime?, string>();
                parms.Add(toDate, companyId.ToString());
                var serializeparms = JsonConvert.SerializeObject(parms);
                CommonObjModel commonObjModel = new CommonObjModel
                {
                    CompanyId = companyId,
                    Params = serializeparms,
                    ServerName = serverName,
                    ApplicationIntest = AppicationIntest,
                    MethodName = "GetFinancialLu",
                };
                string secondarydboj = JsonConvert.SerializeObject(commonObjModel);

                LoggingHelper.LogMessage(BeanLogConstant.ReportsReadOnlyApplicationService, secondarydboj);
                string query = "Declare @CompanyId INT=689,@ToDate DateTime=Convert(datetime,'26-08-2024 00:00:00',103) Select Case when dateadd(YEAR, 0, dateadd(day, 1, cast(REPLACE(BusinessYearEnd, '-', ' ')+cast(datepart(year, @ToDate/*getdate()*/) as char(4)) as date))) < @ToDate then dateadd(YEAR, 0, dateadd(day, 1, cast(REPLACE(BusinessYearEnd, '-', ' ') + cast(datepart(year, @ToDate/*getdate()*/) as char(4)) as date)))else dateadd(YEAR, -1, dateadd(day, 1, cast(REPLACE(BusinessYearEnd, '-', ' ') + cast(datepart(year, @ToDate/*getdate()*/) as char(4)) as date))) end as FromDate from[Common].[Localization] Where CompanyId = @CompanyId";
                using (con = new SqlConnection(connection))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        glLu.fromDate = dr[0] != DBNull.Value ? Convert.ToDateTime(dr[0]) : (DateTime?)null;
                        glLu.toDate = DateTime.UtcNow;
                    }
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return glLu;
        }

        public List<GLViewModelNew> GetGeneralLedger(GeneralLedgerViewModelNew generalLedgerViewModel, string connection)
        {
            try
            {

                var res = connection.Split(';');
                var serverName = res[0].Split('=')[1];
                var AppicationIntest = res[2].Split('=')[1];

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("object", JsonConvert.SerializeObject(generalLedgerViewModel));
                var serializeparms = JsonConvert.SerializeObject(parms);
                CommonObjModel commonObjModel = new CommonObjModel
                {
                    CompanyId = generalLedgerViewModel.CompanyId ?? 0,
                    Params = serializeparms,
                    ServerName = serverName,
                    ApplicationIntest = AppicationIntest,
                    MethodName = "GetGeneralLedger",
                };
                string secondarydboj = JsonConvert.SerializeObject(commonObjModel);

                LoggingHelper.LogMessage(BeanLogConstant.ReportsReadOnlyApplicationService, secondarydboj);



                List<GLViewModelNew> lstglviewModels = new List<GLViewModelNew>();

                if (generalLedgerViewModel != null && generalLedgerViewModel.ServiceCompany != null)
                {
                    using (con = new SqlConnection(connection))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        using (cmd = new SqlCommand("[dbo].[HTMLRpt_General_Ledger_Final]", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.AddWithValue("@CompanyId", generalLedgerViewModel.CompanyId);
                            cmd.Parameters.AddWithValue("@FromDate", generalLedgerViewModel.FromDate);
                            cmd.Parameters.AddWithValue("@ToDate", generalLedgerViewModel.ToDate);
                            cmd.Parameters.AddWithValue("@COA", generalLedgerViewModel.COA);
                            cmd.Parameters.AddWithValue("@ServiceCompany", generalLedgerViewModel.ServiceCompany);
                            cmd.Parameters.AddWithValue("@ExcludeClearedItem", generalLedgerViewModel.ExcludeClearedItem);
                            cmd.Parameters.AddWithValue("@DocType", generalLedgerViewModel.Doc_Type);
                            using (dr = cmd.ExecuteReader())
                            {
                                dt = new DataTable();
                                dt.Load(dr);

                                lstglviewModels = dt.AsEnumerable().Select(z => new GLViewModelNew()
                                {
                                    COA_Name = z.Field<string>(0),
                                    Type = z.Field<string>(2),
                                    Sub_Type = z.Field<string>(3),
                                    DocNo = z.Field<string>(4),
                                    Entity = z.Field<string>(5),
                                    Description = z.Field<string>(6),
                                    Currency = z.Field<string>(7),
                                    Debit = z[8] != DBNull.Value ? Convert.ToDecimal(z[8]) : (decimal?)null,
                                    Credit = z[9] != DBNull.Value ? Convert.ToDecimal(z[9]) : (decimal?)null,
                                    Balance = z[10] != DBNull.Value ? Convert.ToDecimal(z[10]) : (decimal?)null,
                                    DocDebit = z[11] != DBNull.Value ? Convert.ToDecimal(z[11]) : (decimal?)null,
                                    DocCredit = z[12] != DBNull.Value ? Convert.ToDecimal(z[12]) : (decimal?)null,
                                    DocBalance = z[13] != DBNull.Value ? Convert.ToDecimal(z[13]) : (decimal?)null,
                                    Exch_Rate = z[14] != DBNull.Value ? Convert.ToDecimal(z[14]) : (decimal?)null,
                                    DueDate = z[15] != DBNull.Value ? Convert.ToDateTime(z[15]) : (DateTime?)null,
                                    Bank_Clearing = z[16] != DBNull.Value ? Convert.ToDateTime(z[16]) : (DateTime?)null,
                                    Item = z.Field<string>(17),
                                    Quantity = z[18] != DBNull.Value ? Convert.ToInt32(z[18]) : (int?)null,
                                    Unit_Price = z[19] != DBNull.Value ? Convert.ToDecimal(z[19]) : (decimal?)null,
                                    Tax_Code = z.Field<string>(20),
                                    Mode = z.Field<string>(21),
                                    Ref_No = z.Field<string>(22),
                                    Cleared = z.Field<string>(23),
                                    Date = z[24] != DBNull.Value ? Convert.ToDateTime(z[24]) : (DateTime?)null,
                                    DocumentId = z[25] != DBNull.Value ? (Guid?)(z[25]) : (Guid?)null,
                                    RowId = Convert.ToInt64(z[26]),
                                    ServiceCompanyId = z[27] != DBNull.Value ? Convert.ToInt32(z[27]) : (int?)null,
                                    ServiceEntity = z.Field<string>(30)

                                }).ToList();

                            }
                        }
                    }

                    return lstglviewModels;
                }
                else
                {
                    return new List<GLViewModelNew>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally

            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }
        #endregion


        public CustVenAgingLUVM CustVenAgingLU(long? tenantId, bool? isCustomer, string userName, string connection)
        {

            CustVenAgingLUVM custLu = new CustVenAgingLUVM();
            List<CommonClass> commonClass = new List<CommonClass>();
            try
            {
                var res = connection.Split(';');
                var serverName = res[0].Split('=')[1];
                var AppicationIntest = res[2].Split('=')[1];

                Dictionary<bool?, string> parms = new Dictionary<bool?, string>();
                parms.Add(isCustomer, tenantId.ToString());
                var serializeparms = JsonConvert.SerializeObject(parms);
                CommonObjModel commonObjModel = new CommonObjModel
                {
                    CompanyId = tenantId ?? 0,
                    Params = serializeparms,
                    ServerName = serverName,
                    ApplicationIntest = AppicationIntest,
                    MethodName = "CustVenAgingLU",
                };
                string secondarydboj = JsonConvert.SerializeObject(commonObjModel);

                LoggingHelper.LogMessage(BeanLogConstant.ReportsReadOnlyApplicationService, secondarydboj);
                string query = null;
                if (isCustomer == true)
                {

                    query = $"Select Distinct  comp.Id as Id,Name as Name,ShortName from Common.Company comp with(nolock) join Common.CompanyUser CU with(nolock) on comp.ParentId=CU.CompanyId Join Common.CompanyUserDetail CUD with(nolock) on CUD.CompanyUserId=CU.id where ParentId={tenantId} and CUD.ServiceEntityId=comp.Id and cu.Username='{userName}' group by comp.id,Name,ShortName order by ShortName;Select  Distinct EntityId,EntityName From (Select Distinct  BE.Id as EntityId, BE.Name as EntityName From Bean.Journal J with(nolock) Inner Join Bean.JournalDetail JD with(nolock) on J.Id = JD.JournalId Inner Join Bean.Entity BE  with(nolock) on JD.EntityId = BE.Id JOIN Common.Company CE  with(nolock) on CE.ParentId = {tenantId} Where BE.CompanyId = {tenantId}  AND JD.DocType IN('Invoice', 'Credit Note', 'Debit Note', 'Debt Provision') AND (JD.ClearingStatus<>('Cleared') OR JD.ClearingStatus IS NULL)  Union All Select Distinct   BE.Id EntityId, BE.Name as EntityName From Bean.Invoice I  with(nolock) Inner Join Bean.Entity BE  with(nolock) on I.EntityId = BE.Id JOIN Common.Company CE with(nolock) on CE.ParentId = {tenantId} Where BE.CompanyId = {tenantId} AND I.DocType IN('Invoice', 'Credit Note') AND I.DocSubType IN('Opening Bal')) AS A Order By EntityName;Select Distinct jd.DocCurrency as Name from Bean.Journal as j with(nolock) join Bean.JournalDetail as jd with(nolock) on j.Id = jd.JournalId where j.CompanyId = {tenantId}  group by jd.DocCurrency order by Name;Select IC.IsInterCompanyEnabled from Bean.InterCompanySetting IC where CompanyId={tenantId} and ic.InterCompanyType='Billing'";
                }
                else
                {

                    query = $"Select Distinct  comp.Id as Id,Name as Name,ShortName  from Common.Company comp with(nolock) join Common.CompanyUser CU with(nolock) on comp.ParentId=CU.CompanyId Join Common.CompanyUserDetail CUD with(nolock) on CUD.CompanyUserId=CU.id where ParentId={tenantId} and CUD.ServiceEntityId=comp.Id and cu.Username= '{userName}' group by comp.id,Name,ShortName order by ShortName;\r\n\r\nSelect  Distinct EntityId,EntityName  From ( Select Distinct BE.Id EntityId, BE.Name EntityName From Bean.Journal J with(nolock) Inner Join Bean.JournalDetail JD with(nolock) on J.Id = JD.JournalId Inner Join Bean.Entity BE with(nolock) on JD.EntityId = BE.Id JOIN Common.Company CE with(nolock) on CE.ParentId = {tenantId} Where BE.CompanyId = {tenantId} AND JD.DocType IN('Bill','Credit Memo','Bill Payment','Cash Payment','Payroll Bill','Payroll Payment') AND(JD.ClearingStatus<>('Cleared') OR JD.ClearingStatus IS NULL) \r\n\r\nUnion All \r\n\r\nSelect Distinct  BE.Id EntityId, BE.Name EntityName From Bean.Bill J with(nolock) Inner Join Bean.BillDetail JD with(nolock) on J.Id = JD.BillId Inner Join Bean.Entity BE with(nolock) on J.EntityId = BE.Id JOIN Common.Company CE with(nolock) on CE.ParentId = {tenantId} Where BE.CompanyId = {tenantId} AND J.DocType IN('Bill') AND J.DocSubType IN('Opening Bal')\r\n\r\nUnion All \r\n\r\nSelect Distinct BE.Id EntityId, BE.Name EntityName From Bean.CreditMemo J with(nolock) Inner Join Bean.CreditMemoDetail JD with(nolock) on J.Id = JD.CreditMemoId Inner Join Bean.Entity BE with(nolock) on J.EntityId = BE.Id JOIN Common.Company CE with(nolock) on CE.ParentId = {tenantId} Where BE.CompanyId ={tenantId} AND J.DocType IN('Credit Memo') AND J.DocSubType IN('Opening Bal')) AS A Order By EntityName; \r\n\r\nSelect Distinct jd.DocCurrency as Name  from Bean.Journal as j with(nolock) join Bean.JournalDetail as jd with(nolock) on j.Id = jd.JournalId where j.CompanyId = {tenantId}  group by jd.DocCurrency order by Name;\r\n\r\nSelect IC.IsInterCompanyEnabled from Bean.InterCompanySetting IC where CompanyId={tenantId} and ic.InterCompanyType='Billing'";
                }
                using (con = new SqlConnection(connection))
                using (cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    var sqlDataAdapter = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    con.Open();
                    sqlDataAdapter.Fill(ds);
                    con.Close();
                    if (ds.Tables.Count >= 4)
                    {
                        string jsonDt1 = JsonConvert.SerializeObject(ds.Tables[0]);
                        string jsonDt2 = JsonConvert.SerializeObject(ds.Tables[1]);
                        string jsonDt3 = JsonConvert.SerializeObject(ds.Tables[2]);
                        string jsonDt4 = JsonConvert.SerializeObject(ds.Tables[3]);
                        var serviceCompanies = JsonConvert.DeserializeObject<List<ServiceEntity>>(jsonDt1);
                        var entities = JsonConvert.DeserializeObject<List<Entity>>(jsonDt2);
                        var currencies = JsonConvert.DeserializeObject<List<currency>>(jsonDt3);
                        var isIBActivated = JsonConvert.DeserializeObject<List<CompanySettings>>(jsonDt4);
                        custLu.ServiceEntities = serviceCompanies;
                        custLu.Entities = entities;
                        custLu.DocCurrencies = currencies.Select(x => x.Name).ToList();
                        custLu.CreditLimits = new List<string> { "Exceeded", "Not Exceeded" };
                        custLu.Natures = new List<string> { "Trade", "Others" };
                        if (isIBActivated.Exists(x => x.IsInterCompanyEnabled))
                        {
                            custLu.Natures.Add("Interco");
                        }
                        custLu.Currency = new List<string> { "Doc Currency", "Base Currency" };
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return custLu;
        }

        public List<CusVenAgingModel> GetCutomerAndVendorAging(CustomerViewModel customerViewModel, string connection)
        {

            List<CusVenAgingModel> lstCusVenAgingMode = new List<CusVenAgingModel>();
            try
            {

                var res = connection.Split(';');
                var serverName = res[0].Split('=')[1];
                var AppicationIntest = res[2].Split('=')[1];

                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("Object", customerViewModel);
                var serializeparms = JsonConvert.SerializeObject(parms);
                CommonObjModel commonObjModel = new CommonObjModel
                {
                    CompanyId = customerViewModel.CompanyId ?? 0,
                    Params = serializeparms,
                    ServerName = serverName,
                    ApplicationIntest = AppicationIntest,
                    MethodName = "GetCutomerAndVendorAging",
                };
                string secondarydboj = JsonConvert.SerializeObject(commonObjModel);

                LoggingHelper.LogMessage(BeanLogConstant.ReportsReadOnlyApplicationService, secondarydboj);



                string spName = customerViewModel.IsCustomer ? "BC_Customer_GRID_Aging_POC" : "BC_Vendor_GRID_Aging_Final";
                using (con = new SqlConnection(connection))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    using (cmd = new SqlCommand(spName, con))
                    {
                        #region Oldcode
                        //cmd.CommandTimeout = 0;
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@Tenantid", customerViewModel.CompanyId);
                        //cmd.Parameters.AddWithValue("@AsOf", customerViewModel.AsOfDate);
                        //cmd.Parameters.AddWithValue("@Customer", customerViewModel.Entites);
                        //cmd.Parameters.AddWithValue("@Nature", customerViewModel.Nature);
                        //cmd.Parameters.AddWithValue("@ServiceEntity", customerViewModel.ServiceEntites);
                        //cmd.Parameters.AddWithValue("@Currency", customerViewModel.Currency);
                        //cmd.Parameters.AddWithValue("@DocCurrency", customerViewModel.DocCurrency != null ? customerViewModel.DocCurrency : string.Empty);
                        //using (dr = cmd.ExecuteReader())
                        //{
                        //    while (dr.Read())
                        //    {

                        //        CusVenAgingModel cusVenAgingMode = new CusVenAgingModel();
                        //        if (customerViewModel.IsCustomer)
                        //        {
                        //            cusVenAgingMode.Entity = Convert.ToString(dr[0]);
                        //            cusVenAgingMode.Limit = customerViewModel.IsCustomer == true ? Convert.ToString(dr[1]) : string.Empty;
                        //            cusVenAgingMode.Date = dr[2] != DBNull.Value ? Convert.ToDateTime(dr[2]) : (DateTime?)null;
                        //            cusVenAgingMode.DocNo = Convert.ToString(dr[3]);
                        //            cusVenAgingMode.DocCurrency = dr[4] != DBNull.Value ? Convert.ToString(dr[4]) : string.Empty;
                        //            cusVenAgingMode.Current = dr[7] != DBNull.Value ? Convert.ToDecimal(dr[7]) : (decimal?)null;
                        //            cusVenAgingMode._1to30 = dr[8] != DBNull.Value ? Convert.ToDecimal(dr[8]) : (decimal?)null;
                        //            cusVenAgingMode._31to60 = dr[9] != DBNull.Value ? Convert.ToDecimal(dr[9]) : (decimal?)null;
                        //            cusVenAgingMode._61to90 = dr[10] != DBNull.Value ? Convert.ToDecimal(dr[10]) : (decimal?)null;
                        //            cusVenAgingMode._91to120 = dr[11] != DBNull.Value ? Convert.ToDecimal(dr[11]) : (decimal?)null;
                        //            cusVenAgingMode._120 = dr[12] != DBNull.Value ? Convert.ToDecimal(dr[12]) : (decimal?)null;
                        //            cusVenAgingMode.DocType = Convert.ToString(dr[5]);
                        //            cusVenAgingMode.ServiceEntity = Convert.ToString(dr[6]);
                        //            cusVenAgingMode.DocumentId = dr[13] != DBNull.Value ? ((Guid?)dr.GetValue(13)) : (Guid?)null;
                        //            cusVenAgingMode.BaseBalanceAmount = dr[14] != DBNull.Value ? Convert.ToDecimal(dr[14]) : (decimal?)null;
                        //            cusVenAgingMode.DocBalanceAmount = dr[15] != DBNull.Value ? Convert.ToDecimal(dr[15]) : (decimal?)null;
                        //            cusVenAgingMode.ServiceCompanyId = dr[16] != DBNull.Value ? Convert.ToInt64(dr[16]) : (long?)null;
                        //            cusVenAgingMode.SubType = Convert.ToString(dr[17]);
                        //            cusVenAgingMode.DueDate = dr[18] != DBNull.Value ? Convert.ToDateTime(dr[18]) : (DateTime?)null;
                        //            lstCusVenAgingMode.Add(cusVenAgingMode);
                        //        }
                        //        else
                        //        {
                        //            cusVenAgingMode = new CusVenAgingModel();
                        //            cusVenAgingMode.Entity = Convert.ToString(dr[0]);
                        //            cusVenAgingMode.Date = dr[1] != DBNull.Value ? Convert.ToDateTime(dr[1]) : (DateTime?)null;
                        //            cusVenAgingMode.DocNo = Convert.ToString(dr[2]);
                        //            cusVenAgingMode.DocCurrency = dr[3] != DBNull.Value ? Convert.ToString(dr[3]) : string.Empty;
                        //            cusVenAgingMode.Current = dr[6] != DBNull.Value ? Convert.ToDecimal(dr[6]) : (decimal?)null;
                        //            cusVenAgingMode._1to30 = dr[7] != DBNull.Value ? Convert.ToDecimal(dr[7]) : (decimal?)null;
                        //            cusVenAgingMode._31to60 = dr[8] != DBNull.Value ? Convert.ToDecimal(dr[8]) : (decimal?)null;
                        //            cusVenAgingMode._61to90 = dr[9] != DBNull.Value ? Convert.ToDecimal(dr[9]) : (decimal?)null;
                        //            cusVenAgingMode._91to120 = dr[10] != DBNull.Value ? Convert.ToDecimal(dr[10]) : (decimal?)null;
                        //            cusVenAgingMode._120 = dr[11] != DBNull.Value ? Convert.ToDecimal(dr[11]) : (decimal?)null;
                        //            cusVenAgingMode.DocBalanceAmount = dr[13] != DBNull.Value ? Convert.ToDecimal(dr[13]) : (decimal?)null;
                        //            cusVenAgingMode.BaseBalanceAmount = dr[14] != DBNull.Value ? Convert.ToDecimal(dr[14]) : (decimal?)null;
                        //            cusVenAgingMode.DocType = Convert.ToString(dr[4]);
                        //            cusVenAgingMode.ServiceEntity = Convert.ToString(dr[5]);
                        //            cusVenAgingMode.DocumentId = dr[12] != DBNull.Value ? ((Guid?)dr.GetValue(12)) : (Guid?)null;
                        //            cusVenAgingMode.ServiceCompanyId = dr[15] != DBNull.Value ? Convert.ToInt64(dr[15]) : (long?)null;
                        //            cusVenAgingMode.SubType = Convert.ToString(dr[16]);
                        //            cusVenAgingMode.DueDate = dr[17] != DBNull.Value ? Convert.ToDateTime(dr[17]) : (DateTime?)null;

                        //            lstCusVenAgingMode.Add(cusVenAgingMode);
                        //        }

                        //    }
                        //    if (con.State == ConnectionState.Open)
                        //        con.Close();

                        //}
                        #endregion

                        #region New Code
                        DataTable dataTable = new DataTable();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.AddWithValue("@Tenantid", customerViewModel.CompanyId);
                        cmd.Parameters.AddWithValue("@AsOf", customerViewModel.AsOfDate);
                        cmd.Parameters.AddWithValue("@Customer", customerViewModel.Entites);
                        cmd.Parameters.AddWithValue("@Nature", customerViewModel.Nature);
                        cmd.Parameters.AddWithValue("@ServiceEntity", customerViewModel.ServiceEntites);
                        cmd.Parameters.AddWithValue("@Currency", customerViewModel.Currency);
                        cmd.Parameters.AddWithValue("@DocCurrency", customerViewModel.DocCurrency != null ? customerViewModel.DocCurrency : string.Empty);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        try
                        {
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            dataAdapter.Fill(dataTable);
                            if (customerViewModel.IsCustomer)
                            {
                                string json = JsonConvert.SerializeObject(dataTable);
                                lstCusVenAgingMode = JsonConvert.DeserializeObject<List<CusVenAgingModel>>(json);
                            }
                            else
                            {
                                string json = JsonConvert.SerializeObject(dataTable);
                                lstCusVenAgingMode = JsonConvert.DeserializeObject<List<CusVenAgingModel>>(json);
                            }
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        #endregion
                    }
                }
                return lstCusVenAgingMode;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<CusVenAgingUnCheckInvModel> GetCutomerAndVendorAgingUnCheckInv(CustomerViewModel customerViewModel, string connection)
        {
            List<CusVenAgingModel> lstCusVenAgingMode = new List<CusVenAgingModel>();
            try
            {
                using (con = new SqlConnection(connection))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    if (customerViewModel.IsCustomer)
                        cmd = new SqlCommand("[BC_Customer_GRID_Aging_InvoiceUncheck]", con);
                    else
                        cmd = new SqlCommand("[BC_Vendor_GRID_Aging_InvoiceUncheck]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tenantid", customerViewModel.CompanyId);
                    cmd.Parameters.AddWithValue("@AsOf", customerViewModel.AsOfDate);
                    cmd.Parameters.AddWithValue("@Customer", customerViewModel.Entites);
                    cmd.Parameters.AddWithValue("@Nature", customerViewModel.Nature);
                    cmd.Parameters.AddWithValue("@ServiceEntity", customerViewModel.ServiceEntites);
                    cmd.Parameters.AddWithValue("@Currency", customerViewModel.Currency);
                    cmd.Parameters.AddWithValue("@DocCurrency", customerViewModel.DocCurrency != null ? customerViewModel.DocCurrency : string.Empty);
                    dr = cmd.ExecuteReader();
                    dt = new DataTable();
                    return dt.AsEnumerable().Select(z => new CusVenAgingUnCheckInvModel()
                    {
                        Entity = Convert.ToString(z[0]),
                        Currency = z[1] != DBNull.Value ? Convert.ToString(z[1]) : string.Empty,
                        Current = z[2] != DBNull.Value ? Convert.ToDecimal(z[2]) : (decimal?)null,
                        _1to30 = z[3] != DBNull.Value ? Convert.ToDecimal(z[3]) : (decimal?)null,
                        _31to60 = z[4] != DBNull.Value ? Convert.ToDecimal(z[4]) : (decimal?)null,
                        _61to90 = z[5] != DBNull.Value ? Convert.ToDecimal(z[5]) : (decimal?)null,
                        _91to120 = z[6] != DBNull.Value ? Convert.ToDecimal(z[6]) : (decimal?)null,
                        _120 = z[7] != DBNull.Value ? Convert.ToDecimal(z[7]) : (decimal?)null,
                        DocBalanceAmount = z[8] != DBNull.Value ? Convert.ToDecimal(z[8]) : (decimal?)null,
                        BaseBalanceAmount = z[9] != DBNull.Value ? Convert.ToDecimal(z[9]) : (decimal?)null,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }
}
