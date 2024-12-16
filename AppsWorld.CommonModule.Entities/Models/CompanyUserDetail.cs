using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Entities.Models
{
    public class CompanyUserDetail : Entity
    {
        public Guid Id { get; set; }
        public long CompanyUserId { get; set; }
        public long ServiceEntityId { get; set; }
       // public DateTime CreatedDate { get; set; }
    }
}
