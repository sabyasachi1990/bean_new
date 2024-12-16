using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.RepositoryPattern.V3;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Models.V3;
using AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal;
using AppsWorld.JournalVoucherModule.Models;
using BalanceSheetSpModel = AppsWorld.JournalVoucherModule.Models.V3.BalanceSheetSpModel;
using IncomeStatementSpModel = AppsWorld.JournalVoucherModule.Models.V3.IncomeStatementSpModel;

namespace AppsWorld.JournalVoucherModule.Service.cs.V3.Journal
{
    public class JournalServiceV3 : Service<Entities.Models.V3.Journal.CategoryV3>, IJournalServiceV3
    {
        private readonly IJournalVoucherModuleRepositoryAsyncV3<CategoryV3> _categoryRepository;

        private readonly IJournalVoucherModuleRepositoryAsyncV3<SubCategoryV3> _subCategoryRepository;
        
        private readonly IJournalVoucherModuleRepositoryAsyncV3<AccountTypeV3> _accountTypeRepository;
        private readonly IJournalVoucherModuleRepositoryAsyncV3<ChartOfAccountV3> _chartOfAccountRepository;
        public JournalServiceV3(IJournalVoucherModuleRepositoryAsyncV3<CategoryV3> categoryRepository, IJournalVoucherModuleRepositoryAsyncV3<SubCategoryV3> subCategoryTypeRepository, IJournalVoucherModuleRepositoryAsyncV3<AccountTypeV3> accountTypeRepository, IJournalVoucherModuleRepositoryAsyncV3<ChartOfAccountV3> chartOfAccountRepository)  
            : base (categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryTypeRepository;   
            _accountTypeRepository = accountTypeRepository; 
            _chartOfAccountRepository = chartOfAccountRepository;   
        }

