using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IAddressBookService : IService<AddressBook>
    {
        AddressBook GetAddressBook(Guid? AddressBookId);
        AddressBook GetAddressBookByDocumentId(Guid documentId);
        Task<List<AddressBook>> GetAllAddressBook(List<Guid?> addressBookIds);
    }
}
