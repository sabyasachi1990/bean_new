namespace AppsWorld.BillModule.Entities
{
    using System.Data.Entity;
    using Repository.Pattern.Ef6;
    using AppsWorld.JournalVoucherModule.RepositoryPattern;
    using JournalVoucherModule.Entities;
    using JournalVoucherModule.Entities.Models.Mapping;
    using Ziraff.FrameWork;
    using System.Configuration;
    using JournalVoucherModule.Entities.Models;

    public partial class JournalContext : DataContext, IJournalVoucherModuleDataContextAsync
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
        //public JournalContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<JournalContext>(null);
        //}
        static JournalContext()
        {
            Database.SetInitializer<JournalContext>(null);
        }
        public JournalContext()
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
        public DbSet<JournalDetail> JournalDetails { get; set; }
        //public DbSet<JournalGSTDetail> JournalGSTDetails { get; set; }
        public DbSet<CreditNoteApplication> CreditNoteApplications { get; set; }
        public DbSet<DoubtfulDebtAllocation> DoubtfulDebtAllocation { get; set; }
        public DbSet<DoubtfulDebtAllocationDetail> DoubtfulDebtAllocationDetail { get; set; }
        public DbSet<CreditNoteApplicationDetail> CreditNoteApplicationDetail { get; set; }
        //public DbSet<JvActivityLog> JvActivityLogs { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }
        public DbSet<CashSale> CashSales { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ActivityHistory> ActivityHistories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CreditMemo> CreditMemos { get; set; }
        public DbSet<GLClearing> GLClearings { get; set; }
        public DbSet<GLClearingDetail> GLClearingDetails { get; set; }
        public DbSet<CompanyFeature> CompanyFeatures { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }

        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<BankReconciliation> BankReconciliations { get; set; }
        public DbSet<OpeningBalance> OpeningBalancees { get; set; }
        public DbSet<CommonForex> CommonForexs { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> companyUserDetails { get; set; }

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

            //modelBuilder.Configurations.Add(new JournalGSTDetailMap());
            modelBuilder.Configurations.Add(new JournalMap());
            modelBuilder.Configurations.Add(new JournalDetailMap());
            modelBuilder.Configurations.Add(new DoubtfulDebtAllocationMap());
            modelBuilder.Configurations.Add(new DoubtfulDebtAllocationDetailMap());
            modelBuilder.Configurations.Add(new CreditNoteApplicationMap());
            modelBuilder.Configurations.Add(new CreditNoteApplicationDetailMap());
            //modelBuilder.Configurations.Add(new JvActivityLogMap());
            modelBuilder.Configurations.Add(new WithdrawalMap());
            modelBuilder.Configurations.Add(new CashSaleMap());
            modelBuilder.Configurations.Add(new ReceiptMap());
            modelBuilder.Configurations.Add(new ActivityHistoryMap());
            modelBuilder.Configurations.Add(new PaymentMap());
            modelBuilder.Configurations.Add(new CreditMemoMap());
            modelBuilder.Configurations.Add(new GLClearingMap());
            modelBuilder.Configurations.Add(new GLClearingDetailMap());
            modelBuilder.Configurations.Add(new CompanyFeatureMap());
            modelBuilder.Configurations.Add(new FeatureMap());
            modelBuilder.Configurations.Add(new PaymentDetailMap());
            modelBuilder.Configurations.Add(new ReceiptDetailMap());

            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new SubCategoryMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
            modelBuilder.Configurations.Add(new BankReconciliationMap());
            modelBuilder.Configurations.Add(new OpeningBalanceMap());
            modelBuilder.Configurations.Add(new CommonForexMap());
        }
    }

}

