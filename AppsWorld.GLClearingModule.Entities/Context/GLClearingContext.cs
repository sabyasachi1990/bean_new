using AppsWorld.GLClearingModule.RepositoryPattern;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AppsWorld.GLClearingModule.Entities.Mapping;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Entities.Mapping;
using Ziraff.FrameWork;
using System.Configuration;

namespace AppsWorld.GLClearingModule.Entities
{
    public class GLClearingContext : DataContext, IClearingModuleDataContextAsync
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
        //public GLClearingContext() : base(ConnectionString)
        //{
        //    Database.SetInitializer<GLClearingContext>(null);
        //}
        static GLClearingContext()
        {
            Database.SetInitializer<GLClearingContext>(null);
        }
        public GLClearingContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<GLClearing> GLClearings { get; set; }
        public DbSet<GLClearingDetail> GLClearingDetails { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<JournalDetail> JournalDetails { get; set; }
        public DbSet<AutoNumber> AutoNumbers { get; set; }
        public DbSet<AutoNumberCompany> AutoNumberCompanys { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetails { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GLClearingMap());
            modelBuilder.Configurations.Add(new GLClearingDetailMap());
            modelBuilder.Configurations.Add(new AppsWorld.GLClearingModule.Entities.Mapping.JournalMap());
            modelBuilder.Configurations.Add(new AppsWorld.GLClearingModule.Entities.Mapping.JournalDetailMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new AppsWorld.GLClearingModule.Entities.Mapping.AutoNumberMap());
            modelBuilder.Configurations.Add(new AppsWorld.GLClearingModule.Entities.Mapping.AutoNumberCompanyMap());
            modelBuilder.Configurations.Add(new AppsWorld.GLClearingModule.Entities.Mapping.CompanyUserMap());
            modelBuilder.Configurations.Add(new AppsWorld.CommonModule.Entities.Models.Mappings.CompanyUserDetailMap());

        }
    }
}
