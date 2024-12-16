using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ziraff.FrameWork;
using AppsWorld.ReportsModule.Models;
using System.Configuration;
using Ziraff.FrameWork.Logging;
using AppsWorld.ReportsModule.Infra;
using System.Reflection;
using AppsWorld.ReportsModule.Models.V3;
using Newtonsoft.Json;

namespace AppsWorld.ReportsModule.Application
{
    public class ReportsApplicationService
    {
        //public static string ConnectionString2 = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["AppsWorldDBContextClientId"], ConfigurationManager.AppSettings["AppsWorldDBContextClientSecret"], ConfigurationManager.AppSettings["AppsWorldDBContextKeySecretUri"]);

        SqlConnection con = null; /*new SqlConnection(ConnectionString2);*/
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        DataTable dt = null;
        //GetCutomerAndVendorAging move t v3 service
        //public async Task<List<CusVenAgingModel>> GetCutomerAndVendorAging(CustomerViewModel customerViewModel, string ConnectionString)
        //{

        //    List<CusVenAgingModel> lstCusVenAgingMode = new List<CusVenAgingModel>();
        //    try
        //    {
        //        DBLoges("GetCustomerAndVendorAging", "Execution started", "Step 1");
        //        string spName = customerViewModel.IsCustomer ? "BC_Customer_GRID_Aging_POC" : "BC_Vendor_GRID_Aging_Final";
        //        DBLoges(spName, "Execution started", "Step 2");
        //        using (con = new SqlConnection(ConnectionString))
        //        {
        //            if (con.State != ConnectionState.Open)
        //                await con.OpenAsync();
        //            using (cmd = new SqlCommand(spName, con))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@Tenantid", customerViewModel.CompanyId);
        //                cmd.Parameters.AddWithValue("@AsOf", customerViewModel.AsOfDate);
        //                cmd.Parameters.AddWithValue("@Customer", customerViewModel.Entites);
        //                cmd.Parameters.AddWithValue("@Nature", customerViewModel.Nature);
        //                cmd.Parameters.AddWithValue("@ServiceEntity", customerViewModel.ServiceEntites);
        //                cmd.Parameters.AddWithValue("@Currency", customerViewModel.Currency);
        //                cmd.Parameters.AddWithValue("@DocCurrency", customerViewModel.DocCurrency != null ? customerViewModel.DocCurrency : string.Empty);
        //                using (dr = await cmd.ExecuteReaderAsync())
        //                {
        //                    DBLoges(spName, "Execution complete", "Step 3");
        //                    DBLoges(spName, "Binding the values started", "Step 4");
        //                    while (await dr.ReadAsync())
        //                    {

        //                        CusVenAgingModel cusVenAgingMode = new CusVenAgingModel();
        //                        if (customerViewModel.IsCustomer)
        //                        {
        //                            cusVenAgingMode.Entity = Convert.ToString(dr[0]);
        //                            cusVenAgingMode.Limit = customerViewModel.IsCustomer == true ? Convert.ToString(dr[1]) : string.Empty;
        //                            cusVenAgingMode.Date = dr[2] != DBNull.Value ? Convert.ToDateTime(dr[2]) : (DateTime?)null;
        //                            cusVenAgingMode.DocNo = Convert.ToString(dr[3]);
        //                            cusVenAgingMode.DocCurrency = dr[4] != DBNull.Value ? Convert.ToString(dr[4]) : string.Empty;
        //                            cusVenAgingMode.Current = dr[7] != DBNull.Value ? Convert.ToDecimal(dr[7]) : (decimal?)null;
        //                            cusVenAgingMode._1to30 = dr[8] != DBNull.Value ? Convert.ToDecimal(dr[8]) : (decimal?)null;
        //                            cusVenAgingMode._31to60 = dr[9] != DBNull.Value ? Convert.ToDecimal(dr[9]) : (decimal?)null;
        //                            cusVenAgingMode._61to90 = dr[10] != DBNull.Value ? Convert.ToDecimal(dr[10]) : (decimal?)null;
        //                            cusVenAgingMode._91to120 = dr[11] != DBNull.Value ? Convert.ToDecimal(dr[11]) : (decimal?)null;
        //                            cusVenAgingMode._120 = dr[12] != DBNull.Value ? Convert.ToDecimal(dr[12]) : (decimal?)null;
        //                            cusVenAgingMode.DocType = Convert.ToString(dr[5]);
        //                            cusVenAgingMode.ServiceEntity = Convert.ToString(dr[6]);
        //                            cusVenAgingMode.DocumentId = dr[13] != DBNull.Value ? ((Guid?)dr.GetValue(13)) : (Guid?)null;
        //                            cusVenAgingMode.BaseBalanceAmount = dr[14] != DBNull.Value ? Convert.ToDecimal(dr[14]) : (decimal?)null;
        //                            cusVenAgingMode.DocBalanceAmount = dr[15] != DBNull.Value ? Convert.ToDecimal(dr[15]) : (decimal?)null;
        //                            cusVenAgingMode.ServiceCompanyId = dr[16] != DBNull.Value ? Convert.ToInt64(dr[16]) : (long?)null;
        //                            //cusVenAgingMode.IsAddNote = dr["IsAddNote"] != DBNull.Value ? Convert.ToBoolean(dr["IsAddNote"]) : false;
        //                            cusVenAgingMode.SubType = Convert.ToString(dr[17]);
        //                            cusVenAgingMode.DueDate = dr[18] != DBNull.Value ? Convert.ToDateTime(dr[18]) : (DateTime?)null;
        //                            //cusVenAgingMode.IsNote = dr[19] != DBNull.Value ? Convert.ToBoolean(dr[19]) : (bool?)false;
        //                            lstCusVenAgingMode.Add(cusVenAgingMode);
        //                        }
        //                        else
        //                        {
        //                            cusVenAgingMode = new CusVenAgingModel();
        //                            cusVenAgingMode.Entity = Convert.ToString(dr[0]);
        //                            cusVenAgingMode.Date = dr[1] != DBNull.Value ? Convert.ToDateTime(dr[1]) : (DateTime?)null;
        //                            cusVenAgingMode.DocNo = Convert.ToString(dr[2]);
        //                            cusVenAgingMode.DocCurrency = dr[3] != DBNull.Value ? Convert.ToString(dr[3]) : string.Empty;
        //                            cusVenAgingMode.Current = dr[6] != DBNull.Value ? Convert.ToDecimal(dr[6]) : (decimal?)null;
        //                            cusVenAgingMode._1to30 = dr[7] != DBNull.Value ? Convert.ToDecimal(dr[7]) : (decimal?)null;
        //                            cusVenAgingMode._31to60 = dr[8] != DBNull.Value ? Convert.ToDecimal(dr[8]) : (decimal?)null;
        //                            cusVenAgingMode._61to90 = dr[9] != DBNull.Value ? Convert.ToDecimal(dr[9]) : (decimal?)null;
        //                            cusVenAgingMode._91to120 = dr[10] != DBNull.Value ? Convert.ToDecimal(dr[10]) : (decimal?)null;
        //                            cusVenAgingMode._120 = dr[11] != DBNull.Value ? Convert.ToDecimal(dr[11]) : (decimal?)null;
        //                            cusVenAgingMode.DocBalanceAmount = dr[13] != DBNull.Value ? Convert.ToDecimal(dr[13]) : (decimal?)null;
        //                            cusVenAgingMode.BaseBalanceAmount = dr[14] != DBNull.Value ? Convert.ToDecimal(dr[14]) : (decimal?)null;
        //                            cusVenAgingMode.DocType = Convert.ToString(dr[4]);
        //                            cusVenAgingMode.ServiceEntity = Convert.ToString(dr[5]);
        //                            cusVenAgingMode.DocumentId = dr[12] != DBNull.Value ? ((Guid?)dr.GetValue(12)) : (Guid?)null;
        //                            cusVenAgingMode.ServiceCompanyId = dr[15] != DBNull.Value ? Convert.ToInt64(dr[15]) : (long?)null;
        //                            cusVenAgingMode.SubType = Convert.ToString(dr[16]);
        //                            cusVenAgingMode.DueDate = dr[17] != DBNull.Value ? Convert.ToDateTime(dr[17]) : (DateTime?)null;

