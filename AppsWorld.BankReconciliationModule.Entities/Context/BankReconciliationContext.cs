using AppsWorld.BankReconciliationModule.Entities;
using System.Data.Entity;
using AppsWorld.BankReconciliationModule.Entities.Models;
using AppsWorld.BankReconciliationModule.Entities.Models.Mappings;
using Repository.Pattern.Ef6;
using AppsWorld.BankReconciliationModule.RepositoryPattern;


namespace AppsWorld.BankReconciliationModule.Entities
{
    public partial class BankReconciliationContext : DataContext, IBankReconciliationModuleDataContextAsync
    {
        public BankReconciliationContext()
            : base("name=AppsWorldDBContext")
        {
        }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<BankReconciliation> BankReconciliations { get; set; }
        public DbSet<BankReconciliationDetail> BankReconciliationDetails { get; set; }
       
        public DbSet<BankReconciliationSetting> BankReconciliationSettings { get; set; }

        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<FinancialSetting> FinancialSettings { get; set; }
        public DbSet<Forex> Forexes { get; set; }
        public DbSet<GSTSetting> GSTSettings { get; set; }
        public DbSet<TaxCode> TaxCodes { get; set; }
        public DbSet<MultiCurrencySetting> MultiCurrencySettings { get; set; }
        public DbSet<SegmentMaster> SegmentMasters { get; set; }
        public DbSet<SegmentDetail> SegmentDetails { get; set; }
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
        public DbSet<InvoiceGSTDetail> InvoiceGSTDetails { get; set; }
        public DbSet<DebitNoteGSTDetail> DebitNoteGSTDetails { get; set; }

        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public DbSet<ReceiptBalancingItem> ReceiptBalancingItems { get; set; }
        public DbSet<ReceiptGSTDetail> ReceiptGstDetails { get; set; }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            modelBuilder.Configurations.Add(new AccountTypeMap());
            modelBuilder.Configurations.Add(new BankReconciliationMap());
            modelBuilder.Configurations.Add(new BankReconciliationDetailMap());
          
            modelBuilder.Configurations.Add(new BankReconciliationSettingMap());

            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new CurrencyMap());
            modelBuilder.Configurations.Add(new FinancialSettingMap());
            modelBuilder.Configurations.Add(new ForexMap());
            modelBuilder.Configurations.Add(new GSTSettingMap());
            modelBuilder.Configurations.Add(new TaxCodeMap());
            modelBuilder.Configurations.Add(new MultiCurrencySettingMap());
            modelBuilder.Configurations.Add(new SegmentMasterMap());
            modelBuilder.Configurations.Add(new SegmentDetailMap());
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
            modelBuilder.Configurations.Add(new InvoiceGSTDetailMap());
            modelBuilder.Configurations.Add(new DebitNoteGSTDetailMap());
            modelBuilder.Configurations.Add(new ReceiptMap());
            modelBuilder.Configurations.Add(new ReceiptBalancingItemMap());
            modelBuilder.Configurations.Add(new ReceiptDetailMap());
            modelBuilder.Configurations.Add(new ReceiptGSTDetailMap());
        }
    }

}

