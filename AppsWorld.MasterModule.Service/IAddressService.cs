using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IAddressService : IService<Address>
    {
        Task<List<Address>> GetAddres(Guid entityid);
        List<Address> GetAddresById(Guid entityid);
        Address GetAddressId(Guid? entityid);
        Address GetAddressByDocumentId(Guid documentId);
        List<Address> GetAddressByAddId(List<Guid> contactIds);
    }
}
