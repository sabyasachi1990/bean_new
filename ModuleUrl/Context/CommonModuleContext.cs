using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;
using System.Data.Entity;
using ModuleUrl.Model;
using ModuleUrl.Model.Mappings;
namespace ModuleUrl.Context
{
	public partial class CommonModuleContext : DataContext
	{
		public CommonModuleContext()
			: base("name=AppsWorldDBContext")
		{
		}
		public DbSet<ModuleMaster> ModuleMasters { get; set; }

		public DbSet<ModuleDetail> ModuleDetails { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new ModuleDetailMap());
			modelBuilder.Configurations.Add(new ModuleMasterMap());
		}
	}


}
