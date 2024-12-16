using AppsWorld.ReceiptModule.Entities.Models.V2.Receipt;
using AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.Mappings;
using AppsWorld.ReceiptModule.RepositoryPattern.V2;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ziraff.FrameWork;

namespace AppsWorld.ReceiptModule.Entities.Context.V2
{
    public class ReceiptKModuleContext : DataContext, IReceiptKModuleDataContextAsync
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
        //public ReceiptKModuleContext()
        //    : base(ConnectionString)
        //{
        //    Database.SetInitializer<ReceiptKModuleContext>(null);
        //}
        static ReceiptKModuleContext()
        {
            Database.SetInitializer<ReceiptKModuleContext>(null);
        }
        public ReceiptKModuleContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<ReceiptK> ReceiptKs { get; set; }
        public DbSet<BeanEntityCompact> Entities { get; set; }
        public DbSet<CompanyCompact> Companies { get; set; }
        public DbSet<CompanyUserCompact> CompanyUsers { get; set; }
        public DbSet<ChartOfAccountCompact> ChartOfAccountCompacts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ReceiptKMap());
            modelBuilder.Configurations.Add(new BeanEntityCompactMap());
            modelBuilder.Configurations.Add(new CompanyCompactMap());
            modelBuilder.Configurations.Add(new CompanyUserCompactMap());
            modelBuilder.Configurations.Add(new ChartOfAccountCompactMap());
        }
    }
}
