namespace AppsWorld.RevaluationModule.Entities.V2
{
    using System.Data.Entity;
    using AppsWorld.RevaluationModule.Entities.V2;
    using AppsWorld.RevaluationModule.RepositoryPattern.V2;
    using Repository.Pattern.Ef6;
    using Ziraff.FrameWork;
    using System.Configuration;
    using Models.V2;
    using Models.V2.Mappings;

    public partial class RevaluationContext : DataContext, IRevaluationDataContextAsync
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
        //public RevaluationContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<RevaluationContext>(null);
        //}
        static RevaluationContext()
        {
            Database.SetInitializer<RevaluationContext>(null);
        }
        public RevaluationContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Revaluation> Revalutions { get; set; }
        public DbSet<CompanyCompact> Companies { get; set; }
        public DbSet<CompanyUserCompact> CompanyUsers { get; set; }
        public DbSet<AutoNumberCompact> AutoNumbers { get; set; }
        public DbSet<AutoNumberCompanyCompact> AutoNumberCompanies { get; set; }
        public DbSet<RevalutionDetail> RevalutionDetails { get; set; }
        public DbSet<JournalDetailCompact> JournalDetails { get; set; }
        public DbSet<BeanEntityCompact> Entities { get; set; }
        public DbSet<FinancialSettingCompact> FinancialSettings { get; set; }
        public DbSet<ChartOfAccountCompact> ChartOfAccounts { get; set; }
        public DbSet<MultiCurrencySettingCompact> MultiCurrecies { get; set; }
        public DbSet<JournalCompact> Journals { get; set; }
        public DbSet<CommonForex> CommonForexs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RevalutionMap());
            modelBuilder.Configurations.Add(new CompanyCompactMap());
            modelBuilder.Configurations.Add(new CompanyUserCompactMap());
            modelBuilder.Configurations.Add(new AutoNumberCompactMap());
            modelBuilder.Configurations.Add(new AutoNumberCompanyCompactMap());
            modelBuilder.Configurations.Add(new RevalutionDetailMap());
            modelBuilder.Configurations.Add(new JournalDetailCompactMap());
            modelBuilder.Configurations.Add(new BeanEntityCompactMap());
            modelBuilder.Configurations.Add(new FinancialSettingCompactMap());
            modelBuilder.Configurations.Add(new ChartOfAccountCompactMap());
            modelBuilder.Configurations.Add(new MultiCurrencySettingCompactMap());
            modelBuilder.Configurations.Add(new JournalCompactMap());
            modelBuilder.Configurations.Add(new CommonForexMap());
        }
    }
}
