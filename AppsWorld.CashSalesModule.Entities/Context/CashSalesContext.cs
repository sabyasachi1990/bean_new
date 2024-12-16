using AppsWorld.CashSalesModule.Entities.Models;
using AppsWorld.CashSalesModule.Entities.Models.Mappings;
using AppsWorld.CashSalesModule.RepositoryPattern;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Entities.Mapping;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ziraff.FrameWork;

namespace AppsWorld.CashSalesModule.Entities.Context
{
    public partial class CashSalesContext : DataContext, ICashSalesModuleDataContextAsync
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
        //public CashSalesContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<CashSalesContext>(null);
        //}
        static CashSalesContext()
        {
            Database.SetInitializer<CashSalesContext>(null);
        }
        public CashSalesContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<CashSale> CashSales { get; set; }
        public DbSet<CashSaleDetail> CashSaleDetails { get; set; }
        public DbSet<BeanEntity> BeanEntities { get; set; }
        //public DbSet<GSTDetail> GSTDetails { get; set; }
        public DbSet<AppsWorld.CashSalesModule.Entities.Models.AutoNumber> AutoNumbers { get; set; }
        public DbSet<AppsWorld.CashSalesModule.Entities.Models.AutoNumberCompany> AutoNumberCompanys { get; set; }

        public DbSet<AppsWorld.CommonModule.Entities.Company> Companys { get; set; }

        public DbSet<AppsWorld.CommonModule.Entities.ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CashSaleMap());
            modelBuilder.Configurations.Add(new CashSaleDetailMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            //modelBuilder.Configurations.Add(new GSTDetailMap());
            modelBuilder.Configurations.Add(new AppsWorld.CashSalesModule.Entities.Models.Mappings.AutoNumberMap());
            modelBuilder.Configurations.Add(new AppsWorld.CashSalesModule.Entities.Models.Mappings.AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Mapping.CompanyMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Mapping.ChartOfAccountMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
        }

    }
}
