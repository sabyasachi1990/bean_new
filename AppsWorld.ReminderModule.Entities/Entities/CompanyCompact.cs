using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Entities.Entities
{
    public partial class CompanyCompact : Entity
    {
        public CompanyCompact()
        {

            this.Bank = new List<Bank>();

        }

        public long Id { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string RegistrationNo { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> LogoId { get; set; }
        public string CssSprite { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<System.Guid> AddressBookId { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public bool? IsGstSetting { get; set; }
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
        public string ShortName { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string Communication { get; set; }
        public ICollection<Bank> Bank { get; set; }
    }
}
