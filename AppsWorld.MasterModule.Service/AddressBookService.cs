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
    public class AddressBookService : Service<AddressBook>, IAddressBookService
    {
        private readonly IMasterModuleRepositoryAsync<AddressBook> _addressBookRepository;
        public AddressBookService(IMasterModuleRepositoryAsync<AddressBook> addressBookRepository)
            : base(addressBookRepository)
        {
            _addressBookRepository = addressBookRepository;
        }
        public AddressBook GetAddressBook(Guid? AddressBookId)
        {
            return _addressBookRepository.Query(a => a.Id == AddressBookId).Select().FirstOrDefault();
        }
        public AddressBook GetAddressBookByDocumentId(Guid documentId)
        {
            return _addressBookRepository.Query(a => a.DocumentId == documentId).Select().FirstOrDefault();
        }
        public async Task<List<AddressBook>> GetAllAddressBook(List<Guid?> addressBookIds)
        {
            return await Task.Run(()=> _addressBookRepository.Queryable().Where(a => addressBookIds.Contains(a.Id)).ToList());
        }
    }
}
