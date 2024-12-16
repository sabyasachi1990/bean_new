using AppsWorld.TemplateModule.Entities.Models;
using AppsWorld.TemplateModule.Entities.Models.Mapping;
using AppsWorld.TemplateModule.RepositoryPattern;
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
    public class TemplateContext : DataContext, ITemplateModuleDataContextAsync
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
        //public TemplateContext() : base(ConnectionString)
        //{
        //    Database.SetInitializer<TemplateContext>(null);
        //}
        static TemplateContext()
        {
            Database.SetInitializer<TemplateContext>(null);
        }
        public TemplateContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressBook> AddressBooks { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetails { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<JournalDetail> JournalDetails { get; set; }
        public DbSet<TaxCode> TaxCodes { get; set; }
        public DbSet<GenericTemplate> GenericTemplates { get; set; }
        public DbSet<BeanEntity> BeanEntities { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new InvoiceMap());
            modelBuilder.Configurations.Add(new InvoiceDetailMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new ReceiptMap());
            modelBuilder.Configurations.Add(new ReceiptDetailMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AddressBookMap());
            modelBuilder.Configurations.Add(new JournalMap());
            modelBuilder.Configurations.Add(new JournalDetailMap());
            modelBuilder.Configurations.Add(new TaxCodeMap());
            modelBuilder.Configurations.Add(new BankMap());
            modelBuilder.Configurations.Add(new GenericTemplateMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());

        }


    }
}
