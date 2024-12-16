using AppsWorld.OpeningBalancesModule.RepositoryPattern;
using Repository.Pattern;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Entities.Mapping;
using Ziraff.FrameWork;
using System.Configuration;
using AppsWorld.OpeningBalancesModule.Entities.Models.Mappings;

namespace AppsWorld.OpeningBalancesModule.Entities
{
    public class OpeningBalancesContext : DataContext, IOpeningBalancesModuleDataContextAsync
    {
        //private static string _connectionString = null;
        //private static string ConnectionString
        //{
        //    get
        //    {
        //        if (_connectionString == null)
        //        {
        //            _connectionString = KeyVaultService.GetSecret(
        //        ConfigurationManager.AppSettings["AppsWorldDBContextClientId"],
        //        ConfigurationManager.AppSettings["AppsWorldDBContextClientSecret"],
        //        ConfigurationManager.AppSettings["AppsWorldDBContextKeySecretUri"]);
        //        }
        //        return _connectionString;
        //    }
        //}
        //public OpeningBalancesContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<OpeningBalancesContext>(null);
        //}
        static OpeningBalancesContext()
        {
            Database.SetInitializer<OpeningBalancesContext>(null);
        }
        public OpeningBalancesContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<OpeningBalance> OpeningBalances { get; set; }
        public DbSet<OpeningBalanceDetail> OpeningBalanceDetails { get; set; }
        public DbSet<OpeningBalanceDetailLineItem> OpeningBalanceDetailLineItems { get; set; }
        public DbSet<BeanEntity> BeanEntities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<AutoNumber> AutoNumbers { get; set; }
        public DbSet<AutoNumberCompany> AutoNumberCompanies { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<CreditMemo> CreditMemos { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }
        //public DbSet<BeanAutoNumber> beanAutos { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OpeningBalanceMap());
            modelBuilder.Configurations.Add(new OpeningBalanceDetailMap());
            modelBuilder.Configurations.Add(new OpeningBalanceDetailLineItemMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new AutoNumberMap());
            modelBuilder.Configurations.Add(new AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
            modelBuilder.Configurations.Add(new BillMap());
            modelBuilder.Configurations.Add(new CreditMemoMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
            //modelBuilder.Configurations.Add(new BeanAutoNumberMap());
        }
    }
}
