using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;


namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class Role : Entity
    {
        public Role()
        {
            //this.RolePermissions = new List<RolePermission>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        //public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
