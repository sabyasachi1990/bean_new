namespace AppsWorld.ReceiptModule.Entities
{
    using System.Data.Entity;
    using Repository.Pattern.Ef6;
    using AppsWorld.ReceiptModule.RepositoryPattern;
    using AppsWorld.ReceiptModule.Entities.Mapping;
    using Ziraff.FrameWork;
    using System.Configuration;

    public partial class ReceiptContext : DataContext, IReceiptModuleDataContextAsync
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

        //public ReceiptContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<ReceiptContext>(null);
        //}
        static ReceiptContext()
        {
            Database.SetInitializer<ReceiptContext>(null);
        }
        public ReceiptContext()
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
        public DbSet<TaxCode> TaxCodes { get; set; }
        public DbSet<MultiCurrencySetting> MultiCurrencySettings { get; set; }
        //public DbSet<SegmentMaster> SegmentMasters { get; set; }
        //public DbSet<SegmentDetail> SegmentDetails { get; set; }
        public DbSet<Company> Companies { get; set; }


        public DbSet<BeanEntity> BeanEntities { get; set; }
        //public DbSet<Item> Items { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        //public DbSet<CCAccountType> CCAccountTypes { get; set; }
        //public DbSet<AddressBook> AddressBooks { get; set; }
        //public DbSet<Address> Addresses { get; set; }
        //public DbSet<IdType> IdTypes { get; set; }
        public DbSet<TermsOfPayment> TermsOfPaments { get; set; }
        public DbSet<ControlCodeCategory> ControlCodeCategory { get; set; }
        public DbSet<ControlCode> ControleCodes { get; set; }
        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<DebitNoteDetail> DebitNoteDetails { get; set; }

        //public DbSet<JournalEntry> JournalEntries { get; set; }

        public DbSet<CompanySetting> CompanySettings { get; set; }

        //public DbSet<InvoiceNote> InvoiceNotes { get; set; }
        public DbSet<AutoNumber> AutoNumbers { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> companyUserDetails { get; set; }
        public DbSet<AutoNumberCompany> AutoNumberCompanys { get; set; }
        //public DbSet<DebitNoteNote> DebitNoteNotes { get; set; }
        //public DbSet<InvoiceGSTDetail> InvoiceGSTDetails { get; set; }
        //public DbSet<DebitNoteGSTDetail> DebitNoteGSTDetails { get; set; }

        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }

        public DbSet<ReceiptBalancingItem> ReceiptBalancingIteems { get; set; }
        //public DbSet<ReceiptGSTDetail> ReceiptGSTDetails { get; set; }
        //public DbSet<BankReconciliationSetting> BankReconciliations { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<JournalDetail> JournalDetails { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<BillCompact> Bills { get; set; }
        public DbSet<CreditMemoCompact> CreditMemos { get; set; }
        public DbSet<CreditNoteApplicationCompact> CreditNoteApplications { get; set; }
        public DbSet<CreditMemoApplicationCompact> CreditMemoApplications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            modelBuilder.Configurations.Add(new AccountTypeMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new CurrencyMap());
            modelBuilder.Configurations.Add(new FinancialSettingMap());
            //modelBuilder.Configurations.Add(new ForexMap());
            modelBuilder.Configurations.Add(new GSTSettingMap());
            modelBuilder.Configurations.Add(new TaxCodeMap());
            modelBuilder.Configurations.Add(new MultiCurrencySettingMap());
            //modelBuilder.Configurations.Add(new SegmentMasterMap());
            //modelBuilder.Configurations.Add(new SegmentDetailMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            //modelBuilder.Configurations.Add(new ItemMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
            modelBuilder.Configurations.Add(new InvoiceDetailMap());
            //modelBuilder.Configurations.Add(new CCAccountTypeMap());
            //modelBuilder.Configurations.Add(new AddressBookMap());
            //modelBuilder.Configurations.Add(new AddressMap());
            //modelBuilder.Configurations.Add(new IdTypeMap());
            modelBuilder.Configurations.Add(new TermsOfPaymentMap());
            modelBuilder.Configurations.Add(new ControlCodeCategoryMap());
            modelBuilder.Configurations.Add(new ControlCodeMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new DebitNoteMap());
            modelBuilder.Configurations.Add(new DebitNoteDetailMap());
            //modelBuilder.Configurations.Add(new JournalEntryMap());
            modelBuilder.Configurations.Add(new CompanySettingMap());
            //modelBuilder.Configurations.Add(new InvoiceNoteMap());
            modelBuilder.Configurations.Add(new AutoNumberMap());
            modelBuilder.Configurations.Add(new AutoNumberCompanyMap());
            //modelBuilder.Configurations.Add(new DebitNoteNoteMap());
            //modelBuilder.Configurations.Add(new InvoiceGSTDetailMap());
            //modelBuilder.Configurations.Add(new DebitNoteGSTDetailMap());
            modelBuilder.Configurations.Add(new ReceiptMap());
            modelBuilder.Configurations.Add(new ReceiptDetailMap());
            modelBuilder.Configurations.Add(new ReceiptBalancingItemMap());
            //modelBuilder.Configurations.Add(new ReceiptGSTDetailMap());
            //modelBuilder.Configurations.Add(new BankReconciliationSettingMap());
            modelBuilder.Configurations.Add(new JournalMap());
            modelBuilder.Configurations.Add(new JournalDetailMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new BillMap());
            modelBuilder.Configurations.Add(new CreditMemoCompactMap());
            modelBuilder.Configurations.Add(new CreditNoteApplicationCompactMap());
            modelBuilder.Configurations.Add(new CreditMemoApplicationCompactMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
        }
    }
}
