using AppsWorld.CommonModule.Infra;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal;
using AppsWorld.JournalVoucherModule.Infra;
using AppsWorld.JournalVoucherModule.Models.V3;
using AppsWorld.JournalVoucherModule.Service.cs.V3.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ColumnLsts = AppsWorld.JournalVoucherModule.Models.V3.ColumnLsts;

namespace AppsWorld.JournalVoucherModule.Application.V3
{
    public class JournalApplicationServiceV3
    {
        private readonly IJournalServiceV3  _journalServiceV3;
        public JournalApplicationServiceV3(IJournalServiceV3 journalServiceV3)
        {
            _journalServiceV3 = journalServiceV3;
        }

        #region GetNewIncomestatement
        public NewStatementModel GetNewIncomestatement(CommonModel commonModel)
        {
            try
            {
                NewStatementModel newIncomeStatementModel = new NewStatementModel();
                int Period = 0;
                Period = GetPeriodBasedonType(commonModel, Period);
                int SamePeriod = commonModel.SamePeriod ? 1 : 0;
                var lstAccountsData = _journalServiceV3.GetAllAccountsBy_Bean_HTMLIncomeStatmentSP(commonModel.CompanyId, commonModel.CompanyName, commonModel.Fromdate, commonModel.Todate, SamePeriod, Period, commonModel.Frequency, commonModel.IsInterco);
                var lstcategories = _journalServiceV3.GetCategories(commonModel.CompanyId).Where(a => a.IsIncomeStatement == true).ToList();

                var lstsubcategorys = _journalServiceV3.Getsubcategory(commonModel.CompanyId).Where(a => a.IsIncomeStatement == true).ToList();
               
                List<AccountTypeV3> lstleadsheet = _journalServiceV3.GetAllAccounyTypeByCompanyId(commonModel.CompanyId).Where(c => c.Category == "Income Statement").ToList();

                var accountName = lstAccountsData.Select(c => c.Name).FirstOrDefault();
                var ColumnList = lstAccountsData.Where(c => c.Name == accountName).OrderBy(c => c.Recorder).Select(c => c.Year).ToList();

                IncomeStatementModel incomeStatementModel = new IncomeStatementModel();
                BuildColumnsHTMLDataNew(incomeStatementModel, ColumnList, commonModel);
                newIncomeStatementModel.ColumnLists = incomeStatementModel.ColumnLists;

                List<Guid?> lstLeadSheetIds = new List<Guid?>();
                lstLeadSheetIds.AddRange(lstsubcategorys.Where(c => c.Type == "LeadSheet" && c.ParentId == lstcategories.Where(x => x.Name == ControlCodeConstants.GrossProfit).Select(v => v.Id).FirstOrDefault()).OrderBy(b => b.Recorder).Select(c => c.TypeId).ToList());
                lstLeadSheetIds.AddRange(lstsubcategorys.Where(c => c.Type == "LeadSheet" && c.ParentId == lstcategories.Where(x => x.Name == ControlCodeConstants.ProfitbeforeTax).Select(v => v.Id).FirstOrDefault()).OrderBy(b => b.Recorder).Select(c => c.TypeId).ToList());
                lstLeadSheetIds.AddRange(lstsubcategorys.Where(c => c.Type == "LeadSheet" && c.ParentId == lstcategories.Where(x => x.Name == ControlCodeConstants.ProfitafterTax).Select(v => v.Id).FirstOrDefault()).OrderBy(b => b.Recorder).Select(c => c.TypeId).ToList());
                lstleadsheet = lstleadsheet.OrderBy(d => lstLeadSheetIds.IndexOf(d.FRATId)).ToList();

                List<AccountNewModel> lstAccoutNewModel = new List<AccountNewModel>();
                FillIncomeStatementModels(lstAccountsData, lstsubcategorys, lstcategories, lstleadsheet, ColumnList, lstAccoutNewModel);
                newIncomeStatementModel.ListAccountNewModel = AddingFinalTotalsandSetTotalsOrder(ColumnList, lstAccoutNewModel);
                return newIncomeStatementModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private List<AccountNewModel> AddingFinalTotalsandSetTotalsOrder(List<string> ColumnList, List<AccountNewModel> lstAccoutNewModel)
        {
            var grossProfitAccounts = lstAccoutNewModel.Where(c => c.GroupHeading == ControlCodeConstants.GrossProfit).ToList();
            var profitBeforeTaxAccounts = lstAccoutNewModel.Where(c => c.GroupHeading == ControlCodeConstants.ProfitbeforeTax).ToList();
            var profitAfterTaxAccounts = lstAccoutNewModel.Where(c => c.GroupHeading == ControlCodeConstants.ProfitafterTax).ToList();


            //Gross Profit

            AccountNewModel accountNewModel1 = new AccountNewModel();
            accountNewModel1.AccountName = ControlCodeConstants.GrossProfit;
            accountNewModel1.AccountType = ControlCodeConstants.GrossProfit;
            accountNewModel1.Type = ControlCodeConstants.Total;
            accountNewModel1.GroupType = ControlCodeConstants.GrossProfit;
            accountNewModel1.GroupHeading = ControlCodeConstants.GrossProfit;
            accountNewModel1.YearModels = SubTotals(ColumnList, grossProfitAccounts, "SubTotals");
            var amount1 = accountNewModel1.YearModels.Where(x => x.Balance != null).Sum(c => c.Balance);
            accountNewModel1.IsShowZero = (amount1 == null || amount1 == 0) ? true : false;
            accountNewModel1.Recorder = lstAccoutNewModel.Where(c => c.GroupHeading == ControlCodeConstants.GrossProfit).Count();
            lstAccoutNewModel.Insert(lstAccoutNewModel.Where(c => c.GroupHeading == ControlCodeConstants.GrossProfit).Count(), accountNewModel1);

            //Total Expenses

            AccountNewModel TotalExpenses = new AccountNewModel();
            TotalExpenses.AccountName = ControlCodeConstants.TotalExpenses;
            TotalExpenses.AccountType = ControlCodeConstants.TotalExpenses;
            TotalExpenses.Type = ControlCodeConstants.Total;
            TotalExpenses.GroupType = ControlCodeConstants.TotalExpenses;
            TotalExpenses.GroupHeading = ControlCodeConstants.ProfitbeforeTax;
            TotalExpenses.YearModels = SubTotals(ColumnList, profitBeforeTaxAccounts.Where(c => c.Class == "Expenses").ToList(), "SubTotals");
            var totalExpensesAmount = TotalExpenses.YearModels.Where(x => x.Balance != null).Sum(c => c.Balance);
            TotalExpenses.IsShowZero = (totalExpensesAmount == null || totalExpensesAmount == 0) ? true : false;
            TotalExpenses.Recorder = lstAccoutNewModel.Where(c => c.GroupHeading == ControlCodeConstants.GrossProfit).Count() + lstAccoutNewModel.Where(c => c.Class == ControlCodeConstants.Expenses && c.GroupHeading == ControlCodeConstants.ProfitbeforeTax).Count();
            lstAccoutNewModel.Insert((int)TotalExpenses.Recorder, TotalExpenses);



            //Total Other income

            AccountNewModel TotalOtherIncome = new AccountNewModel();
            TotalOtherIncome.AccountName = ControlCodeConstants.TotalOtherIncome;
            TotalOtherIncome.AccountType = ControlCodeConstants.TotalOtherIncome;
            TotalOtherIncome.Type = ControlCodeConstants.Total;
            TotalOtherIncome.GroupType = ControlCodeConstants.TotalOtherIncome;
            TotalOtherIncome.GroupHeading = ControlCodeConstants.ProfitbeforeTax;
            TotalOtherIncome.YearModels = SubTotals(ColumnList, profitBeforeTaxAccounts.Where(c => c.Class == "Income").ToList(), "SubTotals");
            var totalOtherIncomeAmount = TotalOtherIncome.YearModels.Where(x => x.Balance != null).Sum(c => c.Balance);
            TotalOtherIncome.IsShowZero = (totalOtherIncomeAmount == null || totalOtherIncomeAmount == 0) ? true : false;
            TotalOtherIncome.Recorder = lstAccoutNewModel.Where(c => c.GroupHeading == ControlCodeConstants.GrossProfit).Count() + lstAccoutNewModel.Where(c => c.GroupHeading == ControlCodeConstants.ProfitbeforeTax).Count();
            lstAccoutNewModel.Insert((int)TotalOtherIncome.Recorder, TotalOtherIncome);

            //Profit before Tax

            AccountNewModel ProfitbeforeTax = new AccountNewModel();
            ProfitbeforeTax.AccountName = ControlCodeConstants.ProfitbeforeTax;
            ProfitbeforeTax.AccountType = ControlCodeConstants.ProfitbeforeTax;
            ProfitbeforeTax.Type = ControlCodeConstants.Total;
            ProfitbeforeTax.GroupType = ControlCodeConstants.ProfitbeforeTax;
            ProfitbeforeTax.GroupHeading = ControlCodeConstants.ProfitbeforeTax;
            profitBeforeTaxAccounts.AddRange(grossProfitAccounts);
            ProfitbeforeTax.YearModels = SubTotals(ColumnList, profitBeforeTaxAccounts, "SubTotals");
            var profitbeforeTaxAmount = ProfitbeforeTax.YearModels.Where(x => x.Balance != null).Sum(c => c.Balance);
            ProfitbeforeTax.IsShowZero = (profitbeforeTaxAmount == null || profitbeforeTaxAmount == 0) ? true : false;
            ProfitbeforeTax.Recorder = lstAccoutNewModel.Where(c => c.GroupHeading == ControlCodeConstants.GrossProfit).Count() + lstAccoutNewModel.Where(c => c.GroupHeading == ControlCodeConstants.ProfitbeforeTax).Count();
            lstAccoutNewModel.Insert((int)ProfitbeforeTax.Recorder, ProfitbeforeTax);

            //NetProfit

            AccountNewModel NetProfit = new AccountNewModel();
            NetProfit.AccountName = ControlCodeConstants.NetProfit;
            NetProfit.AccountType = ControlCodeConstants.NetProfit;
            NetProfit.Type = ControlCodeConstants.FinalTotal;
            NetProfit.GroupType = ControlCodeConstants.NetProfit;
            NetProfit.GroupHeading = ControlCodeConstants.NetProfit;
            NetProfit.YearModels = SubTotals(ColumnList, lstAccoutNewModel.ToList(), "SubTotals");
            var amount4 = NetProfit.YearModels.Where(x => x.Balance != null).Sum(c => c.Balance);
            NetProfit.IsShowZero = (amount4 == null || amount4 == 0) ? true : false;
            NetProfit.Recorder = lstAccoutNewModel.Count();
            lstAccoutNewModel.Insert((int)NetProfit.Recorder, NetProfit);

            return lstAccoutNewModel;
        }
        private List<YearModels> SubTotals(List<string> ColumnList, List<AccountNewModel> lstAccoutNewModel, string subTotals)
        {
            List<YearModels> lstYearModels = new List<YearModels>();
            decimal? currentPeriod = 0;
            decimal? PriorPeriod = 0;
            int i = 0;
            string classType = null;
            bool isIncome = false;
            bool isExpence = false;

            if (subTotals == "SubTotals")
            {
                isIncome = lstAccoutNewModel.Where(c => c.Class == ControlCodeConstants.Income).Any();
                isExpence = lstAccoutNewModel.Where(c => c.Class == ControlCodeConstants.Expenses).Any();

                if (isIncome && isExpence)
                    classType = ControlCodeConstants.Income;
                else if (isIncome && !isExpence)
                    classType = ControlCodeConstants.Income;
                else if (!isIncome && isExpence)
                    classType = ControlCodeConstants.Expenses;
            }


            var lstIncome = lstAccoutNewModel.Where(c => c.Type == ControlCodeConstants.Item && c.Class == ControlCodeConstants.Income).SelectMany(c => c.YearModels).ToList();
            var lstExpences = lstAccoutNewModel.Where(c => c.Type == ControlCodeConstants.Item && c.Class == ControlCodeConstants.Expenses).SelectMany(c => c.YearModels).ToList();

            foreach (var colom in ColumnList)
            {
                YearModels yearModels = new YearModels();
                yearModels.Year = colom;
                if (isIncome && isExpence)
                    yearModels.Balance = lstIncome.Where(x => x.Year == colom).Sum(c => c.Balance) - lstExpences.Where(x => x.Year == colom).Sum(c => c.Balance);
                else if (isIncome && !isExpence)
                    yearModels.Balance = lstIncome.Where(x => x.Year == colom).Sum(c => c.Balance);
                else if (!isIncome && isExpence)
                    yearModels.Balance = lstExpences.Where(x => x.Year == colom).Sum(c => c.Balance);
                yearModels.Percentage = 0;
                if (i == 0)
                    currentPeriod = yearModels.Balance;
                else if (i == 1)
                    PriorPeriod = yearModels.Balance;
                if (colom.Contains("Vs"))
                {
                    i = 0;
                    yearModels.IsPercentage = true;

                    if (PriorPeriod == 0)
                    {
                        yearModels.Percentage = null;
                        yearModels.FontColor = null;
                    }
                    else if (currentPeriod < 0 && PriorPeriod < 0)
                    {
                        var PriorPeriodValue = PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod;
                        yearModels.Percentage = ((currentPeriod * -1) - PriorPeriodValue) / PriorPeriodValue * 100;
                        yearModels.FontColor = AssignNewColor(classType, yearModels.Percentage);
                    }
                    else if (classType == "Expenses")
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColorTotal(classType, yearModels.Percentage ?? 0, currentPeriod, PriorPeriod);
                    }
                    else
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColor(classType, yearModels.Percentage ?? 0);
                    }
                    currentPeriod = PriorPeriod;
                }
                i++;

                lstYearModels.Add(yearModels);
            }
            return lstYearModels;
        }
        private void FillIncomeStatementModels(List<IncomeStatementSpModel> lstAccountsData, List<SubCategoryV3> lstsubcategorys, List<CategoryV3> lstcategories, List<AccountTypeV3> lstleadsheet, List<string> ColumnList, List<AccountNewModel> lstAccoutNewModel)
        {
            int Rec = 1;
            foreach (var accountType in lstleadsheet)
            {
                var lstChartOfAccounts = lstAccountsData.Where(c => c.FRPATId == accountType.FRATId).ToList();
                lstChartOfAccounts = lstChartOfAccounts.DistinctBy(c => c.FRCoaId).ToList();
                foreach (var coaAccout in lstChartOfAccounts)
                {
                    AccountNewModel accountNewModel = new AccountNewModel();
                    accountNewModel.AccountName = coaAccout.Name;
                    accountNewModel.AccountType = accountType.Name;
                    accountNewModel.Type = ControlCodeConstants.Item;
                    accountNewModel.Class = accountType.Class;
                    accountNewModel.Recorder = Rec;
                    accountNewModel.GroupType = lstcategories.Where(c => c.Id == lstsubcategorys.Where(x => x.Name == accountType.Name).Select(v => v.ParentId).FirstOrDefault()).Select(o => o.Name).FirstOrDefault();
                    FillGroupType(accountNewModel);
                    accountNewModel.GroupHeading = lstcategories.Where(c => c.Id == lstsubcategorys.Where(x => x.Name == accountType.Name).Select(v => v.ParentId).FirstOrDefault()).Select(o => o.Name).FirstOrDefault();

                   
                    var accounts = lstAccountsData.Where(c => c.Name == coaAccout.Name).OrderBy(c => c.Code).ToList();
                    accountNewModel.YearModels = AccountTotals(ColumnList, accounts, accountType);

                    var amount = accountNewModel.YearModels.Where(x => x.Balance != null).Sum(c => c.Balance);
                    accountNewModel.IsShowZero = (amount == null || amount == 0);
                    if ((amount == null || amount == 0) && (accountNewModel.YearModels.Any(d => d.Balance > 0) || accountNewModel.YearModels.Any(d => d.Balance < 0)))
                        accountNewModel.IsShowZero = false;
                    lstAccoutNewModel.Add(accountNewModel);
                    Rec++;
                }

                AccountNewModel accountTypeModel = new AccountNewModel();
                accountTypeModel.AccountName = ControlCodeConstants.Total;
                accountTypeModel.AccountType = accountType.Name;
                accountTypeModel.Type = ControlCodeConstants.Total;
                accountTypeModel.Class = accountType.SubCategory;
                accountTypeModel.GroupType = lstcategories.Where(c => c.Id == lstsubcategorys.Where(x => x.Name == accountType.Name).Select(v => v.ParentId).FirstOrDefault()).Select(o => o.Name).FirstOrDefault();
                FillGroupType(accountTypeModel);
                accountTypeModel.GroupHeading = lstcategories.Where(c => c.Id == lstsubcategorys.Where(x => x.Name == accountType.Name).Select(v => v.ParentId).FirstOrDefault()).Select(o => o.Name).FirstOrDefault();
                accountTypeModel.Recorder = Rec;
                accountTypeModel.YearModels = AccountTypeTotals(ColumnList, lstAccoutNewModel, accountType);
                var amountTotal = lstAccoutNewModel.Where(c => c.AccountType == accountType.Name && c.Type == ControlCodeConstants.Item).SelectMany(c => c.YearModels).Where(x => x.Balance != null && x.Balance != 0).Count();
                accountTypeModel.IsShowZero = amountTotal == 0;
                lstAccoutNewModel.Add(accountTypeModel);
                Rec++;
            }
        }
        private List<YearModels> AccountTypeTotals(List<string> ColumnList, List<AccountNewModel> lstAccoutNewModel, AccountTypeV3 accountType1)
        {
            List<YearModels> lstYearModels = new List<YearModels>();
            decimal? currentPeriod = 0;
            decimal? PriorPeriod = 0;
            int i = 0;
            foreach (var colom in ColumnList)
            {
                YearModels yearModels = new YearModels();
                yearModels.Year = colom;
                yearModels.Balance = lstAccoutNewModel.Where(c => c.Type == ControlCodeConstants.Item && c.AccountType == accountType1.Name).SelectMany(c => c.YearModels).ToList().Where(x => x.Year == colom).Sum(c => c.Balance);

                yearModels.Percentage = 0;
                if (i == 0)
                    currentPeriod = yearModels.Balance;
                else if (i == 1)
                    PriorPeriod = yearModels.Balance;
                if (colom.Contains("Vs"))
                {
                    i = 0;
                    yearModels.IsPercentage = true;

                    if (PriorPeriod == 0)
                    {
                        yearModels.Percentage = null;
                        yearModels.FontColor = null;
                    }
                    else if (currentPeriod < 0 && PriorPeriod < 0)
                    {
                        var PriorPeriodValue = PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod;
                        yearModels.Percentage = ((currentPeriod * -1) - PriorPeriodValue) / PriorPeriodValue * 100;
                        yearModels.FontColor = AssignNewColor(accountType1.Class, yearModels.Percentage);
                    }
                    else if (accountType1.Class == "Expenses")
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = yearModels.Percentage == 0 ? "red" : AssignColorTotal(accountType1.Class, yearModels.Percentage ?? 0, currentPeriod, PriorPeriod);
                    }
                    else
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColor(accountType1.Class, yearModels.Percentage ?? 0);
                    }
                    currentPeriod = PriorPeriod;
                }
                i++;

