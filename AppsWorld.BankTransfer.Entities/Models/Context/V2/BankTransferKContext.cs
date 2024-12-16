using AppsWorld.BankTransferModule.RepositoryPattern.V2;
using Repository.Pattern.Ef6;
using System.Configuration;
using System.Data.Entity;
using Ziraff.FrameWork;

namespace AppsWorld.BankTransferModule.Entities.V2
{
    public partial class BankTransferKContext : DataContext, ITransferKDataContextAsync
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
        //public BankTransferKContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<BankTransferKContext>(null);
        //}

        static BankTransferKContext()
        {
            Database.SetInitializer<BankTransferKContext>(null);
        }
        public BankTransferKContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<BankTransferK> BankTransfers{ get; set; }
        public DbSet<BankTransferDetailK> BankTransferDetails { get; set; }
        public DbSet<CompanyCompact> Companies { get; set; }
        public DbSet<ChartOfAccountCompact> ChartOfAccounts { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
            modelBuilder.Configurations.Add(new BankTransferDetailKMap());
            modelBuilder.Configurations.Add(new BankTransferKMap());
            modelBuilder.Configurations.Add(new CompanyCompactMap());
            modelBuilder.Configurations.Add(new ChartOfAccountCompactMap());
        }
    }
}
