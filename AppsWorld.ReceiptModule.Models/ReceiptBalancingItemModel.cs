using AppsWorld.Framework;
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
    public class ReceiptBalancingItemModel
    {
        public ReceiptBalancingItemModel()
        {

        }
        public Guid Id { get; set; }
        public Guid ReceiptId { get; set; }
        public decimal BaseAmount { get; set; }
        public bool ISdisAllowable { get; set; }
        public decimal BaseTaxAmount { get; set; }
        public decimal DocAmount { get; set; }
        public decimal BaseTotal { get; set; }
        public decimal DocTaxAmount { get; set; }
        public decimal DocTotalAmount { get; set; }
        public string Description { get; set; }
        public string ReciveorPay { get; set; }
        public bool? IsPLAccount { get; set; }
         
    }
}
