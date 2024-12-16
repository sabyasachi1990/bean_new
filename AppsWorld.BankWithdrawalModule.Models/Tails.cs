using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Models
{
    public class Tails
    {
        public long CompanyId { get; set; }
        public long FileShareName { get; set; }
        public string CursorName { get; set; }
        public string Path { get; set; }
        public List<TailsModel> LstTailsModel { get; set; }
    }
}
