using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Models.Resources
{
  public  class Constants
    {
        public const string Constants_FromDate = "@FromDate ";
        public const string Constants_ToDate = "@ToDate ";
        public const string Constants_Cid = "@CompanyId";
        public const string Constants_Line = "line";
        public const string Constants_Column = "column";
        public const string Constants_Role = "@Role";
        public const string Constants_Type = "@ByType";

        //FinancialDashBoard
        public const string Constants_FinancialDashBoard ="[GetFinancialsDashBoard]";
        public const string Constants_FinancialDetailDashBoard ="[GetFinancialsDetailDashBoard]";
        public const string Constants_FinancialScoreCard="[dbo].[GetFinancialsPLandBSScoreCard]";

        //FinanacialRatio KPI
        public const string Constants_FinancialsRatios="[GetFinancialRatiosDashBoard]";

        //BankDetailsDashBoard
        public const string Constants_BankDashBoard="[GetBankDashBoard]";
        public const string Constants_BankDetailsDashBoard ="[GetBankDetailDashBoard]";

        //CustomerBalance
        public const string Constants_CustomerBalances ="[dbo].[GETCustomerBalances]";
        public const string Constants_CustomerBalancesDetails ="[dbo].[GETCustomerBalancesDetails]";
        public const string Constants_Top30CustandVendorDetails ="[dbo].[GETTop30CustandVendorDetails]";

        //BankBalanceAdmin Dashboard
        public const string Constants_GetBankBalanceAdminDashBoard = "[GetBankBalanceAdminDashBoard]";

        //ProfitandLoss DashBoard
        public const string Constants_GetProfitandLossDashBoard = "[GetProfitandLossDashBoard]";

        //FinancialsAdminDashBoard
        public const string Constants_FinancialsAdminDashBoard = "[dbo].[FinancialsAdminDashBoard]";

        //AccountwatchListAdminDashBoard
        public const string Constants_AccountwatchListAdminDashBoard = "[dbo].[AccountwatchListAdminDashBoard ]";
        public const string Constants_AccountwatchListLeftDashBoard = "[dbo].[AccountwatchListLeftDashBoard]";
        public const string Constants_AccountwatchListRightDashBoard = "[dbo].[AccountwatchListRightDashBoard]";
        public const string Constants_AccountwatchListDd = "[dbo].[AccountwatchListDB]";
    }
}
