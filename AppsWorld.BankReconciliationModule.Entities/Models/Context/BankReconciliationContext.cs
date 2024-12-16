using AppsWorld.BankReconciliationModule.Entities;
using System.Data.Entity;
using AppsWorld.BankReconciliationModule.Entities.Models;
using AppsWorld.BankReconciliationModule.Entities.Models.Mappings;
using Repository.Pattern.Ef6;
using AppsWorld.BankReconciliationModule.RepositoryPattern;
using AppsWorld.BankReconciliationModule.Entities.Models.Mapping;
using System.Configuration;
using Ziraff.FrameWork;

namespace AppsWorld.BankReconciliationModule.Entities
{
    public partial class BankReconciliationContext : DataContext, IBankReconciliationModuleDataContextAsync
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
        //public BankReconciliationContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<BankReconciliationContext>(null);
        //}
        static BankReconciliationContext()
        {
            Database.SetInitializer<BankReconciliationContext>(null);
        }
        public BankReconciliationContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<BankReconciliation> BankReconciliations { get; set; }
        public DbSet<BankReconciliationDetail> BankReconciliationDetails { get; set; }

        //public DbSet<BankReconciliationSetting> BankReconciliationSettings { get; set; }

        public DbSet<FinancialSetting> FinancialSettings { get; set; }
        public DbSet<TaxCode> TaxCodes { get; set; }
        public DbSet<Company> Companies { get; set; }


        //  public DbSet<Item> Items { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public DbSet<ReceiptBalancingItem> ReceiptBalancingItems { get; set; }
        //public DbSet<ReceiptGSTDetail> ReceiptGstDetails { get; set; }

        public DbSet<Journal> Journals { get; set; }
        public DbSet<JournalDetail> JournalDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Withdrawal> WithDrawals { get; set; }
        public DbSet<BankTransfer> BankTransfers { get; set; }
        public DbSet<CashSale> CashSales { get; set; }
        public DbSet<BankTransferDetail> BankTransferDetails { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<OpeningBalanceDetail> OpeningBalanceDetails { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BankReconciliationMap());
            modelBuilder.Configurations.Add(new BankReconciliationDetailMap());
            //modelBuilder.Configurations.Add(new BankReconciliationSettingMap());
            modelBuilder.Configurations.Add(new TaxCodeMap());
            modelBuilder.Configurations.Add(new ReceiptMap());
            modelBuilder.Configurations.Add(new ReceiptBalancingItemMap());
            modelBuilder.Configurations.Add(new ReceiptDetailMap());
            //modelBuilder.Configurations.Add(new ReceiptGSTDetailMap());

            modelBuilder.Configurations.Add(new JournalMap());
            modelBuilder.Configurations.Add(new JournalDetailMap());

            modelBuilder.Configurations.Add(new PaymentMap());
            modelBuilder.Configurations.Add(new WithdrawalMap());
            modelBuilder.Configurations.Add(new BankTransferMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new CashSaleMap());
            modelBuilder.Configurations.Add(new BankTransferDetailMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new OpeningBalanceDetailMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
        }
    }

}

