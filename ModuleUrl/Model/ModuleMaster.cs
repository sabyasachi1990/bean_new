using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleUrl.Model
{
	public partial class ModuleMaster
	{
		public ModuleMaster()
		{
		
		}

		public long Id { get; set; }
		public Nullable<long> ParentId { get; set; }
		public string Name { get; set; }
		public long CompanyId { get; set; }
		public string Description { get; set; }
		public string Heading { get; set; }
		public Nullable<System.Guid> LogoId { get; set; }
		public string CssSprite { get; set; }
		public string FontAwesome { get; set; }
		public string Url { get; set; }
		public Nullable<int> RecOrder { get; set; }
		public string Remarks { get; set; }
		public Nullable<int> Status { get; set; }
		
	}
}
