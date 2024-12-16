using AppsWorld.BillModule.Entities;
using AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal;
using AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal.Mapping;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.RepositoryPattern.V3;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Entities.Context.V3
{
    public class JournalV3Context : DataContext, IJournalVoucherModuleDataContextAsyncV3
    {
        static JournalV3Context()
        {
            Database.SetInitializer<JournalContext>(null);
        }
        public JournalV3Context()
            : base(Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys["SecondaryDbConnection"].ToString())
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<AccountTypeV3> AccountTypes { get; set; }
        public DbSet<CategoryV3> Category { get; set; }
        public DbSet<SubCategoryV3> SubCategory { get; set; }
        public DbSet<ChartOfAccountV3> ChartOfAccount { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccountTypeV3Map());
            modelBuilder.Configurations.Add(new CategoryV3Map());
            modelBuilder.Configurations.Add(new SubCategoryV3Map());
            modelBuilder.Configurations.Add(new ChartOfAccountV3Map());
        }
    }
}
