using AppsWorld.GLAccountModule.Models;
using AppsWorld.CommonModule.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;
using Serilog;
using AppsWorld.CommonModule.Application;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace AppsWorld.GLAccountModule.Application
{
    public class GLApplicationService
    {

        private readonly ICompanyService _companyService;
        private readonly IChartOfAccountService _chartOfAccountService;
        AuthInformation authInfo = GetAuthInfo(System.Web.HttpContext.Current.Request.Headers);

        private static AuthInformation GetAuthInfo(NameValueCollection headers)
        {
            return headers["AuthInformation"] != null ? JsonConvert.DeserializeObject<AuthInformation>(headers.GetValues("AuthInformation").FirstOrDefault().ToString()) : new AuthInformation();
        }
        public GLApplicationService(ICompanyService companyService, IChartOfAccountService chartOfAccountService)
        {
            this._companyService = companyService;
            this._chartOfAccountService = chartOfAccountService;
        }
        #region Create and Lookup Call
        public GlAccountLUs GetAccountLUs(long companyId, long subCompanyId)
        {
            GlAccountLUs accountLU = new GlAccountLUs();
            try
            {
                accountLU.SubsideryCompanyLU = _companyService.GetSubCompany(authInfo.userName, companyId, subCompanyId);
                //string AccountName = ClearingValidations.Balance_Sheet;
                //var account = _chartOfAccountService.GetAllBalanceSheet(companyId, AccountName);
                accountLU.ChartOfAccountLU = _chartOfAccountService.listOfChartOfAccounts(companyId, false);

            }
            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return accountLU;
        }
        #endregion
    }
}
