namespace AppsWorld.CommonModule.Entities
{
    using System.Data.Entity;
    using Repository.Pattern.Ef6;
    using AppsWorld.CommonModule.RepositoryPattern;
    using AppsWorld.CommonModule.Entities.Mapping;
    using Models;
    using Models.Mappings;
    using Ziraff.FrameWork;
    using System.Configuration;

    public partial class CommonContext : DataContext, ICommonModuleDataContextAsync
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
        //public CommonContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<CommonContext>(null);

        //}
        static CommonContext()
        {
            Database.SetInitializer<CommonContext>(null);
        }
        public CommonContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<FinancialSetting> FinancialSettings { get; set; }
        //public DbSet<Forex> Forexes { get; set; }
        public DbSet<GSTSetting> GSTSettings { get; set; }
        public DbSet<MultiCurrencySetting> MultiCurrencySettings { get; set; }
        //public DbSet<SegmentMaster> SegmentMasters { get; set; }
        //public DbSet<SegmentDetail> SegmentDetails { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<TaxCode> TaxCodes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<BeanEntity> BeanEntities { get; set; }
        public DbSet<ControlCodeCategory> ControlCodeCategory { get; set; }
        public DbSet<ControlCode> ControleCodes { get; set; }
        public DbSet<CompanySetting> CompanySettings { get; set; }
        public DbSet<AutoNumber> AutoNumbers { get; set; }
        public DbSet<AutoNumberCompany> AutoNumberCompanys { get; set; }
        //public DbSet<BankReconciliationSetting> BankReconciliations { get; set; }
        public DbSet<TermsOfPayment> TermsOfPayments { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<CompanyUserDetail> CompanyUserDetails { get; set; }
        public DbSet<DocRepository> DocRepositorys { get; set; }
        public DbSet<DocumentHistory> DocumentHistories { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccountTypeMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new CurrencyMap());
            modelBuilder.Configurations.Add(new FinancialSettingMap());
            //modelBuilder.Configurations.Add(new ForexMap());
            modelBuilder.Configurations.Add(new GSTSettingMap());
            modelBuilder.Configurations.Add(new MultiCurrencySettingMap());
            //modelBuilder.Configurations.Add(new SegmentMasterMap());
            //modelBuilder.Configurations.Add(new SegmentDetailMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            modelBuilder.Configurations.Add(new ControlCodeCategoryMap());
            modelBuilder.Configurations.Add(new ControlCodeMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new CompanySettingMap());
            modelBuilder.Configurations.Add(new AutoNumberMap());
            modelBuilder.Configurations.Add(new AutoNumberCompanyMap());
            //modelBuilder.Configurations.Add(new BankReconciliationSettingMap());
            modelBuilder.Configurations.Add(new TaxCodeMap());
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new TermsOfPaymentMap());
            modelBuilder.Configurations.Add(new JournalMap());
            modelBuilder.Configurations.Add(new ItemMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new CompanyUserDetailMap());
            modelBuilder.Configurations.Add(new DocRepositoryMap());
            modelBuilder.Configurations.Add(new DocumentHistoryMap());
        }
    }
}
