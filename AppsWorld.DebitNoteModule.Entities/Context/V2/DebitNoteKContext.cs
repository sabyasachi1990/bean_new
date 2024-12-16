using AppsWorld.DebitNoteModule.RepositoryPattern.V2;
using Repository.Pattern.Ef6;
using System.Configuration;
using System.Data.Entity;
using Ziraff.FrameWork;

namespace AppsWorld.DebitNoteModule.Entities.V2
{
    public class DebitNoteKContext : DataContext, IDebitNoteKDataContextAsync
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
        //public DebitNoteKContext() : base(ConnectionString)
        //{
        //    Database.SetInitializer<DebitNoteContext>(null);
        //}
        static DebitNoteKContext()
        {
            Database.SetInitializer<DebitNoteKContext>(null);
        }
        public DebitNoteKContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<DebitNoteK> DebitNotes { get; set; }
        public DbSet<BeanEntityCompact> Entities { get; set; }
        public DbSet<CompanyCompact> Companies { get; set; }
        public DbSet<CompanyUserCompact> CompanyUser { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DebitNoteKMap());
            modelBuilder.Configurations.Add(new BeanEntityCompactMap());
            modelBuilder.Configurations.Add(new CompanyUserCompactMap());
            modelBuilder.Configurations.Add(new CompanyCompactMap());
        }
    }
}
