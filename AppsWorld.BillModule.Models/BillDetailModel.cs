﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;

namespace AppsWorld.BillModule.Models
{
    public class BillDetailModel
    {
        //public COAModel BeanCOA { get; set; }
        //public TaxModel TaxCode { get; set; }

        public BillDetailModel()
        {
            //this.BeanCOA = new COAModel();
            //this.TaxCode = new TaxModel();
        }
        public Guid BillId;
        public Guid Id;
        public decimal? BaseAmount { get; set; }
        public bool IsDisallow { get; set; }
        public decimal? BaseTaxAmount { get; set; }
        public decimal? BaseTotalAmount { get; set; }
        public decimal DocAmount { get; set; }
        public decimal DocTaxAmount { get; set; }
        public decimal DocTotalAmount { get; set; }
        public string Description { get; set; }
        public string RecordStatus { get; set; }
        public int? RecOrder { get; set; }
        public decimal BaseCurrencyRate { get; set; }
        public long? TaxId { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public string TaxType { get; set; }
        public double? TaxRate { get; set; }
        public string TaxIdCode { get; set; }
        public bool? IsPLAccount { get; set; }
        public long COAId { get; set; }
        public string COAName { get; set; }
        public string ClearingState { get; set; }

        // public BillCreditMemoDetailModel BillCreditMemoDetailModel { get; set; }



        public bool? IsTaxCodeNotEditable { get; set; }
        public bool? IsTaxAmountEditable { get; set; }
        public bool? IsTaxAmountEditables { get; set; }
    }
}