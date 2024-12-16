
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class AddressService : Service<Address>, IAddressService
    {
        private readonly IMasterModuleRepositoryAsync<Address> _addressRepository;
        public AddressService(IMasterModuleRepositoryAsync<Address> addressRepository)
            : base(addressRepository)
        {
            _addressRepository = addressRepository;
        }
        public async Task<List<Address>> GetAddres(Guid entityid)
        {
            return await Task.Run(()=> _addressRepository.Query(a =>( a.AddType == "Entity" ||a.AddType== "BeanContact") && a.AddTypeId == entityid).Select().ToList());
        }
        public List<Address> GetAddresById(Guid entityid)
        {
            return _addressRepository.Query(a => a.Id == entityid).Select().ToList();
        }
        public Address GetAddressId(Guid? entityid)
        {
            return _addressRepository.Query(a => a.Id == entityid).Select().FirstOrDefault();
        }
        public Address GetAddressByDocumentId(Guid documentId)
        {
            return _addressRepository.Query(a => a.DocumentId == documentId).Select().FirstOrDefault();
        }
        public List<Address> GetAddressByAddId(List<Guid> contactIds)
        {
            return _addressRepository.Query(a => contactIds.Contains((Guid)a.AddTypeId)).Include(d => d.AddressBook).Select().ToList();
        }
    }
}
