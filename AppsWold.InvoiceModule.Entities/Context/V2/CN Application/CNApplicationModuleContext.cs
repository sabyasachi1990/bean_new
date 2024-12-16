using AppsWorld.InvoiceModule.RepositoryPattern.V2;
using Repository.Pattern.Ef6;
using System.Configuration;
using System.Data.Entity;
using Ziraff.FrameWork;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    public class CNApplicationModuleContext : DataContext, IApplicationCompactModuleDataContextAsync
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
        //public CNApplicationModuleContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<CNApplicationModuleContext>(null);
        //}
        static CNApplicationModuleContext()
        {
            Database.SetInitializer<CNApplicationModuleContext>(null);
        }
        public CNApplicationModuleContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<CreditNoteApplication> CreditNoteApplication { get; set; }
        public DbSet<CreditNoteApplicationDetail> CreditNoteApplicationDetails { get; set; }
        public DbSet<BeanEntityCompact> Entities { get; set; }
        public DbSet<FinancialSettingCompact> FinancialSettings { get; set; }
        public DbSet<InvoiceCompact> Invoices { get; set; }
        public DbSet<DebitNoteCompact> DebitNotes { get; set; }
        public DbSet<AutoNumberCompact> AutoNumbers { get; set; }
        public DbSet<AutoNumberComptCompany> AutoNumberCompanies { get; set; }
        public DbSet<TaxCodeCompact> TaxCodes { get; set; }
        public DbSet<ChartOfAccountCompact> ChartOfAccounts { get; set; }
        //public DbSet<TermsOfPaymentCompact> TermsOfPayment { get; set; }
        //public DbSet<CurrencyCompact> Currencies { get; set; }
        //public DbSet<AccountTypeCompact> AccountTypes { get; set; }
        //public DbSet<ReceiptCompact> Receipts { get; set; }
        public DbSet<CompanyCompact> Companies { get; set; }
        public DbSet<CompanyUserCompact> CompanyUsers { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new InvoiceCompactMap());
            modelBuilder.Configurations.Add(new DebitNoteComactMap());
            modelBuilder.Configurations.Add(new BeanEntityCompactMap());
            modelBuilder.Configurations.Add(new FinancialSettingCompactMap());
            modelBuilder.Configurations.Add(new ItemCompactMap());
            modelBuilder.Configurations.Add(new InvoiceDetailMap());
            modelBuilder.Configurations.Add(new AutoNumberCompactMap());
            modelBuilder.Configurations.Add(new InvoiceNoteCompactMap());
            modelBuilder.Configurations.Add(new AutoNumberCompanyComptMap());
            modelBuilder.Configurations.Add(new TaxCodeCompactMap());
            modelBuilder.Configurations.Add(new ChartOfAccountCompactMap());
            //modelBuilder.Configurations.Add(new TermsOfPaymentCompactMap());
            //modelBuilder.Configurations.Add(new CurrencyCompactMap());
            //modelBuilder.Configurations.Add(new AccountTypeCompactMap());
            //modelBuilder.Configurations.Add(new ReceiptCompactMap());
            modelBuilder.Configurations.Add(new CompanyCompactMap());
            modelBuilder.Configurations.Add(new CompanyUserCompactMap());
            modelBuilder.Configurations.Add(new CreditNoteApplicationMap());
            modelBuilder.Configurations.Add(new CreditNoteApplicationDetailMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
        }
    }
}
