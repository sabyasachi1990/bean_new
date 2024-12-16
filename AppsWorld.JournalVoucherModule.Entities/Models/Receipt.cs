using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class Receipt : Entity
    {
        //public Receipt()
        //{
        //    this.ReceiptBalancingItems = new List<ReceiptBalancingItem>();
        //    this.ReceiptDetails = new List<ReceiptDetail>();
        //    this.ReceiptGSTDetails = new List<ReceiptGSTDetail>();
        //}

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocumentState { get; set; }
        public string Remarks { get; set; }
        public string ModeOfReceipt { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public string ReceiptRefNo { get; set; }
        public long COAId { get; set; }
    }
}
