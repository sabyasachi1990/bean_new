using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class DeleteFileModel
    {
        public long CompanyId { get; set; }
        public string FileShareName { get; set; }
        public string CursorName { get; set; }
        public string Path { get; set; }
    }
}
