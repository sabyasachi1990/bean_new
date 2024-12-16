using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class ServiceEntityLU
    {
        public bool? IsIntercoActivate { get; set; }
        public List<FrameWork.LookUps.LookUp<long>> LstOfServiceEntites { get; set; }
    }
}