        //                            lstCusVenAgingMode.Add(cusVenAgingMode);
        //                        }

        //                    }

        //                    DBLoges(spName, "Binding the value complete", "Step 5");
        //                    if (con.State == ConnectionState.Open)
        //                        con.Close();
        //                }
        //            }
        //        }
        //        return lstCusVenAgingMode;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public List<CusVenAgingUnCheckInvModel> GetCutomerAndVendorAgingUnCheckInv(CustomerViewModel customerViewModel, string ConnectionString)
        {
            List<CusVenAgingModel> lstCusVenAgingMode = new List<CusVenAgingModel>();
            try
            {
                using (con = new SqlConnection(ConnectionString))
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
                    //if (customerViewModel.IsCustomer)
                    //    cmd.Parameters.AddWithValue("@CreditLimit", customerViewModel.CreditLimit);
                    dr = cmd.ExecuteReader();

                    dt = new DataTable();


                    dt.Load(dr);

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


        public object GetGeneralLedgerLu(long companyId, string ConnectionString, string Username)
        {
            GeneralLegderLUVM glLu = new GeneralLegderLUVM();
            List<ServiceEntity> lstServiceEntity = new List<ServiceEntity>();
            List<ChartOfAccount> lstCOA = new List<ChartOfAccount>();

            try
            {
                using (con = new SqlConnection(ConnectionString))
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
                            ServiceEntity serviceEntity = new ServiceEntity();
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
                            ChartOfAccount coa = new ChartOfAccount();
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return glLu;
        }



        public List<GLViewModel> GetGeneralLedger(GeneralLedgerViewModel generalLedgerViewModel, string ConnectionString)
        {
            try
            {
                if (generalLedgerViewModel != null && generalLedgerViewModel.ServiceCompany != null)
                {
                    using (con = new SqlConnection(ConnectionString))
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

                                return dt.AsEnumerable().Select(z => new GLViewModel()
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
                }
                else
                {
                    return new List<GLViewModel>();
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

        public async Task<GST> GetGSTFirstLevel(long companyId, DateTime? fromDate, DateTime? toDate, long serviceCompanyId, string ConnectionString)
        {
            GST gst = new GST();
            List<GSTFirstVM> gstList = new List<GSTFirstVM>();
            try
            {
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("RptGSTReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@CompanyValue", companyId);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    cmd.Parameters.AddWithValue("@ServiceCompany", serviceCompanyId.ToString());
                    using (dr = await cmd.ExecuteReaderAsync())
                    {
                        List<string> colNames = new List<string>() { "Total Value of Standard - Rated Supplies (excluding GST)", "Total Value of Zero-Rated Supplies", "Total Value of Exempt Supplies", "Total Supplies", "Total Value of Taxable Purchases (excluding GST)", "Output Tax Due", "Input Tax and Refunds Claimed", "Net GST to be Paid to / Claimed from IRAS", "Total Value of Goods imported Under MES / 3PL / Other Approved Schemes", "Did you claim for GST you had refunded to tourists?", "Did you make any bad debt relief claims and/or refund claims for reverse charge transactions?", "Did you make any pre-registration claims?", "Revenue", "Did you import services subject to GST under Reverse Charge?", "Did you operate an electronics marketplace to supply digital services subject to GST on behalf of third-party suppliers?" };
                        dt = new DataTable();

                        dt.Load(dr);

                        var columCount = dt.Columns.Count;
                        gst.CompanyId = companyId;
                        gst.GST_Number = dt.AsEnumerable().Select(a => a.Field<string>(0)).FirstOrDefault();
                        gst.Total_GST_input_tax = "Total GST input tax";
                        gst.Total_GST_output_tax = "Total GST output tax";
                        for (int i = 1; i < columCount; i++)
                        {
                            GSTFirstVM gstfirstVM = new GSTFirstVM();
                            gstfirstVM.Name = colNames[i - 1];
                            if (i == 10 || i == 11 || i == 12)
                            {
                                gstfirstVM.Value = null;
                            }
                            else
                                gstfirstVM.Value = String.Format("{0:0.00}", Convert.ToDouble(dt.AsEnumerable().Select(a => a[i] == DBNull.Value ? 0 : a[i]).FirstOrDefault()));

                            if (i == 1 || i == 2 || i == 3 || i == 6 || i == 13 || i == 14 || i == 15)
                                gstfirstVM.Type = "Output";
                            else if (i == 4 || i == 8)
                                gstfirstVM.Type = "null";
                            else
                                gstfirstVM.Type = "Input";
                            gstList.Add(gstfirstVM);
                        }
                        gst.GSTDetails = gstList;
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return gst;
        }


        public IQueryable<GLOutputVMK> GetGSTOutputK(long companyId, DateTime? fromDate, DateTime? toDate, string serviceCompany, string GSTNumber, string ConnectionString)
        {

            try
            {
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("[dbo].[Rpt_GLOUTPUT_TAX]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyValue", companyId);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    cmd.Parameters.AddWithValue("@ServiceCompany", serviceCompany);
                    cmd.Parameters.AddWithValue("@GSTNO", GSTNumber);
                    dr = cmd.ExecuteReader();
                    dt = new DataTable();


                    dt.Load(dr);

                    var list = dt.AsEnumerable().Select(z => new GLOutputVMK()
                    {
                        Doc_Type = z.Field<string>(0),
                        Doc_Date = z[1] != DBNull.Value ? Convert.ToDateTime(z[1]) : (DateTime?)null,
                        Doc_RefNo = z.Field<string>(2),
                        Doc_No = z.Field<string>(3),
                        Entity = z.Field<string>(4),
                        Doc_Description = z.Field<string>(5),
                        Tax_Code = z.Field<string>(6),
                        Tax_RateIn_Var = z.Field<string>(7),
                        SR_GST = z[8] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[8])) : null,
                        ZR_GST = z[9] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[9])) : null,
                        ES33_GST = z[10] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[10])) : null,
                        ESN33_GST = z[11] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[11])) : null,
                        DS_GST = z[12] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[12])) : null,
                        OS_GST = z[13] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[13])) : null,
                        TOTAL_GST = z[14] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[14])) : null,
                        SR_NET = z[15] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[15])) : null,
                        ZR_NET = z[16] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[16])) : null,
                        ES33_NET = z[17] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[17])) : null,
                        ESN33_NET = z[18] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[18])) : null,
                        DS_NET = z[19] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[19])) : null,
                        OS_NET = z[20] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[20])) : null,
                        TOTAL_NET = z[21] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[21])) : null,
                        Gross_Amount = z[22] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[22])) : null,
                        COA = z.Field<string>(23),
                        Account_Type = z.Field<string>(24),
                        Service_Company = z.Field<string>(25),
                        DocumentId = z[26] != DBNull.Value ? (Guid?)(z[26]) : (Guid?)null,
                        ServiceCompamyId = Convert.ToInt64(z[27]),
                        DocSubType = z.Field<string>(28)

                    }).AsQueryable().OrderBy(z => z.Doc_Date);
                    return list;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }


        }

        public IQueryable<GLInputVMK> getGSTInputTaxK(long companyId, DateTime? fromDate, DateTime? toDate, string serviceCompany, string GSTNumber, string ConnectionString)
        {

            try
            {
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("[dbo].[Rpt_GLINPUT_TAX]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyValue", companyId);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    cmd.Parameters.AddWithValue("@ServiceCompany", serviceCompany);
                    cmd.Parameters.AddWithValue("@GSTNO", GSTNumber);
                    dr = cmd.ExecuteReader();
                    dt = new DataTable();

                    dt.Load(dr);

                    return dt.AsEnumerable().Select(z => new GLInputVMK()
                    {
                        Doc_Type = z.Field<string>(0),
                        Doc_Date = z[1] != DBNull.Value ? Convert.ToDateTime(z[1]) : (DateTime?)null,
                        Doc_RefNo = z.Field<string>(2),
                        Doc_No = z.Field<string>(3),
                        Entity = z.Field<string>(4),
                        Doc_Description = z.Field<string>(5),
                        Tax_Code = z.Field<string>(6),
                        Tax_RateIn_Var = z.Field<string>(7),
                        TX_GST = z[8] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[8])) : null,
                        ZP_GST = z[9] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[9])) : null,
                        IM_GST = z[10] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[10])) : null,
                        ME_GST = z[11] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[11])) : null,
                        IGDS_GST = z[12] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[12])) : null,
                        TX_ESS_GST = z[13] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[13])) : null,
                        TX_N33_GST = z[14] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[14])) : null,
                        TX_RE_GST = z[15] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[15])) : null,
                        TOTAL_GST = z[16] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[16])) : null,
                        TX_NET = z[17] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[17])) : null,
                        ZP_NET = z[18] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[18])) : null,
                        IM_NET = z[19] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[19])) : null,
                        ME_NET = z[20] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[20])) : null,
                        IGDS_NET = z[21] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[21])) : null,
                        TX_ESS_NET = z[22] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[22])) : null,
                        TX_N33_NET = z[23] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[23])) : null,
                        TX_RE_NET = z[24] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[24])) : null,
                        TOTAL_NET = z[25] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[25])) : null,
                        Gross_Amount = z[26] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[26])) : null,
                        COA = z.Field<string>(27),
                        Account_Type = z.Field<string>(28),
                        Service_Company = z.Field<string>(29),
                        DocumentId = z[30] != DBNull.Value ? (Guid?)(z[30]) : (Guid?)null,
                        ServiceCompamyId = Convert.ToInt64(z[31]),
                        DocSubType = z.Field<string>(32)
                    }).AsQueryable().OrderBy(z => z.Doc_Date);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

        }

        public CustVenAgingLUVM CustVenAgingLU(long? tenantId, bool? isCustomer, string ConnectionString, string userName)
        {
            DBLoges("CustVenAgingLU",$"CustVenAgingLU Method Entering  {DateTime.UtcNow}","Step 1");

            CustVenAgingLUVM custLu = new CustVenAgingLUVM();
            List<CommonClass> commonClass = new List<CommonClass>();
            try
            {
                string query = null;
                //int isCust = isCustomer == true ? 1 : 0;
                //newlly modified by lokanath
                if (isCustomer == true)
                {
                    var iscustomer = "true";
                    DBLoges(iscustomer, $"CustVenAgingLU Method Entering  {DateTime.UtcNow}", "Step 2");
                    //query1 = $"SELECT MAX(TimeStamp) as TimeStamp FROM Bean.BeanReceivablePackageLogs";
                    query = $"Select Distinct 'ServiceCompany' As TableName,Null as EntityId, comp.Id as Id,Name as Name,ShortName as SName from Common.Company comp with(nolock) join Common.CompanyUser CU with(nolock) on comp.ParentId=CU.CompanyId Join Common.CompanyUserDetail CUD with(nolock) on CUD.CompanyUserId=CU.id where ParentId={tenantId} and CUD.ServiceEntityId=comp.Id and cu.Username='{userName}' group by comp.id,Name,ShortName order by SName;Select  Distinct TableName,EntityId,Null as Id,Name,SName From (Select Distinct 'Entity' As TableName, BE.Id EntityId, BE.Name, Null as SName From Bean.Journal J with(nolock) Inner Join Bean.JournalDetail JD with(nolock) on J.Id = JD.JournalId Inner Join Bean.Entity BE  with(nolock) on JD.EntityId = BE.Id JOIN Common.Company CE  with(nolock) on CE.ParentId = {tenantId} Where BE.CompanyId = {tenantId}  AND JD.DocType IN('Invoice', 'Credit Note', 'Debit Note', 'Debt Provision') AND (JD.ClearingStatus<>('Cleared') OR JD.ClearingStatus IS NULL) Union All Select Distinct  'Entity' As TableName, BE.Id EntityId, BE.Name, Null as SName From Bean.Invoice I  with(nolock) Inner Join Bean.Entity BE  with(nolock) on I.EntityId = BE.Id JOIN Common.Company CE with(nolock) on CE.ParentId = {tenantId} Where BE.CompanyId = {tenantId} AND I.DocType IN('Invoice', 'Credit Note') AND I.DocSubType IN('Opening Bal')) AS A Order By Name;Select Distinct 'Currency' AS TableName, Null as EntityId,Null as Id,jd.DocCurrency as Name, Null as SName from Bean.Journal as j with(nolock) join Bean.JournalDetail as jd with(nolock) on j.Id = jd.JournalId where j.CompanyId = {tenantId}  group by jd.DocCurrency order by Name";
                    DBLoges(iscustomer, $"CustVenAgingLU Method Complted   {DateTime.UtcNow}", "Step 2");
                }
                else
                {
                    var iscustomer = "false";
                    DBLoges(iscustomer, $"CustVenAgingLU Method Entering  {DateTime.UtcNow}", "Step 2");
                    //query1 = $"SELECT MAX(TimeStamp) as TimeStamp FROM Bean.BeanPayablesAgingPackageLogs";
                    query = $"Select Distinct 'ServiceCompany' As TableName,Null as EntityId, comp.Id as Id,Name as Name,ShortName as SName from Common.Company comp with(nolock) join Common.CompanyUser CU with(nolock) on comp.ParentId=CU.CompanyId Join Common.CompanyUserDetail CUD with(nolock) on CUD.CompanyUserId=CU.id where ParentId={tenantId} and CUD.ServiceEntityId=comp.Id and cu.Username= '{userName}' group by comp.id,Name,ShortName order by SName;Select  Distinct TableName,EntityId,Null as Id,Name,SName  From ( Select Distinct 'Entity' As TableName, BE.Id EntityId, BE.Name,Null as SName From Bean.Journal J with(nolock) Inner Join Bean.JournalDetail JD with(nolock) on J.Id = JD.JournalId Inner Join Bean.Entity BE with(nolock) on JD.EntityId = BE.Id JOIN Common.Company CE with(nolock) on CE.ParentId = {tenantId} Where BE.CompanyId = {tenantId} AND JD.DocType IN('Bill','Credit Memo','Bill Payment','Cash Payment','Payroll Bill','Payroll Payment') AND(JD.ClearingStatus<>('Cleared') OR JD.ClearingStatus IS NULL) Union All Select Distinct 'Entity' As TableName, BE.Id EntityId, BE.Name,Null as SName From Bean.Bill J with(nolock) Inner Join Bean.BillDetail JD with(nolock) on J.Id = JD.BillId Inner Join Bean.Entity BE with(nolock) on J.EntityId = BE.Id JOIN Common.Company CE with(nolock) on CE.ParentId = {tenantId} Where BE.CompanyId = {tenantId} AND J.DocType IN('Bill') AND J.DocSubType IN('Opening Bal') Union All Select Distinct 'Entity' As TableName, BE.Id EntityId, BE.Name,Null as SName From Bean.CreditMemo J with(nolock) Inner Join Bean.CreditMemoDetail JD with(nolock) on J.Id = JD.CreditMemoId Inner Join Bean.Entity BE with(nolock) on J.EntityId = BE.Id JOIN Common.Company CE with(nolock) on CE.ParentId = {tenantId} Where BE.CompanyId ={tenantId} AND J.DocType IN('Credit Memo') AND J.DocSubType IN('Opening Bal')) AS A Order By Name; Select Distinct 'Currency' AS TableName, Null as EntityId,Null as Id,jd.DocCurrency as Name, Null as SName from Bean.Journal as j with(nolock) join Bean.JournalDetail as jd with(nolock) on j.Id = jd.JournalId where j.CompanyId = {tenantId}  group by jd.DocCurrency order by Name";
                    DBLoges(iscustomer, $"CustVenAgingLU Method Entering  {DateTime.UtcNow}", "Step 2");
                }

                DBLoges("Executer Reader", $"CustVenAgingLU Method Entering Execute Reader  started {DateTime.UtcNow}", "Step 3");
                int? resultSetCount = query.Split(';').Count();
                //List<string> value1 = new List<string>();
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        for (int i = 0; i <= resultSetCount; i++)
                        {
                            //dt = new DataTable();
                            //dt.Load(dr);
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    commonClass.Add(new CommonClass()
                                    {
                                        TableName = dr[0] != DBNull.Value ? dr[0].ToString() : null,
                                        EntityId = dr[1] != DBNull.Value ? Guid.Parse(dr[1].ToString()) : (Guid?)null,
                                        Id = dr[2] != DBNull.Value ? Convert.ToInt64(dr[2]) : (long?)null,
                                        Name = dr[3] != DBNull.Value ? dr[3].ToString() : null,
                                        SName = dr[4] != DBNull.Value ? dr[4].ToString() : null
                                    });
                                }
                            }
                            dr.NextResult();
                        }
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
                DBLoges("Executer Reader", $"CustVenAgingLU Method Entering Execute Reader completed {DateTime.UtcNow}", "Step 3");
                //Commented by Pradhan due to analytics issue
                //using (con = new SqlConnection(DWHConnection))
                //{
                //    if (con.State != ConnectionState.Open)
                //        con.Open();
                //    cmd = new SqlCommand(query1, con);
                //    cmd.CommandType = CommandType.Text;
                //    SqlDataReader dr = cmd.ExecuteReader();
                //    if (dr.HasRows)
                //    {
                //        while (dr.Read())
                //        {
                //            custLu.LastRefreshedDate = dr["TimeStamp"] != DBNull.Value ? Convert.ToDateTime(dr["TimeStamp"]) : (DateTime?)null;
                //        }
                //    }
                //    if (con.State == ConnectionState.Open)
                //        con.Close();
                //}
                DBLoges("ServiceEntities", $"CustVenAgingLU Method ServiceEntities Lookup Started  {DateTime.UtcNow}", "Step 4");
                custLu.ServiceEntities = commonClass.Where(a => a.TableName == "ServiceCompany").Select(c => new ServiceEntity()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ShortName = c.SName
                }).ToList();
                DBLoges("ServiceEntities", $"CustVenAgingLU Method ServiceEntitiescLookup Completed  {DateTime.UtcNow}", "Step 4");

                DBLoges("Entities", $"CustVenAgingLU Method Entities Lookup Started  {DateTime.UtcNow}", "Step 5");
                custLu.Entities = commonClass.Where(a => a.TableName == "Entity").Select(c => new Entity()
                {
                    EntityId = c.EntityId.Value.ToString(),
                    EntityName = c.Name
                }).ToList();
                DBLoges("Entities", $"CustVenAgingLU Method Entities Lookup Completed  {DateTime.UtcNow}", "Step 5");

                DBLoges("DocCurrencies", $"CustVenAgingLU Method DocCurrencies Lookup Started  {DateTime.UtcNow}", "Step 6");
                custLu.DocCurrencies = commonClass.Where(a => a.TableName == "Currency").Where(a => a.Name != null).Select(c => c.Name).ToList();
                DBLoges("DocCurrencies", $"CustVenAgingLU Method DocCurrencies Lookup Completed  {DateTime.UtcNow}", "Step 6");

                DBLoges("CreditLimits", $"CustVenAgingLU Method CreditLimits Lookup Started  {DateTime.UtcNow}", "Step 7");
                custLu.CreditLimits = new List<string> { "Exceeded", "Not Exceeded" };
                DBLoges("CreditLimits", $"CustVenAgingLU Method CreditLimits Lookup Completed  {DateTime.UtcNow}", "Step 7");

                DBLoges("Natures", $"CustVenAgingLU Method Natures Lookup Started  {DateTime.UtcNow}", "Step 8");
                custLu.Natures = new List<string> { "Trade", "Others" };
                DBLoges("Natures", $"CustVenAgingLU Method Natures Lookup Competed  {DateTime.UtcNow}", "Step 8");

 
                #region Interco_Nature_Based_on_IB_Checked 
                using (con = new SqlConnection(ConnectionString))
                {
                    DBLoges("Natures is Interco", $"CustVenAgingLU Method Natures is Interco Lookup Started  {DateTime.UtcNow}", "Step 9");

                    query = $"Select IC.IsInterCompanyEnabled from Bean.InterCompanySetting IC where CompanyId={tenantId} and ic.InterCompanyType='Billing'";
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand(query, con);
                    bool? isIBActivated = Convert.ToBoolean(cmd.ExecuteScalar());
                    if (con.State != System.Data.ConnectionState.Closed)
                        con.Close();
                    if (isIBActivated == true)
                        custLu.Natures.Add("Interco");
                    DBLoges("Natures is Interco", $"CustVenAgingLU Method Natures is Interco Lookup Completed  {DateTime.UtcNow}", "Step 9");
                }
                #endregion Interco_Nature_Based_on_IB_Checked

                DBLoges("Currency", $"CustVenAgingLU Method Currency  Lookup Started  {DateTime.UtcNow}", "Step 10");
                custLu.Currency = new List<string> { "Doc Currency", "Base Currency" };
                DBLoges("Currency", $"CustVenAgingLU Method Currency  Lookup Completed  {DateTime.UtcNow}", "Step 10");

                DBLoges("CustVenAgingLU", $"CustVenAgingLU Method Completed  {DateTime.UtcNow}", "Step 1");
            }
            catch (Exception ex)
            {

            }
            return custLu;
        }

        public FinancialYearLUVM GetFinancialYear(long companyId, DateTime? toDate, string ConnectionString)
        {
            FinancialYearLUVM fineYear = new FinancialYearLUVM();
            try
            {
                var res = ConnectionString.Split(';');
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
                    MethodName = "GetFinancialYear",
                };
                string secondarydboj = JsonConvert.SerializeObject(commonObjModel);

                LoggingHelper.LogMessage(BeanLogConstant.ReportsReadOnlyApplicationService, secondarydboj);


                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    String query = "[dbo].[BSGLFromDateLink]";
                    cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@AsOfDate", toDate);
                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            fineYear.fromDate = dr[0] != DBNull.Value ? Convert.ToDateTime(dr[0]) : (DateTime?)null;
                        }
                    }
                    else
                    {
                        fineYear.fromDate = (DateTime?)null;
                    }
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return fineYear;
        }

        public IQueryable<GSTVM> GetGSTSecondLevel(long companyId, DateTime? fromDate, DateTime? toDate, long serviceCompanyId, string GSTNumber, string lstOfTaxCodes, string taxType, string ConnectionString)
        {
            try
            {
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("HTML_GLOUTPUTINPUT_GLTAX", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyValue", companyId);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    cmd.Parameters.AddWithValue("@ServiceCompany", serviceCompanyId.ToString());
                    cmd.Parameters.AddWithValue("@GSTNO", GSTNumber);
                    cmd.Parameters.AddWithValue("@Tax_CodeList", lstOfTaxCodes);
                    cmd.Parameters.AddWithValue("@Tax_Type", taxType);
                    using (dr = cmd.ExecuteReader())
                    {
                        dt = new DataTable();

                        dt.Load(dr);

                        var list = dt.AsEnumerable().Select(z => new GSTVM()
                        {
                            Doc_Type = z.Field<string>(0),
                            Doc_Date = z[1] != DBNull.Value ? Convert.ToDateTime(z[1]) : (DateTime?)null,
                            Doc_RefNo = z.Field<string>(2),
                            Doc_No = z.Field<string>(3),
                            Entity = z.Field<string>(4),
                            Doc_Description = z.Field<string>(5),
                            Tax_Code = z.Field<string>(6),
                            Tax_RateIn_Var = z.Field<string>(7),


                            TX_GST = dt.Columns["TX_GST"] != null ? z["TX_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX_GST"])) : null : null,
                            ZP_GST = dt.Columns["ZP_GST"] != null ? z["ZP_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["ZP_GST"])) : null : null,
                            IM_GST = dt.Columns["IM_GST"] != null ? z["IM_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["IM_GST"])) : null : null,
                            ME_GST = dt.Columns["ME_GST"] != null ? z["ME_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["ME_GST"])) : null : null,
                            IGDS_GST = dt.Columns["IGDS_GST"] != null ? z["IGDS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["IGDS_GST"])) : null : null,
                            BL_GST = dt.Columns["BL_GST"] != null ? z["BL_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["BL_GST"])) : null : null,
                            NR_GST = dt.Columns["NR_GST"] != null ? z["NR_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["NR_GST"])) : null : null,
                            EP_GST = dt.Columns["EP_GST"] != null ? z["EP_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["EP_GST"])) : null : null,
                            OP_GST = dt.Columns["OP_GST"] != null ? z["OP_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["OP_GST"])) : null : null,
                            TX_ESS_GST = dt.Columns["TX-ESS_GST"] != null ? z["TX-ESS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX-ESS_GST"])) : null : null,
                            TX_N33_GST = dt.Columns["TX-N33_GST"] != null ? z["TX-N33_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX-N33_GST"])) : null : null,
                            TX_RE_GST = dt.Columns["TX-RE_GST"] != null ? z["TX-RE_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX-RE_GST"])) : null : null,
                            TX_DSPS_GST = dt.Columns["TX-DSPS_GST"] != null ? z["TX-DSPS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX-DSPS_GST"])) : null : null,
                            TXCA_GST = dt.Columns["TXCA_GST"] != null ? z["TXCA_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TXCA_GST"])) : null : null,
                            TX_GMS_GST = dt.Columns["TX-GMS_GST"] != null ? z["TX-GMS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX-GMS_GST"])) : null : null,

                            TXRC_TS_GST = dt.Columns["TXRC-TS_GST"] != null ? z["TXRC-TS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TXRC-TS_GST"])) : null : null,
                            TXRC_ESS_GST = dt.Columns["TXRC-ESS_GST"] != null ? z["TXRC-ESS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TXRC-ESS_GST"])) : null : null,
                            TXRC_N33_GST = dt.Columns["TXRC-N33_GST"] != null ? z["TXRC-N33_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TXRC-N33_GST"])) : null : null,
                            TXRC_RE_GST = dt.Columns["TXRC-RE_GST"] != null ? z["TXRC-RE_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TXRC-RE_GST"])) : null : null,
                            IM_ESS_GST = dt.Columns["IM-ESS_GST"] != null ? z["IM-ESS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["IM-ESS_GST"])) : null : null,
                            IM_N33_GST = dt.Columns["IM-N33_GST"] != null ? z["IM-N33_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["IM-N33_GST"])) : null : null,
                            IM_RE_GST = dt.Columns["IM-RE_GST"] != null ? z["IM-RE_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["IM-RE_GST"])) : null : null,

                            //TOTAL_GST = z[16] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z[16])) : null,

                            TXRC_TS_NET = dt.Columns["TXRC-TS_NET"] != null ? z["TXRC-TS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TXRC-TS_NET"])) : null : null,
                            TXRC_ESS_NET = dt.Columns["TXRC-ESS_NET"] != null ? z["TXRC-ESS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TXRC-ESS_NET"])) : null : null,
                            TXRC_N33_NET = dt.Columns["TXRC-N33_NET"] != null ? z["TXRC-N33_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TXRC-N33_NET"])) : null : null,
                            TXRC_RE_NET = dt.Columns["TXRC-RE_NET"] != null ? z["TXRC-RE_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TXRC-RE_NET"])) : null : null,
                            IM_ESS_NET = dt.Columns["IM-ESS_NET"] != null ? z["IM-ESS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["IM-ESS_NET"])) : null : null,
                            IM_N33_NET = dt.Columns["IM-N33_NET"] != null ? z["IM-N33_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["IM-N33_NET"])) : null : null,
                            IM_RE_NET = dt.Columns["IM-RE_NET"] != null ? z["IM-RE_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["IM-RE_NET"])) : null : null,

                            TX_NET = dt.Columns["TX_NET"] != null ? z["TX_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX_NET"])) : null : null,
                            ZP_NET = dt.Columns["ZP_NET"] != null ? z["ZP_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["ZP_NET"])) : null : null,
                            IM_NET = dt.Columns["IM_NET"] != null ? z["IM_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["IM_NET"])) : null : null,
                            ME_NET = dt.Columns["ME_NET"] != null ? z["ME_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["ME_NET"])) : null : null,
                            IGDS_NET = dt.Columns["IGDS_NET"] != null ? z["IGDS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["IGDS_NET"])) : null : null,
                            BL_NET = dt.Columns["BL_NET"] != null ? z["BL_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["BL_NET"])) : null : null,
                            NR_NET = dt.Columns["NR_NET"] != null ? z["NR_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["NR_NET"])) : null : null,
                            EP_NET = dt.Columns["EP_NET"] != null ? z["EP_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["EP_NET"])) : null : null,
                            OP_NET = dt.Columns["OP_NET"] != null ? z["OP_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["OP_NET"])) : null : null,
                            TX_ESS_NET = dt.Columns["TX-ESS_NET"] != null ? z["TX-ESS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX-ESS_NET"])) : null : null,
                            TX_N33_NET = dt.Columns["TX-N33_NET"] != null ? z["TX-N33_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX-N33_NET"])) : null : null,
                            TX_RE_NET = dt.Columns["TX-RE_NET"] != null ? z["TX-RE_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX-RE_NET"])) : null : null,
                            TX_DSPS_NET = dt.Columns["TX-DSPS_NET"] != null ? z["TX-DSPS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX-DSPS_NET"])) : null : null,
                            TXCA_NET = dt.Columns["TXCA_NET"] != null ? z["TXCA_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TXCA_NET"])) : null : null,
                            TX_GMS_NET = dt.Columns["TX-GMS_NET"] != null ? z["TX-GMS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TX-GMS_NET"])) : null : null,

                            //outupt
                            SR_GST = dt.Columns["SR_GST"] != null ? z["SR_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SR_GST"])) : null : null,
                            ZR_GST = dt.Columns["ZR_GST"] != null ? z["ZR_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["ZR_GST"])) : null : null,
                            ES33_GST = dt.Columns["ES33_GST"] != null ? z["ES33_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["ES33_GST"])) : null : null,
                            ESN33_GST = dt.Columns["ESN33_GST"] != null ? z["ESN33_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["ESN33_GST"])) : null : null,
                            DS_GST = dt.Columns["DS_GST"] != null ? z["DS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["DS_GST"])) : null : null,
                            OS_GST = dt.Columns["OS_GST"] != null ? z["OS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["OS_GST"])) : null : null,
                            SRCA_C_GST = dt.Columns["SRCA-C_GST"] != null ? z["SRCA-C_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SRCA-C_GST"])) : null : null,
                            SRRC_GST = dt.Columns["SRRC_GST"] != null ? z["SRRC_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SRRC_GST"])) : null : null,
                            SROVR_GST = dt.Columns["SROVR_GST"] != null ? z["SROVR_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SROVR_GST"])) : null : null,
                            SRCA_S_GST = dt.Columns["SRCA-S_GST"] != null ? z["SRCA-S_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SRCA-S_GST"])) : null : null,
                            SR_DSPS_GST = dt.Columns["SR-DSPS_GST"] != null ? z["SR-DSPS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SR-DSPS_GST"])) : null : null,
                            SR_GMS_GST = dt.Columns["SR-GMS_GST"] != null ? z["SR-GMS_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SR-GMS_GST"])) : null : null,
                            TOTAL_GST = z["TOTAL_GST"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TOTAL_GST"])) : null,
                            SRRC_NET = dt.Columns["SRRC_NET"] != null ? z["SRRC_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SRRC_NET"])) : null : null,
                            SROVR_NET = dt.Columns["SROVR_NET"] != null ? z["SROVR_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SROVR_NET"])) : null : null,
                            SR_NET = dt.Columns["SR_NET"] != null ? z["SR_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SR_NET"])) : null : null,
                            ZR_NET = dt.Columns["ZR_NET"] != null ? z["ZR_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["ZR_NET"])) : null : null,
                            ES33_NET = dt.Columns["ES33_NET"] != null ? z["ES33_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["ES33_NET"])) : null : null,
                            ESN33_NET = dt.Columns["ESN33_NET"] != null ? z["ESN33_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["ESN33_NET"])) : null : null,
                            DS_NET = dt.Columns["DS_NET"] != null ? z["DS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["DS_NET"])) : null : null,
                            OS_NET = dt.Columns["OS_NET"] != null ? z["OS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["OS_NET"])) : null : null,
                            SRCA_C_NET = dt.Columns["SRCA-C_NET"] != null ? z["SRCA-C_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SRCA-C_NET"])) : null : null,
                            SRCA_S_NET = dt.Columns["SRCA-S_NET"] != null ? z["SRCA-S_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SRCA-S_NET"])) : null : null,
                            SR_DSPS_NET = dt.Columns["SR-DSPS_NET"] != null ? z["SR-DSPS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SR-DSPS_NET"])) : null : null,
                            SR_GMS_NET = dt.Columns["SR-GMS_NET"] != null ? z["SR-GMS_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["SR-GMS_NET"])) : null : null,
                            TOTAL_NET = z["TOTAL_NET"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TOTAL_NET"])) : null,
                            Gross_Amount = z["Gross Amount"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["Gross Amount"])) : null,
                            COA = z.Field<string>("COA"),
                            Account_Type = z.Field<string>("Account Type"),
                            Service_Company = z.Field<string>("Service Comapany"),
                            DocumentId = z["DocumentId"] != DBNull.Value ? (Guid?)(z["DocumentId"]) : (Guid?)null,
                            ServiceCompamyId = Convert.ToInt64(z["ServiceCompanyId"]),
                            DocSubType = z.Field<string>("DocSubType"),
                            BaseTax_Amount = z["TOTAL_GSTBase"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TOTAL_GSTBase"])) : null,
                            BaseNet_Amount = z["TOTAL_NETBase"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["TOTAL_NETBase"])) : null,
                            BaseGross_Amount = z["BaseGross Amount"] != DBNull.Value ? string.Format("{0:0.00}", Convert.ToDouble(z["BaseGross Amount"])) : null,
                        }).AsQueryable().OrderBy(z => z.Doc_Date);
                        if (con.State == ConnectionState.Open)
                            con.Close();
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public FinancialYearLUVM GetFinancialLu(long companyId, DateTime? toDate, string ConnectionString)
        {
            FinancialYearLUVM glLu = new FinancialYearLUVM();
            try
            {
                string query = "Declare @CompanyId INT=" + companyId + ",@ToDate DateTime='" + toDate + "' Select Case when dateadd(YEAR, 0, dateadd(day, 1, cast(REPLACE(BusinessYearEnd, '-', ' ')+cast(datepart(year, @ToDate/*getdate()*/) as char(4)) as date))) < @ToDate then dateadd(YEAR, 0, dateadd(day, 1, cast(REPLACE(BusinessYearEnd, '-', ' ') + cast(datepart(year, @ToDate/*getdate()*/) as char(4)) as date)))else dateadd(YEAR, -1, dateadd(day, 1, cast(REPLACE(BusinessYearEnd, '-', ' ') + cast(datepart(year, @ToDate/*getdate()*/) as char(4)) as date))) end as FromDate from[Common].[Localization] Where CompanyId = @CompanyId";
                using (con = new SqlConnection(ConnectionString))
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



        public void DBLoges(string methodName, string message, string steps)
        {
            using (SqlConnection connection = new SqlConnection(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection))
            {
                string query = "INSERT INTO common.DbLogs (Id, MethodName, Message, Steps, Datetime) VALUES (@Id, @MethodName, @Message, @Steps, @Datetime)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                    command.Parameters.Add("@MethodName", SqlDbType.NVarChar, 250).Value = methodName;
                    command.Parameters.Add("@Message", SqlDbType.NVarChar, 250).Value = message;
                    command.Parameters.Add("@Steps", SqlDbType.NVarChar, 250).Value = steps;
                    command.Parameters.Add("@Datetime", SqlDbType.DateTime2).Value = DateTime.UtcNow;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
