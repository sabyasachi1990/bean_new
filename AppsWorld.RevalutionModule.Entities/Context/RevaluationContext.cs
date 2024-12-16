namespace AppsWorld.RevaluationModule.Entities
{
    using System.Data.Entity;
    using AppsWorld.RevaluationModule.Entities.Models;
    using AppsWorld.RevaluationModule.Entities.Models.Mappings;
    using AppsWorld.RevaluationModule.RepositoryPattern;
    using Repository.Pattern.Ef6;
    using Ziraff.FrameWork;
    using System.Configuration;

    public partial class RevaluationContext : DataContext, IRevaluationModuleDataContextAsync
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
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<MultiCurrencySetting> MultiCurrencySettings { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<JournalDetail> JournalDetails { get; set; }
        public DbSet<BeanEntity> Entities { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Forex> Forexes { get; set; }
        public DbSet<FinancialSetting> FinancialSettings { get; set; }
        public DbSet<GSTSetting> GstSettings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<AutoNumber> AutoNumbers { get; set; }
        public DbSet<AutoNumberCompany> AutoNumberCompanies { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RevalutionMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new MultiCurrencySettingMap());
            modelBuilder.Configurations.Add(new JournalMap());
            modelBuilder.Configurations.Add(new JournalDetailMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            modelBuilder.Configurations.Add(new AccountTypeMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new ForexMap());
            modelBuilder.Configurations.Add(new FinancialSettingMap());
            modelBuilder.Configurations.Add(new GSTSettingMap());
            modelBuilder.Configurations.Add(new RevalutionDetailMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
            modelBuilder.Configurations.Add(new BillMap());
            modelBuilder.Configurations.Add(new DebitNoteMap());
            modelBuilder.Configurations.Add(new AutoNumberMap());
            modelBuilder.Configurations.Add(new AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());

        }

    }
}
