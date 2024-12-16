using AppsWorld.Framework;
using AppsWorld.ReceiptModule.Entities;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Models
{
    public class JournalSaveModel
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocNo { get; set; }
        public string ModifiedBy { get; set; }

    }
}
