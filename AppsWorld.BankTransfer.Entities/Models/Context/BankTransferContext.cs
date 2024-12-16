using AppsWorld.BankTransferModule.Entities.Models.Mapping;
using AppsWorld.BankTransferModule.RepositoryPattern;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Entities.Mapping;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ziraff.FrameWork;

namespace AppsWorld.BankTransferModule.Entities.Models.Context
{
    public partial class BankTransferContext : DataContext, IBankTransferModuleDataContextAsync
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
        //public BankTransferContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<BankTransferContext>(null);
        //}
        static BankTransferContext()
        {
            Database.SetInitializer<BankTransferContext>(null);
        }
        public BankTransferContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<BankTransfer> BankTransfers { get; set; }
        public DbSet<BankTransferDetail> BankTransferDetails { get; set; }
        //public DbSet<AppsWorld.BankTransferModule.Entities.AutoNumber> AutoNumbers { get; set; }
        //public DbSet<AppsWorld.BankTransferModule.Entities.AutoNumberCompany> AutoNumberCompanys { get; set; }
        //public DbSet<Company> Companies { get; set; }
        //public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<SettlementDetail> SettlementDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<DebitNote> DebitNotes { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Journal> Journals { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new BankTransferDetailMap());
            modelBuilder.Configurations.Add(new BankTransferMap());
            modelBuilder.Configurations.Add(new AppsWorld.BankTransferModule.Entities.Models.Mapping.AutoNumberMap());
            modelBuilder.Configurations.Add(new AppsWorld.BankTransferModule.Entities.Models.Mapping.AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new SettlementDetailMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
            modelBuilder.Configurations.Add(new DebitNoteMap());
            modelBuilder.Configurations.Add(new BillMap());
            modelBuilder.Configurations.Add(new AppsWorld.BankTransferModule.Entities.Models.Mapping.JournalMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserMap());
        }
    }
}
