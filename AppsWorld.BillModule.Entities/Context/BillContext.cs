namespace AppsWorld.BillModule.Entities
{
    using System.Data.Entity;
    using Repository.Pattern.Ef6;
    using AppsWorld.BillModule.RepositoryPattern;
    using Ziraff.FrameWork;
    using System.Configuration;
    using AppsWorld.BillModule.Entities.Models.Mapping;

    public partial class BillContext : DataContext, IBillModuleDataContextAsync
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
        //public BillContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<BillContext>(null);
        //}
        static BillContext()
        {
            Database.SetInitializer<BillContext>(null);
        }
        public BillContext()
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
        public DbSet<Item> Items { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<CCAccountType> CCAccountTypes { get; set; }
        public DbSet<AddressBook> AddressBooks { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<IdType> IdTypes { get; set; }
        public DbSet<TermsOfPayment> TermsOfPaments { get; set; }
        public DbSet<ControlCodeCategory> ControlCodeCategory { get; set; }
        public DbSet<ControlCode> ControleCodes { get; set; }
        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<DebitNoteDetail> DebitNoteDetails { get; set; }

        public DbSet<JournalEntry> JournalEntries { get; set; }

        public DbSet<CompanySetting> CompanySettings { get; set; }

        public DbSet<InvoiceNote> InvoiceNotes { get; set; }
        public DbSet<AutoNumber> AutoNumbers { get; set; }

        public DbSet<AutoNumberCompany> AutoNumberCompanys { get; set; }
        public DbSet<DebitNoteNote> DebitNoteNotes { get; set; }
        //public DbSet<InvoiceGSTDetail> InvoiceGSTDetails { get; set; }
        //public DbSet<DebitNoteGSTDetail> DebitNoteGSTDetails { get; set; }

        public DbSet<Bill> Bill { get; set; }
        public DbSet<BillDetail> BillDetail { get; set; }
        //public DbSet<BillGSTDetail> BillGSTDetail { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CreditMemo> CreditMemos { get; set; }
        public DbSet<CreditMemoApplication> CreditMemoApplications { get; set; }
        public DbSet<CreditMemoApplicationDetail> CreditMemoApplicationDetails { get; set; }
        public DbSet<Localization> Localizations { get; set; }
        public DbSet<CompanyUser> CompanyUser { get; set; }
        public DbSet<JournalDetail> JournalDetails { get; set; }
        public DbSet<ReceiptCompact> Receipts { get; set; }
        public DbSet<ReceiptDetailCompact> ReceiptDetails { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }

        public DbSet<CommonForex> commonForexs { get; set; }
        public DbSet<PeppolInboundInvoice> PeppolInboundInvoice { get; set; }
        //public DbSet<BeanAutoNumber> beanAutoNumbers { get; set; }

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
            modelBuilder.Configurations.Add(new ItemMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
            modelBuilder.Configurations.Add(new InvoiceDetailMap());
            modelBuilder.Configurations.Add(new CCAccountTypeMap());
            modelBuilder.Configurations.Add(new AddressBookMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new IdTypeMap());
            modelBuilder.Configurations.Add(new TermsOfPaymentMap());
            modelBuilder.Configurations.Add(new ControlCodeCategoryMap());
            modelBuilder.Configurations.Add(new ControlCodeMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new DebitNoteMap());
            modelBuilder.Configurations.Add(new DebitNoteDetailMap());
            modelBuilder.Configurations.Add(new JournalEntryMap());
            modelBuilder.Configurations.Add(new CompanySettingMap());
            modelBuilder.Configurations.Add(new InvoiceNoteMap());
            modelBuilder.Configurations.Add(new AutoNumberMap());
            modelBuilder.Configurations.Add(new AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new DebitNoteNoteMap());
            //modelBuilder.Configurations.Add(new InvoiceGSTDetailMap());
            //modelBuilder.Configurations.Add(new DebitNoteGSTDetailMap());

            modelBuilder.Configurations.Add(new BillMap());
            modelBuilder.Configurations.Add(new BillDetailMap());
            //modelBuilder.Configurations.Add(new BillGSTDetailMap());
            modelBuilder.Configurations.Add(new JournalMap());
            modelBuilder.Configurations.Add(new PaymentDetailMap());
            modelBuilder.Configurations.Add(new PaymentMap());
            modelBuilder.Configurations.Add(new CreditMemoMap());
            modelBuilder.Configurations.Add(new CreditMemoApplicationMap());
            modelBuilder.Configurations.Add(new CreditMemoApplicationDetailMap());
            modelBuilder.Configurations.Add(new LocalizationMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new JournalDetailMap());
            modelBuilder.Configurations.Add(new ReceiptCompactMap());
            modelBuilder.Configurations.Add(new ReceiptDetailCompactMap());
            modelBuilder.Configurations.Add(new CommonForexMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
            modelBuilder.Configurations.Add(new PeppolInboundInvoiceMap());
            //modelBuilder.Configurations.Add(new BeanAutoNumberMap());
        }
    }

}

