using AppsWold.InvoiceModule.RepositoryPattern;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Entities.Mapping;
using AppsWorld.InvoiceModule.Entities.Models;
using AppsWorld.InvoiceModule.Entities.Models.Mappings;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ziraff.FrameWork;

namespace AppsWorld.InvoiceModule.Entities
{
    public class InvoiceModuleContext : DataContext, IInvoiceModuleDataContextAsync
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
        //public InvoiceModuleContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<InvoiceModuleContext>(null);
        //}
        static InvoiceModuleContext()
        {
            Database.SetInitializer<InvoiceModuleContext>(null);
        }
        public InvoiceModuleContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        //public DbSet<InvoiceGSTDetail> InvoiceGSTDetails { get; set; }
        public DbSet<InvoiceNote> InvoiceNotes { get; set; }
        //public DbSet<Provision> Provisions { get; set; }
        public DbSet<CreditNoteApplicationDetail> CreditNoteApplicationDetails { get; set; }
        public DbSet<CreditNoteApplication> CreditNoteApplications { get; set; }
        public DbSet<DoubtfulDebtAllocationDetail> DoubtfulDebtAllocationDetails { get; set; }
        public DbSet<DoubtfulDebtAllocation> DoubtfulDebtAllocations { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<DebitNoteDetail> DebitNoteDetails { get; set; }
        public DbSet<DebitNoteGSTDetail> DebitNoteGSTDetails { get; set; }
        public DbSet<DebitNoteNote> DebitNoteNotes { get; set; }
        //public DbSet<JournalLedger> JournalLedgers { get; set; }
        public DbSet<BeanEntity> BeanEntities { get; set; }
        public DbSet<AppsWorld.InvoiceModule.Entities.AutoNumber> AutoNumbers { get; set; }
        public DbSet<AppsWorld.InvoiceModule.Entities.AutoNumberCompany> AutoNumberCompanies { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public DbSet<ReceiptBalancingItem> ReceiptBalancingItems { get; set; }
        public DbSet<AppsWorld.InvoiceModule.Entities.JournalDetail> JournalDetails { get; set; }
        public DbSet<AppsWorld.InvoiceModule.Entities.Journal> Journals { get; set; }
        public DbSet<PaymentCompact> Payments { get; set; }
        public DbSet<PaymentDetailCompact> PaymentDetails { get; set; }

        public DbSet<AppsWorld.CommonModule.Entities.Company> Companys { get; set; }

        public DbSet<AppsWorld.CommonModule.Entities.TermsOfPayment> TermsOfPayments { get; set; }

        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }
        public DbSet<Bank> Bank { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<AddressBook> AddressBooks { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<CommonForex> CommonForexs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AppsWorld.InvoiceModule.Entities.Models.Mappings.InvoiceMap());
            modelBuilder.Configurations.Add(new InvoiceDetailMap());
            //modelBuilder.Configurations.Add(new InvoiceGSTDetailMap());
            modelBuilder.Configurations.Add(new InvoiceNoteMap());
            //modelBuilder.Configurations.Add(new ProvisionMap());
            modelBuilder.Configurations.Add(new CreditNoteApplicationDetailMap());
            modelBuilder.Configurations.Add(new CreditNoteApplicationMap());
            modelBuilder.Configurations.Add(new DoubtfulDebtAllocationDetailMap());
            modelBuilder.Configurations.Add(new DoubtfulDebtAllocationMap());
            modelBuilder.Configurations.Add(new ItemMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new DebitNoteMap());
            modelBuilder.Configurations.Add(new DebitNoteDetailMap());
            modelBuilder.Configurations.Add(new DebitNoteGSTDetailMap());
            modelBuilder.Configurations.Add(new DebitNoteNoteMap());
            //modelBuilder.Configurations.Add(new JournalLedgerMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            modelBuilder.Configurations.Add(new AppsWorld.InvoiceModule.Entities.Models.Mappings.AutoNumberMap());
            modelBuilder.Configurations.Add(new AppsWorld.InvoiceModule.Entities.Models.Mappings.AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new ReceiptMap());
            modelBuilder.Configurations.Add(new ReceiptDetailMap());
            modelBuilder.Configurations.Add(new ReceiptBalancingItemMap());
            modelBuilder.Configurations.Add(new AppsWorld.InvoiceModule.Entities.Models.Mappings.JournalDetailMap());
            modelBuilder.Configurations.Add(new AppsWorld.InvoiceModule.Entities.Models.Mappings.JournalMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Mapping.CompanyMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Mapping.TermsOfPaymentMap());
           
            modelBuilder.Configurations.Add(new BankMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AddressBookMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
            modelBuilder.Configurations.Add(new PaymentCompactMap());
            modelBuilder.Configurations.Add(new PaymentDetailCompactMap());
            modelBuilder.Configurations.Add(new CommonForexMap());
        }
    }
}
