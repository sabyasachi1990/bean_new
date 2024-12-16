using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Repository.Pattern.Ef6;
using AppsWorld.DebitNoteModule.Entities.Mapping;
using AppsWorld.DebitNoteModule.RepositoryPattern;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Entities.Mapping;
using Ziraff.FrameWork;
using System.Configuration;

namespace AppsWorld.DebitNoteModule.Entities
{
    public class DebitNoteContext : DataContext, IDebitNoteModuleDataContextAsync
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
        public DbSet<DebitNoteDetail> DebitNoteDetails { get; set; }
        //public DbSet<DebitNoteGSTDetail> DebitNoteGSTDetails { get; set; }
        public DbSet<DebitNoteNote> DebitNoteNotes { get; set; }
        public DbSet<AppsWorld.DebitNoteModule.Entities.AutoNumber> AutoNumbers { get; set; }
        public DbSet<AppsWorld.DebitNoteModule.Entities.AutoNumberCompany> AutoNumberCompanies { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        //public DbSet<Provision> Provisions { get; set; }
        public DbSet<BeanEntity> BeanEntities { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public DbSet<ReceiptBalancingItem> ReceiptBalancingItems { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<CreditNoteApplication> CreditNoteApplications { get; set; }
        public DbSet<CreditNoteApplicationDetail> CreditNoteApplicationDetails { get; set; }
        public DbSet<DoubtfulDebtAllocation> DoubtfulDebtAllocations { get; set; }
        public DbSet<DoubtfulDebtAllocationDetail> DoubtfulDebtAllocationDetails { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.TermsOfPayment> TermsOfPayments { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Company> Companys { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DebitNoteMap());
            modelBuilder.Configurations.Add(new DebitNoteDetailMap());
            //modelBuilder.Configurations.Add(new DebitNoteGSTDetailMap());
            modelBuilder.Configurations.Add(new DebitNoteNoteMap());
            modelBuilder.Configurations.Add(new AppsWorld.DebitNoteModule.Entities.Mapping.AutoNumberMap());
            modelBuilder.Configurations.Add(new AppsWorld.DebitNoteModule.Entities.Mapping.AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
            modelBuilder.Configurations.Add(new InvoiceDetailMap());
            //modelBuilder.Configurations.Add(new ProvisionMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            modelBuilder.Configurations.Add(new ReceiptDetailMap());
            modelBuilder.Configurations.Add(new ReceiptBalancingItemMap());
            modelBuilder.Configurations.Add(new ReceiptMap());
            modelBuilder.Configurations.Add(new CreditNoteApplicationMap());
            modelBuilder.Configurations.Add(new CreditNoteApplicationDetailMap());
            modelBuilder.Configurations.Add(new DoubtfulDebtAllocationMap());
            modelBuilder.Configurations.Add(new DoubtfulDebtAllocationDetailMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Mapping.TermsOfPaymentMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Mapping.CompanyMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
            modelBuilder.Configurations.Add(new CompanyUserMap());

        }
    }
}