                lstYearModels.Add(yearModels);
            }
            return lstYearModels;
        }
        private List<YearModels> AccountTotals(List<string> ColumnList, List<IncomeStatementSpModel> lstAccoutNewModel, AccountTypeV3 accountType1)
        {
            List<YearModels> lstYearModels = new List<YearModels>();
            decimal? currentPeriod = 0;
            decimal? PriorPeriod = 0;
            int i = 0;

            foreach (var colom in ColumnList)
            {
                YearModels yearModels = new YearModels();
                yearModels.Year = colom;
                yearModels.Balance = lstAccoutNewModel.Where(c => c.Year == colom).Select(c => c.Balance).FirstOrDefault();

                yearModels.Percentage = 0;
                if (i == 0)
                    currentPeriod = yearModels.Balance;
                else if (i == 1)
                    PriorPeriod = yearModels.Balance;
                if (colom.Contains("Vs"))
                {
                    i = 0;
                    yearModels.IsPercentage = true;

                    if (PriorPeriod == 0)
                    {
                        yearModels.Percentage = null;
                        yearModels.FontColor = null;
                    }
                    else if (currentPeriod < 0 && PriorPeriod < 0)
                    {
                        var PriorPeriodValue = PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod;
                        yearModels.Percentage = ((currentPeriod * -1) - PriorPeriodValue) / PriorPeriodValue * 100;
                        yearModels.FontColor = AssignNewColor(accountType1.Class, yearModels.Percentage);
                    }
                    else if (accountType1.Class == "Expenses")
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColorTotal(accountType1.Class, yearModels.Percentage ?? 0, currentPeriod, PriorPeriod);
                    }
                    else
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColor(accountType1.Class, yearModels.Percentage ?? 0);
                    }
                    currentPeriod = PriorPeriod;
                }
                i++;

                lstYearModels.Add(yearModels);
            }
            return lstYearModels;
        }
        private string AssignColor(string lType, decimal? percentage, bool changeAssetColor = false)
        {
            if (lType == "Assets" & changeAssetColor == true)
                return "red";
            if (percentage == 0)
                return "green";
            var adata = "Liabilities,Expenses".Split(',').ToList().Contains(lType) ? percentage > 0 ? "red" : "green"
                 : "Assets,Income,Equity".Split(',').ToList().Contains(lType) ? percentage > 0 ? "green" : "red" : null;
            return adata;
        }
        private string AssignColorTotal(string value, decimal? percentage, decimal? currentvalue, decimal? priorvalue)
        {
            if (currentvalue < priorvalue || percentage == 0)
                return "green";
            return "red";
        }
        private string AssignNewColor(string accountClass, decimal? percentage)
        {
            string color = null;
            if (percentage == 0)
                color = "green";
            else
                color = "Liabilities,Expenses".Split(',').ToList().Contains(accountClass) ? percentage > 0 ? "green" : "red"
                    : "Assets,Income,Equity".Split(',').ToList().Contains(accountClass) ? percentage > 0 ? "red" : "green" : color;
            return color;
        }

        private static void FillGroupType(AccountNewModel accountNewModel)
        {
            if (accountNewModel.GroupType == ControlCodeConstants.ProfitbeforeTax)
            {
                if (accountNewModel.Class == ControlCodeConstants.Expenses)
                {
                    accountNewModel.GroupType = ControlCodeConstants.TotalExpenses;
                }
                else if (accountNewModel.Class == ControlCodeConstants.Income)
                {
                    accountNewModel.GroupType = ControlCodeConstants.TotalOtherIncome;
                }
            }
        }
        private static int GetPeriodBasedonType(Models.V3.CommonModel commonModel, int Period)
        {
            if (commonModel.Period == ControlCodeConstants.Monthly)
                Period = 3;
            else if (commonModel.Period == ControlCodeConstants.Quarterly)
                Period = 2;
            else if (commonModel.Period == ControlCodeConstants.SemiAnnually)
                Period = 1;
            else if (commonModel.Period == ControlCodeConstants.Annually)
                Period = 0;
            return Period;
        }
        private static void BuildColumnsHTMLDataNew(Models.V3.IncomeStatementModel incomeStatementModel, List<string> ColumnList, Models.V3.CommonModel commonModel)
        {
            Dictionary<string, string> columnList = new Dictionary<string, string>();
            columnList.Add("AccountName", "");

            foreach (var item in ColumnList)
            {
                if (item.Contains("Vs"))
                    columnList.Add(item, "% Change");
                else
                    columnList.Add(item, item);
            }
            List<Models.V3.ColumnLsts> columns = new List<Models.V3.ColumnLsts>();
            foreach (var lst in columnList)
            {
                ColumnLsts column = new ColumnLsts();
                column.Column = lst.Key;

                if (lst.Value.Contains("To"))
                {
                    column.HtmlData = lst.Value.Split('o').Select(c => c).LastOrDefault().Trim();
                }
                else
                {
                    column.HtmlData = lst.Value;
                }

                if (lst.Key == "AccountName" || lst.Key == "Code")
                {
                    column.IsAmount = false;
                }
                else
                {
                    column.IsAmount = true;
                }
                columns.Add(column);
            }
            incomeStatementModel.ColumnLists = columns;


            if (commonModel.Frequency != 1)
            {
                int index = 0;
                bool isFirst = true;
                List<ColumnLsts> lstClmList = new List<ColumnLsts>();
                foreach (var item in incomeStatementModel.ColumnLists)
                {
                    if (item.Column.Contains("Vs"))
                    {
                        if (isFirst)
                        {
                            lstClmList.Insert(index - 2, item);
                            isFirst = false;
                        }
                        else
                            lstClmList.Insert(index - 1, item);
                    }
                    else
                        lstClmList.Add(item);
                    index++;
                }
                incomeStatementModel.ColumnLists = lstClmList;
            }
        }
        #endregion

        public NewStatementModel GetNewBalanceSheet(CommonModel commonModel)
        {
            //var res = connection.Split(';');
            //var serverName = res[0].Split('=')[1];
            //var AppicationIntest = res[2].Split('=')[1];

            //Dictionary<string, Object> parms = new Dictionary<string,Object>();
            //parms.Add("Object", commonModel);
            //var serializeparms = JsonConvert.SerializeObject(parms);
            //CommonObjModel commonObjModel = new CommonObjModel
            //{
            //    CompanyId = companyId,
            //    Params = serializeparms,
            //    ServerName = serverName,
            //    ApplicationIntest = AppicationIntest,
            //    MethodName = "Journal-GetNewBalanceSheet",
            //};
            //string secondarydboj = JsonConvert.SerializeObject(commonObjModel);

            // LoggingHelper.LogMessage(BeanLogConstant.ReportsReadOnlyApplicationService, secondarydboj);
            NewStatementModel finalBalanceSheetModel = new NewStatementModel();
            int Period = 0;
            Period = GetPeriodBasedonType(commonModel, Period);
            int SamePeriod = commonModel.SamePeriod ? 1 : 0;

            var lstAccountsData = _journalServiceV3.GetAllAccountsBy_Bean_HTMLBalanceSheetSP(commonModel.CompanyId, commonModel.CompanyName, commonModel.Fromdate, (int)commonModel.Frequency, Period, SamePeriod);

            var lstleadsheet = _journalServiceV3.GetAllAccounyTypeByCompanyId(commonModel.CompanyId).Where(c => c.Category == ControlCodeConstants.BalanceSheet).ToList();


            //KG Enhancement for clearing - 16/05/2020        
            Dictionary<long, long?> lstOfIntercoAccounts = _journalServiceV3.GetListChartofAccounts(lstleadsheet.Where(a => a.Name == COANameConstants.Intercompany_billing || a.Name == COANameConstants.Intercompany_clearing).Select(c => c.Id).ToList(), commonModel.CompanyId, commonModel.CompanyName.Split(',').Select(long.Parse).ToList());


            //** fr dynamic colomns
            var accountName = lstAccountsData.Select(c => c.Name).FirstOrDefault();
            var ColumnList = lstAccountsData.Where(c => c.Name == accountName).OrderBy(c => c.Recorder).Select(c => c.Year).ToList();
            NewBuildColoumnsHTMLData(finalBalanceSheetModel, ColumnList, commonModel);
            lstleadsheet = lstleadsheet.OrderBy(c => c.RecOrder).ToList();


            List<string> lstStrings = new List<string>();
            lstStrings.Add(ControlCodeConstants.Assets);
            lstStrings.Add(ControlCodeConstants.Liabilities);
            lstStrings.Add(ControlCodeConstants.Equity);
            lstleadsheet = lstleadsheet.OrderBy(d => lstStrings.IndexOf(d.Class)).ThenBy(x => x.SubCategory).ThenBy(v => v.RecOrder).ToList();

            List<AccountNewModel> lstAccoutNewModel = new List<AccountNewModel>();
            int rec = 1;
            foreach (var accountType in lstleadsheet)
            {
                var lstChartOfAccounts = lstAccountsData.Where(c => c.FRPATId == accountType.FRATId).ToList();
                lstChartOfAccounts = lstChartOfAccounts.DistinctBy(c => c.FRCoaId).ToList();
                foreach (var coaAccout in lstChartOfAccounts)
                {
                    AccountNewModel accountNewModel = new AccountNewModel();
                    accountNewModel.AccountName = coaAccout.Name;
                    accountNewModel.AccountType = accountType.Name;
                    accountNewModel.Type = ControlCodeConstants.Item;
                    accountNewModel.Class = accountType.SubCategory;
                    accountNewModel.GroupType = accountType.Class;
                    accountNewModel.Recorder = rec;
                    accountNewModel.GroupHeading = accountType.SubCategory != ControlCodeConstants.Equity ? accountType.SubCategory + " " + accountType.Class : ControlCodeConstants.Equity;

                    var accounts = lstAccountsData.Where(c => c.Name == coaAccout.Name).OrderBy(c => c.Code).ToList();
                    accountNewModel.YearModels = AccountTotalsBalSheet(ColumnList, accounts, accountType);
                    var amount = accountNewModel.YearModels.Where(x => x.Balance != null).Sum(c => c.Balance);
                    accountNewModel.IsShowZero = (amount == null || amount == 0);

                    accountNewModel.IsInterco = (accountType.Name == COANameConstants.Intercompany_billing || accountType.Name == COANameConstants.Intercompany_clearing) && lstOfIntercoAccounts.Any() ? lstOfIntercoAccounts.Any(a => a.Key == coaAccout.CoaId) : false;

                    lstAccoutNewModel.Add(accountNewModel);
                    rec++;
                }
                AccountNewModel accountTypeModel = new AccountNewModel();
                accountTypeModel.AccountName = ControlCodeConstants.Total;
                accountTypeModel.AccountType = accountType.Name;
                accountTypeModel.Type = ControlCodeConstants.Total;
                accountTypeModel.Class = accountType.SubCategory;
                accountTypeModel.GroupType = accountType.Class;
                accountTypeModel.GroupHeading = accountType.SubCategory != ControlCodeConstants.Equity ? accountType.SubCategory + " " + accountType.Class : ControlCodeConstants.Equity;
                //accountTypeModel.YearModels = AccountTypeTotalYearModels(lstAccoutNewModel, ColumnList, accountType);
                accountTypeModel.YearModels = AccountTypeTotals(ColumnList, lstAccoutNewModel, accountType);
                var amountTotal = lstAccoutNewModel.Where(c => c.AccountType == accountType.Name && c.Type == ControlCodeConstants.Item).SelectMany(c => c.YearModels).Count(x => x.Balance != null && x.Balance != 0);
                accountTypeModel.IsShowZero = amountTotal == 0;
                accountTypeModel.Recorder = rec;
                lstAccoutNewModel.Add(accountTypeModel);
                rec++;
            }

            var currentAssetsAccounts = lstAccoutNewModel.Where(c => c.GroupType == ControlCodeConstants.Assets && c.Class == ControlCodeConstants.Current && c.Type == ControlCodeConstants.Item).ToList();
            var nonCurrentAssetsAccounts = lstAccoutNewModel.Where(c => c.GroupType == ControlCodeConstants.Assets && c.Class == ControlCodeConstants.Noncurrent && c.Type == ControlCodeConstants.Item).ToList();
            var currentLibilitiesAccounts = lstAccoutNewModel.Where(c => c.GroupType == ControlCodeConstants.Liabilities && c.Class == ControlCodeConstants.Current && c.Type == ControlCodeConstants.Item).ToList();
            var nonCurrentLibilitiesAccounts = lstAccoutNewModel.Where(c => c.GroupType == ControlCodeConstants.Liabilities && c.Class == ControlCodeConstants.Noncurrent && c.Type == ControlCodeConstants.Item).ToList();
            var equityAccounts = lstAccoutNewModel.Where(c => c.GroupType == ControlCodeConstants.Equity && c.Class == ControlCodeConstants.Equity && c.Type == ControlCodeConstants.Item).ToList();



            FillTotals("Total Current Assets", currentAssetsAccounts, ControlCodeConstants.Current + " " + ControlCodeConstants.Assets, ControlCodeConstants.Assets, ControlCodeConstants.Current, ControlCodeConstants.Total, ColumnList, lstAccoutNewModel, lstAccoutNewModel.Count(c => c.GroupType == ControlCodeConstants.Assets && c.Class == ControlCodeConstants.Current));
            FillTotals("Total Non-Current Assets", nonCurrentAssetsAccounts, ControlCodeConstants.Noncurrent + " " + ControlCodeConstants.Assets, ControlCodeConstants.Assets, ControlCodeConstants.Noncurrent, ControlCodeConstants.Total, ColumnList, lstAccoutNewModel, lstAccoutNewModel.Count(c => c.GroupType == ControlCodeConstants.Assets));
            //total Assets

            FillTotals("Total Assets", lstAccoutNewModel.Where(c => c.Type == ControlCodeConstants.Item && c.GroupType == ControlCodeConstants.Assets).ToList(), "Total Assets", ControlCodeConstants.Assets, null, ControlCodeConstants.Total, ColumnList, lstAccoutNewModel, lstAccoutNewModel.Count(c => c.GroupType == ControlCodeConstants.Assets));


            FillTotals("Total Current Liabilities", currentLibilitiesAccounts, ControlCodeConstants.Current + " " + ControlCodeConstants.Liabilities, ControlCodeConstants.Liabilities, ControlCodeConstants.Current, ControlCodeConstants.Total, ColumnList, lstAccoutNewModel, lstAccoutNewModel.Count(c => c.GroupType == ControlCodeConstants.Assets) + lstAccoutNewModel.Count(c => c.GroupType == ControlCodeConstants.Liabilities && c.Class == ControlCodeConstants.Current));
            FillTotals("Total Non-Current Liabilities", nonCurrentLibilitiesAccounts, ControlCodeConstants.Noncurrent + " " + ControlCodeConstants.Liabilities, ControlCodeConstants.Liabilities, ControlCodeConstants.Noncurrent, ControlCodeConstants.Total, ColumnList, lstAccoutNewModel, lstAccoutNewModel.Count(c => c.GroupType == ControlCodeConstants.Assets) + lstAccoutNewModel.Count(c => c.GroupType == ControlCodeConstants.Liabilities));
            //total Liabilities
            FillTotals("Total Liabilities", lstAccoutNewModel.Where(c => c.Type == ControlCodeConstants.Item && c.GroupType == ControlCodeConstants.Liabilities).ToList(), "Total Liabilities", ControlCodeConstants.Liabilities, null, ControlCodeConstants.Total, ColumnList, lstAccoutNewModel, lstAccoutNewModel.Count(c => c.GroupType == ControlCodeConstants.Assets || c.GroupType == ControlCodeConstants.Liabilities));

            //Net Assets
            FillSubTotalsTotals("Net Assets", lstAccoutNewModel.Where(c => c.Type == ControlCodeConstants.Item && (c.GroupType == ControlCodeConstants.Liabilities || c.GroupType == ControlCodeConstants.Assets)).ToList(), "Net Assets", ControlCodeConstants.Assets, ControlCodeConstants.GrandTotal, ControlCodeConstants.GrandTotal, ColumnList, lstAccoutNewModel, lstAccoutNewModel.Count(c => c.GroupType == ControlCodeConstants.Liabilities || c.GroupType == ControlCodeConstants.Assets));

            FillTotals("Total Equity", equityAccounts, ControlCodeConstants.Equity, ControlCodeConstants.Equity, ControlCodeConstants.Equity, ControlCodeConstants.GrandTotal, ColumnList, lstAccoutNewModel, lstAccoutNewModel.Count());

            finalBalanceSheetModel.ListAccountNewModel = lstAccoutNewModel;

            return finalBalanceSheetModel;


        }
        private List<YearModels> AccountTotalsBalSheet(List<string> ColumnList, List<BalanceSheetSpModel> lstAccoutNewModel, AccountTypeV3 accountType1)
        {
            List<YearModels> lstYearModels = new List<YearModels>();
            decimal? currentPeriod = 0;
            decimal? PriorPeriod = 0;
            int i = 0;

            foreach (var colom in ColumnList)
            {
                YearModels yearModels = new YearModels();
                yearModels.Year = colom;
                yearModels.Balance = lstAccoutNewModel.Where(c => c.Year == colom).Select(c => c.Balance).FirstOrDefault();

                yearModels.Percentage = 0;
                if (i == 0)
                    currentPeriod = yearModels.Balance;
                else if (i == 1)
                    PriorPeriod = yearModels.Balance;
                if (colom.Contains("Vs"))
                {
                    i = 0;
                    yearModels.IsPercentage = true;

                    if (PriorPeriod == 0)
                    {
                        yearModels.Percentage = null;
                        yearModels.FontColor = null;
                    }
                    else if (currentPeriod < 0 && PriorPeriod < 0)
                    {
                        var PriorPeriodValue = PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod;
                        yearModels.Percentage = ((currentPeriod * -1) - PriorPeriodValue) / PriorPeriodValue * 100;
                        yearModels.FontColor = AssignNewColor(accountType1.Class, yearModels.Percentage);
                    }
                    else if (accountType1.Class == "Expenses")
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColorTotal(accountType1.Class, yearModels.Percentage ?? 0, currentPeriod, PriorPeriod);
                    }
                    else
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColor(accountType1.Class, yearModels.Percentage ?? 0);
                    }
                    currentPeriod = PriorPeriod;
                }
                i++;

                lstYearModels.Add(yearModels);
            }
            return lstYearModels;
        }

        private void NewBuildColoumnsHTMLData(NewStatementModel finalBalanceSheetModel, List<string> ColumnList, CommonModel commonModel)
        {
            Dictionary<string, string> columnList = new Dictionary<string, string>();
            columnList.Add("AccountName", "");

            foreach (var item in ColumnList)
            {
                if (item.Contains("Vs"))
                    columnList.Add(item, "% Change");
                else
                    columnList.Add(item, item);
            }

            List<ColumnLsts> columns = new List<ColumnLsts>();
            foreach (var lst in columnList)
            {
                ColumnLsts column = new ColumnLsts();
                column.Column = lst.Key;

                if (lst.Value.Contains("To"))
                {
                    column.HtmlData = lst.Value.Split('o').Select(c => c).LastOrDefault().Trim();
                }
                else
                {
                    column.HtmlData = lst.Value;
                }


                if (lst.Key == "AccountName" || lst.Key == "Code")
                {
                    column.IsAmount = false;
                }
                else
                {
                    column.IsAmount = true;
                }
                columns.Add(column);
            }
            finalBalanceSheetModel.ColumnLists = columns;
            if (commonModel.Frequency != 1)
            {
                int index = 0;
                bool isFirst = true;
                List<ColumnLsts> lstClmList = new List<ColumnLsts>();
                foreach (var item in finalBalanceSheetModel.ColumnLists)
                {
                    if (item.Column.Contains("Vs"))
                    {
                        if (isFirst)
                        {
                            lstClmList.Insert(index - 2, item);
                            isFirst = false;
                        }
                        else
                            lstClmList.Insert(index - 1, item);
                    }
                    else
                        lstClmList.Add(item);
                    index++;
                }
                finalBalanceSheetModel.ColumnLists = lstClmList;
            }
        }

        private List<AccountNewModel> FillTotals(string HeadingName, List<AccountNewModel> lstAccoutNewModel, string groupHeading, string accountClass, string subCategory, string type, List<string> coloumnList, List<AccountNewModel> lstAccoutNewFinalModel,
           int IndexCount)
        {
            AccountNewModel accountTypeModel = new AccountNewModel();
            accountTypeModel.AccountName = HeadingName;
            accountTypeModel.AccountType = accountClass;
            accountTypeModel.Type = type;
            accountTypeModel.Class = subCategory;
            accountTypeModel.GroupType = accountClass;
            accountTypeModel.GroupHeading = groupHeading;

            List<YearModels> lstYearModels = new List<YearModels>();
            decimal? currentPeriod = 0;
            decimal? PriorPeriod = 0;
            int i = 0;
            foreach (var colom in coloumnList)
            {
                YearModels yearModels = new YearModels();
                yearModels.Year = colom;
                yearModels.Balance = lstAccoutNewModel.SelectMany(c => c.YearModels).ToList().Where(v => v.Year == colom).Sum(x => x.Balance) ?? 0;
                if (i == 0)
                    currentPeriod = yearModels.Balance;
                else if (i == 1)
                    PriorPeriod = yearModels.Balance;
                if (colom.Contains("Vs"))
                {
                    i = 0;
                    yearModels.IsPercentage = true;

                    if (PriorPeriod == 0)
                    {
                        yearModels.Percentage = null;
                        yearModels.FontColor = null;
                    }
                    else if (currentPeriod < 0 && PriorPeriod < 0)
                    {
                        var PriorPeriodValue = PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod;
                        yearModels.Percentage = ((currentPeriod * -1) - PriorPeriodValue) / PriorPeriodValue * 100;
                        yearModels.FontColor = AssignNewColor(accountClass, yearModels.Percentage);
                    }
                    else if (accountClass == "Expenses")
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColorTotal(accountClass, yearModels.Percentage ?? 0, currentPeriod, PriorPeriod);
                    }
                    else
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColor(accountClass, yearModels.Percentage ?? 0);
                    }
                    currentPeriod = PriorPeriod;
                }
                i++;
                lstYearModels.Add(yearModels);
            }

            accountTypeModel.YearModels = lstYearModels;
            var amountTotal = accountTypeModel.YearModels.Where(x => x.Balance != null).Sum(c => c.Balance);
            accountTypeModel.IsShowZero = (amountTotal == null || amountTotal == 0) ? true : false;
            lstAccoutNewFinalModel.Insert(IndexCount, accountTypeModel);
            return lstAccoutNewFinalModel;
        }

        private List<AccountNewModel> FillSubTotalsTotals(string HeadingName, List<AccountNewModel> lstAccoutNewModel, string groupHeading, string accountClass, string subCategory, string type, List<string> coloumnList, List<AccountNewModel> lstAccoutNewFinalModel, int IndexCount)
        {
            AccountNewModel accountTypeModel = new AccountNewModel();
            accountTypeModel.AccountName = HeadingName;
            accountTypeModel.AccountType = accountClass;
            accountTypeModel.Type = type;
            accountTypeModel.Class = subCategory;
            accountTypeModel.GroupType = accountClass;
            accountTypeModel.GroupHeading = groupHeading;

            List<YearModels> lstYearModels = new List<YearModels>();
            decimal? currentPeriod = 0;
            decimal? PriorPeriod = 0;
            int i = 0;
            foreach (var colom in coloumnList)
            {
                YearModels yearModels = new YearModels();
                yearModels.Year = colom;
                var assetsBalance = lstAccoutNewModel.Where(c => c.GroupType == "Assets").SelectMany(c => c.YearModels).ToList().Where(v => v.Year == colom).Sum(x => x.Balance) ?? 0;
                var liabilitiesBal = lstAccoutNewModel.Where(c => c.GroupType == "Liabilities").SelectMany(c => c.YearModels).ToList().Where(v => v.Year == colom).Sum(x => x.Balance) ?? 0;
                yearModels.Balance = assetsBalance - liabilitiesBal;
                yearModels.Percentage = 0;
                bool changeAssetColor = false;
                if (i == 0)
                    currentPeriod = yearModels.Balance;
                else if (i == 1)
                    PriorPeriod = yearModels.Balance;
                if (colom.Contains("Vs"))
                {
                    i = 0;
                    yearModels.IsPercentage = true;

                    if (PriorPeriod == 0)
                    {
                        yearModels.Percentage = null;
                        yearModels.FontColor = null;
                    }
                    else if (currentPeriod < 0 && PriorPeriod < 0)
                    {
                        var PriorPeriodValue = PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod;
                        yearModels.Percentage = ((currentPeriod * -1) - PriorPeriodValue) / PriorPeriodValue * 100;
                        yearModels.FontColor = AssignNewColor(accountClass, yearModels.Percentage);
                    }
                    else if (accountClass == "Expenses")
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColorTotal(accountClass, yearModels.Percentage ?? 0, currentPeriod, PriorPeriod);
                    }
                    else
                    {
                        yearModels.Percentage = (currentPeriod - PriorPeriod) / (PriorPeriod < 0 ? PriorPeriod * -1 : PriorPeriod) * 100;
                        yearModels.FontColor = AssignColor(accountClass, yearModels.Percentage ?? 0);
                    }
                    currentPeriod = PriorPeriod;
                }
                i++;
                lstYearModels.Add(yearModels);
            }

            accountTypeModel.YearModels = lstYearModels;
            var amountTotal = accountTypeModel.YearModels.Where(x => x.Balance != null).Sum(c => c.Balance);
            accountTypeModel.IsShowZero = (amountTotal == null || amountTotal == 0) ? true : false;
            lstAccoutNewFinalModel.Insert(IndexCount, accountTypeModel);
            return lstAccoutNewFinalModel;
        }
    }
}
