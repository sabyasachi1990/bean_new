using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankWithdrawalModule.Entities;
using AppsWorld.BankWithdrawalModule.Entities.Mapping;
using AppsWorld.BankWithdrawalModule.RepositoryPattern;
using Repository.Pattern.Ef6;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Entities.Mapping;
using Ziraff.FrameWork;
using System.Configuration;

namespace AppsWorld.BankWithdrawalModule.Entities
{
    public partial class BankWithdrawalContext : DataContext, IBankWithdrawalModuleDataContextAsync
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
        //public BankWithdrawalContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<BankWithdrawalContext>(null);
        //}
        static BankWithdrawalContext()
        {
            Database.SetInitializer<BankWithdrawalContext>(null);
        }
        public BankWithdrawalContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Withdrawal> Withdrawals { get; set; }
        public DbSet<WithdrawalDetail> WithdrawalDetails { get; set; }
        public DbSet<BeanEntity> BeanEntities { get; set; }
        public DbSet<TaxCode> TaxCodes { get; set; }
        public DbSet<GSTDetail> GSTDetails { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<AppsWorld.BankWithdrawalModule.Entities.AutoNumber> AutoNumbers { get; set; }
        public DbSet<AppsWorld.BankWithdrawalModule.Entities.AutoNumberCompany> AutoNumberCompanys { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new WithdrawalMap());
            modelBuilder.Configurations.Add(new WithdrawalDetailMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new TaxCodeMap());
            modelBuilder.Configurations.Add(new GSTDetailMap());
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new AppsWorld.BankWithdrawalModule.Entities.Mapping.AutoNumberMap());
            modelBuilder.Configurations.Add(new AppsWorld.BankWithdrawalModule.Entities.Mapping.AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());

        }
    }
}
