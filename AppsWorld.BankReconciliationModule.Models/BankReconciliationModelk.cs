using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;
using AppsWorld.Framework;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.BankReconciliationModule.Models
{
    public class BankReconciliationModelk
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public long ServiceCompanyId { get; set; }
        public long COAId { get; set; }
        public System.DateTime BankReconciliationDate { get; set; }
        public string Currency { get; set; }
        public string BankAccount { get; set; }
        public double StatementAmount { get; set; }
        public string SubsidiaryCompany { get; set; }
        public string State { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> StatementDate { get; set; }
        public decimal? GLBalance { get; set; }
        //public string Identity { get; set; }
        public bool? IsReRunBR { get; set; }
        public bool? IsLocked { get; set; }
        public string DocType { get; set; }
        public byte[] RowVersion
        {
            get;
            set;
        }

        public string Version
        {
            get
            {
                if (this.RowVersion != null)
                {
                    //return Convert.ToBase64String(this.Version);
                    return "0x" + string.Concat(Array.ConvertAll(RowVersion, x => x.ToString("X2")));
                }

                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.RowVersion = null;
                }
                else
                {
                    this.RowVersion = Convert.FromBase64String(value);
                }
            }
        }

    }
}
