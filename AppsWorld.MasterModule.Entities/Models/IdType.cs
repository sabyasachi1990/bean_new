using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities
{
    public partial class IdType:Entity
    {
        public IdType()
        {
        //    this.Clients = new List<Client>();
            //this.Entities = new List<Entity>();
           // this.Accounts = new List<Account>();
           // this.Vendors = new List<Vendor>();
           // this.AccountTypeIdTypes = new List<AccountTypeIdType>();
           // this.Client1 = new List<Client1>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public Nullable<long> TempIdTypeId { get; set; }
        RecordStatusEnum _status;
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
        //public virtual ICollection<Client> Clients { get; set; }
        //public virtual ICollection<Entity> Entities { get; set; }
        //public virtual ICollection<Account> Accounts { get; set; }
        //public virtual ICollection<Vendor> Vendors { get; set; }
        //public virtual ICollection<AccountTypeIdType> AccountTypeIdTypes { get; set; }
        //public virtual Company Company { get; set; }
        //public virtual ICollection<Client1> Client1 { get; set; }
    }
}
