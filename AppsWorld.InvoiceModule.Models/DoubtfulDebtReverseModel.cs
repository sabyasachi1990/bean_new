using AppsWorld.BeanCursor.Entities.Models;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class DoubtfulDebtReverseModel
    {
        public System.Guid DoubtfulDebtId { get; set; }
        public long CompanyId { get; set; }
        public Nullable<DateTime> FinancialStartDate { get; set; }
        public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public System.DateTime DocDate { get; set; }
        public string Version { get; set; }
        public Nullable<System.DateTime> ReverseDate { get; set; }
        public bool? ReverseIsSupportingDocument { get; set; }
        public string ReverseRemarks { get; set; }
        public string PeriodLockPassword { get; set; }
    }
}
