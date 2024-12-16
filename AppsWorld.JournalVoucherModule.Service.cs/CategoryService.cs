using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.Models;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Ziraff.FrameWork;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Category> _categoryRepository;
        private readonly IJournalVoucherModuleRepositoryAsync<SubCategory> _subCategoryTypeRepository;
        private readonly IJournalVoucherModuleRepositoryAsync<AccountType> _accountTypeRepository;

        public CategoryService(IJournalVoucherModuleRepositoryAsync<Category> categoryTypeRepository,
            IJournalVoucherModuleRepositoryAsync<SubCategory> subCategoryTypeRepository,
            IJournalVoucherModuleRepositoryAsync<AccountType> accountTypeRepository)
            : base(categoryTypeRepository)
        {
            _categoryRepository = categoryTypeRepository;
            _subCategoryTypeRepository = subCategoryTypeRepository;
            _accountTypeRepository = accountTypeRepository;
        }


        private static string _connectionString = null;
        private static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    //    _connectionString = KeyVaultService.GetSecret(
                    //ConfigurationManager.AppSettings["AppsWorldDBContextClientId"],
                    //ConfigurationManager.AppSettings["AppsWorldDBContextClientSecret"],
                    //ConfigurationManager.AppSettings["AppsWorldDBContextKeySecretUri"]);
                    _connectionString = Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection;
                }
                return _connectionString;
            }
        }



        public List<SubCategory> Getsubcategory(long companyid)
        {
            return _subCategoryTypeRepository.Query(c => c.CompanyId == companyid).Select().ToList();
        }
        public List<Category> GetCategories(long companyid)
        {
            return _categoryRepository.Query(c => c.CompanyId == companyid).Select().ToList();
        }
        public List<AccountType> GetAllAccounyTypeByCompanyId(long companyid)
        {
            var data = _accountTypeRepository.Queryable().Where(c => c.CompanyId == companyid).AsQueryable();
            return data.OrderBy(k => k.RecOrder).ToList();
        }
        public  List<TrailBalanceSpModel> GetAllAccountsBy_Bean_HTMLTrailBalanceSP(long companyid, string compnayName, DateTime date, long frequency, int Period, int SamePeriod)
        {
            date = date.Date;
            List<TrailBalanceSpModel> lstTrailBalanceViewModels = new List<TrailBalanceSpModel>();
            SqlDataReader dr;
            SqlCommand selectCommand;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (selectCommand = new SqlCommand("Bean_HTMLTrailBalance", connection))
                {
                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 0;
                    selectCommand.Parameters.AddWithValue("@companyId", companyid);
                    selectCommand.Parameters.AddWithValue("@ServiceCompany", compnayName);
                    selectCommand.Parameters.AddWithValue("@Date", date);
                    selectCommand.Parameters.AddWithValue("@Frequency", frequency);
                    selectCommand.Parameters.AddWithValue("@Comparision", Period);
                    selectCommand.Parameters.AddWithValue("@SamePeriod", SamePeriod);
                    using (dr =  selectCommand.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(dr);
                        foreach (DataRow row in dataTable.Rows)
                        {
                            TrailBalanceSpModel trailBalanceViewModel = new TrailBalanceSpModel();

                            var id = row["id"].ToString();
                            if (id != null && id != string.Empty)
                            {
                                trailBalanceViewModel.Recorder = Convert.ToInt64(id);
                            }

                            trailBalanceViewModel.Name = row["COA Name"].ToString();
                            trailBalanceViewModel.Code = row["COA Code"].ToString();
                            trailBalanceViewModel.Class = row["Class"].ToString();
                            trailBalanceViewModel.Year = row["Period"].ToString();
                            trailBalanceViewModel.Debit = row["Debit"].ToString();
                            trailBalanceViewModel.Credit = row["Credit"].ToString();
                            trailBalanceViewModel.Percentage = row["Comparision"].ToString();

                            lstTrailBalanceViewModels.Add(trailBalanceViewModel);
                        }
                        connection.Close();
                    }
                }
            }
            return lstTrailBalanceViewModels;
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
        public  List<BalanceSheetSpModel> GetAllAccountsBy_Bean_HTMLBalanceSheetSP(long companyid, string companyName, DateTime asOfDate, int frequency, int period, int SamePeriod)
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
                    using (dr =  selectCommand.ExecuteReader())
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
        //public long GetMaxIdFromCategory()
        //{
        //    long j = 1;
        //    var lstData = _categoryRepository.Query().Select(c => c.Id).ToList();
        //    var maxId = lstData.Count() == 0 ? j : lstData.Max() + j;
        //    return maxId;

        //}

        public Category GetCategorie(Guid categoryId)
        {
            return _categoryRepository.Queryable().Where(a => a.Id == categoryId).FirstOrDefault();
        }
    }
}

