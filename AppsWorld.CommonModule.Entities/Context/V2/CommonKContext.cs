namespace AppsWorld.CommonModule.Entities.V2
{
    using System.Data.Entity;
    using Repository.Pattern.Ef6;
    using AppsWorld.CommonModule.RepositoryPattern.V2;
    using Ziraff.FrameWork;
    using System.Configuration;

    public partial class CommonKContext : DataContext, ICommonKModuleDataContextAsync
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
        //public CommonKContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<CommonKContext>(null);

        //}
        static CommonKContext()
        {
            Database.SetInitializer<CommonKContext>(null);
        }
        public CommonKContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<ChartOfAccountK> ChartsOfAccountKs { get; set; }
        public DbSet<CompanyUserK> CompanyUserKs { get; set; }
        public DbSet<CompanyK> CompanyKs { get; set; }
        public DbSet<BeanEntityK> BeanEntityKs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ChartOfAccountKMap());
            modelBuilder.Configurations.Add(new CompanyUserKMap());
            modelBuilder.Configurations.Add(new CompanyKMap());
            modelBuilder.Configurations.Add(new BeanEntityKMap());
        }
    }
}
