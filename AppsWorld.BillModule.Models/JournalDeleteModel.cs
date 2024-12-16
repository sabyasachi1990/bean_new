using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
	public class JournalDeleteModel
    {
		public Guid Id { get; set; }
		public long CompanyId { get; set; }
        public string DocNo { get; set; }
        public string ModifiedBy { get; set; }
    }
}
