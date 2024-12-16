using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Entities
{
    public partial class AccountTypeIdType : Entity
    {
        public long Id { get; set; }
        public long AccountTypeId { get; set; }
        public long IdTypeId { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public virtual IdType IdType { get; set; }
        [ForeignKey("AccountTypeId")]
        public virtual CCAccountType AccountTypes { get; set; }
    }
}