        private static string _connectionString = null;
        private static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                  
                    _connectionString = Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys["SecondaryDbConnection"].ToString();
                }
                return _connectionString;
            }
        }
        public List<SubCategoryV3> Getsubcategory(long companyid)
        {
            return _subCategoryRepository.Query(c => c.CompanyId == companyid).Select().ToList();
        }
        public List<CategoryV3> GetCategories(long companyid)
        {
            return _categoryRepository.Query(c => c.CompanyId == companyid).Select().ToList();
        }
        public List<AccountTypeV3> GetAllAccounyTypeByCompanyId(long companyid)
        {
            var data = _accountTypeRepository.Queryable().Where(c => c.CompanyId == companyid).AsQueryable();
            return data.OrderBy(k => k.RecOrder).ToList();
        }
        public Dictionary<long, long?> GetListChartofAccounts(List<long> accTypeIds, long companyId, List<long> servEntityIds)
        {
            return _chartOfAccountRepository.Query(a => accTypeIds.Contains(a.AccountTypeId) && a.CompanyId == companyId && a.IsRevaluation != 1 && servEntityIds.Contains(a.SubsidaryCompanyId.Value)).Select(a => new { a.Id, a.SubsidaryCompanyId }).ToDictionary(t => t.Id, t => t.SubsidaryCompanyId);
        }

        public List<IncomeStatementSpModel> GetAllAccountsBy_Bean_HTMLIncomeStatmentSP(long companyid, string compnayName, DateTime fromdate, DateTime todate, int samePeriod, int period, long frequency, bool isInterco)
        {
            List<IncomeStatementSpModel> lstincomeStatementSpModels = new List<IncomeStatementSpModel>();
            SqlDataReader dr;
            SqlCommand selectCommand;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (selectCommand = new SqlCommand("Bean_HTMLIncomeStatment", connection))
                {
                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 0;
                    selectCommand.Parameters.AddWithValue("@companyId", companyid);
                    selectCommand.Parameters.AddWithValue("@ServiceCompany", compnayName);
                    selectCommand.Parameters.AddWithValue("@FDate", fromdate);
                    selectCommand.Parameters.AddWithValue("@Date", todate);
                    selectCommand.Parameters.AddWithValue("@frequency", frequency);
                    selectCommand.Parameters.AddWithValue("@Comparision", period);
                    selectCommand.Parameters.AddWithValue("@SamePeriod", samePeriod);
                    selectCommand.Parameters.AddWithValue("@ExcludeInterco", isInterco);
                    using (dr = selectCommand.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(dr);
                        foreach (DataRow row in dataTable.Rows)
                        {
                            IncomeStatementSpModel incomeStatementSpModel = new IncomeStatementSpModel();
                            var id = row["RId"].ToString();
                            if (id != null && id != string.Empty)
                            {
                                incomeStatementSpModel.Recorder = Convert.ToInt64(id);
                            }
                            incomeStatementSpModel.FRCoaId = Guid.Parse(row["FRCoaId"].ToString());
                            incomeStatementSpModel.FRPATId = Guid.Parse(row["FRPATId"].ToString());
                            var catid = (row["CategoryId"].ToString());
                            if (catid != null && catid != string.Empty)
                                incomeStatementSpModel.CategoryId = Guid.Parse(row["CategoryId"].ToString());
                            else
                                incomeStatementSpModel.CategoryId = null;
                            var subcatid = (row["SubCategoryId"].ToString());
                            if (subcatid != null && subcatid != string.Empty)
                                incomeStatementSpModel.SubCategoryId = Guid.Parse(row["SubCategoryId"].ToString());
                            else
                                incomeStatementSpModel.SubCategoryId = null;
                            incomeStatementSpModel.Code = row["COA Code"].ToString();
                            incomeStatementSpModel.Name = row["COA Name"].ToString();
                            incomeStatementSpModel.Year = row["Period"].ToString();
                            var fsds = (row["Balance"].ToString());
                            incomeStatementSpModel.Balance = (fsds != null && fsds != string.Empty) ? Convert.ToDecimal(fsds) : 0;
                            var companision = row["Comparision"].ToString();
                            if (companision != null && companision != string.Empty)
                                incomeStatementSpModel.Percentage = Convert.ToDecimal(row["Comparision"].ToString());
                            else
                                incomeStatementSpModel.Percentage = null;

                            var FRRecOrder = row["FRRecOrder"].ToString();
                            if (FRRecOrder != null && FRRecOrder != string.Empty)
                                incomeStatementSpModel.FRRecOrder = Convert.ToInt32(row["FRRecOrder"].ToString());
                            else
                                incomeStatementSpModel.FRRecOrder = null;

                            lstincomeStatementSpModels.Add(incomeStatementSpModel);
                        }
                        connection.Close();
                    }
                }
            }

            return lstincomeStatementSpModels;
        }


        public List<BalanceSheetSpModel> GetAllAccountsBy_Bean_HTMLBalanceSheetSP(long companyid, string companyName, DateTime asOfDate, int frequency, int period, int SamePeriod)
        {
            List<BalanceSheetSpModel> lstBalanceSheetSpModel = new List<BalanceSheetSpModel>();
            SqlDataReader dr;
            SqlCommand selectCommand;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (selectCommand = new SqlCommand("Bean_HTMLBalanceSheet", connection))
                {
                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 0;
                    selectCommand.Parameters.AddWithValue("@CompanyId", companyid);
                    selectCommand.Parameters.AddWithValue("@ServiceCompany", companyName);
                    selectCommand.Parameters.AddWithValue("@date", asOfDate);
                    selectCommand.Parameters.AddWithValue("@frequency", frequency);
                    selectCommand.Parameters.AddWithValue("@Comparision", period);
                    selectCommand.Parameters.AddWithValue("@SamePeriod", SamePeriod);
                    using (dr = selectCommand.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(dr);
                        foreach (DataRow row in dataTable.Rows)
                        {
                            BalanceSheetSpModel balanceSheetSpModel = new BalanceSheetSpModel();

                            var id = row["RId"].ToString();
                            if (id != null && id != string.Empty)
                            {
                                balanceSheetSpModel.Recorder = Convert.ToInt64(id);
                            }


                            balanceSheetSpModel.FRCoaId = Guid.Parse(row["FRCoaId"].ToString());
                            balanceSheetSpModel.FRPATId = Guid.Parse(row["FRPATId"].ToString());
                            var catid = (row["CategoryId"].ToString());
                            if (catid != null && catid != string.Empty)
                                balanceSheetSpModel.CategoryId = Guid.Parse(row["CategoryId"].ToString());
                            else
                                balanceSheetSpModel.CategoryId = null;
                            var subcatid = (row["SubCategoryId"].ToString());
                            if (subcatid != null && subcatid != string.Empty)
                                balanceSheetSpModel.SubCategoryId = Guid.Parse(row["SubCategoryId"].ToString());
                            else
                                balanceSheetSpModel.SubCategoryId = null;
                            balanceSheetSpModel.Code = row["COA Code"].ToString();
                            balanceSheetSpModel.Name = row["COA Name"].ToString();
                            balanceSheetSpModel.Year = row["Period"].ToString();
                            var fsds = (row["Balance"].ToString());
                            balanceSheetSpModel.Balance = (fsds != null && fsds != string.Empty) ? Convert.ToDecimal(fsds) : 0;
                            var companision = row["Comparision"].ToString();
                            if (companision != null && companision != string.Empty)
                                balanceSheetSpModel.Percentage = Convert.ToDecimal(row["Comparision"].ToString());
                            else
                                balanceSheetSpModel.Percentage = null;

                            var FRRecOrder = row["FRRecOrder"].ToString();
                            if (FRRecOrder != null && FRRecOrder != string.Empty)
                                balanceSheetSpModel.FRRecOrder = Convert.ToInt32(row["FRRecOrder"].ToString());
                            else
                                balanceSheetSpModel.FRRecOrder = null;

                            balanceSheetSpModel.CoaId = row["COAID"] != DBNull.Value ? Convert.ToInt64(row["COAID"]) : (long?)null;

                            lstBalanceSheetSpModel.Add(balanceSheetSpModel);
                        }
                        connection.Close();
                    }
                }
            }
            return lstBalanceSheetSpModel;
        }
      
    }
}
