using AppsWorld.InvoiceModule.RepositoryPattern.V2;
using Repository.Pattern.Ef6;
using System.Configuration;
using System.Data.Entity;
using Ziraff.FrameWork;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    public class InvoiceKModuleContext : DataContext, IInvoiceKModuleDataContextAsync
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
        //public InvoiceKModuleContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<InvoiceKModuleContext>(null);
        //}
        static InvoiceKModuleContext()
        {
            Database.SetInitializer<InvoiceKModuleContext>(null);
        }
        public InvoiceKModuleContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<InvoiceK> Invoices { get; set; }
        public DbSet<BeanEntityCompact> Entities { get; set; }
        public DbSet<CompanyCompact> Companies { get; set; }
        public DbSet<CompanyUserCompact> CompanyUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new  InvoiceKMap());
            modelBuilder.Configurations.Add(new BeanEntityCompactMap());
            modelBuilder.Configurations.Add(new CompanyCompactMap());
            modelBuilder.Configurations.Add(new CompanyUserCompactMap());
        }
    }
}
