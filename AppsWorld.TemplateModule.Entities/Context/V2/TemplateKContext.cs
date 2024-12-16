using AppsWorld.TemplateModule.Entities.Models;
using AppsWorld.TemplateModule.Entities.Models.Mapping;
using AppsWorld.TemplateModule.Entities.Models.V2;

using AppsWorld.TemplateModule.RepositoryPattern;
using AppsWorld.TemplateModule.RepositoryPattern.V2;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ziraff.FrameWork;

namespace AppsWorld.TemplateModule.Entities.Context
{
    public class TemplateKContext : DataContext, ITemplateCompactModuleDataContextAsync
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
        //public TemplateKContext() : base(ConnectionString)
        //{
        //    Database.SetInitializer<TemplateKContext>(null);
        //}
        static TemplateKContext()
        {
            Database.SetInitializer<TemplateKContext>(null);
        }
        public TemplateKContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Models.V2.Company> Company { get; set; }
        public DbSet<Models.V2.Invoice> Invoices { get; set; }
        public DbSet<Models.V2.InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Models.V2.Bank> Banks { get; set; }
        public DbSet<Models.V2.BeanEntity> BeanEntitys { get; set; }
        public DbSet<Models.V2.Address> Addresses { get; set; }
        public DbSet<Models.V2.AddressBook> AddressBooks { get; set; }
        public DbSet<Models.V2.CompanyTemplateSettings> CompanyTemplateSettingss { get; set; }
        public DbSet<Models.V2.GenericTemplate> GenericTemplates { get; set; }
        public DbSet<Models.V2.Receipt> Receipts { get; set; }
        public DbSet<Models.V2.ReceiptDetail> ReceiptDetails { get; set; }
        public DbSet<Models.V2.Journal> Journals { get; set; }
        public DbSet<Models.V2.JournalDetail> JournalDetails { get; set; }
        public DbSet<Models.V2.TaxCode> TaxCodes { get; set; }
        public DbSet<Models.V2.Template> Templates { get; set; }
        public DbSet<Models.V2.Localization> Localizations { get; set; }
        public DbSet<Models.V2.Contact> Contacts { get; set; }
        public DbSet<Models.V2.ContactDetail> ContactDetails { get; set; }
        public DbSet<Models.V2.CompanyUser> CompanyUsers { get; set; }
        public DbSet<Models.V2.ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<Models.V2.GSTSetting> GSTSettings { get; set; }
        public DbSet<Models.V2.TermsOfPayment> TermsOfPayments { get; set; }
        public DbSet<Models.V2.IdType> IdTypes { get; set; }
        public DbSet<Models.V2.DebitNote> DebitNotes { get; set; }
        public DbSet<Models.V2.DebitNoteDetail> DebitNoteDetails { get; set; }
        public DbSet<Models.V2.Item> Items { get; set; }
        public DbSet<Models.V2.CashSale> CashSales { get; set; }
        public DbSet<Models.V2.MediaRepository> MediaRepositories { get; set; }
        public DbSet<Models.V2.CashSaleDetail> CashSaleDetails { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Models.V2.Mapping.CompanyMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.InvoiceMap());            
            modelBuilder.Configurations.Add(new Models.V2.Mapping.CompanyTemplateSettingMap());            
            modelBuilder.Configurations.Add(new Models.V2.Mapping.ReceiptMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.ReceiptDetailMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.AddressMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.AddressBookMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.JournalMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.JournalDetailMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.TaxCodeMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.BankMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.GenericTemplateMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.BeanEntityMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.TemplateMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.InvoiceDetailMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.LocalizationMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.ContactMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.ContactDetailMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.CompanyUserMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.ChartOfAccountMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.GSTSettingMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.TermsOfPaymentMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.IdTypeMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.DebitNoteMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.DebitNoteDetailMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.ItemMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.CashSaleMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.CashSaleDetailMap());
            modelBuilder.Configurations.Add(new Models.V2.Mapping.MediaRepositoryMap());
        }


    }
}
