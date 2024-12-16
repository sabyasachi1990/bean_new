using AppsWorld.CashSalesModule.Entities.V2;
using AppsWorld.CashSalesModule.RepositoryPattern.V2;
using Repository.Pattern.Ef6;
using System.Configuration;
using System.Data.Entity;
using Ziraff.FrameWork;

namespace AppsWorld.CashSalesModule.Entities.Context.V2
{
    public partial class CashSaleKContext : DataContext, ICashSalesKModuleDataContextAsync
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
        //public CashSaleKContext() : base(ConnectionString)
        //{
        //    Database.SetInitializer<CashSaleKContext>(null);
        //}
        static CashSaleKContext()
        {
            Database.SetInitializer<CashSaleKContext>(null);
        }
        public CashSaleKContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<CashSaleK> CashSaleKs { get; set; }
        public DbSet<ChartOfAccountK> ChartsOfAccountKs { get; set; }
        public DbSet<CompanyUserK> CompanyUserKs { get; set; }
        public DbSet<CompanyK> CompanyKs { get; set; }
        public DbSet<BeanEntityK> BeanEntityKs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CashSaleKMap());
            modelBuilder.Configurations.Add(new ChartOfAccountKMap());
            modelBuilder.Configurations.Add(new CompanyUserKMap());
            modelBuilder.Configurations.Add(new CompanyKMap());
            modelBuilder.Configurations.Add(new BeanEntityKMap());
        }
    }
}
