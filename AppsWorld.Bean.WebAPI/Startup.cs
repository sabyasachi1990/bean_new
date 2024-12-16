using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Web.Http;
using Domain.Events.Autofac;
using Domain.Events.Autofac.Extensions;
using Autofac;
using Domain.Events;
using System.Configuration;
using Hangfire;
using Ziraff.Section;
using Ziraff.FrameWork;
using Hangfire.Dashboard;
using IdentityServer3.AccessTokenValidation;

[assembly: OwinStartup(typeof(AppsWorld.Bean.WebAPI.Startup))]

namespace AppsWorld.Bean.WebAPI
{
    public partial class Startup
    {
        //public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        //public static string PublicClientId { get; private set; }
        //private string ConnectionString { get; set; }

        public void Configuration(IAppBuilder app)
        {
            //ConnectionString = KeyVaultService.GetSecret(
            //ConfigurationManager.AppSettings["HangFireContextClientId"],
            //     ConfigurationManager.AppSettings["HangFireContextClientSecret"],
            //     ConfigurationManager.AppSettings["HangFireContextKeySecretUri"]);//commented by lokanath on 28/09/2019
            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsAuthorize"]))
            {
                //System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                ConfigureAuth(app);
            }

            // Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings[ConnectionString].ToString()); 

            //Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage(ConnectionString);//commented by lokanath on 28/09/2019

            //app.UseHangfireDashboard("hangfire", new DashboardOptions
            //{
            //    AuthorizationFilters = Enumerable.Empty<IAuthorizationFilter>()
            //});

            //app.UseHangfireDashboard();
            //app.UseHangfireServer();//commented by lokanath on 28/09/2019

        }

       

        public void ConfigureAuth(IAppBuilder app)
        {
            string strAuthority = ConfigurationManager.AppSettings["Authority"].ToString();
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = strAuthority,
                RequiredScopes = new string[] { AuthConstant.Role, AuthConstant.CompanyId },
                //RequiredScopes = new[] { "openid" },
                //AuthenticationMode =Microsoft.Owin.Security.AuthenticationMode.Active
            });
        }


        //Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["HangFireContext"].ToString());
        //app.UseHangfireDashboard();
        //app.UseHangfireServer();
        //RecurringJob.AddOrUpdate(() => new AppsWorld.CommonModule.Service.HangfireService().CreateDailyAttendance(), "05 00 * * *", TimeZoneInfo.Local);
        //RecurringJob.AddOrUpdate(() => new AppsWorld.LeaveManagementModule.Service.HangfireService().GetProratedLeaves(), "05 00 * * *", TimeZoneInfo.Local);
        //RecurringJob.AddOrUpdate(() => new AppsWorld.PaymentModule.Service.HangfireService().GetProratedLeaves(), Cron.Monthly, TimeZoneInfo.Local);
        //RecurringJob.AddOrUpdate(() => new AppsWorld.JournalVoucherModule.Application.JournalApplicationService.HangfireService().ArchieveleaveEntitlements(), Cron.Daily, TimeZoneInfo.Local);
        //  RecurringJob.AddOrUpdate(()=> new AppsWorld.JournalVoucherModule.Service.HangfireService().

    }
}
