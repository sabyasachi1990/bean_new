using AppsWorld.DashBoardModule.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AppsWorld.DashBoardModule.Models.Resources;
using AppsWorld.DashBoardModule.Model;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace AppsWorld.DashBoardModule.Service
{
    public class DashBoardServices
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AppsWorldPrdDwh"].ToString());
        SqlCommand cmd;
        SqlDataReader dr;
        DataTable dt;

        SqlCommand cmd1;
        SqlDataReader dr1;
        DataTable dt1;

        #region FinancialDashBoard
        public FinancialVM GetFinancialsDashBoard(DateTime? fromdate, string todate, int cid, string role, string type)
        {
            FinancialVM finObj = new FinancialVM();
            try
            {
                conn.Open();
                if (fromdate == null || todate == null)
                {
                    finObj.ToDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    DateTime prevyear = Convert.ToDateTime(finObj.ToDate).AddYears(-1);
                    finObj.FromDate = new DateTime(prevyear.Year, prevyear.Month, 1);
                }
                else
                {
                    finObj.FromDate = (DateTime)fromdate;
                    finObj.ToDate = (string)todate + "T" + "23:59:59";
                }
                if (type == "null")
                    cmd = new SqlCommand(Constants.Constants_FinancialDashBoard, conn);

                else if (type == "Profit and Loss" || type == "Balance Sheet")
                    cmd = new SqlCommand(Constants.Constants_FinancialDetailDashBoard, conn);
                else
                    cmd = new SqlCommand(Constants.Constants_FinancialScoreCard, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(Constants.Constants_FromDate, finObj.FromDate);
                cmd.Parameters.AddWithValue(Constants.Constants_ToDate, finObj.ToDate);
                cmd.Parameters.AddWithValue(Constants.Constants_Cid, cid);
                cmd.Parameters.AddWithValue(Constants.Constants_Role, role == "null" ? (object)DBNull.Value : role);
                if (type != "null")
                {
                    cmd.Parameters.AddWithValue(Constants.Constants_Type, type);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (type == "null" || type == "Profit and Loss" || type == "Balance Sheet")
                {
                    finObj.GetFinancialDashBoard = GetMultiDrillDownChart(dr);
                }
                if (type == "null" || type == "Profit and Loss ScoreCard" || type == "Balance Sheet ScoreCard")
                {
                    finObj.FinancialScoreCardModels = GetFinancialScoreCardData(dr, type);
                }
                conn.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return finObj;
        }



        private BasicColumnDrillDownVM GetMultiDrillDownChart(SqlDataReader dr)
        {
            BasicColumnDrillDownVM drildownChart = new BasicColumnDrillDownVM();
            List<DrillDownMainData> mainDataList = new List<DrillDownMainData>();
            List<DrillDownSubData> subDataList = new List<DrillDownSubData>();
            if (dr.HasRows)
            {
                dt = new DataTable();
                dt.Load(dr);
                IEnumerable<FinancialData> financialDataList = (from b in dt.AsEnumerable()
                                                                group b by b.Field<string>(0) into g
                                                                select new FinancialData
                                                                {
                                                                    SubCategory = g.Key,
                                                                    MonthYear = g.Select(r => r.Field<string>(1)).FirstOrDefault(),
                                                                    Amount = Math.Round(g.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2)
                                                                }).Distinct().ToList();
                foreach (var fndata in financialDataList)
                {
                    DrillDownMainData mainData = new DrillDownMainData();
                    List<DrillDownSeriesData> seriesDataList = new List<DrillDownSeriesData>();
                    mainData.name = fndata.SubCategory;
                    var financialDataList1 = (from row in dt.AsEnumerable()
                                              group row by new
                                              {
                                                  subCategory = row.Field<string>(0),
                                                  monthYear = row.Field<string>("MonthYear")
                                              } into g
                                              select new
                                              {
                                                  MonthYear = g.Key.monthYear,
                                                  Amount = Math.Round(g.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2),
                                                  SubCategory = g.Select(r => r.Field<string>(0)).FirstOrDefault()
                                              }).Distinct().ToList();

                    DrillDownSeriesData seriesData = null;
                    foreach (var fndata1 in financialDataList1)
                    {
                        if (mainData.name == fndata1.SubCategory)
                        {
                            seriesData = new DrillDownSeriesData()
                            {
                                name = fndata1.MonthYear,
                                y = fndata1.Amount,
                                drilldown = fndata1.MonthYear + "-" + mainData.name
                            };
                            seriesDataList.Add(seriesData);
                            DrillDownSubData subdata = new DrillDownSubData()
                            {
                                type = Constants.Constants_Column,
                                name = mainData.name,
                                id = seriesData.drilldown
                            };
                            var rows = dt.AsEnumerable()
                                      .Where(s => s.Field<string>(1) + "-" + s.Field<string>(0) == seriesData.drilldown)
                                      .GroupBy(s => s.Field<string>(0))
                                      .Select(b => new
                                      {
                                          SubName = b.Key,
                                          SubValue = Math.Round(b.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2)
                                      }).ToList();

                            foreach (var n in rows)
                            {
                                subdata.data.Add(new List<string> { n.SubName, n.SubValue.ToString() });
                            }
                            subDataList.Add(subdata);
                        }
                    }
                    mainData.data = seriesDataList;
                    mainDataList.Add(mainData);
                }
                drildownChart.MainData = mainDataList;
                drildownChart.SubData = subDataList;
            }
            else
            {
                MultipleLineStaticData(drildownChart, mainDataList);
            }
            return drildownChart;
        }

        private static void MultipleLineStaticData(BasicColumnDrillDownVM drildownChart, List<DrillDownMainData> mainDataList)
        {
            DrillDownMainData mainData = new DrillDownMainData()
            {
                name = "No Data",
                data = new List<DrillDownSeriesData>()
                                                 {
                                                   new DrillDownSeriesData {name="No Data" ,y=0}
                                                 }.ToList()
            };
            mainDataList.Add(mainData);
            drildownChart.MainData = mainDataList;
        }

        private List<FinancialScoreCardModel> GetFinancialScoreCardData(SqlDataReader dr, string type)
        {
            List<FinancialScoreCardModel> lstfnscoredata = new List<FinancialScoreCardModel>();
            DataTable dt = new DataTable("scorecardTable");
            dt.Load(dr);
            return lstfnscoredata = (from DataRow row in dt.Rows
                                     select new FinancialScoreCardModel()
                                     {
                                         Name = row["Name"].ToString(),
                                         CurrentMonthAmount = Convert.ToDouble(row["CurrentMonthAmount"]),
                                         PreviousMonthAmount = row.IsNull("PreviousMonthAmount") ? 0 : Convert.ToDouble(row["PreviousMonthAmount"]),
                                         //DifferanceMonthAmount = row.IsNull("DifferanceMonthAmount") ? 0 : Convert.ToDouble(row ["DifferanceMonthAmount"]),
                                         CurrentYearAmount = row.IsNull("CurrentYearAmount") ? 0 : Convert.ToDouble(row["CurrentYearAmount"]),
                                         PreviousYearAmount = row.IsNull("PreviousYearAmount") ? 0 : Convert.ToDouble(row["PreviousYearAmount"]),
                                     }).ToList();
        }

        #endregion

        #region GetFinancialRatios

        public KPIVM GetFinancialRatio(DateTime? fromdate, string todate, long cid, string role)
        {
            KPIVM kpi = new KPIVM();
            try
            {
                conn.Open();
                if (fromdate == null || todate == null)
                {
                    kpi.ToDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    DateTime prevyear = Convert.ToDateTime(kpi.ToDate).AddYears(-1);
                    kpi.FromDate = new DateTime(prevyear.Year, prevyear.Month, 1);
                }
                else
                {
                    kpi.ToDate = (string)todate + "T" + "23:59:59";
                    kpi.FromDate = (DateTime)fromdate;
                }
                cmd = new SqlCommand(Constants.Constants_FinancialsRatios, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(Constants.Constants_FromDate, kpi.FromDate);
                cmd.Parameters.AddWithValue(Constants.Constants_ToDate, kpi.ToDate);
                cmd.Parameters.AddWithValue(Constants.Constants_Cid, cid);
                cmd.Parameters.AddWithValue(Constants.Constants_Role, role == "null" ? (object)DBNull.Value : role);
                dr = cmd.ExecuteReader();
                kpi.KPIVMRatio = GetNetAssetKpi(dr);
                conn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return kpi;
        }

        private List<KPINetData> GetNetAssetKpi(SqlDataReader dr)
        {
            List<KPINetData> lstNet = new List<KPINetData>();
            DataTable dt = new DataTable("kpinetTable");
            if (dr.HasRows)
            {
                dt.Load(dr);
                string NetAssetratio1 = null;
                string NetAssetratio2 = null;
                string workData1 = null;
                string workData2 = null;
                string debtData1 = null;
                string debtData2 = null;
                double returnRatio = 0;
                double profitLoss = 0;
                NetAssetratio1 += dt.Rows[0]["Net Asset Ratio"].ToString().Split(':').First();
                NetAssetratio2 += dt.Rows[0]["Net Asset Ratio"].ToString().Split(':').Last();
                dt.Load(dr);
                workData1 += dt.Rows[1]["Working Capital Ratio"].ToString().Split(':').First();
                workData2 += dt.Rows[1]["Working Capital Ratio"].ToString().Split(':').Last();
                dt.Load(dr);
                debtData1 += dt.Rows[2]["Debt Equity Ratio"].ToString().Split(':').First();
                debtData2 += dt.Rows[2]["Debt Equity Ratio"].ToString().Split(':').Last();
                dt.Load(dr);
                returnRatio = Math.Round(dt.Rows[3].Field<decimal?>(3) == null ? 0 : Convert.ToDouble(dt.Rows[3].Field<decimal>(3)), 0);
                dt.Load(dr);
                profitLoss = Math.Round(dt.Rows[4].Field<decimal?>(4) == null ? 0 : Convert.ToDouble(dt.Rows[4].Field<decimal>(4)), 2);
                lstNet = new List<KPINetData>() {
                                                 new KPINetData()
                                                  {
                                                    NetAssetRatio1=NetAssetratio1.ToString(),
                                                    NetAssetRation2=NetAssetratio2.ToString(),
                                                    WorkingCapitalRatio1=workData1.ToString(),
                                                    WorkingCapitalRatio2=workData2.ToString(),
                                                    DebtEquityRatio1=debtData1.ToString(),
                                                    DebtEquityRatio2=debtData2.ToString(),
                                                    ReturnOnEquity=Convert.ToInt32(returnRatio),
                                                    ProfitorLoss=Convert.ToInt32(profitLoss)
                                                    }
                                                };
            }
            else
            {
                KPINetData data = new KPINetData();
                data.NetAssetRatio1 = "0";
                data.NetAssetRation2 = "0";
                data.WorkingCapitalRatio1 = "0";
                data.WorkingCapitalRatio2 = "0";
                data.DebtEquityRatio1 = "0";
                data.DebtEquityRatio2 = "0";
                data.ReturnOnEquity = 0;
                data.ProfitorLoss = 0;
                lstNet.Add(data);

            }
            return lstNet;
        }

        #endregion

        #region BankDetailsDashboard
        public BankDetailsDashBoardVM GetBankDetailsDashBoard(DateTime? fromdate, string todate, int cid, string role, string type)
        {
            BankDetailsDashBoardVM bnkObj = new BankDetailsDashBoardVM();
            try
            {
                conn.Open();
                if (fromdate == null || todate == null)
                {
                    bnkObj.ToDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    DateTime prevyear = Convert.ToDateTime(bnkObj.ToDate).AddYears(-1);
                    bnkObj.FromDate = new DateTime(prevyear.Year, prevyear.Month, 1);
                }
                else
                {
                    bnkObj.FromDate = (DateTime)fromdate;
                    bnkObj.ToDate = (string)todate + "T" + "23:59:59";
                }
                if (type == "null")
                    cmd = new SqlCommand(Constants.Constants_BankDashBoard, conn);
                else
                    cmd = new SqlCommand(Constants.Constants_BankDetailsDashBoard, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(Constants.Constants_FromDate, bnkObj.FromDate);
                cmd.Parameters.AddWithValue(Constants.Constants_ToDate, bnkObj.ToDate);
                cmd.Parameters.AddWithValue(Constants.Constants_Cid, cid);
                cmd.Parameters.AddWithValue(Constants.Constants_Role, role == "null" ? (object)DBNull.Value : role);
                if (type != "null")
                {
                    cmd.Parameters.AddWithValue(Constants.Constants_Type, type);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (type == "null" || type == "Bank Balance")
                {
                    bnkObj.BankBalanceDrillDownChart = GetBankDetailsDrillDownChart(dr);
                }
                if (type == "Change In Monthly Bank Balance")
                {
                    bnkObj.ChangeMonthlyDrillDownChart = GetChangeMonthlyLineChart(dr);
                }
                if (type == "null")
                {
                    bnkObj.BankDashBoardGrid = GetBankDashBoardGrid(dr, type);
                }
                conn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return bnkObj;

        }

        private List<BankDashBoardGrid> GetBankDashBoardGrid(SqlDataReader dr, string type)
        {
            BankDashBoardGrid bnkObj = new BankDashBoardGrid();
            DataTable dt = new DataTable("gridTable");
            dt.Load(dr);
            decimal I1to10Dr = 0;
            decimal F1to10Dr = 0;
            decimal I1to10Cr = 0;
            decimal F1to10Cr = 0;
            decimal I11to30Dr = 0;
            decimal F11to30Dr = 0;
            decimal I11to30Cr = 0;
            decimal F11to30Cr = 0;
            decimal I31to60Dr = 0;
            decimal F31to60Dr = 0;
            decimal I31to60Cr = 0;
            decimal F31to60Cr = 0;
            decimal Igreaterthan61Dr = 0;
            decimal Fgreaterthan61Dr = 0;
            decimal Igreaterthan61Cr = 0;
            decimal Fgreaterthan61Cr = 0;
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                I1to10Dr += Convert.ToDecimal(dt.Rows[i]["1-10 Dr"].ToString().Split('-').First());
                F1to10Dr += Convert.ToDecimal(dt.Rows[i]["1-10 Dr"].ToString().Split('-').Last());
                I1to10Cr += Convert.ToDecimal(dt.Rows[i]["1-10 Cr"].ToString().Split('-').First());
                F1to10Cr += Convert.ToDecimal(dt.Rows[i]["1-10 Cr"].ToString().Split('-').Last());
                I11to30Dr += Convert.ToDecimal(dt.Rows[i]["11-30 Dr"].ToString().Split('-').First());
                F11to30Dr += Convert.ToDecimal(dt.Rows[i]["11-30 Dr"].ToString().Split('-').Last());
                I11to30Cr += Convert.ToDecimal(dt.Rows[i]["11-30 Cr"].ToString().Split('-').First());
                F11to30Cr += Convert.ToDecimal(dt.Rows[i]["11-30 Cr"].ToString().Split('-').Last());
                I31to60Dr += Convert.ToDecimal(dt.Rows[i]["31-60 Dr"].ToString().Split('-').First());
                F31to60Dr += Convert.ToDecimal(dt.Rows[i]["31-60 Dr"].ToString().Split('-').Last());
                I31to60Cr += Convert.ToDecimal(dt.Rows[i]["31-60 Cr"].ToString().Split('-').First());
                F31to60Cr += Convert.ToDecimal(dt.Rows[i]["31-60 Cr"].ToString().Split('-').Last());
                Igreaterthan61Dr += Convert.ToDecimal(dt.Rows[i][">61 Dr"].ToString().Split('-').First());
                Fgreaterthan61Dr += Convert.ToDecimal(dt.Rows[i][">61 Dr"].ToString().Split('-').Last());
                Igreaterthan61Cr += Convert.ToDecimal(dt.Rows[i][">61 Cr"].ToString().Split('-').First());
                Fgreaterthan61Cr += Convert.ToDecimal(dt.Rows[i][">61 Cr"].ToString().Split('-').Last());
            }
            List<BankDashBoardGrid> lstBank = new List<BankDashBoardGrid>() {
                new BankDashBoardGrid()
                {
                    Name="Deposit(Dr)",
                    BDBG_61_More = "#"+Igreaterthan61Dr + "-" + "$"+Fgreaterthan61Dr.ToString("#,##0"),
                    BDBG_11_30 ="#"+ I11to30Dr + "-" +"$"+ F11to30Dr.ToString("#,##0"),
                    BDBG_1_10= "#"+I1to10Dr + "-" +"$"+ F1to10Dr.ToString("#,##0"),
                    BDBG_31_60 ="#"+ I31to60Dr + "-" +"$"+ F31to60Dr.ToString("#,##0")
                 },
                new BankDashBoardGrid()
                {
                    Name="Payment(Cr)",
                    BDBG_61_More="#"+ Igreaterthan61Cr + "-" +"$"+ Fgreaterthan61Cr.ToString("#,##0"),
                    BDBG_11_30="#"+ I11to30Cr + "-" +"$"+ F11to30Cr.ToString("#,##0"),
                    BDBG_1_10="#"+ I1to10Cr + "-" +"$"+ F1to10Cr.ToString("#,##0"),
                    BDBG_31_60  ="#"+ I31to60Cr + "-" +"$"+ F31to60Cr.ToString("#,##0")
                }
            };
            return lstBank;
        }

        private BasicColumnDrillDownVM GetChangeMonthlyLineChart(SqlDataReader dr)
        {

            BasicColumnDrillDownVM drildownChart = new BasicColumnDrillDownVM();
            List<DrillDownMainData> mainDataList = new List<DrillDownMainData>();
            List<DrillDownSubData> subDataList = new List<DrillDownSubData>();
            if (dr.HasRows)
            {
                dt = new DataTable();
                dt.Load(dr);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string name = dt.Columns[i].ColumnName;
                    switch (name)
                    {

                        case "Inflow":
                            DrillDownMainData mainData = new DrillDownMainData();
                            List<DrillDownSeriesData> seriesDataList = new List<DrillDownSeriesData>();
                            mainData.name = name;
                            var bankDataList = (from row in dt.AsEnumerable()
                                                group row by row.Field<string>(4) into g
                                                select new BankDetailDashBoardModel
                                                {
                                                    MonthYear = g.Key,
                                                    Inflow = Math.Round(g.Sum(r => Convert.ToDouble(r[1] == DBNull.Value ? 0 : r[1])), 2),
                                                    Outflow = Math.Round(g.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2),
                                                    Differance = Math.Round(g.Sum(r => Convert.ToDouble(r[3] == DBNull.Value ? 0 : r[3])), 2),
                                                    Name = g.Select(r => r.Field<string>(0)).FirstOrDefault()
                                                }).Distinct().ToList();

                            DrillDownSeriesData seriesData = null;
                            foreach (var bnkdata in bankDataList)
                            {
                                if (mainData.name == name)
                                {
                                    seriesData = new DrillDownSeriesData()
                                    {
                                        name = bnkdata.MonthYear,
                                        y = bnkdata.Inflow,
                                        drilldown = bnkdata.MonthYear + "-" + mainData.name
                                    };
                                    seriesDataList.Add(seriesData);
                                    DrillDownSubData subdata = new DrillDownSubData()
                                    {
                                        type = Constants.Constants_Column,
                                        name = mainData.name,
                                        id = seriesData.drilldown
                                    };
                                    var rows = dt.AsEnumerable()
                                              .Where(s => s.Field<string>(4) + "-" + mainData.name == seriesData.drilldown)
                                              .GroupBy(s => s.Field<string>(0))
                                              .Select(b => new
                                              {
                                                  SubName = b.Key,
                                                  SubValue = Math.Round(b.Sum(r => Convert.ToDouble(r[1] == DBNull.Value ? 0 : r[1])), 2)
                                              }).ToList();

                                    foreach (var n in rows)
                                    {
                                        subdata.data.Add(new List<string> { n.SubName, n.SubValue.ToString() });
                                    }
                                    subDataList.Add(subdata);
                                }
                            }
                            mainData.data = seriesDataList;
                            mainDataList.Add(mainData);

                            break;
                        case "Outflow":
                            DrillDownMainData mainData1 = new DrillDownMainData();
                            List<DrillDownSeriesData> seriesDataList1 = new List<DrillDownSeriesData>();
                            mainData1.name = name;
                            var bankDataList1 = (from row in dt.AsEnumerable()
                                                 group row by row.Field<string>(4) into g
                                                 select new BankDetailDashBoardModel
                                                 {

                                                     MonthYear = g.Key,
                                                     Inflow = Math.Round(g.Sum(r => Convert.ToDouble(r[1] == DBNull.Value ? 0 : r[1])), 2),
                                                     Outflow = Math.Round(g.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2),
                                                     Differance = Math.Round(g.Sum(r => Convert.ToDouble(r[3] == DBNull.Value ? 0 : r[3])), 2),
                                                     Name = g.Select(r => r.Field<string>(0)).FirstOrDefault()
                                                 }).Distinct().ToList();

                            DrillDownSeriesData seriesData1 = null;
                            foreach (var bnkdata1 in bankDataList1)
                            {
                                if (mainData1.name == name)
                                {
                                    seriesData = new DrillDownSeriesData()
                                    {
                                        name = bnkdata1.MonthYear,
                                        y = bnkdata1.Outflow,
                                        drilldown = bnkdata1.MonthYear + "-" + mainData1.name
                                    };
                                    seriesDataList1.Add(seriesData);
                                    DrillDownSubData subdata = new DrillDownSubData()
                                    {
                                        type = Constants.Constants_Column,
                                        name = mainData1.name,
                                        id = seriesData.drilldown
                                    };
                                    var rows = dt.AsEnumerable()
                                              .Where(s => s.Field<string>(4) + "-" + mainData1.name == seriesData.drilldown)
                                              .GroupBy(s => s.Field<string>(0))
                                              .Select(b => new
                                              {
                                                  SubName = b.Key,
                                                  SubValue = Math.Round(b.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2)
                                              }).ToList();

                                    foreach (var n in rows)
                                    {
                                        subdata.data.Add(new List<string> { n.SubName, n.SubValue.ToString() });
                                    }
                                    subDataList.Add(subdata);
                                }
                            }
                            mainData1.data = seriesDataList1;
                            mainDataList.Add(mainData1);

                            break;
                        case "Differance":
                            DrillDownMainData mainData2 = new DrillDownMainData();
                            List<DrillDownSeriesData> seriesDataList2 = new List<DrillDownSeriesData>();
                            mainData2.name = name;
                            var bankDataList2 = (from row in dt.AsEnumerable()
                                                 group row by row.Field<string>(4) into g
                                                 select new BankDetailDashBoardModel
                                                 {

                                                     MonthYear = g.Key,
                                                     Inflow = Math.Round(g.Sum(r => Convert.ToDouble(r[1] == DBNull.Value ? 0 : r[1])), 2),
                                                     Outflow = Math.Round(g.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2),
                                                     Differance = Math.Round(g.Sum(r => Convert.ToDouble(r[3] == DBNull.Value ? 0 : r[3])), 2),
                                                     Name = g.Select(r => r.Field<string>(0)).FirstOrDefault()
                                                 }).Distinct().ToList();

                            DrillDownSeriesData seriesData2 = null;
                            foreach (var bnkdata2 in bankDataList2)
                            {
                                if (mainData2.name == name)
                                {
                                    seriesData = new DrillDownSeriesData()
                                    {
                                        name = bnkdata2.MonthYear,
                                        y = bnkdata2.Differance,
                                        drilldown = bnkdata2.MonthYear + "-" + mainData2.name
                                    };
                                    seriesDataList2.Add(seriesData);
                                    DrillDownSubData subdata = new DrillDownSubData()
                                    {
                                        type = Constants.Constants_Column,
                                        name = mainData2.name,
                                        id = seriesData.drilldown
                                    };
                                    var rows = dt.AsEnumerable()
                                              .Where(s => s.Field<string>(4) + "-" + mainData2.name == seriesData.drilldown)
                                              .GroupBy(s => s.Field<string>(0))
                                              .Select(b => new
                                              {
                                                  SubName = b.Key,
                                                  SubValue = Math.Round(b.Sum(r => Convert.ToDouble(r[3] == DBNull.Value ? 0 : r[3])), 2)
                                              }).ToList();

                                    foreach (var n in rows)
                                    {
                                        subdata.data.Add(new List<string> { n.SubName, n.SubValue.ToString() });
                                    }
                                    subDataList.Add(subdata);
                                }
                            }
                            mainData2.data = seriesDataList2;
                            mainDataList.Add(mainData2);

                            break;
                    }
                    drildownChart.MainData = mainDataList;
                    drildownChart.SubData = subDataList;
                }
            }
            else
            {
                DrillDownMainData mainData = new DrillDownMainData()
                {
                    name = "No Data",
                    data = new List<DrillDownSeriesData>()
                                                 {
                                                   new DrillDownSeriesData {name="No Data"}
                                                 }.ToList()
                };
                mainDataList.Add(mainData);
                drildownChart.MainData = mainDataList;
            }
            return drildownChart;
        }

        private DrillDownChartVM GetBankDetailsDrillDownChart(SqlDataReader dr)
        {
            DrillDownChartVM drildownChart = new DrillDownChartVM();
            List<DrillDownMainData1> maindataList = new List<DrillDownMainData1>();
            List<DrillDownSubData1> subdataList = new List<DrillDownSubData1>();
            if (dr.HasRows)
            {
                dt = new DataTable();
                dt.Load(dr);
                IEnumerable<BankDetailData> bankDetailDataList = (from b in dt.AsEnumerable()
                                                                  group b by b.Field<string>(2) into g
                                                                  select new BankDetailData
                                                                  {
                                                                      MonthYear = g.Key,
                                                                      Amount = Math.Round(g.Sum(r => Convert.ToDouble(r[1] == DBNull.Value ? 0 : r[1])), 2),
                                                                      Name = g.Select(r => r.Field<string>(0)).FirstOrDefault()
                                                                  }).ToList();

                foreach (var bnk in bankDetailDataList)
                {
                    DrillDownMainData1 maindata = new DrillDownMainData1()
                    {
                        name = bnk.MonthYear,
                        y = bnk.Amount,
                        drilldown = bnk.MonthYear
                    };
                    maindataList.Add(maindata);
                    DrillDownSubData1 subdata = new DrillDownSubData1()
                    {
                        name = maindata.name,
                        id = maindata.drilldown
                    };
                    var rows = dt.AsEnumerable().Where(s => s.Field<string>(2) == maindata.drilldown).
                        GroupBy(s => s.Field<string>(0))
                       .Select(b => new { SubName = b.Key, SubValue = Math.Round(b.Sum(r => Convert.ToDouble(r[1] == DBNull.Value ? 0 : r[1])), 2) }).ToList();

                    foreach (var n in rows)
                    {
                        subdata.data.Add(new List<string> { n.SubName, n.SubValue.ToString() });
                    }
                    subdataList.Add(subdata);
                }
                drildownChart.MainData = maindataList;
                drildownChart.SubData = subdataList;
            }
            else
            {
                DrillDownMainData1 maindata = new DrillDownMainData1() { name = "No Data", y = 0 };
                maindataList.Add(maindata);
                drildownChart.MainData = maindataList;
            }
            return drildownChart;
        }
        #endregion

        #region CustomerBalances
        public CustomerBalanceVM CustomerBalances(DateTime? fromdate, string todate, long cid, string role, string type)
        {
            CustomerBalanceVM custBalance = new CustomerBalanceVM();
            try
            {
                conn.Open();
                if (fromdate == null || todate == null)
                {
                    custBalance.ToDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    DateTime prevyear = Convert.ToDateTime(custBalance.ToDate).AddYears(-1);
                    custBalance.FromDate = new DateTime(prevyear.Year, prevyear.Month, 1);
                }
                else
                {
                    custBalance.FromDate = (DateTime)fromdate;
                    custBalance.ToDate = (string)todate + "T" + "23:59:59";
                }
                if (type == "null")
                    cmd = new SqlCommand(Constants.Constants_CustomerBalances, conn);
                else if (type == "Customer  Balance")
                    cmd = new SqlCommand(Constants.Constants_CustomerBalancesDetails, conn);
                else
                    cmd = new SqlCommand(Constants.Constants_Top30CustandVendorDetails, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(Constants.Constants_FromDate, custBalance.FromDate);
                cmd.Parameters.AddWithValue(Constants.Constants_ToDate, custBalance.ToDate);
                cmd.Parameters.AddWithValue(Constants.Constants_Cid, cid);
                cmd.Parameters.AddWithValue(Constants.Constants_Role, role == "null" ? (object)DBNull.Value : role);
                if (type != "null")
                {
                    cmd.Parameters.AddWithValue(Constants.Constants_Type, type);
                }
                dr = cmd.ExecuteReader();
                if (type == "null" || type == "Customer  Balance")
                {
                    custBalance.CustomerBalanceDetails = GetCustomerBalanceDetails(dr);
                }
                if (type == "null" || type == "Top 30 Customers" || type == "Top 30 Vendors")
                {
                    custBalance.Top30CustomerOrVendorDetails = GetTop30CustomerDetails(dr, type);
                }

                conn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return custBalance;
        }

        private List<Top30CustomersOrVendors> GetTop30CustomerDetails(SqlDataReader dr, string type)
        {
            List<Top30CustomersOrVendors> lstcustDetails = new List<Top30CustomersOrVendors>();
            DataTable dt = new DataTable("customerTable");
            dt.Load(dr);
            return lstcustDetails = (from DataRow row in dt.Rows
                                     select new Top30CustomersOrVendors()
                                     {
                                         CustomerOrVendor = row[0].ToString(),
                                         Billing = Convert.ToDouble(row["Billing"]),
                                         Balance = row.IsNull("Balance") ? 0 : Convert.ToDouble(row["Balance"]),
                                         Current = row["Current"].ToString(),
                                         OverDue1to30 = row.IsNull("OverDue 1-30") ? 0 : Convert.ToDouble(row["OverDue 1-30"]),
                                         OverDue31to60 = row.IsNull("OverDue 31-60") ? 0 : Convert.ToDouble(row["OverDue 31-60"]),
                                         OverDue61more = row.IsNull("OverDue >61") ? 0 : Convert.ToDouble(row["OverDue >61"]),
                                     }).Take(30).ToList();
        }

        private DrillDownChartVM GetCustomerBalanceDetails(SqlDataReader dr)
        {
            List<CustomerBalanceData> cbList = new List<CustomerBalanceData>();
            DrillDownChartVM drildownChart = new DrillDownChartVM();
            List<DrillDownMainData1> mainDataList = new List<DrillDownMainData1>();
            List<DrillDownSubData1> SubData = new List<DrillDownSubData1>();
            if (dr.HasRows)
            {
                dt = new DataTable();
                dt.Load(dr);
                IEnumerable<CustomerBalanceData> customerBalanceDataList = (from b in dt.AsEnumerable()
                                                                            group b by b.Field<string>(0) into g
                                                                            select new CustomerBalanceData
                                                                            {
                                                                                MonthYear = g.Key,
                                                                                Aging = g.Select(r => r.Field<string>(1)).ToString(),
                                                                                TotalBalance = Math.Round(g.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2),

                                                                            }).ToList();

                foreach (var cbData in customerBalanceDataList)
                {
                    DrillDownMainData1 mainData = new DrillDownMainData1()
                    {
                        name = cbData.MonthYear,
                        y = cbData.TotalBalance,
                        drilldown = cbData.MonthYear
                    };
                    mainDataList.Add(mainData);
                    DrillDownSubData1 subData = new DrillDownSubData1()
                    {
                        name = mainData.name,
                        id = mainData.drilldown
                    };
                    var rows = dt.AsEnumerable()
                        .Where(s => s.Field<string>(0) == mainData.drilldown)
                        .GroupBy(s => s.Field<string>(1))
                        .Select(b => new { SubName = b.Key, SubValue = Math.Round(b.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2), Amount = Math.Round(b.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2) }).ToList();

                    foreach (var n in rows)
                    {
                        subData.data.Add(new List<string> { n.SubName, n.SubValue.ToString() });
                    }
                    SubData.Add(subData);
                }
                drildownChart.MainData = mainDataList;
                drildownChart.SubData = SubData;
            }
            else
            {
                DrillDownMainData1 maindata = new DrillDownMainData1() { name = "No Data", y = 0 };
                mainDataList.Add(maindata);
                drildownChart.MainData = mainDataList;
            }
            return drildownChart;
        }
        #endregion

        #region BankBalanceAdminDashBoard
        public BankBalanceAdminVM GetBankBalanceAdminDashBoard(DateTime? fromdate, string todate, long cid)
        {
            BankBalanceAdminVM bankbal = new BankBalanceAdminVM();
            try
            {
                conn.Open();
                if (fromdate == null || todate == null)
                {
                    bankbal.toDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    DateTime prevyear = Convert.ToDateTime(bankbal.toDate).AddYears(-1);
                    bankbal.fromDate = new DateTime(prevyear.Year, prevyear.Month, 1);
                }
                else
                {
                    bankbal.toDate = (string)todate + "T" + "23:59:59";
                    bankbal.fromDate = (DateTime)fromdate;
                }
                cmd = new SqlCommand(Constants.Constants_GetBankBalanceAdminDashBoard, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(Constants.Constants_FromDate, bankbal.fromDate);
                cmd.Parameters.AddWithValue(Constants.Constants_ToDate, bankbal.toDate);
                cmd.Parameters.AddWithValue(Constants.Constants_Cid, cid);
                cmd.Parameters.AddWithValue(Constants.Constants_Role, "null");
                dr = cmd.ExecuteReader();
                bankbal.BankBalanceKPI = GetBankBalnceKPI(dr);
                bankbal.BankBalanceDrillDownChart = GetBankBalanceDrillDownChart(dr);
                bankbal.BankTotalInandOutBasicColumnDrillDown = GetBankBalanceBasicColumnDrillDown(dr);
                conn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return bankbal;
        }
        private BankBalanceKPI GetBankBalnceKPI(SqlDataReader dr)
        {
            BankBalanceKPI kpi = new BankBalanceKPI();
            dt = new DataTable();
            dt.Load(dr);
            if (dt.Rows.Count > 0)
            {
                kpi.In = Math.Round(dt.AsEnumerable().Select(x => x.Field<decimal>(0)).FirstOrDefault(), 2);
                kpi.Out = Math.Round(dt.AsEnumerable().Select(x => x.Field<decimal>(1)).FirstOrDefault(), 2);
                kpi.Balance = Math.Round(dt.AsEnumerable().Select(x => x.Field<decimal>(2)).FirstOrDefault(), 2);
            }
            else
            {
                kpi.In = 0;
                kpi.Out = 0;
                kpi.Balance = 0;
            }
            return kpi;
        }
        public DrillDownChartVM GetBankBalanceDrillDownChart(SqlDataReader dr)
        {
            DrillDownChartVM drildownChart = new DrillDownChartVM();
            List<DrillDownMainData1> maindataList = new List<DrillDownMainData1>();
            List<DrillDownSubData1> subdataList = new List<DrillDownSubData1>();
            dt = new DataTable();
            dt.Load(dr);
            if (dt.Rows.Count > 0)
            {
                IEnumerable<BankBalanceData> bankBalanceDataList = (from b in dt.AsEnumerable()
                                                                    group b by b.Field<string>(1) into g
                                                                    select new BankBalanceData
                                                                    {
                                                                        Monthyear = g.Key,
                                                                        Balance = Math.Round(g.Sum(r => Convert.ToDouble(r[0] == DBNull.Value ? 0 : r[0])), 2),
                                                                        Date = g.Select(r => r.Field<DateTime>(2)).FirstOrDefault()
                                                                    }).ToList();
                foreach (var bnk in bankBalanceDataList)
                {
                    DrillDownMainData1 maindata = new DrillDownMainData1()
                    {
                        name = bnk.Monthyear,
                        y = bnk.Balance,
                        drilldown = bnk.Monthyear
                    };
                    maindataList.Add(maindata);
                    DrillDownSubData1 subdata = new DrillDownSubData1()
                    {
                        name = maindata.name,
                        id = maindata.drilldown,
                        type = "line"
                    };
                    var rows = dt.AsEnumerable().Where(s => s.Field<string>(1) == maindata.drilldown).GroupBy(s => s.Field<DateTime>(2).ToString()).Select(x => new { SubName = x.Key, SubValue = Math.Round(x.Sum(r => Convert.ToDouble(r[0] == DBNull.Value ? 0 : r[0])), 2) }).ToList();
                    foreach (var n in rows)
                    {
                        subdata.data.Add(new List<string> { n.SubName, n.SubValue.ToString() });
                    }
                    subdataList.Add(subdata);
                }
                drildownChart.MainData = maindataList;
                drildownChart.SubData = subdataList;
            }
            else
            {
                DrillDownMainData1 maindata = new DrillDownMainData1()
                {
                    name = "No Data"
                };
                maindataList.Add(maindata);
                drildownChart.MainData = maindataList;
            }
            return drildownChart;
        }
        public BasicColumnDrillDownVM GetBankBalanceBasicColumnDrillDown(SqlDataReader dr)
        {
            BasicColumnDrillDownVM basicColumnDrillDown = new BasicColumnDrillDownVM();
            List<DrillDownMainData> mainDataList = new List<DrillDownMainData>();
            List<DrillDownSubData> subDataList = new List<DrillDownSubData>();
            DataTable dt = new DataTable();
            dt.Load(dr);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string name = dt.Columns[i].ColumnName;
                    if (name == "IN" || name == "Out")
                    {
                        DrillDownMainData mainData = new DrillDownMainData();
                        List<DrillDownSeriesData> seriesDataList = new List<DrillDownSeriesData>();
                        mainData.name = name;
                        mainData.color = (mainData.name == "IN") ? "#26aae1" : "#c9cac8";
                        var BasicColumnDataList = (from row in dt.AsEnumerable()
                                                   group row by row.Field<string>(3) into g
                                                   select new BankBalanceData
                                                   {
                                                       Monthyear = g.Key,
                                                       Balance = Math.Round(g.Sum(r => Convert.ToDouble(r[i] == DBNull.Value ? 0 : r[i])), 2),
                                                       Date = g.Select(r => r.Field<DateTime>(4)).FirstOrDefault(),
                                                       IN = Math.Round(g.Sum(r => Convert.ToDouble(r[0] == DBNull.Value ? 0 : r[0])), 2),
                                                       Out = Math.Round(g.Sum(r => Convert.ToDouble(r[1] == DBNull.Value ? 0 : r[1])), 2),
                                                       Diff = Math.Round(g.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2)
                                                   }).Distinct().ToList();
                        DrillDownSeriesData seriesData = null;
                        foreach (var bnkdata in BasicColumnDataList)
                        {
                            if (mainData.name == name)
                            {
                                seriesData = new DrillDownSeriesData()
                                {
                                    name = bnkdata.Monthyear,
                                    y = bnkdata.Balance,
                                    drilldown = bnkdata.Monthyear + "-" + mainData.name,
                                    IN = bnkdata.IN,
                                    Out = bnkdata.Out,
                                    Diff = bnkdata.Diff
                                };
                                seriesDataList.Add(seriesData);
                                DrillDownSubData subdata = new DrillDownSubData()
                                {
                                    name = mainData.name,
                                    id = seriesData.drilldown,
                                    type = "line"
                                };
                                var rows = dt.AsEnumerable()
                                          .Where(s => s.Field<string>(3) + "-" + mainData.name == seriesData.drilldown)
                                          .GroupBy(s => s.Field<DateTime>(4).ToString("dd-MM-yyyy"))
                                          .Select(b => new
                                          {
                                              SubName = b.Key,
                                              SubValue = Math.Round(b.Sum(r => Convert.ToDouble(r[i] == DBNull.Value ? 0 : r[i])), 2),
                                              IN = Math.Round(b.Sum(r => Convert.ToDouble(r[0] == DBNull.Value ? 0 : r[0])), 2),
                                              Out = Math.Round(b.Sum(r => Convert.ToDouble(r[1] == DBNull.Value ? 0 : r[1])), 2),
                                              Diff = Math.Round(b.Sum(r => Convert.ToDouble(r[2] == DBNull.Value ? 0 : r[2])), 2),
                                          }).ToList();
                                foreach (var n in rows)
                                {
                                    subdata.data.Add(new List<string> { n.SubName, n.SubValue.ToString(), n.IN.ToString(), n.Out.ToString(), n.Diff.ToString() });
                                }
                                subDataList.Add(subdata);
                            }
                        }
                        mainData.data = seriesDataList;
                        mainDataList.Add(mainData);
                    }
                    basicColumnDrillDown.MainData = mainDataList;
                    basicColumnDrillDown.SubData = subDataList;
                }
            }
            else
            {
                DrillDownMainData maindata = new DrillDownMainData()
                {
                    name = "No Data"
                };
                mainDataList.Add(maindata);
                basicColumnDrillDown.MainData = mainDataList;
            }
            return basicColumnDrillDown;
        }
        #endregion

        //#region ProfitandLossDashBoard
        //public ProfitandLossVM GetProfitandLossDashBoard(DateTime? fromdate, string todate, int cid)
        //{
        //    ProfitandLossVM ProfitandLoss = new ProfitandLossVM();
        //    try
        //    {
        //        conn.Open();
        //        if (fromdate == null && todate == "null")
        //        {
        //            ProfitandLoss.toDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        //            DateTime prevyear = Convert.ToDateTime(ProfitandLoss.toDate).AddYears(-1);
        //            ProfitandLoss.fromDate = new DateTime(prevyear.Year, prevyear.Month, 1);
        //        }
        //        else
        //        {
        //            ProfitandLoss.fromDate = (DateTime)fromdate;
        //            ProfitandLoss.toDate = (string)todate;
        //        }
        //        cmd = new SqlCommand(Constants.Constants_GetProfitandLossDashBoard, conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue(Constants.Constants_FromDate, ProfitandLoss.fromDate);
        //        cmd.Parameters.AddWithValue(Constants.Constants_ToDate, ProfitandLoss.toDate);
        //        cmd.Parameters.AddWithValue(Constants.Constants_Cid, cid);
        //        dr = cmd.ExecuteReader();
        //        ProfitandLoss.BasicDrillDown = MultiDrillDown(dr);
        //        conn.Close();
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //    return ProfitandLoss;
        //}
        //private MultiDrillDownVM MultiDrillDown(SqlDataReader dr)
        //{
        //    MultiDrillDownVM basicdrillDown = new MultiDrillDownVM();
        //    List<MultiDrillDownMainData> MainDatalist = new List<MultiDrillDownMainData>();
        //    List<MultiDrillDownSubData> SubDatalist = new List<MultiDrillDownSubData>();
        //    dt = new DataTable();
        //    dt.Load(dr);
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            string name = dt.Columns[i].ColumnName;
        //            if (i == 0 || i == 1 || i == 2)
        //            {
        //                MultiDrillDownMainData mainData = new MultiDrillDownMainData();
        //                MultiDrillDownSubData subData = new MultiDrillDownSubData();
        //                //BasicDrillDownSubData subData1 = new BasicDrillDownSubData();
        //                List<MultiDrillDownSeriesData> SeriesDatalist = new List<MultiDrillDownSeriesData>();
        //                mainData.name = name;
        //                mainData.color = (name == "Income") ? "Yellow" : (name == "Expenses") ? "Green" : "Pink";
        //                mainData.y = dt.AsEnumerable().Sum(a => Convert.ToDecimal(a.Field<decimal>(i)));
        //                mainData.drilldown = name;
        //                MainDatalist.Add(mainData);
        //                IEnumerable<ProfitAndLossData> basicdata1 = (from x in dt.AsEnumerable()
        //                                                             group x by x.Field<string>(3) into g
        //                                                             select new ProfitAndLossData
        //                                                             {
        //                                                                 MonthYear = g.Key,
        //                                                                 Amount = g.Sum(a => Convert.ToDecimal(a.Field<decimal>(i))),
        //                                                                 Date = g.Select(a => a.Field<DateTime>(4).ToString()).FirstOrDefault(),
        //                                                                 Amount1 = g.Select(a => Convert.ToDecimal(a.Field<decimal>(i))).FirstOrDefault()
        //                                                             }).ToList();
        //                MultiDrillDownSubData subData1;
        //                foreach (var result in basicdata1)
        //                {
        //                    if (mainData.name == name)
        //                    {
        //                        subData.name = mainData.name;
        //                        subData.id = mainData.name;
        //                        MultiDrillDownSeriesData seriesdata = new MultiDrillDownSeriesData()
        //                        {
        //                            name = result.MonthYear,
        //                            y = result.Amount,
        //                            drilldown = result.MonthYear + ":" + mainData.name
        //                        };
        //                        SeriesDatalist.Add(seriesdata);
        //                        subData1 = new MultiDrillDownSubData();
        //                        subData1.name = seriesdata.name + ":" + mainData.name;
        //                        subData1.id = seriesdata.name + ":" + mainData.name;
        //                        subData1.type = "line";
        //                        IEnumerable<ProfitAndLossData> seriesdata1 = (from x in dt.AsEnumerable()
        //                                                                      where x.Field<string>(3) + ":" + mainData.name == seriesdata.drilldown
        //                                                                      group x by new { MonthYear = x.Field<string>(3), Date = x.Field<DateTime>(4) } into g
        //                                                                      select new ProfitAndLossData
        //                                                                      {
        //                                                                          Date = g.Key.Date.ToString(),
        //                                                                          Amount = g.Select(a => Convert.ToDecimal(a.Field<decimal>(i))).FirstOrDefault(),
        //                                                                          MonthYear = g.Key.MonthYear,
        //                                                                      }).ToList();
        //                        List<MultiDrillDownSeriesData> s2 = new List<MultiDrillDownSeriesData>();
        //                        foreach (var item in seriesdata1)
        //                        {
        //                            if (seriesdata.drilldown == item.MonthYear + ":" + mainData.name)
        //                            {
        //                                MultiDrillDownSeriesData seriesubdata = new MultiDrillDownSeriesData()
        //                                {
        //                                    name = item.Date,
        //                                    y = item.Amount
        //                                };
        //                                s2.Add(seriesubdata);
        //                                subData1.data = s2;
        //                            }
        //                        }
        //                        SubDatalist.Add(subData1);
        //                    }
        //                }
        //                subData.data = SeriesDatalist;
        //                SubDatalist.Add(subData);
        //            }
        //            basicdrillDown.mainData = MainDatalist;
        //            basicdrillDown.subData = SubDatalist;
        //        }
        //    }
        //    return basicdrillDown;
        //}
        //#endregion

        #region FinancialsAdminDashBoard
        public FinancialsAdminDashBoardVM GetFinancialsAdminDashBoard(int cid, DateTime? fromdate, string todate)
        {
            FinancialsAdminDashBoardVM financialsAdminVM = new FinancialsAdminDashBoardVM();
            try
            {
                conn.Open();
                if (fromdate == null || todate == null)
                {
                    financialsAdminVM.ToDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    DateTime prevyear = Convert.ToDateTime(financialsAdminVM.ToDate).AddYears(-1);
                    financialsAdminVM.FromDate = new DateTime(prevyear.Year, prevyear.Month, 1);
                }
                else
                {
                    financialsAdminVM.ToDate = (string)todate + "T" + "23:59:59";
                    financialsAdminVM.FromDate = (DateTime)fromdate;
                }
                cmd = new SqlCommand(Constants.Constants_FinancialsAdminDashBoard, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(Constants.Constants_FromDate, financialsAdminVM.FromDate);
                cmd.Parameters.AddWithValue(Constants.Constants_ToDate, financialsAdminVM.ToDate);
                cmd.Parameters.AddWithValue(Constants.Constants_Cid, cid);
                cmd.Parameters.AddWithValue(Constants.Constants_Type, "null");
                dr = cmd.ExecuteReader();
                financialsAdminVM.FinancialsAdminKPI = GetFinancialsAdminKPI(dr);
                financialsAdminVM.FinancialsAdminBlanceSheet = GetFinancialsAdmin3LevelChart1(dr);
                financialsAdminVM.FinancialsAdminProfitandLoss = GetFinancialsAdmin3LevelChart2(dr);
                conn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return financialsAdminVM;
        }
        private FinancialsAdminKPI GetFinancialsAdminKPI(SqlDataReader dr)
        {
            FinancialsAdminKPI financialsAdminKpi = new FinancialsAdminKPI();
            dt = new DataTable();
            dt.Load(dr);
            if (dt.Rows.Count > 0)
            {
                financialsAdminKpi.Income = dt.AsEnumerable().Select(x => x.Field<decimal>(0)).FirstOrDefault();
                financialsAdminKpi.Expenses = dt.AsEnumerable().Select(x => x.Field<decimal>(1)).FirstOrDefault();
                financialsAdminKpi.NetProfit = dt.AsEnumerable().Select(x => x.Field<decimal>(2)).FirstOrDefault();
            }
            else
            {
                financialsAdminKpi.Income = 0;
                financialsAdminKpi.Expenses = 0;
                financialsAdminKpi.NetProfit = 0;
            }
            return financialsAdminKpi;
        }
        public MultiDrillDownVM GetFinancialsAdmin3LevelChart1(SqlDataReader dr)
        {
            MultiDrillDownVM basicDrillDown = new MultiDrillDownVM();
            List<MultiDrillDownMainData> MainDataList = new List<MultiDrillDownMainData>();
            List<MultiDrillDownSubData> SubDataList = new List<MultiDrillDownSubData>();
            DataTable dt = new DataTable();
            dt.Load(dr);
            if (dt.Rows.Count > 0)
            {
                IEnumerable<FinancialsAdminData> financialsAdminDataList = (from x in dt.AsEnumerable()
                                                                            group x by x.Field<string>(1) into g
                                                                            select new FinancialsAdminData
                                                                            {
                                                                                Subcategory = g.Key,
                                                                                Value = Math.Round(g.Sum(a => a.Field<decimal>(2)), 2)
                                                                            }).ToList();
                foreach (var financialsAdminData in financialsAdminDataList)
                {
                    MultiDrillDownMainData maindata = new MultiDrillDownMainData();
                    MultiDrillDownSubData subdata = new MultiDrillDownSubData();
                    List<MultiDrillDownSeriesData> seriesList = new List<MultiDrillDownSeriesData>();
                    maindata.name = financialsAdminData.Subcategory;
                    maindata.y = financialsAdminData.Value;
                    maindata.drilldown = financialsAdminData.Subcategory;
                    MainDataList.Add(maindata);
                    IEnumerable<FinancialsAdminData> financialsAdminList = (from x in dt.AsEnumerable()
                                                                            where x.Field<string>(1) == maindata.drilldown
                                                                            group x by new { Monthyear = x.Field<string>(5), Subcategory = x.Field<string>(1) } into g
                                                                            select new FinancialsAdminData
                                                                            {
                                                                                MonthYear = g.Key.Monthyear,
                                                                                Value = Math.Round(g.Sum(a => Convert.ToDecimal(a.Field<decimal>(2))), 2)
                                                                            }).ToList();
                    MultiDrillDownSubData subData1;
                    foreach (var financialsAdmin in financialsAdminList)
                    {
                        if (maindata.name == financialsAdminData.Subcategory)
                        {
                            //2nd level
                            subdata.name = maindata.name;
                            subdata.id = maindata.name;
                            MultiDrillDownSeriesData seriesdata = new MultiDrillDownSeriesData()
                            {
                                name = financialsAdmin.MonthYear,
                                y = financialsAdmin.Value,
                                drilldown = financialsAdmin.MonthYear + ":" + maindata.name
                            };
                            seriesList.Add(seriesdata);
                            //3rd Level
                            subData1 = new MultiDrillDownSubData();
                            subData1.name = seriesdata.name + ":" + maindata.name;
                            subData1.id = seriesdata.name + ":" + maindata.name;
                            subData1.type = "line";
                            IEnumerable<FinancialsAdminData> financialsDataList = (from x in dt.AsEnumerable()
                                                                                   where x.Field<string>(5) + ":" + x.Field<string>(1) == seriesdata.drilldown
                                                                                   group x by new { Createddate = x.Field<DateTime>(6), MonthYear = x.Field<string>(5), SubCategory = x.Field<string>(1) } into g
                                                                                   select new FinancialsAdminData
                                                                                   {
                                                                                       CreatedDate = g.Key.Createddate,
                                                                                       Value = Math.Round(g.Select(a => Convert.ToDecimal(a.Field<decimal>(2))).FirstOrDefault(), 2),
                                                                                       MonthYear = g.Select(a => a.Field<string>(5)).FirstOrDefault()
                                                                                   }).ToList();
                            List<MultiDrillDownSeriesData> SeriesDataList = new List<MultiDrillDownSeriesData>();
                            foreach (var financialsData in financialsDataList)
                            {
                                if (seriesdata.drilldown == financialsData.MonthYear + ":" + maindata.name)
                                {
                                    MultiDrillDownSeriesData seriesubdata = new MultiDrillDownSeriesData()
                                    {
                                        name = financialsData.CreatedDate.ToString("dd-MM-yyyy"),
                                        y = financialsData.Value
                                    };
                                    SeriesDataList.Add(seriesubdata);
                                    subData1.data = SeriesDataList;
                                }
                            }
                            SubDataList.Add(subData1);//3rd level
                        }
                    }
                    subdata.data = seriesList;
                    SubDataList.Add(subdata);//2nd level
                }
                basicDrillDown.MainData = MainDataList;
                basicDrillDown.SubData = SubDataList;
            }
            else
            {
                MultiDrillDownMainData mainData = new MultiDrillDownMainData()
                {
                    name = "No Data"
                };
                MainDataList.Add(mainData);
                basicDrillDown.MainData = MainDataList;
            }
            return basicDrillDown;
        }

        public MultiDrillDownVM GetFinancialsAdmin3LevelChart2(SqlDataReader dr)
        {
            MultiDrillDownVM basicDrillDown = new MultiDrillDownVM();
            List<MultiDrillDownMainData> MainDataList = new List<MultiDrillDownMainData>();
            List<MultiDrillDownSubData> SubDataList = new List<MultiDrillDownSubData>();
            DataTable dt = new DataTable();
            dt.Load(dr);
            if (dt.Rows.Count > 0)
            {
                IEnumerable<FinancialsData> financialDataList = (from x in dt.AsEnumerable()
                                                                 group x by x.Field<string>(1) into g
                                                                 select new FinancialsData
                                                                 {
                                                                     Class = g.Key,
                                                                     Value = Math.Round(g.Sum(a => a.Field<decimal>(2)), 2)
                                                                 }).ToList();
                foreach (var financialData in financialDataList)
                {
                    MultiDrillDownMainData maindata = new MultiDrillDownMainData();
                    MultiDrillDownSubData subdata = new MultiDrillDownSubData();
                    List<MultiDrillDownSeriesData> seriesList = new List<MultiDrillDownSeriesData>();
                    maindata.name = financialData.Class;
                    maindata.y = financialData.Value;
                    maindata.drilldown = financialData.Class;
                    MainDataList.Add(maindata);
                    IEnumerable<FinancialsData> financialsList = (from x in dt.AsEnumerable()
                                                                  where x.Field<string>(1) == maindata.drilldown
                                                                  group x by new { Monthyear = x.Field<string>(5), Class = x.Field<string>(1) } into g
                                                                  select new FinancialsData
                                                                  {
                                                                      Monthyear = g.Key.Monthyear,
                                                                      Value = Math.Round(g.Sum(a => Convert.ToDecimal(a.Field<decimal>(2))), 2)
                                                                  }).ToList();
                    MultiDrillDownSubData subData1;
                    foreach (var financials in financialsList)
                    {
                        if (maindata.name == financialData.Class)
                        {
                            //2nd level
                            subdata.name = maindata.name;
                            subdata.id = maindata.name;
                            MultiDrillDownSeriesData seriesdata = new MultiDrillDownSeriesData()
                            {
                                name = financials.Monthyear,
                                y = financials.Value,
                                drilldown = financials.Monthyear + ":" + maindata.name
                            };
                            seriesList.Add(seriesdata);
                            //3rd Level
                            subData1 = new MultiDrillDownSubData();
                            subData1.name = seriesdata.name + ":" + maindata.name;
                            subData1.id = seriesdata.name + ":" + maindata.name;
                            subData1.type = "line";
                            IEnumerable<FinancialsData> financialsList1 = (from x in dt.AsEnumerable()
                                                                           where x.Field<string>(5) + ":" + x.Field<string>(1) == seriesdata.drilldown
                                                                           group x by new { Createddate = x.Field<DateTime>(6), MonthYear = x.Field<string>(5), SubCategory = x.Field<string>(1) } into g
                                                                           select new FinancialsData
                                                                           {
                                                                               CreatedDate = g.Key.Createddate,
                                                                               Value = Math.Round(g.Select(a => Convert.ToDecimal(a.Field<decimal>(2))).FirstOrDefault(), 2),
                                                                               Monthyear = g.Select(a => a.Field<string>(5)).FirstOrDefault()
                                                                           }).ToList();
                            List<MultiDrillDownSeriesData> SeriesDataList = new List<MultiDrillDownSeriesData>();
                            foreach (var financials1 in financialsList1)
                            {
                                if (seriesdata.drilldown == financials1.Monthyear + ":" + maindata.name)
                                {
                                    MultiDrillDownSeriesData seriesubdata = new MultiDrillDownSeriesData()
                                    {
                                        name = financials1.CreatedDate.ToString("dd-MM-yyyy"),
                                        y = financials1.Value
                                    };
                                    SeriesDataList.Add(seriesubdata);
                                    subData1.data = SeriesDataList;
                                }
                            }
                            SubDataList.Add(subData1);//3rd level
                        }
                    }
                    subdata.data = seriesList;
                    SubDataList.Add(subdata);//2nd level
                }
                basicDrillDown.MainData = MainDataList;
                basicDrillDown.SubData = SubDataList;
            }
            else
            {
                MultiDrillDownMainData mainData = new MultiDrillDownMainData()
                {
                    name = "No Data"
                };
                MainDataList.Add(mainData);
                basicDrillDown.MainData = MainDataList;
            }
            return basicDrillDown;
        }
        #endregion

        #region AccountwatchListAdminDashBoard
        public AccountwatchListAdminDashBoardVM GetAccountwatchListAdminDashBoard(int cid, DateTime? fromdate, string todate, string dataReqType, string filterType)
        {
            AccountwatchListAdminDashBoardVM accountwatchListModel = new AccountwatchListAdminDashBoardVM();
            AccountwatchListKPI accountwatchListKpi = new AccountwatchListKPI();
            try
            {
                string[] ddValues1 = null;
                string[] ddValues2 = null;
                conn.Open();
                if (filterType == "null" && dataReqType == "null" && fromdate == null && todate == "null")
                {
                    accountwatchListModel.ToDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                    DateTime prevyear = Convert.ToDateTime(accountwatchListModel.ToDate).AddYears(-1);
                    accountwatchListModel.FromDate = new DateTime(prevyear.Year, prevyear.Month, 1);
                    cmd = new SqlCommand(Constants.Constants_AccountwatchListAdminDashBoard, conn);

                    cmd1 = new SqlCommand(Constants.Constants_AccountwatchListDd, conn);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue(Constants.Constants_Cid, cid);
                    dr1 = cmd1.ExecuteReader();
                    DataTable dt1 = new DataTable();
                    dt1.Load(dr1);
                    ddValues1 = (dt1.AsEnumerable().Select(a => a.Field<string>(0)).ToList()).ToArray();
                    ddValues1 = ddValues1 != null ? ddValues1 : null;
                    ddValues2 = (dt1.AsEnumerable().Select(a => a.Field<string>(1)).ToList()).ToArray();
                    ddValues2 = ddValues2 != null ? ddValues2 : null;

                }
                else if (filterType == "null" && dataReqType == "null" && fromdate != null && todate != "null")
                {
                    accountwatchListModel.FromDate = (DateTime)fromdate;
                    accountwatchListModel.ToDate = (string)todate + "T" + "23:59:59";
                    cmd = new SqlCommand(Constants.Constants_AccountwatchListAdminDashBoard, conn);
                }
                else if (filterType != "null" && dataReqType == "AccountWatchListBalanceSheet")
                {
                    accountwatchListModel.FromDate = (DateTime)fromdate;
                    accountwatchListModel.ToDate = (string)todate + "T" + "23:59:59";
                    cmd = new SqlCommand(Constants.Constants_AccountwatchListLeftDashBoard, conn);
                }
                else
                {
                    accountwatchListModel.FromDate = (DateTime)fromdate;
                    accountwatchListModel.ToDate = (string)todate + "T" + "23:59:59";
                    cmd = new SqlCommand(Constants.Constants_AccountwatchListRightDashBoard, conn);
                }
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(Constants.Constants_FromDate, accountwatchListModel.FromDate);
                cmd.Parameters.AddWithValue(Constants.Constants_ToDate, accountwatchListModel.ToDate);
                cmd.Parameters.AddWithValue(Constants.Constants_Cid, cid);
                if (filterType == "null" || filterType != "null")
                {
                    cmd.Parameters.AddWithValue(Constants.Constants_Type, filterType);
                }
                dr = cmd.ExecuteReader();
                if (filterType == "null")
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            accountwatchListKpi.Income += dr[0] == DBNull.Value ? 0 : Convert.ToInt32(dr[0]);
                            accountwatchListKpi.Expenses += dr[1] == DBNull.Value ? 0 : Convert.ToInt32(dr[1]);
                            accountwatchListKpi.NetProfit += dr[2] == DBNull.Value ? 0 : Convert.ToInt32(dr[2]);
                        }
                    }
                    else
                    {
                        accountwatchListKpi.Income = 0;
                        accountwatchListKpi.Expenses = 0;
                        accountwatchListKpi.NetProfit = 0;
                    }
                    accountwatchListModel.AccountWatchListKPI = accountwatchListKpi;
                }
                if (filterType == "null" || dataReqType == "AccountWatchListBalanceSheet" && filterType != "null")
                    accountwatchListModel.AccountWatchListBalanceSheet = GetAccountwatchList3LevelDD1(dr, filterType, ddValues1);
                if (filterType == "null" || dataReqType == "AccountWatchListProfitandLoss" && filterType != "null")
                    accountwatchListModel.AccountWatchListProfitandLoss = GetAccountwatchList3LevelDD2(dr, filterType, ddValues2);

            }
            catch (Exception exception)
            {
                throw exception;
            }
            return accountwatchListModel;
        }
        public MultiDrillDownVM GetAccountwatchList3LevelDD1(SqlDataReader dr, string filterType, string[] ddValues)
        {
            MultiDrillDownVM basicDrillDown = new MultiDrillDownVM();
            List<MultiDrillDownMainData> MainDataList = new List<MultiDrillDownMainData>();
            List<MultiDrillDownSubData> SubDataList = new List<MultiDrillDownSubData>();
            DataTable dt = new DataTable();
            if (filterType == "null")
                dr.NextResult();
            dt.Load(dr);
            if (dt.Rows.Count > 0)
            {
                IEnumerable<AccountwatchListAdminData> accountwatchListAdminDataList = (from x in dt.AsEnumerable()
                                                                                        group x by x.Field<string>(7) into g
                                                                                        select new AccountwatchListAdminData
                                                                                        {
                                                                                            ChartOfList = g.Key,
                                                                                            Value = Math.Round(g.Sum(a => a.Field<decimal>(2)), 2)
                                                                                        }).ToList();
                foreach (var accountwatchListAdminData in accountwatchListAdminDataList)
                {
                    MultiDrillDownMainData maindata = new MultiDrillDownMainData();
                    MultiDrillDownSubData subdata = new MultiDrillDownSubData();
                    List<MultiDrillDownSeriesData> seriesList = new List<MultiDrillDownSeriesData>();
                    maindata.name = accountwatchListAdminData.ChartOfList;
                    maindata.y = accountwatchListAdminData.Value;
                    maindata.drilldown = accountwatchListAdminData.ChartOfList;
                    MainDataList.Add(maindata);
                    IEnumerable<AccountwatchListAdminData> accountwatchListDataList = (from x in dt.AsEnumerable()
                                                                                       where x.Field<string>(7) == maindata.drilldown
                                                                                       group x by new { Monthyear = x.Field<string>(5), ChartOfList = x.Field<string>(7) } into g
                                                                                       select new AccountwatchListAdminData
                                                                                       {
                                                                                           MonthYear = g.Key.Monthyear,
                                                                                           Value = Math.Round(g.Sum(a => Convert.ToDecimal(a.Field<decimal>(2))), 2)
                                                                                       }).ToList();
                    MultiDrillDownSubData subData1;
                    foreach (var accountwatchListData in accountwatchListDataList)
                    {
                        if (maindata.name == accountwatchListAdminData.ChartOfList)
                        {
                            //2nd level
                            subdata.name = maindata.name;
                            subdata.id = maindata.name;
                            MultiDrillDownSeriesData seriesdata = new MultiDrillDownSeriesData()
                            {
                                name = accountwatchListData.MonthYear,
                                y = accountwatchListData.Value,
                                drilldown = accountwatchListData.MonthYear + ":" + maindata.name
                            };
                            seriesList.Add(seriesdata);
                            //3rd Level
                            subData1 = new MultiDrillDownSubData();
                            subData1.name = seriesdata.name + ":" + maindata.name;
                            subData1.id = seriesdata.name + ":" + maindata.name;
                            subData1.type = "line";
                            IEnumerable<AccountwatchListAdminData> AccountDataList = (from x in dt.AsEnumerable()
                                                                                      where x.Field<string>(5) + ":" + x.Field<string>(7) == seriesdata.drilldown
                                                                                      group x by new { Createddate = x.Field<DateTime>(6), MonthYear = x.Field<string>(5), ChartOfList = x.Field<string>(7) } into g
                                                                                      select new AccountwatchListAdminData
                                                                                      {
                                                                                          CreatedDate = g.Key.Createddate,
                                                                                          Value = Math.Round(g.Select(a => Convert.ToDecimal(a.Field<decimal>(2))).FirstOrDefault(), 2),
                                                                                          MonthYear = g.Select(a => a.Field<string>(5)).FirstOrDefault()
                                                                                      }).ToList();
                            List<MultiDrillDownSeriesData> SeriesDataList = new List<MultiDrillDownSeriesData>();
                            foreach (var AccountData in AccountDataList)
                            {
                                if (seriesdata.drilldown == AccountData.MonthYear + ":" + maindata.name)
                                {
                                    MultiDrillDownSeriesData seriesubdata = new MultiDrillDownSeriesData()
                                    {
                                        name = AccountData.CreatedDate.ToString("dd-MM-yyyy"),
                                        y = AccountData.Value
                                    };
                                    SeriesDataList.Add(seriesubdata);
                                    subData1.data = SeriesDataList;
                                }
                            }
                            SubDataList.Add(subData1);//3rd level
                        }
                    }
                    subdata.data = seriesList;
                    SubDataList.Add(subdata);//2nd level
                }
                basicDrillDown.MainData = MainDataList;
                basicDrillDown.SubData = SubDataList;
                basicDrillDown.DropDownValues = ddValues;
            }
            else
            {
                MultiDrillDownMainData mainData = new MultiDrillDownMainData()
                {
                    name = "No Data"
                };
                MainDataList.Add(mainData);
                basicDrillDown.MainData = MainDataList;
            }
            return basicDrillDown;
        }

        public MultiDrillDownVM GetAccountwatchList3LevelDD2(SqlDataReader dr, string filterType, string[] ddValues)
        {
            MultiDrillDownVM basicDrillDown = new MultiDrillDownVM();
            List<MultiDrillDownMainData> MainDataList = new List<MultiDrillDownMainData>();
            List<MultiDrillDownSubData> SubDataList = new List<MultiDrillDownSubData>();
            DataTable dt = new DataTable();
            dt.Load(dr);
            if (dt.Rows.Count > 0)
            {
                IEnumerable<AccountwatchListAdminData> accountwatchListAdminDataList = (from x in dt.AsEnumerable()
                                                                                        group x by x.Field<string>(7) into g
                                                                                        select new AccountwatchListAdminData
                                                                                        {
                                                                                            ChartOfList = g.Key,
                                                                                            Value = Math.Round(g.Sum(a => a.Field<decimal>(2)), 2)
                                                                                        }).ToList();
                foreach (var accountwatchListAdminData in accountwatchListAdminDataList)
                {
                    MultiDrillDownMainData maindata = new MultiDrillDownMainData();
                    MultiDrillDownSubData subdata = new MultiDrillDownSubData();
                    List<MultiDrillDownSeriesData> seriesList = new List<MultiDrillDownSeriesData>();
                    maindata.name = accountwatchListAdminData.ChartOfList;
                    maindata.y = accountwatchListAdminData.Value;
                    maindata.drilldown = accountwatchListAdminData.ChartOfList;
                    MainDataList.Add(maindata);
                    IEnumerable<AccountwatchListAdminData> accountwatchListDataList = (from x in dt.AsEnumerable()
                                                                                       where x.Field<string>(7) == maindata.drilldown
                                                                                       group x by new { Monthyear = x.Field<string>(5), ChartOfList = x.Field<string>(7) } into g
                                                                                       select new AccountwatchListAdminData
                                                                                       {
                                                                                           MonthYear = g.Key.Monthyear,
                                                                                           Value = Math.Round(g.Sum(a => Convert.ToDecimal(a.Field<decimal>(2))), 2)
                                                                                       }).ToList();
                    MultiDrillDownSubData subData1;
                    foreach (var accountwatchListData in accountwatchListDataList)
                    {
                        if (maindata.name == accountwatchListAdminData.ChartOfList)
                        {
                            //2nd level
                            subdata.name = maindata.name;
                            subdata.id = maindata.name;
                            MultiDrillDownSeriesData seriesdata = new MultiDrillDownSeriesData()
                            {
                                name = accountwatchListData.MonthYear,
                                y = accountwatchListData.Value,
                                drilldown = accountwatchListData.MonthYear + ":" + maindata.name
                            };
                            seriesList.Add(seriesdata);
                            //3rd Level
                            subData1 = new MultiDrillDownSubData();
                            subData1.name = seriesdata.name + ":" + maindata.name;
                            subData1.id = seriesdata.name + ":" + maindata.name;
                            subData1.type = "line";
                            IEnumerable<AccountwatchListAdminData> AccountDataList = (from x in dt.AsEnumerable()
                                                                                      where x.Field<string>(5) + ":" + x.Field<string>(7) == seriesdata.drilldown
                                                                                      group x by new { Createddate = x.Field<DateTime>(6), MonthYear = x.Field<string>(5), ChartOfList = x.Field<string>(7) } into g
                                                                                      select new AccountwatchListAdminData
                                                                                      {
                                                                                          CreatedDate = g.Key.Createddate,
                                                                                          Value = Math.Round(g.Select(a => Convert.ToDecimal(a.Field<decimal>(2))).FirstOrDefault(), 2),
                                                                                          MonthYear = g.Select(a => a.Field<string>(5)).FirstOrDefault()
                                                                                      }).ToList();
                            List<MultiDrillDownSeriesData> SeriesDataList = new List<MultiDrillDownSeriesData>();
                            foreach (var AccountData in AccountDataList)
                            {
                                if (seriesdata.drilldown == AccountData.MonthYear + ":" + maindata.name)
                                {
                                    MultiDrillDownSeriesData seriesubdata = new MultiDrillDownSeriesData()
                                    {
                                        name = AccountData.CreatedDate.ToString("dd-MM-yyyy"),
                                        y = AccountData.Value
                                    };
                                    SeriesDataList.Add(seriesubdata);
                                    subData1.data = SeriesDataList;
                                }
                            }
                            SubDataList.Add(subData1);//3rd level
                        }
                    }
                    subdata.data = seriesList;
                    SubDataList.Add(subdata);//2nd level
                }
                basicDrillDown.MainData = MainDataList;
                basicDrillDown.SubData = SubDataList;
                basicDrillDown.DropDownValues = ddValues;
            }
            else
            {
                MultiDrillDownMainData mainData = new MultiDrillDownMainData()
                {
                    name = "No Data"
                };
                MainDataList.Add(mainData);
                basicDrillDown.MainData = MainDataList;
            }
            return basicDrillDown;
        }
        #endregion 
    }
}
