using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.ReceiptModule.Entities
{
    //public partial class CCAccountType : Entity
    //{
    //    //public AccountType1()
    //    //{
    //    //    this.Entities = new List<Entity>();
    //    //    this.Accounts = new List<Account>();
    //    //    this.AccountTypeIdTypes = new List<AccountTypeIdType>();
    //    //}

    //    public long Id { get; set; }

    //    [Required]
    //    [StringLength(100)]
    //    public string Name { get; set; }

    //    public long CompanyId { get; set; }

    //    public Nullable<int> RecOrder { get; set; }

    //    [StringLength(256)]
    //    public string Remarks { get; set; }

    //    [StringLength(254)]
    //    public string UserCreated { get; set; }

    //    public Nullable<System.DateTime> CreatedDate { get; set; }

    //    [StringLength(254)]
    //    public string ModifiedBy { get; set; }

    //    public Nullable<System.DateTime> ModifiedDate { get; set; }

    //    public Nullable<short> Version { get; set; }

    //    RecordStatusEnum _status;
    //    [Required]
    //    [JsonConverter(typeof(StringEnumConverter))]
    //    [StatusValue]
    //    public RecordStatusEnum Status
    //    {
    //        get
    //        {
    //            return _status;
    //        }
    //        set { _status = (RecordStatusEnum)value; }
    //    }

    //    //public virtual ICollection<Entity> Entities { get; set; }

    //    //public virtual ICollection<Account> Accounts { get; set; }
    //    //public virtual Company Company { get; set; }
    //    //public virtual ICollection<AccountTypeIdType> AccountTypeIdTypes { get; set; }
    //}
}
