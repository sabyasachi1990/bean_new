namespace AppsWorld.PaymentModule.Entities
{
    using System.Data.Entity;
    using Repository.Pattern.Ef6;
    using AppsWorld.PaymentModule.RepositoryPattern;
    using AppsWorld.PaymentModule.Entities.Mapping;
    using AppsWorld.CommonModule.Entities;
    using AppsWorld.CommonModule.Entities.Mapping;
    using Ziraff.FrameWork;
    using System.Configuration;

    public partial class PaymentContext : DataContext, IPaymentModuleDataContextAsync
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
        //public PaymentContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<PaymentContext>(null);
        //}
        static PaymentContext()
        {
            Database.SetInitializer<PaymentContext>(null);
        }
        public PaymentContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<BeanEntity> BeanEntities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<AppsWorld.PaymentModule.Entities.AutoNumber> AutoNumbers { get; set; }
        public DbSet<AppsWorld.PaymentModule.Entities.AutoNumberCompany> AutoNumberCompanys { get; set; }
        public DbSet<AppsWorld.PaymentModule.Entities.JournalDetail> JournalDetails { get; set; }
        public DbSet<AppsWorld.PaymentModule.Entities.Feature> Features { get; set; }
        public DbSet<AppsWorld.PaymentModule.Entities.CompanyFeature> CompanyFeatures { get; set; }
        public DbSet<AppsWorld.PaymentModule.Entities.Journal> Journals { get; set; }
        public DbSet<AppsWorld.PaymentModule.Entities.CompanyUser> Companyuser { get; set; }
        public DbSet<CreditMemoCompact> CreditMemoCompacts { get; set; }
        public DbSet<InvoiceCompact> InvoiceCompacts { get; set; }
        public DbSet<CreditNoteApplicationCompact> CreditNoteApplicationCompacts { get; set; }
        public DbSet<CreditMemoApplicationCompact> CreditMemoApplicationCompacts { get; set; }
        public DbSet<DebitNoteCompact> DebitNoteCompacts { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BillMap());
            modelBuilder.Configurations.Add(new PaymentMap());
            modelBuilder.Configurations.Add(new PaymentDetailMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mapping.AutoNumberMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mapping.AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mapping.JournalDetailMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mapping.FeatureMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mapping.CompanyFeatureMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mapping.JournalMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mapping.CompanyUserMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mappings.CreditMemoCompactMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mappings.InvoiceCompactMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mappings.CreditNoteApplicationCompactMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mappings.CreditMemoApplicationCompactMap());
            modelBuilder.Configurations.Add(new AppsWorld.PaymentModule.Entities.Mappings.DebitNoteCompactMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
        }
    }
}
