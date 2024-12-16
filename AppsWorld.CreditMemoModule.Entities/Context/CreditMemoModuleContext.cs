using AppsWorld.CreditMemoModule.RepositoryPattern;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AppsWorld.CreditMemoModule.Entities.Mapping;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Entities.Mapping;
using Ziraff.FrameWork;
using System.Configuration;

namespace AppsWorld.CreditMemoModule.Entities
{
    public class CreditMemoModuleContext : DataContext, ICreditMemoModuleDataContextAsync
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
        //public CreditMemoModuleContext() : base(ConnectionString)
        //{
        //    Database.SetInitializer<CreditMemoModuleContext>(null);
        //}
        static CreditMemoModuleContext()
        {
            Database.SetInitializer<CreditMemoModuleContext>(null);
        }
        public CreditMemoModuleContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<CreditMemo> CreditMemoes { get; set; }
        public DbSet<CreditMemoApplication> CreditMemoApplications { get; set; }
        public DbSet<CreditMemoApplicationDetail> CreditMemoApplicationDetails { get; set; }
        public DbSet<CreditMemoDetail> CreditMemoDetails { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<TaxCode> TaxCodes { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<BeanEntity> BeanEntities { get; set; }
        public DbSet<AppsWorld.CreditMemoModule.Entities.AutoNumber> AutoNumbers { get; set; }
        public DbSet<AppsWorld.CreditMemoModule.Entities.AutoNumberCompany> AutoNumberCompanies { get; set; }
        public DbSet<AppsWorld.CreditMemoModule.Entities.JournalDetail> JournalDetails { get; set; }
        public DbSet<AppsWorld.CreditMemoModule.Entities.Journal> Journals { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CreditMemoMap());
            modelBuilder.Configurations.Add(new CreditMemoDetailMap());
            modelBuilder.Configurations.Add(new CreditMemoApplicationMap());
            modelBuilder.Configurations.Add(new CreditMemoApplicationDetailMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new TaxCodeMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new AppsWorld.CreditMemoModule.Entities.Mapping.AutoNumberMap());
            modelBuilder.Configurations.Add(new AppsWorld.CreditMemoModule.Entities.Mapping.AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new AppsWorld.CreditMemoModule.Entities.Mapping.JournalDetailMap());
            modelBuilder.Configurations.Add(new AppsWorld.CreditMemoModule.Entities.Mapping.JournalMap());
            modelBuilder.Configurations.Add(new BillMap());
            modelBuilder.Configurations.Add(new BillDetailMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
        }
    }
}
