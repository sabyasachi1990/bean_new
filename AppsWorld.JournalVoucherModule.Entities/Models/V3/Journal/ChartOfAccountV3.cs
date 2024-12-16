using AppsWorld.Framework;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FrameWork;

namespace AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal
{
    public class ChartOfAccountV3 : Entity
    {
        public ChartOfAccountV3()
        {
            this.DebitNoteDetails = new List<DebitNoteDetail>();
            this.InvoiceDetails = new List<InvoiceDetail>();
        }
        public long Id { get; set; }//FRCoaId
        public Guid? FRCoaId { get; set; }//FRCoaId
        public Guid? FRPATId { get; set; }//account type id
        public long CompanyId { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public long AccountTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Class { get; set; }


        [StringLength(100)]
        public string Category { get; set; }


        [StringLength(100)]
        public string SubCategory { get; set; }


        [StringLength(100)]
        public string Nature { get; set; }

        [StringLength(5)]
        public string Currency { get; set; }

        public Nullable<bool> ShowRevaluation { get; set; }

        [StringLength(50)]
        public string CashflowType { get; set; }

        [StringLength(50)]
        public string AppliesTo { get; set; }
        public Nullable<bool> IsSystem { get; set; }
        public Nullable<bool> IsShowforCOA { get; set; }

        public Nullable<int> RecOrder { get; set; }

        [StringLength(256)]
        public string Remarks { get; set; }

        [StringLength(254)]
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        [StringLength(254)]
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public Nullable<int> IsRevaluation { get; set; }
        [StringLength(50)]
        public string ModuleType { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public bool? IsRealCOA { get; set; }
        RecordStatusEnum _status;

        [Required]
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

        public bool? IsSubLedger { get; set; }

        public bool? IsCodeEditable { get; set; }

        public bool? ShowCurrency { get; set; }

        public bool? ShowCashFlow { get; set; }
        public bool? IsBank { get; set; }
        public bool? ShowAllowable { get; set; }
        public bool? Revaluation { get; set; }
        public bool? DisAllowable { get; set; }
        public bool? RealisedExchangeGainOrLoss { get; set; }
        public bool? UnrealisedExchangeGainOrLoss { get; set; }
        public Nullable<int> FRRecOrder { get; set; }

        public long? SubsidaryCompanyId { get; set; }
        public virtual AccountType AccountType { get; set; }

        public virtual ICollection<DebitNoteDetail> DebitNoteDetails { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        //     public ICollection<JournalDetail> JournalDetails { get; internal set; }
    }
}
