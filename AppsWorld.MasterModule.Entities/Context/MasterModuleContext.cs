using AppsWorld.MasterModule.Entities.Models.Mappings;
using AppsWorld.MasterModule.RepositoryPattern;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ziraff.FrameWork;

namespace AppsWorld.MasterModule.Entities
{
    public class MasterModuleContext : DataContext, IMasterModuleDataContextAsync
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
        //public MasterModuleContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<MasterModuleContext>(null);
        //}
        static MasterModuleContext()
        {
            Database.SetInitializer<MasterModuleContext>(null);
        }
        public MasterModuleContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<BeanEntity> BeanEntities { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Address> Addresss { get; set; }
        public DbSet<AddressBook> AddressBooks { get; set; }
        public DbSet<ControlCode> ControlCodes { get; set; }
        public DbSet<ControlCodeCategory> ControlCodeCategorys { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<IdType> IdTypes { get; set; }
        public DbSet<MultiCurrencySetting> MultiCurrencySettings { get; set; }
        public DbSet<TermsOfPayment> TermsOfPayments { get; set; }
        public DbSet<FinancialSetting> FinancialSettings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        //public DbSet<Forex> Forexs { get; set; }
        public DbSet<GSTSetting> GSTSettings { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<CompanySetting> CompanySettings { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        //public DbSet<BankReconciliationSetting> BankReconciliationSettings { get; set; }
        //public DbSet<SegmentDetail> SegmentDetails { get; set; }
        public DbSet<CompanyFeature> CompanyFeatures { get; set; }
        public DbSet<Feature> Features { get; set; }
        //public DbSet<SegmentMaster> SegmentMasters { get; set; }
        public DbSet<TaxCode> TaxCodes { get; set; }
        public DbSet<Item> Items { get; set; }
        //public DbSet<JournalLedger> JournalLedgers { get; set; }
        public DbSet<ModuleMaster> ModuleMasters { get; set; }
        public DbSet<CCAccountType> CCAccountTypes { get; set; }
        public DbSet<AccountTypeIdType> AccountTypeIdTypes { get; set; }
        public DbSet<ActivityHistory> ActivityHistorys { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<JournalDetail> JournalDetails { get; set; }
        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<CreditNoteApplication> CreditNoteApplications { get; set; }
        public DbSet<Receipt> Receipt { get; set; }
        public DbSet<CashSale> CashSales { get; set; }
        public DbSet<CashSaleDetail> CashSaleDetails { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<ContactDetail> ContactDetail { get; set; }
        public DbSet<MediaRepository> MediaRepository { get; set; }
        public DbSet<OpeningBalanceDetail> OpeningBalanceDetails { get; set; }
        public DbSet<Communication> Communications { get; set; }
        public DbSet<InterCompanySetting> InterCompanySettings { get; set; }
        public DbSet<InterCompanySettingDetail> InterCompanySettingDetail { get; set; }
        public DbSet<COAMapping> COAMappings { get; set; }
        public DbSet<COAMappingDetail> COAMappingDetails { get; set; }
        public DbSet<TaxCodeMapping> TaxCodeMappings { get; set; }
        public DbSet<TaxCodeMappingDetail> TaxCodeMappingDetails { get; set; }
        public DbSet<Models.CommonForex> CommonForexs { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Company> Companys { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> companyUserDetails { get; set; }
        public DbSet<AppsWorld.MasterModule.Entities.Models.SSICCodes> sSICCodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BeanEntityMap());
            modelBuilder.Configurations.Add(new AccountTypeMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AddressBookMap());
            modelBuilder.Configurations.Add(new ControlCodeCategoryMap());
            modelBuilder.Configurations.Add(new ControlCodeMap());
            modelBuilder.Configurations.Add(new IdTypeMap());
            modelBuilder.Configurations.Add(new MultiCurrencySettingMap());
            modelBuilder.Configurations.Add(new TermsOfPaymentMap());
            modelBuilder.Configurations.Add(new CurrencyMap());
            modelBuilder.Configurations.Add(new FinancialSettingMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
            //modelBuilder.Configurations.Add(new ForexMap());
            modelBuilder.Configurations.Add(new GSTSettingMap());
            modelBuilder.Configurations.Add(new JournalMap());
            modelBuilder.Configurations.Add(new CompanySettingMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            //modelBuilder.Configurations.Add(new BankReconciliationSettingMap());
            //modelBuilder.Configurations.Add(new SegmentMasterMap());
            //modelBuilder.Configurations.Add(new SegmentDetailMap());
            modelBuilder.Configurations.Add(new TaxCodeMap());
            modelBuilder.Configurations.Add(new ItemMap());
            //modelBuilder.Configurations.Add(new JournalLedgerMap());
            modelBuilder.Configurations.Add(new ModuleMasterMap());
            modelBuilder.Configurations.Add(new CompanyFeatureMap());
            modelBuilder.Configurations.Add(new FeatureMap());
            modelBuilder.Configurations.Add(new CCAccountTypeMap());
            modelBuilder.Configurations.Add(new AccountTypeIdTypeMap());
            modelBuilder.Configurations.Add(new ActivityHistoryMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new InvoiceDetailMap());
            modelBuilder.Configurations.Add(new JournalDetailMap());
            modelBuilder.Configurations.Add(new DebitNoteMap());
            modelBuilder.Configurations.Add(new CreditNoteApplicationMap());
            modelBuilder.Configurations.Add(new ReceiptMap());
            modelBuilder.Configurations.Add(new CashSaleMap());
            modelBuilder.Configurations.Add(new CashSaleDetailMap());
            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new ContactDetailMap());
            modelBuilder.Configurations.Add(new MediaRepositoryMap());
            modelBuilder.Configurations.Add(new OpeningBalanceDetailMap());
            modelBuilder.Configurations.Add(new CommunicationMap());
            modelBuilder.Configurations.Add(new InterCompanySettingMap());
            modelBuilder.Configurations.Add(new InterCompanySettingDetailMap());
            modelBuilder.Configurations.Add(new COAMappingMap());
            modelBuilder.Configurations.Add(new COAMappingDetailMap());
            modelBuilder.Configurations.Add(new TaxCodeMappingMap());
            modelBuilder.Configurations.Add(new TaxCodeMappingDetailMap());
            modelBuilder.Configurations.Add(new CommonForexMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Mapping.CompanyMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
            modelBuilder.Configurations.Add(new SSICCodesMap());

        }
    }
}
