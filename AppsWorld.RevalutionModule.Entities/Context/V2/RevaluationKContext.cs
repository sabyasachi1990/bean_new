namespace AppsWorld.RevaluationModule.Entities.V2
{
    using System.Data.Entity;
    using AppsWorld.RevaluationModule.Entities.V2;
    using AppsWorld.RevaluationModule.RepositoryPattern.V2;
    using Repository.Pattern.Ef6;
    using Ziraff.FrameWork;
    using System.Configuration;

    public partial class RevaluationKContext : DataContext, IRevaluationKDataContextAsync
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
        //public RevaluationKContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<RevaluationContext>(null);
        //}
        static RevaluationKContext()
        {
            Database.SetInitializer<RevaluationKContext>(null);
        }
        public RevaluationKContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<RevaluationK> Revalutions { get; set; }
        public DbSet<CompanyCompact> Companies { get; set; }
        public DbSet<CompanyUserCompact> CompanyUsers { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RevalutionKMap());
            modelBuilder.Configurations.Add(new CompanyCompactMap());
            modelBuilder.Configurations.Add(new CompanyUserCompactMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
        }
    }
}
