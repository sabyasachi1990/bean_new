using AppsWorld.MasterModule.Entities;
using System;
using System.Collections.Generic;

namespace AppsWorld.MasterModule.Models
{
    public class SaveAddressModel
    {
        //public List<Address> ListAddress { get; set; }
        //public Guid TypeId { get; set; }
        //public string Type { get; set; }
        //public bool? IsCopy { get; set; }


        public List<Address> ListAddress { get; set; }
        public Guid TypeId { get; set; }
        public string Type { get; set; }
        public bool? IsCopy { get; set; }
        public long? AddTypeIntId { get; set; }
    }
}
