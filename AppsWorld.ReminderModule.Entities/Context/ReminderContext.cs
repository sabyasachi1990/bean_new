using AppsWorld.BillModule.Entities;
using AppsWorld.ReminderModule.Entities.Entities;
using AppsWorld.ReminderModule.Entities.Mappings;
using AppsWorld.ReminderModule.RepositoryPattern;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Context
{
    public class ReminderContext : DataContext, IReminderModuleDataContextAsync
    {
        static ReminderContext()
        {
            Database.SetInitializer<ReminderContext>(null);
        }
        public ReminderContext()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<SOAReminderBatchList> SOAReminderBatchLists { get; set; }
        public DbSet<SOAReminderBatchListDetails> SOAReminderBatchListDetails { get; set; }
        public DbSet<BeanEntity> BeanEntities { get; set; }
        public DbSet<BillModule.Entities.ControlCodeCategory> ControlCodeCategories { get; set; }
        public DbSet<BillModule.Entities.ControlCode> ControlCodes { get; set; }
        public DbSet<CompanyCompact> Companies { get; set; }
        public DbSet<LocalizationCompact> Localizations { get; set; }
        public DbSet<GenericTemplateCompact> GenericTemplates { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressBook> AddressBooks { get; set; }
        public DbSet<GSTSetting> GSTSettings { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<IdType> IdTypes { get; set; }
        public DbSet<CommunicationCompact> Communications { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SOAReminderBatchListMap());
            modelBuilder.Configurations.Add(new SOAReminderBatchListDetailsMap());
            modelBuilder.Configurations.Add(new BeanEntityMap());
            modelBuilder.Configurations.Add(new BillModule.Entities.ControlCodeCategoryMap());
            modelBuilder.Configurations.Add(new BillModule.Entities.ControlCodeMap());
            modelBuilder.Configurations.Add(new Mappings.CompanyMap());
            modelBuilder.Configurations.Add(new Mappings.LocalizationMap());

            modelBuilder.Configurations.Add(new AppsWorld.BillModule.Entities.AddressMap());
            modelBuilder.Configurations.Add(new AppsWorld.BillModule.Entities.AddressBookMap());
            modelBuilder.Configurations.Add(new AppsWorld.BillModule.Entities.GSTSettingMap());
            modelBuilder.Configurations.Add(new AppsWorld.BillModule.Entities.IdTypeMap());
            modelBuilder.Configurations.Add(new Mappings.GenericTemplateMap());
            modelBuilder.Configurations.Add(new Mappings.BankMap());
            modelBuilder.Configurations.Add(new Mappings.CommunicationCompactMap());
        }
    }
}
