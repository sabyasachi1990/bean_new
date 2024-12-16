using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;

namespace AppsWorld.MasterModule.Entities
{
    public partial class CompanyUser:Entity
    {
        public CompanyUser()
        {
           
        }
        public long Id { get; set; }
        public long CompanyId { get; set; } 
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
    }
}
