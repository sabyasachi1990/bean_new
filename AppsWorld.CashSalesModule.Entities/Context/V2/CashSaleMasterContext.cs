using AppsWorld.CashSalesModule.Entities.V2;
using AppsWorld.CashSalesModule.RepositoryPattern.V2;
using Repository.Pattern.Ef6;
using System.Configuration;
using System.Data.Entity;
using Ziraff.FrameWork;

namespace AppsWorld.CashSalesModule.Entities.Context.V2
{
    public partial class CashSaleMasterContext : DataContext, ICashSalesMasterDataContextAsync
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
        //public CashSaleMasterContext() : base(ConnectionString)
        //{
        //    Database.SetInitializer<CashSaleMasterContext>(null);
        //}
        static CashSaleMasterContext()
        {
            Database.SetInitializer<CashSaleMasterContext>(null);
        }
        public CashSaleMasterContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<CashSale> CashSales { get; set; }
        public DbSet<ChartOfAccountCompact> ChartsOfAccounts { get; set; }
        public DbSet<AutoNumberCompact> AutoNumbers { get; set; }
        public DbSet<FinancialSettingCompact> FinancialSettings { get; set; }
        public DbSet<TaxCodeCompact> TaxCodes { get; set; }
        public DbSet<AutoNumberComptCompany> AutoNumbersCompany { get; set; }
        public DbSet<BeanEntityCompact> BeanEntities { get; set; }
        public DbSet<ItemCompact> Items { get; set; }
        public DbSet<CompanyK> Companies { get; set; }
        public DbSet<CashSaleDetail> CashSaleDetails { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CashSaleMap());
            modelBuilder.Configurations.Add(new ChartOfAccountCompactMap());
            modelBuilder.Configurations.Add(new BeanEntityCompactMap());
            modelBuilder.Configurations.Add(new AutoNumberCompactMap());
            modelBuilder.Configurations.Add(new AutoNumberCompanyComptMap());
            modelBuilder.Configurations.Add(new ItemCompactMap());
            modelBuilder.Configurations.Add(new CompanyKMap());
            modelBuilder.Configurations.Add(new FinancialSettingCompactMap());
            modelBuilder.Configurations.Add(new TaxCodeCompactMap());
            modelBuilder.Configurations.Add(new CashSaleDetailMap());
        }
    }
}
