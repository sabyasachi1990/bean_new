using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.Models
{
    public class ClearedModelK
    {
        public Nullable<System.Guid> Id { get; set; }
        public Nullable<System.DateTime> ClearingDate { get; set; }
        public string AccountName { get; set; }
        public decimal? CheckAmount { get; set; }
        public long? TransactionCount { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

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
        public bool? IsLocked { get; set; }
    }
}
