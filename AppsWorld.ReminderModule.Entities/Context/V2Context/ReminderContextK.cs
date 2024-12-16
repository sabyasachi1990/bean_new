using AppsWorld.ReminderModule.Entities.V2Entities;
using AppsWorld.ReminderModule.Entities.V2Entities.V2Mappings;
using AppsWorld.ReminderModule.RepositoryPattern.V2Repository;
using Repository.Pattern.Ef6;
using System.Data.Entity;

namespace AppsWorld.ReminderModule.Entities.Context.V2Context
{
    public class ReminderContextK : DataContext, IReminderKModuleDataContextAsync
    {
        static ReminderContextK()
        {
            Database.SetInitializer<ReminderContextK>(null);
        }
        public ReminderContextK()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<SOAReminderBatchListEntity> SOAReminderBatchLists { get; set; }
        public DbSet<SOAReminderBatchListDetailsEntity> SOAReminderBatchListDetails { get; set; }
        public DbSet<BeanEntity> BeanEntity { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SOAReminderBatchListMapping());
            modelBuilder.Configurations.Add(new SOAReminderBatchListDetailsMapping());
            modelBuilder.Configurations.Add(new BeanEntityMapping());
        }
    }
}
