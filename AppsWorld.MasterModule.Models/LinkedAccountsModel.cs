using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class LinkedAccountsModel
    {
        public string Cursor { get; set; }
        public string Subsection { get; set; }
        public string Feature { get; set; }
        public string FeatureName { get; set; }
        public string COAName { get; set; }
        public string AccountType { get; set; }
    }
}
