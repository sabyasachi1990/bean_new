using AppsWorld.DebitNoteModule.RepositoryPattern.V2;
using Repository.Pattern.Ef6;
using System.Configuration;
using System.Data.Entity;
using Ziraff.FrameWork;

namespace AppsWorld.DebitNoteModule.Entities.V2
{
    public class DebitNoteContext : DataContext, IDebitNoteDataContextAsync
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
        //public DebitNoteContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<DebitNoteContext>(null);
        //}
        static DebitNoteContext()
        {
            Database.SetInitializer<DebitNoteContext>(null);
        }
        public DebitNoteContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<BeanEntityCompact> Entities { get; set; }
        public DbSet<FinancialSettingCompact> FinancialSettings { get; set; }
        public DbSet<DebitNoteDetail> InvoiceDetails { get; set; }
        public DbSet<AutoNumberCompact> AutoNumbers { get; set; }
        public DbSet<AutoNumberComptCompany> AutoNumberCompanies { get; set; }
        public DbSet<TaxCodeCompact> TaxCodes { get; set; }
        public DbSet<ChartOfAccountCompact> ChartOfAccounts { get; set; }
        //public DbSet<TermsOfPaymentCompact> TermsOfPayment { get; set; }
        public DbSet<CompanyCompact> Companies { get; set; }
        public DbSet<CompanyUserCompact> CompanyUsers { get; set; }
        public DbSet<ReceiptCompact> ReceiptCompacts { get; set; }
        public DbSet<InvoiceCompact> InvoiceCompact { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DebitNoteMap());
            modelBuilder.Configurations.Add(new BeanEntityCompactMap());
            modelBuilder.Configurations.Add(new FinancialSettingCompactMap());
            modelBuilder.Configurations.Add(new AutoNumberCompactMap());
            modelBuilder.Configurations.Add(new AutoNumberCompanyComptMap());
            modelBuilder.Configurations.Add(new TaxCodeCompactMap());
            modelBuilder.Configurations.Add(new ChartOfAccountCompactMap());
            //modelBuilder.Configurations.Add(new TermsOfPaymentCompactMap());
            modelBuilder.Configurations.Add(new CompanyCompactMap());
            modelBuilder.Configurations.Add(new CompanyUserCompactMap());
            modelBuilder.Configurations.Add(new DebitNoteDetailMap());
            modelBuilder.Configurations.Add(new ReceiptCompactMap());
            modelBuilder.Configurations.Add(new InvoiceCompactMap());
        }
    }
}
