using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.PaymentModule.Models
{
    public class CurrencyLU
    {
        public CurrencyLU()
        {
            CurrencyLu = new List<LookUpCategory<string>>();
        }
        public long CompanyId { get; set; }
        public List<LookUpCategory<string>> CurrencyLu { get; set; }
        public LookUpCategory<string> NatureLU { get; set; }
    }
}
