using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public partial class EnableOrDisable
    {
        public List<string> id { get; set; }
        public string tableName { get; set; }
        public long CompanyId { get; set; }
        public string UserName { get; set; }
        public List<string> status { get; set; }
    }
}
